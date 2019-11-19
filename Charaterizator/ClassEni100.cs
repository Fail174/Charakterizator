using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.Collections;
using System.Windows.Forms;


//namespace ENI100
namespace Charaterizator
{
    public struct SensorID
    {
        //        const int COEFF_COUNT = 24;//число коэффициентов
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

        public byte CurrentExit;//токовый выход (0 - 4мА, 1 - 20мА)

        public byte PressureType;
        public char[] PressureModel;

        public float[] Coefficient;
        public SensorID(byte a)
        {
            message = new byte[24];
            desc = new byte[12];
            teg = new byte[6];
            Coefficient = new float[24];
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
    }
    public string GetdevType()
        {
            switch (devType)
            { 
                case 0xCC:
                    return "ЭнИ - 100";
                case 0xCD:
                    return "ЭнИ-12";
                case 0xCE:
                    return "ЭнИ-100-ЖК2";
                case 0xCF:
                    return "ЭнИ-12М";
                default:
                    return "не определено";
            }
        }
        public string GetUnit()  // 
        {
            switch (MesUnit)
            {
                case 0x0C:
                    return "кПа";
                case 0xED:
                    return "МПа";
                default:
                    return "";
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
        const int WAIT_TIMEOUT = 300;//таймаут ожидания ответа от датчика

        static SerialPort port = null;
        static FastFifo readbuf = new FastFifo();
        static bool SensorConnect = false;
        public List<SensorID> sensorList = new List<SensorID>();//список обнаруженных датчиков
        public SensorID sensor;//текущий обслуживаемый датчик
        public int SelSensorChannal = 0;
        //        public SensorTeg steg = new SensorTeg(6, 12);
        private Thread ReadThread;
        private bool PreambulFinded=false;

        private int ReadAvtState = 1;       //состояние автомата
        private int CommandCod = -1;          //код ответной команды
        private int CountByteToRead = 0;    //количество байт в команде
        private byte Adress = 0;               //адрес устройства

        public ClassEni100()
        {
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
            }
            return c >= timeout;
        }

        //поиск датчика в списке по номеру канала
        public bool SelectSensor(int index)
        {
            for (int i = 0; i < sensorList.Count; i++)
            {
                if (sensorList[i].Channal == index)
                {//датчик в канале найден
                    sensor = sensorList[i];
                    SelSensorChannal = index;
                    return true;
                }
            }
            return false;

/*            if (sensorList.Count > index)
            {

                sensor = sensorList[index];
                return true;
            }
            else
            {
                return false;
            }*/
        }
        //чтение списка подключенных датчиков
        public bool SeachSensor(int ch)
        {
            if((port != null)&&(SensorConnect))
            {
                Thread.Sleep(200);

                byte[] data = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x02, 0x80, 0x00, 0x00, 0x82 };
                //sensorList.Clear();
                /*                for (int i = 0; i < 15; i++)
                                {
                                    data[6] = (byte)(0x80 | i);
                                    data[data.Length - 1] = GetCRC(data, 5);
                                    port.Write(data, 0, data.Length);
                                    WaitSensorAnswer(20, WAIT_TIMEOUT);
                                    if (ParseReadBuffer(WAIT_TIMEOUT)>=0)//выходим при обнаружении первого попавшегося датчика
                                    {
                                        break;
                                    }
                                }*/
                //поиск производим только по 0му адресу 30.10.2019
                SelSensorChannal = ch;
                data[data.Length - 1] = GetCRC(data, 5);
                port.Write(data, 0, data.Length);
                WaitSensorAnswer(20, WAIT_TIMEOUT);
//                for (int i = 0; i < 3; i++)
 //               {
                    if (ParseReadBuffer(WAIT_TIMEOUT) >= 0)
                    {
                        return true;
                    }
/*                    else
                    {
                        return false;
                    }*/
//                }
            }
            return false;
        }
        
        //Чтение ТЕГа выбранного датчика
        public bool TegRead()
        {
            if ((port != null) && (SensorConnect))
            {
                Thread.Sleep(200);

                byte[] data = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x02, 0x80, 0x0D, 0x00, 0x8F };
                data[sensor.pre + 1] = (byte)(0x80 | sensor.Addr);
                data[data.Length - 1] = GetCRC(data, sensor.pre);//CRC
                port.Write(data, 0, 10);
                WaitSensorAnswer(10, WAIT_TIMEOUT);
                return ParseReadBuffer(WAIT_TIMEOUT) >= 0;
            }
            return false;
        }

