using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace Charaterizator
{
    class CMultimetr
    {
        public bool Connected;
        private SerialPort Port;

        public CMultimetr()
        {
            Port = new SerialPort();
        }

        public int DisConnect()
        {
            if (Connected)
            {
                Port.Close();
                Connected = false;
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
                Port.ReadTimeout = 1000;
                Port.WriteTimeout = 1000;
                Port.DtrEnable = true;
                Port.RtsEnable = true;
                Port.Open();
                if (ReadData() >= 0)
                {
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
                Connected = false;
                return -1;
            }
        }
        public int InitDevice()
        {
            if (Connected)
            {
                Port.WriteLine("SYST:REM");
                return 0;
            }
            else
            {
                return -1;
            }
        }
        public float ReadData()
        {
            float Data;
            if (Connected)
            {
                try
                {
                    Port.WriteLine("MEAS:VOLT:DC? 10, 0.001");
                    Thread.Sleep(300);
                    string str = Port.ReadLine();
                    Data = Convert.ToSingle(str);
                    return Data;
                }
                catch
                {
                    //запись в лог
//                    Connected = false;
                    return -2;
                }
            }
            else
            {
                return -1;
            }
        }
    }
}
