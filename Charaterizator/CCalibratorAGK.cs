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
    class CCalibratorAGK
    {
        // Переменные
        public bool Connected;          // флаг соединения с прибором по COM (true - есть соединение / false - нет)
        public SerialPort Port;         // переменная для работы по COM-порту

        private Thread ReadThreadAGK;   // поток для периодического чтения данных с калибратора
        private volatile bool readAGK;  // true - чтение данных, прибор занят

        public int READ_PAUSE = 500;    // количество циклов (по 1мс) ожидания ответа прибора на отправленнную команду по COM порту   
        public string strData;          // переменная буфер для хранения ответа прибора по COM

        public List<string> ListMod = new List<string>();       // список подключенных модулей калибратора

        public double UserPoint;        // уставка (заданная пользователем) 
        public double press;            // текущее давление
        public int typePress;           // тип давления (0-АБС. или 1-ИЗБ.)
        public int mode;                // режим работы (0-ИЗМЕРЕНИЕ, 1-ЗАДАЧА, 2-СБРОС, 3-СТОП)
        public bool errors;             // errors=false - если данные прочитаны с прибора по СОМ успешно




        //---------------------------------------------------------------------------------------------------
        // Конструктор класса
        //---------------------------------------------------------------------------------------------------
        public CCalibratorAGK()
        {
            UserPoint = 0;       
            press = 0;
            typePress = 0;
            mode = 0;
            errors = true;
            readAGK = false;
            Connected = false;
            ListMod.Clear();
            Port = new SerialPort();            
        }



        //---------------------------------------------------------------------------------------------------
        // Подключение, отключение и тестировани прибора 
        //---------------------------------------------------------------------------------------------------
        // Подключение
        public int Connect(string PortName, int BaudRate, int DataBits, int StopBits, int Parity)
        {
            if (Connected)
                return 1;

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
                Port.NewLine = "\r\n";
                Port.Open();        

                if (InitDevice())   // идентифицируем подключенный прибор
                {
                    // Запускаем поток
                    ReadThreadAGK = new Thread(ReadDataThreadAGK);
                    ReadThreadAGK.Priority = ThreadPriority.AboveNormal;
                    ReadThreadAGK.Start();
                    Thread.Sleep(2000);                   
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
            if (ReadThreadAGK != null)
                ReadThreadAGK.Abort(0);

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


        // Тестирование прибора        
        // возвращает: true - при успешном завершении теста / false - в обратном случае
        private bool InitDevice()
        {
            int i;
            bool result = false;
            bool waitAnswer;


            //Задаем тип внешних команд по COM - как от Mensor
            waitAnswer = false;
            sendCommand("CMDSET 1", waitAnswer);     //  CMDSET C (C=0 — команды Alfapascal C = 1 — команды Mensor)
            Thread.Sleep(50);
                                               
            // Запрашиваем ID калибратора 
            waitAnswer = true;
            strData = sendCommand("ID?", waitAnswer);
            if (!string.IsNullOrEmpty(strData))
            {
                result = true; // если есть ответ от прибора
            }
            else
            {
                result = false;
                return result;
            }
            
            // устанавливаем единицы измерения KPA — кПа
            Thread.Sleep(50);
            waitAnswer = false;
            strData = sendCommand("UNITS KPA", waitAnswer);
                       

            // запрашиваем список модулей калибратора
            Thread.Sleep(50);
            waitAnswer = true;
            strData = sendCommand("SENSOR?", waitAnswer);
            // ответ нужно распарсить и поместить списком во внутреннюю переменную ListMod и cmbx cbMensorTypeR
            // пока помещаем в ListMod = SENSOR 0 - это означает автовыбор модуля калибратора
            ListMod.Add("SENSOR 0");

            return result;
        }



        //---------------------------------------------------------------------------------------------------
        // Управление прибором
        //---------------------------------------------------------------------------------------------------

        // ФУНКЦИЯ - Установка типа давления (запись)
        // принимаемые значения:    0 - Абсолютное (Absolute)
        //                          1 - Избыточное (Gauge)
        // возвращаемые значения:   нет                         
        public void SetTypePress(int num)
        {
            bool waitAnswer = false;
            switch (num)
            {
                case 0:
                    sendCommand("PTYPE ABSOLUTE", waitAnswer);
                    break;
                case 1:
                    sendCommand("PTYPE GAUGE", waitAnswer);
                    break;
            }
        }


        // ФУНКЦИЯ - Задает уставку Point (запись)
        // принимаемые значения:    Val - значение уставки (с плавающей точкой в кПА)       
        // возвращаемые значения:   нет     
        public void SetPoint(double Val)
        {
            UserPoint = Val; // помещаем заданную пользователем уставку во внутреннюю переменную
            string command_str = "SETPT " + Convert.ToString(Val).Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);  //Replace(",", ".");           

            bool waitAnswer = false;
            sendCommand(command_str, waitAnswer);
        }


        // ФУНКЦИЯ - Задает режим работы: ИЗМЕРЕНИЕ, ЗАДАЧА, СБРОС (запись)
        // принимаемые значения:    0 - ИЗМЕРЕНИЕ
        //                          1 - ЗАДАЧА
        //                          2 - СБРОС
        //                          3 - СТОП
        // возвращаемые значения:   нет          
        public void SetMode(int num)
        {
            bool waitAnswer = false;
            switch (num)
            {
                case 0:
                    sendCommand("STANDBY", waitAnswer); // стоп прибора
                    Thread.Sleep(500);
                    sendCommand("MEASURE", waitAnswer);
                    break;
                case 1:
                    sendCommand("STANDBY", waitAnswer); // стоп прибора
                    Thread.Sleep(500);
                    sendCommand("CONTROL", waitAnswer);
                    break;
                case 2:
                    sendCommand("STANDBY", waitAnswer); // стоп прибора
                    Thread.Sleep(500);
                    sendCommand("VENT", waitAnswer);
                    break;
                case 3:
                    sendCommand("STANDBY", waitAnswer); // стоп прибора                   
                    break;
                default:
                    break;
            }
        }


        //--------------------------------------------------------------------------------------------------------
        // Функция отправляет команду содержащуюся в "command" по ком-порту 
        // если "waitAnswer" = true то ожидает и вовзращает ответ (если ответа нет в течении READ_PAUSE*1мс, то возвращает "")
        string sendCommand(string command, bool waitAnswer)
        {
            string answer = "";
            int i = 0;

            // если порт не подключен, то выходим из функции и возвращаем ""
            if (!Connected)
                return answer;

            // отправка команды
            try
            {                
                while ((readAGK) && (i < READ_PAUSE))
                {
                    Thread.Sleep(1);
                    i++;
                }
                readAGK = true;
                
                // очистка буфера
                while (Port.BytesToRead > 0)
                {
                    Port.ReadLine();
                }

                // отправляем команду
                Port.WriteLine(command);
                               
                if (waitAnswer)
                {
                    i = 0;
                    // ожидание ответа
                    while ((Port.BytesToRead <= 0) && (i < READ_PAUSE))
                    {
                        i++;
                        Thread.Sleep(1);
                    }
                    // если получили ответ то возвращаем true
                    if (Port.BytesToRead > 0)
                    {
                        answer = Port.ReadLine();
                    }
                }
            }
            catch
            {               
                Console.WriteLine("Ошибка при обмене данных с калибратором АГК");
            }
            finally
            {
                readAGK = false;
                Console.WriteLine("команда: " + command + "  ответ: " + answer);
            }
            
            return answer;   
        }
        //--------------------------------------------------------------------------------------------------------




        //---------------------------------------------------------------------------------------------------
        // Чтение данных с прибора 
        //---------------------------------------------------------------------------------------------------

        // Функция периодического чтения параметров прибора в потоке
        void ReadDataThreadAGK()
        {
            bool waitAnswer;
            errors = false;
            //int i;

            while (Port.IsOpen)
            {              
                try
                {                                         
                    // считываем текущее давление  
                    waitAnswer = true;
                    strData = sendCommand("A?", waitAnswer);

                    if (string.IsNullOrEmpty(strData))
                    {
                        press = -1;                       
                        errors = true;
                        return;
                    }
                    else
                    {                     
                        // Заменяем запятую на разделитель десятичной части InvariantCulture (точка)
                        strData = strData.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
                        if (double.TryParse(strData, out press))   //TryParse(strData, NumberStyles.Float, CultureInfo.InvariantCulture, out press))
                        {
                            // Преобразование успешно
                        }
                        else
                        {
                            press = -1;                          
                            errors = true;
                            return;
                        }
                    }

                    // считываем режим работы (0-ИЗМЕРЕНИЕ, 1-ЗАДАЧА, 2-СБРОС, 3-СТОП)
                    strData = sendCommand("MODE?", waitAnswer);

                    if (string.IsNullOrEmpty(strData))
                    {
                        mode = -1;                       
                        errors = true;
                        return;
                    }
                    else
                    {
                        if (strData == "MEASURE")
                        {
                            mode = 0;
                        }
                        else if (strData == "CONTROL")
                        {
                            mode = 1;
                        }
                        else if (strData == "VENT")
                        {
                            mode = 2;
                        }
                        else if (strData == "STANDBY")
                        {
                            mode = 3;
                        }
                        else
                        {
                            mode = -1;                           
                            errors = true;
                            return;
                        }
                    }


                    // считываем тип давления (0-АБС. или 1-ИЗБ.)
                    strData = sendCommand("PTYPE?", waitAnswer);

                    if (string.IsNullOrEmpty(strData))
                    {
                        typePress = -1;
                        errors = true;
                        return;
                    }
                    else
                    {
                        if (strData == "ABSOLUTE")
                        {
                            typePress = 0;
                        }
                        else if (strData == "GAUGE")
                        {
                            typePress = 1;
                        }
                        else
                        {
                            typePress = -1;                           
                            errors = true;
                            return;
                        }
                    }

                    // считываем уставку 
                    strData = sendCommand("SETPOINT?", waitAnswer);

                    if (string.IsNullOrEmpty(strData))
                    {
                        UserPoint = 0;                     
                        errors = true;
                        return;
                    }
                    else
                    {
                        strData = strData.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
                        if (Double.TryParse(strData, out UserPoint))
                        {
                            // Преобразование успешно
                        }
                        else
                        {
                            UserPoint = 0;                      
                            errors = true;
                            return;
                        }
                    }

                }
                catch
                {                   
                    Program.txtlog.WriteLineLog("Калибратор АГК: Ошибка чтения в потоке", 1);
                    errors = true;
                }
                finally
                {                  
                    Thread.Sleep(500);
                }
            }
        }
    }
}