        //Чтение выбранного датчика
        public bool SensorRead()
        {
            if ((port != null) && (SensorConnect))
            {
                Thread.Sleep(200);

                byte[] data = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x02, 0x80, 0x0E, 0x00, 0x8C };
                data[sensor.pre + 1] = (byte)(0x80 | sensor.Addr);
                data[data.Length - 1] = GetCRC(data, sensor.pre);//CRC
//                data[9] = (byte)(data[9] + sensor.Addr);

                port.Write(data, 0, 10);
                WaitSensorAnswer(10, WAIT_TIMEOUT);
                return ParseReadBuffer(WAIT_TIMEOUT) >= 0;
            }
            return false;
        }

        //Чтение измеренных параметров выбранного датчика
        public bool SensorValueReadC03()
        {
            if ((port != null) && (SensorConnect))
            {
                Thread.Sleep(200);

                byte[] data = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x02, 0x80, 0x03, 0x00, 0x00 };
                data[sensor.pre+1] = (byte)(0x80 | sensor.Addr);
                data[data.Length - 1] = GetCRC(data, sensor.pre);//CRC
                port.Write(data, 0, 10);
                WaitSensorAnswer(20, WAIT_TIMEOUT);
                return ParseReadBuffer(WAIT_TIMEOUT) >= 0;
            }
            return false;
        }

        //Запись нового адреса выбранного датчика
        public bool SensorAddrWrite()
        {
            if ((port != null) && (SensorConnect))
            {
                Thread.Sleep(200);

                byte[] data = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x02, 0x80, 0x0E, 0x00, 0x8C };
                data[sensor.pre + 1] = (byte)(0x80 | sensor.Addr);
                data[data.Length - 1] = GetCRC(data, sensor.pre);//CRC
                port.Write(data, 0, data.Length);
                WaitSensorAnswer(10, WAIT_TIMEOUT);
                return ParseReadBuffer(WAIT_TIMEOUT) >= 0;
            }
            return false;
        }
        //Переход в сервесный режим
        public bool EnterServis()
        {
            if ((port != null) && (SensorConnect))
            {
                Thread.Sleep(200);
                byte[] data = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x02, 0x80, 0xF5, 0x06, 0x41, 0x73, 0x62, 0x4D, 0x6B, 0x39, 0x00};
                data[sensor.pre + 1] = (byte)(0x80 | sensor.Addr);
                data[data.Length-1] = GetCRC(data, sensor.pre);//CRC

                port.Write(data, 0, 16);
                WaitSensorAnswer(10, WAIT_TIMEOUT);
                return ParseReadBuffer(WAIT_TIMEOUT) >= 0;
            }
            return false;
        }

        //Запись верхнего и нижнего пределов ПД, минимального диапазона (команда 249)
        public bool UpDownWriteC249()
        {
            if ((port != null) && (SensorConnect))
            {
                Thread.Sleep(200);
                int i;
                byte[] data = new byte[23];
                for(i=0;i< sensor.pre; i++) data[i] = 0xFF;
                data[i] = 0x02;
                data[i+1] = (byte)(0x80 | sensor.Addr);
                data[i+2] = 0xF9;
                data[i+3] = 0x0D;

                data[9] = sensor.MesUnit;
                UInt32 tmp;
                tmp = BitConverter.ToUInt32(BitConverter.GetBytes(sensor.UpLevel),0);
                data[10] = (byte)((tmp >> 24)&0xFF);
                data[11] = (byte)((tmp >> 16) & 0xFF);
                data[12] = (byte)((tmp >> 8) & 0xFF);
                data[13] = (byte)(tmp & 0xFF);
                tmp = BitConverter.ToUInt32(BitConverter.GetBytes(sensor.DownLevel), 0);
                data[14] = (byte)((tmp >> 24) & 0xFF);
                data[15] = (byte)((tmp >> 16) & 0xFF);
                data[16] = (byte)((tmp >> 8) & 0xFF);
                data[17] = (byte)(tmp & 0xFF);
                tmp = BitConverter.ToUInt32(BitConverter.GetBytes(sensor.MinLevel), 0);
                data[18] = (byte)((tmp >> 24) & 0xFF);
                data[19] = (byte)((tmp >> 16) & 0xFF);
                data[20] = (byte)((tmp >> 8) & 0xFF);
                data[21] = (byte)(tmp & 0xFF);

                data[22] = GetCRC(data,sensor.pre);//CRC

                port.Write(data, 0, data.Length);

                WaitSensorAnswer(10, WAIT_TIMEOUT);
                return ParseReadBuffer(WAIT_TIMEOUT) >= 0;
            }
            return false;
        }

        //Запись серийного номера ПД(команда 49).
        public bool WriteSerialNumberC49()
        {
            if ((port != null) && (SensorConnect))
            {
                Thread.Sleep(200);
                int i;
                byte[] data = new byte[13];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0x31;
                data[i + 3] = 0x03;
                data[i + 4] = (byte)((sensor.SerialNumber>>16) & 0xFF);
                data[i + 5] = (byte)((sensor.SerialNumber >> 8) & 0xFF);
                data[i + 6] = (byte)(sensor.SerialNumber & 0xFF);
                data[12] = GetCRC(data, sensor.pre);//CRC
                port.Write(data, 0, 13);
                WaitSensorAnswer(10, WAIT_TIMEOUT);
                return ParseReadBuffer(WAIT_TIMEOUT) >= 0;
            }
            return false;
        }

        //Запись модели приемника давления (команда 241).
        public bool C241WritePressureModel()
        {
            if ((port != null) && (SensorConnect))
            {
                Thread.Sleep(200);
                int i;
                byte[] data = new byte[15];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0xF1;
                data[i + 3] = 0x06;
                data[i + 4] = sensor.PressureType;
                for (int j=0;j<5;j++)
                {
                    data[i + 4 + j] = (byte)sensor.PressureModel[4-j];
                }

                data[14] = GetCRC(data, sensor.pre);//CRC
                port.Write(data, 0, 15);
                WaitSensorAnswer(10, WAIT_TIMEOUT);
                return ParseReadBuffer(WAIT_TIMEOUT) >= 0;
            }
            return false;
        }

        //Чтение модели приемника давления (команда 140).
        public bool C140ReadPressureModel()
        {
            if ((port != null) && (SensorConnect))
            {
                Thread.Sleep(200);
                int i;
                byte[] data = new byte[10];
                for (i = 0; i < sensor.pre; i++) data[i] = 0xFF;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0x8C;
                data[i + 3] = 0x00;
                data[9] = GetCRC(data, sensor.pre);//CRC
                port.Write(data, 0, 10);
                WaitSensorAnswer(10, WAIT_TIMEOUT);
                return ParseReadBuffer(WAIT_TIMEOUT) >= 0;
            }
            return false;
        }

        public bool SensorCoefficientReadC251()
        {
            if ((port != null) && (SensorConnect))
            {
                Thread.Sleep(200);

                byte[] data = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x02, 0x80, 0xFB, 0x01, 0x00, 0x00 };
                data[sensor.pre + 1] = (byte)(0x80 | sensor.Addr);
                for (int i = 0; i <= 5; i++)
                {
                    data[data.Length - 2] = (byte)i;
                    data[data.Length - 1] = GetCRC(data, sensor.pre);//CRC
                    port.Write(data, 0, data.Length);
                    WaitSensorAnswer(20, WAIT_TIMEOUT);
                    ParseReadBuffer(WAIT_TIMEOUT);
                }
                return true;
            }
            return false;
        }

        //запись параметра токового выхода
        public bool C129WriteCurrenExit()
        {
            if ((port != null) && (SensorConnect))
            {
                Thread.Sleep(200);
                int i;
                byte[] data = new byte[13];
                for (i = 0; i < 5; i++) data[i] = 0xFF;
                data[i] = 0x02;
                data[i + 1] = (byte)(0x80 | sensor.Addr);
                data[i + 2] = 0x81;
                data[i + 3] = 0x01;
                data[i + 4] = (byte)((sensor.CurrentExit) & 0xFF);
                data[10] = GetCRC(data, sensor.pre);//CRC
                port.Write(data, 0, 11);
                WaitSensorAnswer(10, WAIT_TIMEOUT);
                return ParseReadBuffer(WAIT_TIMEOUT) >= 0;
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
            byte[] indata = new byte[30];
            int rt = 0;

            while (readbuf.Count > 0)
            {
                Application.DoEvents();
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
                                    return -2;//неверный заголовок ответной команды
                                }
                            }
                        }
                        break;
                    case 2://читаем адрес
                        indata = readbuf.Pop(1);
                        if ((indata[0] & 0x80) == 0x80)//адрес
                        {
                            ReadAvtState = 3;
                            Adress = (byte)(indata[0] & 0x7F);
                        }
                        else//не верный адрес, ждем следующую команду
                        {
                            ReadAvtState = 1;
                        }
                        break;
                    case 3://читаем код команды
                        indata = readbuf.Pop(1);
                        CommandCod = indata[0];
                        ReadAvtState = 4;
                        break;
                    case 4://читаем количество байт в комманде
                        indata = readbuf.Pop(1);
                        CountByteToRead = indata[0];
                        ReadAvtState = 5;
                        rt = 0;
                        break;
                    case 5:
                        if (readbuf.Count >= CountByteToRead+1)
                        {//+CRC 1байт
                            indata = readbuf.Pop(CountByteToRead + 1);//читаем комманду + CRC
                            switch (CommandCod)
                            {
                                case 0x0://Запрос уникального идентификатора (команда 0)
                                    ReadCommand0(Adress, indata);
                                    break;
                                case 0x1://Считать первичную переменную (команда 1)
                                    break;
                                case 0x2://Считать ток и процент диапазона (команда 2)
                                    break;
                                case 0x3://Чтение измеренных значений (4 переменных) (команда 3)
                                    ReadCommand3(Adress, indata);
                                    break;
                                case 0x6://Записать адрес устройства (команда 6)
                                    sensor.state = (ushort)((indata[0] << 8) | indata[1]);//состояние по команде перехода в сервесный режим
                                    sensor.Addr = indata[2];
                                    break;
                                case 0x0B://Считать уникальный идентификатор, связанный с тэгом(команда 11)
                                    break;
                                case 0x0C://Считать сообщение (команда 12)
                                    ReadCommand12(Adress, indata);
                                    break;
                                case 0x0D://Считать тэг, дескриптор, дату (команда 13)
                                    ReadCommand13(Adress, indata);
                                    break;
                                case 0x0E://Чтение серийного номера и параметров приемника давления (команда 14)
                                    ReadCommand14(Adress, indata);
                                    break;
                                case 0x0F://Чтение ВПИ, НПИ (команда 15)
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
                                    break;
                                case 0x23://Запись ВПИ и НПИ (команда 35)
                                    break;
                                case 0x26://Сброс флага изменения конфигурации (команда 38)
                                    break;
                                case 0x28://Вход/выход в режим фиксированного тока (команда 40)
                                    break;
                                case 0x29://Запрос на выполнение самодиагностики (команда 41)
                                    break;
                                case 0x2A://Перезагрузка датчика (команда 42)
                                    break;
                                case 0x2B://Установить ноль первичной переменной (коррекция нуля от монтажного положения) (команда 43)
                                    break;
                                case 0x2C://Установить единицы измерения первичной переменной  (команда 44)
                                    break;
                                case 0x2D://Коррекция нуля ЦАП (команда 45)
                                    break;
                                case 0x2E://Коррекция коэффициента усиления ЦАП (команда 46)
                                    break;
                                case 0x2F://Установка функции преобразования первичной переменной (команда 47)
                                    break;
                                case 0x30://Чтение дополнительного статуса (команда 48)
                                    break;
                                case 0x31://Запись серийного номера ПД (команда 49)
                                    ReadCommand49(Adress, indata);
                                    break;
                                case 0x3B://Записать кол-во преамбул (команда 59)
                                    break;
                                case 0x80:// Считать параметры токового выхода(команда 128).
                                    ReadCommand128(Adress, indata);
                                    break;
                                case 0x81://Записать параметры токового выхода (команда 129)
                                    ReadCommand129(Adress, indata);
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
                                    ReadCommand140(Adress, indata);
                                    break;
                                case 0x8D://Запись данных о языке вывода названий меню – только для исполнения ЖК-2 (команда 141)
                                    break;
                                case 0x8E://Чтение данных о языке вывода названий меню – только для исполнения ЖК-2 (команда 142)
                                    break;
                                case 0x8F://Коррекция нуля путем ввода значения атмосферного давления(команда 143)
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
                                case 0xF1://Запись данных о модели приемника давления, тип давления (команда 241)
                                    ReadCommand241(Adress, indata);
                                    break;
                                case 0xF5://Перевод датчика в сервисный режим ( команда 245)
                                    sensor.state = (ushort)((indata[0] << 8) | indata[1]);//состояние по команде перехода в сервесный режим
                                    break;
                                case 0xF6://Чтение EEPROM (команда 246)
                                    break;
                                case 0xF7://Чтение кодов АЦП (команда 247)
                                    break;
                                case 0xF8://Запись серийного номера ДД (команда 248)
                                    break;
                                case 0xF9://Запись верхнего и нижнего пределов ПД, минимального диапазона (команда 249)
                                    ReadCommand249(Adress, indata);
                                    break;
                                case 0xFA://Запись калибровочных коэффициентов для датчика давления (команда 250)
                                    break;
                                case 0xFB://Чтение калибровочных коэффициентов (команда 251)
                                    if (!ReadCommand251(Adress, indata))
                                    {
                                        ReadAvtState = 1;
                                        return -5;//неверная структура данных в ответной команде
                                    }
                                    break;
                                case 0xFC://Запись калибровочных коэффициентов из RAM в EEPROM (команда 252)
                                    break;
                                case 0xFD://Переключение набора калибровочных коэффициентов для записи в EEPROM ( команда 253)
                                    break;
                                default:
                                    ReadAvtState = 1;
                                    return -3;//неизвестная комманда
                            }
                            ReadAvtState = 1;
                            return CommandCod;
                        }
                        else
                        {
                            rt++;
                            if (rt>ReadTimeout)//превышен таймаут ожидания
                            {
                                Console.WriteLine("Таймаут чтения команды ЭНИ100! Байт в буфере:" + readbuf.Count + " Ожидание: " + CountByteToRead + "/n");
                                ReadAvtState = 1;
                                readbuf.Clear();
                                return -4;
                            }
                            Thread.Sleep(1);
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
                    /*                    if (port.BytesToRead > 0)
                                        {
                                            Console.WriteLine(port.BytesToRead);
                                            byte[] indata = new byte[port.BytesToRead];
                                            if (port.Read(indata, 0, port.BytesToRead) > 0)
                                            {
                                                readbuf.Push(indata);
                                            }
                                        }*/
            }
                catch (TimeoutException)
                {
                    Console.WriteLine("Превышен таймаут на чтение данных с датчиков!\n");
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
                ReadThread.Priority = ThreadPriority.AboveNormal;
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

        //Ответ на комманду Запрос уникального идентификатора (команда 0)
        private void ReadCommand0(byte addr, byte[] indata)
        {
            SensorID id = new SensorID(addr);
            //            byte crc = GetCRC(indata, 0);
            //            if (crc != indata[indata.Length-1])
            //                return;
            id.Channal = SelSensorChannal;
            id.state = (ushort)((indata[0] << 8) | indata[1]);
            id.devCode = indata[3];
            id.devType = indata[4];
            id.pre = indata[5];
            id.v1 = indata[6];
            id.v2 = indata[7];
            id.v3 = indata[8];
            id.v4 = indata[9];
            id.flag = indata[10];
            id.uni = (UInt32)((indata[11] << 16) | (indata[12] << 8) | indata[13]);
            sensorList.Add(id);
        }
        //Ответ на комманду Чтение измеренных значений (4 переменных) (команда 3)
        private void ReadCommand3(int addr, byte[] indata)
        {
            int tmp;
            sensor.state = (ushort)((indata[0] << 8) | indata[1]);
            tmp = (indata[5] << 24) | (indata[4] << 16) | (indata[3] << 8) | indata[2];
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
        }


        //Считать сообщение (команда 12)
        private void ReadCommand12(int addr, byte[] indata)
        {
//            sensor.message = new byte[24];
            sensor.state = (ushort)((indata[0] << 8) | indata[1]);
            int j = 2;
            for (int i = 0; i < 24; i++)
            {
                sensor.message[i] = indata[j];
                j++;
            }
            Array.Reverse(sensor.message);//инвертируем порядок 
        }

        //Считать тэг, дескриптор, дату (команда 13)
        private void ReadCommand13(int addr, byte[] indata)
        {
            sensor.state = (ushort)((indata[0] << 8) | indata[1]);
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

            sensor.data = (UInt32)((indata[j] << 16) | (indata[j+1] << 8) | indata[j+2]);
        }

        //Чтение серийного номера и параметров приемника давления (команда 14)
        private void ReadCommand14(int addr, byte[] indata)
        {
            int tmp;
            sensor.state = (ushort)((indata[0] << 8) | indata[1]);
            sensor.SerialNumber = (UInt32)((indata[2] << 16) | (indata[3] << 8) | indata[4]);
            sensor.MesUnit = indata[5];
            tmp = (indata[6] << 24) | (indata[7] << 16) | (indata[8] << 8) | indata[9];
            sensor.UpLevel = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

            tmp = (indata[10] << 24) | (indata[11] << 16) | (indata[12] << 8) | indata[13];
            sensor.DownLevel = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

            tmp = (indata[14] << 24) | (indata[15] << 16) | (indata[16] << 8) | indata[17];
            sensor.MinLevel = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);
        }

        //Ответ на команду запись серийного номера ПД (команда 49)
        private void ReadCommand49(int addr, byte[] indata)
        {
            sensor.state = (ushort)((indata[0] << 8) | indata[1]);
            sensor.SerialNumber = (UInt32)((indata[2] << 16) | (indata[3] << 8) | indata[4]);
        }

        //Ответ на команду чтение токового выхода (команда 128)
        private void ReadCommand128(int addr, byte[] indata)
        {
            sensor.state = (ushort)((indata[0] << 8) | indata[1]);
            sensor.CurrentExit = indata[2];
        }
        //Ответ на команду запись токового выхода (команда 129)
        private void ReadCommand129(int addr, byte[] indata)
        {
            sensor.state = (ushort)((indata[0] << 8) | indata[1]);
            sensor.CurrentExit = indata[2];
        }

        //Ответ на команду чтение модели приемника давления (команда 140)
        private void ReadCommand140(int addr, byte[] indata)
        {
            sensor.state = (ushort)((indata[0] << 8) | indata[1]);
            sensor.PressureType = indata[2];
            for (int i = 0; i < 5; i++)//5 символов
            {
                sensor.PressureModel[4-i] = (char)indata[i+3];
            }
        }

        //Ответ на команду запись модели приемника давления (команда 241)
        private void ReadCommand241(int addr, byte[] indata)
        {
            sensor.state = (ushort)((indata[0] << 8) | indata[1]);
            sensor.PressureType = indata[2];
            for (int i = 0; i < 5; i++)//5 символов
            {
                sensor.PressureModel[4 - i] = (char)indata[i + 3];
            }
        }

        //ответ на запись верхнего и нижнего пределов ПД, минимального диапазона (команда 249)
        //2 байта статуса(сообщение об ошибках, 00 00 - ок)
        //1 байт – единицы измерения, 0x0C – кПа, 0xED – МПа
        //4 байта – верхняя граница диапазона ПД, тип float
        //4 байта – нижняя граница диапазона ПД, тип float
        //4 байта – минимальный диапазон ПД, тип float
        private void ReadCommand249(int addr, byte[] indata)
        {
            int tmp;
            sensor.state = (ushort)((indata[0] << 8) | indata[1]);
            sensor.MesUnit = indata[2];
            tmp = (indata[3] << 24) | (indata[4] << 16) | (indata[5] << 8) | indata[6];
            sensor.UpLevel = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

            tmp = (indata[7] << 24) | (indata[8] << 16) | (indata[9] << 8) | indata[10];
            sensor.DownLevel = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);

            tmp = (indata[11] << 24) | (indata[12] << 16) | (indata[13] << 8) | indata[14];
            sensor.MinLevel = BitConverter.ToSingle(BitConverter.GetBytes(tmp), 0);
        }

        //Чтение калибровочных коэффициентов(команда 251).
        private bool ReadCommand251(int addr, byte[] indata)
        {
            int tmp;
            sensor.state = (ushort)((indata[0] << 8) | indata[1]);

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
                return true;
            }
            else
            {
                //ошибка (Неверные данные)
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
