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
        public int Diapazon;
        public double Pressure;
        public double OutVoltage;
        public double Resistance;
    }

    //структура канала с датчиком, включает множество точек измерения
    struct SChanal
    {
        public int ChannalNummber;//номер канала
        public int FactoryNumber;//заводской номер датчика
        public string FileNameArchiv;
        public List<SPoint> Points;
        public SChanal(int ChNum, int FN)
        {
            ChannalNummber = ChNum;
            FactoryNumber = FN;
            FileNameArchiv = string.Format("Archiv/CH/CH_Ch{0}_F{1}.txt", ChannalNummber, FactoryNumber);
            Points = new List<SPoint>();
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
                                        "Сопротивление |";
        //конструктор класса
        //вход: число каналов и заводской номер датчика в каждом канале
        public СResultCH(int ChannalCount, int[] FN)
        {
            StreamWriter fs;
            for (int i = 0; i < ChannalCount; i++)
            {
                SChanal ch = new SChanal(i+1, FN[i]);
                Channal.Add(ch);
                Directory.CreateDirectory("CH");
                Directory.CreateDirectory("Archiv");
                Directory.CreateDirectory("Archiv/CH");
                string filename = string.Format("CH/CH_Result{0}.txt",ch.ChannalNummber);
                fs = File.CreateText(filename);//создаем файл канала
                fs.WriteLine(string.Format("Результаты характеризации датчика в канале {0}, заводской номер {1}", ch.ChannalNummber, ch.FactoryNumber));
                fs.WriteLine(HeaderString);
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


        public void Update(int ch, double Temp, int D, double Press, double U, double R)
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
                };
                Channal[ch].Points.RemoveAt(Channal[ch].Points.Count - 1);
                Channal[ch].Points.Add(point);
            }
        }

        public void AddPoint(int ch, double Temp, int D, double Press, double U, double R)
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
                point.Resistance.ToString("    00000.0000") + " |";
        }

        //создаем файл  архива на диске
        private StreamWriter CreateFileArhiv(SChanal ch)
        {
            StreamWriter writer = null;
            writer = File.CreateText(ch.FileNameArchiv);//создаем файл БД
            if (writer != null)
            {
                writer.WriteLine(string.Format("Архив данных характеризации датчика"));
                writer.WriteLine(string.Format("Канал:{0}; Заводской номер:{1}", ch.ChannalNummber, ch.FactoryNumber));
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

        //пересоздаем архив для датчика в канале i
        public void SaveToArhiv(int i)
        {
            if ((Channal.Count <= 0) || (i >= Channal.Count))
            {
                Program.txtlog.WriteLineLog("CH:Отсутсвуют данные характеризации для датчика в канале: " + i, 1);
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
                writer.Close();
                writer = null;
                Program.txtlog.WriteLineLog("CH:Данные характеризации успешно перезаписаны.", 0);
            }
            else
            {
                Program.txtlog.WriteLineLog("CH:Ошибка записи в архив  характеризации: " + ch.FileNameArchiv, 1);
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
    }
}
