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
    class CBarometr
    {
        public bool Connected;              // флаг соединения с прибором по COM (true - есть соединение / false - нет)
        public SerialPort Port;             // переменная для работы по COM-порту       
        public int READ_PAUSE = 1000;       // интервал опроса по COM порту, мс  
        public string strData;
        public double amtPress { get; set; }    // текущее амтосфероное давление
        private Thread ReadThreadBar;           // поток
        private bool ReadBar = false;
        public bool Error = false;

        public CBarometr()
        {
            amtPress = -1;
            Port = new SerialPort();
        }



        // Подключение прибора по СОМ, Настройки СОМ порта:
        // brate 1200 / data 8 / parity none / stopbits 1
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
                //Port.NewLine = "\r\n";

                Port.Open();        // открываем порт

                //if (InitDevice())   // идентифицируем подключенный прибор
                if (Port.IsOpen)   // идентифицируем подключенный прибор
                {
                    // Запускаем поток
                    ReadThreadBar = new Thread(BarReadThread);
                    ReadThreadBar.Priority = ThreadPriority.Normal;
                    ReadThreadBar.Start();
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
          
            if (ReadThreadBar != null)
                ReadThreadBar.Abort(0);

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

               

        // Функция периодического чтения параметров прибора в потоке
        void BarReadThread()
        {
            int i;

            while (Port.IsOpen)
            {               
                try
                {                   

                    i = 0;
                    // считываем текущее атмосферное давление              
                    while ((Port.BytesToRead <= 0) && (i < READ_PAUSE))
                    {
                        i++;
                        Thread.Sleep(1);
                    }
                    if (Port.BytesToRead > 0)
                    {
                        strData = Port.ReadLine();
                        amtPress = float.Parse(strData.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        continue;
                    }

                   
                }
                catch
                {
                   
                    Program.txtlog.WriteLineLog("Barometr: Ошибка чтения в потоке", 1);
                    Error = true;
                }
                finally
                {
                    //ReadBar = false;
                    Thread.Sleep(500);
                }
            }
        }




















    }
}
