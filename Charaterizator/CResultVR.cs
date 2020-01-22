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
        public List<SPointVR> Points;
        public SChanalVR(int ChNum, int FN)
        {
            ChannalNummber = ChNum;
            FactoryNumber = FN;
            Points = new List<SPointVR>();
        }
    }

    class CResultVR
    {
        //StreamWriter[] FileStream;//поток записи
        public List<StreamWriter> FileStream = new List<StreamWriter>();
        public List<SChanalVR> Channal = new List<SChanalVR>();//список обнаруженных датчиков
        string FileName = "VR_Result.txt";

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
                string filename = string.Format("VR/VR_Result{0}.txt", ch.ChannalNummber);
                fs = File.CreateText(filename);//создаем файл канала
                fs.WriteLine(string.Format("Результаты верификации датчика в канале {0}, заводской номер {1}", ch.ChannalNummber, ch.FactoryNumber));
                fs.WriteLine("Дата               |" +
                                "Температура   |" +
                                "Диапазон      |" +
                                "Давление (з)  |" +
                                "Давление (ф)  |" +
                                "Ток (ф)       |");
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
            return point.Datetime.ToString() + "|" +
                point.Temperature.ToString("f11") + "|" +
                point.Diapazon.ToString("f11") + "|" +
                point.PressureZ.ToString("f11") + "|" +
                point.PressureF.ToString("f11") + "|" +
                point.CurrentF.ToString("f11") + "|";
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
                        writer.WriteLine(string.Format("Файл данных верификации датчиков"));
                        writer.WriteLine("Дата и время       |" +
                                        "Зав. номер    |" +
                                        "Номер канала  |" +
                                        "Температура   |" +
                                        "Диапазон      |" +
                                        "Давление (з)  |" +
                                        "Давление (ф)  |" +
                                        "Ток (ф)       |");
                    }
                    catch
                    {
                        writer.Close();
                        writer = null;
                    }
                }
                else
                {
                    Program.txtlog.WriteLineLog("VR:Ошибка создания файла результатов верификации!", 1);
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
                        SChanalVR ch = Channal[i];
                        for (int j = 0; j < ch.Points.Count; j++)//перебор точек измерения для датчика
                        {
                            string str = ch.Points[j].Datetime.ToString() + "|" +
                                         ch.FactoryNumber.ToString("      00000000") + "|" +
                                         ch.ChannalNummber.ToString("           000") + "|" +
                                         ch.Points[j].Temperature.ToString("    0000.0000") + "|" +
                                         ch.Points[j].Diapazon.ToString("          0000") + "|" +
                                         ch.Points[j].PressureZ.ToString("    00000.0000") + "|" +
                                         ch.Points[j].PressureF.ToString("    00000.0000") + "|" +
                                         ch.Points[j].CurrentF.ToString("    00000.0000") + "|";
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
                Program.txtlog.WriteLineLog("VR:Ошибка доступа к файлу результатов верификации!", 1);
            }
        }
    }
}
