using System;
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
using System.Globalization;
using System.Runtime.InteropServices;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra;





namespace Charaterizator
{
    public partial class MainForm : Form
    {
        [DllImport("PCalcCoefAGAT.dll")]
//        public extern static int CalcCoefP(int , double*, double*, double*, double*, double &, double &, double &, double &);
        public extern static int CalcCoefP(
			int _countRec, //Количество переданных точек для расчета
			double [] _ut,   //Значения по каналу температуры
			double [] _up,   //Значения по каналу давления
			double []_upInParts, //Значения по каналу давления в долях от текущего ВПИ
			double [] _resP,      //Расчитанные коэффициенты (выходные данные)
			double [] _upmin,     //Минимальная величина по каналу давления
			double [] _upmn,      //Диапазон изменения сигнала по каналу давления
			double [] _utmin,     //Минимальная величина по каналу температуры
            double [] _utmn     //Диапазон изменения сигнала по каналу температуры
		);

        // Занесены в настройку
        public int MAIN_TIMER = 1000;        
        private int MAX_ERROR_COUNT = 3; //Количество ошибок чтения данных с устройств перед отключением
        private double MIN_SENSOR_CURRENT = 1.5;//минимльный ток датчика для обнаружения, мА
        private int MAX_COUNT_CAP_READ = 3;//максимальное количество циклов чтения тока ЦАП
        private double SKO_CURRENT = 0.5;//допуск по току ЦАП датчика до калибровки, мА
        private double SKO_CALIBRATION_CURRENT = 0.003;//допуск по току ЦАП после калибровки, мА

        private static int MaxChannalCount = 32;//максимальное количество каналов коммутаторы
        private static int MaxLevelCount = 4;//максимальное количество уровней датчиков (идентичных групп)
       
        private int MAX_COUNT_POINT = 5;//ожидание стабилизации давления в датчике, в циклах таймера
        private double SKO_PRESSURE = 0.2;  //(СКО) допуск по давлению, кПа



        //Не занесены в настройку
        const int MAX_CALIBRATION_COUNT = 3;//максимальное количество циклов калибровки тока ЦАП        
        private int MENSOR_PRESSUER_WAIT = 60;//время установления давления в менсоре, сек
        private int SENSOR_PRESSUER_WAIT = 5;//ожидание стабилизации давления в датчике, сек        
        const double DEFAULT_TEMPERATURE = 23.0;//стандартная температура для чтения ЦАП

        // Номера столбцов в dataGrid1 - начальное окно с выбором датчиков
        byte ch = 0;        // номер канала
        byte sel = 1;       // выбор
        byte sen = 2;       // датчик
        byte zn = 3;        // заводской номер        
        byte pow = 4;       // питание
        byte ok = 5;        // исправность

        private int MensorCountPoint = 0;// счетчик для уставки (выдержки) давления в датчиках - для установки зелены цветом 
        
        public static int SettingsSelIndex { set; get; }


        private readonly Font DrawingFont = new Font(new FontFamily("DS-Digital"), 28.0F);
        private CMultimetr Multimetr = new CMultimetr();
        private ClassEni100 sensors = new ClassEni100(MaxChannalCount / MaxLevelCount);
        private FormSwitch Commutator = new FormSwitch();
        private FormMensor Mensor = new FormMensor();
        public static FormSensorsDB SensorsDB = new FormSensorsDB();
        private CThermalCamera ThermalCamera = new CThermalCamera();
        private CCalculation CalculationMtx = new CCalculation();

        //        private int MaxChannalCount = 30;//максимальное количество каналов коммутатора

        //        private СResultCH ResultCH = new СResultCH(MaxChannalCount);//результаты характеризации датчиков
        //        private CResultCI ResultCI = new CResultCI(MaxChannalCount);//результаты характеризации датчиков
        private СResultCH ResultCH = null;//результаты характеризации датчиков
        private CResultCI ResultCI = null;//результаты калибровки тока датчиков
        private CResultVR ResultVR = null;//результаты калибровки тока датчиков


        private int CommutatorReadError = 0;//число ошибко чтения данных с коммутатора 
        private int MultimetrReadError = 0;//число ошибко чтения данных с мультиметра
        private int MensorReadError = 0;//число ошибко чтения данных с менсора
        private bool SensorBusy = false;//Признак обмена данными с датчиками
        private bool ProcessStop = false;//Флаг остановки операции

        private bool TemperatureReady = false;//готовность термокамеры , температура датчиков стабилизирована
        private bool PressureReady = false;//готовность менсора , давление в датчиках стабилизировано
        private bool isSensorRead = false;
        //        private bool SensorPeriodRead = false;//Переодиское чтение параметров датчика

        private int SelectedLevel = 1;//выбранный номер уровеня характеризации



        //Инициализация переменных основной программы
        public MainForm()
        {
            InitializeComponent();
            Program.txtlog = new CTxtlog(rtbConsole, "Charakterizator.log");//создаем класс лог, с выводов в richtextbox и в файл
                                                                            //                        btmMultimetr_Click(null, null);           
                                                                            //                        btnCommutator_Click(null, null);
                                                                            //                        btnMensor_Click(null, null);
                                                                            // Properties.Settings.Default.Reset();

            try
            {
                // ЗАГРУЗКА настроек из файла settings - в переменные, 
                // Общие настройки программы
                MAIN_TIMER = Properties.Settings.Default.set_MainTimer;                     // Общий интервал опроса
                MAX_ERROR_COUNT = Properties.Settings.Default.set_MaxErrorCount;            //Количество ошибок чтения данных с устройств перед отключением
                MIN_SENSOR_CURRENT = Properties.Settings.Default.set_MinSensorCurrent;      //минимльный ток датчика для обнаружения, мА
                MAX_COUNT_CAP_READ = Properties.Settings.Default.set_MaxCountCAPRead;       //максимальное количество циклов чтения тока ЦАП
                SKO_CURRENT = Properties.Settings.Default.set_SKOCurrent;                   //допуск по току ЦАП датчика до калибровки, мА
                SKO_CALIBRATION_CURRENT = Properties.Settings.Default.set_SKOCalibrationCurrent; //допуск по току ЦАП после калибровки, мА            
                Multimetr.REZISTOR = Properties.Settings.Default.set_Rezistor;              // сопротивление резистора

                Multimetr.WAIT_READY = Properties.Settings.Default.set_MultimDataReady;     //время ожидания стабилизации тока, мсек
                Multimetr.WAIT_TIMEOUT = Properties.Settings.Default.set_MultimReadTimeout; //таймаут ожидания ответа от мультиметра, мсек
                Multimetr.SAMPLE_COUNT = Properties.Settings.Default.set_MultimReadCount;   //количество отчетов измерения мультиметром, раз
                Multimetr.READ_PERIOD = Properties.Settings.Default.set_MultimReadPeriod;   //период опроса мультиметра, мсек

                Commutator.MAX_SETCH = Properties.Settings.Default.set_CommMaxSetCH;        // максимально разрешенное коичество подключаемых к изм. линии датчиков 15
                Commutator.READ_PERIOD = Properties.Settings.Default.set_CommReadPeriod;    // Время опроса и обновление информации, мс
                Commutator.READ_PAUSE = Properties.Settings.Default.set_CommReadPause;      // время выдержки после переключения коммутатора (переходные процессы), мс
                MaxChannalCount = Properties.Settings.Default.set_CommReadCH;               // максимальное количество каналов коммутаторы
                MaxLevelCount = Properties.Settings.Default.set_CommMaxLevelCount;          // максимальное количество уровней датчиков (идентичных групп)

                Mensor.READ_PERIOD = Properties.Settings.Default.set_MensorReadPeriod;      // Время опроса состояния менсора при работе с формой
                Mensor.READ_PAUSE = Properties.Settings.Default.set_MensorReadPause;        // задержка между приемом и передачей команд по COM порту, мс     

                MAX_COUNT_POINT = Properties.Settings.Default.set_MensorMaxCountPoint/MAIN_TIMER+1;      //ожидание стабилизации давления в датчике, в циклах таймера


                SKO_PRESSURE = Properties.Settings.Default.set_MensorSKOPressure;           //(СКО) допуск по давлению, кПа

                sensors.WAIT_TIMEOUT = Properties.Settings.Default.set_SensWaitTimeout;     //таймаут ожидания ответа от датчика
                sensors.WRITE_COUNT = Properties.Settings.Default.set_SensReadCount;        //число попыток записи команд в датчик
                sensors.WRITE_PERIOD = Properties.Settings.Default.set_SensReadPause;       //период выдачи команд

            }
            // нужно ли вычислять MaxSensorOnLevel = 8;//количество датиков на уровне
            catch
            {
                Program.txtlog.WriteLineLog("Не удалось загрузить настройки из файла", 1);
            }




            //********************  Цифровой шрифт *********************
            tbDateTime.Font = DrawingFont;
            tbMensorData.Font = DrawingFont;
            tbNumCH.Font = DrawingFont;
            tbMultimetrData.Font = DrawingFont;
            //tbMensorRate.Font = DrawingFont;
            tbTemperature.Font = DrawingFont;
            numMensorPoint.Font = DrawingFont;
            numTermoCameraPoint.Font = DrawingFont;
            //**********************************************************
            UpdateItems();//обновляем списки визуальных элементов
        }


        //Обновление визуальных элементов согласно установленным параметрам
        private void UpdateItems()
        {
//            cbChannalCharakterizator.Items.Clear();
//            cbChannalVerification.Items.Clear();
            dataGridView1.Rows.Clear();
            for (int i = 0; i < MaxChannalCount; i++)
            {
                dataGridView1.Rows.Add(i + 1, false, "Нет данных", "Нет данных", false, false);
                dataGridView1[pow, i].Style.BackColor = Color.IndianRed;
                dataGridView1[ok, i].Style.BackColor = Color.IndianRed;

//                cbChannalCharakterizator.Items.Add(string.Format("Канал {0}", i + 1));
//                cbChannalVerification.Items.Add(string.Format("Канал {0}", i + 1));
            }
        }

        private void UpDateSelectedChannal()
        {
            cbChannalCharakterizator.Items.Clear();
            cbChannalVerification.Items.Clear();
            for (int i = 0; i < MaxChannalCount; i++)
            {
                if (!CheckChannalEnable(i)) continue;//Если канал не выбран пропускаем 
                cbChannalCharakterizator.Items.Add(string.Format("Канал {0}", i + 1));
                cbChannalVerification.Items.Add(string.Format("Канал {0}", i + 1));
            }
        }

        //Выполняем при загрузке главной формы
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                //                Visible = false;
                string strFileNameDB = Charaterizator.Properties.Settings.Default.FileNameDB;   // получаем путь и имя файла из Settings
                SensorsDB.SetConnectionDB(strFileNameDB);                                       // устанавливаем соединение с БД           
                // устанавливаем связь с БД
                btnMultimetr.PerformClick();
                btnCommutator.PerformClick();
                btnMensor.PerformClick();
                btnThermalCamera.PerformClick();
                Application.DoEvents();

