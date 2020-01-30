using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charaterizator
{
    //Структура точки верификации датчика
    struct SPointVR
    {
        public DateTime Datetime;
        public double Temperature;
        public int Diapazon;
        public double PressureZ;
        public double PressureF;
        public double CurrentF;
    }

    //структура канала с датчиком, включает множество точек измерения
    struct SChanalVR
    {
        public int ChannalNummber;//номер канала
        public int FactoryNumber;//заводской номер датчика
        public string FileNameArchiv;
        public List<SPointVR> Points;
        public SChanalVR(int ChNum, int FN)
        {
            ChannalNummber = ChNum;
            FactoryNumber = FN;
            FileNameArchiv = string.Format("Archiv/VR/VR_Ch{0}_F{1}.txt", ChannalNummber, FactoryNumber);
            Points = new List<SPointVR>();
        }
    }

    class CResultVR
    {
        //StreamWriter[] FileStream;//поток записи
        public List<StreamWriter> FileStream = new List<StreamWriter>();
        public List<SChanalVR> Channal = new List<SChanalVR>();//список обнаруженных датчиков
        private string HeaderString = "Дата и время       |" +
                                    "Температура   |" +
                                    "Диапазон      |" +
                                    "Давление (з)  |" +
                                    "Давление (ф)  |" +
                                    "Ток (ф)       |";

        //конструктор класса
        //вход: число каналов и заводской номер датчика в каждом канале
        public CResultVR(int ChannalCount, int[] FN)
        {
            StreamWriter fs;
            for (int i = 0; i < ChannalCount; i++)
            {
                SChanalVR ch = new SChanalVR(i + 1, FN[i]);
                Channal.Add(ch);
                Directory.CreateDirectory("VR");
                Directory.CreateDirectory("Archiv/VR");
                string filename = string.Format("VR/VR_Result{0}.txt", ch.ChannalNummber);
                fs = File.CreateText(filename);//создаем файл канала
                fs.WriteLine(string.Format("Результаты верификации датчика в канале {0}, заводской номер {1}", ch.ChannalNummber, ch.FactoryNumber));
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

        public void AddPoint(int ch, double Temp, int D, double PressZ, double PressF, double CurF)
        {
            try
            {
                SPointVR point = new SPointVR
                {
                    Datetime = DateTime.Now,
                    Temperature = Temp,
                    Diapazon = D,
                    PressureZ = PressZ,
                    PressureF = PressF,
                    CurrentF = CurF,
                };
                Channal[ch].Points.Add(point);
                //                FileStream[ch].
                FileStream[ch].WriteLine(GetStringFromPoint(point));
                FileStream[ch].Flush();
            }
            catch
            {
                Program.txtlog.WriteLineLog(string.Format("VR:Ошибка записи в файл результатов верификации (канал {0})", ch), 1);
            }
        }

        //возвращает строку результатов характеризации в точке
        private string GetStringFromPoint(SPointVR point)
        {
            return  point.Datetime.ToString() + "|" +
                point.Temperature.ToString("     +0000.00;     -0000.00;          0.0") + " |" +
                point.Diapazon.ToString("           00") + " |" +
                point.PressureZ.ToString("  +00000.0000;  -00000.0000;          0.0") + " |" +
                point.PressureF.ToString("  +00000.0000;  -00000.0000;          0.0") + " |" +
                point.CurrentF.ToString("  +00000.0000;  -00000.0000;          0.0") + " |";
        }


        //Сохранение в текстовый файл
        public void SaveToFile()
        {
            StreamWriter writer;

            try
            {
                for (int i = 0; i < Channal.Count; i++)//перебор каналов
                {
                    SChanalVR ch = Channal[i];
                    if (ch.Points.Count <= 0) continue;

                    writer = File.CreateText(ch.FileNameArchiv);//создаем файл БД
                    if (writer != null)
                    {
                        writer.WriteLine(string.Format("Файл данных верификации датчика"));
                        writer.WriteLine(string.Format("Канал:{0}; Заводской номер:{1}", ch.ChannalNummber, ch.FactoryNumber));
                        writer.WriteLine(HeaderString);
                        writer.WriteLine("--------------------------------------------------------------------------------------------");
                        for (int j = 0; j < ch.Points.Count; j++)//перебор точек измерения для датчика
                        {
                            writer.WriteLine(GetStringFromPoint(ch.Points[j]));
                        }
                        writer.Close();
                        writer = null;
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("VR:Ошибка создания файла данных верификации: " + ch.FileNameArchiv, 1);
                        continue;
                    }
                }
            }
            catch
            {
                Program.txtlog.WriteLineLog("VR:Критическая ошибка записи в архив верификации!", 1);
            }
        }
    }
}
