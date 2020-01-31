using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;



namespace Charaterizator
{
    public partial class FormSwitch : Form
    {


        // Занесены в настройки
        public int MAX_SETCH = 14;        // максимально разрешенное коичество подключаемых к изм. линии датчиков начиная с нулевого
        public int READ_PERIOD = 1000;    // Время опроса и обновление информации, мс
        public int READ_PAUSE = 100;       //время выдержки после переключения коммутатора (переходные процессы), мс
        public int WaitTime = 100;        //время выдержки после переключения коммутатора (переходные процессы), мс

        //не занесена в настройки
        int MAXBUSY = 2000;             //Максимальное количество циклов ожидания (по 1 мс)

        bool CommutatorBusy = false;//признак доступности коммутатора для приема команд
        public bool Connected = false;      // true  - соединение установлено, 
        private SerialPort serialPort1;

        // Состяние выходов коммутатора
        Int32 StateCHPower = 0;     // Питание
        public Int32 _StateCHPower
        {
            get
            {
                return StateCHPower;
            }
            
            set
            {

            }

        }

        Int32 StateCH = 0;          // Измерительная цепь
        public Int32 _StateCH
        {
            get
            {
                return  StateCH;
            }
            
            set
            {

            }

        }


        int NumOfConnectInputs = 0; // Количество одновременно подключенных каналов
        public int _NumOfConnectInputs
        {
            get
            {
                return  NumOfConnectInputs;
            }

            set
            {

            }

        }

        public FormSwitch()
        {
            InitializeComponent();
            serialPort1 = new SerialPort();
        }
          
        

        public int DisConnect()
        {
            if (Connected)
            {
                // останавливаем таймер                
                timer1.Stop();
                timer1.Enabled = false;

                serialPort1.Close();
                Connected = false;
                return 0;
            }
            else
            {
                return 1;
            }
        }


        // Функция подключения коммутатора по COM порту
        public int Connect(string PortName, int BaudRate, int DataBits, int StopBits, int Parity)
        {
          

            if (Connected)
            {
                return 1;
            }
            try
            {
                serialPort1.PortName = PortName;
                serialPort1.BaudRate = BaudRate;
                serialPort1.DataBits = DataBits;
                serialPort1.StopBits = (StopBits)StopBits;
                serialPort1.Parity = (Parity)Parity;
                serialPort1.ReadTimeout = 1000;
                serialPort1.WriteTimeout = 1000;
                serialPort1.DtrEnable = true;
                serialPort1.RtsEnable = true;
                serialPort1.Open();


                
                if (ReadCommutatorID())
                {
                    // Запускаем таймер
                    timer1.Interval = READ_PERIOD;
                    timer1.Enabled = true;
                    timer1.Start();
                    Connected = true;
                    return 0;
                }
                else
                {
                    Connected = false;
                    serialPort1.Close();
                    return -1;
                }



            }
            catch
            {
                Connected = false;
                return -1;
            }
        }



        public void CommStartTimer()

        {
            // останавливаем таймер                
            timer1.Stop();
            timer1.Enabled = false;

            // Запускаем таймер
            timer1.Interval = READ_PERIOD;
            timer1.Enabled = true;
            timer1.Start();
        }




        //-----------------------------------------------------------------------------
        // Функция обработки нажатия на кнопки: ВКЛ / ВЫКЛ питания выходов коммутатора
        private void bPower_Click(object sender, EventArgs e)
        {            
            Button b = (Button)sender;
            int busycount = 0;
            while ((CommutatorBusy) && (busycount < MAXBUSY))
            {
                Thread.Sleep(1);
                busycount++;
            }

            CommutatorBusy = true;

            // вызов функции отправки КОМАНД по COM порту для ВКЛ / ВЫКЛ питания выходов коммутатора
            SetPower(Convert.ToInt32(b.Tag), b.ImageIndex);

            CommutatorBusy = false;


        }


