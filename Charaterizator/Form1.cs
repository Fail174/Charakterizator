﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
//using ENI100;



namespace Charaterizator
{

    public partial class MainForm : Form
    {
        const int MAX_ERROR_COUNT = 3; //Количество ошибок чтения данных с устройств перед отключением
        const int MaxChannalCount = 32;//максимальное количество каналов коммутаторы
        const int MaxLevelCount = 4;//максимальное количество уровней датчиков (идентичных групп)
        const int MENSOR_PRESSUER_WAIT = 100;//время установления давления в датчиках в сек

        public static int SettingsSelIndex { set; get; }


        private readonly Font DrawingFont = new Font(new FontFamily("DS-Digital"), 28.0F);
        private CMultimetr Multimetr = new CMultimetr();
        private ClassEni100 sensors = new ClassEni100();
        private FormSwitch Commutator = new FormSwitch();
        private FormMensor Mensor = new FormMensor();
        public static FormSensorsDB SensorsDB = new FormSensorsDB();
        private CThermalCamera ThermalCamera = new CThermalCamera();

        //        private int MaxChannalCount = 30;//максимальное количество каналов коммутаторы

//        private СResultCH ResultCH = new СResultCH(MaxChannalCount);//результаты характеризации датчиков
//        private CResultCI ResultCI = new CResultCI(MaxChannalCount);//результаты характеризации датчиков
        private СResultCH ResultCH = null;//результаты характеризации датчиков
        private CResultCI ResultCI = null;//результаты калибровки тока датчиков
        private CResultVR ResultVR = null;//результаты калибровки тока датчиков
        


        private int MultimetrReadError = 0;//число ошибко чтения данных с мультиметра
        private int MensorReadError = 0;//число ошибко чтения данных с менсора
        private bool SensorBusy = false;//Признак обмена данными с датчиками

        private bool TemperatureReady=false;//готовность термокамеры , температура датчиков стабилизирована
        private bool PressureReady = false;//готовность менсора , давление в датчиках стабилизировано

        private double SKOPressure = 1;//допуск по давлению
        private int WaitPressureTime = 10;//ожидание стабилизации давления в датчике, сек

        private int SelectedLevel = 1;//выбранный номер уровеня характеризации

        //Инициализация переменных основной программы
        public MainForm()
        {
            InitializeComponent();
            Program.txtlog = new CTxtlog(rtbConsole, "Charakterizator.log");//создаем класс лог, с выводов в richtextbox и в файл

            //            writer = File.CreateText("Charakterizator.log");//создаем лог файл сессии

            //            btmMultimetr_Click(null, null);           
            //            btnCommutator_Click(null, null);
            //            btnMensor_Click(null, null);

            //********************  Цифровой шрифт *********************
            tbDateTime.Font = DrawingFont;
            tbMensorData.Font = DrawingFont;
            tbNumCH.Font = DrawingFont;
            tbMultimetrData.Font = DrawingFont;
            tbMensorRate.Font = DrawingFont;
            tbTemperature.Font = DrawingFont;
            numMensorPoint.Font = DrawingFont;
            numTermoCameraPoint.Font = DrawingFont;
            //**********************************************************

            //            Properties.Settings.Default.Save();  // Сохраняем настройки программы
        }

        //Выполняем при загрузке главной формы
        private void MainForm_Load(object sender, EventArgs e)
        {
            // устанавливаем связь с БД
            string strFileNameDB = Charaterizator.Properties.Settings.Default.FileNameDB;   // получаем путь и имя файла из Settings
            SensorsDB.SetConnectionDB(strFileNameDB);                                  // устанавливаем соединение с БД           

            btmMultimetr.PerformClick();
            btnCommutator.PerformClick();
            btnMensor.PerformClick();
            btnThermalCamera.PerformClick();
            Application.DoEvents();

            for (int i = 0; i < MaxChannalCount; i++)
            {
                dataGridView1.Rows.Add(i + 1, "Нет данных", "Нет данных", false, false, false);
                dataGridView1[4, i].Style.BackColor = Color.IndianRed;
                dataGridView1[5, i].Style.BackColor = Color.IndianRed;
                dataGridView1[6, i].Style.BackColor = Color.IndianRed;
//                dataGridView2.Rows.Add("", "", "", "", "", "");
//                dataGridView3.Rows.Add("", "", "", "", "");

                cbChannalCharakterizator.Items.Add("Канал " + (i+1).ToString());
                cbChannalVerification.Items.Add("Канал " + (i + 1).ToString());
            }

            MainTimer.Enabled = true;
            MainTimer.Start();

        }



        private void ToolStripMenuItem_MultimetrSetings_Click(object sender, EventArgs e)
        {
            FormPortSettings newForm = new FormPortSettings();
            newForm.InitPortsettings(Properties.Settings.Default.COMMultimetr,
                Properties.Settings.Default.COMMultimetr_Speed,
                Properties.Settings.Default.COMMultimetr_DatabBits,
                Properties.Settings.Default.COMMultimetr_StopBits,
                Properties.Settings.Default.COMMultimetr_Parity);
            if (newForm.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.COMMultimetr = newForm.GetPortName();
                Properties.Settings.Default.COMMultimetr_Speed = newForm.GetPortSpeed();
                Properties.Settings.Default.COMMultimetr_DatabBits = newForm.GetPortDataBits();
                Properties.Settings.Default.COMMultimetr_StopBits = newForm.GetPortStopBits();
                Properties.Settings.Default.COMMultimetr_Parity = newForm.GetPortParity();
                Properties.Settings.Default.Save();  // Сохраняем переменные.
            }
        }

        private void MI_MensorSetings_Click(object sender, EventArgs e)
        {
            FormPortSettings newForm = new FormPortSettings();
            newForm.InitPortsettings(Properties.Settings.Default.COMMensor,
                Properties.Settings.Default.COMMensor_Speed,
                Properties.Settings.Default.COMMensor_DataBits,
                Properties.Settings.Default.COMMensor_StopBits,
                Properties.Settings.Default.COMMensor_Parity);
            if (newForm.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.COMMensor = newForm.GetPortName();
                Properties.Settings.Default.COMMensor_Speed = newForm.GetPortSpeed();
                Properties.Settings.Default.COMMensor_DataBits = newForm.GetPortDataBits();
                Properties.Settings.Default.COMMensor_StopBits = newForm.GetPortStopBits();
                Properties.Settings.Default.COMMensor_Parity = newForm.GetPortParity();
                Properties.Settings.Default.Save();  // Сохраняем переменные.
            }
        }

        private void MI_CommutatorSetings_Click(object sender, EventArgs e)
        {
            FormPortSettings newForm = new FormPortSettings();
            newForm.InitPortsettings(Properties.Settings.Default.COMComutator,
                Properties.Settings.Default.COMComutator_Speed,
                Properties.Settings.Default.COMComutator_DataBits,
                Properties.Settings.Default.COMComutator_StopBits,
                Properties.Settings.Default.COMComutator_Parity);
            if (newForm.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.COMComutator = newForm.GetPortName();
                Properties.Settings.Default.COMComutator_Speed = newForm.GetPortSpeed();
                Properties.Settings.Default.COMComutator_DataBits = newForm.GetPortDataBits();
                Properties.Settings.Default.COMComutator_StopBits = newForm.GetPortStopBits();
                Properties.Settings.Default.COMComutator_Parity = newForm.GetPortParity();
                Properties.Settings.Default.Save();  // Сохраняем переменные.
            }
        }

        private void MI_ColdCameraSetings_Click(object sender, EventArgs e)
        {
            FormPortSettings newForm = new FormPortSettings();
            newForm.InitPortsettings(Properties.Settings.Default.COMColdCamera,
                Properties.Settings.Default.COMColdCamera_Speed,
                Properties.Settings.Default.COMColdCamera_DataBits,
                Properties.Settings.Default.COMColdCamera_StopBits,
                Properties.Settings.Default.COMColdCamera_Parity);
            if (newForm.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.COMColdCamera = newForm.GetPortName();
                Properties.Settings.Default.COMColdCamera_Speed = newForm.GetPortSpeed();
                Properties.Settings.Default.COMColdCamera_DataBits = newForm.GetPortDataBits();
                Properties.Settings.Default.COMColdCamera_StopBits = newForm.GetPortStopBits();
                Properties.Settings.Default.COMColdCamera_Parity = newForm.GetPortParity();
                Properties.Settings.Default.Save();  // Сохраняем переменные.
            }
        }

        private void MI_SensorSetings_Click(object sender, EventArgs e)
        {
            FormPortSettings newForm = new FormPortSettings();
            newForm.InitPortsettings(Properties.Settings.Default.COMSensor,
                Properties.Settings.Default.COMSensor_Speed,
                Properties.Settings.Default.COMSensor_DataBits,
                Properties.Settings.Default.COMSensor_StopBits,
                Properties.Settings.Default.COMSensor_Parity);
            if (newForm.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.COMSensor = newForm.GetPortName();
                Properties.Settings.Default.COMSensor_Speed = newForm.GetPortSpeed();
                Properties.Settings.Default.COMSensor_DataBits = newForm.GetPortDataBits();
                Properties.Settings.Default.COMSensor_StopBits = newForm.GetPortStopBits();
                Properties.Settings.Default.COMSensor_Parity = newForm.GetPortParity();
                Properties.Settings.Default.Save();  // Сохраняем переменные.
            }
        }
        
        //подключение термокамеры
        private void btnThermalCamera_Click(object sender, EventArgs e)
        {
            if (ThermalCamera.Connect(Properties.Settings.Default.COMColdCamera,
                Properties.Settings.Default.COMColdCamera_Speed,
                Properties.Settings.Default.COMColdCamera_DataBits,
                Properties.Settings.Default.COMColdCamera_StopBits,
                Properties.Settings.Default.COMColdCamera_Parity) >= 0)
            {
                btnThermalCamera.BackColor = Color.Green;
                btnThermalCamera.Text = "Подключен";
                Program.txtlog.WriteLineLog("Термокамера подключена", 0);
            }
            else
            {
                btnThermalCamera.BackColor = Color.IndianRed;
                btnThermalCamera.Text = "Не подключен";
                Program.txtlog.WriteLineLog("Термокамера не подключена", 1);
            }

        }

        //подключение мультиметра
        private void btmMultimetr_Click(object sender, EventArgs e)
        {
            if (Multimetr.Connect(Properties.Settings.Default.COMMultimetr,
                Properties.Settings.Default.COMMultimetr_Speed,
                Properties.Settings.Default.COMMultimetr_DatabBits,
                Properties.Settings.Default.COMMultimetr_StopBits,
                Properties.Settings.Default.COMMultimetr_Parity) >= 0)
            {
                btmMultimetr.BackColor = Color.Green;
                btmMultimetr.Text = "Подключен";
                Program.txtlog.WriteLineLog("Мультиметр подключен", 0);
            }
            else
            {
                btmMultimetr.BackColor = Color.IndianRed;
                btmMultimetr.Text = "Не подключен";
                Program.txtlog.WriteLineLog("Мультиметр не подключен", 1);
            }
        }



