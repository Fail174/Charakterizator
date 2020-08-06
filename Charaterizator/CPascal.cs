using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.Globalization;



namespace Charaterizator
{
    class CPascal
    {
        public bool Connected;              // флаг соединения с прибором по COM (true - есть соединение / false - нет)
        public SerialPort Port;            // переменная для работы по COM-порту
        private Thread ReadThreadPascal;    // поток
        string diagnostic = "EEPROM:1 ALU:1 M0:1 M1:1 M2:0";  // ответ прибора на команду провести диагностику используется для идентификации прибора         
        public int READ_PAUSE = 200;            // задержка между приемом и передачей команд по COM порту, мс      
        public double UserPoint = 0;

        public string strData;
        string[] Data;

        public bool Error = false;

        public double press { get; set; }    // текущее давление
        public int[] rangeModule { get; set; }    // текущий используемый модуль (n, m) n-внутр 1, внеш 2, m - номер модуля с единицы

        public bool target { get; set; }        // уставка задана true /не задана false
        public bool modeStart { get; set; } // текущий режим установки и регулирования давление СТАРТ(true)/СТОП(false)
        public bool modeVent { get; set; }  //  ВКЛ(true)/ОТКЛ(false) вентиляции
        public bool modeClearP { get; set; }// Обнулении показаний давления УСПЕШНО(true)/НЕ УСПЕШНО(false) 
        public bool SetModuleOK { get; set; }// Установлен заданный модуль или нет

        public List<string> ListMod = new List<string>();       // список подключенных модулей внутренныих и внешних
        public int M1num;                                       // количество внутренних модулей
        public int M2num;                                       // количество внешних модулей
        private bool ReadPascal = false;


        public CPascal()
        {
            rangeModule = new int[2];
            rangeModule[0] = 1;
            rangeModule[1] = 1;
            press = 0;
            modeStart = false;
            modeVent = false;
            modeClearP = false;
            target = false;
            SetModuleOK = false;

           Port = new SerialPort();
        }




        // Подключение прибора по СОМ
        // настройки СОМ порта
        // brate 19200 / data 8 / parity odd / stopbits 1
        public int Connect(string PortName, int BaudRate, int DataBits, int StopBits, int Parity)
        {
            if (Connected)
            {
                return 1;
            }

            try
            {
                              
              /* УБРАТЬ
                strData = "M: 1 SR: 3 LRL: -100.0 URL: 700";
                Data = strData.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                               
                rangeModule[0] = Convert.ToInt16(Data[1]);
                rangeModule[1] = Convert.ToInt16(Data[3]);*/
                       
                Port.PortName = PortName;
                Port.BaudRate = BaudRate;
                Port.DataBits = DataBits;
                Port.StopBits = (StopBits)StopBits;
                Port.Parity = (Parity)Parity;
                Port.ReadTimeout = 2000;
                Port.WriteTimeout = 2000;
                Port.DtrEnable = true;
                Port.RtsEnable = true;
                Port.NewLine = "\r\n";
                Port.Open();        // открываем порт

                if (InitDevice())   // идентифицируем подключенный прибор
                {
                    // Запускаем поток
                    ReadThreadPascal = new Thread(PascalReadThread);
                    ReadThreadPascal.Priority = ThreadPriority.AboveNormal;
                    ReadThreadPascal.Start();
                    Thread.Sleep(1000);
                    Connected = true;
                    return 0;
                }
                else
                {
                    Port.Close();
                    Connected = false;
                    return -1;
                }
            }

            catch
            {                
                DisConnect();
                return -1;
            }
        }
        


        // Отключение прибора от COM порта
        public int DisConnect()
        {
            Connected = false;
            ListMod.Clear();
            if (ReadThreadPascal != null)
                ReadThreadPascal.Abort(0);

            if (Port.IsOpen)
            {
                Port.WriteLine("LOCAL");   // Отключаем удаленное управление
                Port.Close();
                return 0;
            }
            else
            {
                return 1;
            }
        }



