using System;
using System.Collections.Generic;
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
        public List<SPoint> Points;
        public SChanal(int ChNum, int FN)
        {
            ChannalNummber = ChNum;
            FactoryNumber = FN;
            Points = new List<SPoint>();
        }
    }

    //Класс результатов измерения параметров датчика при характеризации
    class СResultCH
    {
        //StreamWriter[] FileStream;//поток записи
        public List<StreamWriter> FileStream = new List<StreamWriter>();
        public List<SChanal> Channal = new List<SChanal>();//список обнаруженных датчиков
        string FileName = "CH_Result.txt";

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
                string filename = string.Format("CH/CH_Result{0}.txt",ch.ChannalNummber);
                fs = File.CreateText(filename);//создаем файл канала
                fs.WriteLine(string.Format("Результаты характеризации датчика в канале {0}, заводской номер {1}", ch.ChannalNummber, ch.FactoryNumber));
                fs.WriteLine(   "Дата и время       |" +
                                "Температура   |" +
                                "Диапазон      |" +
                                "Давление      |" +
                                "Напряжение    |" +
                                "Сопротивление |");
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
                point.Temperature.ToString("     +0000.00;     -0000.00;          0.0") + " |" +
                point.Diapazon.ToString("           00") + " |" +
                point.Pressure.ToString("   +00000.00;   -00000.00;         0.0") + " |" +
                point.OutVoltage.ToString("    +0000.00;    -0000.00;         0.0") + " |" +
                point.Resistance.ToString("     00000.00") + " |";
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
                        writer.WriteLine(string.Format("Файл данных характеризации датчиков"));
                        writer.WriteLine("Дата и время       |" +
                                        "Зав. номер    |" +
                                        "Номер канала  |" +
                                        "Температура   |" +
                                        "Диапазон      |" +
                                        "Давление      |" +
                                        "Напряжение    |" +
                                        "Сопротивление |");
                    }
                    catch
                    {
                        writer.Close();
                        writer = null;
                    }
                }
                else
                {
                    Program.txtlog.WriteLineLog("CH:Ошибка создания файла результатов характеризации!", 1);
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
                        SChanal ch = Channal[i];
                        for (int j = 0; j < ch.Points.Count; j++)//перебор точек измерения для датчика
                        {
                            string str = ch.Points[j].Datetime.ToString() + "|" +
                                         ch.FactoryNumber.ToString("      0000000") + " |" +
                                         ch.ChannalNummber.ToString("           00") + " |" +
                                         ch.Points[j].Temperature.ToString("     +0000.00;     -0000.00;          0.0") + " |" +
                                         ch.Points[j].Diapazon.ToString("           00") + " |" +
                                         ch.Points[j].Pressure.ToString("    +00000.00;    -00000.00;          0.0") + " |" +
                                         ch.Points[j].OutVoltage.ToString("     +0000.00;     -0000.00;          0.0") + " |" +
                                         ch.Points[j].Resistance.ToString("     00000.00") + " |";
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
                Program.txtlog.WriteLineLog("CH:Ошибка доступа к файлу результатов характеризации!", 1);
            }
        }
    }
}
