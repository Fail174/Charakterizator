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
        // Занесены в настройки
        public int READ_PERIOD = 1000;  // Время опроса состояния менсора при работе с формой
        public int READ_PAUSE = 50;            // задержка между приемом и передачей команд по COM порту, мс      


        // Не занесенные
        private Thread ReadThread;          // поток
        public SerialPort _serialPort_M;    // переменная для работы с COM портом     
        public bool checkTimer = false;     // флаг запущен таймер или нет         
        public bool Connected = false;      // true  - соединение установлено,         
        public const double PsiToPa = 0.14503773773;  // для перевода psi в кПА    

        public double UserPoint = 0;

        public int activCH = -1;            // номер активного канала (0 - канал А, 1 - канал В)
        public int _activCH
        {   get {return activCH;}
            set { }        }

        int typeR;                          // Тип ДатПреобр - 1..2 Д1-Д2, 2- AutoRange
        public int _typeR
        {   get { return typeR; }
            set { }        }

        double typeR_Pmin;                 // Мин давление преобразовател
        public double _typeR_Pmin
        {   get { return typeR_Pmin; }
            set { }        }

        double typeR_Pmax;                  // Макс давление преобразовател
        public double _typeR_Pmax
        {   get { return typeR_Pmax; }
            set { }        }

        int umeas;                          // Ед.изм.
        public int _umeas
        {   get { return umeas; }
            set { }        }
        
        int tpress;                         // Тип давления
        public int _tpress
        {   get { return tpress; }
            set { }        }
        
        double press;                       // Тек. давление
        public double _press
        {   get { return press; }
            set { }        }
        
        double point;                       // Уставка
        public double _point
        {   get { return point; }
            set { }        }
        
        int mode;                           // Режим 0-ИЗМ.  1-ЗАДАЧА  2-СБРОС
        public int _mode
        {   get { return mode; }
            set { }        }

        double rate;                       // Режим 0-ИЗМ.  1-ЗАДАЧА  2-СБРОС
        public double _rate
        {   get { return rate; }
            set { }
        }

        double barometr;                   // Режим 0-ИЗМ.  1-ЗАДАЧА  2-СБРОС
        public double _barometr
        {   get { return barometr; }
            set { }        }


        public List<string> ListMod = new List<string>();       // список подключенных модулей внутренныих и внешних
        //---------------------------------------------------------------------------


        public FormMensor()
        {
            InitializeComponent();
            _serialPort_M = new SerialPort();           
        }


        //*** Функция отключения от СОМ-порта
        public int DisConnect()
        {
            if (Connected)
            {               
                if (ReadThread != null)
                    ReadThread.Abort(0);

                timer1.Stop();
                timer1.Enabled = false;

                if (_serialPort_M.IsOpen)
                {
                    _serialPort_M.Close();                    
                }
                Connected = false;
                ListMod.Clear();
                return 0;
            }
            else
            {
                return 1;
            }
        }


        //*** Функция подключения коммутатора по COM порту
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

                //функция считывающая что менсор реально подключен а не просто открыт порт для него
                int ReadMensorID = ChannelRead();

                if ((ReadMensorID==0) || (ReadMensorID == 1))
                {
                    //timer1_Tick(null, null);// Вызываем функцию для начального обновления формы
                    ListMod.AddRange(new string[] { "[канал А] ДП-1", "[канал А] ДП-2", "[канал А] AutoRange", "[канал B] ДП-1", "[канал B] ДП-2", "[канал B] AutoRange", });        // список подключенных модулей
                    //ListMod.AddRange(new string[] { "[канал А] AutoRange", "[канал B] AutoRange"});        // список подключенных модулей

                    // Запускаем поток
                    ReadThread = new Thread(MensorReadThread);
                    ReadThread.Priority = ThreadPriority.AboveNormal;
                    ReadThread.Start();
                    Thread.Sleep(1000);

                    Connected = true;
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
                //timer1.Stop();
                //timer1.Enabled = false;
                //ReadThread.Suspend();
                int num = cbRangeCHA.SelectedIndex;     // Получаем индекс выбранного диапазона-преобразователя (0..1 - ПРЕОБР.(Д1 и Д2), 2 - AutoRange)                       
                SetTypeRange(num);                      // Отправляем команду установить заданный диапазон-преобразователь
                //Thread.Sleep(pause_ms);
            }
            finally
            {
               // ReadThread.Resume();
                timer1.Enabled = true;
                timer1.Start();
            }           
        }


        // CHB
        private void cbRangeCHB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                timer1.Stop();
                timer1.Enabled = false;
                //ReadThread.Suspend();
                int num = cbRangeCHB.SelectedIndex;     // Получаем индекс выбранного диапазона-преобразователя (0..3 - ПРЕОБР.(0..3), 4 - AutoRange)                       
                SetTypeRange(num);                      // Отправляем команду установить заданный диапазон-преобразователь
                //Thread.Sleep(pause_ms);
            }
            finally
            {
                timer1.Enabled = true;
                timer1.Start();
                //ReadThread.Resume();
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
                timer1.Stop();
                timer1.Enabled = false;
                //ReadThread.Suspend();

                int num = cbUMeasCHA.SelectedIndex; // получаем номер выбранной ед.изм.            
                SetUMeas(num);                      // передаем команду установить соотв. ед. изм.
                //Thread.Sleep(pause_ms);
            }
            finally
            {
                timer1.Enabled = true;
                timer1.Start();
                //ReadThread.Resume();
            }
        }

        //CHB
        private void cbUMeasCHB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                timer1.Stop();
                timer1.Enabled = false;
                //ReadThread.Suspend();
                int num = cbUMeasCHB.SelectedIndex; // получаем номер выбранной ед.изм.            
                SetUMeas(num);                      // передаем команду установить соотв. ед. изм.
                //Thread.Sleep(pause_ms);
            }
            finally
            {
                timer1.Enabled = true;
                timer1.Start();
                //ReadThread.Resume();
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
                timer1.Stop();
                timer1.Enabled = false;
                //ReadThread.Suspend();
                int num = cbTypePressCHA.SelectedIndex;     // получаем выбранный пользователем тип давления (номер 0 - Абс, 1 - Избыт.)
                SetTypePress(num);                          // передаем команду установить тип давления 
                //Thread.Sleep(pause_ms);
            }
            finally
            {
                timer1.Enabled = true;
                timer1.Start();
                //ReadThread.Resume();
            }
        }

        //CHB
        private void cbTypePressCHB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {                
                timer1.Stop();
                timer1.Enabled = false;
                //ReadThread.Suspend();

                int num = cbTypePressCHB.SelectedIndex;     // получаем выбранный пользователем тип давления (номер 0 - Абс, 1 - Избыт.)
                SetTypePress(num);                          // передаем команду установить тип давления        
                //Thread.Sleep(pause_ms);
            }
            finally
            {
                timer1.Enabled = true;
                timer1.Start();
                //ReadThread.Resume();
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
            double[] val = new double[] { 0, 0 }; // Мин. макс.    

            try
            {  //CHA
                if (CH == 0)
                {
                    // Считываем тип датчика-преобразователя
                    typeR = ReadTypeRange();
                    Thread.Sleep(READ_PAUSE);
                    // Считываем установленную ед. измерения            
                    umeas = ReadUMeas();
                    Thread.Sleep(READ_PAUSE);
                    // Считываем минимальный и максимальный значения диапазона, 
                    val = ReadRangeVal();
                    typeR_Pmin = val[0] * CalcMultCHA(umeas);
                    typeR_Pmax = val[1] * CalcMultCHA(umeas);
                    Thread.Sleep(READ_PAUSE);
                    // Считываем значение уставки и выводим на экран
                    point = ReadPoint();
                    Thread.Sleep(READ_PAUSE);
                    // считываем установленный тип давления
                    tpress = ReadTypePress();
                    Thread.Sleep(READ_PAUSE);
                    // Считываем текущее давление, Барометр и Скорость - выводим на экран           
                    press = ReadPRESS();
                    Thread.Sleep(READ_PAUSE);
                    barometr = ReadBAR();
                    Thread.Sleep(READ_PAUSE);
                    rate = ReadRATE();
                    Thread.Sleep(READ_PAUSE);
                    // считываем установленный режим            
                    mode = ReadMode();
                }

                //CHB
                else if (CH == 1)
                {
                    // Считываем тип датчика-преобразователя
                    typeR = ReadTypeRange();
                    Thread.Sleep(READ_PAUSE);
                    // Считываем установленную ед. измерения          
                    umeas = ReadUMeas();
                    Thread.Sleep(READ_PAUSE);
                    // Считываем минимальный и максимальный значения диапазона    
                    val = ReadRangeVal();
                    typeR_Pmin = val[0] * CalcMultCHB(umeas);
                    typeR_Pmax = val[1] * CalcMultCHB(umeas);
                    Thread.Sleep(READ_PAUSE);
                    // Считываем значение уставки
                    point = ReadPoint();
                    Thread.Sleep(READ_PAUSE);
                    // считываем установленный тип давления            
                    tpress = ReadTypePress();
                    Thread.Sleep(READ_PAUSE);
                    // Считываем текущее давление, и  - выводим на экран           
                    press = ReadPRESS();
                    Thread.Sleep(READ_PAUSE);
                    // Барометр 
                    barometr = ReadBAR();
                    Thread.Sleep(READ_PAUSE);
                    //Скорость
                    rate = ReadRATE();
                    Thread.Sleep(READ_PAUSE);
                    // считываем установленный режим           
                    mode = ReadMode();
                    Thread.Sleep(READ_PAUSE);

                }
            }
            catch
            {
                //Program.txtlog.WriteLineLog("Mensor: Ошибка чтения данных", 1);
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
                    while(_serialPort_M.BytesToRead > 0)
                        _serialPort_M.ReadLine();

                    activCH = ChannelRead();    // считываем номер активного канала  
                   
                    switch (activCH)
                    {
                        case 0: // Канал A                           
                            // Читаем и устанавливаем значения параметров окна A
                            ReadInitialSet(0);
                            break;

                        case 1: // Канал B                            
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
                    Console.WriteLine("Mensor: Ошибка чтения данных в потоке");
                    //activCH = -1;

                    //_serialPort_M.Close();
                    //Connected = false;
                    //Thread.Sleep(20);
                    //_serialPort_M.Open();
                    //Task.Delay(20);
                }
            }
            activCH = -1;
            //Program.txtlog.WriteLineLog("Mensor: Поток чтения данных завершен", 1);
        }


        //................................................................................
        // ОБРАБОТЧИК СРАБАТЫВАНИЯ ТАЙМЕРА
        // (1 раз в секунду - по умолчанию)       
        private void timer1_Tick(object sender, EventArgs e)
        {
            // При срабатываниие таймера, в зависимости от номера активного канала
            // обновляем элемены формы
            try
            {  //CHA
                if (_activCH == 0)
                {
                    // Меняем цвет кнопок                
                    bSetCHA.BackColor = Color.DarkGreen;
                    bSetCHB.BackColor = Color.Transparent;
                    // Делаем активными кнопки поля канала A и неактивными поля канала B                   
                    EnableCHA(true);
                    EnableCHB(false);
                 
                    // тип датчика-преобразователя               
                    if (cbRangeCHA.SelectedIndex != typeR)
                        cbRangeCHA.SelectedIndex = typeR;

                    // установленную ед. измерения 
                    if (cbUMeasCHA.SelectedIndex != umeas)
                        cbUMeasCHA.SelectedIndex = umeas;

                    // минимальный и максимальный значения диапазона, выводим их на экран           
                    lRangeMinCHA.Text = Convert.ToString(typeR_Pmin);
                    //lRangeMinCHA.Text = String.Format("{0,-6:#0.0#}", typeR_Pmin); 
                    lRangeMaxCHA.Text = Convert.ToString(typeR_Pmax);
                    //lRangeMaxCHA.Text = String.Format("{0,-6:#0.0#}", typeR_Pmax);               

                    // значение уставки и выводим на экран              
                    lPointCHA.Text = String.Format("{0,-5:0.#}", point);

                    // считываем установленный тип давления                
                    if (cbTypePressCHA.SelectedIndex != tpress)
                        cbTypePressCHA.SelectedIndex = tpress;

                    // текущее давление
                    if (press >= (point - point * 0.05))
                    {
                        lDataCHA.ForeColor = Color.SpringGreen;
                    }
                    else
                    {
                        lDataCHA.ForeColor = Color.White;
                    }
                    lDataCHA.Text = String.Format("{0,-9:#0.00000#}", press);

                    // барометр
                    lBarometerCHA.Text = String.Format("{0,-8:#0.000#}", barometr / PsiToPa);
                    // Скорость
                    lSpeedCHA.Text = String.Format("{0,-8:#0.000#}", rate);



                    // установленный режим          
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
                else if (activCH == 1)
                {
                    // Меняем цвет кнопок                
                    bSetCHA.BackColor = Color.Transparent;
                    bSetCHB.BackColor = Color.DarkSlateBlue;
                    // Делаем активными кнопки поля канала B и неактивными поля канала A                    
                    //EnableCHA(false);
                    EnableCHB(true);

                    // тип датчика-преобразователя                
                    if (cbRangeCHB.SelectedIndex != typeR)
                        cbRangeCHB.SelectedIndex = typeR;

                    // установленную ед. измерения          
                    umeas = ReadUMeas();
                    if (cbUMeasCHB.SelectedIndex != umeas)
                        cbUMeasCHB.SelectedIndex = umeas;

                    // минимальный и максимальный значения диапазона, выводим их на экран      
                    lRangeMinCHB.Text = Convert.ToString(typeR_Pmin);
                    //lRangeMinCHB.Text = String.Format("{0,-6:#0.0#}", typeR_Pmin*CalcMult(umeas));             
                    lRangeMaxCHB.Text = Convert.ToString(typeR_Pmax);
                    //lRangeMinCHB.Text = String.Format("{0,-6:#0.0#}", typeR_Pmax*CalcMult(umeas));     

                    // Считываем значение уставки и выводим на экран               
                    lPointCHB.Text = String.Format("{0,-5:0.#}", point);

                    //установленный тип давления           
                    if (cbTypePressCHB.SelectedIndex != tpress)
                        cbTypePressCHB.SelectedIndex = tpress;

                    // текущее давление,  и Скорость - выводим на экран               
                    if (press >= (point - point * 0.05))
                    {
                        lDataCHA.ForeColor = Color.SpringGreen;
                    }
                    else
                    {
                        lDataCHA.ForeColor = Color.White;
                    }
                    lDataCHB.Text = String.Format("{0,-9:#0.00000#}", press);

                    // Барометр               
                    lBarometerCHB.Text = String.Format("{0,-8:#0.000#}", barometr / PsiToPa);

                    //Скорость                
                    lSpeedCHB.Text = String.Format("{0,-8:#0.000#}", rate);
                    
                   
                    // установленный режим       
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
            catch
            {
                //Program.txtlog.WriteLineLog("Mensor: Ошибка чтения данных", 1);
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
            try
            {
                _serialPort_M.WriteLine("OUTP:CHAN " + CH);              
            }
            catch
            {                
            }
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
                //_serialPort_M.WriteLine("*IDN?");
            
                Thread.Sleep(READ_PAUSE);
                str = _serialPort_M.ReadLine();         // считываем                                                  

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

            try
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
            catch
            {
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

            try
            {
                _serialPort_M.WriteLine("OUTP:AUTOR?");  // запрашиваем состояние AutoRange (ON/OFF)
                                                         //toolStripStatusLabel1.Text = ("SEND: " + "OUTP:AUTOR?");

                Thread.Sleep(READ_PAUSE);

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

                    Thread.Sleep(READ_PAUSE);

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
                
            }
            
            catch
            {
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

            try
            {
                _serialPort_M.WriteLine("SENS:PRES:RANG:LOW?");  // запрашиваем мин. значение диапазона
                                                                 //toolStripStatusLabel1.Text = ("SEND: " + "SENS:RANG:LOW?");

                Thread.Sleep(READ_PAUSE);

                str = _serialPort_M.ReadLine();     // считываем
                val[0] = StrToDouble(str);
                //toolStripStatusLabel2.Text = ("READ: " + Convert.ToString(val[0]));
                //Thread.Sleep(pause_ms);

                _serialPort_M.WriteLine("SENS:PRES:RANG:UPP?");   // запрашиваем макс. значение диапазона
                                                                  //toolStripStatusLabel1.Text = ("SEND: " + "SENS:RANG:UPP?");

                Thread.Sleep(READ_PAUSE);

                str = _serialPort_M.ReadLine();
                val[1] = StrToDouble(str);    // считываем
                                              //toolStripStatusLabel2.Text = ("READ: " + Convert.ToString(val[1]));
            }
            catch
            {
            }

                return val;
        }
        //--------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------
        // ФУНКЦИЯ - Задает уставку Point (запись)
        // принимаемые значения:    Val - значение уставки        
        // возвращаемые значения:   нет           
        public void SetPoint(double Val)
        {
            try
            {
                UserPoint = Val;
                string str = "SOUR:PRES:LEV:IMM:AMPL " + Convert.ToString(Val).Replace(",", ".");
                _serialPort_M.WriteLine(str);
                //_serialPort_M.WriteLine("SOUR:PRES:LEV:IMM:AMPL " + Convert.ToString(Val).Replace(CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator, "."));
                //toolStripStatusLabel1.Text = ("SEND: " + "SOUR:PRES:LEV:IMM:AMPL " + Convert.ToString(Val));


            }
            catch
            {
            }
        }
        //--------------------------------------------------------------------------------


        //--------------------------------------------------------------------------------
        // ФУНКЦИЯ  Считывет заданную уставку Point (чтение)
        // принимаемые значения:    нет        
        // возвращаемые значения:   res - считанное знаение уставки (string)           
        double ReadPoint()
        {
            double res = -1;

            try
            {
                _serialPort_M.WriteLine("SOUR:PRES:LEV:IMM:AMPL?");
                //toolStripStatusLabel1.Text = ("SEND: " + "SOUR:PRES:LEV:IMM:AMPL?");

                Thread.Sleep(READ_PAUSE);

                string str = _serialPort_M.ReadLine();
                //toolStripStatusLabel2.Text = ("READ: " + str);
                res = StrToDouble(str);
            }
            catch
            {
            }
            
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
            try
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
            catch
            {
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

            try
            {
                _serialPort_M.WriteLine("UNIT:INDEX?");
                //toolStripStatusLabel1.Text = ("SEND: " + "UNIT:INDEX?");

                Thread.Sleep(READ_PAUSE);

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
            }
            catch
            {
            }
          
            return res;
        }
        //--------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------
        // ФУНКЦИЯ - Установка типа давления (запись)
        // принимаемые значения:    0 - Абсолютное (Absolute)
        //                          1 - Избыточное (Gauge)
        // возвращаемые значения:   нет                         
        public void SetTypePress(int num)
        {
            try
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
            catch
            {
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

            try
            {
                _serialPort_M.WriteLine("SENS:PRES:MODE?");     // запрашиваем установленный тип давления
                                                                //toolStripStatusLabel1.Text = ("SEND: " + "SENS:PRES:MODE?");

                Thread.Sleep(READ_PAUSE);

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
            }
            catch
            {
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
            try
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
            catch
            {
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

            try
            {
                _serialPort_M.WriteLine("OUTP:MODE?");   // Запрашиваем режим работы
                                                         //toolStripStatusLabel1.Text = ("SEND: " + "OUTP:MODE?");

                Thread.Sleep(READ_PAUSE);

                str = _serialPort_M.ReadLine();
                //toolStripStatusLabel2.Text = ("READ: " + str);

                if (str == "\"MEASURE\"")
                {
                    res = 0;
                }
                else if (str == "\"CONTROL\"")
                {
                    res = 1;
                }
                else if (str == "\"VENT\"")
                {
                    res = 2;
                }
            }
            catch
            {
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

            try
            {
                _serialPort_M.WriteLine("MEAS:PRES?");     // запрашиваем показание (не понятно диапазон R???)
                                                           //toolStripStatusLabel1.Text = ("SEND: " + "MEAS:PRES?");
                Thread.Sleep(READ_PAUSE);

                string str = _serialPort_M.ReadLine();
                res = StrToDouble(str);
                //toolStripStatusLabel2.Text = ("READ: " + Convert.ToString(res)); 
            }
            catch
            {
            }

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

            try
            {
                _serialPort_M.WriteLine("MEAS:BARO?");     // запрашиваем показание
                                                           //toolStripStatusLabel1.Text = ("SEND: " + "MEAS:BARO?");

                Thread.Sleep(READ_PAUSE);

                string str = _serialPort_M.ReadLine();
                res = StrToDouble(str);
                //toolStripStatusLabel2.Text = ("READ: " + Convert.ToString(res));
            }

            catch
            {
            }

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

            try
            {
                _serialPort_M.WriteLine("MEAS:RATE?");     // запрашиваем показание
                                                           //toolStripStatusLabel1.Text = ("SEND: " + "MEAS:RATE?");

                Thread.Sleep(READ_PAUSE);

                string str = _serialPort_M.ReadLine();
                res = StrToDouble(str);
                // toolStripStatusLabel2.Text = ("READ: " + Convert.ToString(res));
            }
            catch
            {
            }

            return res;
        }
        //--------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------
        // ФУНКЦИЯ - Установка сброса на ноль канал A
        // принимаемые значения:   нет
        // возвращаемые значения:  нет
        private void ZeroCHA_Click(object sender, EventArgs e)
        {

               timer1.Stop();
               timer1.Enabled = false;
            try
            {
                //ReadThread.Suspend();
                _serialPort_M.WriteLine("CAL:PRES:ZERO:RUN");
                //toolStripStatusLabel1.Text = ("SEND: CAL: PRES: ZERO: RUN");
            }
            finally
            {
                 timer1.Enabled = true;
                 timer1.Start();
                //ReadThread.Resume();
            }
        }

        //CHB
        private void ZeroCHB_Click(object sender, EventArgs e)
        {
            try
            {
                timer1.Stop();
                timer1.Enabled = false;
                // ReadThread.Suspend();
                _serialPort_M.WriteLine("CAL:PRES:ZERO:RUN");
                //toolStripStatusLabel1.Text = ("SEND: CAL: PRES: ZERO: RUN");
            }
            finally
            {
                //ReadThread.Resume();
                timer1.Enabled = true;
                timer1.Start();
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


        // Отключаем таймер при закрытии формы
        private void FormMensor_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Stop();
            timer1.Enabled = false;
        }

        // Включаем таймер (при открытии формы)
        public void MenStartTimer()
        {
            // Устанавливаем интервал таймера и задержку            
            timer1.Interval = READ_PERIOD;

            // запускаем таймер
            timer1.Enabled = true;
            timer1.Start();
        }

        // Останов таймера при выборе элементов из списка combobox
        private void cbRangeCHA_DropDown(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Enabled = false;
        }

        private void cbUMeasCHA_DropDown(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Enabled = false;
        }

        private void cbTypePressCHA_DropDown(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Enabled = false;
        }

        private void cbRangeCHB_DropDown(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Enabled = false;
        }

        private void cbUMeasCHB_DropDown(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Enabled = false;
        }

        private void cbTypePressCHB_DropDown(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Enabled = false;
        }
    }
}

 