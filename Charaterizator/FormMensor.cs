using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Globalization;

namespace Charaterizator
{
    public partial class FormMensor : Form
        
    {
        private Thread ReadThread;
        public SerialPort _serialPort_M;           // переменная для работы с COM портом      
        int pause_ms;                       // задержка между приемом и передачей команд по COM порту, мс 
       
        public bool checkTimer = false;     // флаг запущен таймер или нет
        public const double PsiToPa = 0.14503773773;  // для перевода psi в кПА


        

        public bool Connected = false;      // true  - соединение установлено, 

        public int activCH = -1;            // номер активного канала (0 - канал А, 1 - канал В)
        public int _activCH
        {
            get {return activCH;}
            set { }
        }

        int typeR;                          // Тип ДатПреобр - 1..2 Д1-Д2, 2- AutoRange
        public int _typeR
        {
            get { return typeR; }
            set { }
        }


        int umeas;                          // Ед.изм.
        public int _umeas
        {
            get { return umeas; }
            set { }
        }


        int tpress;                         // Тип давления
        public int _tpress
        {
            get { return tpress; }
            set { }
        }



        double press;                       // Тек. давление
        public double _press
        {
            get { return press; }
            set { }
        }


        double point;                       // Уставка
        public double _point
        {
            get { return point; }
            set { }
        }


        int mode;                           // Режим 0-ИЗМ.  1-ЗАДАЧА  2-СБРОС
        public int _mode
        {
            get { return mode; }
            set { }
        }

        double rate;                           // Режим 0-ИЗМ.  1-ЗАДАЧА  2-СБРОС
        public double _rate
        {
            get { return rate; }
            set { }
        }


        public FormMensor()
        {
            InitializeComponent();
            _serialPort_M = new SerialPort();
            


            /* PortNames_M = SerialPort.GetPortNames();   // Обнаружение доступных COM-портов на ПЭВМ           
             if (PortNames_M.Length > 0)                // если порты обнаружены
             {
                 cbSetComPort.Items.Clear();
                 cbSetComPort.Items.Add("Выберите COM порт");
                 cbSetComPort.Items.AddRange(PortNames_M);
                 cbSetComPort.SelectedIndex = 0;
             }
             else                                      // если порты не обнаружены
             {
                 cbSetComPort.Items.Clear();
                 cbSetComPort.Items.Add("Нет доступных портов");
                 cbSetComPort.SelectedIndex = 0;
             }*/
        }

        public int DisConnect()
        {
            if (Connected)
            {
                // останавливаем таймер                
//              timer1.Stop();
//              timer1.Enabled = false;
                if (ReadThread != null)
                    ReadThread.Abort(0);

                _serialPort_M.Close();
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
                _serialPort_M.PortName = PortName;
                _serialPort_M.BaudRate = BaudRate;
                _serialPort_M.DataBits = DataBits;
                _serialPort_M.StopBits = (StopBits)StopBits;
                _serialPort_M.Parity = (Parity)Parity;
                _serialPort_M.ReadTimeout = 1000;
                _serialPort_M.WriteTimeout = 1000;
                _serialPort_M.DtrEnable = true;
                _serialPort_M.RtsEnable = true;
                _serialPort_M.Open();

                //bool ReadMensorID = true;  // заглушка, д.б. функция считывающая что менсор ресльно подключен а не просто открыт порт для него
                int ReadMensorID = ChannelRead();
                if ((ReadMensorID==0) && (ReadMensorID == 1))
                {
                  
                    // устанавливаем заданные паузу и таймер (время считывания информации с mensor)
                    pause_ms = Convert.ToInt32(tbPauseMC.Text);
//                    timer1.Interval = Convert.ToInt32(tbTimer.Text);
                    // запускаем таймер
//                    timer1.Enabled = true;
//                    timer1.Start();
//                    timer1_Tick(null, null);
                    Connected = true;

                    ReadThread = new Thread(MensorReadThread);
                    ReadThread.Priority = ThreadPriority.AboveNormal;
                    ReadThread.Start();

                    return 0;
                }
                else
                {
                    Connected = false;
                    _serialPort_M.Close();
                    return -1;
                }
                             

            }
            catch
            {
                Connected = false;
                return -1;
            }
        }

        
        
        // *** Обработчик ВЫБОРА COM порта       
        private void cbSetComPort_SelectedIndexChanged(object sender, EventArgs e)
        {
           /* // COM порт выбран? (если да - кнопака "Подключить" - становится активной, нет - неактивной)
            if (cbSetComPort.SelectedIndex > 0)
            {
                bComPortConnect.Enabled = true;
            }
            else
            {
                bComPortConnect.Enabled = false;
            }*/
        }


