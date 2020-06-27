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
        private SerialPort Port = new SerialPort();            // переменная для работы по COM-порту
        private Thread ReadThreadPascal;    // поток
        string diagnostic = "EEPROM:1 ALU:1 M0:1 M1:1 M2:0";  // ответ прибора на команду провести диагностику используется для идентификации прибора         
        public int READ_PAUSE = 50;            // задержка между приемом и передачей команд по COM порту, мс      


        public bool Error = false;
        public double press{ get; set; }    // текущее давление
        public bool modeStart { get; set; } // текущий режим установки и регулирования давление СТАРТ(true)/СТОП(false)
        public bool modeVent { get; set; }  //  ВКЛ(true)/ОТКЛ(false) вентиляции
        public bool modeClearP { get; set; }// Обнулении показаний давления УСПЕШНО(true)/НЕ УСПЕШНО(false) вентиляции



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
                Port.PortName = PortName;
                Port.BaudRate = BaudRate;
                Port.DataBits = DataBits;
                Port.StopBits = (StopBits)StopBits;
                Port.Parity = (Parity)Parity;
                Port.ReadTimeout = 2000;
                Port.WriteTimeout = 2000;
                Port.DtrEnable = true;
                Port.RtsEnable = true;
                Port.Open();        // открываем порт
                Connected = true;             

                if (InitDevice())   // идентифицируем подключенный прибор
                {
                    // Запускаем поток
                    ReadThreadPascal = new Thread(PascalReadThread);
                    ReadThreadPascal.Priority = ThreadPriority.AboveNormal;
                    ReadThreadPascal.Start();
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
        bool res = false;

            try
            {
                Port.WriteLine("R");               // переводим прибор в режим удаленного управления
                Thread.Sleep(READ_PAUSE);
                string str = Port.ReadLine();
                if (str == "REMOTE")
                {
                    //Port.WriteLine("DIAGNOSTIC");  // выполняем диагностику прибора
                    //if (str == diagnostic)
                    //{
                    //}
                    res = true;
                }
            }
            catch
            {
            }          

        return res;
        }


       
        // Задает уставку давления 
        // возвращаемые значения:   нет          
        public void SetPress(double num)
        {
            if (Port.IsOpen)
            {
                try
                {
                    // устанавливаем значение давления
                    Port.WriteLine("TARGET<" + num + ">");
                    Thread.Sleep(READ_PAUSE);  
                    string str = Port.ReadLine();
                }
                catch
                {
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
                    // запускаем установку и поддержания давления прибором                    
                    Port.WriteLine("ON_KEY_START");
                    Thread.Sleep(READ_PAUSE);
                    string str = Port.ReadLine();

                    if (str == "START_REGULATION")
                    {
                        modeStart = true;
                    }
                    else if (str == "START_REGULATION")
                    {
                        modeStart = false;
                    }
                    else
                    { }
                    
                }
                catch
                {
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
                    { }

                }
                catch
                {
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
                }

            }
        }





        //-----------------------------------------------------------------------------------------------

        // Функция периодического чтения параметров прибора в потоке
        void PascalReadThread()
        {
            while (Port.IsOpen)
            {
                try
                {
                    while (Port.BytesToRead > 0)
                    {
                        Port.ReadLine();
                    }

                    // считываем текущее давление
                    Port.WriteLine("PRES?");
                    Thread.Sleep(READ_PAUSE);
                    string str = Port.ReadLine();
                    press = float.Parse(str.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);

                }
                catch
                {
                   
                    Program.txtlog.WriteLineLog("Pascal: Ошибка чтения в потоке", 1);
                    Error = true;
                }
            }
        }







    }
}
