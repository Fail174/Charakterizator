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
        //        public int PointsCount;   //количество точек измерения в канале
        public int ChannalNummber;//номер канала
        public SChanalCI(int ChNum)
        {
            ChannalNummber = ChNum;
            Points = new List<SPointCI>();
            //PointsCount = 0;
        }
    }

    class CResultCI
    {
        StreamWriter FileStream = null;//поток записи
        public List<SChanalCI> Channal = new List<SChanalCI>();//список обнаруженных датчиков
        public CResultCI(int ChannalCount)
        {
            for (int i = 0; i < ChannalCount; i++)
            {
                SChanalCI ch = new SChanalCI(i);
                Channal.Add(ch);
            }
            FileStream = File.CreateText("CI_Result.txt");//создаем файл сессии
            FileStream.WriteLine("№ Канала" + " |" +
                            "Дата          |" +
                            "Температура   |" +
                            "Давление      |" +
                            "Ток 4мА       |" +
                            "Ток 20мА      |");
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
            FileStream.WriteLine(GetStringFromPoint(ch + 1, point));
            FileStream.Flush();
        }

        private string GetStringFromPoint(int i, SPointCI point)
        {
            return "Канал " + i.ToString() + "|" +
                point.Datetime.ToString() + "|" +
                point.Temperature.ToString("f") + "|" +
                point.Pressure.ToString("f") + "|" +
                point.I4.ToString("f") + "|" +
                point.I20.ToString("f") + "|";
        }
        //Сохранение в текстовый файл
        public void SaveToFile(string FileName)
        {
            StreamWriter writer = File.CreateText(FileName);//создаем файл сессии
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
            }
        }

    }
}
