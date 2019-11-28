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
                fs.WriteLine(   "Дата               |" +
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
                point.Temperature.ToString("f11") + "|" +
                point.Diapazon.ToString("f11") + "|" +
                point.Pressure.ToString("f11") + "|" +
                point.OutVoltage.ToString("f11") + "|" +
                point.Resistance.ToString("f11") + "|";
        }
        //Сохранение в текстовый файл
        public void SaveToFile(string FileName)
        {
          /*  StreamWriter writer = File.CreateText(FileName);//создаем файл сессии
            if (writer != null)
            {
                try
                {
                    writer.WriteLine("Дата          |" +
                                    "Температура   |" +
                                    "Давление      |" +
                                    "Напряжение    |" +
                                    "Сопротивление |" +
                                    "Ток           |");
                    for (int i = 0; i < Channal.Count; i++)//перебор каналов
                    {
                        SChanal ch = Channal[i];
                        for (int j = 0; j < ch.Points.Count; j++)//перебор точек измерения для датчика
                        {
                            writer.WriteLine(GetStringFromPoint(i+1, ch.Points[j]));
                        }
                    }
                }
                finally
                {
                    writer.Close();
                    writer = null;
                }
            }*/
        }
    }
}
