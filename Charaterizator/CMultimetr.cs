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
    class CMultimetr
    {
        public double REZISTOR = 500;      //Сопротивление нагрузочного резистора, Ом
        public int WAIT_READY = 300;    //время ожидания стабилизации тока, мсек
        public int WAIT_TIMEOUT = 300;  //таймаут ожидания ответа от мультиметра, мсек
//        public int READ_COUNT = 20;   //количество опросов мультиметра, раз
        public int READ_PERIOD = 100;  //период опроса мультиметра, мсек
        public int SAMPLE_COUNT = 30;   //количество отчетов при усреднении

        public bool Connected;
        private float Value;//Напряжение в мВ
        public float Current//ток в мА
        {
            get { return Convert.ToSingle(Value*1000/REZISTOR); }
            set { }
        }


        private SerialPort Port;
        private Thread ReadThread;          // поток
        public bool Error = false;

        public CMultimetr()
        {
            Port = new SerialPort();
            Value = 0;
        }

        public int DisConnect()
        {
            Connected = false;
            if (ReadThread != null)
                ReadThread.Abort(0);

            if (Port.IsOpen)
            {
                Port.WriteLine("*RST");
                Port.Close();
                return 0;
            }
            else
            {
                return 1;
            }
        }
        public int Connect(string PortName, int BaudRate, int DataBits, int StopBits, int Parity)
        {
            if (Connected)
            {
                //DisConnect();
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
                Port.Open();
                Connected = true;
                InitDevice();
                if (ConnectionTest() )
                {
                    // Запускаем поток
                    Error = false;
                    ReadThread = new Thread(MultimetrReadThread);
                    ReadThread.Priority = ThreadPriority.AboveNormal;
                    ReadThread.Start();
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
                //                Connected = false;
                DisConnect();
                return -1;
            }
        }
        public int InitDevice()
        {
            if (Connected)
            {
                Port.WriteLine("*RST");
                Thread.Sleep(READ_PERIOD);
                Port.WriteLine("SYST:REM");
                Thread.Sleep(READ_PERIOD);
                return 0;
            }
            else
            {
                return -1;
            }
        }


        void MultimetrReadThread()
        {
            while (Port.IsOpen)
            {
                try
                {
                    while (Port.BytesToRead > 0)
                    {
                        Port.ReadLine();
                    }
                    //Thread.Sleep(1);

                    if (ReadData())
                    {
                        Error = false;
                    }
                    else
                    {
                        Error = true;
                    }
                }
                catch
                {
                    //Console.WriteLine("Multimetr: Ошибка чтения данных");
                    Program.txtlog.WriteLineLog("Agilent: Ошибка выполнения потока", 1);
                    Error = true;
                }
            }
        }

        public bool ReadData1()
        {
            if (Connected)
            {
                try
                {
                   // Thread.Sleep(WAIT_READY);
                    Port.WriteLine("CONF:VOLT:DC 10, 0.0001");
                    Thread.Sleep(10);
                    Port.WriteLine("SAMP:COUN " + SAMPLE_COUNT.ToString());
                    Thread.Sleep(10);
                    Port.WriteLine("CALC:FUNC AVER");
                    Thread.Sleep(10);
                    Port.WriteLine("CALC:STAT ON");
                    Thread.Sleep(10);
                    Port.WriteLine("INIT");
                    //Port.WriteLine("* WAI");
                    Thread.Sleep(READ_PERIOD);
                    Port.WriteLine("CALC:AVER:AVER?");
                    int i = 0;
                    while ((Port.BytesToRead <= 0) && (i < WAIT_TIMEOUT))
                    {
                            i++;
                            Thread.Sleep(1);
                    }
                    string str = Port.ReadLine();
                    Value = float.Parse(str.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                    //Program.txtlog.WriteLineLog(string.Format("Agilent: Произведено измерение напряжения мультиметра {0} ", Value), 0);
                    return true;
                }
                catch
                {
                    //запись в лог
                    //Program.txtlog.WriteLineLog("Agilent: Ошибка чтения данных. ", 1);
                    //PortClear();//отчищаем буферы порта
                    Port.Close();
                    Thread.Sleep(1);
                    Port.Open();
                    Value = 0;
                    return false;
                }
            }
            else
            {
                Value = 0;
                return false;
            }
        }
        public bool ConnectionTest()
        {
            if (Connected)
            {
                try
                {
                        Thread.Sleep(WAIT_TIMEOUT);
                        Port.WriteLine("MEAS:VOLT:DC? 10, 0.00001");
                        int i = 0;
                        while ((Port.BytesToRead <= 0) && (i < WAIT_TIMEOUT))
                        {
                            i++;
                            Thread.Sleep(1);
                        }
                        if (Port.BytesToRead > 0)
                        {
                            string str = Port.ReadLine();
                            Value = float.Parse(str.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            Program.txtlog.WriteLineLog("Agilent: Отсутсвуют данные для чтения.", 1);
                            return false;
                        }
                    return true;
                }
                catch
                {
                    //запись в лог
                    Program.txtlog.WriteLineLog("Agilent: Ошибка чтения данных.", 1);
                    Port.Close();
                    Thread.Sleep(1);
                    Port.Open();
                    Value = 0;
                    return false;
                }
            }
            else
            {
                Value = 0;
                return false;
            }

        }

        //Читает напряжение в мВ
        public bool ReadData()
        {
            if (Connected)
            {
                try
                {
                    Thread.Sleep(WAIT_TIMEOUT);
                    float Mean =0;
                    float Min = 100000, Max = -100000;
                    for (int c = 0; c < SAMPLE_COUNT; c++)
                    {
                        Port.WriteLine("MEAS:VOLT:DC? 10, 0.00001");
                        int i = 0;
                        while ((Port.BytesToRead <= 0) && (i < WAIT_TIMEOUT))
                        {
                            i++;
                            Thread.Sleep(1);
                        }
                        if (Port.BytesToRead > 0)
                        {
                            string str = Port.ReadLine();
                            Value = float.Parse(str.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                            Mean = Value + Mean;
                            if (Max < Value) Max = Value;
                            if (Min > Value) Min = Value;
                            //Thread.Sleep(READ_PERIOD);
                        }
                        else
                        {
                            Program.txtlog.WriteLineLog("Agilent: Отсутсвуют данные для чтения.", 1);
                            return false;
                        }
                    }
                    if (SAMPLE_COUNT >= 3)
                    {
                        Value = (Mean - Max - Min)/ (SAMPLE_COUNT-2);//усредняем
                    }
                    else
                    {
                        Value = Mean / SAMPLE_COUNT;//усредняем
                    }
                    return true;
                }
                catch
                {
                    //запись в лог
                    Program.txtlog.WriteLineLog("Agilent: Ошибка чтения данных.", 1);
                    Port.Close();
                    Thread.Sleep(1);
                    Port.Open();
                    Value = 0;
                    return false;
                }
            }
            else
            {
                Value = 0;
                return false;
            }
        }
    }
}
