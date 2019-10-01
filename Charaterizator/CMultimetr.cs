using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

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
                Port.PortName = PortName;
                Port.BaudRate = BaudRate;
                Port.DataBits = DataBits;
                Port.StopBits = (StopBits)StopBits;
                Port.Parity = (Parity)Parity;
                Port.Open();
                Connected = true;
                return 1;
            }
            try
            {
                Connected = false;
                return 0;
            }
            catch
            {
                return -1;
            }
        }
        public int InitDevice()
        {
            if (Connected)
            {
                Port.WriteLine("");
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
                Port.WriteLine("");
                string str = Port.ReadLine();
                Data = Convert.ToSingle(str);
                return Data;
            }
            else
            {
                return 0;
            }
        }
    }
}