        // Подключение КОММУТАТОРА
        private void btnCommutator_Click(object sender, EventArgs e)
        {
            if (Commutator.Connect(Properties.Settings.Default.COMComutator,
               Properties.Settings.Default.COMComutator_Speed,
               Properties.Settings.Default.COMComutator_DataBits,
               Properties.Settings.Default.COMComutator_StopBits,
               Properties.Settings.Default.COMComutator_Parity) >= 0)
            {
                btnCommutator.BackColor = Color.Green;
                btnCommutator.Text = "Подключен";
                Program.txtlog.WriteLineLog("Коммутатор подключен", 0);
            }
            else
            {
                btnCommutator.BackColor = Color.IndianRed;
                btnCommutator.Text = "Не подключен";
                Program.txtlog.WriteLineLog("Коммутатор не подключен", 1);
            }
        }



        // Подключение МЕНСОРА
        private void btnMensor_Click(object sender, EventArgs e)
        {
            if (Mensor.Connect(Properties.Settings.Default.COMMensor,
              Properties.Settings.Default.COMMensor_Speed,
              Properties.Settings.Default.COMMensor_DataBits,
              Properties.Settings.Default.COMMensor_StopBits,
              Properties.Settings.Default.COMMensor_Parity) >= 0)
            {
                btnMensor.BackColor = Color.Green;
                btnMensor.Text = "Подключен";
                Program.txtlog.WriteLineLog("Задатчик давления подключен", 0);
            }
            else
            {
                btnMensor.BackColor = Color.IndianRed;
                btnMensor.Text = "Не подключен";
                Program.txtlog.WriteLineLog("Задатчик давления не подключен", 1);
            }
        }



        // Обработка нажатия кнопки управления КОММУТАТОРОМ
        // Открываем окно с интерфейсом коммутатора
        private void btnFormCommutator_Click(object sender, EventArgs e)
        {
            if (Commutator != null)
            {
                Commutator.ShowDialog();
            }
            else
            {
                Commutator = new FormSwitch();
                btnCommutator.PerformClick();

            }
        }



        // Обработка нажатия кнопки управления МЕНСОРОМ
        // Открываем окно с интерфейсом МЕНСОРА
        private void btnFormMensor_Click(object sender, EventArgs e)
        {
            if (Mensor != null)
            {
                Mensor.ShowDialog();
            }
            else
            {
                Mensor = new FormMensor();
                btnMensor.PerformClick();
            }
        }

        private void UpdateDataGrids(int i)
        {
            dataGridView1.Rows[i].Cells[1].Value = sensors.sensor.GetdevType() + " : " + new String(sensors.sensor.PressureModel);     //тип датчика
            dataGridView1.Rows[i].Cells[2].Value = sensors.sensor.uni.ToString();   //заводской номер
            dataGridView1.Rows[i].Cells[3].Value = true;   //Добавлен в список
//            dataGridView1.Rows[i].Cells[3].Style.BackColor = Color.Green;
            dataGridView1.Rows[i].Cells[6].Style.BackColor = Color.Green;
            dataGridView1.Rows[i].Cells[6].Value = true;                            //исправность датчика                            
        }
        private void UpdateSensorInfoPanel(int i)
        {
            if (sensors.SelectSensor(i))
            {
                tbSelChannalNumber.Text = string.Format("Канал {0}" , i+1);
                tbInfoDesc.Text = sensors.sensor.GetDesc();
                tbInfoTeg.Text = sensors.sensor.GetTeg();
                tbInfoPressureModel.Text = new String(sensors.sensor.PressureModel);
                tbInfoUp.Text = sensors.sensor.UpLevel.ToString("f6");
                tbInfoDown.Text = sensors.sensor.DownLevel.ToString("f6");
                tbInfoSerialNumber.Text = sensors.sensor.SerialNumber.ToString();
                tbInfoMin.Text = sensors.sensor.MinLevel.ToString("f6");
                tbInfoMesUnit.Text = sensors.sensor.GetUnit();
                //                DateTime dt = new DateTime(1900 + (int)(sensors.sensor.data & 0xFF), (int)(sensors.sensor.data >> 8) & 0xFF, (int)((sensors.sensor.data >> 16) & 0xFF));
                //                dtpInfoDate.Value = dt;

                tbInfoDeviceAdress.Text = sensors.sensor.Addr.ToString("D2");
                tbInfoFactoryNumber.Text = sensors.sensor.uni.ToString();
                tbInfoSoftVersion.Text = sensors.sensor.v3.ToString();
                cbInfoPreambul.Text = sensors.sensor.pre.ToString();
                tbInfoSensorType.Text = sensors.sensor.GetdevType();
                /*            string SelectedSensor = sensors.sensor.Addr.ToString("D2") + " | " + sensors.sensor.GetdevType() + " | " + sensors.sensor.uni;
                            tbCharact.Text = SelectedSensor;
                            tbCoef.Text = SelectedSensor;*/
            }
            else
            {
//                Program.txtlog.WriteLineLog("Датчик на выбранной линии не обнаружен!", 1);
                DialogResult result = MessageBox.Show(
                        "Выполнить поиск датчика?",
                        "Датчик на выбранной линии не обнаружен!",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.Yes)
                {
                    // коммутируем
                    Commutator.SetConnectors(i, 2); // команда подключить датчик с индексом i
                    if (sensors.IsConnect())
                    {
                        if (sensors.SeachSensor(i))//поиск датчиков
                        {
                            Thread.Sleep(100);
                            if (sensors.SelectSensor(i))//выбор обнаруженного датчика
                            {//датчик найден, обновляем таблицу
                                Program.txtlog.WriteLineLog("Датчик обнаружен! Выполняем чтение параметров датчика по HART.", 0);
                                sensors.TegRead();          //читаем инфомацию о датчике
                                sensors.SensorRead();       //чтение данных с датчика
                                sensors.C140ReadPressureModel();//читаем модель ПД
                                UpdateDataGrids(i);         //обновляем информацию по датчику в таблице
                                UpdateSensorInfoPanel(i);
                            }
                            else
                            {
                                Program.txtlog.WriteLineLog(string.Format("Датчик на линии {0} обнаружен. Ошибка подключения к датчику!", i + 1), 1);
                            }
                        }
                        else
                        {
                            //                            Program.txtlog.WriteLineLog(string.Format("Нет подключения! Поиск датчиков на линии {0} не выполнен!",i+1), 1);
                            Program.txtlog.WriteLineLog(string.Format("Датчики на линии {0} не обнаружены!", i + 1), 1);
                        }
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("Нет подключение к датчикам!", 1);
                    }

                    Commutator.SetConnectors(i, 3); // команда отключить датчик с индексом i                    
                }
            }
        }

        //калибровка по току
        private void ReadSensorCurrent()
        {
            int StartNumber = 0;    //начальный канал
            int FinishNumber = 0;   //конечный канал

            Program.txtlog.WriteLineLog("Старт операции чтения ЦАП ... ", 0);
            //******** расчитываем номера каналов текущего выбранного уровня ********************************
            int step = MaxChannalCount / MaxLevelCount;
            switch (SelectedLevel)
            {
                case 1:
                    StartNumber = 0;
                    FinishNumber = step - 1;
                    break;
                case 2:
                    StartNumber = step;
                    FinishNumber = step * 2 - 1;
                    break;
                case 3:
                    StartNumber = step * 2;
                    FinishNumber = step * 3 - 1;
                    break;
                case 4:
                    StartNumber = step * 3;
                    FinishNumber = step * 4 - 1;
                    break;
            }
            //************************************************************************************************

            pbCHProcess.Maximum = FinishNumber - StartNumber;
            pbCHProcess.Minimum = 0;
            pbCHProcess.Value = 0;
            for (int i = StartNumber; i <= FinishNumber; i++)//перебор каналов
            {
                pbCHProcess.Value = i - StartNumber;

                Commutator.SetConnectors(i, 2);

                if (sensors.SelectSensor(i))//выбор датчика на канале i
                {

                    double I4=0, I20=0;
                    sensors.sensor.CurrentExit = 0;//ток 4мА
                    if (sensors.C129WriteCurrenExit())
                    {
                        //                        ResultCI.AddPoint(i, sensors.sensor.Temperature, sensors.sensor.Pressure, sensors.sensor., sensors.sensor.Resistance, sensors.sensor.OutCurrent);
                        I4 = Multimetr.Current;
                        Program.txtlog.WriteLineLog("Выполнено чтение тока датчика с мультиметра в канале " + (i + 1).ToString(), 0);
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("Ток 4мА не установлен!", 1);
                    }
                    sensors.sensor.CurrentExit = 1;//ток 20мА
                    if (sensors.C129WriteCurrenExit())
                    {
                        I20 = Multimetr.Current;
                        Program.txtlog.WriteLineLog("Выполнено чтение тока датчика с мультиметра в канале " + (i + 1).ToString(), 0);
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("Ток 20мА не установлен!", 1);
                    }
                    cbChannalCharakterizator.SelectedIndex = i;
                    ResultCI.AddPoint(i, (double)numTermoCameraPoint.Value, I4, I20);

                    UpdateCurrentGrid(i);
                }
                else
                {
                    Program.txtlog.WriteLineLog("Датчик не найден в канале " + (i + 1).ToString(), 1);
                }
            }
        }