        // *** Обработчик нажатия кнопки "ПОДКЛЮЧИТЬ/ОТКЛЮЧИТЬ COM - порт"
        private void bComPortConnect_Click(object sender, EventArgs e)
        {
           /* // Подключаем COM-порт
            if (bComPortConnect.Text == "Подключить")
            {
                try
                {
                    _serialPort_M = new SerialPort();
                    // Настраиваем COM порт         
                    // Значения прибора установленные по умолчанию
                    _serialPort_M.PortName = cbSetComPort.Text;
                    _serialPort_M.BaudRate = 9600;
                    _serialPort_M.DataBits = 8;
                    _serialPort_M.Parity = Parity.None;
                    _serialPort_M.StopBits = StopBits.One;
                    // устанавливаем заданные пользователем RTO и WTO
                    _serialPort_M.ReadTimeout = Convert.ToInt32(tbRTOut.Text); 
                    _serialPort_M.WriteTimeout = Convert.ToInt32(tbWTOut.Text);
                    //_serialPort_M.Encoding = Encoding.ASCII;
                    //_serialPort_M.DtrEnable = true;
                    //_serialPort_M.RtsEnable = true;                 
                    _serialPort_M.Open();

                }
                catch (Exception ex)
                {
                    lComPortState.Text = "ERROR: Не удалось подключить COM порт. " + ex.ToString();
                }
                
                if (_serialPort_M.IsOpen)  // если COM порт открыт
               {
                    bComPortConnect.Text = "Отключить";
                    lComPortState.Text = "Устройство подключено...";
                    // делаем доступными панели каналов А и B
                    pCHA.Enabled = true;
                    pCHB.Enabled = true;          
                    // устанавливаем заданные паузу и таймер (время считывания информации с mensor)
                    pause_ms = Convert.ToInt32(tbPauseMC.Text);
                    timer1.Interval = Convert.ToInt32(tbTimer.Text);
                    // запускаем таймер
                    timer1.Enabled = true;
                    timer1.Start();
                }
            }
            // Отключаем COM порт
            else
            {
                // останавливаем таймер                
                timer1.Stop();
                timer1.Enabled = false;
                // закрываем COM-порт
                _serialPort_M.Close();
                // восстанавливаем начальную форму
                bComPortConnect.Text = "Подключить";
                lComPortState.Text = "Устройство отключено!";
                cbSetComPort.SelectedIndex = 0;
                pCHA.Enabled = false;
                pCHB.Enabled = false;
            }

    */
        }

   




        //********************************************************************************
        //
        //   Обработчики нажатия кнопок
        //
        //********************************************************************************



        //................................................................................       
        // CHA - Обработчик нажатия кнопки "СДЕЛАТЬ АКТИВНЫМ КАНАЛ А"
        private void bSetCHA_Click(object sender, EventArgs e)
        {            
            ChannelSet("A");            // передаем команду: установить активным канал А (номер 0)               
        }
        //................................................................................
        


        //................................................................................
        // CHB - Обработчик нажатия кнопки "СДЕЛАТЬ АКТИВНЫМ КАНАЛ B"
        private void bSetCHB_Click(object sender, EventArgs e)
        {
            ChannelSet("B");            // передаем команду: установить активным канал А (номер 0)          
        }
        //................................................................................




        //................................................................................
        // Обработчик выбора "УДЕРЖИВАЕМЫЙ ДИАПАЗОН"
        // CHA
        private void cbRangeCHA_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
//                timer1.Stop();
//                timer1.Enabled = false;
                ReadThread.Suspend();
                int num = cbRangeCHA.SelectedIndex;     // Получаем индекс выбранного диапазона-преобразователя (0..1 - ПРЕОБР.(Д1 и Д2), 2 - AutoRange)                       
                SetTypeRange(num);                      // Отправляем команду установить заданный диапазон-преобразователь
                //Thread.Sleep(pause_ms);
            }
            finally
            {
                ReadThread.Resume();
//                timer1.Enabled = true;
//                timer1.Start();
            }           
        }


        // CHB
        private void cbRangeCHB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
//                timer1.Stop();
//                timer1.Enabled = false;
                ReadThread.Suspend();
                int num = cbRangeCHB.SelectedIndex;     // Получаем индекс выбранного диапазона-преобразователя (0..3 - ПРЕОБР.(0..3), 4 - AutoRange)                       
                SetTypeRange(num);                      // Отправляем команду установить заданный диапазон-преобразователь
                //Thread.Sleep(pause_ms);
            }
            finally
            {
//                timer1.Enabled = true;
//                timer1.Start();
                ReadThread.Resume();
            }
        }
        //................................................................................



        //................................................................................
        // *** Обработчик нажатия кнопки "УСТАВКА"
        // CHA
        private void bPointCHA_Click(object sender, EventArgs e)
        {
            pPoinSetCHA.Visible = true;        // делаем видимой панель с вводом значения уставки
        }

        // *** Обработчик ввода значение уставки "ПРИНЯТЬ"
        //CHA
        private void bPointSetOKCHA_Click(object sender, EventArgs e)
        {            
            double Point = (double)numPointSetCHA.Value;  // получаем заданное значение уставки
            SetPoint(Point);                        // отправляем значение уставки по COM порту    
            pPoinSetCHA.Visible = false;            // делаем панель с вводом уставки невидимой           
        }
        

        //CHB
        private void bPointCHB_Click(object sender, EventArgs e)
        {
            pPoinSetCHB.Visible = true;        // делаем видимой панель с вводом значения уставки
        }

        // *** Обработчик ввода значение уставки "ПРИНЯТЬ"       
        private void bPointSetOKCHB_Click(object sender, EventArgs e)
        {
            double Point = (double)numPointSetCHB.Value;  // получаем заданное значение уставки
            SetPoint(Point);                       // отправляем значение уставки по COM порту   
            pPoinSetCHB.Visible = false;           // делаем панель с вводом уставки невидимой           
        }                 
        //................................................................................



        //................................................................................
        // *** Обработчик нажатия кнопки "ВЫБОР ЕД.ИЗМ."
        //CHA
        private void cbUMeasCHA_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //                timer1.Stop();
                //                timer1.Enabled = false;
                ReadThread.Suspend();

                int num = cbUMeasCHA.SelectedIndex; // получаем номер выбранной ед.изм.            
                SetUMeas(num);                      // передаем команду установить соотв. ед. изм.
                //Thread.Sleep(pause_ms);
            }
            finally
            {
//                timer1.Enabled = true;
//                timer1.Start();
                ReadThread.Resume();
            }
        }

        //CHB
        private void cbUMeasCHB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
