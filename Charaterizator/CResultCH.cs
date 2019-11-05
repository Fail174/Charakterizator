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
        public float Temperature;
        public float Pressure;
        public float OutVoltage;
        public float Resistance;
        public float OutCurrent;
    }

    //структура канала с датчиком, включает множество точек измерения
    struct SChanal
    {
        public List<SPoint> Points;
        public int PointsCount;   //количество точек измерения в канале
        public int ChannalNummber;//номер канала
        public SChanal(int ChNum)
        {
            ChannalNummber = ChNum;
            Points = new List<SPoint>();
            PointsCount = 0;
        }
    }

    //Класс результатов измерения параметров датчика при характеризации
    class CResultCH
    {
        StreamWriter FileStream = null;//поток записи
        public List<SChanal> Channal = new List<SChanal>();//список обнаруженных датчиков
        public CResultCH(int ChannalCount)
        {
            for (int i = 0; i < ChannalCount; i++)
            {
                SChanal ch = new SChanal(i);
                Channal.Add(ch);
            }
            FileStream = File.CreateText("CH_Result.txt");//создаем файл сессии
            FileStream.WriteLine("№ Канала" + " |" +
                            "Дата          |" +
                            "Температура   |" +
                            "Давление      |" +
                            "Напряжение    |" +
                            "Сопротивление |" +
                            "Ток           |");
        }

        ~CResultCH()
        {
            //FileStream.Close();
        }

        public void AddPoint(int ch, float Temp, float Press, float U, float R, float I)
        {
            SPoint point = new SPoint();
            point.Datetime = DateTime.Now;
            point.Temperature = Temp;
            point.Pressure = Press;
            point.OutVoltage = U;
            point.Resistance = R;
            point.OutCurrent = I;

            Channal[ch].Points.Add(point);
            FileStream.WriteLine(GetStringFromPoint(ch+1, point));
            FileStream.Flush();
        }

        private string GetStringFromPoint(int i, SPoint point)
        {
            return  "Канал " + i.ToString() + "|" +
                point.Datetime.ToString() + "|" +
                point.Temperature.ToString("f") + "|" +
                point.Pressure.ToString("f") + "|" +
                point.OutVoltage.ToString("f") + "|" +
                point.Resistance.ToString("f") + "|" +
                point.OutCurrent.ToString("f") + "|";
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
                                    "Напряжение    |" +
                                    "Сопротивление |" +
                                    "Ток           |");
                    for (int i = 0; i < Channal.Count; i++)//перебор каналов
                    {
                        SChanal ch = new SChanal(i);
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
            }
        }
    }
}