        //Проверка доступности канала(по выбору пользователя)
        private bool CheckChannalEnable(int i)
        {
            if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[3].Value))   //Добавлен в список?
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //характеризация датчиков
        //чтение всех измеренных параметров с текущего датчика давления
        private void ReadSensorParametrs()
        {
            int StartNumber=0;    //начальный канал
            int FinishNumber=0;   //конечный канал
            int Diapazon = 1;

            Program.txtlog.WriteLineLog("CH: Старт операции характеризации для выбранных датчиков ... ", 0);

            //******** расчитываем номера каналов текущего выбранного уровня ********************************
            int step = MaxChannalCount / MaxLevelCount;
            switch (SelectedLevel)
            {
                case 1:
                    StartNumber = 0;
                    FinishNumber = step - 1;
                    Diapazon = Convert.ToInt32(cbDiapazon1.Text);
                    break;
                case 2:
                    StartNumber = step;
                    FinishNumber = step * 2 - 1;
                    Diapazon = Convert.ToInt32(cbDiapazon2.Text);
                    break;
                case 3:
                    StartNumber = step * 2;
                    FinishNumber = step * 3 - 1;
                    Diapazon = Convert.ToInt32(cbDiapazon3.Text);
                    break;
                case 4:
                    StartNumber = step * 3;
                    FinishNumber = step * 4 - 1;
                    Diapazon = Convert.ToInt32(cbDiapazon4.Text);
                    break;
            }
            //************************************************************************************************

            pbCHProcess.Maximum = FinishNumber - StartNumber;
            pbCHProcess.Minimum = 0;
            pbCHProcess.Value = 0;

            for (int i = StartNumber; i <= FinishNumber; i++)//перебор каналов
            {
                pbCHProcess.Value = i - StartNumber;
                Application.DoEvents();
                if (!CheckChannalEnable(i)) continue;//Если канал не выбран пропускаем обработку

                Commutator.SetConnectors(i, 2);

                if (sensors.SelectSensor(i))//выбор датчика на канале i
                {
                    if (sensors.SensorValueReadC03())
                    {
                        ResultCH.AddPoint(i, (double)numTermoCameraPoint.Value, Diapazon, (double)numMensorPoint.Value, sensors.sensor.OutVoltage, sensors.sensor.Resistance);
                        cbChannalCharakterizator.SelectedIndex = i;
                        UpDateCharakterizatorGrid(i);
                        Program.txtlog.WriteLineLog("CH: Выполнено чтение параметров датчика в канале " + (i + 1).ToString(), 0);
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("CH: Параметры датчика не прочитаны!", 1);
                    }
                }
                else
                {
                    Program.txtlog.WriteLineLog("CH: Датчик не найден в канале " + (i + 1).ToString(), 1);
                }
            }
            Program.txtlog.WriteLineLog("CH: Операция характеризации завершена ... ", 0);
        }

        //верификация датчиков
        //чтение всех измеренных параметров с текущего датчика давления
        private void ReadSensorPressure()
        {
            int StartNumber = 0;    //начальный канал
            int FinishNumber = 0;   //конечный канал

            Program.txtlog.WriteLineLog("VR: Старт операции верификации для выбранных датчиков ... ", 0);

            //******** расчитываем номера каналов текущего выбранного уровня ********************************
            int step = MaxChannalCount / MaxLevelCount;
            int Diapazon=1;
            switch (SelectedLevel)
            {
                case 1:
                    StartNumber = 0;
                    FinishNumber = step - 1;
                    Diapazon = Convert.ToInt32(cbVRDiapazon1.Text);
                    break;
                case 2:
                    StartNumber = step;
                    FinishNumber = step * 2 - 1;
                    Diapazon = Convert.ToInt32(cbVRDiapazon2.Text);
                    break;
                case 3:
                    StartNumber = step * 2;
                    FinishNumber = step * 3 - 1;
                    Diapazon = Convert.ToInt32(cbVRDiapazon3.Text);
                    break;
                case 4:
                    StartNumber = step * 3;
                    FinishNumber = step * 4 - 1;
                    Diapazon = Convert.ToInt32(cbVRDiapazon4.Text);
                    break;
            }
            //************************************************************************************************

            pbVRProcess.Maximum = FinishNumber - StartNumber;
            pbVRProcess.Minimum = 0;
            pbVRProcess.Value = 0;
            for (int i = StartNumber; i <= FinishNumber; i++)//перебор каналов
            {
                    pbVRProcess.Value = i - StartNumber;
                    Application.DoEvents();

                    Commutator.SetConnectors(i, 2);

                if (sensors.SelectSensor(i))//выбор датчика на канале i
                {
                    if (sensors.SensorValueReadC03())
                    {
                        ResultVR.AddPoint(i, (double)numTermoCameraPoint.Value, Diapazon, (double)numMensorPoint.Value, sensors.sensor.Pressure, Multimetr.Current);
                        UpDateVerificationGrid(i);
                        Program.txtlog.WriteLineLog("VR: Выполнено чтение параметров датчика в канале " + (i + 1).ToString(), 0);
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("VR: Параметры датчика не прочитаны!", 1);
                    }
                }
                else
                {
                    Program.txtlog.WriteLineLog("VR: Датчик не найден в канале " + (i + 1).ToString(), 1);
                }
            }
            Program.txtlog.WriteLineLog("VR: Операция верификации завершена ... ", 0);
        }


        //обновляем грид результатов характеризации для датчика в канале i
        private void UpDateCharakterizatorGrid(int i)
        {
            dataGridView2.Rows.Clear();
            for (int j=0;j<ResultCH.Channal[i].Points.Count;j++)//заполняем грид данными текущего датчика
            {
                dataGridView2.Rows.Add("", "", "", "", "", "");
                dataGridView2.Rows[j].Cells[0].Value = ResultCH.Channal[i].Points[j].Datetime.ToString();                 //
                dataGridView2.Rows[j].Cells[1].Value = ResultCH.Channal[i].Points[j].Temperature.ToString();   //
                switch (SelectedLevel)
                {
                    case 1:
                        dataGridView2.Rows[j].Cells[2].Value = cbDiapazon1.Text;   //
                        break;
                    case 2:
                        dataGridView2.Rows[j].Cells[2].Value = cbDiapazon2.Text;   //
                        break;
                    case 3:
                        dataGridView2.Rows[j].Cells[2].Value = cbDiapazon3.Text;   //
                        break;
                    case 4:
                        dataGridView2.Rows[j].Cells[2].Value = cbDiapazon4.Text;   //
                        break;
                }

                dataGridView2.Rows[j].Cells[3].Value = ResultCH.Channal[i].Points[j].Pressure.ToString("f");   //
                dataGridView2.Rows[j].Cells[4].Value = ResultCH.Channal[i].Points[j].OutVoltage.ToString("f");
                dataGridView2.Rows[j].Cells[5].Value = ResultCH.Channal[i].Points[j].Resistance.ToString("f");
            }
        }

        //обновляем грид результатов верификации для датчика в канале i
        private void UpDateVerificationGrid(int i)
        {
            dataGridView3.Rows.Clear();
            for (int j = 0; j < ResultVR.Channal[i].Points.Count; j++)//заполняем грид данными текущего датчика
            {
                dataGridView3.Rows.Add("", "", "", "", "", "");
                dataGridView3.Rows[j].Cells[0].Value = ResultVR.Channal[i].Points[j].Datetime.ToString();      //
                dataGridView3.Rows[j].Cells[1].Value = ResultVR.Channal[i].Points[j].Temperature.ToString();   //
                switch (SelectedLevel)
                {
                    case 1:
                        dataGridView3.Rows[j].Cells[2].Value = cbVRDiapazon1.Text;   //
                        break;
                    case 2:
                        dataGridView3.Rows[j].Cells[2].Value = cbVRDiapazon2.Text;   //
                        break;
                    case 3:
                        dataGridView3.Rows[j].Cells[2].Value = cbVRDiapazon3.Text;   //
                        break;
                    case 4:
                        dataGridView3.Rows[j].Cells[2].Value = cbVRDiapazon4.Text;   //
                        break;
                }
                dataGridView3.Rows[j].Cells[3].Value = ResultVR.Channal[i].Points[j].PressureZ.ToString("f");
                dataGridView3.Rows[j].Cells[4].Value = ResultVR.Channal[i].Points[j].PressureF.ToString("f");
                dataGridView3.Rows[j].Cells[5].Value = ResultVR.Channal[i].Points[j].CurrentF.ToString("f");
            }
        }

        //обновляем грид калибровки тока для датчика в канале i
        private void UpdateCurrentGrid(int i)
        {
            dataGridView4.Rows.Clear();
            for (int j = 0; j < ResultCI.Channal[i].Points.Count; j++)//заполняем грид данными текущего датчика
            {
                dataGridView4.Rows.Add("", "", "", "", "");
                dataGridView4.Rows[j].Cells[0].Value = ResultCI.Channal[i].Points[j].Datetime.ToString();                 //
                dataGridView4.Rows[j].Cells[1].Value = ResultCI.Channal[i].Points[j].Temperature.ToString();   //
//                dataGridView4.Rows[j].Cells[2].Value = ResultCI.Channal[i].Points[j].Pressure.ToString("f");   //
                dataGridView4.Rows[j].Cells[2].Value = ResultCI.Channal[i].Points[j].I4.ToString("f");
                dataGridView4.Rows[j].Cells[3].Value = ResultCI.Channal[i].Points[j].I20.ToString("f");
            }
        }

