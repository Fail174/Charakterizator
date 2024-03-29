﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.Collections;
using System.Windows.Forms;
using TxtLog;

//namespace ENI100
namespace Charaterizator
{
    
    public struct SensorID
    {
        public int Channal;    //Номер канала коммутатора
        public int Group;      //Номер группы (1, 2, 3, 4)

        public byte Addr;      //адрес устройства

        public ushort state;   //bb bb – 2 байта статуса(сообщение об ошибках, 00 00 - ок)
        public byte devCode;   //21 – код изготовителя(число, char)
        public byte devType;   //СС – код типа устройства(ЭнИ-100-СД/ЖК1) (число, char), CD(ЭнИ-12), CE(ЭнИ-100-ЖК2), CF(ЭнИ-12М)
        public byte pre;       //05 – число преамбул(5) (число, char)
        public byte v1;        //V1 –  версия универсальных команд(число, char)
        public byte v2;        //V2 – версия специфических команд(число, char)
        public byte v3;        //V3 – версия ПО(число, char)
        public byte v4;        //V4 – версия аппаратного обеспечения(число, char)
        public byte flag;      //Fl – флаги функций устройства(0x01 – multi sensor field device)
        public UInt32 uni;     //UNI – заводской номер(3 байта)

        public byte[] message; // 24 байта

        public byte[] teg;     //тэг – 6 байт(своя таблица ASCII, один символ занимает 6 бит)
        public byte[] desc;    //дескриптор – 12 байт(своя таблица ASCII, один символ занимает 6 бит)
        public UInt32 data;    //дата – 3 байта(день, месяц, год = 1900 + число)
        public UInt32 SerialNumber;
        public float UpLevel;
        public float DownLevel;
        public float MinLevel;
        public byte MesUnit;

        public float OutCurrent;//начение выходного тока, мА, тип float
        public float Pressure;//значение измеренного давления, тип float
        public float OutVoltage;//значение выходного напряжения с сенсора, тип float
        public float Resistance;//значение сопротивления диагонали тензомоста, тип float
        public float Temperature;//значение температуры, тип float
        public byte CurrentUnit;
        public byte PressueUnit;
        public byte VoltageUnit;
        public byte ResistanceUnit;
        public byte TemperatureUnit;

        public float VPI;   //ВПИ
        public float NPI;   //НПИ
        public float DempfTime; //время демпфирования


        public byte CurrentExit;//токовый выход (0 - 4мА, 1 - 20мА)

        public byte PressureType;
        public char[] PressureModel;

        public float[] Coefficient;

        public byte qrc;  //колличество наборов коэф
        public byte UGain1;
        public byte UGain2;
        public byte UPower1;
        public byte UPower2;
        public byte ExtTemp;
        public byte TPower1;
        public byte TPower2;
        public float TranspPoint;
        public byte Param0;
        public byte Param1;
        public byte Param2;
        public byte Param3;

        public double[] DCoefficient;


        public byte SetOfCoef;//набор коэффикиентов 0 - первый, 1 - второй
        public SensorID(byte a, int CoeffCount)
        {
            message = new byte[24];
            desc = new byte[12];
            teg = new byte[6];
            Coefficient = new float[CoeffCount];
            DCoefficient = new double[CoeffCount];
            PressureModel = new char[5];

            Addr = a;
            Channal = 0;
            Group = 1;

            state = 0;   //bb bb – 2 байта статуса(сообщение об ошибках, 00 00 - ок)
            devCode = 0;   //21 – код изготовителя(число, char)
            devType = 0xCE;   //СС – код типа устройства(ЭнИ-100-СД/ЖК1) (число, char), CD(ЭнИ-12), CE(ЭнИ-100-ЖК2), CF(ЭнИ-12М)
            pre = 5;       //05 – число преамбул(5) (число, char)
            v1 = 0;        //V1 –  версия универсальных команд(число, char)
            v2 = 0;        //V2 – версия специфических команд(число, char)
            v3 = 0;        //V3 – версия ПО(число, char)
            v4 = 0;        //V4 – версия аппаратного обеспечения(число, char)
            flag = 0;      //Fl – флаги функций устройства(0x01 – multi sensor field device)
            uni = 0;     //UNI – заводской номер(3 байта)

            data = 0;    //дата – 3 байта(день, месяц, год = 1900 + число)
            SerialNumber = 0;
            UpLevel = 0;
            DownLevel = 0;
            MinLevel = 0;
            MesUnit = 0;

            OutCurrent=0;//начение выходного тока, мА, тип float
            Pressure = 0;//значение измеренного давления, тип float
            OutVoltage = 0;//значение выходного напряжения с сенсора, тип float
            Resistance = 0;//значение сопротивления диагонали тензомоста, тип float
            Temperature = 0;//значение температуры, тип float
            CurrentUnit = 0;
            PressueUnit = 0;
            VoltageUnit = 0;
            ResistanceUnit = 0;
            TemperatureUnit = 0;

            CurrentExit = 0;

            PressureType = 0;

            VPI = 0;
            NPI = 0;
            DempfTime = 0;
            SetOfCoef = 0;

            qrc = 1;
            UGain1 = 16;
            UGain2 = 16;
            UPower1 = 5;
            UPower2 = 5;
            ExtTemp = 0;
            TPower1 = 3;
            TPower2 = 3;
            TranspPoint = 0;
            Param0 = 0;
            Param1 = 0;
            Param2 = 0;
            Param3 = 0;
    }
    public string GetdevType()
        {
            switch (devType)
            { 
                case 0xCC:
                    return "ЭНИ-100";
                case 0xCD:
                    return "ЭНИ-12";
                case 0xCE:
                    return "ЭНИ-100-ЖК2";
                case 0xCF:
                    return "ЭНИ-12М";
                default:
                    return "не определено";
            }
        }
        public string GetUnit()  // 
        {
            switch (MesUnit)
            {
                case 0x05:
                    return "мм рт.ст.";
                case 0x07:
                    return "бар";
                case 0x08:
                    return "мбар";
                case 0x0B:
                    return "Па";
                case 0x0D:
                    return "атм";
                case 0x0C:
                    return "кПа";
                case 0xED:
                    return "МПа";
                default:
                    return "";
            }
        }

        public void SetUnit(string u)  // 
        {
            switch (u)
            {
                case "мм рт.ст.":
                    MesUnit = 0x05;
                    break;
                case "бар":
                    MesUnit = 0x07;
                    break;
                case "мбар":
                    MesUnit = 0x08;
                    break;
                case "Па":
                    MesUnit = 0x0B;
                    break;
                case "кПа":
                    MesUnit=0x0C;
                    break;
                case "атм":
                    MesUnit = 0x0D;
                    break;
                case "МПа":
                    MesUnit = 0xED;
                    break;
                default:
                    MesUnit = 0x0C;
                    break;
            }
        }
        public string GetTeg()
        {
            BitArray bits = new BitArray(teg);
            BitArray bits6 = new BitArray(6);
            byte[] bytearray = new byte[8];
            int j = 0;

            for (int k = 0; k < 8; k++)
            {
                for (int i = 0; i < 6; i++)
                {
                    bits6[i] = bits[j];
                    j++;
                }
                bits6.CopyTo(bytearray, k);
            }
            return Encoding.ASCII.GetString(bytearray);
        }
        public string GetDesc()
        {
            BitArray bits = new BitArray(desc);
            BitArray bits6 = new BitArray(6);
            byte[] bytearray = new byte[16];
            int j = 0;

            for (int k = 0; k < 16; k++)
            {
                for (int i = 0; i < 6; i++)
                {
                    bits6[i] = bits[j];
                    j++;
                }
                bits6.CopyTo(bytearray, k);
            }
            return Encoding.ASCII.GetString(bytearray);
        }
        public string GetMes()
        {
            BitArray bits = new BitArray(message);
            BitArray bits6 = new BitArray(6);
            byte[] bytearray = new byte[32];
            int j = 0;

            for (int k = 0; k < 32; k++)
            {
                for (int i = 0; i < 6; i++)
                {
                    bits6[i] = bits[j];
                    j++;
                }
                bits6.CopyTo(bytearray, k);
            }
            return Encoding.ASCII.GetString(bytearray);
        }

    }
 
    public class ClassEni100
    {
        public int COEFF_COUNT = 48;//число коэффициентов

        public int WRITE_PERIOD = 300;  //период выдачи команд
        public int WRITE_COUNT = 1;     //число попыток записи команд в датчик
        public int WAIT_TIMEOUT = 300;  //таймаут ожидания ответа от датчика
        public int MaxSensorOnLevel = 8;//количество датиков на уровне

        static SerialPort port = null;
        static FastFifo readbuf = new FastFifo();
        static bool SensorConnect = false;
        public List<SensorID> sensorList = new List<SensorID>();//список обнаруженных датчиков
        public SensorID sensor;//текущий обслуживаемый датчик
        public int SelSensorChannal = 0;//номер текущего канала
        public int SelSensorIndex = 0;//номер выбранного датчика в списке
        //        public SensorTeg steg = new SensorTeg(6, 12);
        private Thread ReadThread;
        private bool PreambulFinded=false;

        private int ReadAvtState = 1;       //состояние автомата
        private int CommandCod = -1;          //код ответной команды
        private int CountByteToRead = 0;    //количество байт в команде
        private byte Adress = 0;               //адрес устройства

        //public CTxtlog txtlog;

        public ClassEni100(int sl)
        {
            MaxSensorOnLevel = sl;//количество датиков на уровне
            

            SelSensorChannal = 0;
            sensorList.Clear();
            port = new SerialPort();
            readbuf.Clear();
        }

        ~ClassEni100()
        {
            DisConnect();
            sensorList.Clear();
            readbuf.Clear();
        }

        public bool IsConnect()
        {
            return SensorConnect;
        }