//                timer1.Stop();
//                timer1.Enabled = false;
                ReadThread.Suspend();

                int num = cbUMeasCHB.SelectedIndex; // получаем номер выбранной ед.изм.            
                SetUMeas(num);                      // передаем команду установить соотв. ед. изм.
                //Thread.Sleep(pause_ms);
            }
            finally
            {
                //                timer1.Enabled = true;
                //                timer1.Start();
                ReadThread.Resume();
            }
        }
        //................................................................................



        //................................................................................
        // *** Обработчик нажатия кнопки "ТИП ДАВЛ."
        //CHA
        private void cbTypePressCHA_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
//                timer1.Stop();
//                timer1.Enabled = false;
                ReadThread.Suspend();

                int num = cbTypePressCHA.SelectedIndex;     // получаем выбранный пользователем тип давления (номер 0 - Абс, 1 - Избыт.)
                SetTypePress(num);                          // передаем команду установить тип давления 
                //Thread.Sleep(pause_ms);
            }
            finally
            {
  //              timer1.Enabled = true;
  //              timer1.Start();
                ReadThread.Resume();
            }
        }

        //CHB
        private void cbTypePressCHB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {                
//                timer1.Stop();
//                timer1.Enabled = false;
                ReadThread.Suspend();

                int num = cbTypePressCHB.SelectedIndex;     // получаем выбранный пользователем тип давления (номер 0 - Абс, 1 - Избыт.)
                SetTypePress(num);                          // передаем команду установить тип давления        
                //Thread.Sleep(pause_ms);
            }
            finally
            {
//                timer1.Enabled = true;
//                timer1.Start();
                ReadThread.Resume();
            }
        }
        //................................................................................



        //................................................................................
        // *** Обработчик нажатия кнопки "ИЗМЕРЕНИЕ"
        //CHA
        private void bMeasureCHA_Click(object sender, EventArgs e)
        {
            SetMode(0);             // установить режим ИЗМЕРЕНИЕ (0)
        }

        //CHB
        private void bMeasureCHB_Click(object sender, EventArgs e)
        {
            SetMode(0);             // установить режим ИЗМЕРЕНИЕ (0)
        }
        //................................................................................



        //................................................................................
        // *** Обработчик нажатия кнопки "ЗАДАЧА"
        //CHA
        private void bControlCHA_Click(object sender, EventArgs e)
        {
            SetMode(1);                 // Установить режим ЗАДАЧА (1)
        }

        //CHB
        private void bControlCHB_Click(object sender, EventArgs e)
        {
            SetMode(1);                 // Установить режим ЗАДАЧА (1)
        }
        //................................................................................



        //................................................................................
        // *** Обработчик нажатия кнопки "СБРОС"
        void bResetCHA_Click(object sender, EventArgs e)
        {
            SetMode(2);                 // Установить режим СБРОС (2)
        }

        //CHB
        private void bResetCHB_Click(object sender, EventArgs e)
        {
            SetMode(2);                 // Установить режим СБРОС (2)
        }
        //................................................................................
               


        //................................................................................
        // *** ЧТЕНИЕ ДАННЫХ И НАСТРОЕК ПРИБОРА (функция вызывается из обработчика таймера)
        void ReadInitialSet(int CH)
        {
            double[] val = new double[] {0, 0}; // Мин. макс. значения диапазона        
            

            //CHA
            if (CH==0)
            {            
            // Считываем тип датчика-преобразователя
            typeR = ReadTypeRange();
                if (cbRangeCHA.SelectedIndex != typeR)
                {
                    cbRangeCHA.SelectedIndex = typeR;
                }
            Thread.Sleep(pause_ms);


            // Считываем установленную ед. измерения            
            umeas = ReadUMeas();
                if (cbUMeasCHA.SelectedIndex != umeas)
                {
                    cbUMeasCHA.SelectedIndex = umeas;       
                }                
            Thread.Sleep(pause_ms);


            // Считываем минимальный и максимальный значения диапазона, выводим их на экран      
            val = ReadRangeVal();         
            lRangeMinCHA.Text = Convert.ToString(val[0]*CalcMultCHA(umeas));       
            //lRangeMinCHA.Text = String.Format("{0,-6:#0.0#}", val[0]*CalcMult(umeas)); 
            lRangeMaxCHA.Text = Convert.ToString(val[1]*CalcMultCHA(umeas));
            //lRangeMaxCHA.Text = String.Format("{0,-6:#0.0#}", val[1]*CalcMult(umeas));
            Thread.Sleep(pause_ms);

            // Считываем значение уставки и выводим на экран
            point = ReadPoint();
            lPointCHA.Text = String.Format("{0,-5:0.#}", point);
            //lPointCHA.Text = String.Format("{0,-5:0.#}", ReadPoint());
            Thread.Sleep(pause_ms);

            // считываем установленный тип давления
            tpress = ReadTypePress();     
                if(cbTypePressCHA.SelectedIndex != tpress)
                {
                    cbTypePressCHA.SelectedIndex = tpress;
                }           
            Thread.Sleep(pause_ms);

            // Считываем текущее давление, Барометр и Скорость - выводим на экран
            //lDataCHA.Text = Convert.ToString(ReadPRESS());
            press = ReadPRESS();
                if (press >= (point - point*0.05))
                {
                    lDataCHA.ForeColor = Color.SpringGreen;
                }
                else
                {
                    lDataCHA.ForeColor = Color.White;
                }                  
            lDataCHA.Text = String.Format("{0,-9:#0.00000#}", press);
       

            //lBarometerCHA.Text = Convert.ToString(ReadBAR());
                lBarometerCHA.Text = String.Format("{0,-8:#0.000#}", ReadBAR()/PsiToPa);

            //lSpeedCHA.Text = Convert.ToString(ReadRATE());
            rate = ReadRATE();
            lSpeedCHA.Text = String.Format("{0,-8:#0.000#}", rate);

            // считываем установленный режим            
            mode = ReadMode();                                 
            if (mode == 0)
            {
                bMeasureCHA.FlatAppearance.BorderSize = 4;
                bControlCHA.FlatAppearance.BorderSize = 1;
                bResetCHA.FlatAppearance.BorderSize = 1;
            }       
            else if (mode == 1)
            {
                bMeasureCHA.FlatAppearance.BorderSize = 1;
                bControlCHA.FlatAppearance.BorderSize = 4;
                bResetCHA.FlatAppearance.BorderSize = 1;
            }       
            else if (mode == 2)
            {
                bMeasureCHA.FlatAppearance.BorderSize = 1;
                bControlCHA.FlatAppearance.BorderSize = 1;
                bResetCHA.FlatAppearance.BorderSize = 4;
            }

            }


            //CHB
            else if(CH==1)
            {
            // Считываем тип датчика-преобразователя
            typeR = ReadTypeRange();
                if (cbRangeCHB.SelectedIndex != typeR)
                {
                    cbRangeCHB.SelectedIndex = typeR;
                }
            Thread.Sleep(pause_ms);

            // Считываем установленную ед. измерения          
            umeas = ReadUMeas();
                if(cbUMeasCHB.SelectedIndex != umeas)
                {
                    cbUMeasCHB.SelectedIndex = umeas;
                }              
            Thread.Sleep(pause_ms);

            // Считываем минимальный и максимальный значения диапазона, выводим их на экран       
            val = ReadRangeVal();           
            lRangeMinCHB.Text = Convert.ToString(val[0]*CalcMultCHB(umeas));
            //lRangeMinCHB.Text = String.Format("{0,-6:#0.0#}", val[0]*CalcMult(umeas));             
            lRangeMaxCHB.Text = Convert.ToString(val[1]*CalcMultCHB(umeas));
            //lRangeMinCHB.Text = String.Format("{0,-6:#0.0#}", val[1]*CalcMult(umeas));      
            Thread.Sleep(pause_ms);

            // Считываем значение уставки и выводим на экран
            point = ReadPoint();
            lPointCHB.Text = String.Format("{0,-5:0.#}", point);
            Thread.Sleep(pause_ms);

            // считываем установленный тип давления            
            tpress = ReadTypePress();
                if (cbTypePressCHB.SelectedIndex != tpress)
                {
                    cbTypePressCHB.SelectedIndex = tpress;   
                }            
            Thread.Sleep(pause_ms);

            // Считываем текущее давление, Барометр и Скорость - выводим на экран
            //lDataCHB.Text = String.Format("{0,-9:#0.00000#}", ReadPRESS());
            press = ReadPRESS();
            if (press >= (point - point * 0.05))
                {
                    lDataCHA.ForeColor = Color.SpringGreen;
                }
            else
                {
                    lDataCHA.ForeColor = Color.White;
                }
             lDataCHB.Text = String.Format("{0,-9:#0.00000#}", press);              

             //lBarometerCHB.Text = Convert.ToString(ReadBAR());
             lBarometerCHB.Text = String.Format("{0,-8:#0.000#}", ReadBAR()/PsiToPa);

            //lSpeedCHB.Text = Convert.ToString(ReadRATE());
            lSpeedCHB.Text = String.Format("{0,-8:#0.000#}", ReadRATE());

            // считываем установленный режим           
            mode = ReadMode();
            if (mode == 0)
            {
                bMeasureCHB.FlatAppearance.BorderSize = 4;
                bControlCHB.FlatAppearance.BorderSize = 1;
                bResetCHB.FlatAppearance.BorderSize = 1;
            }
            else if (mode == 1)
            {
                bMeasureCHB.FlatAppearance.BorderSize = 1;
                bControlCHB.FlatAppearance.BorderSize = 4;
                bResetCHB.FlatAppearance.BorderSize = 1;
            }
            else if (mode == 2)
            {
                bMeasureCHB.FlatAppearance.BorderSize = 1;
                bControlCHB.FlatAppearance.BorderSize = 1;
                bResetCHB.FlatAppearance.BorderSize = 4;
            }
            }
        }
        //................................................................................

        //Поток опроса менсора 
         //Поток останавливается при завершении сенса связи с менсором
        void MensorReadThread()
        {
            while (_serialPort_M.IsOpen)
            {
                try
                {
                    activCH = ChannelRead();    // считываем номер активного канала  

                    switch (activCH)
                    {
                        case 0: // Канал A
                                // Меняем цвет кнопок                
                            bSetCHA.BackColor = Color.DarkGreen;
                            bSetCHB.BackColor = Color.Transparent;
                            // Делаем активными кнопки поля канала A и неактивными поля канала B                   
                            EnableCHA(true);
                            EnableCHB(false);
                            // Читаем и устанавливаем значения параметров окна A
                            ReadInitialSet(0);
                            break;

                        case 1: // Канал B
                                // Меняем цвет кнопок                
                            bSetCHA.BackColor = Color.Transparent;
                            bSetCHB.BackColor = Color.DarkSlateBlue;
                            // Делаем активными кнопки поля канала B и неактивными поля канала A                    
                            EnableCHA(false);
                            EnableCHB(true);
                            // Читаем и устанавливаем значения параметров окна B
                            ReadInitialSet(1);
                            break;

                        default:
                            break;
                    }
                }
                catch
                {
                    // если считать не удалось (ошибка по TimeOut или другая... перезапускаем COM-порт)
                    //Program.txtlog.WriteLineLog("Mensor: Ошибка чтения данных", 1);
                    //Console.WriteLine("Mensor: Ошибка чтения данных");
                    _serialPort_M.Close();
                    Connected = false;
                    /*_serialPort_M.Open();
                    Task.Delay(20);*/
                }
            }
        }


        //................................................................................
        // ОБРАБОТЧИК СРАБАТЫВАНИЯ ТАЙМЕРА
        // (1 раз в секунду - по умолчанию)       
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!_serialPort_M.IsOpen) return;      // если COM отключен  - выходим из процедуры

            // Если процедура обработки еще не закончена - выходим (пропускаем)
            if (checkTimer) return;
            
            // Обработчик...
            checkTimer = true;
            bool st = true;

            try
            {
                activCH = ChannelRead();    // считываем номер активного канала  

                switch (activCH)
                {
                    case 0: // Канал A
                        // Меняем цвет кнопок                
                        bSetCHA.BackColor = Color.DarkGreen;
                        bSetCHB.BackColor = Color.Transparent;
                        // Делаем активными кнопки поля канала A и неактивными поля канала B                   
                        EnableCHA(st);
                        EnableCHB(!st);
                        // Читаем и устанавливаем значения параметров окна A
                        ReadInitialSet(0);
                        break;

                    case 1: // Канал B
                        // Меняем цвет кнопок                
                        bSetCHA.BackColor = Color.Transparent;
                        bSetCHB.BackColor = Color.DarkSlateBlue;
                        // Делаем активными кнопки поля канала B и неактивными поля канала A                    
                        EnableCHA(!st);
                        EnableCHB(st);                      
                        // Читаем и устанавливаем значения параметров окна B
                        ReadInitialSet(1);
                        break;

                    default:
                        break;
                }
            }
            catch
            {
                // если считать не удалось (ошибка по TimeOut или другая... перезапускаем COM-порт)
                Console.WriteLine("Ошибка чтения по таймеру");
                _serialPort_M.Close();
                Thread.Sleep(20);
                _serialPort_M.Open();
            }
            finally
            {
                checkTimer = false;
            }
        }
        //................................................................................










  
        ///////////////////////////////////////////////////////////////////////////////////////////////
        //
        // Функции реализующие прием/передачу данных и команд по COM порту
        //
        ///////////////////////////////////////////////////////////////////////////////////////////////

                       

        //--------------------------------------------------------------------------------
        // ФУНКЦИЯ - Установка активного канала (запись)
        // принимаемые значения:  A - сделать активным канал А
        //                        B - сделать активным канал B
        // возвращаемые значения: нет
        public void ChannelSet(string CH)
        {
            _serialPort_M.WriteLine("OUTP:CHAN " + CH);      
            // вывод запроса в статусну строку
            //toolStripStatusLabel1.Text = ("SEND: " + "OUTP:CHAN " + CH);
        }
        //--------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------
        // ФУНКЦИЯ - Чтение активного канала (запись/чтение)
        // принимаемые значения:  нет
        // возвращаемые значения: 0 - активный канал А
        //                        1 - активный канал В     
        //                       -1 активный канал D/нет ответа/или не правильно интерпретировали команду
        int ChannelRead()
        {
            string str = "";
            int res = -1;
            try
            {
                _serialPort_M.WriteLine("OUTP:CHAN?");  // запрашиваем номер активного канала
                                                        //toolStripStatusLabel1.Text = ("SEND: " + "OUTP:CHAN?");
                                                        //            Task.Delay(pause_ms);
                Thread.Sleep(pause_ms);

                str = _serialPort_M.ReadLine();         // считываем
                                                        //toolStripStatusLabel2.Text = ("READ: " + str);

                if ((str == "A") | (str == "0"))
                {
                    res = 0;
                }
                else if ((str == "B") | (str == "1"))
                {
                    res = 1;
                }
                else
                {
                    res = -1;
                }
            }
            catch
            {
                return -1;
            }
            return res;
        }
        //--------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------
        // ФУНКЦИЯ - Установки удерживаемого диапазона-преобразователя (запись/чтение)
        // принимаемые значения:  0 - сделать активным Д1П1
        //                        1 - сделать активным Д2П1                               
        //                        2 - сделать активным AutoRange
        // возвращаемые значения: нет 
        public void SetTypeRange(int Num)
        {
            switch (Num)
            {
                case 0:
                    {
                        //_serialPort_M.WriteLine("OUTP:AUTOR OFF");      // отключаем AutoRange      
                        //toolStripStatusLabel1.Text = ("SEND: " + "OUTP:AUTOR OFF");
                        //Thread.Sleep(pause_ms);

                        _serialPort_M.WriteLine("SENS:ACT 11");      // устанавливаем активным Преобр. 11
                        //toolStripStatusLabel1.Text = ("SEND: " + "SENS:ACT 11");
                        break;
                    }
                case 1:
                    {
                        //_serialPort_M.WriteLine("OUTP:AUTOR OFF");      // отключаем AutoRange      
                        //toolStripStatusLabel1.Text = ("SEND: " + "OUTP:AUTOR OFF");
                        //Thread.Sleep(pause_ms);

                        _serialPort_M.WriteLine("SENS:ACT 21");      // устанавливаем активным Преобр. 12
                        //toolStripStatusLabel1.Text = ("SEND: " + "SENS:ACT 21");
                        break;
                    }
                case 2:
                    {
                        _serialPort_M.WriteLine("OUTP:AUTOR ON");       // включаем AutoRange
                        //toolStripStatusLabel1.Text = ("SEND: " + "OUTP:AUTOR ON");
                        break;
                    }                                    
                default:
                    {
                        break;
                    }
            }
        }
        //--------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------
        // ФУНКЦИЯ - Считывания удерживаемого диапазона-преобразователя (запись-чтение) 
        // принимаемые значения:  нет
        // возвращаемые значения: 0..1 - номер активного Преобр., или 2 - AutoRange
        public int ReadTypeRange()
        {
            int res = -1;
            string str = "";

            _serialPort_M.WriteLine("OUTP:AUTOR?");  // запрашиваем состояние AutoRange (ON/OFF)
            //toolStripStatusLabel1.Text = ("SEND: " + "OUTP:AUTOR?");

            Thread.Sleep(pause_ms);

            str = _serialPort_M.ReadLine();           // считываем ответ прибора 
            //toolStripStatusLabel2.Text = ("READ: " + str);

            if (str == "\"ON\"")        // если включен
            {
                res = 2;
            }
            else if (str == "\"OFF\"")  // если выключен
            {
                // Запрашиваем номер активного преобразователя
                _serialPort_M.WriteLine("SENS:ACT?");
                //toolStripStatusLabel1.Text = ("SEND: " + "SENS:ACT?");

                Thread.Sleep(pause_ms);

                str = _serialPort_M.ReadLine();   // Считываем ответ прибора (что возвращает номер или имя?)
                //toolStripStatusLabel2.Text = ("READ: " + str);               
                res = Convert.ToInt32(str);

                switch (res)
                {
                    case 11:
                    res = 0;
                    break;
                        
                    case 21:
                    res = 1;
                    break;

                    default:
                    res = -1;
                    break;
                }               
            }
            return res;
        }
        //--------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------
        // ФУНКЦИЯ - Считать Мин и Макс значения удержания диапазона (запись-чтение)
        // принимаемые значения:  нет
        // возвращаемые значения: val массив содержит: мин. и макс. значения удержания диапазона  
        double[] ReadRangeVal()
        {
            double[] val = new double[] { -1.0, -1.0 };
            string str;
            
            _serialPort_M.WriteLine("SENS:PRES:RANG:LOW?");  // запрашиваем мин. значение диапазона
            //toolStripStatusLabel1.Text = ("SEND: " + "SENS:RANG:LOW?");

            Thread.Sleep(pause_ms);

            str = _serialPort_M.ReadLine();     // считываем
            val[0] = StrToDouble(str);            
            //toolStripStatusLabel2.Text = ("READ: " + Convert.ToString(val[0]));
            //Thread.Sleep(pause_ms);

            _serialPort_M.WriteLine("SENS:PRES:RANG:UPP?");   // запрашиваем макс. значение диапазона
            //toolStripStatusLabel1.Text = ("SEND: " + "SENS:RANG:UPP?");

            Thread.Sleep(pause_ms);

            str = _serialPort_M.ReadLine();
            val[1] = StrToDouble(str);    // считываем
            //toolStripStatusLabel2.Text = ("READ: " + Convert.ToString(val[1]));
            return val;
        }
        //--------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------
        // ФУНКЦИЯ - Задает уставку Point (запись)
        // принимаемые значения:    Val - значение уставки        
        // возвращаемые значения:   нет           
        public void SetPoint(double Val)
        {
           _serialPort_M.WriteLine("SOUR:PRES:LEV:IMM:AMPL " + Convert.ToString(Val));            
            //toolStripStatusLabel1.Text = ("SEND: " + "SOUR:PRES:LEV:IMM:AMPL " + Convert.ToString(Val));
        }
        //--------------------------------------------------------------------------------


        //--------------------------------------------------------------------------------
        // ФУНКЦИЯ  Считывет заданную уставку Point (чтение)
        // принимаемые значения:    нет        
        // возвращаемые значения:   res - считанное знаение уставки (string)           
        double ReadPoint()
        {
            double res = -1;

            _serialPort_M.WriteLine("SOUR:PRES:LEV:IMM:AMPL?");
            //toolStripStatusLabel1.Text = ("SEND: " + "SOUR:PRES:LEV:IMM:AMPL?");

            Thread.Sleep(pause_ms);

            string str = _serialPort_M.ReadLine();
            //toolStripStatusLabel2.Text = ("READ: " + str);
            res = StrToDouble(str); 

            return res;
        }
        //--------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------
        // ФУНКЦИЯ - Установка ед. измерения (запись)
        // принимаемые значения:    0 - бар
        //                          1 - мбар
        //                          2 - Па 
        //                          3 - кПа
        //                          4 - МПа
        // возвращаемые значения:   нет
        void SetUMeas(int num)
        {
            switch (num)
            {
                case 0:
                    _serialPort_M.WriteLine("UNIT:INDEX 0");
                    //toolStripStatusLabel1.Text = ("SEND: " + "UNIT:INDEX 0");
                    break;
                case 1:
                    _serialPort_M.WriteLine("UNIT:INDEX 1");
                    //toolStripStatusLabel1.Text = ("SEND: " + "UNIT:INDEX 1");
                    break;
                case 2:
                    _serialPort_M.WriteLine("UNIT:INDEX 2");
                    //toolStripStatusLabel1.Text = ("SEND: " + "UNIT:INDEX 2");
                    break;
                case 3:
                    _serialPort_M.WriteLine("UNIT:INDEX 7");
                    //toolStripStatusLabel1.Text = ("SEND: " + "UNIT:INDEX 7");
                    break;
                case 4:
                    _serialPort_M.WriteLine("UNIT:INDEX 29");
                    //toolStripStatusLabel1.Text = ("SEND: " + "UNIT:INDEX 29");
                    break;
                default:
                    break;
            }
        }
        //--------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------
        // ФУНКЦИЯ - Чтение установленных ед. измерения (чтение)
        // принимаемые значения:     нет
        // возвращаемые значения:    0 - бар
        //                           1 - мбар
        //                           2 - Па 
        //                           3 - кПа
        //                           4 - МПа
        int ReadUMeas()
        {
            int res = -1;
            int ind = -1;           

            _serialPort_M.WriteLine("UNIT:INDEX?");
            //toolStripStatusLabel1.Text = ("SEND: " + "UNIT:INDEX?");

            Thread.Sleep(pause_ms);

            ind = Convert.ToInt32(_serialPort_M.ReadLine());            
            //toolStripStatusLabel2.Text = ("READ: " + Convert.ToString(ind));

            switch (ind)
            {
                case 0:                    
                    res = 0;
                    break;
                case 1:
                    res = 1;
                    break;
                case 2:
                    res = 2;
                    break;
                case 7:
                    res = 3;
                    break;
                case 29:
                    res = 4;
                    break;
                default:
                    res = -1;
                    break;
            }
            return res;
        }
        //--------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------
        // ФУНКЦИЯ - Установка типа давления (запись)
        // принимаемые значения:    0 - Абсолютное (Absolute)
        //                          1 - Избыточное (Gauge)
        // возвращаемые значения:   нет                         
        void SetTypePress(int num)
        {
            switch (num)
            {
                case 0:
                    _serialPort_M.WriteLine("SENS:PRES:MODE ABS"); 
                    //toolStripStatusLabel1.Text = ("SEND: " + "SENS:PRES:MODE: ABS");
                    break;
                case 1:
                    _serialPort_M.WriteLine("SENS:PRES:MODE GAUGE");
                    //toolStripStatusLabel1.Text = ("SEND: " + "SENS:PRES:MODE: GAUGE");
                    break;
                default:
                    break;
            }
        }
        //--------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------
        // ФУНКЦИЯ - Чтение типа давления-преобразователя (чтение)
        // принимаемые значения:   нет
        // возвращаемые значения: 0 - ABSOLUTE
        //                        1 - GAUGE
        int ReadTypePress()
        {
            int res = -1;
            string str = "";

            _serialPort_M.WriteLine("SENS:PRES:MODE?");     // запрашиваем установленный тип давления
            //toolStripStatusLabel1.Text = ("SEND: " + "SENS:PRES:MODE?");
            
            Thread.Sleep(pause_ms);

            str = _serialPort_M.ReadLine();         // считываем
            //toolStripStatusLabel2.Text = ("READ: " + str);

            if (str == "\"ABSOLUTE\"")
            {
                res = 0;
            }
            else if (str == "\"GAUGE\"")
            {
                res = 1;
            }            
            return res;
        }
        //--------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------
        // ФУНКЦИЯ - Задает режим работы: ИЗМЕРЕНИЕ, ЗАДАЧА, СБРОС (запись)
        // принимаемые значения:    0 - ИЗМЕРЕНИЕ
        //                          1 - ЗАДАЧА
        //                          2 - СБРОС
        // возвращаемые значения:   нет          
        public void SetMode(int num)
        {
            switch (num)
            {
                case 0:
                    _serialPort_M.WriteLine("OUTP:MODE MEAS");  
                    //toolStripStatusLabel1.Text = ("SEND: " + "OUTP:MODE MEAS");
                    break;
                case 1:
                    _serialPort_M.WriteLine("OUTP:MODE CONT");
                    //toolStripStatusLabel1.Text = ("SEND: " + "OUTP:MODE CONT");
                    break;
                case 2:
                    _serialPort_M.WriteLine("OUTP:MODE VENT");
                    //toolStripStatusLabel1.Text = ("SEND: " + "OUTP:MODE VENT");
                    break;
                default:
                    break;
            }
        }
        //--------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------
        // ФУНКЦИЯ - Считывает режим работы: ИЗМЕРЕНИЕ, ЗАДАЧА, СБРОС (запись)
        // принимаемые значения:    нет
        // возвращаемые значения:   0 - ИЗМЕРЕНИЕ
        //                          1 - ЗАДАЧА
        //                          2 - СБРОС         
        int ReadMode()
        {
            string str = "";
            int res = -1;

            _serialPort_M.WriteLine("OUTP:MODE?");   // Запрашиваем режим работы
            //toolStripStatusLabel1.Text = ("SEND: " + "OUTP:MODE?");
            
            Thread.Sleep(pause_ms);

            str = _serialPort_M.ReadLine();
            //toolStripStatusLabel2.Text = ("READ: " + str);

            if (str == "\"MEAS\"")
            {
                res = 0;
            }
            else if (str == "\"CONT\"")
            {
                res = 1;
            }
            else if (str == "\"VENT\"")
            {
                res = 2;
            }

            return res;
        }
        //--------------------------------------------------------------------------------
        


        //--------------------------------------------------------------------------------
        // ФУНКЦИЯ - Чтение показаний: ДАВЛЕНИЕ
        // принимаемые значения:   нет
        // возвращаемые значения:  считанное показание Давление
        double ReadPRESS()
        {
            double res = 0;

            _serialPort_M.WriteLine("MEAS:PRES?");     // запрашиваем показание (не понятно диапазон R???)
            //toolStripStatusLabel1.Text = ("SEND: " + "MEAS:PRES?");
            Thread.Sleep(pause_ms);

            string str = _serialPort_M.ReadLine();
            res = StrToDouble(str);         
            //toolStripStatusLabel2.Text = ("READ: " + Convert.ToString(res));
            return res;
        }
        //--------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------
        // ФУНКЦИЯ - Чтение показаний: БАРОМЕТР
        // принимаемые значения:   нет
        // возвращаемые значения:  считанное показание Барометр
        double ReadBAR()
        {
            double res = 0;

            _serialPort_M.WriteLine("MEAS:BARO?");     // запрашиваем показание
            //toolStripStatusLabel1.Text = ("SEND: " + "MEAS:BARO?");

            Thread.Sleep(pause_ms);

            string str = _serialPort_M.ReadLine();
            res = StrToDouble(str);    
            //toolStripStatusLabel2.Text = ("READ: " + Convert.ToString(res));
            return res;
        }
        //--------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------
        // ФУНКЦИЯ - Чтение показаний: СКОРОСТЬ (RATE)
        // принимаемые значения:   нет
        // возвращаемые значения:  считанное показание Скорости
        double ReadRATE()
        {
            double res = 0;

            _serialPort_M.WriteLine("MEAS:RATE?");     // запрашиваем показание
            //toolStripStatusLabel1.Text = ("SEND: " + "MEAS:RATE?");

            Thread.Sleep(pause_ms);

            string str = _serialPort_M.ReadLine();
            res = StrToDouble(str);    
           // toolStripStatusLabel2.Text = ("READ: " + Convert.ToString(res));
            return res;
        }
        //--------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------
        // ФУНКЦИЯ - Установка сброса на ноль канал A
        // принимаемые значения:   нет
        // возвращаемые значения:  нет
        private void ZeroCHA_Click(object sender, EventArgs e)
        {

            //            timer1.Stop();
            //            timer1.Enabled = false;
            try
            {


                ReadThread.Suspend();

                _serialPort_M.WriteLine("CAL:PRES:ZERO:RUN");
                //toolStripStatusLabel1.Text = ("SEND: CAL: PRES: ZERO: RUN");
            }
            finally
            {
                //            timer1.Enabled = true;
                //            timer1.Start();
                ReadThread.Resume();
            }
        }

        //CHB
        private void ZeroCHB_Click(object sender, EventArgs e)
        {
            try
            {
                //            timer1.Stop();
                //            timer1.Enabled = false;
                ReadThread.Suspend();

                _serialPort_M.WriteLine("CAL:PRES:ZERO:RUN");
                //toolStripStatusLabel1.Text = ("SEND: CAL: PRES: ZERO: RUN");
            }
            finally
            {
                ReadThread.Resume();
                //            timer1.Enabled = true;
                //            timer1.Start();
            }
        }
        //--------------------------------------------------------------------------------








        //********************************************************************************
        //
        //   Вспомогательные функции
        //
        //********************************************************************************


        // Делает активными (state = true)  или неактивными (state = false) кнопки поля канала A
        public void EnableCHA(bool state)
        {
            cbRangeCHA.Enabled = state;
            bPointCHA.Enabled = state;
            cbUMeasCHA.Enabled = state;
            cbTypePressCHA.Enabled = state;
            bBarometerCHA.Enabled = state;
            bSpeedCHA.Enabled = state;
            bMeasureCHA.Enabled = state;
            bControlCHA.Enabled = state;
            bResetCHA.Enabled = state;
            ZeroCHA.Enabled = state;
        }
        

        // Делает активными (state = true)  или неактивными (state = false) кнопки поля канала В
        public void EnableCHB(bool state)
        {
            cbRangeCHB.Enabled = state;
            bPointCHB.Enabled = state;
            cbUMeasCHB.Enabled = state;
            cbTypePressCHB.Enabled = state;
            bBarometerCHB.Enabled = state;
            bSpeedCHB.Enabled = state;
            bMeasureCHB.Enabled = state;
            bControlCHB.Enabled = state;
            bResetCHB.Enabled = state;
            ZeroCHB.Enabled = state;
        }
                

        // Включение/Выключение ручного режима
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
             // останавливаем таймер                