        //Поиск подключенных датчиков
        //Формирует списко датчиков в датагриде
        //Выход: число подключенных датчиков
        private int SeachConnectedSensor()
        {
            bool SensorFind = false;
//            SensorBusy = true;
            try
            {
                Program.txtlog.WriteLineLog("Старт поиска датчиков...", 0);
                pbSensorSeach.Maximum = MaxChannalCount;
                pbSensorSeach.Minimum = 0;
                pbSensorSeach.Value = 0;

                sensors.sensorList.Clear();
                if (sensors.Connect(Properties.Settings.Default.COMSensor,
                    Properties.Settings.Default.COMSensor_Speed,
                    Properties.Settings.Default.COMSensor_DataBits,
                    Properties.Settings.Default.COMSensor_StopBits,
                    Properties.Settings.Default.COMSensor_Parity) >= 0)
                {
                    for (int i = 0; i < MaxChannalCount; i++)
                    {
                        if (!SensorBusy) break;//прекращаем поиск 

                        dataGridView1.Rows[i].Selected = true;
                        Application.DoEvents();
//                        if (!Convert.ToBoolean(dataGridView1.Rows[i].Cells[3].Value))   //Добавлен в список?
//                            continue;
                        if (!CheckChannalEnable(i)) continue;//Если канал не выбран пропускаем обработку


                        Program.txtlog.WriteLineLog(string.Format("Поиск датчиков на линии {0} ...", i+1), 0);

                        // коммутируем
                        Commutator.SetConnectors(i, 2); // команда подключить датчик с индексом i

                        //***** поиск датчиков по току потребления ********************************************
                        if (Multimetr.Connected)
                        {
                            Multimetr.ReadData();
                            double Current = Multimetr.Current;
                            if (Current == 0)//чтение тока мультиметра 
                            {
                                //нет тока мультиметра, => датчик отсутсвует
                                Program.txtlog.WriteLineLog("Датчик не обнаружен! Ток потребления: " + Current.ToString(), 1);
                                continue;
                            }
                            else
                            {
                                Program.txtlog.WriteLineLog("Датчик обнаружен! Ток потребления: " + Current.ToString(), 0);
                            }
                        }
                        //*************************************************************************************


                        if (sensors.SeachSensor(i))//поиск датчиков по HART
                        {
                            if (sensors.SelectSensor(i))//выбор обнаруженного датчика
                            {//датчик найден, обновляем таблицу
                                Program.txtlog.WriteLineLog("Датчик обнаружен! Выполняем чтение параметров датчика по HART.", 0);
                                sensors.TegRead();          //читаем информацию о датчике
                                sensors.SensorRead();       //чтение данных с датчика
                                sensors.C140ReadPressureModel();//читаем модель ПД
                                Thread.Sleep(500);          
                                sensors.ParseReadBuffer(500);//ждем завершения операций по датчику в потоке
                                UpdateDataGrids(i);         //обновляем информацию по датчику в таблице
                                SensorFind = true;
                            }
                            else
                            {
                                Program.txtlog.WriteLineLog(string.Format("Датчик на линии {0} обнаружен. Ошибка подключения к датчику!", i+1), 1);
                            }
                        }
                        else
                        {
//                            Program.txtlog.WriteLineLog(string.Format("Нет подключения! Поиск датчиков на линии {0} не выполнен!",i+1), 1);
                            Program.txtlog.WriteLineLog(string.Format("Датчики на линии {0} не обнаружены!", i + 1), 1);
                        }

                        Commutator.SetConnectors(i, 3); // команда отключить датчик с индексом i                    
                        pbSensorSeach.Value = i;
                    }
                }
                else
                {
                    Program.txtlog.WriteLineLog("Нет соединения с датчиками. Проверте подключение коммутатора.", 1);

                }
            }
            finally
            {
//                SensorBusy = false;
                pbSensorSeach.Value = 0;
            }
            if (SensorFind)
            {
                CreatSessionFiles();//создаем временные файла результатов
                return 0;
            }
            else
            {
                return -1;
            }
        }
        private void btnSensorSeach_Click_1(object sender, EventArgs e)
        {
            if (!SensorBusy)
            {
                try
                {
                    SensorBusy = true;
                    int i;
                    for (i = 0; i < MaxChannalCount; i++)
                    {
                        if (CheckChannalEnable(i)) //Есть выбранные каналы?
                            break;
                    }
                    if (i < MaxChannalCount)
                    {
                        //                        btnSensorSeach.Enabled = false;
                        btnSensorSeach.Text = "Идет поиск датчиков... Остановить! ";
                        SeachConnectedSensor();
                    }
                    else
                    {
                        MessageBox.Show("Не выбраны каналы для поиска датчиков","Операция прервана",MessageBoxButtons.OK);
                    }
                }
                finally
                {
                    //                    btnSensorSeach.Enabled = true;
                    btnSensorSeach.Text = "Поиск датчиков";
                    SensorBusy = false;
                }
            }
            else
            {
                SensorBusy = false;
                //                Program.txtlog.WriteLineLog("Команда не выполнена. Идет обмен данными с датчиками.",0);
                Program.txtlog.WriteLineLog("Поиск прекращен по команде пользователя", 0);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            MainTimer.Stop();
            MainTimer.Enabled = false;
            sensors.DisConnect();
            Mensor.DisConnect();
            Multimetr.DisConnect();
            Commutator.DisConnect();
        }



        private void MainTimer_Tick(object sender, EventArgs e)
        {

            if (Commutator.Connected)
            {
                StateComutators(Commutator._StateCH);
                PowerComutators(Commutator._StateCHPower); //обновляем данные с коммутатора
            }

            if (Multimetr.Connected)
            {
                ReadMultimetr(); //обновляем данные с мультиметра
            }


            if (Mensor.Connected)
            {
                ReadMensor(); //обновляем данные с Менсора
            }
            tbDateTime.Text = DateTime.Now.ToString();
        }


        //чтение данных с МЕНСОРА
        private void ReadMensor()
        {
            int CH_mensor = Mensor._activCH;    // Получаем номер активного канала (0 значит А,   1 значит B,   -1 = не прочитали )                    

            if (CH_mensor != -1)
            {
                // Получаем текущее значение давления и обновляем гл. форму 
                tbMensorData.Text = Mensor._press.ToString();
                // Получаем тек. значение уставки  и обновляем гл. форму
                numMensorPoint.Text = Mensor._point.ToString();
                // Получаем тек. значение скорости  и обновляем гл. форму
                tbMensorRate.Text = Mensor._rate.ToString();

                // Получаем тип преобразователя (удерживаемый диапазон)
                int typeR = Mensor._typeR;  // 0-Д1П1,  1-Д2П1,  2-AutoRange 
                // Обновляем тип преобзарователя
                if (CH_mensor == 0)
                {
                    cbMensorTypeR.SelectedIndex = typeR; // от 0 до 2-х по списку
                }
                else if (CH_mensor == 1)
                {
                    cbMensorTypeR.SelectedIndex = typeR + 3; // от 3 до 5-ти по списку
                }

                MensorReadError = 0;
            }
            else
            {
                tbMensorData.Text = "";
                tbMensorRate.Text = "";
                numMensorPoint.Text = "";
                cbMensorTypeR.SelectedIndex = -1;
                MensorReadError++;
            }

            if (MensorReadError >= MAX_ERROR_COUNT)
            {
                Mensor.DisConnect();
                btnMensor.BackColor = Color.IndianRed;
                btnMensor.Text = "Не подключен";
                Program.txtlog.WriteLineLog("Нет данных с задатчика давления. Устройство отключено.", 1);
            }
        }



        //чтение данных с мультиметра
        private void ReadMultimetr()
        {
            bool res = Multimetr.ReadData();
            if (res)
            {
                tbMultimetrData.Text = Multimetr.Current.ToString();
                MultimetrReadError = 0;
            }
            else
            {
                tbMultimetrData.Text = "";
                MultimetrReadError++;
            }
            if (MultimetrReadError >= MAX_ERROR_COUNT)
            {
                Multimetr.DisConnect();
                btmMultimetr.BackColor = Color.IndianRed;
                btmMultimetr.Text = "Не подключен";
                Program.txtlog.WriteLineLog("Нет данных с мультиметра. Устройство отключено.", 1);
            }
        }



        // Обновление на главной форме состояния каналов коммутатора (checkbox)
        private void StateComutators(Int32 data32)
        {

            for (int i = 0; i < MaxChannalCount; i++)
            {
                if (((data32 >> i) & 01) == 1)
                {
                    dataGridView1.Rows[i].Cells[4].Value = 1;
                    dataGridView1[4, i].Style.BackColor = Color.Green;
                }
                else
                {
                    dataGridView1.Rows[i].Cells[4].Value = 0;
                    dataGridView1[4, i].Style.BackColor = Color.IndianRed;
                }
            }

            tbNumCH.Text = Convert.ToString(Commutator._NumOfConnectInputs);
        }


        // Обновление на главной форме состояния питания коммутатора (checkbox)
        private void PowerComutators(Int32 data32)
        {
            for (int i = 0; i < MaxChannalCount; i++)
            {
                if (((data32 >> i) & 01) == 1)
                {
                    dataGridView1.Rows[i].Cells[5].Value = true;
                    dataGridView1[5, i].Style.BackColor = Color.Green;
                }
                else
                {
                    dataGridView1.Rows[i].Cells[5].Value = false;
                    dataGridView1[5, i].Style.BackColor = Color.IndianRed;
                }
            }
        }




        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!Commutator.Connected)
            {
                MessageBox.Show("Нет подключения к коммутатору!", "Операция прервана", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (e.ColumnIndex <= 2)//выбор датчика
            {
                UpdateSensorInfoPanel(e.RowIndex);
            }

            if (e.ColumnIndex == 4)//Состояние датчика - подключение
            {
                dataGridView1.Rows[e.RowIndex].Cells[4].Value = !Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[4].Value);

                if (Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[4].Value))
                {
                    Commutator.SetConnectors(e.RowIndex, 2); // команда подключить датчик с индексом e.RowIndex
                    dataGridView1[4, e.RowIndex].Style.BackColor = Color.Green;

                }
                else
                {
                    Commutator.SetConnectors(e.RowIndex, 3); // команда отключить датчик с индексом e.RowIndex
                    dataGridView1[4, e.RowIndex].Style.BackColor = Color.IndianRed;

                }

            }

            else if (e.ColumnIndex == 5)// Питание датчика - подключение
            {
                dataGridView1.Rows[e.RowIndex].Cells[5].Value = !Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[5].Value);