        // Тестирование прибора
        // Перевод прибора в режим удаленного управления (по COM), тестирование прибора
        // возвращает: true - при успешном завершении теста / false - в обратном случае
        private bool InitDevice()
        {
        string str;
        bool res = false;

            try
            {
                ///Port.Write("RESET\r\n");
                Port.WriteLine("R");               // переводим прибор в режим удаленного управления
                //Port.Write("R\r\n");
                Thread.Sleep(READ_PAUSE);
                //if (Port.BytesToRead > 0)
                //    ;
                //Port.Read();
                str = Port.ReadLine();
                if (str == "REMOTE")
                {
                    //Port.WriteLine("DIAGNOSTIC");  // выполняем диагностику прибора
                    //if (str == diagnostic)
                    //{
                    //}
                    // запрашиваем список подключенных модулей
                    Port.WriteLine("SEEK_MODUL");
                    Thread.Sleep(READ_PAUSE);
                    str = Port.ReadLine();

                    // ожидаем три секунды для сбора информации
                    Thread.Sleep(3000);

                    // запрашиваем информацию о внутренних модулях
                    Port.WriteLine("READ_M1?");
                    Thread.Sleep(READ_PAUSE);
                    str = Port.ReadLine();
                                      
                    str = str.Substring(2); //
                    str = str.Replace("[", "Внутр.модуль: ");
                    string[] M1 = str.Split(new char[] { ']' }, StringSplitOptions.RemoveEmptyEntries);
                    M1num = M1.Length;
                    ListMod.AddRange(M1);

                    // запрашиваем информацию о внешних модулях
                    Port.WriteLine("READ_M2?");
                    Thread.Sleep(READ_PAUSE);
                    str = Port.ReadLine();

                    str = str.Substring(2); //
                    str = str.Replace(" [", "Внеш.модуль: ");
                    str = str.Remove(str.Length - 1, 1);
                    //str = str.Substring(str.Length-1); //
                    string[] M2 = str.Split(new char[] { ']' }, StringSplitOptions.RemoveEmptyEntries);
                    M2num = M2.Length;
                    ListMod.AddRange(M2);

                    rangeModule[0] = 1;
                    rangeModule[1] = 1;

                    res = true;
                }
            }
            catch
            {
                M1num = -1;
                M2num = -1;
                ListMod.Clear();
            }          

        return res;
        }



        // Устанавливает текущий модуль
        // входныет данные (n, m)
        // n - 1 внутр, 2 - внешний модуль
        // m - порядковый номер модуля по списку
        public void SetModule(int n, int m)
        {
            if (Port.IsOpen)
            {
                try
                {
                    int i = 0;
                    while ((ReadPascal) && (i < READ_PAUSE))
                    {
                        Thread.Sleep(1);
                        i++;
                    }
                    ReadPascal = true;
                    
                    while (Port.BytesToRead > 0)
                    {
                        Port.ReadLine();
                    }

                    Port.WriteLine("RANGE " + n.ToString() + "," + m.ToString());
                    //Thread.Sleep(READ_PAUSE);
                    string str = Port.ReadLine();

                    if (str == "OK")
                    {
                        SetModuleOK = true;
                    }
                    else
                    {
                        SetModuleOK = false;
                    }

                }
                catch
                {
                    SetModuleOK = false;
                }
                finally
                {
                    ReadPascal = false;
                }

            }

        }





        // Задает уставку давления 
        // возвращаемые значения:   нет          
        public void SetPress(double Val)
        {
            if (Port.IsOpen)
            {
                try
                {
                    int i = 0;
                    while ((ReadPascal) && (i < READ_PAUSE))
                    {
                        Thread.Sleep(1);
                        i++;
                    }
                    ReadPascal = true;
                    while (Port.BytesToRead > 0)
                    {
                        Port.ReadLine();
                    }
                    UserPoint = Val;
                    // устанавливаем значение давления
                    Port.WriteLine("TARGET " + Val.ToString());
                    Thread.Sleep(READ_PAUSE);  
                    string str = Port.ReadLine();

                    if (str == "OK")
                    {
                        target = true;
                    }
                    else
                    {
                        target = false;
                    }

                }
                catch
                {
                    target = false;
                }
                finally
                {
                    ReadPascal = false;
                }

            }         

        }



        // Задает режим установки и регулирования давления: ПУСК / СТОП 
        // записывает в переменную modeStart установленный режим 1 - Пуск / 0 - стоп          
        public void SetModeKeyStart()
        {
            if (Port.IsOpen)
            {
                try
                {
                    int i = 0;
                    while ((ReadPascal) && (i < READ_PAUSE))
                    {
                        Thread.Sleep(1);
                        i++;
                    }
                    ReadPascal = true;
                    while (Port.BytesToRead > 0)
                    {
                        Port.ReadLine();
                    }
                    // запускаем установку и поддержания давления прибором                    
                    Port.WriteLine("ON_KEY_START");
                    Thread.Sleep(READ_PAUSE);
                    string str = Port.ReadLine();

                    if (str == "START_REGULATION")
                    {
                        modeStart = true;
                    }
                    else if (str == "STOP_REGULATION")
                    {
                        modeStart = false;
                    }
                    else
                    {
                        modeStart = false;
                    }
                    
                }
                catch
                {
                    modeStart = false;
                }
                finally
                {
                    ReadPascal = false;
                }
            }
        }