        // Функция отправки КОМАНД по COM порту для ВКЛ / ВЫКЛ питания выходов коммутатора
        public int SetPower(Int32 CH, int mode)
        {
            if (!serialPort1.IsOpen) return -1;

            byte[] indata = new byte[10];

            if (mode == 0)
            {
                Int32 _CH = (1 << CH) | StateCHPower;
                serialPort1.Write(WriteHoldingRegister(0, _CH), 0, 9);
                Thread.Sleep(READ_PAUSE);

                serialPort1.Read(indata, 0, 10);
                StateCHPower = Convert.ToInt32((indata[4] << 24) + (indata[5] << 16) + (indata[6] << 8) + indata[7]);
                SetState(StateCHPower, StateCH);

            }
            else
            {

                CH = (1 << CH);
                CH = ~CH;
                Int32 _CH = StateCHPower & CH;
                serialPort1.Write(WriteHoldingRegister(0, _CH), 0, 9);
                Thread.Sleep(READ_PAUSE);
                serialPort1.Read(indata, 0, 10);
                StateCHPower = Convert.ToInt32((indata[4] << 24) + (indata[5] << 16) + (indata[6] << 8) + indata[7]);
                SetState(StateCHPower, StateCH);
            }
            return 0;
        }
        //-----------------------------------------------------------------------------




        //-----------------------------------------------------------------------------
        // Функция обработки нажания на кнопки ПОДКЛ / ОТКЛ датчиков к измерительной петле
        private void bInput_MouseDown(object sender, MouseEventArgs e)
        {
            Button b = (Button)sender;
            int mode = 4;
            

            int busycount = 0;
            while ((CommutatorBusy) && (busycount < MAXBUSY))
            {
                Thread.Sleep(1);
                busycount++;
            }


            CommutatorBusy = true;


            // вызов функции....
            // левая кнопка мыши - режим подключения только ОДНОГО датчика (остальные отключаются)
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (b.ImageIndex == 0)  // если датчик не подключен, отключам все, подключаем текущий
                {
                    mode = 0;              
                }
                else // если датчик уже подключен , то отключаем его и все остальные
                {
                    mode = 1;                  
                }
            }
            // правая кнопка мыши - режим подключения нескольких датчиков датчиков
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (b.ImageIndex == 0)  // если датчик не подключен, подключаем его
                {
                    mode = 2;                   
                }
                else // если датчик уже подключен, отключаем его
                {
                    mode = 3;
                }
            }

            SetConnectors(Convert.ToInt32(b.Tag), mode);
                                 