                MainTimer.Interval = MAIN_TIMER;
                MainTimer.Enabled = true;
                MainTimer.Start();
            }
            finally
            {
                //                Visible = true;
            }
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
            //Properties.Settings.Default.set_SensReadPause
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
            MultimetrReadError = 0;
            if (Multimetr.Connect(Properties.Settings.Default.COMMultimetr,
                Properties.Settings.Default.COMMultimetr_Speed,
                Properties.Settings.Default.COMMultimetr_DatabBits,
                Properties.Settings.Default.COMMultimetr_StopBits,
                Properties.Settings.Default.COMMultimetr_Parity) >= 0)
            {
                btnMultimetr.BackColor = Color.Green;
                btnMultimetr.Text = "Подключен";
                Program.txtlog.WriteLineLog("Мультиметр подключен", 0);
            }
            else
            {
                btnMultimetr.BackColor = Color.IndianRed;
                btnMultimetr.Text = "Не подключен";
                Program.txtlog.WriteLineLog("Мультиметр не подключен", 1);
            }
        }



        // Подключение КОММУТАТОРА
        private void btnCommutator_Click(object sender, EventArgs e)
        {
            CommutatorReadError = 0;
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
            MensorReadError = 0;
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

                Mensor.MenStartTimer();
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
            dataGridView1.Rows[i].Cells[sen].Value = sensors.sensor.GetdevType() + " : " + new String(sensors.sensor.PressureModel);     //тип датчика
            dataGridView1.Rows[i].Cells[zn].Value = sensors.sensor.uni.ToString();   //заводской номер
            dataGridView1.Rows[i].Cells[sel].Value = true;   //Добавлен в список
                                                             //            dataGridView1.Rows[i].Cells[3].Style.BackColor = Color.Green;
            dataGridView1.Rows[i].Cells[ok].Style.BackColor = Color.Green;
            dataGridView1.Rows[i].Cells[ok].Value = true;                            //исправность датчика                            
        }
        private void UpdateSensorInfoPanel(int i)
        {
            if (SensorBusy)
            {
                Program.txtlog.WriteLineLog("Процесс занят. Дождитесь завершения текущих операций", 0);
                return;
            }

            if (sensors.SelectSensor(i))
            {
                tbSelChannalNumber.Text = string.Format("Канал {0}", i + 1);
                tbInfoDesc.Text = sensors.sensor.GetDesc();
                tbInfoTeg.Text = sensors.sensor.GetTeg();
                tbInfoPressureModel.Text = new String(sensors.sensor.PressureModel);
                tbInfoUp.Text = sensors.sensor.UpLevel.ToString("f6");
                tbInfoDown.Text = sensors.sensor.DownLevel.ToString("f6");
                tbInfoSerialNumber.Text = sensors.sensor.SerialNumber.ToString();
                tbInfoMin.Text = sensors.sensor.MinLevel.ToString("f6");
                tbInfoMesUnit.Text = sensors.sensor.GetUnit();

                DateTime dt = new DateTime(1900 + (int)(sensors.sensor.data & 0xFF), (int)(sensors.sensor.data >> 8) & 0xFF, (int)((sensors.sensor.data >> 16) & 0xFF));
                dtpInfoDate.Value = dt;

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
                DialogResult result = MessageBox.Show(
                        "Выполнить поиск датчика?",
                        "Датчик на выбранной линии не обнаружен!",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    // коммутируем
                    Commutator.SetConnectors(i, 0); // команда подключить датчик с индексом i
                    if (sensors.IsConnect())
                    {
                        if (sensors.SeachSensor(i))//поиск датчиков
                        {
                            Thread.Sleep(100);
                            if (sensors.SelectSensor(i))//выбор обнаруженного датчика
                            {//датчик найден, обновляем таблицу
                                Program.txtlog.WriteLineLog("Датчик обнаружен! Выполняем чтение параметров датчика по HART.", 0);
                                sensors.EnterServis();
                                sensors.TegRead();          //читаем инфомацию о датчике
                                sensors.C14SensorRead();       //чтение данных с датчика
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

                    //Commutator.SetConnectors(i, 1); // команда отключить датчик с индексом i                    
                }
            }
        }

        //чтение ЦАП
        private void ReadSensorCurrent()
        {
            int seli = 0;
            int StartNumber = 0;    //начальный канал
            int FinishNumber = MaxChannalCount - 1;   //конечный канал

            Program.txtlog.WriteLineLog("CAP: Старт операции чтения ЦАП ... ", 2);

            pbCHProcess.Maximum = FinishNumber - StartNumber;
            pbCHProcess.Minimum = 0;
            pbCHProcess.Value = 0;

            for (int i = StartNumber; i <= FinishNumber; i++)//перебор каналов
            {
                if (ProcessStop) return;//прекращаем поиск 

                pbCHProcess.Value = i - StartNumber;
                if (!CheckChannalEnable(i)) continue;//Если канал не выбран пропускаем обработку

                Application.DoEvents();
                Commutator.SetConnectors(i, 0);
                Thread.Sleep(Commutator.READ_PERIOD);//ждем переключения

                if (sensors.SelectSensor(i))//выбор датчика на канале i
                {
                    Program.txtlog.WriteLineLog("CAP: Выполняем чтение токов датчика в канале " + (i + 1).ToString(), 0);

                    double I4 = 0, I20 = 0;
                    int ci = 0;//счетчик циклов чтения ЦАП
                    do
                    {
                        if (sensors.С40WriteFixCurrent(4))
                        {
                            Thread.Sleep(Multimetr.WAIT_READY);
                            I4 = Multimetr.Current;
                            Program.txtlog.WriteLineLog("CAP: Выполнено чтение тока 4мА с мультиметра в канале " + (i + 1).ToString(), 0);
                        }
                        else
                        {
                            I4 = 0;
                            Program.txtlog.WriteLineLog("CAP:Ток 4мА не установлен!", 1);
                        }
                        ci++;
                        Application.DoEvents();

                    } while ((Math.Abs(I4-4.0) > SKO_CURRENT) && (ci< MAX_COUNT_CAP_READ));

                    ci = 0;
                    do
                    {
                        if (sensors.С40WriteFixCurrent(20))
                        {
                            Thread.Sleep(Multimetr.WAIT_READY);
                            I20 = Multimetr.Current;
                            Program.txtlog.WriteLineLog("CAP:Выполнено чтение тока 20мА с мультиметра в канале " + (i + 1).ToString(), 0);
                        }
                        else
                        {
                            I20 = 0;
                            Program.txtlog.WriteLineLog("CAP:Ток 20мА не установлен!", 1);
                        }
                        ci++;
                        Application.DoEvents();

                    } while ((Math.Abs(I20 - 20.0) > SKO_CURRENT) && (ci < MAX_COUNT_CAP_READ));

                    ResultCI.AddPoint(i, (double)numTermoCameraPoint.Value, I4, I20);

                    if (!cbChannalFix.Checked)
                    {//если стоит фиксация канал не меняем
                        cbChannalCharakterizator.SelectedIndex = seli;
                        UpDateCharakterizatorGrid(i);
                        UpdateCurrentGrid(i);
                        UpdateUpStatus(i);
                    }
                    else
                    {
                        if (cbChannalCharakterizator.SelectedIndex == seli)
                        {
                            UpdateCurrentGrid(i);
                        }
                    }

                }
                else
                {
                    Program.txtlog.WriteLineLog("CAP: Датчик не найден в канале " + (i + 1).ToString(), 1);
                }
                sensors.С40WriteFixCurrent(0);
                //Commutator.SetConnectors(i, 1); // команда отключить датчик с индексом i           
                seli++;
            }
            Program.txtlog.WriteLineLog("Чтение ЦАП завершено!", 2);

        }

        //Калибровка датчиков
        private void SensorCalibration()
        {
            int seli = 0;
            pbCHProcess.Maximum = MaxChannalCount;
            pbCHProcess.Minimum = 0;
            pbCHProcess.Value = 0;
            int ci = 0;
            int cc = 0;
            float I4 = 0;
            float I20=0;

            Program.txtlog.WriteLineLog("CL: Старт калибровки тока датчиков. Температура: " + numTermoCameraPoint.Text, 2);
            for (int i = 0; i < MaxChannalCount; i++)
            {
                if (ProcessStop) return;//прекращаем поиск 
                pbCHProcess.Value = i + 1;

                if (!CheckChannalEnable(i)) continue;//Если канал не выбран пропускаем обработку
                if (!cbChannalFix.Checked)
                {//если стоит фиксация канал не меняем
                    cbChannalCharakterizator.SelectedIndex = seli;
                    UpDateCharakterizatorGrid(i);
                    UpdateCurrentGrid(i);
                    UpdateUpStatus(i);
                }


                Application.DoEvents();
                Commutator.SetConnectors(i, 0);
                Thread.Sleep(Commutator.READ_PERIOD);//ждем переключения

                if (sensors.SelectSensor(i))
                {
                    Program.txtlog.WriteLineLog(string.Format("CL: Датчик в канале {0} подключен, выполяем калибровку...", i + 1), 0);

                    cc = 0;
                    do//цикл калибровки (MAX_CALIBRATION_COUNT попыток)
                    {
                        ci = 0;
                        sensors.С40WriteFixCurrent(4);
                        do//цикл чтения тока (MAX_COUNT_CAP_READ попыток)
                        {
                            Thread.Sleep(Multimetr.WAIT_READY);
                            I4 = Multimetr.Current;
                            ci++;
                        } while ((Math.Abs(I4 - 4.0) > SKO_CURRENT) && (ci < MAX_COUNT_CAP_READ));
                        Application.DoEvents();

                        if (Math.Abs(I4 - 4.0) > SKO_CURRENT)
                        {
                            DialogResult result = MessageBox.Show(
                                    "Выполнить калибровку?",
                                    "Превышено максимальное отклонение тока ЦАП (4мА)!",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Information,
                                    MessageBoxDefaultButton.Button1);
                            if (result == DialogResult.No)
                            {
                                cc++;
                                continue;
                            }
                        }
                        sensors.С45WriteCurrent4mA(I4);//Калибруем...
                        Program.txtlog.WriteLineLog("CL:Выполняем калибровку ЦАП 4мА...", 0);

                        ci = 0;
                        sensors.С40WriteFixCurrent(20);
                        do//цикл чтения тока (MAX_COUNT_CAP_READ попыток)
                        {
                            Thread.Sleep(Multimetr.WAIT_READY);
                            I20 = Multimetr.Current;
                            ci++;
                        } while ((Math.Abs(I20 - 20.0) > SKO_CURRENT) && (ci < MAX_COUNT_CAP_READ));
                        Application.DoEvents();

                        if (Math.Abs(I20 - 20.0) > SKO_CURRENT)
                        {
                            DialogResult result = MessageBox.Show(
                                    "Выполнить калибровку?",
                                    "Превышено максимальное отклонение тока ЦАП (20мА)!",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Information,
                                    MessageBoxDefaultButton.Button1);
                            if (result == DialogResult.No)
                            {
                                cc++;
                                continue;
                            }
                        }

                        sensors.С46WriteCurrent20mA(I20);//Калибруем...
                        Program.txtlog.WriteLineLog("CL:Выполняем калибровку ЦАП 20мА...", 0);
                        
                        ci = 0;
                        do//цикл чтения тока (MAX_COUNT_CAP_READ попыток)
                        {
                            sensors.С40WriteFixCurrent(4);
                            Thread.Sleep(Multimetr.WAIT_READY);
                            I4 = Multimetr.Current;
                            ci++;
                        } while ((Math.Abs(I4 - 4.0) > SKO_CALIBRATION_CURRENT) && (ci < MAX_COUNT_CAP_READ));
                        Application.DoEvents();

                        ci = 0;
                        do//цикл чтения тока (MAX_COUNT_CAP_READ попыток)
                        {
                            sensors.С40WriteFixCurrent(20);
                            Thread.Sleep(Multimetr.WAIT_READY);
                            I20 = Multimetr.Current;
                            ci++;
                        } while ((Math.Abs(I20 - 20.0) > SKO_CALIBRATION_CURRENT) && (ci < MAX_COUNT_CAP_READ));

                        cc++;
                        Application.DoEvents();

                    } while ((Math.Abs(I4 - 4.0) > SKO_CALIBRATION_CURRENT) && (Math.Abs(I20 - 20.0) > SKO_CALIBRATION_CURRENT) && (cc < MAX_CALIBRATION_COUNT));

                    if ((Math.Abs(I4 - 4.0) > SKO_CALIBRATION_CURRENT)&& (Math.Abs(I20 - 20.0) > SKO_CALIBRATION_CURRENT))
                    {
                        Program.txtlog.WriteLineLog("CL: Значение тока ЦАП вне допуска. Калибровка не выполнена!", 1);
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog(string.Format("CL: Калибровка датчика в канале {0} завершена", i + 1), 0);
                    }
                    sensors.С40WriteFixCurrent(0);
                }
                else
                {
                    Program.txtlog.WriteLineLog(string.Format("CL: Датчик не обнаружен в канале {0}", i + 1), 1);
                }
                //Commutator.SetConnectors(i, 1);
                seli++;
            }
            Program.txtlog.WriteLineLog("CL: Калибровка ЦАП завершена!", 2);
        }


        //Проверка доступности канала(по выбору пользователя)
        private bool CheckChannalEnable(int i)
        {
            if (i >= dataGridView1.Rows.Count) return false;

            if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[sel].Value))   //Добавлен в список?
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //Чтение параметров выбранного датчика
        private void ReadSensor()
        {
            if (cbChannalCharakterizator.Text == "") return;
            try
            {
                isSensorRead = true;

                string str = cbChannalCharakterizator.Text.Remove(0, 6);
                int i = Convert.ToInt32(str)-1;

                //                int i = cbChannalCharakterizator.SelectedIndex;

                if ((i < 0) || !CheckChannalEnable(i)) return;//Если канал не выбран пропускаем обработку

                Commutator.SetConnectors(i, 0);
                if (sensors.SelectSensor(i))//выбор датчика на канале i
                {

                    if (sensors.SensorValueReadC03())
                    {
                        if (!sensors.ValidateSensorParam())
                        {
                            Program.txtlog.WriteLineLog("Считаны недопустимые параметры датчика в канале " + (i + 1).ToString(), 1);
                        }
                        else
                        {
                             UpdateUpStatus(i);
//                            ResultCH.Update(i, (double)numTermoCameraPoint.Value, Diapazon, (double)numMensorPoint.Value, sensors.sensor.OutVoltage, sensors.sensor.Resistance);
//                            UpDateCharakterizatorGrid(i);
                        }
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("Параметры датчика не прочитаны в канале " + (i + 1).ToString(), 1);
                    }
                }
            }
            finally
            {
                isSensorRead = false;
            }
        }
        //характеризация датчиков
        //чтение всех измеренных параметров с текущего датчика давления
        private void ReadSensorParametrs()
        {
            int seli = 0;
            int StartNumber = 0;    //начальный канал
            int FinishNumber = MaxChannalCount-1;   //конечный канал
            int Diapazon;
            if (cbDiapazon1.Text != "")
            {
                Diapazon = Convert.ToInt32(cbDiapazon1.Text);
            }
            else
            {
                Diapazon = 1;
            }

            Program.txtlog.WriteLineLog("CH: Старт операции характеризации для выбранных датчиков ... ", 2);

            //******** расчитываем номера каналов текущего выбранного уровня ********************************
/*            int step = MaxChannalCount / MaxLevelCount;
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
            }*/
            //************************************************************************************************

            pbCHProcess.Maximum = FinishNumber - StartNumber;
            pbCHProcess.Minimum = 0;
            pbCHProcess.Value = 0;

            for (int i = StartNumber; i <= FinishNumber; i++)//перебор каналов
            {
                if (ProcessStop) return;//прекращаем поиск 

                pbCHProcess.Value = i - StartNumber;
                Application.DoEvents();
                if (!CheckChannalEnable(i)) continue;//Если канал не выбран пропускаем обработку
                Commutator.SetConnectors(i, 0);
                Thread.Sleep(Commutator.READ_PERIOD);//ждем переключения

                if (sensors.SelectSensor(i))//выбор датчика на канале i
                {
                    bool readresult = false;
                    int ch = 0;
                    do
                    {
                        readresult =  sensors.SensorValueReadC03();
                        Application.DoEvents();
                        ch++;
                    } while ((!sensors.ValidateSensorParam()) && (ch < sensors.WRITE_COUNT));

                    if (readresult)
                    {
                        if (!sensors.ValidateSensorParam())
                        {
                            Program.txtlog.WriteLineLog("CH: Считаны недопустимые параметры датчика в канале " + (i + 1).ToString(), 1);
                        }
                        else
                        {
                            ResultCH.AddPoint(i, (double)numTermoCameraPoint.Value, Diapazon, (double)numMensorPoint.Value, sensors.sensor.OutVoltage, sensors.sensor.Resistance);
                            if (!cbChannalFix.Checked)
                            {//если стоит фиксация канал не меняем
                                cbChannalCharakterizator.SelectedIndex = seli;
                                UpDateCharakterizatorGrid(i);
                                UpdateCurrentGrid(i);
                                UpdateUpStatus(i);
                            }
                            else
                            {
                                if (cbChannalCharakterizator.SelectedIndex == seli)
                                {
                                    UpDateCharakterizatorGrid(i);
                                }
                            }
                            Program.txtlog.WriteLineLog("CH: Выполнено чтение параметров датчика в канале " + (i + 1).ToString(), 0);
                        }
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
                //Commutator.SetConnectors(i, 1); // команда отключить датчик с индексом i   
                seli++;
            }
            Program.txtlog.WriteLineLog("CH: Операция характеризации завершена!", 2);
        }

        //расчет коэффициентов и запись в датчики
        private void СaclSensorCoeff()
        {
            Program.txtlog.WriteLineLog("CH: Тест 4... ", 2);

            int seli = 0;
            int StartNumber = 0;    //начальный канал
            int FinishNumber = MaxChannalCount - 1;   //конечный канал

            Program.txtlog.WriteLineLog("CH: Старт расчета коэффициентов для выбранных датчиков ... ", 2);

            pbCHProcess.Maximum = FinishNumber - StartNumber;
            pbCHProcess.Minimum = 0;
            pbCHProcess.Value = 0;
            Program.txtlog.WriteLineLog("CH: Тест 5... ", 2);

            for (int i = StartNumber; i <= FinishNumber; i++)//перебор каналов
            {
                if (ProcessStop) return;//прекращаем поиск 

                pbCHProcess.Value = i - StartNumber;
                Application.DoEvents();
                if (!CheckChannalEnable(i)) continue;//Если канал не выбран пропускаем обработку
                Commutator.SetConnectors(i, 0);
                Thread.Sleep(Commutator.READ_PERIOD);//ждем переключения

                if (sensors.SelectSensor(i))//выбор датчика на канале i
                {
                    double Diapazon = sensors.sensor.UpLevel - sensors.sensor.DownLevel;
                    if ((Diapazon <= 0) || (Diapazon > 1000000))
                    {
                        Program.txtlog.WriteLineLog("CH: Не верные НПИ и ВПИ датчика в канале:" + (i + 1).ToString(), 1);
                        continue;
                    }
                    // Из списка List формируем промежуточные матрицы Сопротивления, Напряжения и Давления
                    // размером 100х100
                    Matrix<double> mtxR = DenseMatrix.Create(30, 30, 0);
                    Matrix<double> mtxU = DenseMatrix.Create(30, 30, 0);
                    Matrix<double> mtxP = DenseMatrix.Create(30, 30, 0);

                    double val = -1000;
                    int c_row = 0;
                    int c_cols = 0;

                    for (int j = 0; j < ResultCH.Channal[i].Points.Count; j++)
                    {
                        if ((val != ResultCH.Channal[i].Points[j].Temperature) && (j != 0))
                        {
                            c_cols = c_cols + 1;
                            c_row = 0;
                        }
                        val = ResultCH.Channal[i].Points[j].Temperature;
                        mtxP[c_row, c_cols] = ResultCH.Channal[i].Points[j].Pressure/Diapazon;
                        mtxR[c_row, c_cols] = ResultCH.Channal[i].Points[j].Resistance;
                        mtxU[c_row, c_cols] = ResultCH.Channal[i].Points[j].OutVoltage;
                        //mtxP[c_cols, c_row] = ResultCH.Channal[i].Points[j].Pressure / Diapazon;
                        //mtxR[c_cols, c_row] = ResultCH.Channal[i].Points[j].Resistance;
                        //mtxU[c_cols, c_row] = ResultCH.Channal[i].Points[j].OutVoltage;

                        c_row = c_row + 1;
                    }
                    c_cols = c_cols + 1;


                    // Создаем матрицы нужного размера и копируем в них 
                    // ненулевые данные из промежуочных матриц 
                    Matrix<double> Pnew = DenseMatrix.Create(c_row, c_cols, 0);
                    Matrix<double> Rnew = DenseMatrix.Create(c_row, c_cols, 0);
                    Matrix<double> Unew = DenseMatrix.Create(c_row, c_cols, 0);

                    for (int ii = 0; ii < c_row; ii++)
                    {
                        for (int jj = 0; jj < c_cols; jj++)
                        {
                            Rnew[ii, jj] = mtxR.At(ii, jj);
                            Pnew[ii, jj] = mtxP.At(ii, jj);
                            Unew[ii, jj] = mtxU.At(ii, jj);
                        }
                    }

                    try
                    {
                        
                        Matrix<double> ResulCoefmtx = CalculationMtx.CalculationCoef(Rnew, Unew, Pnew);
                        if (ResulCoefmtx.RowCount != 24)
                        {
                            Program.txtlog.WriteLineLog("CH: Количество точек при характеризация не равно 24.", 1);
                            continue;
                        }
                        Program.txtlog.WriteLineLog("CH: Расчитанные коэффициенты для датчика в канале " + (i + 1).ToString(), 0);
                        for (int j = 0; j < ResulCoefmtx.RowCount; j++)
                        {
                            Program.txtlog.WriteLineLog("Коэффициент "+ (j + 1).ToString() + ": " + ResulCoefmtx.At(j, 0), 0);
                            if (j < 24)
                                sensors.sensor.Coefficient[j] = Convert.ToSingle(ResulCoefmtx.At(j, 0));
                        }
                    }
                    catch
                    {
                        Program.txtlog.WriteLineLog("CH: Ошибка расчета коэффициентов для датчика в канале " + (i + 1).ToString(), 1);
                    }

                    if (!sensors.С15ReadVPI_NPI())
                    {
                        Program.txtlog.WriteLineLog("CH: Ошибка чтения НПИ и ВПИ датчик в канале " + (i + 1).ToString(), 1);
                    }
                    if (!sensors.C250SensorCoefficientWrite())
                    {
                        Program.txtlog.WriteLineLog("CH: Ошибка записи коэффициентов в датчик в канале " + (i + 1).ToString(), 1);
                    }
                }
                else
                {
                    Program.txtlog.WriteLineLog("CH: Датчик не найден в канале " + (i + 1).ToString(), 1);
                }
                //Commutator.SetConnectors(i, 1); // команда отключить датчик с индексом i   
                seli++;
            }
            Program.txtlog.WriteLineLog("CH: Операция вычисления коэффициентов завершена!", 2);
        }


        //верификация датчиков
        //чтение всех измеренных параметров с текущего датчика давления
        private void ReadSensorPressure()
        {
            int seli = 0;
            int StartNumber = 0;    //начальный канал
            int FinishNumber = MaxChannalCount-1;   //конечный канал
            float VPI, NPI;
            VPI = Convert.ToSingle(nud_VR_VPI.Value);
            NPI = Convert.ToSingle(nud_VR_NPI.Value);

            Program.txtlog.WriteLineLog("VR: Старт операции верификации для выбранных датчиков ... ", 2);

            //******** расчитываем номера каналов текущего выбранного уровня ********************************
 /*           int step = MaxChannalCount / MaxLevelCount;
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
            }*/
            //************************************************************************************************

            pbVRProcess.Maximum = FinishNumber - StartNumber;
            pbVRProcess.Minimum = 0;
            pbVRProcess.Value = 0;
            for (int i = StartNumber; i <= FinishNumber; i++)//перебор каналов
            {
                if (ProcessStop) return;//прекращаем верификацию 

                pbVRProcess.Value = i - StartNumber;
                Application.DoEvents();
                if (!CheckChannalEnable(i)) continue;//Если канал не выбран пропускаем обработку

                Commutator.SetConnectors(i, 0);
                Thread.Sleep(Commutator.READ_PERIOD);//ждем переключения


                if (sensors.SelectSensor(i))//выбор датчика на канале i
                {
                    if (sensors.С15ReadVPI_NPI())
                    {
                        Program.txtlog.WriteLineLog("VR: Выполнено чтение НПИ ВПИ датчика в канале " + (i + 1).ToString(), 0);
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("VR: Ошибка чтения НПИ ВПИ датчика в канале " + (i + 1).ToString(), 1);
                    }


                    if (sensors.SensorValueReadC03())
                    {
                        Thread.Sleep(Multimetr.WAIT_READY);//ждем измерения мультиметром

                        float Ir = 4 + (16 / (sensors.sensor.VPI - sensors.sensor.NPI)) * ((float)numMensorPoint.Value - sensors.sensor.NPI);//расчетный ток
                        ResultVR.AddPoint(i, (double)numTermoCameraPoint.Value, sensors.sensor.NPI, sensors.sensor.VPI, (double)numMensorPoint.Value, sensors.sensor.Pressure, Multimetr.Current, Ir);

                        if (!cbChannalFixVR.Checked)
                        {//если стоит фиксация канал не меняем
                            cbChannalVerification.SelectedIndex = seli;
                            UpDateVerificationGrid(i);
                            UpdateUpStatus(i);
                        }
                        else
                        {
                            if (cbChannalVerification.SelectedIndex == seli)
                            {
                                UpDateVerificationGrid(i);
                            }
                        }

//                        UpDateVerificationGrid(i);
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
                //Commutator.SetConnectors(i, 1); // команда отключить датчик с индексом i                    
                seli++;
            }
            Program.txtlog.WriteLineLog("VR: Операция верификации завершена ... ", 2);
        }

        //Запись НПИ и ВПИ в выбранные датчики
        private void WriteSensorVPI_NPI()
        {
            int StartNumber = 0;    //начальный канал
            int FinishNumber = MaxChannalCount - 1;   //конечный канал
            float VPI, NPI;
            VPI = Convert.ToSingle(nud_VR_VPI.Value);
            NPI = Convert.ToSingle(nud_VR_NPI.Value);

            Program.txtlog.WriteLineLog("VR: Старт записи НПИ ВПИ для выбранных датчиков ... ", 2);
            pbVRProcess.Maximum = FinishNumber - StartNumber;
            pbVRProcess.Minimum = 0;
            pbVRProcess.Value = 0;
            for (int i = StartNumber; i <= FinishNumber; i++)//перебор каналов
            {
                if (ProcessStop) return;//прекращаем верификацию 

                pbVRProcess.Value = i - StartNumber;
                Application.DoEvents();
                if (!CheckChannalEnable(i)) continue;//Если канал не выбран пропускаем обработку

                Commutator.SetConnectors(i, 0);
                Thread.Sleep(Commutator.READ_PERIOD);//ждем переключения

                if (sensors.SelectSensor(i))//выбор датчика на канале i
                {
                    if (sensors.С35WriteVPI_NPI(VPI,NPI))
                    {
                        Program.txtlog.WriteLineLog("VR: Выполнена запись НПИ ВПИ датчика в канале " + (i + 1).ToString(), 0);
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("VR: Запись НПИ ВПИ датчика не выполнена!", 1);
                    }
                }
                else
                {
                    Program.txtlog.WriteLineLog("VR: Датчик не найден в канале " + (i + 1).ToString(), 1);
                }
                //Commutator.SetConnectors(i, 1); // команда отключить датчик с индексом i                    

            }
            Program.txtlog.WriteLineLog("VR: Операция записи НПИ ВПИ завершена", 2);

        }

        //Установка нуля для датчиков
        private void SetZero()
        {
            int StartNumber = 0;    //начальный канал
            int FinishNumber = MaxChannalCount - 1;   //конечный канал

            Program.txtlog.WriteLineLog("VR: Установка нуля для выбранных датчиков ... ", 2);
            pbVRProcess.Maximum = FinishNumber - StartNumber;
            pbVRProcess.Minimum = 0;
            pbVRProcess.Value = 0;
            for (int i = StartNumber; i <= FinishNumber; i++)//перебор каналов
            {
                if (ProcessStop) return;//прекращаем верификацию 

                pbVRProcess.Value = i - StartNumber;
                Application.DoEvents();
                if (!CheckChannalEnable(i)) continue;//Если канал не выбран пропускаем обработку

                Commutator.SetConnectors(i, 0);
                Thread.Sleep(Commutator.READ_PERIOD);//ждем переключения

                if (sensors.SelectSensor(i))//выбор датчика на канале i
                {
                    if (sensors.С43SetZero())
                    {

                        Program.txtlog.WriteLineLog("VR: Выполнена установка нуля датчика в канале " + (i + 1).ToString(), 0);
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("VR: Установка нуля датчика не выполнена!", 1);
                    }
                }
                else
                {
                    Program.txtlog.WriteLineLog("VR: Датчик не найден в канале " + (i + 1).ToString(), 1);
                }
                //Commutator.SetConnectors(i, 1); // команда отключить датчик с индексом i                    

            }
            Program.txtlog.WriteLineLog("VR: Операция установки нуля завершена", 2);
        }


        //обновляем грид результатов характеризации для датчика в канале i
        private void UpDateCharakterizatorGrid(int i)
        {
            dataGridView2.Rows.Clear();
            if ((ResultCH == null) || (ResultCH.Channal.Count <= i) || (i<0) || (ResultCH.Channal[i].Points.Count <= 0))
            {
                Program.txtlog.WriteLineLog("Result CH: Результаты характеризации не сформированы!", 1);
                return;
            }
            for (int j = 0; j < ResultCH.Channal[i].Points.Count; j++)//заполняем грид данными текущего датчика
            {
                dataGridView2.Rows.Add(j + 1, "", "", "", "", "", "");
                dataGridView2.Rows[j].Cells[1].Value = ResultCH.Channal[i].Points[j].Datetime.ToString("dd.MM.yyyy HH:mm:ss");                 //
                dataGridView2.Rows[j].Cells[2].Value = ResultCH.Channal[i].Points[j].Temperature.ToString();   //
                dataGridView2.Rows[j].Cells[3].Value = ResultCH.Channal[i].Points[j].Diapazon.ToString();
                dataGridView2.Rows[j].Cells[4].Value = ResultCH.Channal[i].Points[j].Pressure.ToString("f");   //
                dataGridView2.Rows[j].Cells[5].Value = ResultCH.Channal[i].Points[j].OutVoltage.ToString("f");
                dataGridView2.Rows[j].Cells[6].Value = ResultCH.Channal[i].Points[j].Resistance.ToString("f");
            }
            dataGridView2.Sort(dataGridView2.Columns[0], ListSortDirection.Descending);
            dataGridView2.ClearSelection();
            dataGridView2.Rows[0].Selected = true;
//            dataGridView2.Rows[0].Cells[0].Selected = true;
            //dataGridView2.FirstDisplayedCell = dataGridView2.Rows[0].Cells[0];
        }

        //обновляем грид результатов верификации для датчика в канале i
        private void UpDateVerificationGrid(int i)
        {
            dataGridView3.Rows.Clear();

            if ((ResultVR == null) || (ResultVR.Channal.Count <= i) || (i < 0) || (ResultVR.Channal[i].Points.Count <= 0))
            {
                Program.txtlog.WriteLineLog("Result VR: Результаты верификации не сформированы!", 1);
                return;
            }

            for (int j = 0; j < ResultVR.Channal[i].Points.Count; j++)//заполняем грид данными текущего датчика
            {
                dataGridView3.Rows.Add(j+1, "", "", "", "", "", "", "", "");
                dataGridView3.Rows[j].Cells[1].Value = ResultVR.Channal[i].Points[j].Datetime.ToString("dd.MM.yyyy HH:mm:ss");      //
                dataGridView3.Rows[j].Cells[2].Value = ResultVR.Channal[i].Points[j].Temperature.ToString();   //
                dataGridView3.Rows[j].Cells[3].Value = ResultVR.Channal[i].Points[j].NPI.ToString();   //
                dataGridView3.Rows[j].Cells[4].Value = ResultVR.Channal[i].Points[j].VPI.ToString();   //
                dataGridView3.Rows[j].Cells[5].Value = ResultVR.Channal[i].Points[j].PressureZ.ToString("f3");
                dataGridView3.Rows[j].Cells[6].Value = ResultVR.Channal[i].Points[j].PressureF.ToString("f3");
                dataGridView3.Rows[j].Cells[7].Value = ResultVR.Channal[i].Points[j].CurrentR.ToString("f4");
                dataGridView3.Rows[j].Cells[8].Value = ResultVR.Channal[i].Points[j].CurrentF.ToString("f4");
            }
            dataGridView3.Sort(dataGridView3.Columns[0], ListSortDirection.Descending);
            dataGridView3.ClearSelection();
            dataGridView3.Rows[0].Selected = true;
//            dataGridView3.Rows[0].Cells[0].Selected = true;
        }

        //обновляем грид калибровки тока для датчика в канале i
        private void UpdateCurrentGrid(int i)
        {
            if ((ResultCI == null) || (ResultCI.Channal.Count <= i))
            {
                Program.txtlog.WriteLineLog("Result CL: Результаты чтения ЦАП не сформированы!", 1);
                return;
            }

            dataGridView4.Rows.Clear();
            for (int j = 0; j < ResultCI.Channal[i].Points.Count; j++)//заполняем грид данными текущего датчика
            {
                dataGridView4.Rows.Add("", "", "", "", "");
                dataGridView4.Rows[j].Cells[0].Value = ResultCI.Channal[i].Points[j].Datetime.ToString();                 //
                dataGridView4.Rows[j].Cells[1].Value = ResultCI.Channal[i].Points[j].Temperature.ToString("f2");   //
//                dataGridView4.Rows[j].Cells[2].Value = ResultCI.Channal[i].Points[j].Pressure.ToString("f");   //
                dataGridView4.Rows[j].Cells[2].Value = ResultCI.Channal[i].Points[j].I4.ToString("f4");
                dataGridView4.Rows[j].Cells[3].Value = ResultCI.Channal[i].Points[j].I20.ToString("f4");
            }
        }

        //Поиск подключенных датчиков
        //Формирует списко датчиков в датагриде
        //Выход: число подключенных датчиков
        private int SeachConnectedSensor()
        {
            bool SensorFind = false;
            try
            {
                Program.txtlog.WriteLineLog("Старт поиска датчиков...", 2);
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
                        if (ProcessStop) break;//прекращаем поиск 

                        if (!CheckChannalEnable(i)) continue;//Если канал не выбран пропускаем обработку

                        dataGridView1.Rows[i].Selected = true;
                        dataGridView1.Rows[i].Cells[ch].Selected = true;
                        Application.DoEvents();


                        Program.txtlog.WriteLineLog(string.Format("Поиск датчиков на линии {0} ...", i+1), 0);

                        // коммутируем
                        Commutator.SetConnectors(i, 0); // команда подключить датчик с индексом i
                        Thread.Sleep(Commutator.READ_PERIOD);//ждем переключения

                        //***** поиск датчиков по току потребления ********************************************
                        if (Multimetr.Connected)
                        {
                            //Multimetr.ReadData();
                            Thread.Sleep(Multimetr.WAIT_TIMEOUT);

                            double Current = Multimetr.Current;//чтение тока мультиметра 
                            if (Current < MIN_SENSOR_CURRENT)
                            {
                                //нет тока мультиметра, => датчик отсутсвует
                                //                                Program.txtlog.WriteLineLog("Датчик не обнаружен! Ток потребления: " + Current.ToString(), 1);
                                Program.txtlog.WriteLineLog("Датчик не обнаружен! Ток потребления менее 1.5мА!", 1);

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
                                sensors.EnterServis();
                                sensors.TegRead();          //читаем информацию о датчике
                                if(!sensors.C14SensorRead())       //чтение данных с датчика
                                    Program.txtlog.WriteLineLog(string.Format("Параметры ПД датчика на линии {0} не прочитаны!", i + 1), 1);
                               // Program.txtlog.WriteLineLog(string.Format("ВПИ ПД датчика {0}", sensors.sensor.UpLevel), 0);
                                if (!sensors.C140ReadPressureModel())//читаем модель ПД
                                    Program.txtlog.WriteLineLog(string.Format("Модель ПД датчика на линии {0} не прочитана!", i + 1), 1);

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

                        //Commutator.SetConnectors(i, 1); // команда отключить датчик с индексом i                    
                        pbSensorSeach.Value = i;
                    }
                    Program.txtlog.WriteLineLog("Поиск датчиков завершен!", 2);

                }
                else
                {
                    Program.txtlog.WriteLineLog("Нет соединения с датчиками. Проверте подключение коммутатора.", 1);
                }
            }
            finally
            {
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
                    int i;
                    for (i = 0; i < MaxChannalCount; i++)
                    {
                        if (CheckChannalEnable(i)) //Есть выбранные каналы?
                            break;
                    }
                    if (i < MaxChannalCount)
                    {
                        btnSensorSeach.Text = "Идет поиск датчиков... Остановить! ";
                        UpdateItemState(1);
                        SeachConnectedSensor();
                    }
                    else
                    {
                        MessageBox.Show("Не выбраны каналы для поиска датчиков","Операция прервана",MessageBoxButtons.OK);
                    }
                }
                finally
                {
                    btnSensorSeach.Text = "Поиск датчиков";
                    UpdateItemState(0);
                }
            }
            else
            {
                ProcessStop = true;
                Program.txtlog.WriteLineLog("Поиск прекращен по команде пользователя", 0);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Properties.Settings.Default.Save();
            MainTimer.Stop();
            MainTimer.Enabled = false;
            sensors.DisConnect();
            Mensor.DisConnect();
            Multimetr.DisConnect();
            Commutator.DisConnect();
        }




//**************************************************************************************************
//
// СРАБАТЫВАНИЕ ГЛАВНОГО ТАЙМЕРА
//
//**************************************************************************************************


        private void MainTimer_Tick(object sender, EventArgs e)
        {

            if (Commutator.Connected)
            {
                //StateComutators(Commutator._StateCH);
                PowerComutators(Commutator._StateCHPower); //обновляем данные с коммутатора
                // Обновление номера активного канала
                tbNumCH.Text = Convert.ToString(Commutator.ActivCH);
            }
            else
            {
                if (btnCommutator.BackColor != Color.IndianRed)
                {
                    btnCommutator.BackColor = Color.IndianRed;
                }
                else
                {
                    btnCommutator.BackColor = Color.Transparent;
                }
            }

            if (Multimetr.Connected)
            {
                ReadMultimetr(); //обновляем данные с мультиметра
            }
            else
            {
                if (btnMultimetr.BackColor != Color.IndianRed)
                {
                    btnMultimetr.BackColor = Color.IndianRed;
                }
                else
                {
                    btnMultimetr.BackColor = Color.Transparent;
                }
            }


            if (Mensor.Connected)
            {
                ReadMensor(); //обновляем данные с Менсора
            }
            else
            {
                if (btnMensor.BackColor != Color.IndianRed)
                {
                    btnMensor.BackColor = Color.IndianRed;
                }
                else
                {
                    btnMensor.BackColor = Color.Transparent;
                }
            }


            if (!SensorBusy && sensors.IsConnect()) 
            {
                if ((cbSensorPeriodRead.Checked)&&(!isSensorRead))
                {
                    ReadSensor(); //выполняем переодичекое чтение датчика
                }
            }
            else
            {
                cbSensorPeriodRead.Checked = false;
            }

            tbDateTime.Text = DateTime.Now.ToString();
        }


        //чтение данных с МЕНСОРА
        private void ReadMensor()
        {
            int CH_mensor = Mensor._activCH;    // Получаем номер активного канала (0 значит А,   1 значит B,   -1 = не прочитали )                   
            if (CH_mensor != -1)
            {                
               
                
                if (SKO_PRESSURE > Math.Abs(Mensor._press - Mensor.UserPoint))//Convert.ToDouble(numMensorPoint.Value)))
                {
                    MensorCountPoint++;
                    if (MensorCountPoint >= MAX_COUNT_POINT)
                    {
                        tbMensorData.BackColor = Color.MediumSeaGreen;
                    }
                    else
                    {
                        tbMensorData.BackColor = Color.Yellow;
                    }
                }
                else
                {
                    tbMensorData.BackColor = Color.White;
                    MensorCountPoint = 0;
                }
                
                // Получаем текущее значение давления и обновляем гл. форму 
                tbMensorData.Text = Mensor._press.ToString("f3");


                // Получаем тек. значение уставки  и обновляем гл. форму
                //numMensorPoint.Text = Mensor._point.ToString("f2");
                // Получаем тек. значение скорости  и обновляем гл. форму
                //tbMensorRate.Text = Mensor._rate.ToString("f2");

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

                switch (Mensor._mode)
                {
                    case 0:
                        bMensorMeas.BackColor = Color.LightGreen;
                        bMensorControl.BackColor = Color.Transparent;
                        bMensorVent.BackColor = Color.Transparent;
                        break;
                    case 1:
                        bMensorMeas.BackColor = Color.Transparent;
                        bMensorControl.BackColor = Color.LightGreen;
                        bMensorVent.BackColor = Color.Transparent;
                        break;
                    case 2:
                        bMensorMeas.BackColor = Color.Transparent;
                        bMensorControl.BackColor = Color.Transparent;
                        bMensorVent.BackColor = Color.LightGreen;
                        break;
                }


                MensorReadError = 0;
            }
            else
            {
                tbMensorData.Text = "-1";
                //tbMensorRate.Text = "-1";
                numMensorPoint.Text = "-1";
                cbMensorTypeR.SelectedIndex = -1;               
                MensorReadError++;
                Program.txtlog.WriteLineLog("Ошибка чтения данных с Менсора. Количество ошибок: " + MensorReadError.ToString(), 1);
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
            bool res = Multimetr.Error;
            if (!res)
            {
                tbMultimetrData.Text = Multimetr.Current.ToString("f3");
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
                btnMultimetr.BackColor = Color.IndianRed;
                btnMultimetr.Text = "Не подключен";
                Program.txtlog.WriteLineLog("Нет данных с мультиметра. Устройство отключено.", 1);
            }
        }



        // Обновление на главной форме состояния каналов коммутатора (checkbox)
        private void StateComutators(Int32 data32)
        {

            /* for (int i = 0; i < MaxChannalCount; i++)
             {
                 if (((data32 >> i) & 01) == 1)
                 {
                     //dataGridView1.Rows[i].Cells[4].Value = 1;
                     //dataGridView1[4, i].Style.BackColor = Color.Green;
                 }
                 else
                 {
                     //dataGridView1.Rows[i].Cells[4].Value = 0;
                     //dataGridView1[4, i].Style.BackColor = Color.IndianRed;
                 }
             }*/

            //tbNumCH.Text = Convert.ToString(Commutator._NumOfConnectInputs);
           
        }


        // Обновление на главной форме состояния питания коммутатора (checkbox)
        private void PowerComutators(Int32 data32)
        {
            if (data32 >= 0)
            {
                for (int i = 0; i < MaxChannalCount; i++)
                {
                    if (((data32 >> i) & 01) == 1)
                    {
                        dataGridView1.Rows[i].Cells[pow].Value = true;
                        dataGridView1[pow, i].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[pow].Value = false;
                        dataGridView1[pow, i].Style.BackColor = Color.IndianRed;
                    }
                }
                CommutatorReadError = 0;
            }
            else
            {
                CommutatorReadError++;
                if (CommutatorReadError >= MAX_ERROR_COUNT)
                {
                    Commutator.DisConnect();
                    btnCommutator.BackColor = Color.IndianRed;
                    btnCommutator.Text = "Не подключен";
                    Program.txtlog.WriteLineLog("Нет данных с коммутатора. Устройство отключено.", 1);
                }
            }
        }



        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!Commutator.Connected)
            {
                MessageBox.Show("Нет подключения к коммутатору!", "Операция прервана", MessageBoxButtons.OK, MessageBoxIcon.Stop);
//                return;
            }
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == sel)
            {
                if (System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Control)
                {
                    for (int i = e.RowIndex; i >= 0; i--)
                    {
                        if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[sel].Value) == true)
                        {
                            break;
                        }
                        else
                        {
                            dataGridView1.Rows[i].Cells[sel].Value = true;
                        }
                    }
                }
//                UpDateSelectedChannal();
            }


            //if (e.ColumnIndex <= 2)//выбор датчика     - было
            if (e.ColumnIndex != 1)//выбор датчика         
            {
            UpdateSensorInfoPanel(e.RowIndex);
            }

           /* if (e.ColumnIndex == 4)//Состояние датчика - подключение
            {
                dataGridView1.Rows[e.RowIndex].Cells[4].Value = !Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[4].Value);

                if (Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[4].Value))
                {
                    Commutator.SetConnectors(e.RowIndex, 0); // команда подключить датчик с индексом e.RowIndex
                    dataGridView1[4, e.RowIndex].Style.BackColor = Color.Green;

                }
                else
                {
                    Commutator.SetConnectors(e.RowIndex, 1); // команда отключить датчик с индексом e.RowIndex
                    dataGridView1[4, e.RowIndex].Style.BackColor = Color.IndianRed;

                }

            }*/

            if (e.ColumnIndex == pow)// Питание датчика - подключение
            {
                dataGridView1.Rows[e.RowIndex].Cells[pow].Value = !Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[pow].Value);


                if (Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[pow].Value))
                {
                    Commutator.SetPower(e.RowIndex, 0);     // команда подключить питание датчика
                    dataGridView1[pow, e.RowIndex].Style.BackColor = Color.Green;
                }
                else
                {
                    Commutator.SetPower(e.RowIndex, 1);   // команда отключить питание датчика
                    dataGridView1[pow, e.RowIndex].Style.BackColor = Color.IndianRed;
                }
            }

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int si = -1;

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
                        pUpStatusBar.Visible = false;
//                        label1.Visible = false;
//                        tbNumCH.Visible = false;
                        splitter1.Visible = false;

                        return;
                    }

                // ОКНО - ХАРАКТЕРИЗАЦИЯ    
                case 1:  
                    {
                        pUpStatusBar.Visible = true;
//                        label1.Visible = true;
//                        tbNumCH.Visible = true;
                        splitter1.Visible = true;
                        UpDateSelectedChannal();


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
                            si = sensors.FindSensorGroup(1);
                            if (si >= 0)
                            {
                                SelectModel = new String(sensors.sensorList[si].PressureModel);
                                // Определяем сколько диапазонов у датчиков                               
                                NumOfRange = Convert.ToInt16(SensorsDB.GetDataSensors(SelectModel, "NumOfRange"));  
                                                               
                                if (NumOfRange == 1)
                                {
                                    cbDiapazon1.SelectedIndex = 0;
                                    cbDiapazon1.Enabled = false;
                                    // Занесение данных из ДБ в combobox
                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarTempPoint1");  // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != "")
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbCHTermoCamera1.Items.Clear();
                                        cbCHTermoCamera1.Items.AddRange(SPcmbox);
                                        cbCHTermoCamera1.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != "")
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
                                    if (SensParam != "")
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbCHTermoCamera1.Items.Clear();

                                        cbCHTermoCamera1.Items.AddRange(SPcmbox);
                                        cbCHTermoCamera1.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != "")
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
                            si = sensors.FindSensorGroup(2);
                            if (si>=0)
                            {
                                SelectModel = new String(sensors.sensorList[si].PressureModel);
                                // Определяем сколько диапазонов у датчиков                               
                                NumOfRange = Convert.ToInt16(SensorsDB.GetDataSensors(SelectModel, "NumOfRange"));  

                                if (NumOfRange == 1)
                                {
                                    cbDiapazon2.SelectedIndex = 0;
                                    cbDiapazon2.Enabled = false;
                                    // Занесение данных из ДБ в combobox
                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarTempPoint1");  // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != "")
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbCHTermoCamera2.Items.Clear();
                                        cbCHTermoCamera2.Items.AddRange(SPcmbox);
                                        cbCHTermoCamera2.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != "")
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
                                    if (SensParam != "")
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbCHTermoCamera2.Items.Clear();
                                        cbCHTermoCamera2.Items.AddRange(SPcmbox);
                                        cbCHTermoCamera2.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != "")
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
                            si = sensors.FindSensorGroup(3);
                            if (si >= 0)
                            {
                                SelectModel = new String(sensors.sensorList[si].PressureModel);
                                // Определяем сколько диапазонов у датчиков                               
                                NumOfRange = Convert.ToInt16(SensorsDB.GetDataSensors(SelectModel, "NumOfRange"));

                                if (NumOfRange == 1)
                                {
                                    cbDiapazon3.SelectedIndex = 0;
                                    cbDiapazon3.Enabled = false;
                                    // Занесение данных из ДБ в combobox
                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarTempPoint1");  // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != "")
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbCHTermoCamera3.Items.Clear();
                                        cbCHTermoCamera3.Items.AddRange(SPcmbox);
                                        cbCHTermoCamera3.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != "")
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
                                    if (SensParam != "")
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbCHTermoCamera3.Items.Clear();
                                        cbCHTermoCamera3.Items.AddRange(SPcmbox);
                                        cbCHTermoCamera3.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != "")
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
                            si = sensors.FindSensorGroup(4);
                            if (si >= 0)
                            {
                                SelectModel = new String(sensors.sensorList[si].PressureModel);
                                // Определяем сколько диапазонов у датчиков                               
                                NumOfRange = Convert.ToInt16(SensorsDB.GetDataSensors(SelectModel, "NumOfRange"));

                                if (NumOfRange == 1)
                                {
                                    cbDiapazon4.SelectedIndex = 0;
                                    cbDiapazon4.Enabled = false;
                                    // Занесение данных из ДБ в combobox
                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarTempPoint1");  // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != "")
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbCHTermoCamera4.Items.Clear();
                                        cbCHTermoCamera4.Items.AddRange(SPcmbox);
                                        cbCHTermoCamera4.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != "")
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
                                    if (SensParam != "")
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbCHTermoCamera4.Items.Clear();
                                        cbCHTermoCamera4.Items.AddRange(SPcmbox);
                                        cbCHTermoCamera4.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "HarPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != "")
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
                        pUpStatusBar.Visible = true;
                        //label1.Visible = true;
                        //tbNumCH.Visible = true;
                        splitter1.Visible = false;
                        UpDateSelectedChannal();


                        // Занесение данных из ДБ в combobox                     
                        if (SensorsDB._сonnection.State == System.Data.ConnectionState.Open)
                        {
                            int NumOfRange = -1;
                            string SensParam;


                            // УРОВЕНЬ - 1
                            si = sensors.FindSensorGroup(1);
                            if (si >= 0)
                            {
                                SelectModel = new String(sensors.sensorList[si].PressureModel);
                                // Определяем сколько диапазонов у датчиков                               
                                NumOfRange = Convert.ToInt16(SensorsDB.GetDataSensors(SelectModel, "NumOfRange"));

                                if (NumOfRange == 1)
                                {
                                    cbVRDiapazon1.SelectedIndex = 0;
                                    cbVRDiapazon1.Enabled = false;
                                    // Занесение данных из ДБ в combobox
                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerTempPoint1");  // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != "")
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbVRTermoCamera1.Items.Clear();
                                        cbVRTermoCamera1.Items.AddRange(SPcmbox);
                                        cbVRTermoCamera1.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != "")
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
                                    if (SensParam != "")
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbVRTermoCamera1.Items.Clear();
                                        cbVRTermoCamera1.Items.AddRange(SPcmbox);
                                        cbVRTermoCamera1.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != "")
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
                            si = sensors.FindSensorGroup(2);
                            if (si >= 0)
                            {
                                SelectModel = new String(sensors.sensorList[si].PressureModel);
                                // Определяем сколько диапазонов у датчиков                               
                                NumOfRange = Convert.ToInt16(SensorsDB.GetDataSensors(SelectModel, "NumOfRange"));

                                if (NumOfRange == 1)
                                {
                                    cbVRDiapazon2.SelectedIndex = 0;
                                    cbVRDiapazon2.Enabled = false;
                                    // Занесение данных из ДБ в combobox
                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerTempPoint1");  // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != "")
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbVRTermoCamera2.Items.Clear();
                                        cbVRTermoCamera2.Items.AddRange(SPcmbox);
                                        cbVRTermoCamera2.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != "")
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
                                    if (SensParam != "")
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbVRTermoCamera2.Items.Clear();
                                        cbVRTermoCamera2.Items.AddRange(SPcmbox);
                                        cbVRTermoCamera2.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != "")
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
                            si = sensors.FindSensorGroup(3);
                            if (si >= 0)
                            {
                                SelectModel = new String(sensors.sensorList[si].PressureModel);
                                // Определяем сколько диапазонов у датчиков                               
                                NumOfRange = Convert.ToInt16(SensorsDB.GetDataSensors(SelectModel, "NumOfRange"));

                                if (NumOfRange == 1)
                                {
                                    cbVRDiapazon3.SelectedIndex = 0;
                                    cbVRDiapazon3.Enabled = false;
                                    // Занесение данных из ДБ в combobox
                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerTempPoint1");  // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != "")
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbVRTermoCamera3.Items.Clear();
                                        cbVRTermoCamera3.Items.AddRange(SPcmbox);
                                        cbVRTermoCamera3.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != "")
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
                                    if (SensParam != "")
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbVRTermoCamera3.Items.Clear();
                                        cbVRTermoCamera3.Items.AddRange(SPcmbox);
                                        cbVRTermoCamera3.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != "")
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
                            si = sensors.FindSensorGroup(4);
                            if (si >= 0)
                            {
                                SelectModel = new String(sensors.sensorList[si].PressureModel);
                                // Определяем сколько диапазонов у датчиков                               
                                NumOfRange = Convert.ToInt16(SensorsDB.GetDataSensors(SelectModel, "NumOfRange"));

                                if (NumOfRange == 1)
                                {
                                    cbVRDiapazon4.SelectedIndex = 0;
                                    cbVRDiapazon4.Enabled = false;
                                    // Занесение данных из ДБ в combobox
                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerTempPoint1");  // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != "")
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbVRTermoCamera4.Items.Clear();
                                        cbVRTermoCamera4.Items.AddRange(SPcmbox);
                                        cbVRTermoCamera4.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != "")
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
                                    if (SensParam != "")
                                    {
                                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        cbVRTermoCamera4.Items.Clear();
                                        cbVRTermoCamera4.Items.AddRange(SPcmbox);
                                        cbVRTermoCamera4.SelectedIndex = 0;
                                    }

                                    SensParam = SensorsDB.GetDataSensors(SelectModel, "VerPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                    if (SensParam != "")
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
            ResultCH.LoadFromFile();

            //***************** создаем файлы результатов калибровки ***************************************
            ResultCI = new CResultCI(MaxChannalCount, FN);//результаты калибровки датчиков
            ResultCI.LoadFromFile();

            //***************** создаем файлы результатов верификации ***********************************
            ResultVR = new CResultVR(MaxChannalCount, FN);
            ResultVR.LoadFromFile();

            //**********************************************************************************************

        }

        // Отработка нажатия на гл. форме МЕНСОР - ЗАДАЧА
        private void bMensorControl_Click(object sender, EventArgs e)
        {
            if (Mensor._serialPort_M.IsOpen)
            {
                bMensorControl.BackColor = Color.LightGreen;
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
                bMensorVent.BackColor = Color.LightGreen;
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
                else if ((ind >= 3) && (ind <= 5)) // активный канал B
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
            if (!SensorBusy)
            {
                int i;
                for (i = 0; i < MaxChannalCount; i++)
                {
                    if (CheckChannalEnable(i)) //Есть выбранные каналы?
                        break;
                }
                if (i >= MaxChannalCount)
                {
                    Program.txtlog.WriteLineLog("Не выбраны каналы для характеризации датчиков. Операция прервана.", 0);
                    return;
                }

                //                if (TemperatureReady && PressureReady)
                //                {
                try
                {
                        btnCHStart.Text = "Выполняется процесс характеризации ... Отменить?";
                        UpdateItemState(2);
                        ReadSensorParametrs();
                    }
                    finally
                    {
                        btnCHStart.Text = "Старт характеризации";
                        UpdateItemState(0);
                    }
                /*                }
                                else
                                {
                                    Program.txtlog.WriteLineLog("Не заданны параметры для характеризации.", 1);
                                }*/
            }
            else
            {
                if (MessageBox.Show("Отменить текущую операцию?", "Подтверждение команды", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ProcessStop = true;
                    Program.txtlog.WriteLineLog("Операция прекращена пользователем", 0);
                }
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex == sel)&&(dataGridView1.RowCount>0))//выбор датчиков
            {
                bool CurentSet = Convert.ToBoolean(dataGridView1.Rows[0].Cells[sel].Value);
                for (int i = 0; i <= (dataGridView1.RowCount - 1); i++)
                {
                    dataGridView1.Rows[i].Cells[sel].Value = !CurentSet;
//                    dataGridView1.Rows[i].Cells[3].Style.BackColor = Color.White;
                }
            }
        }

        private void cbChannalCharakterizator_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!SensorBusy)
            {
                if (cbChannalCharakterizator.Text == "") return;

                string str = cbChannalCharakterizator.Text.Remove(0, 6);
                int i = Convert.ToInt32(str)-1;

                UpDateCharakterizatorGrid(i);
                UpdateCurrentGrid(i);
                UpdateUpStatus(i);
            }
        }


        private void UpdateUpStatus(int i)
        {
            if (i >= 0)
            {
                if (sensors.SelectSensor(i))
                {
                    UpStModel.Text = new String(sensors.sensor.PressureModel); 
                    UpStSerial.Text = sensors.sensor.uni.ToString();
                    UpStCh.Text = (sensors.sensor.Channal + 1).ToString();

                    label_UpStVoltage.Text = sensors.sensor.OutVoltage.ToString();
                    label_UpStResistance.Text = sensors.sensor.Resistance.ToString();
                    label_UpStPressure.Text = sensors.sensor.Pressure.ToString();

                    //UpdateCHnumber(sensors.sensorList[i].Channal + 1);
                }
                else
                {
                    UpStModel.Text = "";
                    UpStSerial.Text = "";
                    UpStCh.Text = "";
                }
            }
        }


/*
        public void UpdateCHnumber(int i)
        {           
                tbNumCH.Text = i.ToString();
           
        }
*/




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
            if (strValue == "")
            {
                MessageBox.Show("Введите значение температуры", "Не задана температура в термокамере");
                return;
            }

            if (ThermalCamera.Connected)
            {
                numTermoCameraPoint.Text = strValue;
                Program.txtlog.WriteLineLog("Температура задана. Ожидаем завершение стабилизации показаний.", 0);
                TemperatureReady = true;
                //btnReadCAP.BackColor = Color.LightGreen;
                MessageBox.Show("температура установлена.", "Успешное завершение операции");
            }
            else
            {
                Program.txtlog.WriteLineLog("CH: Нет cвязи c термокамерой.", 1);
              //  if (MessageBox.Show("Хотите установить температуру в ручную?", "Нет соединения с термокамерой", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    numTermoCameraPoint.Text = strValue;
                    TemperatureReady = true;
                    //btnReadCAP.BackColor = Color.LightGreen;
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
            if (strValue == "")
            {
                MessageBox.Show("Введите значение давления в кПа", "Не установлено давление в задатчике");
                return;
            }

            if (Mensor.Connected)
            {
                double Point=0;
                double shift=0;
                try
                {
                    btnCHPressureSet1.Enabled = false;
                    btnCHPressureSet2.Enabled = false;
                    btnCHPressureSet3.Enabled = false;
                    btnCHPressureSet4.Enabled = false;


                    Point = double.Parse(strValue.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture); 
                    //Point = Convert.ToDouble(strValue);// получаем заданное значение уставки
                    numMensorPoint.Text = strValue;
                    bMensorSet.PerformClick();      //выставляем давление
                    bMensorControl.PerformClick();  //запускаем задачу
                    int i = 0;
                    do//ожидаем установления давления
                    {
                        Application.DoEvents();
                        i++;
                        Thread.Sleep(1000);
                        shift = Math.Abs(Mensor._press - Point);
                    } while ((shift > SKO_PRESSURE) && (i < MENSOR_PRESSUER_WAIT));
                    if (i >= MENSOR_PRESSUER_WAIT)
                    {//давление не установлено
                        Program.txtlog.WriteLineLog("CH: Истекло время установки давления в датчиках", 1);
                        //MessageBox.Show("Повторите установку давления.", "Истекло время установки давления в датчиках");
                    }
                    else
                    {//давление установлено
                        Thread.Sleep(SENSOR_PRESSUER_WAIT * 1000);//ожидаем стабилизации
                        PressureReady = true;
//                        btnCHStart.BackColor = Color.LightGreen;
//                        btnCalculateCoeff.BackColor = Color.IndianRed;
                        Program.txtlog.WriteLineLog("CH: Давление в датчиках установлено.", 0);
                        //MessageBox.Show("Давление установлено.", "Успешное завершение операции");
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
//                    btnCHStart.BackColor = Color.LightGreen;
//                    btnCalculateCoeff.BackColor = Color.IndianRed;
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
            if (!SensorBusy)
            {
                if (!Multimetr.Connected)
                {
                    Program.txtlog.WriteLineLog("CI: Нет подключения к мультиметру, чтение ЦАП не выполнено!", 0);
                    return;
                }

                if ((Math.Abs((double)numTermoCameraPoint.Value - DEFAULT_TEMPERATURE) <= 0.1) || (MessageBox.Show(string.Format("CL: Температура в камере: {0}. Выполнить чтение ЦАП?", numTermoCameraPoint.Text), "Подтверждение операции", MessageBoxButtons.YesNo) == DialogResult.Yes))
                {
                    try
                    {
                        btnReadCAP.Text = "Выполняется процесс чтения ЦАП... Отменить?";
                        UpdateItemState(3);
                        ReadSensorCurrent();

                    }
                    finally
                    {
                        btnReadCAP.Text = "Чтение параметров ЦАП";
                        UpdateItemState(0);
                    }
                }
                else
                {
                    Program.txtlog.WriteLineLog("Не заданны температура 23град для чтения ЦАП.", 1);
                }
            }
            else
            {
                if (MessageBox.Show("Отменить текущую операцию?", "Подтверждение команды", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ProcessStop = true;
                    Program.txtlog.WriteLineLog("Операция прекращена пользователем", 0);
                }
            }
        }

        //Запуск калибровки токов ЦАП
        private void btnCalibrateCurrent_Click(object sender, EventArgs e)
        {
            if (!SensorBusy)
            {
                if (!Multimetr.Connected)
                {
                    Program.txtlog.WriteLineLog("CL: Нет подключения к мультиметру, калибровка не выполнена!", 0);
                    return;
                }
                if (numTermoCameraPoint.Value != 23)
                {
                    if (MessageBox.Show(string.Format("CL: Температура в камере: {0}. Продолжить калибровку?", numTermoCameraPoint.Text), "Подтверждение операции", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        Program.txtlog.WriteLineLog("CL: Операция прервана. Установите температуру в камере 23 градуса и продолжите калибровку!", 1);
                        return;
                    }
                }
                try
                {
                    btnCalibrateCurrent.Text = "Выполняется калибровка... Отменить?";
                    UpdateItemState(4);
                    SensorCalibration();
                }
                finally
                {
                    btnCalibrateCurrent.Text = "Калибровка тока    (4 и 20 мА)";
                    UpdateItemState(0);
                }
            }
            else
            {
                if (MessageBox.Show("Отменить текущую операцию?", "Подтверждение команды", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ProcessStop = true;
                    Program.txtlog.WriteLineLog("Операция прекращена пользователем", 0);
                }
            }
        }

        private void cbChannalVerification_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!SensorBusy)
            {
                if (cbChannalVerification.Text == "") return;

                string str = cbChannalVerification.Text.Remove(0, 6);
                int ii = Convert.ToInt32(str) - 1;

                UpDateVerificationGrid(ii);
                UpdateUpStatus(ii);
            }

        }



        // Меню НАСТРОЙКИ
        // Настройки программы
        private void MenuItemMainSettings_Click(object sender, EventArgs e)
        {
            SettingsSelIndex = 0;
            FormSettings Settings = new FormSettings();
            if (Settings.ShowDialog() == DialogResult.OK)
            {
                SetSettings();
                /*MAIN_TIMER = Properties.Settings.Default.set_MainTimer;
                MainTimer.Stop();
                MainTimer.Enabled = false;
                MainTimer.Interval = MAIN_TIMER;
                MainTimer.Enabled = true;
                MainTimer.Start();
                Properties.Settings.Default.Save();  // Сохраняем переменные.*/
            }
        }
        // Настройки Мультиметра
        private void параметрыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsSelIndex = 2;
            FormSettings Settings = new FormSettings();
            if (Settings.ShowDialog() == DialogResult.OK)
            {
                SetSettings();
               /* Multimetr.WAIT_READY = Properties.Settings.Default.set_MultimDataReady;    //время ожидания стабилизации тока, мсек
                Multimetr.WAIT_TIMEOUT = Properties.Settings.Default.set_MultimReadTimeout;  //таймаут ожидания ответа от мультиметра, мсек
                Multimetr.SAMPLE_COUNT = Properties.Settings.Default.set_MultimReadCount;      //количество опросов мультиметра, раз
                Multimetr.READ_PERIOD = Properties.Settings.Default.set_MultimReadPeriod;   //период опроса мультиметра, мсек
                Properties.Settings.Default.Save();  // Сохраняем переменные.*/
            }
        }
        // Настройки Задатчика давления
        private void параметрыToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SettingsSelIndex = 3;
            FormSettings Settings = new FormSettings();
            if (Settings.ShowDialog() == DialogResult.OK)
            {
                SetSettings();
                /*Mensor.READ_PERIOD = Properties.Settings.Default.set_MensorReadPeriod;
                Mensor.READ_PAUSE = Properties.Settings.Default.set_MensorReadPause;
                Properties.Settings.Default.Save();  // Сохраняем переменные.*/
            }
        }

        // Настройки Коммутатора
        private void параметрыToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SettingsSelIndex = 1;
            FormSettings Settings = new FormSettings();
            if (Settings.ShowDialog() == DialogResult.OK)
            {
                SetSettings();
                /*Commutator.MAX_SETCH = Properties.Settings.Default.set_CommMaxSetCH;     // максимально разрешенное коичество подключаемых к изм. линии датчиков
                Commutator.READ_PERIOD = Properties.Settings.Default.set_CommReadPeriod; // Время опроса и обновление информации, мс
                Commutator.READ_PAUSE = Properties.Settings.Default.set_CommReadPause;    //время выдержки после переключения коммутатора (переходные процессы), мс
                //Commutator.WaitTime = Properties.Settings.Default.set_CommReadPause;    //время выдержки после переключения коммутатора (переходные процессы), мс
                Properties.Settings.Default.Save();  // Сохраняем переменные.
                Commutator.CommStartTimer();*/
            }
        }
        // Настройки Термокамеры
        private void параметрыToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            SettingsSelIndex = 4;
            FormSettings Settings = new FormSettings();
            if(Settings.ShowDialog()== DialogResult.OK)
            {
                SetSettings();
                //Properties.Settings.Default.Save();  // Сохраняем переменные.

            }
        }
        // Настройки Датчиков
        private void параметрыToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            SettingsSelIndex = 5;
            FormSettings Settings = new FormSettings();
            if (Settings.ShowDialog() == DialogResult.OK)
            {
                SetSettings();
                /*sensors.WAIT_TIMEOUT = Properties.Settings.Default.set_SensReadPause;
                sensors.WRITE_COUNT = Properties.Settings.Default.set_SensReadCount;
                sensors.WRITE_PERIOD = Properties.Settings.Default.set_SensReadPause;
                SKO_CALIBRATION_CURRENT = 0.003; // Properties.Settings.Default.set_SensSKOCalibrCurrent;
                Properties.Settings.Default.Save();  // Сохраняем переменные.*/
            }
        }


        private void SetSettings()
        {
            try
            {
                // Общие настройки программы
                MAIN_TIMER = Properties.Settings.Default.set_MainTimer;                     // Общий интервал опроса

                MainTimer.Stop();
                MainTimer.Enabled = false;
                MainTimer.Interval = MAIN_TIMER;

                MAX_ERROR_COUNT = Properties.Settings.Default.set_MaxErrorCount;            //Количество ошибок чтения данных с устройств перед отключением
                MIN_SENSOR_CURRENT = Properties.Settings.Default.set_MinSensorCurrent;      //минимльный ток датчика для обнаружения, мА
                MAX_COUNT_CAP_READ = Properties.Settings.Default.set_MaxCountCAPRead;       //максимальное количество циклов чтения тока ЦАП
                SKO_CURRENT = Properties.Settings.Default.set_SKOCurrent;                   //допуск по току ЦАП датчика до калибровки, мА
                SKO_CALIBRATION_CURRENT = Properties.Settings.Default.set_SKOCalibrationCurrent; //допуск по току ЦАП после калибровки, мА            
                Multimetr.REZISTOR = Properties.Settings.Default.set_Rezistor;              // сопротивление резистора

                Multimetr.WAIT_READY = Properties.Settings.Default.set_MultimDataReady;     //время ожидания стабилизации тока, мсек
                Multimetr.WAIT_TIMEOUT = Properties.Settings.Default.set_MultimReadTimeout; //таймаут ожидания ответа от мультиметра, мсек
                Multimetr.SAMPLE_COUNT = Properties.Settings.Default.set_MultimReadCount;   //количество отчетов измерения мультиметром, раз
                Multimetr.READ_PERIOD = Properties.Settings.Default.set_MultimReadPeriod;   //период опроса мультиметра, мсек

                Commutator.MAX_SETCH = Properties.Settings.Default.set_CommMaxSetCH;        // максимально разрешенное коичество подключаемых к изм. линии датчиков 15
                Commutator.READ_PERIOD = Properties.Settings.Default.set_CommReadPeriod;    // Время опроса и обновление информации, мс
                Commutator.READ_PAUSE = Properties.Settings.Default.set_CommReadPause;      // время выдержки после переключения коммутатора (переходные процессы), мс
                MaxChannalCount = Properties.Settings.Default.set_CommReadCH;               // максимальное количество каналов коммутаторы
                MaxLevelCount = Properties.Settings.Default.set_CommMaxLevelCount;          // максимальное количество уровней датчиков (идентичных групп)

                Mensor.READ_PERIOD = Properties.Settings.Default.set_MensorReadPeriod;      // Время опроса состояния менсора при работе с формой
                Mensor.READ_PAUSE = Properties.Settings.Default.set_MensorReadPause;        // задержка между приемом и передачей команд по COM порту, мс     
                MAX_COUNT_POINT = Properties.Settings.Default.set_MensorMaxCountPoint / MAIN_TIMER + 1;        //ожидание стабилизации давления в датчике, в циклах таймера
                SKO_PRESSURE = Properties.Settings.Default.set_MensorSKOPressure;           //(СКО) допуск по давлению, кПа

                sensors.WAIT_TIMEOUT = Properties.Settings.Default.set_SensWaitTimeout;     //таймаут ожидания ответа от датчика
                sensors.WRITE_COUNT = Properties.Settings.Default.set_SensReadCount;        //число попыток записи команд в датчик
                sensors.WRITE_PERIOD = Properties.Settings.Default.set_SensReadPause;       //период выдачи команд

                MainTimer.Enabled = true;
                MainTimer.Start();
            }
            catch
            {
                Program.txtlog.WriteLineLog("Не удалось задать настройки программы", 1);
            }
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

        //Установка температуры в термокамере для верификации
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
            if (strValue == "")
            {
                MessageBox.Show("Введите значение температуры", "Не задана температура в термокамере");
                return;
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

        //установка давления (верификация)
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

            if (strValue == "")
            {
                MessageBox.Show("Введите значение давления в кПа", "Не задано давление в задатчике");
                return;
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
                    Point = double.Parse(strValue.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                    //Point = Convert.ToDouble(strValue);// получаем заданное значение уставки
                    numMensorPoint.Text = strValue;
                    bMensorSet.PerformClick();      //выставляем давление
                    bMensorControl.PerformClick();  //запускаем задачу
                    int i = 0;
                    do//ожидаем установления давления
                    {
                        Application.DoEvents();
                        i++;
                        Thread.Sleep(1000);
                        double realpoint = Mensor._press;
                        shift = Math.Abs(realpoint - Point);
                    } while ((shift > SKO_PRESSURE) && (i < MENSOR_PRESSUER_WAIT));
                    if (i >= MENSOR_PRESSUER_WAIT)
                    {//давление не установлено
                        Program.txtlog.WriteLineLog("VR: Истекло время установки давления в датчиках", 1);
//                        MessageBox.Show("Повторите установку давления.", "Истекло время установки давления в датчиках");
                    }
                    else
                    {//давление установлено
                        Thread.Sleep(SENSOR_PRESSUER_WAIT * 1000);//ожидаем стабилизации
                        PressureReady = true;
//                        btnVRParamRead.BackColor = Color.LightGreen;
//                        MessageBox.Show("Давление установлено.", "Успешное завершение операции");
                        Program.txtlog.WriteLineLog("VR: Давление установлено", 0);
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
                Program.txtlog.WriteLineLog("VR: Нет cвязи c Менсором.", 1);
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
            if (!SensorBusy)
            {
                int i;
                for (i = 0; i < MaxChannalCount; i++)
                {
                    if (CheckChannalEnable(i)) //Есть выбранные каналы?
                        break;
                }
                if (i >= MaxChannalCount)
                {
                    Program.txtlog.WriteLineLog("Не выбраны каналы для верификации датчиков. Операция прервана.", 0);
                    return;
                }

                //                if (TemperatureReady && PressureReady)
                //                {
                try
                {
                        btnVRParamRead.Text = "Выполняется процесс верификации ... Отменить?";
                        UpdateItemState(6);
                        ReadSensorPressure();
                    }
                    finally
                    {
                        btnVRParamRead.Text = "Старт верификации";
                        UpdateItemState(0);
                    }
                /*                }
                                else
                                {
                                    Program.txtlog.WriteLineLog("Не установлены параметры для верификации.", 1);
                                }*/
            }
            else
            {
                if (MessageBox.Show("Отменить верификацию датчиков?", "Подтверждение команды", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ProcessStop = true;
                    Program.txtlog.WriteLineLog("VR:Операция прекращена пользователем", 0);
                }
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


        // Останавливаем таймер при работе с комбобосом Выбор преобразователя
        private void cbMensorTypeR_DropDown(object sender, EventArgs e)
        {
            MainTimer.Stop();
            MainTimer.Enabled = false;

        }

        private void bMensorMeas_Click(object sender, EventArgs e)
        {
            if (Mensor._serialPort_M.IsOpen)
            {
                bMensorMeas.BackColor = Color.LightGreen;
                Mensor.SetMode(0);
            }
            else
            {
                Program.txtlog.WriteLineLog("Нет Связи. Задатчик давления не подключен", 1);
            }
        }

        private void dataGridView2_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            ToolStripMenuDeleteResult_Click(sender,null);
        }

        private void ToolStripMenuDeleteResult_Click(object sender, EventArgs e)
        {
            if (ResultCH != null)
            {
                if (cbChannalCharakterizator.Text == "") return;

                string str = cbChannalCharakterizator.Text.Remove(0, 6);
                int ii = Convert.ToInt32(str)-1;

                DataGridViewSelectedRowCollection s = dataGridView2.SelectedRows;
                if (s.Count <= 0) return;

                DialogResult result = MessageBox.Show(
                        "Выбранные записи будут удалены из таблицы и архива данных характеризации. Продолжить?",
                        "Подтверждение операции",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    dataGridView2.Sort(dataGridView2.Columns[0], ListSortDirection.Ascending);
                    for (int i = 0; i < s.Count; i++)
                    {
                        ResultCH.DeletePoint(ii, s[i].Index);
                    }
                    UpDateCharakterizatorGrid(ii);
                    ResultCH.SaveToArhiv(ii);
                }
            }
        }

        private void tsMenuVerificationDetele_Click(object sender, EventArgs e)
        {
            if (ResultVR != null)
            {
                if (cbChannalVerification.Text == "") return;
                string str = cbChannalVerification.Text.Remove(0, 6);
                int ii = Convert.ToInt32(str)-1;

                DataGridViewSelectedRowCollection s = dataGridView3.SelectedRows;
                if (s.Count <= 0) return;

                DialogResult result = MessageBox.Show(
                        "Выбранные записи будут удалены из таблицы и архива данных верификации. Продолжить?",
                        "Подтверждение операции",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    dataGridView3.Sort(dataGridView3.Columns[0], ListSortDirection.Ascending);
                    for (int i = 0; i < s.Count; i++)
                    {
                        ResultVR.DeletePoint(ii, s[i].Index);
                    }
                    UpDateVerificationGrid(ii);
                    ResultVR.SaveToArhiv(ii);
                }
            }

        }

        private void dataGridView3_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            tsMenuVerificationDetele_Click(sender, null);
        }

        private void cbSensorPeriodRead_CheckedChanged(object sender, EventArgs e)
        {
            if (SensorBusy)
            {
                cbSensorPeriodRead.Checked = false;
                MessageBox.Show("Отмените текущие операции с датчиками.", "Процесс занят...", MessageBoxButtons.OK);
            }
        }

        private void UpdateItemState(int state)
        {
            switch (state)
            {
                case 0://исходное состояние
                    btnSensorSeach.Enabled = true;
                    btnSensorSeach.BackColor = Color.LightGreen;

                    btnCHStart.BackColor = Color.LightGreen;
                    btnCHStart.Enabled = true;
                    btnReadCAP.BackColor = Color.LightGreen;
                    btnReadCAP.Enabled = true;
                    btnCalibrateCurrent.BackColor = Color.LightGreen;
                    btnCalibrateCurrent.Enabled = true;
                    btnCalculateCoeff.BackColor = Color.LightGreen;
                    btnCalculateCoeff.Enabled = true;
                    cbChannalCharakterizator.Enabled = true;

                    btnVRParamRead.BackColor = Color.LightGreen;
                    btnVRParamRead.Enabled = true;
                    cbChannalVerification.Enabled = true;

                    btnVR_VPI_NPI.BackColor = Color.LightGreen;
                    btnVR_VPI_NPI.Enabled = true;

                    btnVR_SetZero.BackColor = Color.LightGreen;
                    btnVR_SetZero.Enabled = true;

                    SensorBusy = false;
                    ProcessStop = true;
                    break;
                case 1://поиск датчиков
                    btnSensorSeach.Enabled = true;
                    btnSensorSeach.BackColor = Color.LightGreen;

                    btnCHStart.BackColor = Color.IndianRed;
                    btnCHStart.Enabled = false;
                    btnReadCAP.BackColor = Color.IndianRed;
                    btnReadCAP.Enabled = false;
                    btnCalibrateCurrent.BackColor = Color.IndianRed;
                    btnCalibrateCurrent.Enabled = false;
                    btnCalculateCoeff.BackColor = Color.IndianRed;
                    btnCalculateCoeff.Enabled = false;
                    cbChannalCharakterizator.Enabled = false;

                    btnVRParamRead.BackColor = Color.IndianRed;
                    btnVRParamRead.Enabled = false;
                    cbChannalVerification.Enabled = false;

                    btnVR_VPI_NPI.BackColor = Color.IndianRed;
                    btnVR_VPI_NPI.Enabled = false;

                    btnVR_SetZero.BackColor = Color.IndianRed;
                    btnVR_SetZero.Enabled = false;

                    SensorBusy = true;
                    ProcessStop = false;
                    break;
                case 2://характеризация
                    btnSensorSeach.Enabled = false;
                    btnSensorSeach.BackColor = Color.IndianRed;

                    btnCHStart.BackColor = Color.LightGreen;
                    btnCHStart.Enabled = true;
                    btnReadCAP.BackColor = Color.IndianRed;
                    btnReadCAP.Enabled = false;
                    btnCalibrateCurrent.BackColor = Color.IndianRed;
                    btnCalibrateCurrent.Enabled = false;
                    btnCalculateCoeff.BackColor = Color.IndianRed;
                    btnCalculateCoeff.Enabled = false;
                    cbChannalCharakterizator.Enabled = false;

                    btnVRParamRead.BackColor = Color.IndianRed;
                    btnVRParamRead.Enabled = false;
                    cbChannalVerification.Enabled = false;

                    btnVR_VPI_NPI.BackColor = Color.IndianRed;
                    btnVR_VPI_NPI.Enabled = false;

                    btnVR_SetZero.BackColor = Color.IndianRed;
                    btnVR_SetZero.Enabled = false;

                    SensorBusy = true;
                    ProcessStop = false;
                    break;
                case 3://чтение ЦАП
                    btnSensorSeach.Enabled = false;
                    btnSensorSeach.BackColor = Color.IndianRed;

                    btnCHStart.BackColor = Color.IndianRed;
                    btnCHStart.Enabled = false;
                    btnReadCAP.BackColor = Color.LightGreen;
                    btnReadCAP.Enabled = true;
                    btnCalibrateCurrent.BackColor = Color.IndianRed;
                    btnCalibrateCurrent.Enabled = false;
                    btnCalculateCoeff.BackColor = Color.IndianRed;
                    btnCalculateCoeff.Enabled = false;
                    cbChannalCharakterizator.Enabled = false;

                    btnVRParamRead.BackColor = Color.IndianRed;
                    btnVRParamRead.Enabled = false;
                    cbChannalVerification.Enabled = false;

                    btnVR_VPI_NPI.BackColor = Color.IndianRed;
                    btnVR_VPI_NPI.Enabled = false;

                    btnVR_SetZero.BackColor = Color.IndianRed;
                    btnVR_SetZero.Enabled = false;

                    SensorBusy = true;
                    ProcessStop = false;
                    break;
                case 4://калибровка
                    btnSensorSeach.Enabled = false;
                    btnSensorSeach.BackColor = Color.IndianRed;

                    btnCHStart.BackColor = Color.IndianRed;
                    btnCHStart.Enabled = false;
                    btnReadCAP.BackColor = Color.IndianRed;
                    btnReadCAP.Enabled = false;
                    btnCalibrateCurrent.BackColor = Color.LightGreen;
                    btnCalibrateCurrent.Enabled = true;
                    btnCalculateCoeff.BackColor = Color.IndianRed;
                    btnCalculateCoeff.Enabled = false;
                    cbChannalCharakterizator.Enabled = false;

                    btnVRParamRead.BackColor = Color.IndianRed;
                    btnVRParamRead.Enabled = false;
                    cbChannalVerification.Enabled = false;

                    btnVR_VPI_NPI.BackColor = Color.IndianRed;
                    btnVR_VPI_NPI.Enabled = false;

                    btnVR_SetZero.BackColor = Color.IndianRed;
                    btnVR_SetZero.Enabled = false;

                    SensorBusy = true;
                    ProcessStop = false;
                    break;
                case 5://расчет коэффициентов
                    btnSensorSeach.Enabled = false;
                    btnSensorSeach.BackColor = Color.IndianRed;

                    btnCHStart.BackColor = Color.IndianRed;
                    btnCHStart.Enabled = false;
                    btnReadCAP.BackColor = Color.IndianRed;
                    btnReadCAP.Enabled = false;
                    btnCalibrateCurrent.BackColor = Color.IndianRed;
                    btnCalibrateCurrent.Enabled = false;
                    btnCalculateCoeff.BackColor = Color.LightGreen;
                    btnCalculateCoeff.Enabled = true;
                    cbChannalCharakterizator.Enabled = false;

                    btnVRParamRead.BackColor = Color.IndianRed;
                    btnVRParamRead.Enabled = false;
                    cbChannalVerification.Enabled = false;

                    btnVR_VPI_NPI.BackColor = Color.IndianRed;
                    btnVR_VPI_NPI.Enabled = false;

                    btnVR_SetZero.BackColor = Color.IndianRed;
                    btnVR_SetZero.Enabled = false;

                    SensorBusy = true;
                    ProcessStop = false;
                    break;
                case 6://верификация
                    btnSensorSeach.Enabled = false;
                    btnSensorSeach.BackColor = Color.IndianRed;

                    btnCHStart.BackColor = Color.IndianRed;
                    btnCHStart.Enabled = false;
                    btnReadCAP.BackColor = Color.IndianRed;
                    btnReadCAP.Enabled = false;
                    btnCalibrateCurrent.BackColor = Color.IndianRed;
                    btnCalibrateCurrent.Enabled = false;
                    btnCalculateCoeff.BackColor = Color.IndianRed;
                    btnCalculateCoeff.Enabled = false;
                    cbChannalCharakterizator.Enabled = false;

                    btnVRParamRead.BackColor = Color.LightGreen;
                    btnVRParamRead.Enabled = true;
                    cbChannalVerification.Enabled = false;

                    btnVR_VPI_NPI.BackColor = Color.IndianRed;
                    btnVR_VPI_NPI.Enabled = false;

                    btnVR_SetZero.BackColor = Color.IndianRed;
                    btnVR_SetZero.Enabled = false;

                    SensorBusy = true;
                    ProcessStop = false;
                    break;
                case 7://ВПИ НПИ
                    btnSensorSeach.Enabled = false;
                    btnSensorSeach.BackColor = Color.IndianRed;

                    btnCHStart.BackColor = Color.IndianRed;
                    btnCHStart.Enabled = false;
                    btnReadCAP.BackColor = Color.IndianRed;
                    btnReadCAP.Enabled = false;
                    btnCalibrateCurrent.BackColor = Color.IndianRed;
                    btnCalibrateCurrent.Enabled = false;
                    btnCalculateCoeff.BackColor = Color.IndianRed;
                    btnCalculateCoeff.Enabled = false;
                    cbChannalCharakterizator.Enabled = false;

                    btnVRParamRead.BackColor = Color.IndianRed;
                    btnVRParamRead.Enabled = false;
                    cbChannalVerification.Enabled = false;

                    btnVR_VPI_NPI.BackColor = Color.LightGreen;
                    btnVR_VPI_NPI.Enabled = true;

                    btnVR_SetZero.BackColor = Color.IndianRed;
                    btnVR_SetZero.Enabled = false;

                    SensorBusy = true;
                    ProcessStop = false;
                    break;
                case 8://Установка нуля
                    btnSensorSeach.Enabled = false;
                    btnSensorSeach.BackColor = Color.IndianRed;

                    btnCHStart.BackColor = Color.IndianRed;
                    btnCHStart.Enabled = false;
                    btnReadCAP.BackColor = Color.IndianRed;
                    btnReadCAP.Enabled = false;
                    btnCalibrateCurrent.BackColor = Color.IndianRed;
                    btnCalibrateCurrent.Enabled = false;
                    btnCalculateCoeff.BackColor = Color.IndianRed;
                    btnCalculateCoeff.Enabled = false;
                    cbChannalCharakterizator.Enabled = false;

                    btnVRParamRead.BackColor = Color.IndianRed;
                    btnVRParamRead.Enabled = false;
                    cbChannalVerification.Enabled = false;

                    btnVR_VPI_NPI.BackColor = Color.IndianRed;
                    btnVR_VPI_NPI.Enabled = false;

                    btnVR_SetZero.BackColor = Color.LightGreen;
                    btnVR_SetZero.Enabled = true;

                    SensorBusy = true;
                    ProcessStop = false;
                    break;
                case 9://расчет коэффициентов
                    btnSensorSeach.Enabled = false;
                    btnSensorSeach.BackColor = Color.IndianRed;

                    btnCHStart.BackColor = Color.IndianRed;
                    btnCHStart.Enabled = false;
                    btnReadCAP.BackColor = Color.IndianRed;
                    btnReadCAP.Enabled = false;
                    btnCalibrateCurrent.BackColor = Color.IndianRed;
                    btnCalibrateCurrent.Enabled = false;
                    btnCalculateCoeff.BackColor = Color.LightGreen;
                    btnCalculateCoeff.Enabled =true;
                    cbChannalCharakterizator.Enabled = false;

                    btnVRParamRead.BackColor = Color.IndianRed;
                    btnVRParamRead.Enabled = false;
                    cbChannalVerification.Enabled = false;

                    btnVR_VPI_NPI.BackColor = Color.IndianRed;
                    btnVR_VPI_NPI.Enabled = false;

                    btnVR_SetZero.BackColor = Color.IndianRed;
                    btnVR_SetZero.Enabled = false;

                    SensorBusy = true;
                    ProcessStop = false;
                    break;
            }
        }

        private void btnCalculateCoeff_Click(object sender, EventArgs e)
        {


            if (!SensorBusy)
            {
                int i;
                for (i = 0; i < MaxChannalCount; i++)
                {
                    if (CheckChannalEnable(i)) //Есть выбранные каналы?
                        break;
                }
                if (i >= MaxChannalCount)
                {
                    Program.txtlog.WriteLineLog("Не выбраны датчики для расчета коэффициентов. Операция прервана.", 0);
                    return;
                }

                //                if (TemperatureReady && PressureReady)
                //                {
                try
                {
                    Program.txtlog.WriteLineLog("CH: Тест 1... ", 2);
                    btnCalculateCoeff.Text = "Выполняется расчет и запись коэффициентов ... Отменить?";
                    Program.txtlog.WriteLineLog("CH: Тест 2... ", 2);
                    UpdateItemState(9);
                    Program.txtlog.WriteLineLog("CH: Тест 3... ", 2);
                    СaclSensorCoeff();
                    Program.txtlog.WriteLineLog("CH: Тест 6... ", 2);
                }
                catch
                {
                    Program.txtlog.WriteLineLog("CH:Расчет коэффициентов не выполнен.", 1);
                }
                finally
                {
                    btnCalculateCoeff.Text = "Расчет коэффициентов";
                    UpdateItemState(0);
                }
                /*                }
                                else
                                {
                                    Program.txtlog.WriteLineLog("Не заданны параметры для характеризации.", 1);
                                }*/
            }
            else
            {
                if (MessageBox.Show("Отменить текущую операцию?", "Подтверждение команды", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ProcessStop = true;
                    Program.txtlog.WriteLineLog("Операция прекращена пользователем", 0);
                }
            }

        }

        private void btn_VR_VPI_NPI_Click(object sender, EventArgs e)
        {
            if (!SensorBusy)
            {
                int i;
                for (i = 0; i < MaxChannalCount; i++)
                {
                    if (CheckChannalEnable(i)) //Есть выбранные каналы?
                        break;
                }
                if (i >= MaxChannalCount)
                {
                    Program.txtlog.WriteLineLog("Не выбраны каналы для записи ВПИ НПИ датчиков. Операция прервана.", 0);
                    return;
                }

                try
                {
                    btnVR_VPI_NPI.Text = "Отменить";
                    UpdateItemState(7);
                    WriteSensorVPI_NPI();
                }
                finally
                {
                    btnVR_VPI_NPI.Text = "Задать";
                    UpdateItemState(0);
                }
            }
            else
            {
                if (MessageBox.Show("Отменить запись ВПИ НПИ датчиков?", "Подтверждение команды", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ProcessStop = true;
                    Program.txtlog.WriteLineLog("VR:Операция прекращена пользователем", 0);
                }
            }

        }

        private void btnVR_SetZero_Click(object sender, EventArgs e)
        {
            if (!SensorBusy)
            {
                int i;
                for (i = 0; i < MaxChannalCount; i++)
                {
                    if (CheckChannalEnable(i)) //Есть выбранные каналы?
                        break;
                }
                if (i >= MaxChannalCount)
                {
                    Program.txtlog.WriteLineLog("Не выбраны каналы для обнуления датчиков. Операция прервана.", 0);
                    return;
                }

                try
                {
                    btnVR_SetZero.Text = "Отменить";
                    UpdateItemState(8);
                    SetZero();
                }
                finally
                {
                    btnVR_SetZero.Text = "Обнулить";
                    UpdateItemState(0);
                }
            }
            else
            {
                if (MessageBox.Show("Отменить зобнуление датчиков?", "Подтверждение команды", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ProcessStop = true;
                    Program.txtlog.WriteLineLog("VR:Операция прекращена пользователем", 0);
                }
            }


        }

        private void tsmCurrentDelete_Click(object sender, EventArgs e)
        {
            if (ResultCI != null)
            {
                if (cbChannalCharakterizator.Text == "") return;

                string str = cbChannalCharakterizator.Text.Remove(0, 6);
                int ii = Convert.ToInt32(str) - 1;

                DataGridViewSelectedRowCollection s = dataGridView4.SelectedRows;
                if (s.Count <= 0) return;

                DialogResult result = MessageBox.Show(
                        "Выбранные записи будут удалены из таблицы и архива данных ЦАП. Продолжить?",
                        "Подтверждение операции",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    dataGridView2.Sort(dataGridView4.Columns[0], ListSortDirection.Ascending);
                    for (int i = 0; i < s.Count; i++)
                    {
                        ResultCI.DeletePoint(ii, s[i].Index);
                    }
                    UpdateCurrentGrid(ii);
                    ResultCI.SaveToArhiv(ii);
                }
            }

        }
    }
}

