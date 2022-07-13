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
    class CElemer
    {
        public bool Connected;              // флаг соединения с прибором по COM (true - есть соединение / false - нет)
        public SerialPort Port;            // переменная для работы по COM-порту
        private Thread ReadThreadElmer;    // поток
        string diagnostic = "EEPROM:1 ALU:1 M0:1 M1:1 M2:0";  // ответ прибора на команду провести диагностику используется для идентификации прибора         
        public int READ_PAUSE = 500;            // задержка между приемом и передачей команд по COM порту, мс      
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


        public CElemer()
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

                Port.PortName = PortName;
                Port.BaudRate = BaudRate;
                Port.DataBits = DataBits;
                Port.StopBits = (StopBits)StopBits;
                Port.Parity = (Parity)Parity;
                Port.ReadTimeout = 2000;
                Port.WriteTimeout = 2000;
                Port.DtrEnable = true;
                Port.RtsEnable = true;
                Port.NewLine = "\r";
                Port.Open();        // открываем порт

                if (InitDevice())   // идентифицируем подключенный прибор
                {

                    // Запускаем поток
                    ReadThreadElmer = new Thread(ElmerReadThread);
                    ReadThreadElmer.Priority = ThreadPriority.AboveNormal;
                    ReadThreadElmer.Start();
                    Thread.Sleep(1000);
                    Connected = true;
                    SetPress(0);
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
            if (ReadThreadElmer != null)
                ReadThreadElmer.Abort(0);

            if (Port.IsOpen)
            {
                Port.Close();
                return 0;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Расчет контрольной суммы LRC8
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        private int CalculateCRC(byte[] buf)
        {
            int crc = 0;
            for(int i=0; i<buf.Length;i++)
            {
                crc = crc + buf[i];
            }
            
            return (0xFF - (crc & 0xFF)) &  0xFF + 1;
        }

        // Тестирование прибора
        // Перевод прибора в режим удаленного управления (по COM), тестирование прибора
        // возвращает: true - при успешном завершении теста / false - в обратном случае
        private bool InitDevice()
        {
            int i;
            bool res = false;
            char[] buf = new char[6];
            buf[0] = ':';
            buf[1] = '1';
            buf[2] = ';';
            buf[3] = '0';
            buf[4] = ';';
            try
            {
                Port.Write(buf,0,4);
                Thread.Sleep(READ_PAUSE);
                i = 0;
                while (Port.BytesToRead > 5)
                {
                    i++;
                    Port.Read(buf,0,5);
                }
                if ((buf[2] == 105) || (buf[2] == 106))
                {

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
                    string str;
                    i = 0;
                    do
                    {
                        // устанавливаем значение давления
                        Port.WriteLine("TARGET " + Convert.ToString(Val).Replace(",", "."));// Val.ToString());
                                                                                            //Port.WriteLine("TARGET 7.25");
                                                                                            //Thread.Sleep(READ_PAUSE);  
                        str = Port.ReadLine();

                        if (str == "OK")
                        {
                            target = true;
                            break;
                        }
                        else
                        {
                            target = false;
                        }
                        i++;
                    } while (i < 3);

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
                    while ((ReadPascal) && (i < READ_PAUSE * 2))
                    {
                        Thread.Sleep(1);
                        i++;
                    }
                    ReadPascal = true;
                    while (Port.BytesToRead > 0)
                    {
                        Port.ReadLine();
                    }

                    string str;
                    i = 0;
                    do
                    {
                        // запускаем установку и поддержания давления прибором                    
                        Port.WriteLine("ON_KEY_START");
                        //Thread.Sleep(READ_PAUSE);
                        str = Port.ReadLine();

                        if (str == "START_REGULATION")
                        {
                            modeStart = true;
                            modeVent = false;
                            break;
                        }
                        else
                        {
                            if (str == "STOP_REGULATION")
                            {
                                modeStart = false;
                                break;
                            }
                            else
                            {
                                modeStart = false;
                            }
                        }
                        i++;
                    } while (i < 3);

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
                    while ((ReadPascal) && (i < READ_PAUSE * 2))
                    {
                        Thread.Sleep(1);
                        i++;
                    }
                    ReadPascal = true;
                    while (Port.BytesToRead > 0)
                    {
                        Port.ReadLine();
                    }
                    string str;
                    i = 0;
                    do
                    {
                        // запускаем вентиляцию прибора
                        Port.WriteLine("ON_KEY_VENT");
                        //Thread.Sleep(READ_PAUSE);
                        str = Port.ReadLine();

                        if (str == "VENT_ON")
                        {
                            modeVent = true;
                            modeStart = false;
                            break;
                        }
                        else if (str == "VENT_OFF")
                        {
                            modeVent = false;
                            break;
                        }
                        else
                        {
                            modeVent = false;
                        }
                        i++;
                    } while (i < 3);

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
                    while ((ReadPascal) && (i < READ_PAUSE * 2))
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
                    //Thread.Sleep(READ_PAUSE);
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
        void ElmerReadThread()
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
                    //Thread.Sleep(READ_PAUSE);
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