        public byte GetCRC(byte[] data,int start)
        {
            byte res = 0;
            for (int i = start; i < data.Length-1; i++)
            {
                res = (byte)(res ^ data[i]);
            }
            return res;
        }

        //проверка сопротивления и выходного напряжения датчика на корректность
        public bool ValidateSensorParam()
        {
            if ((Math.Abs(sensor.Resistance) < 0.0) || (Math.Abs(sensor.OutVoltage) < 0.0))
            {
                return false;//проверка не пройдена
            }
            else
            {
                return true;//проверка пройдена
            }
        }

        //ожидание ответа от датчика
        //Вход: 
        //count - количество байт для чтения
        //timeout - время ожидания в мс
        //Выход: 
        //true - получены данный с датчика
        //false - данных с датчика нет
        public bool WaitSensorAnswer(int count, int timeout)
        {
            int c=0;
            while ((readbuf.Count < count) && (c < timeout))
            {
                c++;
                Thread.Sleep(1);
                Application.DoEvents();
                if (Program.mainwnd != null)
                    if (Program.mainwnd.ProcessStop) return false;
            }
            return c < timeout;
        }

        /// <summary>
        /// Поиск повторений серийников
        /// </summary>
        /// <returns></returns>
        public bool CheckValidSN()
        {
            for (int i = 0; i < sensorList.Count; i++)
            {
                if (sensorList[i].Channal != sensor.Channal)
                    if (sensorList[i].uni == sensor.uni)
                        return false;
            }
            return true;
        }

        //поиск датчика в списке по номеру канала
        public bool SelectSensor(int ch)
        {
            for (int i = 0; i < sensorList.Count; i++)
            {
                if (sensorList[i].Channal == ch)
                {//датчик в канале найден
                    sensor = sensorList[i];
                    SelSensorChannal = ch;
                    SelSensorIndex = i;
                    return true;
                }
            }
            //SelSensorChannal = -1;
            //SelSensorIndex = -1;
            return false;
        }
        //поиск подключенных датчиков в заданном канале
        public bool SeachSensor(int ch)
        {
            if((port != null)&&(SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть
                int i;
                sensor.pre = 7;
                byte[] data = new byte[sensor.pre + 5];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                i = sensor.pre;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0x00;
                data[i + 3] = 0x00;
//  data[i + 4] = 0x82;
                //byte[] data = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x02, 0x80, 0x00, 0x00, 0x82 };

                //поиск производим только по 0му адресу 30.10.2019
                SelSensorChannal = ch;
                data[data.Length - 1] = GetCRC(data, sensor.pre);
                for (int j = 0; j < WRITE_COUNT; j++)
                {
                    Thread.Sleep(WRITE_PERIOD);
                    port.Write(data, 0, data.Length);
                    WaitSensorAnswer(10, WAIT_TIMEOUT);
                    if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                        return true;
                }
            }
            return false;
        }
        
        //Чтение ТЕГа выбранного датчика
        public bool TegRead()
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть
                int i;
                byte[] data = new byte[sensor.pre + 5];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                i = sensor.pre;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0x0D;
                data[i + 3] = 0x00;

               // byte[] data = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x02, 0x80, 0x0D, 0x00, 0x8F };

               // data[sensor.pre + 1] = (byte)(0x80 | sensor.Addr);
                data[data.Length - 1] = GetCRC(data, sensor.pre);//CRC
                for (int j = 0; j < WRITE_COUNT; j++)
                {
                    Thread.Sleep(WRITE_PERIOD);
                    port.Write(data, 0, data.Length);
                    WaitSensorAnswer(sensor.pre, WAIT_TIMEOUT);
                    if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                        return true;
                }
            }
            return false;
        }

        //Чтение выбранного датчика
        public bool C14SensorRead()
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть
                int i;
                byte[] data = new byte[sensor.pre + 5];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                i = sensor.pre;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0x0E;
                data[i + 3] = 0x00;
                data[i + 4] = GetCRC(data, sensor.pre);//CRC
//                data[9] = (byte)(data[9] + sensor.Addr);

