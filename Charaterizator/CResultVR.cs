﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charaterizator
{
    //Структура точки верификации датчика
    struct SPointVR
    {
        public DateTime Datetime;
        public double Temperature;
        public double NPI;
        public double VPI;
        public double PressureZ;
        public double PressureF;
        public double CurrentR;
        public double CurrentF;
        public double OutVoltage;
        public double Resistance;
    }

    //структура канала с датчиком, включает множество точек измерения
    struct SChanalVR
    {
        public int ChannalNummber;//номер канала
        public int FactoryNumber;//заводской номер датчика
        public string FileNameArchiv;
        public List<SPointVR> Points;
        public byte SensorType;
        public char[] PressureModel;

        public SChanalVR(int ChNum, int FN, byte Type, string Model)
        {
            ChannalNummber = ChNum;
            FactoryNumber = FN;
            SensorType = Type;
            PressureModel = Model.ToCharArray();
            //            FileNameArchiv = string.Format("Archiv/VR/VR_Ch{0}_F{1}.txt", ChannalNummber, FactoryNumber);
            FileNameArchiv = string.Format("Archiv/VR/VR_FN_{0}.txt", FactoryNumber);

            Points = new List<SPointVR>();
        }
        public string GetSensorType()
        {
            switch (SensorType)
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

        public string GetSensorModel()
        {

            string str = new string(PressureModel);
            return str;
        }
    }

    class CResultVR
    {
        //StreamWriter[] FileStream;//поток записи
        public List<StreamWriter> FileStream = new List<StreamWriter>();
        public List<SChanalVR> Channal = new List<SChanalVR>();//список обнаруженных датчиков
        private string HeaderString = "Дата и время       |" +
                                    "Температура   |" +
                                    "НПИ           |" +
                                    "ВПИ           |" +
                                    "Давление (з)  |" +
                                    "Давление (ф)  |" +
                                    "Ток (р)       |" +
                                    "Ток (ф)       |" +
                                    "Напряжение    |" +
                                    "Сопротивление |" ;

        //конструктор класса
        //вход: число каналов и заводской номер датчика в каждом канале
        public CResultVR(int ChannalCount, int[] FN, byte[] Type, string[] Model)
        {
            StreamWriter fs;
            for (int i = 0; i < ChannalCount; i++)
            {
                SChanalVR ch = new SChanalVR(i + 1, FN[i], Type[i], Model[i]);
                Channal.Add(ch);
                Directory.CreateDirectory("VR");
                Directory.CreateDirectory("Archiv/VR");
                string filename = string.Format("VR/VR_Result{0}.txt", ch.ChannalNummber);
                fs = File.CreateText(filename);//создаем файл канала
                fs.WriteLine(string.Format("Результаты верификации датчика в канале {0}, заводской номер {1}", ch.ChannalNummber, ch.FactoryNumber));
                fs.WriteLine("-----------------------------------------------------------------------------------------------------------------------------");
                fs.WriteLine(HeaderString);
                fs.WriteLine("-----------------------------------------------------------------------------------------------------------------------------");
                fs.Flush();
                FileStream.Add(fs);
            }
        }

        public void CloseAll()
        {
            for (int i = 0; i < FileStream.Count; i++)
            {
                FileStream[i].Close();
            }
            FileStream.Clear();
        }

        public void DeletePoint(int ch, int i)
        {
            if ((Channal.Count > ch) && (Channal[ch].Points.Count > i))
            {
                Channal[ch].Points.RemoveAt(i);
            }
            else
            {
                Program.txtlog.WriteLineLog("VR: Ошибка удаления записи в таблице верификации", 1);
            }
        }


        public void AddPoint(int ch, double Temp, double npi, double vpi, double PressZ, double PressF, double CurF, double CurR, double Volt, double Resist)
        {
            try
            {
                SPointVR point = new SPointVR
                {
                    Datetime = DateTime.Now,
                    Temperature = Temp,
                    NPI = npi,
                    VPI = vpi,
                    PressureZ = PressZ,
                    PressureF = PressF,
                    CurrentF = CurF,
                    CurrentR = CurR,
                    OutVoltage = Volt,
                    Resistance = Resist,
                };
                Channal[ch].Points.Add(point);
                FileStream[ch].WriteLine(GetStringFromPoint(point));
                FileStream[ch].Flush();
                WriteToArhiv(Channal[ch],point);
            }
            catch
            {
                Program.txtlog.WriteLineLog(string.Format("VR:Ошибка записи в файл результатов верификации (канал {0})", ch), 1);
            }
        }

        //возвращает строку результатов характеризации в точке
        private string GetStringFromPoint(SPointVR point)
        {
            return  point.Datetime.ToString() + "|" +
                point.Temperature.ToString("      +000.00;      -000.00;          0.0") + " |" +
                point.NPI.ToString("   +00000.000;   -00000.000;          0.0") + " |" +
                point.VPI.ToString("   +00000.000;   -00000.000;          0.0") + " |" +
                point.PressureZ.ToString("   +00000.000;   -00000.000;          0.0") + " |" +
                point.PressureF.ToString("   +00000.000;   -00000.000;          0.0") + " |" +
                point.CurrentR.ToString("  +00000.0000;  -00000.0000;          0.0") + " |" +
                point.CurrentF.ToString("  +00000.0000;  -00000.0000;          0.0") + " |"+
                point.OutVoltage.ToString("  +00000.0000;  -00000.0000;          0.0") + " |"+
                point.Resistance.ToString("  +00000.0000;  -00000.0000;          0.0") + " |";
        }

        //создаем файл  архива на диске
        private StreamWriter CreateFileArhiv(SChanalVR ch)
        {
            StreamWriter writer = null;
            writer = File.CreateText(ch.FileNameArchiv);//создаем файл БД
            if (writer != null)
            {
                writer.WriteLine(string.Format("Архив данных верификации датчика"));
//                writer.WriteLine(string.Format("Канал:{0}; Заводской номер:{1}", ch.ChannalNummber, ch.FactoryNumber));
                writer.WriteLine(string.Format("Канал:{0}; Заводской номер:{1}; Тип:{2}; Модель:{3}", ch.ChannalNummber, ch.FactoryNumber, ch.GetSensorType(), ch.GetSensorModel()));
                writer.WriteLine("-----------------------------------------------------------------------------------------------");
                writer.WriteLine(HeaderString);
                writer.WriteLine("-----------------------------------------------------------------------------------------------");
            }
            return writer;
        }

        //Добавление записи текущего измерения в архив для датчика в канале ch
        public void WriteToArhiv(SChanalVR ch, SPointVR point)
        {
            StreamWriter writer = null;
            if (!File.Exists(ch.FileNameArchiv))
            {
                writer = CreateFileArhiv(ch);
            }
            else
            {
                writer = new StreamWriter(ch.FileNameArchiv, true);//открываем файл БД
            }

            if (writer != null)
            {
                //                if (ch.Points.Count > 0)
                //                    writer.WriteLine(GetStringFromPoint(ch.Points[ch.Points.Count - 1]));
                writer.WriteLine(GetStringFromPoint(point));
                writer.Close();
                writer = null;
            }
            else
            {
                Program.txtlog.WriteLineLog("VR:Ошибка записи в архив верификации: " + ch.FileNameArchiv, 1);
            }

        }

        //пересоздаем архив для датчика в канале i
        public void SaveToArhiv(int i)
        {
            if ((Channal.Count <= 0) || (i >= Channal.Count))
            {
                Program.txtlog.WriteLineLog("VR:Отсутсвуют данные верификации для датчика в канале: " + i, 1);
                return;
            }
            SChanalVR ch = Channal[i];
            StreamWriter writer = CreateFileArhiv(ch);
            if (writer != null)
            {
                for (int j = 0; j < ch.Points.Count; j++)//перебор точек измерения для датчика
                {
                    writer.WriteLine(GetStringFromPoint(ch.Points[j]));
                }
                writer.Close();
                writer = null;
                Program.txtlog.WriteLineLog("VR:Данные верификации успешно перезаписаны.", 0);
            }
            else
            {
                Program.txtlog.WriteLineLog("VR:Ошибка записи в файл данных верификации: " + ch.FileNameArchiv, 1);
            }
        }

        //Полная перезапись всех данных верификации в архивы
        public void SaveToFile()
        {
            try
            {
                for (int i = 0; i < Channal.Count; i++)//перебор каналов
                {
                    SaveToArhiv(i);
                }
            }
            catch
            {
                Program.txtlog.WriteLineLog("VR:Критическая ошибка записи в архив верификации!", 1);
            }
        }

        //Чтение из файла
        public void LoadFromFile()
        {
            StreamReader reader;

            try
            {
                for (int i = 0; i < Channal.Count; i++)//перебор каналов
                {
                    SChanalVR ch = Channal[i];
                    if (!File.Exists(ch.FileNameArchiv))
                    {
                        continue;
                    }
                    else
                    {
                        reader = new StreamReader(ch.FileNameArchiv);//открываем файл БД
                    }
                    if (reader != null)
                    {
                        string str = reader.ReadLine();
                        str = reader.ReadLine();
                        str = reader.ReadLine();
                        str = reader.ReadLine();
                        str = reader.ReadLine();
                        do
                        {
                            str = reader.ReadLine();
                            string[] strarr = str.Split('|');
                            SPointVR point;
                            if (strarr.Length > 7)
                            {
                                point.Datetime = Convert.ToDateTime(strarr[0]);
                                double.TryParse(strarr[1].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), NumberStyles.Float, CultureInfo.InvariantCulture, out point.Temperature);
                                double.TryParse(strarr[2].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), NumberStyles.Float, CultureInfo.InvariantCulture, out point.NPI);
                                double.TryParse(strarr[3].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), NumberStyles.Float, CultureInfo.InvariantCulture, out point.VPI);
                                double.TryParse(strarr[4].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), NumberStyles.Float, CultureInfo.InvariantCulture, out point.PressureZ);
                                double.TryParse(strarr[5].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), NumberStyles.Float, CultureInfo.InvariantCulture, out point.PressureF);
                                double.TryParse(strarr[6].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), NumberStyles.Float, CultureInfo.InvariantCulture, out point.CurrentR);
                                double.TryParse(strarr[7].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), NumberStyles.Float, CultureInfo.InvariantCulture, out point.CurrentF);

                                //point.Temperature = double.Parse(strarr[1].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                                //point.NPI = double.Parse(strarr[2].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                                //point.VPI = double.Parse(strarr[3].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                                //point.PressureZ = double.Parse(strarr[4].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                                //point.PressureF = double.Parse(strarr[5].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                                //point.CurrentR = double.Parse(strarr[6].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                                //point.CurrentF = double.Parse(strarr[7].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                                if (strarr.Length > 9)
                                {
                                    double.TryParse(strarr[8].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), NumberStyles.Float, CultureInfo.InvariantCulture, out point.OutVoltage);
                                    double.TryParse(strarr[9].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), NumberStyles.Float, CultureInfo.InvariantCulture, out point.Resistance);

                                    //point.OutVoltage = double.Parse(strarr[8].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                                    //point.Resistance = double.Parse(strarr[9].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    point.OutVoltage = 0;
                                    point.Resistance = 0;
                                }
                                ch.Points.Add(point);
                            }
                            
                        } while (!reader.EndOfStream);
                        Program.txtlog.WriteLineLog("VR:Архив данных верификации загружен из файла: " + ch.FileNameArchiv, 0);
                        reader.Close();
                        reader = null;
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("VR:Ошибка доступа к файлу архива верификации: " + ch.FileNameArchiv, 1);
                        continue;
                    }
                }
            }
            catch
            {
                Program.txtlog.WriteLineLog("VR:Критическая ошибка чтения архива верификации!", 1);
            }
        }

    }
}