        // Задает режим ВЕНТИЛЯЦИИ: ВКЛ / ВЫКЛ 
        // записывает в переменную modeVent установленный режим 1 - ВКЛ / 0 - ВЫКЛ          
        public void SetModeVent()
        {
            if (Port.IsOpen)
            {
                try
                {
                    int i = 0;
                    while ((ReadPascal) && (i < READ_PAUSE))
                    {
                        Thread.Sleep(1);
                        i++;
                    }
                    ReadPascal = true;
                    while (Port.BytesToRead > 0)
                    {
                        Port.ReadLine();
                    }
                    // запускаем вентиляцию прибора
                    Port.WriteLine("ON_KEY_VENT");
                    Thread.Sleep(READ_PAUSE);
                    string str = Port.ReadLine();

                    if (str == "VENT_ON")
                    {
                        modeVent = true;
                    }
                    else if (str == "VENT_OFF")
                    {
                        modeVent = false;
                    }
                    else
                    {
                        modeVent = false;
                    }

                }
                catch
                {
                    modeVent = false;
                    
                }
                finally
                {
                    ReadPascal = false;
                }

            }
        }



        // Обнуление показаний давления в текущем модуле/диапазоне              
        public void SetClearP()
        {
            if (Port.IsOpen)
            {
                try
                {
                    int i = 0;
                    while ((ReadPascal) && (i < READ_PAUSE))
                    {
                        Thread.Sleep(1);
                        i++;
                    }
                    ReadPascal = true;

                    while (Port.BytesToRead > 0)
                    {
                        Port.ReadLine();
                    }
                    // запускаем вентиляцию прибора
                    Port.WriteLine("CLEAR_P");
                    Thread.Sleep(READ_PAUSE);
                    string str = Port.ReadLine();

                    if (str == "OK")
                    {
                        modeClearP = true;
                    }
                    else 
                    {
                        modeClearP = false;
                    }                

                }
                catch
                {
                    modeClearP = false;
                }
                finally
                {
                    ReadPascal = false;
                }

            }
        }





        //-----------------------------------------------------------------------------------------------

        // Функция периодического чтения параметров прибора в потоке
        void PascalReadThread()
        {
            int i;

            while (Port.IsOpen)
            {
                i = 0;
                while ((ReadPascal) && (i < READ_PAUSE))
                {
                    Thread.Sleep(1);
                    i++;
                }
                try
                {
                    ReadPascal = true;
                   
                    while (Port.BytesToRead > 0)
                    {
                        Port.ReadByte();
                        //Port.ReadLine();
                    }

                    i = 0;
                    // считываем текущее давление
                    //Thread.Sleep(READ_PAUSE);
                    Port.WriteLine("PRES?");
                    while ((Port.BytesToRead <= 0) && (i < READ_PAUSE))
                    {
                        i++;
                        Thread.Sleep(1);
                    }
                    if (Port.BytesToRead > 0)
                    {
                        strData = Port.ReadLine();
                        press = float.Parse(strData.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        continue;
                    }

                    // считываем текущий используемый модуль
                    Thread.Sleep(READ_PAUSE);
                    Port.WriteLine("RANGE?");
                    //Thread.Sleep(READ_PAUSE);
                    i = 0;
                    while ((Port.BytesToRead <= 0) && (i < READ_PAUSE))
                    {
                        i++;
                        Thread.Sleep(1);
                    }
                    if (Port.BytesToRead > 0)
                    {
                        strData = Port.ReadLine();
                        Data = strData.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        rangeModule[0] = Convert.ToInt16(Data[1]);
                        rangeModule[1] = Convert.ToInt16(Data[3]);
                    }
                }
                catch
                {
                    /*rangeModule[0] = -1;
                    rangeModule[1] = -1;
                    press = -1;*/
                    //Port.Close();
                    Program.txtlog.WriteLineLog("Pascal: Ошибка чтения в потоке", 1);
                    Error = true;
                }
                finally
                {
                    ReadPascal = false;
                    Thread.Sleep(500);
                }
            }
        }







    }
}