                for (int j = 0; j < WRITE_COUNT; j++)
                {
                    Thread.Sleep(WRITE_PERIOD);
                    port.Write(data, 0, data.Length);
                    WaitSensorAnswer(sensor.pre, WAIT_TIMEOUT);
                    if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                        return true;
                }
            }
            return false;
        }

        //Чтение измеренных параметров выбранного датчика
        public bool SensorValueReadC03()
        {
            int res = 0;
            int j;
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть
                int i;
                byte[] data = new byte[sensor.pre + 5];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                i = sensor.pre;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0x03;
                data[i + 3] = 0x00;
                data[i + 4] = GetCRC(data, sensor.pre);//CRC
                for (j = 0; j < WRITE_COUNT; j++)
                {
                    Thread.Sleep(WRITE_PERIOD);
                    port.Write(data, 0, data.Length);
                    WaitSensorAnswer(20, WAIT_TIMEOUT);
                    res = ParseReadBuffer(WAIT_TIMEOUT);
                    if (res >= 0)
                        return true;
                }
            }
            return false;
        }

        //Запись нового адреса выбранного датчика
        public bool SensorAddrWrite()
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть
                int i;
                byte[] data = new byte[sensor.pre + 5];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                i = sensor.pre;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0x0E;
                data[i + 3] = 0x00;
                data[data.Length - 1] = GetCRC(data, sensor.pre);//CRC
                for (int j = 0; j < WRITE_COUNT; j++)
                {
                    Thread.Sleep(WRITE_PERIOD);
                    port.Write(data, 0, data.Length);
                    WaitSensorAnswer(sensor.pre, WAIT_TIMEOUT);
                    if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                        return true;
                }
            }
            return false;
        }
        //Переход в сервесный режим
        public bool EnterServis()
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть
                int i;
                byte[] data = new byte[sensor.pre + 11];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                i = sensor.pre;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0xF5;
                data[i + 3] = 0x06;
                data[i + 4] = 0x41;
                data[i + 5] = 0x73;
                data[i + 6] = 0x62;
                data[i + 7] = 0x4D;
                data[i + 8] = 0x6B;
                data[i + 9] = 0x39;
                //byte[] data = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x02, 0x80, 0xF5, 0x06, 0x41, 0x73, 0x62, 0x4D, 0x6B, 0x39, 0x00};

                //data[sensor.pre + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 10] = GetCRC(data, sensor.pre);//CRC

                for (int j = 0; j < WRITE_COUNT; j++)
                {
                    Thread.Sleep(WRITE_PERIOD);
                    port.Write(data, 0, data.Length);
                    WaitSensorAnswer(sensor.pre, WAIT_TIMEOUT);
                    if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                        return true;
                }
            }
            return false;
        }

        //Запись верхнего и нижнего пределов ПД, минимального диапазона (команда 249)
        public bool UpDownWriteC249()
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть
                int i;
                byte[] data = new byte[sensor.pre + 18];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                i = sensor.pre;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i+2] = 0xF9;
                data[i+3] = 0x0D;

                data[i+4] = sensor.MesUnit;
                UInt32 tmp;
                tmp = BitConverter.ToUInt32(BitConverter.GetBytes(sensor.UpLevel),0);
                data[i + 5] = (byte)((tmp >> 24)&0xFF);
                data[i + 6] = (byte)((tmp >> 16) & 0xFF);
                data[i + 7] = (byte)((tmp >> 8) & 0xFF);
                data[i + 8] = (byte)(tmp & 0xFF);
                tmp = BitConverter.ToUInt32(BitConverter.GetBytes(sensor.DownLevel), 0);
                data[i + 9] = (byte)((tmp >> 24) & 0xFF);
                data[i + 10] = (byte)((tmp >> 16) & 0xFF);
                data[i + 11] = (byte)((tmp >> 8) & 0xFF);
                data[i + 12] = (byte)(tmp & 0xFF);
                tmp = BitConverter.ToUInt32(BitConverter.GetBytes(sensor.MinLevel), 0);
                data[i + 13] = (byte)((tmp >> 24) & 0xFF);
                data[i + 14] = (byte)((tmp >> 16) & 0xFF);
                data[i + 15] = (byte)((tmp >> 8) & 0xFF);
                data[i + 16] = (byte)(tmp & 0xFF);

                data[i + 17] = GetCRC(data,sensor.pre);//CRC

                for (int j = 0; j < WRITE_COUNT; j++)
                {
                    Thread.Sleep(WRITE_PERIOD);
                    port.Write(data, 0, data.Length);
                    WaitSensorAnswer(sensor.pre, WAIT_TIMEOUT);
                    if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                        return true;
                }
            }
            return false;
        }

        //Чтение ВПИ и НПИ (команда 15)
        public bool С15ReadVPI_NPI()
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть
                int i;
                byte[] data = new byte[sensor.pre + 5];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                i = sensor.pre;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0xF;//код команды
                data[i + 3] = 0x00;//количество байт
                data[i + 4] = GetCRC(data, sensor.pre);//CRC
                for (int j = 0; j < WRITE_COUNT; j++)
                {
                    Thread.Sleep(WRITE_PERIOD);
                    port.Write(data, 0, data.Length);
                    WaitSensorAnswer(sensor.pre, WAIT_TIMEOUT);
                    if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                        return true;
                }
            }
            return false;
        }


        //Запись времени демпфирования (команда 34)
        public bool С34WriteDTime(float DTime)
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть
                int i;
                byte[] data = new byte[sensor.pre + 10];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                i = sensor.pre;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0x23;//код команды
                data[i + 3] = 0x09;//количество байт
                data[i + 4] = 0x0C;//

                UInt32 tmp = BitConverter.ToUInt32(BitConverter.GetBytes(DTime), 0);
                data[i + 5] = (byte)((tmp >> 24) & 0xFF);
                data[i + 6] = (byte)((tmp >> 16) & 0xFF);
                data[i + 7] = (byte)((tmp >> 8) & 0xFF);
                data[i + 8] = (byte)(tmp & 0xFF);

                data[i + 9] = GetCRC(data, sensor.pre);//CRC
                for (int j = 0; j < WRITE_COUNT; j++)
                {
                    Thread.Sleep(WRITE_PERIOD);
                    port.Write(data, 0, data.Length);
                    WaitSensorAnswer(sensor.pre, WAIT_TIMEOUT);
                    if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                        return true;
                }
            }
            return false;
        }

        //Запись ВПИ и НПИ (команда 35)
        public int С35WriteVPI_NPI(float VPI, float NPI)
        {
            int result=-1;
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть
                int i;
                byte[] data = new byte[sensor.pre + 14];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                i = sensor.pre;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0x23;//код команды
                data[i + 3] = 0x09;//количество байт
                data[i + 4] = 0x0C;//

                UInt32 tmp = BitConverter.ToUInt32(BitConverter.GetBytes(VPI), 0);
                data[i + 5] = (byte)((tmp >> 24) & 0xFF);
                data[i + 6] = (byte)((tmp >> 16) & 0xFF);
                data[i + 7] = (byte)((tmp >> 8) & 0xFF);
                data[i + 8] = (byte)(tmp & 0xFF);
                tmp = BitConverter.ToUInt32(BitConverter.GetBytes(NPI), 0);
                data[i + 9] = (byte)((tmp >> 24) & 0xFF);
                data[i + 10] = (byte)((tmp >> 16) & 0xFF);
                data[i + 11] = (byte)((tmp >> 8) & 0xFF);
                data[i + 12] = (byte)(tmp & 0xFF);

                data[i + 13] = GetCRC(data, sensor.pre);//CRC
                for (int j = 0; j < WRITE_COUNT; j++)
                {
                    Thread.Sleep(WRITE_PERIOD);
                    port.Write(data, 0, data.Length);
                    WaitSensorAnswer(sensor.pre, WAIT_TIMEOUT);
                    result = ParseReadBuffer(WAIT_TIMEOUT);
                    if (result >= 0)
                        return result;
                }
            }
            return result;
        }



        //Режим фиксированного тока (команда 40)
        public bool С40WriteFixCurrent(float Current)
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть
                int i;
                byte[] data = new byte[sensor.pre + 9];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                i = sensor.pre;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0x28;
                data[i + 3] = 0x04;
                UInt32 tmp = BitConverter.ToUInt32(BitConverter.GetBytes(Current), 0);
                data[i + 4] = (byte)((tmp >> 24) & 0xFF);
                data[i + 5] = (byte)((tmp >> 16) & 0xFF);
                data[i + 6] = (byte)((tmp >> 8) & 0xFF);
                data[i + 7] = (byte)(tmp & 0xFF);
                data[i + 8] = GetCRC(data, sensor.pre);//CRC
                for (int j = 0; j < WRITE_COUNT; j++)
                {
                    Thread.Sleep(WRITE_PERIOD);
                    port.Write(data, 0, data.Length);
                    WaitSensorAnswer(sensor.pre, WAIT_TIMEOUT);
                    if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                        return true;
                }
            }
            return false;
        }

        //Перезагрузка датчика (команда 42).
        public bool С42SensorReset()
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть
                int i;
                byte[] data = new byte[sensor.pre + 5];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                i = sensor.pre;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0x2A;
                data[i + 3] = 0x00;
                data[i + 4] = GetCRC(data, sensor.pre);//CRC
                for (int j = 0; j < WRITE_COUNT; j++)
                {
                    Thread.Sleep(WRITE_PERIOD);
                    port.Write(data, 0, data.Length);
                    WaitSensorAnswer(sensor.pre, WAIT_TIMEOUT);
                    if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                        return true;
                }
            }
            return false;
        }

        //Установить ноль первичной переменной (коррекция нуля от монтажного положения) (команда 43).
        public bool С43SetZero()
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть
                int i;
                byte[] data = new byte[sensor.pre + 5];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                i = sensor.pre;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0x2B;
                data[i + 3] = 0x00;
                data[i + 4] = GetCRC(data, sensor.pre);//CRC
                for (int j = 0; j < WRITE_COUNT; j++)
                {
                    Thread.Sleep(WRITE_PERIOD);
                    port.Write(data, 0, data.Length);
                    WaitSensorAnswer(sensor.pre, WAIT_TIMEOUT);
                    if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Коррекция нуля путем ввода значения атмосферного давления (команда 143)
        /// </summary>
        /// <returns></returns>
        public bool С143SetZero(float value)
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть
                int i;
                byte[] data = new byte[sensor.pre + 9];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                i = sensor.pre;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0x8F;
                data[i + 3] = 0x04;
                UInt32 tmp = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
                data[i + 4] = (byte)((tmp >> 24) & 0xFF);
                data[i + 5] = (byte)((tmp >> 16) & 0xFF);
                data[i + 6] = (byte)((tmp >> 8) & 0xFF);
                data[i + 7] = (byte)(tmp & 0xFF);
                data[i + 8] = GetCRC(data, sensor.pre);//CRC
                for (int j = 0; j < WRITE_COUNT; j++)
                {
                    Thread.Sleep(WRITE_PERIOD);
                    port.Write(data, 0, data.Length);
                    WaitSensorAnswer(sensor.pre, WAIT_TIMEOUT);
                    if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                        return true;
                }
            }
            return false;
        }


        //Установить единицу измерения первичной переменной
        public bool С44WriteMesUnit(string unit)
        {
            if ((port != null) && (SensorConnect))
            {
                sensor.SetUnit(unit);
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть
                int i;
                byte[] data = new byte[sensor.pre + 6];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                i = sensor.pre;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0x2A;
                data[i + 3] = 0x01;
                data[i+4] = sensor.MesUnit;
                data[i + 5] = GetCRC(data, sensor.pre);//CRC
                for (int j = 0; j < WRITE_COUNT; j++)
                {
                    Thread.Sleep(WRITE_PERIOD);
                    port.Write(data, 0, data.Length);
                    WaitSensorAnswer(sensor.pre, WAIT_TIMEOUT);
                    if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                        return true;
                }
            }
            return false;
        }
        //Коррекция нуля ЦАП (команда 45).
        public bool С45WriteCurrent4mA(float Current)
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть
                int i;
                byte[] data = new byte[sensor.pre + 9];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                i = sensor.pre;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0x2D;
                data[i + 3] = 0x04;
                UInt32 tmp = BitConverter.ToUInt32(BitConverter.GetBytes(Current), 0);
                data[i + 4] = (byte)((tmp >> 24) & 0xFF);
                data[i + 5] = (byte)((tmp >> 16) & 0xFF);
                data[i + 6] = (byte)((tmp >> 8) & 0xFF);
                data[i + 7] = (byte)(tmp & 0xFF);
                data[i + 8] = GetCRC(data, sensor.pre);//CRC
                for (int j = 0; j < WRITE_COUNT; j++)
                {
                    Thread.Sleep(WRITE_PERIOD);
                    port.Write(data, 0, data.Length);
                    WaitSensorAnswer(sensor.pre, WAIT_TIMEOUT);
                    if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                        return true;
                }
            }
            return false;
        }

        //Коррекция коэффициента усиления ЦАП (команда 46)
        public bool С46WriteCurrent20mA(float Current)
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть

                int i;
                byte[] data = new byte[sensor.pre+9];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                i = sensor.pre;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0x2E;
                data[i + 3] = 0x04;
                UInt32 tmp = BitConverter.ToUInt32(BitConverter.GetBytes(Current), 0);
                data[i + 4] = (byte)((tmp >> 24) & 0xFF);
                data[i + 5] = (byte)((tmp >> 16) & 0xFF);
                data[i + 6] = (byte)((tmp >> 8) & 0xFF);
                data[i + 7] = (byte)(tmp & 0xFF);
                data[i + 8] = GetCRC(data, sensor.pre);//CRC
                for (int j = 0; j < WRITE_COUNT; j++)
                {
                    Thread.Sleep(WRITE_PERIOD);
                    port.Write(data, 0, data.Length);
                    WaitSensorAnswer(sensor.pre, WAIT_TIMEOUT);
                    if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                        return true;
                }
            }
            return false;
        }

        //Запись серийного номера ПД(команда 49).
        public bool WriteSerialNumberC49()
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть
                int i;
                byte[] data = new byte[sensor.pre+8];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0x31;
                data[i + 3] = 0x03;
                data[i + 4] = (byte)((sensor.SerialNumber>>16) & 0xFF);
                data[i + 5] = (byte)((sensor.SerialNumber >> 8) & 0xFF);
                data[i + 6] = (byte)(sensor.SerialNumber & 0xFF);
                data[i + 7] = GetCRC(data, sensor.pre);//CRC
                for (int j = 0; j < WRITE_COUNT; j++)
                {
                    Thread.Sleep(WRITE_PERIOD);
                    port.Write(data, 0, data.Length);
                    WaitSensorAnswer(sensor.pre, WAIT_TIMEOUT);
                    if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                        return true;
                }
            }
            return false;
        }

        //Запись модели приемника давления (команда 241).
        public bool C241WritePressureModel()
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть

                int i;
                byte[] data = new byte[sensor.pre+11];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0xF1;
                data[i + 3] = 0x06;
                data[i + 4] = sensor.PressureType;
                for (int j=0;j<5;j++)
                {
                    data[i + 5 + j] = Convert.ToByte(sensor.PressureModel[j]);
                }

                data[data.Length-1] = GetCRC(data, sensor.pre);//CRC
                for (int j = 0; j < WRITE_COUNT; j++)
                {
                    Thread.Sleep(WRITE_PERIOD);
                    port.Write(data, 0, data.Length);
                    WaitSensorAnswer(sensor.pre, WAIT_TIMEOUT);
                    if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                        return true;
                }
            }
            return false;
        }

        //Чтение модели приемника давления (команда 140).
        public bool C140ReadPressureModel()
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть
                int i;
                byte[] data = new byte[sensor.pre+5];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0x8C;
                data[i + 3] = 0x00;
                data[i+4] = GetCRC(data, sensor.pre);//CRC
                for (int j = 0; j < WRITE_COUNT; j++)
                {
                    Thread.Sleep(WRITE_PERIOD);
                    port.Write(data, 0, data.Length);
                    WaitSensorAnswer(sensor.pre, WAIT_TIMEOUT);
                    if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Запись калибровочных коэффициентов для датчика давления double (команда 200)
        /// </summary>
        public bool C200SensorCoefficientWrite()
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть
                UInt64 tmp;
                int i;
                byte[] data = new byte[sensor.pre + 38];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                i = sensor.pre;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0xC8;//код команды
                data[i + 3] = 0x21;//количество байт

                for (int ci = 0; ci < 6; ci++)
                {
                    int c = i + 5;
                    data[i + 4] = (byte)ci;
                    for (int n = 0; n < 4; n++)
                    {
                        tmp = BitConverter.ToUInt64(BitConverter.GetBytes(sensor.DCoefficient[4 * ci+n]), 0);
                        for (int j = 7; j >= 0; j--)
                        {
                            data[c + j] = (byte)(tmp & 0xFF);
                            tmp = tmp >> 8;
                        }
                        c = c + 8;
                        sensor.DCoefficient[4 * ci+n] = 0;
                    }

                    data[c] = GetCRC(data, sensor.pre);//CRC

                    for (int j = 0; j < WRITE_COUNT; j++)
                    {
                        Thread.Sleep(WRITE_PERIOD);
                        port.Write(data, 0, data.Length);
                        WaitSensorAnswer(20, WAIT_TIMEOUT);
                        int res = ParseReadBuffer(WAIT_TIMEOUT);
                        if (res >= 0)
                            break;
                        if (res == -8)
                            return false;
                    }
                }
                return true;
            }
            return false;
        }
        //Чтение калибровочных коэффициентов double (команда 201)
        public bool C201SensorCoefficientRead()
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть

                byte[] data = new byte[sensor.pre + 6];
                for (int i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                data[sensor.pre] = 0x02;
                data[sensor.pre + 1] = (byte)(0x80 | sensor.Addr);
                data[sensor.pre + 2] = 0xC9;
                data[sensor.pre + 3] = 0x1;
                for (int i = 0; i < 6; i++)
                {
                    data[sensor.pre + 4] = (byte)i;
                    data[sensor.pre + 5] = GetCRC(data, sensor.pre);//CRC
                    for (int j = 0; j < WRITE_COUNT; j++)
                    {
                        Thread.Sleep(WRITE_PERIOD);
                        port.Write(data, 0, data.Length);
                        WaitSensorAnswer(20, WAIT_TIMEOUT);
                        if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                            break;
                    }
                }
                return true;
            }
            return false;
        }

        //Запись калибровочных коэффициентов для датчика давления (команда 250)
        public bool C250SensorCoefficientWrite()
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть
                UInt32 tmp;
                int i;
                byte[] data = new byte[sensor.pre + 22];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                i = sensor.pre;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0xFA;//код команды
                data[i + 3] = 0x11;//количество байт

                for (int ci = 0; ci < 6; ci++)
                {
                    data[i+4] = (byte)ci;

                    tmp = BitConverter.ToUInt32(BitConverter.GetBytes(sensor.Coefficient[4*ci]), 0);
                    sensor.Coefficient[4 * ci] = 0;
                    data[i + 5] = (byte)((tmp >> 24) & 0xFF);
                    data[i + 6] = (byte)((tmp >> 16) & 0xFF);
                    data[i + 7] = (byte)((tmp >> 8) & 0xFF);
                    data[i + 8] = (byte)(tmp & 0xFF);

                    tmp = BitConverter.ToUInt32(BitConverter.GetBytes(sensor.Coefficient[4 * ci + 1]), 0);
                    sensor.Coefficient[4 * ci+1] = 0;
                    data[i + 9] = (byte)((tmp >> 24) & 0xFF);
                    data[i + 10] = (byte)((tmp >> 16) & 0xFF);
                    data[i + 11] = (byte)((tmp >> 8) & 0xFF);
                    data[i + 12] = (byte)(tmp & 0xFF);

                    tmp = BitConverter.ToUInt32(BitConverter.GetBytes(sensor.Coefficient[4 * ci + 2]), 0);
                    sensor.Coefficient[4 * ci + 2] = 0;
                    data[i + 13] = (byte)((tmp >> 24) & 0xFF);
                    data[i + 14] = (byte)((tmp >> 16) & 0xFF);
                    data[i + 15] = (byte)((tmp >> 8) & 0xFF);
                    data[i + 16] = (byte)(tmp & 0xFF);

                    tmp = BitConverter.ToUInt32(BitConverter.GetBytes(sensor.Coefficient[4 * ci + 3]), 0);
                    sensor.Coefficient[4 * ci + 3] = 0;
                    data[i + 17] = (byte)((tmp >> 24) & 0xFF);
                    data[i + 18] = (byte)((tmp >> 16) & 0xFF);
                    data[i + 19] = (byte)((tmp >> 8) & 0xFF);
                    data[i + 20] = (byte)(tmp & 0xFF);

                    data[i + 21] = GetCRC(data, sensor.pre);//CRC

                    for (int j = 0; j < WRITE_COUNT; j++)
                    {
                        Thread.Sleep(WRITE_PERIOD);
                        port.Write(data, 0, data.Length);
                        WaitSensorAnswer(20, WAIT_TIMEOUT);
                        int res = ParseReadBuffer(WAIT_TIMEOUT);
                        if (res >= 0)
                            break;
                        if (res == -8)
                            return false;
                    }
                }
                return true;
            }
            return false;
        }

        //Чтение калибровочных коэффициентов (команда 251)
        public bool C251SensorCoefficientRead()
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть

//                byte[] data = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x02, 0x80, 0xFB, 0x01, 0x00, 0x00 };

                byte[] data = new byte[sensor.pre + 6];
                for (int i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                data[sensor.pre] = 0x02;
                data[sensor.pre + 1] = (byte)(0x80 | sensor.Addr);
                data[sensor.pre + 2] = 0xFB;
                data[sensor.pre + 3] = 0x1;
                for (int i = 0; i <= 5; i++)
                {
                    data[sensor.pre + 4] = (byte)i;
                    data[sensor.pre + 5] = GetCRC(data, sensor.pre);//CRC
                    for (int j = 0; j < WRITE_COUNT; j++)
                    {
                        Thread.Sleep(WRITE_PERIOD);
                        port.Write(data, 0, data.Length);
                        WaitSensorAnswer(20, WAIT_TIMEOUT);
                        if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                            break;
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Запись дополнительных параметров датчика (команда 151)
        /// </summary>
        public bool C151SensorCoefficientWrite()
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть
                UInt32 tmp;
                int i;
                byte[] data = new byte[sensor.pre + 21];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                i = sensor.pre;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0x97; //код команды
                data[i + 3] = 0x10; //количество байт
                data[i + 4] = sensor.qrc;  //колличество наборов коэф
                data[i + 5] = sensor.UGain1;
                data[i + 6] = sensor.UPower1;
                data[i + 7] = sensor.UGain2;
                data[i + 8] = sensor.UPower2;
                data[i + 9] = sensor.ExtTemp;
                data[i + 10] = sensor.Param0;
                data[i + 11] = sensor.Param1;
                data[i + 12] = sensor.Param2;
                data[i + 13] = sensor.Param3;
                data[i + 14] = sensor.TPower1;
                data[i + 15] = sensor.TPower2;
                tmp = BitConverter.ToUInt32(BitConverter.GetBytes(sensor.TranspPoint), 0);
                data[i + 16] = (byte)((tmp >> 24) & 0xFF);
                data[i + 17] = (byte)((tmp >> 16) & 0xFF);
                data[i + 18] = (byte)((tmp >> 8) & 0xFF);
                data[i + 19] = (byte)(tmp & 0xFF);

                data[i + 20] = GetCRC(data, sensor.pre);//CRC

                    for (int j = 0; j < WRITE_COUNT; j++)
                    {
                        Thread.Sleep(WRITE_PERIOD);
                        port.Write(data, 0, data.Length);
                        WaitSensorAnswer(15, WAIT_TIMEOUT);
                        if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                            break;
                    }

                return true;
            }
            return false;
        }

        /// <summary>
        /// Чтение дополнительных параметров датчика (команда 152)
        /// </summary>
        public bool C152SensorCoefficientRead()
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть

                byte[] data = new byte[sensor.pre + 5];
                for (int i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                data[sensor.pre] = 0x02;
                data[sensor.pre + 1] = (byte)(0x80 | sensor.Addr);
                data[sensor.pre + 2] = 0x98;
                data[sensor.pre + 3] = 0x0;
                data[sensor.pre + 4] = GetCRC(data, sensor.pre);//CRC
                    for (int j = 0; j < WRITE_COUNT; j++)
                    {
                        Thread.Sleep(WRITE_PERIOD);
                        port.Write(data, 0, data.Length);
                        WaitSensorAnswer(15, WAIT_TIMEOUT);
                        if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                            break;
                    }
                return true;
            }
            return false;
        }

        //Запись коэффициентов в EEPROM (команда 252).
        public bool C252EEPROMCoefficientWrite()
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть
                int i;
                byte[] data = new byte[sensor.pre+5];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                i = sensor.pre;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0xFC;
                data[i + 3] = 0x00;
                data[i + 4] = GetCRC(data, sensor.pre);//CRC
                for (int j = 0; j < WRITE_COUNT; j++)
                {
                    Thread.Sleep(WRITE_PERIOD);
                    port.Write(data, 0, data.Length);
                    WaitSensorAnswer(sensor.pre, WAIT_TIMEOUT);
                    if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Переключение набора калибровочных коэффициентов для записи в EEPROM (команда 253)
        /// </summary>
        public bool C253SensorCoefficientNumber()
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть
                byte[] data = new byte[sensor.pre + 6];
                for (int i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                data[sensor.pre] = 0x02;
                data[sensor.pre + 1] = (byte)(0x80 | sensor.Addr);
                data[sensor.pre + 2] = 0xFD;
                data[sensor.pre + 3] = 0x1;
                data[sensor.pre + 4] = sensor.SetOfCoef;
                data[sensor.pre + 5] = GetCRC(data, sensor.pre);//CRC
                for (int j = 0; j < WRITE_COUNT; j++)
                {
                        Thread.Sleep(WRITE_PERIOD);
                        port.Write(data, 0, data.Length);
                        WaitSensorAnswer(sensor.pre, WAIT_TIMEOUT);
                        if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                            return true;
                }
            }
            return false;
        }

        //запись параметра токового выхода
        public bool C129WriteCurrenExit()
        {
            if ((port != null) && (SensorConnect))
            {
                ParseReadBuffer(WAIT_TIMEOUT);//отчищаем буфер входных данных, если они есть

                int i;
                byte[] data = new byte[sensor.pre + 6];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0x81;
                data[i + 3] = 0x01;
                data[i + 4] = (byte)((sensor.CurrentExit) & 0xFF);
                data[i + 5] = GetCRC(data, sensor.pre);//CRC
                for (int j = 0; j < WRITE_COUNT; j++)
                {
                    Thread.Sleep(WRITE_PERIOD);
                    port.Write(data, 0, data.Length);
                    WaitSensorAnswer(sensor.pre, WAIT_TIMEOUT);
                    if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                        return true;
                }
            }
            return false;
        }

        //Обработка данных прочитанных с устройства
        //реализован автомат, типа case технологии
        //Возврат: 
        //>=0  без ошибок (код обработанной команды)
        //-1 нет данных в буфере
        //-2 неверный заголовок ответной команды
        //-3 принята неизвестная команда
        //-4 таймаут чтения даных в принятой команде
        public int ParseReadBuffer(int ReadTimeout)
        {
            byte[] indata = new byte[60];
            int rt = 0;
            byte crc=0;

            while (readbuf.Count > 0)
            {
                Application.DoEvents();
                Thread.Sleep(1);
                //if (Program.mainwnd != null)
                //    if (Program.mainwnd.ProcessStop) return -10;

                switch (ReadAvtState)//автомат
                {
                    case 1://поиск преамбулы
                        indata = readbuf.Pop(1);
                        if (indata[0] == 0xFF)//нашли преамбулу
                        {
                            PreambulFinded = true;
                        }
                        else
                        {
                            if (PreambulFinded)//преамбула закончилась
                            {
                                PreambulFinded = false;
                                if (indata[0] == 0x06)//читаем заголовок
                                {
                                    ReadAvtState = 2;
                                }
                                else
                                {
                                    if (Program.txtlog != null)
                                        Program.txtlog.WriteLineLog("HART:Не верный заголовок ответной команды: " + indata[0], 1);
                                    return -2;//неверный заголовок ответной команды
                                }
                            }
                            else
                            {
                               /* if (Program.txtlog != null)
                                    Program.txtlog.WriteLineLog("HART:Не обнаружены преамбула 0xFF. Cчитанные данные: " + indata[0], 1);*/
                            }
                        }
                        break;
                    case 2://читаем адрес
                        indata = readbuf.Pop(1);
                        if ((indata[0] & 0x80) == 0x80)//адрес
                        {
                            ReadAvtState = 3;
                            Adress = (byte)(indata[0] & 0x7F);
                            crc = (byte)(0x06 ^ indata[0]);
                        }
                        else//не верный адрес, ждем следующую команду
                        {
                            ReadAvtState = 1;
                        }
                        break;
                    case 3://читаем код команды
                        indata = readbuf.Pop(1);
                        CommandCod = indata[0];
                        crc = (byte)(crc ^ indata[0]);
                        ReadAvtState = 4;
                        break;
                    case 4://читаем количество байт в комманде
                        indata = readbuf.Pop(1);
                        CountByteToRead = indata[0]+1;//+CRC
                        crc = (byte)(crc ^ indata[0]);
                        ReadAvtState = 5;
                        rt = 0;
                        break;
                    case 5:
                        if (readbuf.Count >= CountByteToRead)
                        {//+CRC 1байт
                            indata = readbuf.Pop(CountByteToRead-1);//читаем комманду
                            byte[] incrc = readbuf.Pop(1);//читаем CRC от модема
                            //расчет CRC
                            /*for(int c=0;c<CountByteToRead-1;c++)
                            {
                                crc = (byte)(crc ^ indata[c]);
                            }
                            if( crc != incrc)//
                            {
                                ReadAvtState = 1;
                                if (Program.txtlog!= null)
                                Program.txtlog.WriteLineLog(string.Format("HART: Неверная контрольная сумма ответной команды {0}: (расчет {1} , ответ {2})", CommandCod, crc, incrc), 1);
                                return -6;
                            }*/

                            sensor.state = (ushort)((indata[1] << 8) | indata[0]);//читаем состояние исполнения команды
                            if ((sensor.state & 0x00FF) != 0) //первый байт статуса
                            {
                                ReadAvtState = 1;
                                if ((sensor.state & 0x00FF) == 0x40)
                                {
                                    return -8;//команда  не найдена
                                }

                                if (Program.txtlog!= null)
                                    Program.txtlog.WriteLineLog(string.Format("HART: Ошибка выполнения команды {0}. Статус {1}", CommandCod, sensor.state), 1);
                                return -7;
                            }
                            if ((sensor.state & 0xFF00) != 0) //второй байт статуса
                            {
                                ReadAvtState = 1;
                                //if (Program.txtlog != null)
                                //    Program.txtlog.WriteLineLog(string.Format("HART: Неисправность прибора. Команда {0}. Статус {1}", CommandCod, sensor.state), 1);
                                //return -8;  04.08.2021 При не нулевом статусе продолжаем парсинг
                            }
                            switch (CommandCod)
                            {
                                case 0x0://Запрос уникального идентификатора (команда 0)
                                    if (!ReadCommand0(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверные данные в ответной команде
                                    }
                                    break;
                                case 0x1://Считать первичную переменную (команда 1)
                                    break;
                                case 0x2://Считать ток и процент диапазона (команда 2)
                                    break;
                                case 0x3://Чтение измеренных значений (4 переменных) (команда 3)
                                    if (!ReadCommand3(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверные данные в ответной команде
                                    }
                                    break;
                                case 0x6://Записать адрес устройства (команда 6)
                                    if (!ReadCommand6(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверные данные в ответной команде
                                    }
                                    sensor.state = (ushort)((indata[1] << 8) | indata[0]);//состояние по команде перехода в сервесный режим
                                    sensor.Addr = indata[2];
                                    break;
                                case 0x0B://Считать уникальный идентификатор, связанный с тэгом(команда 11)
                                    break;
                                case 0x0C://Считать сообщение (команда 12)
                                    if (!ReadCommand12(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверные данные в ответной команде
                                    }
                                    break;
                                case 0x0D://Считать тэг, дескриптор, дату (команда 13)
                                    if (!ReadCommand13(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверные данные в ответной команде
                                    }
                                    break;
                                case 0x0E://Чтение серийного номера и параметров приемника давления (команда 14)
                                    if (!ReadCommand14(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверные данные в ответной команде
                                    }
                                    break;
                                case 0x0F://Чтение ВПИ, НПИ (команда 15)
                                    if (!ReadCommand15(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверные данные в ответной команде
                                    }
                                    break;
                                case 0x16://Прочитать Final Assembly number (команда 16)
                                    break;
                                case 0x11://Записать сообщение (команда 17)
                                    break;
                                case 0x12://Записать тэг, дескриптор, дату (команда 18)
                                    break;
                                case 0x13://Записать Final Assembly Number (команда 19)
                                    break;
                                case 0x22://Записать время демпфирования (команда 34)
                                    if (!ReadCommand34(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверные данные в ответной команде
                                    }
                                    break;
                                case 0x23://Запись ВПИ и НПИ (команда 35)
                                    if (!ReadCommand35(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверные данные в ответной команде
                                    }
                                    break;
                                case 0x26://Сброс флага изменения конфигурации (команда 38)
                                    break;
                                case 0x28://Вход/выход в режим фиксированного тока (команда 40)
                                    if (!ReadCommand40(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверные данные в ответной команде
                                    }
                                    break;
                                case 0x29://Запрос на выполнение самодиагностики (команда 41)
                                    break;
                                case 0x2A://Перезагрузка датчика (команда 42)
                                    if (!ReadCommand42(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверные данные в ответной команде
                                    }
                                    break;
                                case 0x2B://Установить ноль первичной переменной (коррекция нуля от монтажного положения) (команда 43)
                                    if (!ReadCommand43(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверные данные в ответной команде
                                    }
                                    break;
                                case 0x2C://Установить единицы измерения первичной переменной  (команда 44)
                                    break;
                                case 0x2D://Коррекция нуля ЦАП (команда 45)
                                    if (!ReadCommand45(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверные данные в ответной команде
                                    }
                                    break;
                                case 0x2E://Коррекция коэффициента усиления ЦАП (команда 46)
                                    if (!ReadCommand46(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверные данные в ответной команде
                                    }
                                    break;
                                case 0x2F://Установка функции преобразования первичной переменной (команда 47)
                                    break;
                                case 0x30://Чтение дополнительного статуса (команда 48)
                                    break;
                                case 0x31://Запись серийного номера ПД (команда 49)
                                    if (!ReadCommand49(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверные данные в ответной команде
                                    }
                                    break;
                                case 0x3B://Записать кол-во преамбул (команда 59)
                                    break;
                                case 0x80:// Считать параметры токового выхода(команда 128).
                                    if (!ReadCommand128(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверные данные в ответной команде
                                    }
                                    break;
                                case 0x81://Записать параметры токового выхода (команда 129)
                                    if (!ReadCommand129(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверные данные в ответной команде
                                    }
                                    break;
                                case 0x82://Считать параметры защиты работы магнитной кнопки (команда 130)
                                    break;
                                case 0x83://Записать параметры защиты работы магнитной кнопки (команда 131)
                                    break;
                                case 0x84://Калибровка ВПИ (команда 132)
                                    break;
                                case 0x85://Калибровка НПИ (команда 133)
                                    break;
                                case 0x88://Восстановление заводских настроек прибора (команда 136)
                                    break;
                                case 0x89://Считать параметры критической ошибки (уровни аварийной сигнализации) (команда 137)
                                    break;
                                case 0x8A://Записать параметры критической ошибки (уровни аварийной сигнализации) (команда 138)
                                    break;
                                case 0x8C://Считать данные о модели приемника давления и типа давления (команда 140)
                                    if (!ReadCommand140(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверные данные в ответной команде
                                    }

                                    break;
                                case 0x8D://Запись данных о языке вывода названий меню – только для исполнения ЖК-2 (команда 141)
                                    break;
                                case 0x8E://Чтение данных о языке вывода названий меню – только для исполнения ЖК-2 (команда 142)
                                    break;
                                case 0x8F://Коррекция нуля путем ввода значения атмосферного давления(команда 143)
                                    if (!ReadCommand143(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверные данные в ответной команде
                                    }
                                    break;
                                case 0x90://Запись данных о вкл/выкл условных единицах – только для исполнения ЖК-2 (команда 144)
                                    break;
                                case 0x91://Чтение данных о вкл/выкл условных единицах – только для исполнения ЖК-2 (команда 145)
                                    break;
                                case 0x92://Запись ВПИ, НПИ, названия условных единиц – только для исполнения ЖК-2  (команда 146)
                                    break;
                                case 0x93://Чтение ВПИ, НПИ, названия условных единиц – только для исполнения ЖК-2(команда 147)
                                    break;
                                case 0x94://Сохранение текущей конфигурации прибора(команда 148)
                                    break;

                                case 0x97://Запись дополнительных параметров датчика(команда 151)
                                    if (!ReadCommand151(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверная структура данных в ответной команде
                                    }
                                    break;
                                case 0x98://Чтение дополнительных параметров датчика(команда 152)
                                    if (!ReadCommand152(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверная структура данных в ответной команде
                                    }
                                    break;
                                case 0xC8://Запись калибровочных коэффициентов для датчика давления double (команда 200)
                                    if (!ReadCommand200(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверная структура данных в ответной команде
                                    }
                                    break;
                                case 0xC9://Чтение калибровочных коэффициентов для датчика давления double (команда 201)
                                    if (!ReadCommand201(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверная структура данных в ответной команде
                                    }
                                    break;
                                case 0xF1://Запись данных о модели приемника давления, тип давления (команда 241)
                                    if (!ReadCommand241(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверная структура данных в ответной команде
                                    }
                                    break;
                                case 0xF5://Перевод датчика в сервисный режим ( команда 245)
                                    sensor.state = (ushort)((indata[1] << 8) | indata[0]);//состояние по команде перехода в сервесный режим
                                    if (sensor.state != 0)
                                    {
                                        ReadAvtState = 1;
                                        return -5;
                                    }
                                    break;
                                case 0xF6://Чтение EEPROM (команда 246)
                                    break;
                                case 0xF7://Чтение кодов АЦП (команда 247)
                                    break;
                                case 0xF8://Запись серийного номера ДД (команда 248)
                                    break;
                                case 0xF9://Запись верхнего и нижнего пределов ПД, минимального диапазона (команда 249)
                                    if (!ReadCommand249(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверная структура данных в ответной команде
                                    }
                                    break;
                                case 0xFA://Запись калибровочных коэффициентов для датчика давления (команда 250)
                                    if (!ReadCommand250(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверная структура данных в ответной команде
                                    }
                                    break;
                                case 0xFB://Чтение калибровочных коэффициентов (команда 251)
                                    if (!ReadCommand251(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверная структура данных в ответной команде
                                    }
                                    break;
                                case 0xFC://Запись калибровочных коэффициентов из RAM в EEPROM (команда 252)
                                    if (!ReadCommand252(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверная структура данных в ответной команде
                                    }
                                    break;
                                case 0xFD://Переключение набора калибровочных коэффициентов для записи в EEPROM ( команда 253)
                                    if (!ReadCommand253(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверные данные в ответной команде
                                    }
                                    break;
                                default:
                                    ReadAvtState = 1;
                                    if (Program.txtlog != null)
                                        Program.txtlog.WriteLineLog("HART: Ответная команда не идентифицирована " + CommandCod.ToString(), 1);
                                    return -3;//неизвестная комманда
                            }
                            ReadAvtState = 1;
                            if (Program.txtlog != null)
                                Program.txtlog.WriteLineLog("HART:Выполнено чтение ответного слова на команду " + CommandCod.ToString(), 0);
                            return CommandCod;
                        }
                        else
                        {
                            rt++;
                            if (rt>ReadTimeout)//превышен таймаут ожидания
                            {
                                if (Program.txtlog != null)
                                    Program.txtlog.WriteLineLog("HART:Таймаут чтения команды! Байт в буфере:" + readbuf.Count + " Ожидание:" + CountByteToRead, 1);
                                ReadAvtState = 1;
                                readbuf.Clear();
                                return -4;
                            }
                            Thread.Sleep(5);
                        }
                        break;
                }
            }
            return -1;
        }


        //Поток опроса сенсора 
        //Прочитанные данные записываются в буфер для последующей обработки
        //Поток останавливается при завершении сенса связи с сенсором
        static void SerialReadThread()
        {
            while ((port != null) && SensorConnect)
            {
                try
                {
                    while (port.BytesToRead > 0)
                    {
                        int indata = port.ReadByte();
                        readbuf.PushByte((byte)indata);
                    }
                    Thread.Sleep(1);
                }
                catch (TimeoutException)
                {
                    if (Program.txtlog != null)
                        Program.txtlog.WriteLineLog("HART: Критическая ошибка чтения порта датчика! Не прочитано байт: " + port.BytesToRead.ToString(), 1);
                }
            }
        }


        public void ClosePort()
        {
            SensorConnect = false;
            if (port != null)
            {
                if (ReadThread != null)
                    ReadThread.Abort(0);
                port.Close();
                readbuf.Clear();
            }

        }
/*        public void OpenPort(string PortName)
        {
            if (port != null)
            {
                    port.PortName = PortName;
                    port.BaudRate = 1200;
                    port.DataBits = 8;
                    port.Parity = Parity.Odd;
                    port.StopBits = StopBits.One;
                    port.ReadTimeout = 1000;
                    port.WriteTimeout = 1000;
                    port.DtrEnable = true;
                    port.RtsEnable = true;
                    port.Open();
                    readbuf.Clear();
                    SensorConnect = true;
                    ReadThread = new Thread(SerialReadThread);
                    ReadThread.Priority = ThreadPriority.AboveNormal;
                    ReadThread.Start();
            }

        }*/

        //Соединение с датчиком
        public int Connect(string PortName, int BaudRate, int DataBits, int StopBits, int Parity)
        {
            if (SensorConnect)
            {
                return 1;
            }
            try
            {
                port.PortName = PortName;
                port.BaudRate = BaudRate;
                port.DataBits = DataBits;
                port.StopBits = (StopBits)StopBits;
                port.Parity = (Parity)Parity;
                port.ReadTimeout = 1000;
                port.WriteTimeout = 1000;
                port.DtrEnable = true;
                port.RtsEnable = true;
                port.Open();

                readbuf.Clear();
                SensorConnect = true;
                ReadThread = new Thread(SerialReadThread);
                ReadThread.Priority = ThreadPriority.Highest;
                ReadThread.Start();

                return 0;
            }
            catch
            {
                return -1;
            }
        }

        public int DisConnect()
        {
            if (SensorConnect)
            {
                ReadThread.Abort();
                ReadThread = null;
                SensorConnect = false;
                port.Close();
                return 0;
            }
            else
            {
                return 1;
            }
        }

        //Поиск наличия датчиков в группе ig
        public int FindSensorGroup(int ig)
        {
            for (int i = 0; i < sensorList.Count; i++)
            {
                if (sensorList[i].Group == ig)
                    return i;
            }
            return -1;
        }




        //Ответ на комманду Запрос уникального идентификатора (команда 0)
        private bool ReadCommand0(byte addr, byte[] indata)
        {
            try
            {
                SensorID id = new SensorID(addr, COEFF_COUNT);
                id.Channal = SelSensorChannal;
                id.Group = 1;// (int)(SelSensorChannal / MaxSensorOnLevel) + 1;
                id.state = (ushort)((indata[1] << 8) | indata[0]);
                //if (id.state != 0) return false;

                id.devCode = indata[3];
                id.devType = indata[4];
                id.pre = 7;// indata[5];
                id.v1 = indata[6];
                id.v2 = indata[7];
                id.v3 = indata[8];
                id.v4 = indata[9];
                id.flag = indata[10];
                id.uni = (UInt32)((indata[11] << 16) | (indata[12] << 8) | indata[13]);
                sensorList.Add(id);
                return true;
            }
            catch
            {
                return false;
            }
        }
        //Ответ на комманду Чтение измеренных значений (4 переменных) (команда 3)
        private bool ReadCommand3(int addr, byte[] indata)
        {
            try
            {
                int tmp;

                tmp = (indata[2] << 24) | (indata[3] << 16) | (indata[4] << 8) | indata[5];
                sensor.OutCurrent = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

                sensor.PressueUnit = indata[6];
                tmp = (indata[7] << 24) | (indata[8] << 16) | (indata[9] << 8) | indata[10];
                sensor.Pressure = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

                sensor.VoltageUnit = indata[11];
                tmp = (indata[12] << 24) | (indata[13] << 16) | (indata[14] << 8) | indata[15];
                sensor.OutVoltage = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

                sensor.ResistanceUnit = indata[16];
                tmp = (indata[17] << 24) | (indata[18] << 16) | (indata[19] << 8) | indata[20];
                sensor.Resistance = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

                sensor.TemperatureUnit = indata[21];
                tmp = (indata[22] << 24) | (indata[23] << 16) | (indata[24] << 8) | indata[25];
                sensor.Temperature = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

                sensorList[SelSensorIndex] = sensor;
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Ответ на команду запись адреса (команда 6)
        private bool ReadCommand6(int addr, byte[] indata)
        {
            try
            {
                sensor.Addr = indata[2];
                sensorList[SelSensorIndex] = sensor;
                return true;
            }
            catch
            {
                return false;
            }
        }


        //Считать сообщение (команда 12)
        private bool ReadCommand12(int addr, byte[] indata)
        {
            try
            {
                int j = 2;
                for (int i = 0; i < 24; i++)
                {
                    sensor.message[i] = indata[j];
                    j++;
                }
                Array.Reverse(sensor.message);//инвертируем порядок 
                sensorList[SelSensorIndex] = sensor;
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Считать тэг, дескриптор, дату (команда 13)
        private bool ReadCommand13(int addr, byte[] indata)
        {
            try
            {
                int j = 2;
                for (int i = 0; i < 6; i++)
                {
                    sensor.teg[i] = indata[j];
                    j++;
                }
                for (int i = 0; i < 12; i++)
                {
                    sensor.desc[i] = indata[j];
                    j++;
                }
                Array.Reverse(sensor.teg);//инвертируем порядок 
                Array.Reverse(sensor.desc);//инвертируем порядок 

                sensor.data = (UInt32)((indata[j] << 16) | (indata[j + 1] << 8) | indata[j + 2]);

                sensorList[SelSensorIndex] = sensor;
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Чтение серийного номера и параметров приемника давления (команда 14)
        private bool ReadCommand14(int addr, byte[] indata)
        {
            try
            {
                int tmp;

                sensor.SerialNumber = (UInt32)((indata[2] << 16) | (indata[3] << 8) | indata[4]);
                sensor.MesUnit = indata[5];
                tmp = (indata[6] << 24) | (indata[7] << 16) | (indata[8] << 8) | indata[9];
                sensor.UpLevel = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

                tmp = (indata[10] << 24) | (indata[11] << 16) | (indata[12] << 8) | indata[13];
                sensor.DownLevel = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

                tmp = (indata[14] << 24) | (indata[15] << 16) | (indata[16] << 8) | indata[17];
                sensor.MinLevel = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

                sensorList[SelSensorIndex] = sensor;

                return true;
            }
            catch
            {
                return false;
            }
        }

        //Чтение ВПИ, НПИ (команда 15)
        private bool ReadCommand15(int addr, byte[] indata)
        {
            try
            {


                int tmp;

                int yy = indata[2];//alarm select code
                int bb = indata[3];//Transfer function code
                int edMes = indata[4];//Transfer function code


                tmp = (indata[5] << 24) | (indata[6] << 16) | (indata[7] << 8) | indata[8];
                sensor.VPI = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);
                tmp = (indata[9] << 24) | (indata[10] << 16) | (indata[11] << 8) | indata[12];
                sensor.NPI = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);
                tmp = (indata[13] << 24) | (indata[14] << 16) | (indata[15] << 8) | indata[16];
                sensor.DempfTime = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);//время демпфирования

                int cc = indata[17];//Write protect code
                int dd = indata[18];//private label distributor code

                sensorList[SelSensorIndex] = sensor;

                return true;
            }
            catch
            {
                return false;
            }
        }

        //Запись времени демпфирования (команда 34)
        private bool ReadCommand34(int addr, byte[] indata)
        {
            try
            {
                int tmp;

                tmp = (indata[2] << 24) | (indata[3] << 16) | (indata[4] << 8) | indata[5];
                sensor.DempfTime = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

                sensorList[SelSensorIndex] = sensor;
                return true;
            }   
            catch
            {
                return false;
            }
        }

        //Запись ВПИ НПИ(команда 35)
        private bool ReadCommand35(int addr, byte[] indata)
        {
            try
            {
                int tmp;

                tmp = (indata[2] << 24) | (indata[3] << 16) | (indata[4] << 8) | indata[5];
                sensor.VPI = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);
                tmp = (indata[6] << 24) | (indata[7] << 16) | (indata[8] << 8) | indata[9];
                sensor.NPI = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

                sensorList[SelSensorIndex] = sensor;
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Режим фиксированного тока (команда 40)
        private bool ReadCommand40(int addr, byte[] indata)
        {
            try
            {
                int tmp;
                float Current;
                tmp = (indata[2] << 24) | (indata[3] << 16) | (indata[4] << 8) | indata[5];
                Current = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);
                return true;
                
            }
            catch
            {
                return false;
            }
        }

        //Перезагрузка датчика (команда 42).
        private bool ReadCommand42(int addr, byte[] indata)
        {
            try
            {
                return true;
            }
            catch
            {
                return false;
            }
        }


        //Установить ноль первичной переменной(коррекция нуля от монтажного положения) (команда 43).
        private bool ReadCommand43(int addr, byte[] indata)
        {
            try
            {
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Коррекция нуля путем ввода значения атмосферного давления (команда 143)
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="indata"></param>
        /// <returns></returns>
        private bool ReadCommand143(int addr, byte[] indata)
        {
            try
            {
                return true;
            }
            catch
            {
                return false;
            }
        }


        //Коррекция нуля ЦАП (команда 45)
        private bool ReadCommand45(int addr, byte[] indata)
        {
            try
            {
                int tmp;
                float Current;
                tmp = (indata[2] << 24) | (indata[3] << 16) | (indata[4] << 8) | indata[5];
                Current = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);
                return true;
            }
            catch
            {
                return false;
            }
        }
        //Коррекция коэффициента усиления ЦАП (команда 46)
        private bool ReadCommand46(int addr, byte[] indata)
        {
            try
            {
                int tmp;
                float Current;
                tmp = (indata[2] << 24) | (indata[3] << 16) | (indata[4] << 8) | indata[5];
                Current = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Ответ на команду запись серийного номера ПД (команда 49)
        private bool ReadCommand49(int addr, byte[] indata)
        {
            try
            {
                    sensor.SerialNumber = (UInt32)((indata[2] << 16) | (indata[3] << 8) | indata[4]);
                    sensorList[SelSensorIndex] = sensor;
                    return true;
            }
            catch
            {
                return false;
            }
        }

        //Ответ на команду чтение токового выхода (команда 128)
        private bool ReadCommand128(int addr, byte[] indata)
        {
            try
            {
                    sensor.CurrentExit = indata[2];
                    sensorList[SelSensorIndex] = sensor;
                    return true;
            }
            catch
            {
                return false;
            }
        }
        //Ответ на команду запись токового выхода (команда 129)
        private bool ReadCommand129(int addr, byte[] indata)
        {
            try
            {
                    sensor.CurrentExit = indata[2];
                    sensorList[SelSensorIndex] = sensor;
                    return true;
            }
            catch
            {
                return false;
            }
        }

        //Ответ на команду чтение модели приемника давления (команда 140)
        private bool ReadCommand140(int addr, byte[] indata)
        {
            try
            {
                sensor.PressureType = indata[2];
                for (int i = 0; i < 5; i++)//5 символов
                {
                    sensor.PressureModel[i] = Convert.ToChar(indata[i + 3]);
                }
                sensorList[SelSensorIndex] = sensor;

                return true;
            }
            catch
            {
                return false;
            }
        }

        //Ответ на команду запись модели приемника давления (команда 241)
        private bool ReadCommand241(int addr, byte[] indata)
        {
            try
            {
                sensor.PressureType = indata[2];
                for (int i = 0; i < 5; i++)//5 символов
                {
                    if (indata.Length > (i + 3))
                    {
                        sensor.PressureModel[i] = Convert.ToChar(indata[i + 3]); //(char)indata[i + 3];
                    }
                    else
                    {
                        sensor.PressureModel[i] = ' ';
                    }
                }
                sensorList[SelSensorIndex] = sensor;
                return true;
            }
            catch
            {
                return false;
            }
        }

        //ответ на запись верхнего и нижнего пределов ПД, минимального диапазона (команда 249)
        //2 байта статуса(сообщение об ошибках, 00 00 - ок)
        //1 байт – единицы измерения, 0x0C – кПа, 0xED – МПа
        //4 байта – верхняя граница диапазона ПД, тип float
        //4 байта – нижняя граница диапазона ПД, тип float
        //4 байта – минимальный диапазон ПД, тип float
        private bool ReadCommand249(int addr, byte[] indata)
        {
            try
            {
                int tmp;
                if (indata.Length > 14)
                {
                    sensor.MesUnit = indata[2];
                    tmp = (indata[3] << 24) | (indata[4] << 16) | (indata[5] << 8) | indata[6];
                    sensor.UpLevel = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

                    tmp = (indata[7] << 24) | (indata[8] << 16) | (indata[9] << 8) | indata[10];
                    sensor.DownLevel = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

                    tmp = (indata[11] << 24) | (indata[12] << 16) | (indata[13] << 8) | indata[14];
                    sensor.MinLevel = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

                    sensorList[SelSensorIndex] = sensor;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// Запись дополнительных параметров датчика (команда 151)
        /// </summary>
        private bool ReadCommand151(int addr, byte[] indata)
        {
            try
            {
                sensor.CurrentExit = indata[2];

                sensor.qrc = indata[2];  //колличество наборов коэф
                sensor.UGain1 = indata[3];
                sensor.UPower1 = indata[4];
                sensor.UGain2 = indata[5];
                sensor.UPower2 = indata[6];
                sensor.ExtTemp = indata[7];
                sensor.Param0 = indata[8];
                sensor.Param1 = indata[9];
                sensor.Param2 = indata[10];
                sensor.Param3 = indata[11];
                sensor.TPower1 = indata[12];
                sensor.TPower2 = indata[13];
                Int32 tmp = (indata[14] << 24) | (indata[15] << 16) | (indata[16] << 8) | indata[17];
                sensor.TranspPoint = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);
                sensorList[SelSensorIndex] = sensor;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Чтение дополнительных параметров датчика (команда 152)
        /// </summary>
        private bool ReadCommand152(int addr, byte[] indata)
        {
            try
            {
                sensor.CurrentExit = indata[2];

                sensor.qrc = indata[2];  //колличество наборов коэф
                sensor.UGain1 = indata[3];
                sensor.UPower1 = indata[4];
                sensor.UGain2 = indata[5];
                sensor.UPower2 = indata[6];
                sensor.ExtTemp = indata[7];
                sensor.Param0 = indata[8];
                sensor.Param1 = indata[9];
                sensor.Param2 = indata[10];
                sensor.Param3 = indata[11];
                sensor.TPower1 = indata[12];
                sensor.TPower2 = indata[13];
                Int32 tmp = (indata[14] << 24) | (indata[15] << 16) | (indata[16] << 8) | indata[17];
                sensor.TranspPoint = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);
                sensorList[SelSensorIndex] = sensor;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Запись калибровочных коэффициентов double(команда 200).
        /// </summary>
        private bool ReadCommand200(int addr, byte[] indata)
        {
            try
            {
                byte[] tmp = new byte[8];
                int Number = indata[2];
                int c = 3;
                if ((Number >= 0) && (Number <= 5))
                {
                    for (int i = 0; i < 8; i++)
                    {
                        tmp[7 - i] = indata[c + i];
                    }
                    c = c + 8;
                    sensor.DCoefficient[4 * Number] = BitConverter.ToDouble(tmp, 0);
                    for (int i = 0; i < 8; i++)
                    {
                        tmp[7 - i] = indata[c + i];
                    }
                    c = c + 8;
                    sensor.DCoefficient[4 * Number + 1] = BitConverter.ToDouble(tmp, 0);
                    for (int i = 0; i < 8; i++)
                    {
                        tmp[7 - i] = indata[c + i];
                    }
                    c = c + 8;
                    sensor.DCoefficient[4 * Number + 2] = BitConverter.ToDouble(tmp, 0);
                    for (int i = 0; i < 8; i++)
                    {
                        tmp[7 - i] = indata[c + i];
                    }
                    sensor.DCoefficient[4 * Number + 3] = BitConverter.ToDouble(tmp, 0);

                    sensorList[SelSensorIndex] = sensor;

                    return true;
                }
                else
                {
                    //ошибка (Неверные данные)
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Чтение калибровочных коэффициентов double(команда 201).
        /// </summary>
        private bool ReadCommand201(int addr, byte[] indata)
        {
            try
            {
                byte[] tmp = new byte[8];
                int Number = indata[2];
                int c = 3;
                if ((Number >= 0) && (Number <= 5))
                {
                    for (int i = 0; i < 8; i++)
                    {
                        tmp[7-i] = indata[c + i];
                    }
                    c = c + 8;
                    sensor.DCoefficient[4 * Number] = BitConverter.ToDouble(tmp, 0);
                    for (int i = 0; i < 8; i++)
                    {
                        tmp[7-i] = indata[c + i];
                    }
                    c = c + 8;
                    sensor.DCoefficient[4 * Number+1] = BitConverter.ToDouble(tmp, 0);
                    for (int i = 0; i < 8; i++)
                    {
                        tmp[7-i] = indata[c + i];
                    }
                    c = c + 8;
                    sensor.DCoefficient[4 * Number+2] = BitConverter.ToDouble(tmp, 0);
                    for (int i = 0; i < 8; i++)
                    {
                        tmp[7-i] = indata[c + i];
                    }
                    sensor.DCoefficient[4 * Number+3] = BitConverter.ToDouble(tmp, 0);

                    sensorList[SelSensorIndex] = sensor;

                    return true;
                }
                else
                {
                    //ошибка (Неверные данные)
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        //Запись калибровочных коэффициентов для датчика давления (команда 250)
        private bool ReadCommand250(int addr, byte[] indata)
        {
            try
            {
                int tmp;
                int Number = indata[2];
                if ((Number >= 0) && (Number <= 5))
                {
                    tmp = (indata[3] << 24) | (indata[4] << 16) | (indata[5] << 8) | indata[6];
                    sensor.Coefficient[4 * Number] = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

                    tmp = (indata[7] << 24) | (indata[8] << 16) | (indata[9] << 8) | indata[10];
                    sensor.Coefficient[4 * Number + 1] = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

                    tmp = (indata[11] << 24) | (indata[12] << 16) | (indata[13] << 8) | indata[14];
                    sensor.Coefficient[4 * Number + 2] = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

                    tmp = (indata[15] << 24) | (indata[16] << 16) | (indata[17] << 8) | indata[18];
                    sensor.Coefficient[4 * Number + 3] = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

                    sensorList[SelSensorIndex] = sensor;

                    return true;
                }
                else
                {
                    //ошибка (Неверные данные)
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        //Чтение калибровочных коэффициентов(команда 251).
        private bool ReadCommand251(int addr, byte[] indata)
        {
            try
            {
                int tmp;

                int Number = indata[2];
                if ((Number >= 0) && (Number <= 5))
                {
                    tmp = (indata[3] << 24) | (indata[4] << 16) | (indata[5] << 8) | indata[6];
                    sensor.Coefficient[4 * Number] = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

                    tmp = (indata[7] << 24) | (indata[8] << 16) | (indata[9] << 8) | indata[10];
                    sensor.Coefficient[4 * Number + 1] = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

                    tmp = (indata[11] << 24) | (indata[12] << 16) | (indata[13] << 8) | indata[14];
                    sensor.Coefficient[4 * Number + 2] = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

                    tmp = (indata[15] << 24) | (indata[16] << 16) | (indata[17] << 8) | indata[18];
                    sensor.Coefficient[4 * Number + 3] = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

                    sensorList[SelSensorIndex] = sensor;

                    return true;
                }
                else
                {
                    //ошибка (Неверные данные)
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        //Ответ на команду запись коэффициентов в EEPROM (команда 252)
        private bool ReadCommand252(int addr, byte[] indata)
        {
            try
            {
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool ReadCommand253(int addr, byte[] indata)
        {
            try
            {
                if (sensor.SetOfCoef == indata[2])
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }


    /// <summary>
    /// Класс формирует FIFO буфер для чтения данных с порта
    /// </summary>
    public class FastFifo
    {
        private List<Byte> mi_FifoData = new List<Byte>();

        /// <summary>
        /// Get the count of bytes in the Fifo buffer
        /// </summary>
        public int Count
        {
            get
            {
                lock (mi_FifoData)
                {
                    return mi_FifoData.Count;
                }
            }
        }

        /// <summary>
        /// Clears the Fifo buffer
        /// </summary>
        public void Clear()
        {
            lock (mi_FifoData)
            {
                mi_FifoData.Clear();
            }
        }

        /// <summary>
        /// Append data to the end of the fifo
        /// </summary>
        public void Push(Byte[] u8_Data)
        {
            lock (mi_FifoData)
            {
                // Internally the .NET framework uses Array.Copy() which is extremely fast
                mi_FifoData.AddRange(u8_Data);
//                mi_FifoData.Add();
            }
        }

        /// <summary>
        /// Append 1 byte data to the end of the fifo
        /// </summary>
        public void PushByte(Byte u8_Data)
        {
            lock (mi_FifoData)
            {
                // Internally the .NET framework uses Array.Copy() which is extremely fast
                mi_FifoData.Add(u8_Data);
            }
        }


        /// <summary>
        /// Get data from the beginning of the fifo.
        /// returns null if s32_Count bytes are not yet available.
        /// </summary>
        public Byte[] Pop(int s32_Count)
        {
            lock (mi_FifoData)
            {
                if (mi_FifoData.Count < s32_Count)
                    return null;

                // Internally the .NET framework uses Array.Copy() which is extremely fast
                Byte[] u8_PopData = new Byte[s32_Count];
                mi_FifoData.CopyTo(0, u8_PopData, 0, s32_Count);
                mi_FifoData.RemoveRange(0, s32_Count);
                return u8_PopData;
            }
        }

        /// <summary>
        /// Gets a byte without removing it from the Fifo buffer
        /// returns -1 if the index is invalid
        /// </summary>
        public int PeekAt(int s32_Index)
        {
            lock (mi_FifoData)
            {
                if (s32_Index < 0 || s32_Index >= mi_FifoData.Count)
                    return -1;

                return mi_FifoData[s32_Index];
            }
        }
    }
}
