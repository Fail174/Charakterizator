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
        public int REZISTOR = 500;      //Сопротивление нагрузочного резистора, Ом

        public int WAIT_READY = 100;    //время ожидания стабилизации тока, мсек
        public int WAIT_TIMEOUT = 100;  //таймаут ожидания ответа от мультиметра, мсек
        public int READ_COUNT = 1;      //количество опросов мультиметра, раз
        public int READ_PERIOD = 1000;   //период опроса мультиметра, мсек

        public bool Connected;
        private float Value;//Напряжение в мВ
        public float Current//ток в мА
        {
            get { return Value*1000/REZISTOR; }
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
            if (Connected)
            {
                Connected = false;
                Port.Close();
                if (ReadThread != null)
                    ReadThread.Abort(0);
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
                if (ReadData() )
                {
                    // Запускаем поток
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
                Connected = false;
                return -1;
            }
        }
        public int InitDevice()
        {
            if (Connected)
            {
                Thread.Sleep(WAIT_READY);
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
                    ReadData();
                }
                catch
                {
                    Console.WriteLine("Multimetr: Ошибка чтения данных");
                    Error = true;
                }
            }
        }

        public bool ReadData()
        {
            if (Connected)
            {
                try
                {
                    Thread.Sleep(WAIT_READY);
                    Port.WriteLine("CONF:VOLT:DC 10, 0.0001");
                    Thread.Sleep(1);
                    Port.WriteLine("SAMP:COUN 100");
                    Thread.Sleep(1);
                    Port.WriteLine("CALC:FUNC AVER");
                    Thread.Sleep(1);
                    Port.WriteLine("CALC:STAT ON");
                    Thread.Sleep(1);
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
                    Error = false;
                    return true;
                }
                catch
                {
                    //запись в лог
                    //Program.txtlog.WriteLineLog("Agilent: Ошибка чтения данных. ", 1);
                    Port.Close();
                    Thread.Sleep(1);
                    Port.Open();
                    Value = 0;
                    Error = true;
                    return false;
                }
            }
            else
            {
                Value = 0;
                Error = true;
                return false;
            }
        }

        //Читает напряжение в мВ
        public bool ReadCurrentData()
        {
            if (Connected)
            {
                try
                {
                    //Port.WriteLine("CONF:VOLT:DC 10, 0.01");
                    //Port.WriteLine("READ?");
                    Thread.Sleep(WAIT_READY);
                    float Mean =0;
                    for (int c = 0; c < READ_COUNT; c++)
                    {
                        //Port.WriteLine("CALC:AVER:AVER?");
                        Port.WriteLine("MEAS:VOLT:DC? 10, 0.01");
                        int i = 0;
                        while ((Port.BytesToRead <= 0) && (i < WAIT_TIMEOUT))
                        {
                            i++;
                            Thread.Sleep(1);
                        }
                        string str = Port.ReadLine();
                        // str = str.Replace(".","");
                        Value = float.Parse(str.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                        Mean = Value + Mean;
                        Thread.Sleep(READ_PERIOD);
                    }
                    Value = Mean / READ_COUNT;//усредняем
                    //Program.txtlog.WriteLineLog(string.Format("Agilent: Произведено измерение напряжения мультиметра {0} ", Value), 0);
                    return true;
                }
                catch
                {
                    //запись в лог
                    Program.txtlog.WriteLineLog("Agilent: Ошибка чтения данных. ", 1);
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
