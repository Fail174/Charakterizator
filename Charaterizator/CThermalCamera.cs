using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Charaterizator
{
    class CThermalCamera
    {
        public bool Connected;
        public double point = 0;
        private SerialPort Port;

        public CThermalCamera()
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
        public float ReadData()
        {
            float Data;
            int[] buf = new int[32];
            int i = 0;
            if (Connected)
            {

                while ((Port.BytesToRead > 0) && (i < 32))
                {
                    buf[i] = Port.ReadByte();
                    i++;
                }

                try
                {//Read Holding Registers (0x03)
                    byte[] data = new byte[8];                    data[0] = 1;//адрес устройства
                    data[1] = 3;//код функции

                    data[2] = 0;//начальный адрес рег
                    data[3] = 0;

                    data[4] = 0;//количество регистров
                    data[5] = 0xD;

                    int c = CRC16(data, 6);
                    data[6] = (byte)(c & 0xFF);
                    data[7] = (byte)((c>>8) & 0xFF);

                    Port.Write(data, 0, data.Length);

                    i = 0;
                    while (( Port.BytesToRead> 0)&&(i<32))
                    {
                        buf[i] = Port.ReadByte();
                        i++;
                    }
                    int tmp = (buf[3] << 24) | (buf[4] << 16) | (buf[5] << 8) | buf[6];
                    Data = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);
                    return Data;
                }
                catch
                {
                    Program.txtlog.WriteLineLog("Термокамера: Устройство не отвечает. ", 1);
                    return -2;
                }
            }
            else
            {
                return -1;
            }
        }


        int CRC16(byte[] pdata, int length)
        {
            int flag, crc = 0xFFFF;
            for (int i = 0; i < length; i++)
            {
                crc = crc ^ pdata[i];
                for (int j = 1; j <= 8; j++)
                {
                    flag = crc & 0x0001;
                    crc = crc >> 1;
                    if (flag==0)
                        crc = crc ^ 0xA001;
                }
            }
            //Меняем байты результата местами: младшим вперед
            return ((crc & 0x00FF) << 8) + ((crc & 0xFF00) >> 8);
        }
        public void WriteData(double val)
        {
            point = val;//уставка
        }

    }
}
