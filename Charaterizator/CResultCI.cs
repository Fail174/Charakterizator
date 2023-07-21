using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Charaterizator
{
    //Структура точки измерения тока 
    struct SPointCI
    {
        public DateTime Datetime;
        public double Temperature;
        public double I4;
        public double I20;
    }
    
    //структура канала с датчиком, включает множество точек измерения тока
    struct SChanalCI
    {
        public List<SPointCI> Points;
        public int FactoryNumber;//заводской номер датчика
        public int ChannalNummber;//номер канала
        public string FileNameArchiv;

        public SChanalCI(int ChNum, int FN)
        {
            ChannalNummber = ChNum;
            FactoryNumber = FN;
//            FileNameArchiv = string.Format("Archiv/CI/CI_Ch{0}_F{1}.txt", ChannalNummber, FactoryNumber);
            FileNameArchiv = string.Format("Archiv/CI/CI_FN_{0}.txt", FactoryNumber);

            Points = new List<SPointCI>();
            //PointsCount = 0;
        }
    }

    class CResultCI
    {
        public List<StreamWriter> FileStream = new List<StreamWriter>();
        public List<SChanalCI> Channal = new List<SChanalCI>();//список обнаруженных датчиков
        private string HeaderString = "Дата и время       |" +
                                    "Температура   |" +
                                    "Ток 4мА       |" +
                                    "Ток 20мА      |";

        public CResultCI(int ChannalCount, int[] FN)
        {
            StreamWriter fs;
            for (int i = 0; i < ChannalCount; i++)
            {
                SChanalCI ch = new SChanalCI(i + 1, FN[i]);
                Channal.Add(ch);
                Directory.CreateDirectory("CI");
                Directory.CreateDirectory("Archiv/CI");
                string filename = string.Format("CI/CI_Result{0}.txt", ch.ChannalNummber);
                fs = File.CreateText(filename);//создаем файл канала
                fs.WriteLine(string.Format("Результаты чтения ЦАП датчика в канале {0}, заводской номер {1}", ch.ChannalNummber, ch.FactoryNumber));
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

        public void DeletePoint(int ch, int i)
        {
            if ((Channal.Count > ch) && (Channal[ch].Points.Count > i))
            {
                Channal[ch].Points.RemoveAt(i);
            }
            else
            {
                Program.txtlog.WriteLineLog("CI: Ошибка удаления записи в таблице данных ЦАП", 1);
            }
        }

        public void AddPoint(int ch, double Temp, double I1, double I2)
        {
            SPointCI point = new SPointCI();
            point.Datetime = DateTime.Now;
            point.Temperature = Temp;
            point.I4 = I1;
            point.I20 = I2;

            Channal[ch].Points.Add(point);
            FileStream[ch].WriteLine(GetStringFromPoint(point));
            FileStream[ch].Flush();
            WriteToArhiv(Channal[ch], point);
        }

        private string GetStringFromPoint(SPointCI point)
        {
            return point.Datetime.ToString() + "|" +
                point.Temperature.ToString("      +000.00;      -000.00;          0.0") + " |" +
                point.I4.ToString("   +0000.0000;   -0000.0000;          0.0") + " |" +
                point.I20.ToString("   +0000.0000;   -0000.0000;          0.0") + " |";
        }
        //создаем файл  архива на диске
        private StreamWriter CreateFileArhiv(SChanalCI ch)
        {
            StreamWriter writer = null;
            writer = File.CreateText(ch.FileNameArchiv);//создаем файл БД
            if (writer != null)
            {
                writer.WriteLine(string.Format("Архив данных ЦАП"));
                writer.WriteLine(string.Format("Канал:{0}; Заводской номер:{1}", ch.ChannalNummber, ch.FactoryNumber));
                writer.WriteLine("-----------------------------------------------------------------------------------------------");
                writer.WriteLine(HeaderString);
                writer.WriteLine("-----------------------------------------------------------------------------------------------");
            }
            return writer;
        }

        //Добавление записи текущего измрения в архив для датчика в канале ch
        public void WriteToArhiv(SChanalCI ch, SPointCI point)
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
                writer.WriteLine(GetStringFromPoint(point));
                writer.Close();
                writer = null;
            }
            else
            {
                Program.txtlog.WriteLineLog("CH:Ошибка записи в архив ЦАП: " + ch.FileNameArchiv, 1);
            }

        }

        //пересоздаем архив для датчика в канале i
        public void SaveToArhiv(int i)
        {
            if ((Channal.Count <= 0) || (i >= Channal.Count))
            {
                Program.txtlog.WriteLineLog("CH:Отсутсвуют данные ЦАП для датчика в канале: " + i, 1);
                return;
            }
            SChanalCI ch = Channal[i];
            StreamWriter writer = CreateFileArhiv(ch);
            if (writer != null)
            {
                for (int j = 0; j < ch.Points.Count; j++)//перебор точек измерения для датчика
                {
                    writer.WriteLine(GetStringFromPoint(ch.Points[j]));
                }
                writer.Close();
                writer = null;
                Program.txtlog.WriteLineLog("CH:Данные ЦАП успешно записаны в архив: " + ch.FileNameArchiv, 0);
            }
            else
            {
                Program.txtlog.WriteLineLog("CH:Ошибка записи в архив данных ЦАП: " + ch.FileNameArchiv, 1);
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
                Program.txtlog.WriteLineLog("CH:Критическая ошибка записи в архив данных ЦАП!", 1);
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
                    SChanalCI ch = Channal[i];
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
                            SPointCI point;
                            if (strarr.Length > 3)
                            {
                                point.Datetime = Convert.ToDateTime(strarr[0]);
                                double.TryParse(strarr[1].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), NumberStyles.Float, CultureInfo.InvariantCulture, out point.Temperature);
                                double.TryParse(strarr[2].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), NumberStyles.Float, CultureInfo.InvariantCulture, out point.I4);
                                double.TryParse(strarr[3].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), NumberStyles.Float, CultureInfo.InvariantCulture, out point.I20);
                                //point.Temperature = double.Parse(strarr[1].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                                //point.I4 = double.Parse(strarr[2].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                                //point.I20 = double.Parse(strarr[3].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                                ch.Points.Add(point);
                            }
                        } while (!reader.EndOfStream);
                        Program.txtlog.WriteLineLog("CI:Архив данных ЦАП загружен из файла: " + ch.FileNameArchiv, 0);

                        reader.Close();
                        reader = null;
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("CI:Ошибка доступа к архиву данных ЦАП: " + ch.FileNameArchiv, 1);
                        continue;
                    }
                }
            }
            catch
            {
                Program.txtlog.WriteLineLog("CI:Критическая ошибка чтения архива ЦАП!", 1);
            }
        }

    }
}
