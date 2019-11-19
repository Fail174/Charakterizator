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
        public float Temperature;
        public float Pressure;
        public float I4;
        public float I20;
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
        public CResultCI(int ChannalCount, int[] FN)
        {
            StreamWriter fs;
            for (int i = 0; i < ChannalCount; i++)
            {
                SChanalCI ch = new SChanalCI(i + 1, FN[i]);
                Channal.Add(ch);
                string filename = string.Format("CI/CI_Result{0}.txt", ch.ChannalNummber);
                fs = File.CreateText(filename);//создаем файл канала
                fs.WriteLine(string.Format("Результаты калибровки датчика в канале {0}, заводской номер {1}", ch.ChannalNummber, ch.FactoryNumber));
                fs.WriteLine("Дата          |" +
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

        public void AddPoint(int ch, float Temp, float Press, float I1, float I2)
        {
            SPointCI point = new SPointCI();
            point.Datetime = DateTime.Now;
            point.Temperature = Temp;
            point.Pressure = Press;
            point.I4 = I1;
            point.I20 = I2;

            Channal[ch].Points.Add(point);
            FileStream[ch].WriteLine(GetStringFromPoint(point));
            FileStream[ch].Flush();
        }

        private string GetStringFromPoint(SPointCI point)
        {
            return point.Datetime.ToString() + "|" +
                point.Temperature.ToString("f") + "|" +
//                point.Pressure.ToString("f") + "|" +
                point.I4.ToString("f") + "|" +
                point.I20.ToString("f") + "|";
        }
        //Сохранение в текстовый файл
        public void SaveToFile(string FileName)
        {
            /*StreamWriter writer = File.CreateText(FileName);//создаем файл сессии
            if (writer != null)
            {
                try
                {
                    writer.WriteLine("№ Канала" + " |" +
                                    "Дата          |" +
                                    "Температура   |" +
                                    "Давление      |" +
                                    "Ток 4мА       |" +
                                    "Ток 20мА      |");
                    for (int i = 0; i < Channal.Count; i++)//перебор каналов
                    {
                        SChanalCI ch = new SChanalCI(i);
                        for (int j = 0; j < ch.Points.Count; j++)//перебор точек измерения для датчика
                        {
                            writer.WriteLine(GetStringFromPoint(i + 1, ch.Points[j]));
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
