using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace Charaterizator
{
    class ClassEni201
    {
        public int REZISTOR = 500;      //Сопротивление нагрузочного резистора, Ом
        public int WAIT_READY = 300;    //время ожидания стабилизации тока, мсек
        public int WAIT_TIMEOUT = 300;  //таймаут ожидания ответа от мультиметра, мсек
                                        //        public int READ_COUNT = 20;      //количество опросов мультиметра, раз
        public int READ_PERIOD = 1000;   //период опроса мультиметра, мсек
        public int SAMPLE_COUNT = 30;   //количество отчетов при усреднении

        public bool Connected;
        string version="";
        private float Value;//Напряжение в мВ

        public float Current//ток в мА
        {
            get { return Value * 1000 / REZISTOR; }
            set { }
        }


        private SerialPort Port;
        private Thread ReadThread;          // поток
        public bool Error = false;

        public ClassEni201()
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
                if(InitDevice()==0)
                //if (ConnectionTest())
                {
                    // Запускаем поток
                    Error = false;
                    ReadThread = new Thread(Eni201ReadThread);
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
        public byte GetCRC(byte[] data, int start)
        {
            byte res = 0;
            for (int i = start; i < data.Length - 1; i++)
            {
                res = (byte)(res ^ data[i]);
            }
            return res;
        }
        public int InitDevice()
        {

            if ((Port != null) && (Port.IsOpen))
            {
                int i=0;
                byte[] data = new byte[12];
                data[0] = 0xCA;
                data[1] = 0x35;
                data[9] = 0x20;
                data[10] = 0x01;
                data[11] = GetCRC(data, 0);//CRC
                Port.Write(data, 0, data.Length);
                Thread.Sleep(WAIT_READY);
                byte[] input = new byte[12];
                int c = Port.Read(input,0,12);
                if ((c == 12) && (input[0]==0xCA) && (input[1] == 0x53))
                {
                    version = input[4].ToString() + input[5].ToString()+ input[6].ToString()+ input[7].ToString();
                    return 0;
                }
            }
            return -1;
        }


        void Eni201ReadThread()
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
                    Program.txtlog.WriteLineLog("Eni201: Ошибка выполнения потока", 1);
                    Error = true;
                }
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
                    float Mean = 0;
                    float Min = 100000, Max = -100000;
                    for (int c = 0; c < SAMPLE_COUNT; c++)
                    {
                        byte[] data = new byte[12];
                        data[0] = 0xCA;
                        data[1] = 0x35;
                        data[9] = 0x20;
                        //data[10] = 0x13; //команда чтения FLASH памяти(0x13)
                        data[10] = 0x09; //чтение значения ЦАП генератора напряжения(0x09)
                        data[11] = GetCRC(data, 0);//CRC
                        Port.Write(data, 0, data.Length);//

                        int i = 0;
                        while ((Port.BytesToRead <= 0) && (i < WAIT_TIMEOUT))
                        {
                            i++;
                            Thread.Sleep(1);
                        }
                        if (Port.BytesToRead > 0)
                        {
                            byte[] input = new byte[12];
                            int count = Port.Read(input, 0, 12);
                            if ((count == 12) && (input[0] == 0xCA) && (input[1] == 0x53))
                            {
                                Value = BitConverter.ToSingle(input, 4);
                            }

                            Mean = Value + Mean;
                            if (Max < Value) Max = Value;
                            if (Min > Value) Min = Value;
                            Thread.Sleep(READ_PERIOD);
                        }
                        else
                        {
                            Program.txtlog.WriteLineLog("Eni201: Отсутсвуют данные для чтения.", 1);
                            return false;
                        }
                    }
                    if (SAMPLE_COUNT >= 3)
                    {
                        Value = (Mean - Max - Min) / (SAMPLE_COUNT - 2);//усредняем
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
                    Program.txtlog.WriteLineLog("Eni201: Ошибка чтения данных.", 1);
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
