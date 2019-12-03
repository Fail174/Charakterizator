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
        public bool Connected;
        public double Value;
        private SerialPort Port;

        public CMultimetr()
        {
            Port = new SerialPort();
            Value = 0;
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
                Port.ReadTimeout = 2000;
                Port.WriteTimeout = 2000;
                Port.DtrEnable = true;
                Port.RtsEnable = true;
                Port.Open();
                Connected = true;
                InitDevice();
                if (ReadData() > -10000)
                {

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
                Thread.Sleep(100);
                Port.WriteLine("SYST:REM");
                Thread.Sleep(1000);
                return 0;
            }
            else
            {
                return -1;
            }
        }
        public double ReadData()
        {
            if (Connected)
            {
                try
                {

                    Port.WriteLine("MEAS:VOLT:DC? 10, 0.001");
                    Thread.Sleep(900);
                    string str = Port.ReadLine();
                    str = str.Replace(".","");
                    //Value = double.Parse(str.Replace(".", ""););
                    Value = double.Parse(str.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                    //Value = Convert.ToSingle(str);
                    return Value;
                }
                catch
                {
                    //запись в лог
                    //                    Connected = false;
                    Program.txtlog.WriteLineLog("Agilent: Устройство не отвечает. ", 1);
                    Port.Close();
                    Port.Open();
                    Value = 0;
                    return -10000;
                }
            }
            else
            {
                Value = 0;
                return -10000;
            }
        }
    }
}