                if (Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[5].Value))
                {
                    Commutator.SetPower(e.RowIndex, 0);     // команда подключить питание датчика
                    dataGridView1[5, e.RowIndex].Style.BackColor = Color.Green;
                }
                else
                {
                    Commutator.SetPower(e.RowIndex, 1);   // команда отключить питание датчика
                    dataGridView1[5, e.RowIndex].Style.BackColor = Color.IndianRed;
                }
            }

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            dataGridView1.Visible = (tabControl1.SelectedIndex == 0);
            dataGridView2.Visible = (tabControl1.SelectedIndex == 1);
            dataGridView4.Visible = (tabControl1.SelectedIndex == 1);
            dataGridView3.Visible = (tabControl1.SelectedIndex == 2);

            // При переключении  tabControl1 необходимо получить модель выбранного датчика
            // для обновления cmbox и др элементов
            string SelectModel = "2450";   /// модель выбранного датчика
            SelectedLevel = 1;

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    {
                        return;
                    }

                // ОКНО - ХАРАКТЕРИЗАЦИЯ    
                case 1:  
                    {
                        cbCHTermoCamera1.Items.Clear();
                        cbCHTermoCamera2.Items.Clear();
                        cbCHTermoCamera3.Items.Clear();
                        cbCHTermoCamera4.Items.Clear();
                        cbCHPressureSet1.Items.Clear();
                        cbCHPressureSet2.Items.Clear();
                        cbCHPressureSet3.Items.Clear();
                        cbCHPressureSet4.Items.Clear();

                        // Занесение данных из ДБ в combobox                     
                        if (SensorsDB._сonnection.State == System.Data.ConnectionState.Open)
                        {
                            int NumOfRange = -1;
                            string SensParam;


                            // УРОВЕНЬ - 1
                            if (sensors.sensorList.Count > 0)
                            {
                                SelectModel = new String(sensors.sensorList[0].PressureModel);
                                // Определяем сколько диапазонов у датчиков                               
                                NumOfRange = Convert.ToInt16(SensorsDB.GetDataSensors(SelectModel, "NumOfRange"));  
                                                               
                                if (NumOfRange == 1)
                                {
                                    cbDiapazon1.SelectedIndex = 0;
                                    cbDiapazon1.Enabled = false;
                                    // Занесение данных из ДБ в combobox
                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarTempPoint1");  // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbCHTermoCamera1.Items.Clear();
                                        cbCHTermoCamera1.Items.AddRange(SPcmbox);
                                        cbCHTermoCamera1.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbCHPressureSet1.Items.Clear();
                                        cbCHPressureSet1.Items.AddRange(SPcmbox);
                                        cbCHPressureSet1.SelectedIndex = 0;
                                    }

                                }
                                else if (NumOfRange == 2)
                                {
                                    cbDiapazon1.Enabled = true;
                                    cbDiapazon1.SelectedIndex = 0;
                                    // Занесение данных из ДБ в combobox
                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarTempPoint1");  // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbCHTermoCamera1.Items.Clear();
                                        cbCHTermoCamera1.Items.AddRange(SPcmbox);
                                        cbCHTermoCamera1.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbCHPressureSet1.Items.Clear();
                                        cbCHPressureSet1.Items.AddRange(SPcmbox);                               
                                        cbCHPressureSet1.SelectedIndex = 0;
                                    }
                                }
                                else
                                {
                                    cbDiapazon1.SelectedIndex = -1;
                                    cbDiapazon1.Enabled = false;
                                }                                
                            }


                            // УРОВЕНЬ - 2
                            if (sensors.sensorList.Count > 8)
                            {
                                SelectModel = new String(sensors.sensorList[8].PressureModel);
                                // Определяем сколько диапазонов у датчиков                               
                                NumOfRange = Convert.ToInt16(SensorsDB.GetDataSensors(SelectModel, "NumOfRange"));  

                                if (NumOfRange == 1)
                                {
                                    cbDiapazon2.SelectedIndex = 0;
                                    cbDiapazon2.Enabled = false;
                                    // Занесение данных из ДБ в combobox
                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarTempPoint1");  // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbCHTermoCamera2.Items.Clear();
                                        cbCHTermoCamera2.Items.AddRange(SPcmbox);
                                        cbCHTermoCamera2.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbCHPressureSet2.Items.Clear();
                                        cbCHPressureSet2.Items.AddRange(SPcmbox);
                                        cbCHPressureSet2.SelectedIndex = 0;
                                    }

                                }
                                else if (NumOfRange == 2)
                                {
                                    cbDiapazon2.Enabled = true;
                                    cbDiapazon2.SelectedIndex = 0;
                                    // Занесение данных из ДБ в combobox
                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarTempPoint1");  // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbCHTermoCamera2.Items.Clear();
                                        cbCHTermoCamera2.Items.AddRange(SPcmbox);
                                        cbCHTermoCamera2.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbCHPressureSet2.Items.Clear();
                                        cbCHPressureSet2.Items.AddRange(SPcmbox);                               
                                        cbCHPressureSet2.SelectedIndex = 0;
                                    }
                                }
                                else
                                {
                                    cbDiapazon2.SelectedIndex = -1;
                                    cbDiapazon2.Enabled = false;
                                }                                

                            }


                            // УРОВЕНЬ - 3
                            if (sensors.sensorList.Count > 16)
                            {
                                SelectModel = new String(sensors.sensorList[16].PressureModel);
                                // Определяем сколько диапазонов у датчиков                               
                                NumOfRange = Convert.ToInt16(SensorsDB.GetDataSensors(SelectModel, "NumOfRange"));

                                if (NumOfRange == 1)
                                {
                                    cbDiapazon3.SelectedIndex = 0;
                                    cbDiapazon3.Enabled = false;
                                    // Занесение данных из ДБ в combobox
                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarTempPoint1");  // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbCHTermoCamera3.Items.Clear();
                                        cbCHTermoCamera3.Items.AddRange(SPcmbox);
                                        cbCHTermoCamera3.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbCHTermoCamera3.Items.Clear();
                                        cbCHPressureSet3.Items.AddRange(SPcmbox);
                                        cbCHPressureSet3.SelectedIndex = 0;
                                    }

                                }
                                else if (NumOfRange == 2)
                                {
                                    cbDiapazon3.Enabled = true;
                                    cbDiapazon3.SelectedIndex = 0;
                                    // Занесение данных из ДБ в combobox
                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarTempPoint1");  // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbCHTermoCamera3.Items.Clear();
                                        cbCHTermoCamera3.Items.AddRange(SPcmbox);
                                        cbCHTermoCamera3.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbCHPressureSet3.Items.Clear();
                                        cbCHPressureSet3.Items.AddRange(SPcmbox);
                                        cbCHPressureSet3.SelectedIndex = 0;
                                    }
                                }
                                else
                                {
                                    cbDiapazon3.SelectedIndex = -1;
                                    cbDiapazon3.Enabled = false;
                                }

                            }


                            // УРОВЕНЬ - 4
                            if (sensors.sensorList.Count > 24)
                            {
                                SelectModel = new String(sensors.sensorList[24].PressureModel);
                                // Определяем сколько диапазонов у датчиков                               
                                NumOfRange = Convert.ToInt16(SensorsDB.GetDataSensors(SelectModel, "NumOfRange"));

                                if (NumOfRange == 1)
                                {
                                    cbDiapazon4.SelectedIndex = 0;
                                    cbDiapazon4.Enabled = false;
                                    // Занесение данных из ДБ в combobox
                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarTempPoint1");  // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbCHTermoCamera4.Items.Clear();
                                        cbCHTermoCamera4.Items.AddRange(SPcmbox);
                                        cbCHTermoCamera4.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbCHPressureSet4.Items.Clear();
                                        cbCHPressureSet4.Items.AddRange(SPcmbox);
                                        cbCHPressureSet4.SelectedIndex = 0;
                                    }

                                }
                                else if (NumOfRange == 2)
                                {
                                    cbDiapazon4.Enabled = true;
                                    cbDiapazon4.SelectedIndex = 0;
                                    // Занесение данных из ДБ в combobox
                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarTempPoint1");  // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbCHTermoCamera4.Items.Clear();
                                        cbCHTermoCamera4.Items.AddRange(SPcmbox);
                                        cbCHTermoCamera4.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbCHPressureSet4.Items.Clear();
                                        cbCHPressureSet4.Items.AddRange(SPcmbox);
                                        cbCHPressureSet4.SelectedIndex = 0;
                                    }
                                }
                                else
                                {
                                    cbDiapazon4.SelectedIndex = -1;
                                    cbDiapazon4.Enabled = false;
                                }
                            }
                        }
                        return;
                    }

                // ОКНО - ВЕРИФИКАЦИЯ
                case 2:
                    {
                        // Занесение данных из ДБ в combobox                     
                        if (SensorsDB._сonnection.State == System.Data.ConnectionState.Open)
                        {
                            int NumOfRange = -1;
                            string SensParam;


                            // УРОВЕНЬ - 1
                            if (sensors.sensorList.Count > 0)
                            {
                                SelectModel = new String(sensors.sensorList[0].PressureModel);
                                // Определяем сколько диапазонов у датчиков                               
                                NumOfRange = Convert.ToInt16(SensorsDB.GetDataSensors(SelectModel, "NumOfRange"));

                                if (NumOfRange == 1)
                                {
                                    cbVRDiapazon1.SelectedIndex = 0;
                                    cbVRDiapazon1.Enabled = false;
                                    // Занесение данных из ДБ в combobox
                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerTempPoint1");  // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbVRTermoCamera1.Items.Clear();
                                        cbVRTermoCamera1.Items.AddRange(SPcmbox);
                                        cbVRTermoCamera1.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbVRPressureSet1.Items.Clear();
                                        cbVRPressureSet1.Items.AddRange(SPcmbox);
                                        cbVRPressureSet1.SelectedIndex = 0;
                                    }

                                }
                                else if (NumOfRange == 2)
                                {
                                    cbVRDiapazon1.Enabled = true;
                                    cbVRDiapazon1.SelectedIndex = 0;
                                    // Занесение данных из ДБ в combobox
                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerTempPoint1");  // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbVRTermoCamera1.Items.Clear();
                                        cbVRTermoCamera1.Items.AddRange(SPcmbox);
                                        cbVRTermoCamera1.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbVRPressureSet1.Items.Clear();
                                        cbVRPressureSet1.Items.AddRange(SPcmbox);
                                        cbVRPressureSet1.SelectedIndex = 0;
                                    }
                                }
                                else
                                {
                                    cbVRDiapazon1.SelectedIndex = -1;
                                    cbVRDiapazon1.Enabled = false;
                                }
                            }


                            // УРОВЕНЬ - 2
                            if (sensors.sensorList.Count > 8)
                            {
                                SelectModel = new String(sensors.sensorList[8].PressureModel);
                                // Определяем сколько диапазонов у датчиков                               
                                NumOfRange = Convert.ToInt16(SensorsDB.GetDataSensors(SelectModel, "NumOfRange"));

                                if (NumOfRange == 1)
                                {
                                    cbVRDiapazon2.SelectedIndex = 0;
                                    cbVRDiapazon2.Enabled = false;
                                    // Занесение данных из ДБ в combobox
                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerTempPoint1");  // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbVRTermoCamera2.Items.Clear();
                                        cbVRTermoCamera2.Items.AddRange(SPcmbox);
                                        cbVRTermoCamera2.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbVRPressureSet2.Items.Clear();
                                        cbVRPressureSet2.Items.AddRange(SPcmbox);
                                        cbVRPressureSet2.SelectedIndex = 0;
                                    }

                                }
                                else if (NumOfRange == 2)
                                {
                                    cbVRDiapazon2.Enabled = true;
                                    cbVRDiapazon2.SelectedIndex = 0;
                                    // Занесение данных из ДБ в combobox
                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerTempPoint1");  // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbVRTermoCamera2.Items.Clear();
                                        cbVRTermoCamera2.Items.AddRange(SPcmbox);
                                        cbVRTermoCamera2.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbVRPressureSet2.Items.Clear();
                                        cbVRPressureSet2.Items.AddRange(SPcmbox);
                                        cbVRPressureSet2.SelectedIndex = 0;
                                    }
                                }
                                else
                                {
                                    cbVRDiapazon2.SelectedIndex = -1;
                                    cbVRDiapazon2.Enabled = false;
                                }

                            }


                            // УРОВЕНЬ - 3
                            if (sensors.sensorList.Count > 16)
                            {
                                SelectModel = new String(sensors.sensorList[16].PressureModel);
                                // Определяем сколько диапазонов у датчиков                               
                                NumOfRange = Convert.ToInt16(SensorsDB.GetDataSensors(SelectModel, "NumOfRange"));

                                if (NumOfRange == 1)
                                {
                                    cbVRDiapazon3.SelectedIndex = 0;
                                    cbVRDiapazon3.Enabled = false;
                                    // Занесение данных из ДБ в combobox
                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerTempPoint1");  // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbVRTermoCamera3.Items.Clear();
                                        cbVRTermoCamera3.Items.AddRange(SPcmbox);
                                        cbVRTermoCamera3.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbVRPressureSet3.Items.Clear();
                                        cbVRPressureSet3.Items.AddRange(SPcmbox);
                                        cbVRPressureSet3.SelectedIndex = 0;
                                    }

                                }
                                else if (NumOfRange == 2)
                                {
                                    cbVRDiapazon3.Enabled = true;
                                    cbVRDiapazon3.SelectedIndex = 0;
                                    // Занесение данных из ДБ в combobox
                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerTempPoint1");  // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbVRTermoCamera3.Items.Clear();
                                        cbVRTermoCamera3.Items.AddRange(SPcmbox);
                                        cbVRTermoCamera3.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbVRPressureSet3.Items.Clear();
                                        cbVRPressureSet3.Items.AddRange(SPcmbox);
                                        cbVRPressureSet3.SelectedIndex = 0;
                                    }
                                }
                                else
                                {
                                    cbVRDiapazon3.SelectedIndex = -1;
                                    cbVRDiapazon3.Enabled = false;
                                }

                            }


                            // УРОВЕНЬ - 4
                            if (sensors.sensorList.Count > 24)
                            {
                                SelectModel = new String(sensors.sensorList[24].PressureModel);
                                // Определяем сколько диапазонов у датчиков                               
                                NumOfRange = Convert.ToInt16(SensorsDB.GetDataSensors(SelectModel, "NumOfRange"));

                                if (NumOfRange == 1)
                                {
                                    cbVRDiapazon4.SelectedIndex = 0;
                                    cbVRDiapazon4.Enabled = false;
                                    // Занесение данных из ДБ в combobox
                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerTempPoint1");  // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbVRTermoCamera4.Items.Clear();
                                        cbVRTermoCamera4.Items.AddRange(SPcmbox);
                                        cbVRTermoCamera4.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbVRPressureSet4.Items.Clear();
                                        cbVRPressureSet4.Items.AddRange(SPcmbox);
                                        cbVRPressureSet4.SelectedIndex = 0;
                                    }

                                }
                                else if (NumOfRange == 2)
                                {
                                    cbVRDiapazon4.Enabled = true;
                                    cbVRDiapazon4.SelectedIndex = 0;
                                    // Занесение данных из ДБ в combobox
                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerTempPoint1");  // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbVRTermoCamera4.Items.Clear();
                                        cbVRTermoCamera4.Items.AddRange(SPcmbox);
                                        cbVRTermoCamera4.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != null)
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbVRPressureSet4.Items.Clear();
                                        cbVRPressureSet4.Items.AddRange(SPcmbox);
                                        cbVRPressureSet4.SelectedIndex = 0;
                                    }
                                }
                                else
                                {
                                    cbVRDiapazon4.SelectedIndex = -1;
                                    cbVRDiapazon4.Enabled = false;
                                }

                            }

                        }

                        return;


                    }

            }       
        }

        //Создаем файлы текущей сессии по результатам характеризации, чтения ЦАП и верификации.
        private void CreatSessionFiles()
        {
            //закрываем предыдущие результаты сессии если были открыты
            if (ResultCH != null)
                ResultCH.CloseAll();
            if (ResultCI != null)
                ResultCI.CloseAll();
            if (ResultVR != null)
                ResultVR.CloseAll();

            //Подготавливаем заводские номера по каналам
            int[] FN = new int[MaxChannalCount];
            for (int i = 0; i < MaxChannalCount; i++)
            {
                if (sensors.SelectSensor(i))
                {
                    FN[i] = (int)sensors.sensor.uni;
                }
                else
                {
                    FN[i] = 0;
                }
            }

            //***************** создаем файлы результатов характеризации ***********************************
            ResultCH = new СResultCH(MaxChannalCount, FN);//результаты характеризации датчиков

            //***************** создаем файлы результатов калибровки ***************************************
            ResultCI = new CResultCI(MaxChannalCount, FN);//результаты калибровки датчиков

            //***************** создаем файлы результатов верификации ***********************************
            ResultVR = new CResultVR(MaxChannalCount, FN);

            //**********************************************************************************************

        }

        // Отработка нажатия на гл. форме МЕНСОР - ЗАДАЧА
        private void bMensorControl_Click(object sender, EventArgs e)
        {
            if (Mensor._serialPort_M.IsOpen)
            {
                Mensor.SetMode(1);
            }
            else
            {
                Program.txtlog.WriteLineLog("Нет Связи. Задатчик давления не подключен", 1);
            }
        }


        // Отработка нажатия на гл. форме МЕНСОР - СБРОС
        private void button7_Click(object sender, EventArgs e)
        {
            if (Mensor._serialPort_M.IsOpen)
            {
                Mensor.SetMode(2);
            }
            else
            {
                Program.txtlog.WriteLineLog("Нет Связи. Задатчик давления не подключен", 1);
            }

        }


        // Отработка выбора на гл.форме ТИПА ПРЕОБРАЗОВАТЕЛЯ МЕНСОРА из списка
        private void cbMensorTypeR_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!Mensor._serialPort_M.IsOpen)
            {
                if (cbMensorTypeR.SelectedIndex != -1)
                {
                    Program.txtlog.WriteLineLog("Нет Связи. Задатчик давления не подключен", 1);
                    cbMensorTypeR.SelectedIndex = -1;
                }
                return;
            }

            try
            {
                MainTimer.Stop();
                MainTimer.Enabled = false;

                // Получаем индекс выбранного преобразователя
                int ind = cbMensorTypeR.SelectedIndex;

                // Определяем тип канала соответствующего выбранному преобразователю
                if ((ind >= 0) && (ind <= 2))  // активный канал А
                {

                    Mensor.ChannelSet("A");   // устанвливаем активным канал A
                    Thread.Sleep(100);
                    Mensor.SetTypeRange(ind);     // Устанавливаем тип выбранного преобразователя
                }
                else if ((ind >= 0) && (ind <= 2)) // активный канал B
                {
                    Mensor.ChannelSet("B");   // устанвливаем активным канал B
                    Thread.Sleep(100);
                    Mensor.SetTypeRange(ind - 3);     // Устанавливаем тип выбранного преобразователя
                }

            }

            finally
            {
                MainTimer.Enabled = true;
                MainTimer.Start();
            }


        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!Mensor._serialPort_M.IsOpen)
            {
                Program.txtlog.WriteLineLog("Нет Связи. Задатчик давления не подключен", 1);
                return;
            }

            double Point = (double)numMensorPoint.Value;  // получаем заданное значение уставки
            Mensor.SetPoint(Point);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (TemperatureReady && PressureReady)
            {
                btnCHStart.BackColor = Color.IndianRed;
                btnCHStart.Text = "Выполняется процесс характеризации ...";
                ReadSensorParametrs();
                btnCalculateCoeff.BackColor = Color.LightGreen;
                btnCHStart.Text = "Старт характеризации";
            }
            else
            {
                Program.txtlog.WriteLineLog("Не заданны параметры для характеризации.", 1);
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex == 3)&&(dataGridView1.RowCount>0))//выбор датчиков
            {
                bool CurentSet = Convert.ToBoolean(dataGridView1.Rows[0].Cells[3].Value);
                for (int i = 0; i <= (dataGridView1.RowCount - 1); i++)
                {
                    dataGridView1.Rows[i].Cells[3].Value = !CurentSet;
//                    dataGridView1.Rows[i].Cells[3].Style.BackColor = Color.White;
                }
            }
        }

        private void cbChannalCharakterizator_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpDateCharakterizatorGrid(cbChannalCharakterizator.SelectedIndex);
            UpdateCurrentGrid(cbChannalCharakterizator.SelectedIndex);
        }

        private void gbCHLevel1_Enter(object sender, EventArgs e)
        {
            switch ((sender as GroupBox).Tag)
            {
                case "1":
                    SelectedLevel = 1;
                    gbCHLevel1.BackColor = Color.LightGreen;
                    gbCHLevel2.BackColor = Color.Transparent;
                    gbCHLevel3.BackColor = Color.Transparent;
                    gbCHLevel4.BackColor = Color.Transparent;
                    break;
                case "2":
                    SelectedLevel = 2;
                    gbCHLevel1.BackColor = Color.Transparent;
                    gbCHLevel2.BackColor = Color.LightGreen;
                    gbCHLevel3.BackColor = Color.Transparent;
                    gbCHLevel4.BackColor = Color.Transparent;
                    break;
                case "3":
                    SelectedLevel = 3;
                    gbCHLevel1.BackColor = Color.Transparent;
                    gbCHLevel2.BackColor = Color.Transparent;
                    gbCHLevel3.BackColor = Color.LightGreen;
                    gbCHLevel4.BackColor = Color.Transparent;
                    break;
                case "4":
                    SelectedLevel = 4;
                    gbCHLevel1.BackColor = Color.Transparent;
                    gbCHLevel2.BackColor = Color.Transparent;
                    gbCHLevel3.BackColor = Color.Transparent;
                    gbCHLevel4.BackColor = Color.LightGreen;
                    break;
            }
        }

        private void btnNextStep1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void btnNextStep2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void btnNextStep3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }


        // Обработчик нажатия меню: ФАЙЛ - ОТКРЫТЬ БД
        private void открытьБДДатчиковToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // устанавливаем связь с БД