//                timer1.Stop();
//                timer1.Enabled = false;
                ReadThread.Suspend();

                bSendCom.Enabled = true;
                EnableCHA(false);
                EnableCHB(false);
            }
            else
            {                
                EnableCHA(true);
                EnableCHB(true);

                bSendCom.Enabled = false;
                // запускаем таймер
//                timer1.Enabled = true;
//                timer1.Start();
                ReadThread.Resume();
            }
        }


        // Отправка cообщения в ручном режиме
        private void bSendCom_Click(object sender, EventArgs e)
        {
            string str = tbComamd.Text;

            _serialPort_M.WriteLine(str);
            //toolStripStatusLabel1.Text = ("SEND: " + str);

            if (checkBox2.Checked)
            {
                Thread.Sleep(pause_ms);
                str = _serialPort_M.ReadLine();         // считываем ответ
                tbAnswer.Text = str;                    // выводим на экран
                //toolStripStatusLabel2.Text = ("READ: " + str);
            }                       
        }


        // Коэффициенты для перерасчета мин. и макс. диапазонов канала А
        double CalcMultCHA(int ind)
        {
            double res = 1;

            switch (ind)
            {
                case 0:
                    {
                        res = 1;
                        break;
                    }
                case 1:
                    {
                        res = 1000;
                        break;
                    }
                case 2:
                    {
                        res = 100000;
                        break;
                    }
                case 3:
                    {
                        res = 100;
                        break;
                    }
                case 4:
                    {
                        res = 0.1;
                        break;
                    }
            }
            return res;
        }


        // Коэффициенты для перерасчета мин. и макс. диапазонов канала А
        double CalcMultCHB(int ind)
        {
            double res = 1;

            switch (ind)
            {
                case 0:
                    {
                        res = 0.01;
                        break;
                    }
                case 1:
                    {
                        res = 10;
                        break;
                    }
                case 2:
                    {
                        res = 10000;
                        break;
                    }
                case 3:
                    {
                        res = 1;
                        break;
                    }
                case 4:
                    {
                        res = 100;
                        break;
                    }
            }
            return res;
        }

        
        // Перевод числа в double (вне зависимости от разделителя точка или запятая)
        double StrToDouble(string s)
        {
            // string str = s.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
            return double.Parse(s.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
            // double val = double.Parse(str, CultureInfo.InvariantCulture);
            // return val;
        }



    }

}

 