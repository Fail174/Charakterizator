using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
        public SChanalCI(int ChNum, int FN)
        {
            ChannalNummber = ChNum;
            FactoryNumber = FN;
            Points = new List<SPointCI>();
            //PointsCount = 0;
        }
    }

    class CResultCI
    {
        public List<StreamWriter> FileStream = new List<StreamWriter>();
        public List<SChanalCI> Channal = new List<SChanalCI>();//список обнаруженных датчиков
        string FileName = "CI_Result.txt";

        public CResultCI(int ChannalCount, int[] FN)
        {
            StreamWriter fs;
            for (int i = 0; i < ChannalCount; i++)
            {
                SChanalCI ch = new SChanalCI(i + 1, FN[i]);
                Channal.Add(ch);
                Directory.CreateDirectory("CI");
                string filename = string.Format("CI/CI_Result{0}.txt", ch.ChannalNummber);
                fs = File.CreateText(filename);//создаем файл канала
                fs.WriteLine(string.Format("Результаты чтения ЦАП датчика в канале {0}, заводской номер {1}", ch.ChannalNummber, ch.FactoryNumber));
                fs.WriteLine("Дата               |" +
                            "Температура   |" +
                            "Ток 4мА       |" +
                            "Ток 20мА      |");
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
        }

        private string GetStringFromPoint(SPointCI point)
        {
            return point.Datetime.ToString() + "|" +
                point.Temperature.ToString("     +0000.00;     -0000.00;          0.0") + " |" +
                point.I4.ToString("     +0000.00;     -0000.00;          0.0") + " |" +
                point.I20.ToString("     +0000.00;     -0000.00;          0.0") + " |";
        }
        //Сохранение в текстовый файл
        public void SaveToFile()
        {
            StreamWriter writer;
            if (!File.Exists(FileName))
            {
                writer = File.CreateText(FileName);//создаем файл БД
                if (writer != null)
                {
                    try
                    {
                        writer.WriteLine(string.Format("Файл данных ЦАП датчиков"));
                        writer.WriteLine("Дата и время       |" +
                                        "Зав. номер    |" +
                                        "Номер канала  |" +
                                        "Температура   |" +
                                        "Ток 4мА       |" +
                                        "Ток 20мА      |");
                    }
                    catch
                    {
                        writer.Close();
                        writer = null;
                    }
                }
                else
                {
                    Program.txtlog.WriteLineLog("CI:Ошибка создания файла результатов ЦАП!", 1);
                    return;
                }
            }
            else
            {
                writer = new StreamWriter(FileName, true);//открываем файл БД
            }

            if (writer != null)
            {
                try
                {
                    for (int i = 0; i < Channal.Count; i++)//перебор каналов
                    {
                        SChanalCI ch = Channal[i];
                        for (int j = 0; j < ch.Points.Count; j++)//перебор точек измерения для датчика
                        {
                            string str = ch.Points[j].Datetime.ToString() + "|" +
                                         ch.FactoryNumber.ToString("      0000000") + " |" +
                                         ch.ChannalNummber.ToString("           00") + " |" +
                                         ch.Points[j].Temperature.ToString("     +0000.00;     -0000.00;          0.0") + " |" +
                                         ch.Points[j].I4.ToString("     +0000.00;     -0000.00;          0.0") + " |" +
                                         ch.Points[j].I20.ToString("     +0000.00;     -0000.00;          0.0") + " |";
                            writer.WriteLine(str);
                        }
                    }
                }
                finally
                {
                    writer.Close();
                    writer = null;
                }
            }
            else
            {
                Program.txtlog.WriteLineLog("CI:Ошибка доступа к файлу результатов ЦАП!", 1);
            }
        }

    }
}
