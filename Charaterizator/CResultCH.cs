using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charaterizator
{
    //Структура точки измерения параметров датчика
    struct SPoint
    {
        public DateTime Datetime;
        public double Temperature;
        public Int32 Diapazon;
        public double Pressure;
        public double OutVoltage;
        public double Resistance;
        public double Deviation;
    }

    //структура канала с датчиком, включает множество точек измерения
    struct SChanal
    {
        public Int32 ChannalNummber;//номер канала
        public Int32 FactoryNumber;//заводской номер датчика
        public string FileNameArchiv;
        public List<SPoint> Points;
        public Int32 CCount;
        public byte SensorType;
        public char[] PressureModel;
        public double[] Coefficient;//коэффициенты датчика
        //public double[] Coefficient_dbl;//коэффициенты датчика
        public double[] R2;//отклонение R2
        public SChanal(int ChNum, int FN, int CoefCount, byte Type, string Model)
        {
            CCount = CoefCount;
            SensorType = Type;
            PressureModel = Model.ToCharArray();
            Coefficient = new double[CoefCount];
            //Coefficient_dbl = new double[CoefCount];
            R2 = new double[1];
            ChannalNummber = ChNum;
            FactoryNumber = FN;
            //            FileNameArchiv = string.Format("Archiv/CH/CH_Ch{0}_F{1}.txt", ChannalNummber, FactoryNumber);
            FileNameArchiv = string.Format("Archiv/CH/CH_FN_{0}.txt",  FactoryNumber);
            Points = new List<SPoint>();
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

        public byte GetSensType(string senstype)
        {
            switch (senstype)
            {
                case "ЭНИ-100":
                    return 0xCC;
                case "ЭНИ-12":
                    return 0xCD;
                case "ЭНИ-100-ЖК2":
                    return 0xCE;
                case "ЭНИ-12М":
                    return 0xCF;
                default:
                    return 0;
            }
        }

        public string GetSensorModel()
        {

            string str = new string(PressureModel);
            return str;
        }
    }

    //Класс результатов измерения параметров датчика при характеризации
    class СResultCH
    {
        //StreamWriter[] FileStream;//поток записи
        public List<StreamWriter> FileStream = new List<StreamWriter>();
        public List<SChanal> Channal = new List<SChanal>();//список обнаруженных датчиков
        private string HeaderString =   "Дата и время       |" +
                                        "Температура   |" +
                                        "Диапазон      |" +
                                        "Давление      |" +
                                        "Напряжение    |" +
                                        "Сопротивление |" +
                                        "Отклонение    |";
        //конструктор класса
        //вход: число каналов и заводской номер датчика в каждом канале
        public СResultCH(int ChannalCount, int[] FN, int CoefCount, byte[] Type, string[] Model)
        {
            StreamWriter fs;
            for (int i = 0; i < ChannalCount; i++)
            {
                SChanal ch = new SChanal(i+1, FN[i], CoefCount, Type[i], Model[i]);
                Channal.Add(ch);
                Directory.CreateDirectory("CH");
                Directory.CreateDirectory("Archiv");
                Directory.CreateDirectory("Archiv/CH");
                string filename = string.Format("CH/CH_Result{0}.txt",ch.ChannalNummber);
                fs = File.CreateText(filename);//создаем файл канала
                //fs.WriteLine(string.Format("Результаты характеризации датчика в канале {0}, заводской номер {1}", ch.ChannalNummber, ch.FactoryNumber));
                fs.WriteLine(string.Format("Результаты характеризации датчика в канале:{0}; Заводской номер:{1}; Тип:{2}; Модель:{3}", ch.ChannalNummber, ch.FactoryNumber, ch.GetSensorType(), ch.GetSensorModel()));
                fs.WriteLine(HeaderString);
                fs.Flush();
                FileStream.Add(fs);
            }
        }

        public СResultCH(string FileName)
        {
            LoadFromArchiv(FileName);
        }

        public void CloseAll()
        {
            for (int i = 0; i < FileStream.Count; i++)
            {
                FileStream[i].Close();
            }
            FileStream.Clear();
        }

        ~СResultCH()
        {
            
        }

        public void DeletePoint(int ch, int i)
        {
            if ((Channal.Count > ch) && (Channal[ch].Points.Count > i))
            {
                Channal[ch].Points.RemoveAt(i);
            }
            else
            {
                Program.txtlog.WriteLineLog("CH: Ошибка удаления записи в таблице характеризации", 1);
            }
        }


        public void Update(int ch, double Temp, int D, double Press, double U, double R, double Dev)
        {
            if ((Channal.Count>ch) && (Channal[ch].Points.Count>0))
            {
                SPoint point = new SPoint
                {
                    Datetime = DateTime.Now,
                    Temperature = Temp,
                    Diapazon = D,
                    Pressure = Press,
                    OutVoltage = U,
                    Resistance = R,
                    Deviation = Dev
                };
                Channal[ch].Points.RemoveAt(Channal[ch].Points.Count - 1);
                Channal[ch].Points.Add(point);
            }
        }

        public void AddCoeff(int ch, double [] Coeff)
        {
            for (int i = 0; i < Channal[ch].CCount; i++)
            {
                Channal[ch].Coefficient[i] = Coeff[i];
            }
            SaveToArhiv(ch);
        }

        // 
       /* public void AddCoeff(int ch, double[] Coeff)
        {
            for (int i = 0; i < Channal[ch].CCount; i++)
            {
                Channal[ch].Coefficient_dbl[i] = Coeff[i];
            }
            SaveToArhiv(ch);
        }*/

        // 
        public void AddR2(int ch, double CoeffR2)
        {
            Channal[ch].R2[0] = CoeffR2;
        }


        public void SetSensorInfo(int ch, char[] Model)
        {
            for (int i = 0; i < 5; i++)
            {
                Channal[ch].PressureModel[i] = Model[i];
            }
        }
        public void AddPoint(int ch, double Temp, int D, double Press, double U, double R, double Dev)
        {
            try
            {
                SPoint point = new SPoint
                {
                    Datetime = DateTime.Now,
                    Temperature = Temp,
                    Diapazon = D,
                    Pressure = Press,
                    OutVoltage = U,
                    Resistance = R,
                    Deviation = Dev
                };
                Channal[ch].Points.Add(point);
                FileStream[ch].WriteLine(GetStringFromPoint(point));
                FileStream[ch].Flush();
                WriteToArhiv(Channal[ch], point);
            }
            catch
            {
                Program.txtlog.WriteLineLog(string.Format("Ошибка записи в файл результатов характеризации (канал {0})", ch),1);
            }
        }

        //возвращает строку результатов характеризации в точке
        private string GetStringFromPoint(SPoint point)
        {
            return  point.Datetime.ToString() + "|" +
                point.Temperature.ToString("       +000.0;       -000.0;          0.0") + " |" +
                point.Diapazon.ToString("           00") + " |" +
                point.Pressure.ToString("    +00000.00;    -00000.00;          0.0") + " |" +
                point.OutVoltage.ToString("   +0000.0000;   -0000.0000;          0.0") + " |" +
                point.Resistance.ToString("   00000.0000") + " |"+
                point.Deviation.ToString("   +0000.0000;   -0000.0000;          0.0") + " |";
        }

        //создаем файл  архива на диске
        private StreamWriter CreateFileArhiv(SChanal ch)
        {
            StreamWriter writer = null;
            writer = File.CreateText(ch.FileNameArchiv);//создаем файл БД
            if (writer != null)
            {
                writer.WriteLine(string.Format("Архив данных характеризации датчика"));
                writer.WriteLine(string.Format("Канал:{0}; Заводской номер:{1}; Тип:{2}; Модель:{3}", ch.ChannalNummber, ch.FactoryNumber, ch.GetSensorType(), ch.GetSensorModel()));
                writer.WriteLine("-----------------------------------------------------------------------------------------------");
                writer.WriteLine(HeaderString);
                writer.WriteLine("-----------------------------------------------------------------------------------------------");
            }
            return writer;
        }

        //Добавление записи текущего измрения в архив для датчика в канале ch
        public void WriteToArhiv(SChanal ch, SPoint point)
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
                Program.txtlog.WriteLineLog("CH:Ошибка записи в архив характеризации: " + ch.FileNameArchiv, 1);
            }

        }

        //создаем файл  архива на диске
        private BinaryWriter CreateFileBinary(SChanal ch)
        {
            BinaryWriter writer = new BinaryWriter(File.Open(ch.FileNameArchiv, FileMode.OpenOrCreate));//создаем файл БД
            if (writer != null)
            {
                writer.Write(ch.FactoryNumber);
                writer.Write(ch.ChannalNummber);
            }
            return writer;
        }
        //пересоздаем архив для датчика в канале i
        public void SaveToArhiv(int i)
        {
            if ((Channal.Count <= 0) || (i >= Channal.Count))
            {
                Program.txtlog.WriteLineLog("CH: Отсутсвуют данные характеризации для датчика в канале: " + i, 1);
                return;
            }
            SChanal ch = Channal[i];
            StreamWriter writer = CreateFileArhiv(ch);
            if (writer != null)
            {
                for (int j = 0; j < ch.Points.Count; j++)//перебор точек измерения для датчика
                {
                    writer.WriteLine(GetStringFromPoint(ch.Points[j]));
                }
                if (ch.Coefficient[0] != 0)//если коэффициенты подсчитаны
                {
                    writer.WriteLine("-----------------------------------------------------------------------------------------------");
                    writer.WriteLine("Рассчитанное отклонение R^2");
                    writer.WriteLine(ch.R2[0].ToString("E19"));
                    writer.WriteLine("---------------------------");
                    writer.WriteLine("Коэффициенты датчика");
                    writer.WriteLine("Количество коэффициентов: " + ch.CCount.ToString());
                    for (int c = 0; c < ch.CCount; c++)
                    {
                        writer.WriteLine(c.ToString("D2") + ": " + ch.Coefficient[c].ToString("E19"));
                    }
                }
                writer.Close();
                writer = null;
                //Program.txtlog.WriteLineLog("CH: Данные характеризации успешно перезаписаны.", 0);
            }
            else
            {
                Program.txtlog.WriteLineLog("CH: Ошибка записи в архив  характеризации: " + ch.FileNameArchiv, 1);
            }
        }

        //пересоздаем архив для датчика в канале i
        public void SaveToBinary(int i)
        {
            if ((Channal.Count <= 0) || (i >= Channal.Count))
            {
                Program.txtlog.WriteLineLog("CH: Отсутсвуют данные характеризации для датчика в канале: " + i, 1);
                return;
            }
            SChanal ch = Channal[i];
            BinaryWriter writer = CreateFileBinary(ch);
            if (writer != null)
            {
                writer.Write(ch.Points.Count);
                for (int j = 0; j < ch.Points.Count; j++)//перебор точек измерения для датчика
                {
                    writer.Write(ch.Points[j].Datetime.ToBinary());
                    writer.Write(ch.Points[j].Temperature);
                    writer.Write(ch.Points[j].Diapazon);
                    writer.Write(ch.Points[j].Pressure);
                    writer.Write(ch.Points[j].OutVoltage);
                    writer.Write(ch.Points[j].Resistance);
                    writer.Write(ch.Points[j].Deviation);
                }
                if (ch.Coefficient[0] != 0)//если коэффициенты подсчитаны
                {
                    writer.Write(ch.CCount);
                    for (int c = 0; c < ch.CCount - 1; c++)
                    {
                        writer.Write(ch.Coefficient[c]);
                    }
                }
                writer.Close();
                writer = null;
                Program.txtlog.WriteLineLog("CH: Данные характеризации успешно перезаписаны.", 0);
            }
            else
            {
                Program.txtlog.WriteLineLog("CH: Ошибка записи в архив  характеризации: " + ch.FileNameArchiv, 1);
            }
        }


        public void LoadFromBinary()
        {
            try
            {
                for (int i = 0; i < Channal.Count; i++)//перебор каналов
                {
                    SChanal ch = Channal[i];
                    if (!File.Exists(ch.FileNameArchiv))
                    {
                        continue;
                    }
                    BinaryReader reader = new BinaryReader(File.Open(ch.FileNameArchiv, FileMode.Open));
                    if (reader != null)
                    {
                        ch.FactoryNumber = reader.ReadInt32();
                        ch.ChannalNummber = reader.ReadInt32();
                        int Count = reader.ReadInt32();
                        for (int j = 0; j < Count; j++)//перебор точек измерения для датчика
                        {
                            SPoint point;
                            point.Datetime = DateTime.FromBinary(reader.ReadInt64());
                            point.Temperature = reader.ReadDouble();
                            point.Diapazon = reader.ReadInt32();
                            point.Pressure = reader.ReadDouble();
                            point.OutVoltage = reader.ReadDouble();
                            point.OutVoltage = reader.ReadDouble();
                            point.Resistance = reader.ReadDouble();
                            point.Deviation = reader.ReadDouble();
                            ch.Points.Add(point);
                        }
                        Count = reader.ReadInt32();
                        for (int c = 0; c < Count - 1; c++)
                        {
                            ch.Coefficient[c] = reader.ReadSingle();
                        }

                        reader.Close();
                        Program.txtlog.WriteLineLog("CH:Архив данных характеризации загружен из файла: " + ch.FileNameArchiv, 0);
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("CH:Ошибка доступа к архиву данных характеризации: " + ch.FileNameArchiv, 1);
                    }
                }
            }
            catch
            {
                Program.txtlog.WriteLineLog("CH:Критическая ошибка чтения архива характеризации!", 1);
            }
        }

        //Полная перезапись всех данных характеризации в архивы
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
                Program.txtlog.WriteLineLog("CH:Критическая ошибка записи в архив характеризации!", 1);
            }
        }

        private void LoadFromArchiv(string FileName)
        {
            StreamReader reader;
            SChanal ch = new SChanal(1, 0, 24, 0xCC, "ЭНИ-100");
            //Channal.Add(ch);
            ch.FileNameArchiv = FileName;

            //SChanal ch = Channal[0];
            if (!File.Exists(FileName))
            {
                return;
            }
            else
            {
                reader = new StreamReader(FileName);//открываем файл
            }
            if (reader != null)
            {
                string str = reader.ReadLine();
                str = reader.ReadLine();
                string[] tmp = str.Split(';');
                string[] senstype = tmp[2].Split(':');
                string[] model = tmp[3].Split(':');
                ch.SensorType = ch.GetSensType(senstype[1]);
                ch.PressureModel = model[1].ToCharArray();;
                //model[1];
                str = reader.ReadLine();
                str = reader.ReadLine();
                str = reader.ReadLine();
                do
                {
                    str = reader.ReadLine();
                    if (str == "-----------------------------------------------------------------------------------------------") break;//конец раздела

                    string[] strarr = str.Split('|');
                    SPoint point;
                    if (strarr.Length > 5)
                    {
                        point.Datetime = Convert.ToDateTime(strarr[0]);
                        point.Temperature = double.Parse(strarr[1].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                        point.Diapazon = Convert.ToInt32(strarr[2]);
                        point.Pressure = double.Parse(strarr[3].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                        point.OutVoltage = double.Parse(strarr[4].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                        point.Resistance = double.Parse(strarr[5].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                        if (strarr.Length > 6)
                        {
                            if (strarr[6] != "")
                            {
                                point.Deviation = double.Parse(strarr[6].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                point.Deviation = 0;
                            }
                        }
                        else
                        {
                            point.Deviation = 0;
                        }
                        ch.Points.Add(point);
                    }
                } while (!reader.EndOfStream);
                Program.txtlog.WriteLineLog("CH:Архив данных характеризации загружен из файла: " + ch.FileNameArchiv, 0);

                Channal.Add(ch);
                reader.Close();
                reader = null;
            }
            else
            {
                Program.txtlog.WriteLineLog("CH:Ошибка доступа к архиву данных характеризации: " + ch.FileNameArchiv, 1);
                return;
            }
        }

        //Чтение архивов из файлов
        public void LoadFromFile()
        {
            StreamReader reader;

            try
            {
                for (int i = 0; i < Channal.Count; i++)//перебор каналов
                {
                    SChanal ch = Channal[i];
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
                        do {
                            str = reader.ReadLine();
                            if (str == "-----------------------------------------------------------------------------------------------") break;//конец раздела

                            string[] strarr = str.Split('|');
                            SPoint point;
                            if (strarr.Length > 5)
                            {
                                point.Datetime = Convert.ToDateTime(strarr[0]);
                                point.Temperature = double.Parse(strarr[1].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                                point.Diapazon = Convert.ToInt32(strarr[2]);
                                point.Pressure = double.Parse(strarr[3].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                                point.OutVoltage = double.Parse(strarr[4].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                                point.Resistance = double.Parse(strarr[5].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                                if (strarr.Length > 6)
                                {
                                    if (strarr[6] != "")
                                    {
                                        point.Deviation = double.Parse(strarr[6].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                                    }
                                    else
                                    {
                                        point.Deviation = 0;
                                    }
                                }
                                else
                                {
                                    point.Deviation = 0;
                                }
                                ch.Points.Add(point);
                            }
                        } while (!reader.EndOfStream);
                        Program.txtlog.WriteLineLog("CH:Архив данных характеризации загружен из файла: " + ch.FileNameArchiv, 0);

                        reader.Close();
                        reader = null;
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("CH:Ошибка доступа к архиву данных характеризации: " + ch.FileNameArchiv, 1);
                        continue;
                    }
                }
            }
            catch
            {
                Program.txtlog.WriteLineLog("CH:Критическая ошибка чтения архива характеризации!", 1);
            }
        }

        //Расчет отклонения
        public void CalcDeviation(int i, bool SensorAbsPressuer)
        {
            double P, Pmax = 0, V, Vmax = 0, V0 = 0, Temp = -10000;
            for (int j = 0; j < Channal[i].Points.Count; j++)//Ищем максимальные точки
            {
                if (Math.Abs(Channal[i].Points[j].Temperature - Temp)>1)//новая температура
                {
                    if(j>0)
                    {
                        for (int jj = j-1; jj >= 0; jj--)//расчитываем отклонения для всех точек с данной температурой
                        {
                            if (Math.Abs(Channal[i].Points[jj].Temperature - Temp) > 1)//перебераем точки с данной температурой
                            {
                                break;
                            }
                            else
                            {
                                SPoint point = new SPoint
                                {
                                    Datetime = Channal[i].Points[jj].Datetime,
                                    Temperature = Channal[i].Points[jj].Temperature,
                                    Diapazon = Channal[i].Points[jj].Diapazon,
                                    Pressure = Channal[i].Points[jj].Pressure,
                                    OutVoltage = Channal[i].Points[jj].OutVoltage,
                                    Resistance = Channal[i].Points[jj].Resistance,
                                    Deviation = CalcPressDeviation(Channal[i].Points[jj].Pressure, Channal[i].Points[jj].OutVoltage, Vmax, V0, Pmax),
                                };
                                Channal[i].Points.RemoveAt(jj);
                                Channal[i].Points.Insert(jj, point);
                            }
                        
                        }
                    }
                    Temp = Channal[i].Points[j].Temperature;
                    Pmax = 0;
                    Vmax = 0;
                    V0 = 0;
                }

                P = Channal[i].Points[j].Pressure;
                V = Channal[i].Points[j].OutVoltage;
                if (P > Pmax)
                {
                    Pmax = P;
                    Vmax = V;
                }
                if (P <= 0.1)
                {
                    if (SensorAbsPressuer)
                    {
                        V0 = CalcPressDeviationAbs(P, Vmax, V, Pmax);
                    }
                    else
                    {
                        V0 = V;
                    }
                }
            }


            for (int jj = Channal[i].Points.Count - 1; jj >= 0; jj--)//расчитываем отклонения для всех точек с данной температурой
            {
                if (Math.Abs(Channal[i].Points[jj].Temperature - Temp) > 1)//перебераем точки с данной температурой
                {
                    break;
                }
                else
                {
                    SPoint point = new SPoint
                    {
                        Datetime = Channal[i].Points[jj].Datetime,
                        Temperature = Channal[i].Points[jj].Temperature,
                        Diapazon = Channal[i].Points[jj].Diapazon,
                        Pressure = Channal[i].Points[jj].Pressure,
                        OutVoltage = Channal[i].Points[jj].OutVoltage,
                        Resistance = Channal[i].Points[jj].Resistance,
                        Deviation = CalcPressDeviation(Channal[i].Points[jj].Pressure, Channal[i].Points[jj].OutVoltage, Vmax, V0, Pmax),
                    };
                    Channal[i].Points.RemoveAt(jj);
                    Channal[i].Points.Insert(jj, point);
                }

            }
        }
        private double CalcPressDeviation(double Press, double V, double Vmax, double V0, double Pmax)
        {
            double Vd = Vmax - V0;
            if ((Pmax == 0)|| (Vd==0)) return 0;

            double Vr = V0 + Vd * Press / Pmax;

            return (V - Vr) * 100 / Vd;
        }

        /// <summary>
        /// U0 = V0 — (Vmax — V0)*Press/(Pmax-Press);
        /// </summary>
        /// <param name="Press"></param>
        /// <param name="V"></param>
        /// <param name="Vmax"></param>
        /// <param name="V0"></param>
        /// <param name="Pmax"></param>
        /// <returns></returns>
        private double CalcPressDeviationAbs(double Press,  double Vmax, double V0, double Pmax)
        {
            double Vd = Vmax - V0;
            if ((Pmax == 0) || (Vd == 0)) return 0;

            double Vr = V0 + Vd * Press / (Pmax- Press);

            return Vr;
        }

        /*
        public Matrix<double> GetTemperatureMatrix(int i)
        {
            Matrix<double> mtx = DenseMatrix.OfArray(new double[1, 5]);// { { -40, -10, 23, 50, 80 } });
            if (Channal[i].Points.Count <= 0) return mtx;
            int c = 0;

            double Temp = Channal[i].Points[0].Temperature;
            mtx[0, c] = Temp;

            for (int j = 1; j < Channal[i].Points.Count; j++)//Ищем максимальные точки
            {
                if (Math.Abs(Channal[i].Points[j].Temperature - Temp) > 1)//новая температура
                {
                    c++;
                    if (c >= 5) break;
                    Temp = Channal[i].Points[j].Temperature;
                    mtx[0, c] = Temp;
                }
            }

            return mtx;
        }
        


        public Matrix<double> GetPressuerMatrix(int i)
        {
            Matrix<double> mtx = DenseMatrix.OfArray(new double[6, 5]);// { { -40, -10, 23, 50, 80 } });

            if (Channal[i].Points.Count <= 0) return mtx;
            int t = 0, p = 0;
            int rowCount = 1, colCount = 1;

            double Temp = Channal[i].Points[0].Temperature;
            mtx[0, 0] = Channal[i].Points[0].Pressure;

            for (int j = 1; j < Channal[i].Points.Count; j++)//Ищем максимальные точки
            {
                if (Math.Abs(Channal[i].Points[j].Temperature - Temp) > 1)//новая температура
                {
                    p = 0;
                    t++;
                    if (t >= 5) break;
                    Temp = Channal[i].Points[j].Temperature;
                    mtx[p, t] = Channal[i].Points[j].Pressure;
                }
                else
                {
                    p++;
                    if (p >= 6) continue;
                    mtx[p, t] = Channal[i].Points[j].Pressure;
                }
            }
            rowCount = p++;
            colCount = t++;
            return mtx;
        }

        public Matrix<double> GetVoltageMatrix(int i)
        {
            Matrix<double> mtx = DenseMatrix.OfArray(new double[6, 5]);// { { -40, -10, 23, 50, 80 } });
            if (Channal[i].Points.Count <= 0) return mtx;
            int t = 0, p = 0;

            double Temp = Channal[i].Points[0].Temperature;
            mtx[0, 0] = Channal[i].Points[0].OutVoltage;

            for (int j = 1; j < Channal[i].Points.Count; j++)//Ищем максимальные точки
            {
                if (Math.Abs(Channal[i].Points[j].Temperature - Temp) > 1)//новая температура
                {
                    p = 0;
                    t++;
                    if (t >= 5) break;
                    Temp = Channal[i].Points[j].Temperature;
                    mtx[p, t] = Channal[i].Points[j].OutVoltage;
                }
                else
                {
                    p++;
                    if (p >= 6) continue;

                    mtx[p, t] = Channal[i].Points[j].OutVoltage;
                }
            }
            return mtx;
        }

        public Matrix<double> GetRezistansMatrix(int i)
        {
            Matrix<double> mtx = DenseMatrix.OfArray(new double[6, 5]);// { { -40, -10, 23, 50, 80 } });
            if (Channal[i].Points.Count <= 0) return mtx;
            int t = 0, p = 0;

            double Temp = Channal[i].Points[0].Temperature;
            mtx[0, 0] = Channal[i].Points[0].Resistance;

            for (int j = 1; j < Channal[i].Points.Count; j++)//Ищем максимальные точки
            {
                if (Math.Abs(Channal[i].Points[j].Temperature - Temp) > 1)//новая температура
                {
                    p = 0;
                    t++;
                    if (t >= 5) break;
                    Temp = Channal[i].Points[j].Temperature;
                    mtx[p, t] = Channal[i].Points[j].Resistance;
                }
                else
                {
                    p++;
                    if (p >= 6) continue;

                    mtx[p, t] = Channal[i].Points[j].Resistance;
                }
            }
            return mtx;
        }
        */

        public Matrix<double> GetTemperatureMatrix(int i)
        {
            Matrix<double> mtx = DenseMatrix.OfArray(new double[1, 20]);// { { -40, -10, 23, 50, 80 } });
            if (Channal[i].Points.Count <= 0) return mtx;
            int c = 0;

            double Temp = Channal[i].Points[0].Temperature;
            mtx[0, c] = Temp;

            for (int j = 1; j < Channal[i].Points.Count; j++)//Ищем максимальные точки
            {
                if (Math.Abs(Channal[i].Points[j].Temperature - Temp) > 1)//новая температура
                {
                    c++;
                    //if (c >= 5) break;
                    Temp = Channal[i].Points[j].Temperature;
                    mtx[0, c] = Temp;
                }
            }

            Matrix<double> mtx1 = DenseMatrix.OfArray(new double[1, c+1]);
            for (int m = 0; m < c+1; m++)
            {
                mtx1[0, m] = mtx.At(0, m);              
            }  
            
            return mtx1;
        }


        
        public Matrix<double> GetPressuerMatrix(int i)
        {
            Matrix<double> mtx = DenseMatrix.OfArray(new double[20, 20]);// { { -40, -10, 23, 50, 80 } });
           
            if (Channal[i].Points.Count <= 0) return mtx;
            int t = 0, p=0;
            //int rowCount = 1, colCount = 1; 

            double Temp = Channal[i].Points[0].Temperature;
            mtx[0, 0] = Channal[i].Points[0].Pressure;

            for (int j = 1; j < Channal[i].Points.Count; j++)//Ищем максимальные точки
            {
                if (Math.Abs(Channal[i].Points[j].Temperature - Temp) > 1)//новая температура
                {
                    p = 0;
                    t++;
                    //if (t >= 5) break;
                    Temp = Channal[i].Points[j].Temperature;
                    mtx[p, t] = Channal[i].Points[j].Pressure;
                }
                else
                {
                    p++;
                    //if (p >= 6) continue;
                    mtx[p, t] = Channal[i].Points[j].Pressure;
                }
            }
            
            //rowCount = p++;
            //colCount = t++; 
            Matrix<double> mtx1 = DenseMatrix.OfArray(new double[p+1, t+1]);
            for (int col = 0; col < t+1; col++)
            {
                for (int row = 0; row < p+1; row++)
                {

                    mtx1[row, col] = mtx.At(row, col);
                }
            }
            return mtx1;
        }


        public Matrix<double> GetVoltageMatrix(int i)
        {
            //Matrix<double> mtx = DenseMatrix.OfArray(new double[6, 5]);// { { -40, -10, 23, 50, 80 } });
            Matrix<double> mtx = DenseMatrix.OfArray(new double[20, 20]);// { { -40, -10, 23, 50, 80 } });
            if (Channal[i].Points.Count <= 0) return mtx;
            int t = 0, p = 0;

            double Temp = Channal[i].Points[0].Temperature;
            mtx[0, 0] = Channal[i].Points[0].OutVoltage;

            for (int j = 1; j < Channal[i].Points.Count; j++)//Ищем максимальные точки
            {
                if (Math.Abs(Channal[i].Points[j].Temperature - Temp) > 1)//новая температура
                {
                    p = 0;
                    t++;
                    //if (t >= 5) break;
                    Temp = Channal[i].Points[j].Temperature;
                    mtx[p, t] = Channal[i].Points[j].OutVoltage;
                }
                else
                {
                    p++;
                    //if (p >= 6) continue;
                    mtx[p, t] = Channal[i].Points[j].OutVoltage;
                }
            }
            Matrix<double> mtx1 = DenseMatrix.OfArray(new double[p + 1, t + 1]);
            for (int col = 0; col < t + 1; col++)
            {
                for (int row = 0; row < p + 1; row++)
                {

                    mtx1[row, col] = mtx.At(row, col);
                }
            }
            return mtx1;
        }


        public Matrix<double> GetRezistansMatrix(int i)
        {
            Matrix<double> mtx = DenseMatrix.OfArray(new double[20, 20]);// { { -40, -10, 23, 50, 80 } });
            if (Channal[i].Points.Count <= 0) return mtx;
            int t = 0, p = 0;

            double Temp = Channal[i].Points[0].Temperature;
            mtx[0, 0] = Channal[i].Points[0].Resistance;

            for (int j = 1; j < Channal[i].Points.Count; j++)//Ищем максимальные точки
            {
                if (Math.Abs(Channal[i].Points[j].Temperature - Temp) > 1)//новая температура
                {
                    p = 0;
                    t++;
                    //if (t >= 5) break;
                    Temp = Channal[i].Points[j].Temperature;
                    mtx[p, t] = Channal[i].Points[j].Resistance;
                }
                else
                {
                    p++;
                    //if (p >= 6) continue;
                    mtx[p, t] = Channal[i].Points[j].Resistance;
                }
            }
            
            Matrix<double> mtx1 = DenseMatrix.OfArray(new double[p + 1, t + 1]);
            for (int col = 0; col < t + 1; col++)
            {
                for (int row = 0; row < p + 1; row++)
                {

                    mtx1[row, col] = mtx.At(row, col);
                }
            }            
            return mtx1;          

        }
        
    }
}