            CommutatorBusy = false;

        }


        // Функция отправки КОМАНД по COM порту для ПОДКЛ / ОТКЛ датчиков к измерительной петле


        public void SetConnectors(Int32 CH, int mode)
        {
            if (!serialPort1.IsOpen) return;

            byte[] indata = new byte[10];
            Int32 _CH;

            switch (mode)
            {
                // если датчик не подключен, отключам все, подключаем текущий, отключаем все
                case 0:
                    serialPort1.Write(WriteHoldingRegister(1, 0), 0, 9);
                    Thread.Sleep(READ_PAUSE);
                    serialPort1.Read(indata, 0, 10);
                    Thread.Sleep(READ_PAUSE);

                    _CH = (1 << CH);
                    serialPort1.Write(WriteHoldingRegister(1, _CH), 0, 9);
                    Thread.Sleep(READ_PAUSE);
                    serialPort1.Read(indata, 0, 10);

                    // Сохраняем состояния подключенных датчиков
                    StateCH = Convert.ToInt32((indata[4] << 24) + (indata[5] << 16) + (indata[6] << 8) + indata[7]);
                    SetState(StateCHPower, StateCH);
                    break;

                // если датчик уже подключен, то отключаем его и все остальные
                case 1:
                    serialPort1.Write(WriteHoldingRegister(1, 0), 0, 9);
                    Thread.Sleep(READ_PAUSE);
                    serialPort1.Read(indata, 0, 10);
                    StateCH = Convert.ToInt32((indata[4] << 24) + (indata[5] << 16) + (indata[6] << 8) + indata[7]);
                    SetState(StateCHPower, StateCH);
                    break;

                //режим подключения нескольких датчиков датчиков, подключаем заданный к измерительной петле
                case 2:
                    if (NumOfConnectInputs <= MAX_SETCH)
                    {
                        _CH = (1 << CH) | StateCH;
                        serialPort1.Write(WriteHoldingRegister(1, _CH), 0, 9);
                        Thread.Sleep(READ_PAUSE);
                        serialPort1.Read(indata, 0, 10);
                        StateCH = Convert.ToInt32((indata[4] << 24) + (indata[5] << 16) + (indata[6] << 8) + indata[7]);
                        SetState(StateCHPower, StateCH);
                    }
                  
                    break;

                //режим подключения нескольких датчиков датчиков, отключаем заданный от змерительной петли
                case 3:
                    CH = (1 << CH);
                    CH = ~CH;
                    _CH = StateCH & CH;
                    serialPort1.Write(WriteHoldingRegister(1, _CH), 0, 9);
                    Thread.Sleep(READ_PAUSE);
                    serialPort1.Read(indata, 0, 10);
                    StateCH = Convert.ToInt32((indata[4] << 24) + (indata[5] << 16) + (indata[6] << 8) + indata[7]);
                    SetState(StateCHPower, StateCH);
                    break;

                default:
                    break;

            }
            Thread.Sleep(WaitTime);//ждем установления подключения
        }











        //--------------------------------------------------------------------------------
        // ФУНКЦИЯ отключить все датчики


        public void DisconnectAll()
        {

            bInput0.ImageIndex = 0;
            bInput1.ImageIndex = 0;
            bInput2.ImageIndex = 0;
            bInput3.ImageIndex = 0;
            bInput4.ImageIndex = 0;
            bInput5.ImageIndex = 0;
            bInput6.ImageIndex = 0;
            bInput7.ImageIndex = 0;
            bInput8.ImageIndex = 0;
            bInput9.ImageIndex = 0;
            bInput10.ImageIndex = 0;
            bInput11.ImageIndex = 0;
            bInput12.ImageIndex = 0;
            bInput13.ImageIndex = 0;
            bInput14.ImageIndex = 0;
            bInput15.ImageIndex = 0;
            bInput16.ImageIndex = 0;
            bInput17.ImageIndex = 0;
            bInput18.ImageIndex = 0;
            bInput19.ImageIndex = 0;
            bInput20.ImageIndex = 0;
            bInput21.ImageIndex = 0;
            bInput22.ImageIndex = 0;
            bInput23.ImageIndex = 0;
            bInput24.ImageIndex = 0;
            bInput25.ImageIndex = 0;
            bInput26.ImageIndex = 0;
            bInput27.ImageIndex = 0;
            bInput28.ImageIndex = 0;
            bInput29.ImageIndex = 0;

        }
        //-------------------------------------------------------------------------------



      


        // Подключить отключить питание всех выходов
        private void bAllPower_Click(object sender, EventArgs e)
        {
            byte[] indata = new byte[10];
            //if (bAllPower.ImageIndex == 0) // Если питание отключено - то включаем его
            if (bAllPower.Text == "Включить питание всех датчиков")
            {
                //bAllPower.ImageIndex = 1;
                bAllPower.Text = "Отключить питание всех датчиков";

                /* ///             bPower0.ImageIndex = 1; bInput0.Enabled = true;
                                bPower1.ImageIndex = 1; bInput1.Enabled = true;
                                bPower2.ImageIndex = 1; bInput2.Enabled = true;
                                bPower3.ImageIndex = 1; bInput3.Enabled = true;
                                bPower4.ImageIndex = 1; bInput4.Enabled = true;
                                bPower5.ImageIndex = 1; bInput5.Enabled = true;
                                bPower6.ImageIndex = 1; bInput6.Enabled = true;
                                bPower7.ImageIndex = 1; bInput7.Enabled = true;
                                bPower8.ImageIndex = 1; bInput8.Enabled = true;
                                bPower9.ImageIndex = 1; bInput9.Enabled = true;
                                bPower10.ImageIndex = 1; bInput10.Enabled = true;
                                bPower11.ImageIndex = 1; bInput11.Enabled = true;
                                bPower12.ImageIndex = 1; bInput12.Enabled = true;
                                bPower13.ImageIndex = 1; bInput13.Enabled = true;
                                bPower14.ImageIndex = 1; bInput14.Enabled = true;
                                bPower15.ImageIndex = 1; bInput15.Enabled = true;
                                bPower16.ImageIndex = 1; bInput16.Enabled = true;
                                bPower17.ImageIndex = 1; bInput17.Enabled = true;
                                bPower18.ImageIndex = 1; bInput18.Enabled = true;
                                bPower19.ImageIndex = 1; bInput19.Enabled = true;
                                bPower20.ImageIndex = 1; bInput20.Enabled = true;
                                bPower21.ImageIndex = 1; bInput21.Enabled = true;
                                bPower22.ImageIndex = 1; bInput22.Enabled = true;
                                bPower23.ImageIndex = 1; bInput23.Enabled = true;
                                bPower24.ImageIndex = 1; bInput24.Enabled = true;
                                bPower25.ImageIndex = 1; bInput25.Enabled = true;
                                bPower26.ImageIndex = 1; bInput26.Enabled = true;
                                bPower27.ImageIndex = 1; bInput27.Enabled = true;
                                bPower28.ImageIndex = 1; bInput28.Enabled = true;
                                bPower29.ImageIndex = 1; bInput29.Enabled = true;*//////


                serialPort1.Write(WriteHoldingRegister(0, 0x3FFFFFFF), 0, 9);
                Thread.Sleep(READ_PAUSE);
                serialPort1.Read(indata, 0, 10);
                StateCHPower = Convert.ToInt32((indata[4] << 24) + (indata[5] << 16) + (indata[6] << 8) + indata[7]);
                SetState(StateCHPower, StateCH);

                //Thread.Sleep(TimeSleep);

            }
            else
            {
                bAllPower.Text = "Включить питание всех датчиков";

                /*  ////             bPower0.ImageIndex = 0; bInput0.ImageIndex = 0; bInput0.Enabled = false;
                               bPower1.ImageIndex = 0; bInput1.ImageIndex = 0; bInput1.Enabled = false;
                               bPower2.ImageIndex = 0; bInput2.ImageIndex = 0; bInput2.Enabled = false;
                               bPower3.ImageIndex = 0; bInput3.ImageIndex = 0; bInput3.Enabled = false;
                               bPower4.ImageIndex = 0; bInput4.ImageIndex = 0; bInput4.Enabled = false;
                               bPower5.ImageIndex = 0; bInput5.ImageIndex = 0; bInput5.Enabled = false;
                               bPower6.ImageIndex = 0; bInput6.ImageIndex = 0; bInput6.Enabled = false;
                               bPower7.ImageIndex = 0; bInput7.ImageIndex = 0; bInput7.Enabled = false;
                               bPower8.ImageIndex = 0; bInput8.ImageIndex = 0; bInput8.Enabled = false;
                               bPower9.ImageIndex = 0; bInput9.ImageIndex = 0; bInput9.Enabled = false;
                               bPower10.ImageIndex = 0; bInput10.ImageIndex = 0; bInput10.Enabled = false;
                               bPower11.ImageIndex = 0; bInput11.ImageIndex = 0; bInput11.Enabled = false;
                               bPower12.ImageIndex = 0; bInput12.ImageIndex = 0; bInput12.Enabled = false;
                               bPower13.ImageIndex = 0; bInput13.ImageIndex = 0; bInput13.Enabled = false;
                               bPower14.ImageIndex = 0; bInput14.ImageIndex = 0; bInput14.Enabled = false;
                               bPower15.ImageIndex = 0; bInput15.ImageIndex = 0; bInput15.Enabled = false;
                               bPower16.ImageIndex = 0; bInput16.ImageIndex = 0; bInput16.Enabled = false;
                               bPower17.ImageIndex = 0; bInput17.ImageIndex = 0; bInput17.Enabled = false;
                               bPower18.ImageIndex = 0; bInput18.ImageIndex = 0; bInput18.Enabled = false;
                               bPower19.ImageIndex = 0; bInput19.ImageIndex = 0; bInput19.Enabled = false;
                               bPower20.ImageIndex = 0; bInput20.ImageIndex = 0; bInput20.Enabled = false;
                               bPower21.ImageIndex = 0; bInput21.ImageIndex = 0; bInput21.Enabled = false;
                               bPower22.ImageIndex = 0; bInput22.ImageIndex = 0; bInput22.Enabled = false;
                               bPower23.ImageIndex = 0; bInput23.ImageIndex = 0; bInput23.Enabled = false;
                               bPower24.ImageIndex = 0; bInput24.ImageIndex = 0; bInput24.Enabled = false;
                               bPower25.ImageIndex = 0; bInput25.ImageIndex = 0; bInput25.Enabled = false;
                               bPower26.ImageIndex = 0; bInput26.ImageIndex = 0; bInput26.Enabled = false;
                               bPower27.ImageIndex = 0; bInput27.ImageIndex = 0; bInput27.Enabled = false;
                               bPower28.ImageIndex = 0; bInput28.ImageIndex = 0; bInput28.Enabled = false;
                               bPower29.ImageIndex = 0; bInput29.ImageIndex = 0; bInput29.Enabled = false;*////




                serialPort1.Write(WriteHoldingRegister(0, 0x0), 0, 9);
                Thread.Sleep(READ_PAUSE);
                serialPort1.Read(indata, 0, 10);
                StateCHPower = Convert.ToInt32((indata[4] << 24) + (indata[5] << 16) + (indata[6] << 8) + indata[7]);
                SetState(StateCHPower, StateCH);

                //Thread.Sleep(TimeSleep);

            }

        }




        // Функция сеняет цвет подключенного канала
        public void SetCHColor(int mode)
        {
            // mode: 0 - серый
            // mode: 1 - серый   
            // mode: 2 - серый 


        }


        // Функция возвращает ссылку на кнопку по номеру
        public Button RefButton(int num)
        {
            Button btn = bInput0;

            switch (num)
            {
                case 0:
                    btn = bInput0;
                    break;
                case 1:
                    btn = bInput1;
                    break;
                case 2:
                    btn = bInput2;
                    break;
                case 3:
                    btn = bInput3;
                    break;
                case 4:
                    btn = bInput4;
                    break;
                case 5:
                    btn = bInput5;
                    break;
                case 6:
                    btn = bInput6;
                    break;
                case 7:
                    btn = bInput7;
                    break;
                case 8:
                    btn = bInput8;
                    break;
                case 9:
                    btn = bInput9;
                    break;
                case 10:
                    btn = bInput10;
                    break;
                case 11:
                    btn = bInput11;
                    break;
                case 12:
                    btn = bInput12;
                    break;
                case 13:
                    btn = bInput13;
                    break;
                case 14:
                    btn = bInput14;
                    break;
                case 15:
                    btn = bInput15;
                    break;
                case 16:
                    btn = bInput16;
                    break;
                case 17:
                    btn = bInput17;
                    break;
                case 18:
                    btn = bInput18;
                    break;
                case 19:
                    btn = bInput19;
                    break;
                case 20:
                    btn = bInput20;
                    break;
                case 21:
                    btn = bInput21;
                    break;
                case 22:
                    btn = bInput22;
                    break;
                case 23:
                    btn = bInput23;
                    break;
                case 24:
                    btn = bInput24;
                    break;
                case 25:
                    btn = bInput25;
                    break;
                case 26:
                    btn = bInput26;
                    break;
                case 27:
                    btn = bInput27;
                    break;
                case 28:
                    btn = bInput28;
                    break;
                case 29:
                    btn = bInput29;
                    break;
            }

            return btn;
        }

  
        public static void myCRC(byte[] message, int length, out byte CRCHigh, out byte CRCLow)
        {
            ushort CRCFull = 0xFFFF;
            for (int i = 0; i < length; i++)
            {
                CRCFull = (ushort)(CRCFull ^ message[i]);
                for (int j = 0; j < 8; j++)
                {
                    if ((CRCFull & 0x0001) == 0)
                        CRCFull = (ushort)(CRCFull >> 1);
                    else
                    {
                        CRCFull = (ushort)((CRCFull >> 1) ^ 0xA001);
                    }
                }
            }
            CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
            CRCLow = (byte)(CRCFull & 0xFF);
        }

        public static byte[] ReadHoldingRegister()
        {
            byte[] data = new byte[4];
            Thread.Sleep(100);
            byte High, Low;
            data[0] = 0x1;                       //Номер прибора
            data[1] = 0x45;                        //Функция 
            myCRC(data, 2, out High, out Low);
            data[2] = Low;
            data[3] = High;
            return data;
        }


        public static byte[] WriteHoldingRegister(byte startAddress, Int32 indata)
        {
            byte[] data = new byte[9];
            Thread.Sleep(100);
            byte High, Low;
            data[0] = 0x01;                  //Номер прибора
            data[1] = 0x46;                  //Функция 
            data[2] = startAddress;          //№ первого регистра


            data[3] = (byte)((indata >> 24) & 0xFF);
            data[4] = (byte)((indata >> 16) & 0xFF);
            data[5] = (byte)((indata >> 8) & 0xFF);
            data[6] = (byte)(indata & 0xFF);

            myCRC(data, 7, out High, out Low);
            data[7] = Low;
            data[8] = High;
            return data;
        }


        private void ReadCommutator()
        {
            byte[] indata = new byte[13];
            Int32 data32;

            if (serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Write(ReadHoldingRegister(), 0, 4);
                    Thread.Sleep(READ_PAUSE);
                    serialPort1.Read(indata, 0, 13);
                    Thread.Sleep(READ_PAUSE);

                    // Сохраняем состояния элементов питания каналов
                    data32 = Convert.ToInt32((indata[3] << 24) + (indata[4] << 16) + (indata[5] << 8) + indata[6]);
                    StateCHPower = data32;

                    // Сохраняем состояния подключенных датчиков
                    data32 = Convert.ToInt32((indata[7] << 24) + (indata[8] << 16) + (indata[9] << 8) + indata[10]);
                    StateCH = data32;

                    SetState(StateCHPower, StateCH);
                }
                catch
                {
                    StateCH = -1;
                    StateCHPower = -1;
                }

            }
            else
            {
                StateCH = -1;
                StateCHPower = -1;
            }
        }


        private bool ReadCommutatorID()
        {
            byte[] indata = new byte[13];
            Int32 data32;
            bool CommID = false;

            if (serialPort1.IsOpen)
            {
                serialPort1.Write(ReadHoldingRegister(), 0, 4);
                Thread.Sleep(READ_PAUSE);
                serialPort1.Read(indata, 0, 13);
                Thread.Sleep(READ_PAUSE);

                // проверка заголовка ответного слова коммутатора
                if((indata[0] == 01)&&(indata[1]==69)&&(indata[2]==08))
                    {
                    CommID = true;
                    }
                               
                // Сохраняем состояния элементов питания каналов
                data32 = Convert.ToInt32((indata[3] << 24) + (indata[4] << 16) + (indata[5] << 8) + indata[6]);
                StateCHPower = data32;

                // Сохраняем состояния подключенных датчиков
                data32 = Convert.ToInt32((indata[7] << 24) + (indata[8] << 16) + (indata[9] << 8) + indata[10]);
                StateCH = data32;

                SetState(StateCHPower, StateCH);

            }

            return CommID;
        }






        private void timer1_Tick(object sender, EventArgs e)
        {
            int busycount = 0;
            while ((CommutatorBusy) && (busycount < MAXBUSY))
            {
                Thread.Sleep(1);
                busycount++;
            }
            CommutatorBusy = true;
            ReadCommutator();
            CommutatorBusy = false;

            //ReadCommutator();
        }


        public int CalcNumOfConnectInputs(Int32 StateInputs)
        {
            int Num = 0;
            int Cur = 0;

            for (int i = 0; i < 30; i++)
            {
                Cur = StateInputs & 0x01;
                StateInputs = StateInputs >> 1;
                Num = Num + Cur;
            }

            return Num;
        }



        public void SetState(Int32 StateCHPower, Int32 StateCH)
        {
            Int32 data32;

            data32 = StateCHPower;

            // Обработка состояний 
            bPower0.ImageIndex = (data32 & 0x1); bPower16.ImageIndex = (data32 & 0x10000) >> 16;
            bPower1.ImageIndex = (data32 & 0x2) >> 1; bPower17.ImageIndex = (data32 & 0x20000) >> 17;
            bPower2.ImageIndex = (data32 & 0x4) >> 2; bPower18.ImageIndex = (data32 & 0x40000) >> 18;
            bPower3.ImageIndex = (data32 & 0x8) >> 3; bPower19.ImageIndex = (data32 & 0x80000) >> 19;
            bPower4.ImageIndex = (data32 & 0x10) >> 4; bPower20.ImageIndex = (data32 & 0x100000) >> 20;
            bPower5.ImageIndex = (data32 & 0x20) >> 5; bPower21.ImageIndex = (data32 & 0x200000) >> 21;
            bPower6.ImageIndex = (data32 & 0x40) >> 6; bPower22.ImageIndex = (data32 & 0x400000) >> 22;
            bPower7.ImageIndex = (data32 & 0x80) >> 7; bPower23.ImageIndex = (data32 & 0x800000) >> 23;
            bPower8.ImageIndex = (data32 & 0x100) >> 8; bPower24.ImageIndex = (data32 & 0x1000000) >> 24;
            bPower9.ImageIndex = (data32 & 0x200) >> 9; bPower25.ImageIndex = (data32 & 0x2000000) >> 25;
            bPower10.ImageIndex = (data32 & 0x400) >> 10; bPower26.ImageIndex = (data32 & 0x4000000) >> 26;
            bPower11.ImageIndex = (data32 & 0x800) >> 11; bPower27.ImageIndex = (data32 & 0x8000000) >> 27;
            bPower12.ImageIndex = (data32 & 0x1000) >> 12; bPower28.ImageIndex = (data32 & 0x10000000) >> 28;
            bPower13.ImageIndex = (data32 & 0x2000) >> 13; bPower29.ImageIndex = (data32 & 0x20000000) >> 29;
            bPower14.ImageIndex = (data32 & 0x4000) >> 14;
            bPower15.ImageIndex = (data32 & 0x8000) >> 15;



            bInput0.Enabled = (data32 & 0x1) == 0x1; bInput15.Enabled = (data32 & 0x8000) == 0x8000;
            bInput1.Enabled = (data32 & 0x2) == 0x2; bInput16.Enabled = (data32 & 0x10000) == 0x10000;
            bInput2.Enabled = (data32 & 0x4) == 0x4; bInput17.Enabled = (data32 & 0x20000) == 0x20000;
            bInput3.Enabled = (data32 & 0x8) == 0x8; bInput18.Enabled = (data32 & 0x40000) == 0x40000;
            bInput4.Enabled = (data32 & 0x10) == 0x10; bInput19.Enabled = (data32 & 0x80000) == 0x80000;
            bInput5.Enabled = (data32 & 0x20) == 0x20; bInput20.Enabled = (data32 & 0x100000) == 0x100000;
            bInput6.Enabled = (data32 & 0x40) == 0x40; bInput21.Enabled = (data32 & 0x200000) == 0x200000;
            bInput7.Enabled = (data32 & 0x80) == 0x80; bInput22.Enabled = (data32 & 0x400000) == 0x400000;
            bInput8.Enabled = (data32 & 0x100) == 0x100; bInput23.Enabled = (data32 & 0x800000) == 0x800000;
            bInput9.Enabled = (data32 & 0x200) == 0x200; bInput24.Enabled = (data32 & 0x1000000) == 0x1000000;
            bInput10.Enabled = (data32 & 0x400) == 0x400; bInput25.Enabled = (data32 & 0x2000000) == 0x2000000;
            bInput11.Enabled = (data32 & 0x800) == 0x800; bInput26.Enabled = (data32 & 0x4000000) == 0x4000000;
            bInput12.Enabled = (data32 & 0x1000) == 0x1000; bInput27.Enabled = (data32 & 0x8000000) == 0x8000000;
            bInput13.Enabled = (data32 & 0x2000) == 0x2000; bInput28.Enabled = (data32 & 0x10000000) == 0x10000000;
            bInput14.Enabled = (data32 & 0x4000) == 0x4000; bInput29.Enabled = (data32 & 0x20000000) == 0x20000000;


            data32 = StateCH;

            bInput0.ImageIndex = (data32 & 0x1); bInput15.ImageIndex = (data32 & 0x8000) >> 15;
            bInput1.ImageIndex = (data32 & 0x2) >> 1; bInput16.ImageIndex = (data32 & 0x10000) >> 16;
            bInput2.ImageIndex = (data32 & 0x4) >> 2; bInput17.ImageIndex = (data32 & 0x20000) >> 17;
            bInput3.ImageIndex = (data32 & 0x8) >> 3; bInput18.ImageIndex = (data32 & 0x40000) >> 18;
            bInput4.ImageIndex = (data32 & 0x10) >> 4; bInput19.ImageIndex = (data32 & 0x80000) >> 19;
            bInput5.ImageIndex = (data32 & 0x20) >> 5; bInput20.ImageIndex = (data32 & 0x100000) >> 20;
            bInput6.ImageIndex = (data32 & 0x40) >> 6; bInput21.ImageIndex = (data32 & 0x200000) >> 21;
            bInput7.ImageIndex = (data32 & 0x80) >> 7; bInput22.ImageIndex = (data32 & 0x400000) >> 22;
            bInput8.ImageIndex = (data32 & 0x100) >> 8; bInput23.ImageIndex = (data32 & 0x800000) >> 23;
            bInput9.ImageIndex = (data32 & 0x200) >> 9; bInput24.ImageIndex = (data32 & 0x1000000) >> 24;
            bInput10.ImageIndex = (data32 & 0x400) >> 10; bInput25.ImageIndex = (data32 & 0x2000000) >> 25;
            bInput11.ImageIndex = (data32 & 0x800) >> 11; bInput26.ImageIndex = (data32 & 0x4000000) >> 26;
            bInput12.ImageIndex = (data32 & 0x1000) >> 12; bInput27.ImageIndex = (data32 & 0x8000000) >> 27;
            bInput13.ImageIndex = (data32 & 0x2000) >> 13; bInput28.ImageIndex = (data32 & 0x10000000) >> 28;
            bInput14.ImageIndex = (data32 & 0x4000) >> 14; bInput29.ImageIndex = (data32 & 0x20000000) >> 29;

            // Проверяем количество подключенных выходов
            NumOfConnectInputs = CalcNumOfConnectInputs(StateCH);
            lNumConnectors.Text = "Количество подключенных датчиков: " + Convert.ToString(NumOfConnectInputs);
           

        }
















    }






}
 
