//            string strFileNameDB = Charaterizator.Properties.Settings.Default.FileNameDB;   // получаем путь и имя файла из Settings
//            SensorsDB.SetConnectionDB(strFileNameDB);                                  // устанавливаем соединение с БД           
            SensorsDB.GetData();        // получаем список моделей из БД и записываем его в listbox 
            SensorsDB.eni100 = sensors;
            SensorsDB.ShowDialog();
                       
        }
        
        //Установка температуры для характеризации группы 
        private void btnCHTemperatureSet1_Click(object sender, EventArgs e)
        {
            TemperatureReady = false;
            string strValue="";
            switch ((sender as Button).Tag)
            {
                case "1":
                    strValue = cbCHTermoCamera1.Text;
                    break;
                case "2":
                    strValue = cbCHTermoCamera2.Text;
                    break;
                case "3":
                    strValue = cbCHTermoCamera3.Text;
                    break;
                case "4":
                    strValue = cbCHTermoCamera4.Text;
                    break;
            }

            if (ThermalCamera.Connected)
            {
                numTermoCameraPoint.Text = strValue;
                Program.txtlog.WriteLineLog("Температура задана. Ожидаем завершение стабилизации показаний.", 0);
                TemperatureReady = true;
                btnReadCAP.BackColor = Color.LightGreen;
                MessageBox.Show("температура установлена.", "Успешное завершение операции");
            }
            else
            {
                Program.txtlog.WriteLineLog("CH: Нет cвязи c термокамерой.", 1);
                if (MessageBox.Show("Хотите установить температуру в ручную?", "Нет соединения с термокамерой", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    numTermoCameraPoint.Text = strValue;
                    TemperatureReady = true;
                    btnReadCAP.BackColor = Color.LightGreen;
                }
            }
        }
        

        //Установка давления для характеризации группы 
        private void btnCHPressureSet1_Click(object sender, EventArgs e)
        {
            string strValue="";
            switch ((sender as Button).Tag)
            {
                case "1":
                    strValue = cbCHPressureSet1.Text;
                    break;
                case "2":
                    strValue = cbCHPressureSet2.Text;
                    break;
                case "3":
                    strValue = cbCHPressureSet3.Text;
                    break;
                case "4":
                    strValue = cbCHPressureSet4.Text;
                    break;
            }

            if (Mensor.Connected)
            {
                double Point;
                double shift;
                try
                {
                    btnCHPressureSet1.Enabled = false;
                    btnCHPressureSet2.Enabled = false;
                    btnCHPressureSet3.Enabled = false;
                    btnCHPressureSet4.Enabled = false;
                    Point = Convert.ToDouble(strValue);// получаем заданное значение уставки
                    numMensorPoint.Text = strValue;
                    bMensorSet.PerformClick();      //выставляем давление
                    bMensorControl.PerformClick();  //запускаем задачу

                    int i = 0;
                    do//ожидаем установления давления
                    {
                        i++;
                        Thread.Sleep(1000);
                        double realpoint = Convert.ToDouble(tbMensorData.Text);
                        shift = Math.Abs(realpoint - Point);
                    } while ((shift < SKOPressure) && (i < 100));
                    if (i >= 100)
                    {//давление не установлено
                        MessageBox.Show("Повторите установку давления.", "Истекло время установки давления в датчиках");
                    }
                    else
                    {//давление установлено
                        Thread.Sleep(WaitPressureTime * 1000);//ожидаем стабилизации
                        PressureReady = true;
                        btnCHStart.BackColor = Color.LightGreen;
                        btnCalculateCoeff.BackColor = Color.IndianRed;
                        MessageBox.Show("Давление установлено.", "Успешное завершение операции");
                    }
                }
                finally
                {
                    btnCHPressureSet1.Enabled = true;
                    btnCHPressureSet2.Enabled = true;
                    btnCHPressureSet3.Enabled = true;
                    btnCHPressureSet4.Enabled = true;
                }
            }
            else
            {
                Program.txtlog.WriteLineLog("CH: Нет cвязи c задатчиком давления.", 1);
                if (MessageBox.Show("Хотите установить давление в ручную?", "Нет соединения с Менсором", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    numMensorPoint.Text = strValue;
                    PressureReady = true;
                    btnCHStart.BackColor = Color.LightGreen;
                    btnCalculateCoeff.BackColor = Color.IndianRed;
                }
            }
        }


        private void bThermalCameraSet_Click(object sender, EventArgs e)
        {
            if (ThermalCamera.Connected)
            {
                double Point = (double)numTermoCameraPoint.Value;  // получаем заданное значение уставки
                ThermalCamera.WriteData(Point);
                Program.txtlog.WriteLineLog(string.Format("Задача выполнена. В термокамере установлена температура {0} градусов.", Point), 1);
            }
            else
            {
                Program.txtlog.WriteLineLog("Нет Связи. Термокамера не подключена", 1);
            }
        }

        //чтение параметров ЦАП
        private void btnReadCAP_Click(object sender, EventArgs e)
        {
            if (TemperatureReady && PressureReady)
            {
                btnReadCAP.BackColor = Color.IndianRed;
                btnReadCAP.Text = "Выполняется процесс чтения ЦАП...";
                ReadSensorCurrent();
                btnReadCAP.Text = "Чтение параметров ЦАП";
            }
            else
            {
                Program.txtlog.WriteLineLog("Не заданны параметры для чтения ЦАП.", 1);
            }
        }

        private void btnCalibrateCurrent_Click(object sender, EventArgs e)
        {
            if (!Multimetr.Connected)
            {
                Program.txtlog.WriteLineLog("Нет подключения к мультиметру, калибровка не выполнена!", 0);
                return;
            }
            if (MessageBox.Show(string.Format("Температура в камере: {0}. Продолжить калибровку?", tbTemperature.Text), "Подтверждение операции",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Program.txtlog.WriteLineLog("Старт калибровки тока датчиков. Температура: " + tbTemperature.Text, 0);
                for (int i = 0; i < MaxChannalCount; i++)
                {
                    if (sensors.SelectSensor(i))
                    {
                        sensors.sensor.CurrentExit = 0;
                        sensors.C129WriteCurrenExit();
                        Thread.Sleep(1000);//ожидаем измерения мультиметром
                        sensors.С45WriteCurrent4mA(Multimetr.Current);

                        Thread.Sleep(100);

                        sensors.sensor.CurrentExit = 1;
                        sensors.C129WriteCurrenExit();
                        Thread.Sleep(1000);//ожидаем измерения мультиметром
                        sensors.С46WriteCurrent20mA(Multimetr.Current);
                        Program.txtlog.WriteLineLog(string.Format("Калибровка датчика в канале {0} выполнена", i + 1), 0);
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog(string.Format("Калибровка: Датчик не обнаружен в канале {0}", i+1), 1);
                    }
                }

            }
        }

        private void cbChannalVerification_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpDateVerificationGrid(cbChannalVerification.SelectedIndex);
        }

        // Меню НАСТРОЙКИ
        // Настройки программы
        private void MenuItemMainSettings_Click(object sender, EventArgs e)
        {
            SettingsSelIndex = 0;
            FormSettings Settings = new FormSettings();            
            Settings.ShowDialog();

        }
        // Настройки Мультиметра
        private void параметрыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsSelIndex = 2;
            FormSettings Settings = new FormSettings();            
            Settings.ShowDialog();
        }
        // Настройки Задатчика давления
        private void параметрыToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SettingsSelIndex = 3;
            FormSettings Settings = new FormSettings();
            Settings.ShowDialog();
        }
        // Настройки Коммутатора
        private void параметрыToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SettingsSelIndex = 1;
            FormSettings Settings = new FormSettings();
            Settings.ShowDialog();
        }
        // Настройки Термокамеры
        private void параметрыToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            SettingsSelIndex = 4;
            FormSettings Settings = new FormSettings();
            Settings.ShowDialog();
        }
        // Настройки Датчиков
        private void параметрыToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            SettingsSelIndex = 5;
            FormSettings Settings = new FormSettings();
            Settings.ShowDialog();
        }




        // ХАРАКТЕРИЗАЦИЯ
        // УРОВЕНЬ-1
        // Выбор диапазона в combobox
        // и отбражение данный по точкам температуры и давлению из ДБ
        private void cbDiapazon1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SensorsDB._сonnection.State == System.Data.ConnectionState.Open)
            {                
                string SensParam;

                if (sensors.sensorList.Count > 0)
                {
                    string SelectModel = new String(sensors.sensorList[0].PressureModel);                 

                    if (cbDiapazon1.SelectedIndex == 0)
                    {
                        SensParam = SensorsDB.GetDataSensors(SelectModel, "HarPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                        if (SensParam != null)
                        {
                            string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            cbCHPressureSet1.Items.Clear();
                            cbCHPressureSet1.Items.AddRange(SPcmbox);
                            cbCHPressureSet1.SelectedIndex = 0;
                        }
                    }
                    else if (cbDiapazon1.SelectedIndex == 1)
                    {
                        SensParam = SensorsDB.GetDataSensors(SelectModel, "HarPressPoint2"); // функция запроса данных из БД по номеру модели и параметру
                        if (SensParam != null)
                        {
                            string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            cbCHPressureSet1.Items.Clear();
                            cbCHPressureSet1.Items.AddRange(SPcmbox);
                            cbCHPressureSet1.SelectedIndex = 0;
                        }

                    }
                    else
                    {                        
                    }
                }

            }

        }

        // УРОВЕНЬ-2
        // Выбор диапазона в combobox
        // и отбражение данный по точкам температуры и давлению из БД
        private void cbDiapazon2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SensorsDB._сonnection.State == System.Data.ConnectionState.Open)
            {
                string SensParam;

                if (sensors.sensorList.Count > 8)
                {
                    string SelectModel = new String(sensors.sensorList[8].PressureModel);

                    if (cbDiapazon2.SelectedIndex == 0)
                    {
                        SensParam = SensorsDB.GetDataSensors(SelectModel, "HarPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                        if (SensParam != null)
                        {
                            string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            cbCHPressureSet2.Items.Clear();
                            cbCHPressureSet2.Items.AddRange(SPcmbox);
                            cbCHPressureSet2.SelectedIndex = 0;
                        }
                    }
                    else if (cbDiapazon2.SelectedIndex == 1)
                    {
                        SensParam = SensorsDB.GetDataSensors(SelectModel, "HarPressPoint2"); // функция запроса данных из БД по номеру модели и параметру
                        if (SensParam != null)
                        {
                            string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            cbCHPressureSet2.Items.Clear();
                            cbCHPressureSet2.Items.AddRange(SPcmbox);
                            cbCHPressureSet2.SelectedIndex = 0;
                        }

                    }
                    else
                    {
                    }
                }

            }
        }


        // УРОВЕНЬ-3
        // Выбор диапазона в combobox
        // и отбражение данный по точкам температуры и давлению из БД
        private void cbDiapazon3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SensorsDB._сonnection.State == System.Data.ConnectionState.Open)
            {
                string SensParam;

                if (sensors.sensorList.Count > 16)
                {
                    string SelectModel = new String(sensors.sensorList[16].PressureModel);

                    if (cbDiapazon3.SelectedIndex == 0)
                    {
                        SensParam = SensorsDB.GetDataSensors(SelectModel, "HarPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                        if (SensParam != null)
                        {
                            string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            cbCHPressureSet3.Items.Clear();
                            cbCHPressureSet3.Items.AddRange(SPcmbox);
                            cbCHPressureSet3.SelectedIndex = 0;
                        }
                    }
                    else if (cbDiapazon3.SelectedIndex == 1)
                    {
                        SensParam = SensorsDB.GetDataSensors(SelectModel, "HarPressPoint2"); // функция запроса данных из БД по номеру модели и параметру
                        if (SensParam != null)
                        {
                            string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            cbCHPressureSet3.Items.Clear();
                            cbCHPressureSet3.Items.AddRange(SPcmbox);
                            cbCHPressureSet3.SelectedIndex = 0;
                        }

                    }
                    else
                    {
                    }
                }

            }
        }


        // УРОВЕНЬ-4
        // Выбор диапазона в combobox
        // и отбражение данный по точкам температуры и давлению из БД
        private void cbDiapazon4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SensorsDB._сonnection.State == System.Data.ConnectionState.Open)
            {
                string SensParam;

                if (sensors.sensorList.Count > 24)
                {
                    string SelectModel = new String(sensors.sensorList[24].PressureModel);

                    if (cbDiapazon4.SelectedIndex == 0)
                    {
                        SensParam = SensorsDB.GetDataSensors(SelectModel, "HarPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                        if (SensParam != null)
                        {
                            string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            cbCHPressureSet4.Items.Clear();
                            cbCHPressureSet4.Items.AddRange(SPcmbox);
                            cbCHPressureSet4.SelectedIndex = 0;
                        }
                    }
                    else if (cbDiapazon4.SelectedIndex == 1)
                    {
                        SensParam = SensorsDB.GetDataSensors(SelectModel, "HarPressPoint2"); // функция запроса данных из БД по номеру модели и параметру
                        if (SensParam != null)
                        {
                            string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            cbCHPressureSet4.Items.Clear();
                            cbCHPressureSet4.Items.AddRange(SPcmbox);
                            cbCHPressureSet4.SelectedIndex = 0;
                        }

                    }
                    else
                    {
                    }
                }

            }
        }

        private void btnVRTemperatureSet1_Click(object sender, EventArgs e)
        {
            TemperatureReady = false;
            string strValue = "";
            switch ((sender as Button).Tag)
            {
                case "1":
                    strValue = cbVRTermoCamera1.Text;
                    break;
                case "2":
                    strValue = cbVRTermoCamera2.Text;
                    break;
                case "3":
                    strValue = cbVRTermoCamera3.Text;
                    break;
                case "4":
                    strValue = cbVRTermoCamera4.Text;
                    break;
            }

            if (ThermalCamera.Connected)
            {
                numTermoCameraPoint.Text = strValue;
                Program.txtlog.WriteLineLog("Температура задана. Ожидаем завершение стабилизации показаний.", 0);
                TemperatureReady = true;
            }
            else
            {
                Program.txtlog.WriteLineLog("Нет cвязи c термокамерой.", 1);
                if (MessageBox.Show("Хотите установить температуру в ручную?", "Нет соединения с Термокамерой", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    numTermoCameraPoint.Text = strValue;
                    TemperatureReady = true;
                }
            }

        }

        private void btnVRPressureSet1_Click(object sender, EventArgs e)
        {
            string strValue = "";
            switch ((sender as Button).Tag)
            {
                case "1":
                    strValue = cbVRPressureSet1.Text;
                    break;
                case "2":
                    strValue = cbVRPressureSet2.Text;
                    break;
                case "3":
                    strValue = cbVRPressureSet3.Text;
                    break;
                case "4":
                    strValue = cbVRPressureSet4.Text;
                    break;
            }

            if (Mensor.Connected)
            {
                double Point;
                double shift;
                try
                {
                    btnVRPressureSet1.Enabled = false;
                    btnVRPressureSet2.Enabled = false;
                    btnVRPressureSet3.Enabled = false;
                    btnVRPressureSet4.Enabled = false;
                    Point = Convert.ToDouble(strValue);// получаем заданное значение уставки
                    numMensorPoint.Text = strValue;
                    bMensorSet.PerformClick();      //выставляем давление
                    bMensorControl.PerformClick();  //запускаем задачу
                    int i = 0;
                    do//ожидаем установления давления
                    {
                        i++;
                        Application.DoEvents();
                        Thread.Sleep(1000);
                        double realpoint = Convert.ToDouble(tbMensorData.Text);
                        shift = Math.Abs(realpoint - Point);
                    } while ((shift < SKOPressure) && (i < MENSOR_PRESSUER_WAIT));
                    if (i >= MENSOR_PRESSUER_WAIT)
                    {//давление не установлено
                        MessageBox.Show("Повторите установку давления.", "Истекло время установки давления в датчиках");
                    }
                    else
                    {//давление установлено
                        Thread.Sleep(WaitPressureTime * 1000);//ожидаем стабилизации
                        PressureReady = true;
//                        btnCHStart.BackColor = Color.LightGreen;
//                        btnReadCAP.BackColor = Color.LightGreen;
                        MessageBox.Show("Давление установлено.", "Успешное завершение операции");
                    }
                }
                finally
                {
                    btnVRPressureSet1.Enabled = true;
                    btnVRPressureSet2.Enabled = true;
                    btnVRPressureSet3.Enabled = true;
                    btnVRPressureSet4.Enabled = true;
                }
            }
            else
            {
                if (MessageBox.Show("Хотите установить давление в ручную?", "Нет соединения с Менсором", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    numMensorPoint.Text = strValue;
                    PressureReady = true;
                    btnVRParamRead.BackColor = Color.LightGreen;
                }
            }

        }

        private void btnVRParamRead_Click(object sender, EventArgs e)
        {
            if (TemperatureReady && PressureReady)
            {
                btnVRParamRead.BackColor = Color.IndianRed;
                btnVRParamRead.Text = "Выполняется процесс верификации ...";
                ReadSensorPressure();
                btnVRParamRead.Text = "Чтение показаний";
            }
            else
            {
                Program.txtlog.WriteLineLog("Не установлены параметры для верификации.", 1);
            }
        }

        private void gbVRLevel1_Enter(object sender, EventArgs e)
        {
            switch ((sender as GroupBox).Tag)
            {
                case "1":
                    SelectedLevel = 1;
                    gbVRLevel1.BackColor = Color.LightGreen;
                    gbVRLevel2.BackColor = Color.Transparent;
                    gbVRLevel3.BackColor = Color.Transparent;
                    gbVRLevel4.BackColor = Color.Transparent;
                    break;
                case "2":
                    SelectedLevel = 2;
                    gbVRLevel1.BackColor = Color.Transparent;
                    gbVRLevel2.BackColor = Color.LightGreen;
                    gbVRLevel3.BackColor = Color.Transparent;
                    gbVRLevel4.BackColor = Color.Transparent;
                    break;
                case "3":
                    SelectedLevel = 3;
                    gbVRLevel1.BackColor = Color.Transparent;
                    gbVRLevel2.BackColor = Color.Transparent;
                    gbVRLevel3.BackColor = Color.LightGreen;
                    gbVRLevel4.BackColor = Color.Transparent;
                    break;
                case "4":
                    SelectedLevel = 4;
                    gbVRLevel1.BackColor = Color.Transparent;
                    gbVRLevel2.BackColor = Color.Transparent;
                    gbVRLevel3.BackColor = Color.Transparent;
                    gbVRLevel4.BackColor = Color.LightGreen;
                    break;
            }

        }

        private void cbVRDiapazon1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SensorsDB._сonnection.State == System.Data.ConnectionState.Open)
            {
                string SensParam;

                if (sensors.sensorList.Count > 0)
                {
                    string SelectModel = new String(sensors.sensorList[0].PressureModel);

                    if (cbVRDiapazon1.SelectedIndex == 0)
                    {
                        SensParam = SensorsDB.GetDataSensors(SelectModel, "VerPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                        if (SensParam != null)
                        {
                            string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            cbVRPressureSet1.Items.Clear();
                            cbVRPressureSet1.Items.AddRange(SPcmbox);
                            cbVRPressureSet1.SelectedIndex = 0;
                        }
                    }
                    else if (cbVRDiapazon1.SelectedIndex == 1)
                    {
                        SensParam = SensorsDB.GetDataSensors(SelectModel, "VerPressPoint2"); // функция запроса данных из БД по номеру модели и параметру
                        if (SensParam != null)
                        {
                            string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            cbVRPressureSet1.Items.Clear();
                            cbVRPressureSet1.Items.AddRange(SPcmbox);
                            cbVRPressureSet1.SelectedIndex = 0;
                        }

                    }
                    else
                    {
                    }
                }

            }
        }

        private void cbVRDiapazon2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SensorsDB._сonnection.State == System.Data.ConnectionState.Open)
            {
                string SensParam;

                if (sensors.sensorList.Count > 0)
                {
                    string SelectModel = new String(sensors.sensorList[0].PressureModel);

                    if (cbVRDiapazon2.SelectedIndex == 0)
                    {
                        SensParam = SensorsDB.GetDataSensors(SelectModel, "VerPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                        if (SensParam != null)
                        {
                            string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            cbVRPressureSet2.Items.Clear();
                            cbVRPressureSet2.Items.AddRange(SPcmbox);
                            cbVRPressureSet2.SelectedIndex = 0;
                        }
                    }
                    else if (cbVRDiapazon2.SelectedIndex == 1)
                    {
                        SensParam = SensorsDB.GetDataSensors(SelectModel, "VerPressPoint2"); // функция запроса данных из БД по номеру модели и параметру
                        if (SensParam != null)
                        {
                            string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            cbVRPressureSet2.Items.Clear();
                            cbVRPressureSet2.Items.AddRange(SPcmbox);
                            cbVRPressureSet2.SelectedIndex = 0;
                        }

                    }
                    else
                    {
                    }
                }

            }
        }

        private void cbVRDiapazon3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SensorsDB._сonnection.State == System.Data.ConnectionState.Open)
            {
                string SensParam;

                if (sensors.sensorList.Count > 0)
                {
                    string SelectModel = new String(sensors.sensorList[0].PressureModel);

                    if (cbVRDiapazon3.SelectedIndex == 0)
                    {
                        SensParam = SensorsDB.GetDataSensors(SelectModel, "VerPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                        if (SensParam != null)
                        {
                            string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            cbVRPressureSet3.Items.Clear();
                            cbVRPressureSet3.Items.AddRange(SPcmbox);
                            cbVRPressureSet3.SelectedIndex = 0;
                        }
                    }
                    else if (cbVRDiapazon3.SelectedIndex == 1)
                    {
                        SensParam = SensorsDB.GetDataSensors(SelectModel, "VerPressPoint2"); // функция запроса данных из БД по номеру модели и параметру
                        if (SensParam != null)
                        {
                            string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            cbVRPressureSet3.Items.Clear();
                            cbVRPressureSet3.Items.AddRange(SPcmbox);
                            cbVRPressureSet3.SelectedIndex = 0;
                        }

                    }
                    else
                    {
                    }
                }

            }
        }


        private void cbVRDiapazon4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SensorsDB._сonnection.State == System.Data.ConnectionState.Open)
            {
                string SensParam;

                if (sensors.sensorList.Count > 0)
                {
                    string SelectModel = new String(sensors.sensorList[0].PressureModel);

                    if (cbVRDiapazon4.SelectedIndex == 0)
                    {
                        SensParam = SensorsDB.GetDataSensors(SelectModel, "VerPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                        if (SensParam != null)
                        {
                            string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            cbVRPressureSet4.Items.Clear();
                            cbVRPressureSet4.Items.AddRange(SPcmbox);
                            cbVRPressureSet4.SelectedIndex = 0;
                        }
                    }
                    else if (cbVRDiapazon4.SelectedIndex == 1)
                    {
                        SensParam = SensorsDB.GetDataSensors(SelectModel, "VerPressPoint2"); // функция запроса данных из БД по номеру модели и параметру
                        if (SensParam != null)
                        {
                            string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            cbVRPressureSet4.Items.Clear();
                            cbVRPressureSet4.Items.AddRange(SPcmbox);
                            cbVRPressureSet4.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                    }
                }

            }
        }
    }
}

