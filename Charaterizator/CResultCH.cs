using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charaterizator
{
    struct SPoint
    {
        public DateTime Datetime;
        public float Temperature;
        public float Pressure;
        public float OutVoltage;
        public float Resistance;
        public float OutCurrent;
    }
    struct SChanal
    {
        public List<SPoint> Points;
        public int PointsCount;    //количество точек измерения в канале
        public int ChannalNummber;//номер канала
        public SChanal(int ChNum)
        {
            ChannalNummber = ChNum;
            Points = new List<SPoint>();
            PointsCount = 0;
        }
    }

    class CResultCH
    {
        public List<SChanal> Channal = new List<SChanal>();//список обнаруженных датчиков
        public CResultCH(int ChannalCount)
        {
            for (int i = 0; i < ChannalCount; i++)
            {
                SChanal ch = new SChanal(i);
                Channal.Add(ch);
            }
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
        }

    }
}
