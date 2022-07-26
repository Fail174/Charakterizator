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
using TxtLog;




namespace Charaterizator
{
    public partial class MainForm : Form
    {
        /*[DllImport("PCalcCoefAGAT.dll")]
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
		);*/

        // Занесены в настройку
        public int MAIN_TIMER = 1000;
        private int MAX_ERROR_COUNT = 3; //Количество ошибок чтения данных с устройств перед отключением
        private double MIN_SENSOR_CURRENT = 1.5;//минимльный ток датчика для обнаружения, мА
        private int MAX_COUNT_CAP_READ = 3;//максимальное количество циклов чтения тока ЦАП
        private double SKO_CURRENT = 0.5;//допуск по току ЦАП датчика до калибровки, мА
        private double SKO_CALIBRATION_CURRENT = 0.003;//допуск по току ЦАП после калибровки, мА

        private static int MaxChannalCount = 60;//максимальное количество каналов коммутаторы
        private static int MaxLevelCount = 4;//максимальное количество уровней датчиков (идентичных групп)

        private int MAX_COUNT_POINT = 5;//ожидание стабилизации давления в датчике, в циклах таймера
        private double SKO_PRESSURE = 0.2;  //(СКО) допуск по давлению, кПа
        private double TrhDeviation = 0.001;  // порог по отклонению R^2

        //private bool UseMensor;
        private int selectedPressurer = 0; // указывает какой задатчик давления использовать: 0) Mensor 1) Паскаль 2) Элемер 
        
        //Не занесены в настройку
        const int MAX_CALIBRATION_COUNT = 3;//максимальное количество циклов калибровки тока ЦАП        
        private int MENSOR_PRESSUER_WAIT = 60000;//время установления давления в менсоре, мсек
        private int SENSOR_PRESSUER_WAIT = 5000;//ожидание стабилизации давления в датчике, мсек        
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
        private CCalcMNK CCalcMNK = new CCalcMNK();
        private CPascal Pascal = new CPascal();
        private CBarometr Barometr = new CBarometr();
        private CElemer Elemer = new CElemer();




        //        private int MaxChannalCount = 30;//максимальное количество каналов коммутатора

        //        private СResultCH ResultCH = new СResultCH(MaxChannalCount);//результаты характеризации датчиков
        //        private CResultCI ResultCI = new CResultCI(MaxChannalCount);//результаты характеризации датчиков
        private СResultCH ResultCH = null;//результаты характеризации датчиков
        private CResultCI ResultCI = null;//результаты калибровки тока датчиков
        private CResultVR ResultVR = null;//результаты верификации датчиков
        private CResultMET ResultMET = null;//результаты сдачи метрологу датчиков

        private DateTime TimerValueSec;//таймер часов
        private int CommutatorReadError = 0;//число ошибко чтения данных с коммутатора 
        private int MultimetrReadError = 0;//число ошибко чтения данных с мультиметра
        private int MensorReadError = 0;//число ошибко чтения данных с менсора
        private bool SensorBusy = false;//Признак обмена данными с датчиками
        public bool ProcessStop = false;//Флаг остановки операции

        private bool TemperatureReady = false;//готовность термокамеры , температура датчиков стабилизирована
        private bool PressureReady = false;//готовность менсора , давление в датчиках стабилизировано
        private bool isSensorRead = false;
        //        private bool SensorPeriodRead = false;//Переодиское чтение параметров датчика

        private bool AutoRegim = false;//автоматический режим

        private int SelectedLevel = 1;//выбранный номер уровеня характеризации
        private bool SensorAbsPressuer = false;//датчик абсолютного давления

        private int TimerTickCount = 0;
        //private bool FormLoaded = false;
        private string strPascaleModule;
        private int numPascaleModule;


        private ListViewItem heldDownItem = null;//элемент для перетаскивания мышью
        private Point heldDownPoint;

        public bool AlgorithmMNK;
        public bool fPushPress=false;//расскачка

        //Инициализация переменных основной программы
        public MainForm()
        {
            //            FormLoad fm = new FormLoad();
            InitializeComponent();
            Thread t = new Thread(new ThreadStart(LoadScreen));
            t.Start();
            //            Focus();
            Thread.Sleep(2000);


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
                CCalculation.flag_ObrHod = Properties.Settings.Default.set_flagObrHod;      // Не учитывать обратный ход по давлени
                CCalculation.flag_MeanR = Properties.Settings.Default.set_MeanR;            // усреднять или нет матрицу сопротивлений
                AutoRegim = Properties.Settings.Default.set_AutoRegim;                      // автоматический режим
                TrhDeviation = Properties.Settings.Default.set_Deviation;
                fPushPress = Properties.Settings.Default.set_PushPress;

                tsmiPanelCommutator.Checked = Properties.Settings.Default.set_CommutatorVisible;
                tsmiPanelMultimetr.Checked = Properties.Settings.Default.set_MultimetrVisible;
                tsmiPanelMensor.Checked = Properties.Settings.Default.set_MensorVisible;
                tsmiPanelTermocamera.Checked = Properties.Settings.Default.set_TermocameraVisible;
                tsmiPanelLog.Checked = Properties.Settings.Default.set_PanelLogVisible;
                gbCommutator.Visible = tsmiPanelCommutator.Checked;
                gbMultimetr.Visible = tsmiPanelMultimetr.Checked;
                gbMensor.Visible = tsmiPanelMensor.Checked;
                gbTermoCamera.Visible = tsmiPanelTermocamera.Checked;
                panelLog.Visible = tsmiPanelLog.Checked;


                Multimetr.WAIT_READY = Properties.Settings.Default.set_MultimDataReady;     //время ожидания стабилизации тока, мсек
                Multimetr.WAIT_TIMEOUT = Properties.Settings.Default.set_MultimReadTimeout; //таймаут ожидания ответа от мультиметра, мсек
                Multimetr.SAMPLE_COUNT = Properties.Settings.Default.set_MultimReadCount;   //количество отчетов измерения мультиметром, раз
                Multimetr.READ_PERIOD = Properties.Settings.Default.set_MultimReadPeriod;   //период опроса мультиметра, мсек

                Commutator.MAX_SETCH = Properties.Settings.Default.set_CommMaxSetCH;        // максимально разрешенное коичество подключаемых к изм. линии датчиков 15
                Commutator.READ_PERIOD = Properties.Settings.Default.set_CommReadPeriod;    // Время опроса и обновление информации, мс
                Commutator.READ_PAUSE = Properties.Settings.Default.set_CommReadPause;      // время выдержки после переключения коммутатора (переходные процессы), мс
                MaxChannalCount = Properties.Settings.Default.set_CommReadCH;               // максимальное количество каналов коммутаторы
                Commutator.SetMaxChannal(MaxChannalCount);

                MaxLevelCount = Properties.Settings.Default.set_CommMaxLevelCount;          // максимальное количество уровней датчиков (идентичных групп)

                Mensor.READ_PERIOD = Properties.Settings.Default.set_MensorReadPeriod;      // Время опроса состояния менсора при работе с формой
                Mensor.READ_PAUSE = Properties.Settings.Default.set_MensorReadPause;        // задержка между приемом и передачей команд по COM порту, мс   - МЕНСОР
                Pascal.READ_PAUSE = Properties.Settings.Default.set_MensorReadPause;        // задержка между приемом и передачей команд по COM порту, мс   - ПАСКАЛЬ


                selectedPressurer = Properties.Settings.Default.set_selectPressurer;    // указывает какой задатчик давления использовать: 0) Mensor 1) Паскаль 2) Элемер 

                // UseMensor = Properties.Settings.Default.set_UseMensor;                      // указывает какой задатчик давления использовать: 1) Mensor значение true  / 2) Паскаль - значение false 
                //gbBarometr.Visible = !UseMensor;

                MAX_COUNT_POINT = (Properties.Settings.Default.set_MensorMaxCountPoint * 1000) / MAIN_TIMER + 1;      //ожидание стабилизации давления в датчике, в циклах таймера
                SENSOR_PRESSUER_WAIT = (Properties.Settings.Default.set_MensorMaxCountPoint * 1000);


                SKO_PRESSURE = Properties.Settings.Default.set_MensorSKOPressure;           //(СКО) допуск по давлению, кПа

                sensors.WAIT_TIMEOUT = Properties.Settings.Default.set_SensWaitTimeout;     //таймаут ожидания ответа от датчика
                sensors.WRITE_COUNT = Properties.Settings.Default.set_SensReadCount;        //число попыток записи команд в датчик
                sensors.WRITE_PERIOD = Properties.Settings.Default.set_SensReadPause;       //период выдачи команд


                CCalcMNK.Kf = Properties.Settings.Default.set_Math_Kf;
                CCalcMNK.Kpmax_dop = Properties.Settings.Default.set_Math_Kmax_dop;              
                CCalcMNK.Amax = Properties.Settings.Default.set_Math_Amax;
                CCalcMNK.Mmax = Properties.Settings.Default.set_Math_Mmax;
                CCalcMNK.Tnku = Properties.Settings.Default.set_Math_Tnku;
                CCalcMNK.KdM = Properties.Settings.Default.set_Math_KdM;
                CCalcMNK.deltaFdop_min = Properties.Settings.Default.set_Math_DFdop_min;
                CCalcMNK.Res_count_max = Properties.Settings.Default.set_Math_Res_count_max;
                AlgorithmMNK = Properties.Settings.Default.set_Math_AlgorithmMNK;
                

                string strFileNameDB = Properties.Settings.Default.FileNameDB;   // получаем путь и имя файла из Settings
                SensorsDB.SetConnectionDB(strFileNameDB);                        // устанавливаем соединение с БД    

                Application.Idle += IdleFunction;


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
            tbTemperature.Font = DrawingFont;
            numMensorPoint.Font = DrawingFont;
            numTermoCameraPoint.Font = DrawingFont;
            dtpClockTimer.Font = DrawingFont;
            numATMpress.Font = DrawingFont;
            //**********************************************************

            UpdateItems();//обновляем списки визуальных элементов
            btmMultimetr_Click(null, null);
            btnCommutator_Click(null, null);
            btnMensor_Click(null, null);
            btnThermalCamera_Click(null, null);



            //string s = SensorsDB.GetDataSensors("ЭНИ-100","2450","NumOfRange");

            t.Abort();
            FormSettings.EventCalcMNK += CalcMNK;
        }

        private void IdleFunction(Object sender, EventArgs e)
        {
            bool ProcessPause = false;
            while (ProcessPause)
            {
                Application.DoEvents();

            }
        }

        public void LoadScreen()
        {
            FormLoad fm = new FormLoad();
            //            fm.Show();
            //            while (!FormLoaded)
            //                Application.DoEvents();
            Application.Run(fm);
        }



        //Обновление визуальных элементов согласно установленным параметрам
        private void UpdateItems()
        {
            dataGridView1.Rows.Clear();
            for (int i = 0; i < MaxChannalCount; i++)
            {
                dataGridView1.Rows.Add(i + 1, false, "Нет данных", "Нет данных", false, false);
                dataGridView1[pow, i].Style.BackColor = Color.IndianRed;
                dataGridView1[ok, i].Style.BackColor = Color.IndianRed;
            }
        }

        private void UpDateSelectedChannal()
        {
            cbChannalCharakterizator.Items.Clear();
            cbChannalVerification.Items.Clear();
            cbChannalMetrolog.Items.Clear();
            for (int i = 0; i < MaxChannalCount; i++)
            {
                if (!CheckChannalEnable(i)) continue;//Если канал не выбран пропускаем 
                cbChannalCharakterizator.Items.Add(string.Format("Канал {0}", i + 1));
                cbChannalVerification.Items.Add(string.Format("Канал {0}", i + 1));
                cbChannalMetrolog.Items.Add(string.Format("Канал {0}", i + 1));
            }
        }

        //Выполняем при загрузке главной формы
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                //Visible = false;
                /*                btnMultimetr.PerformClick();
                                                                btnCommutator.PerformClick();
                                                                btnMensor.PerformClick();
                                                                btnThermalCamera.PerformClick();
                Application.DoEvents();*/

                MainTimer.Interval = MAIN_TIMER;
                MainTimer.Enabled = true;
                MainTimer.Start();
            }
            finally
            {
                //                Visible = true;
                //                FormLoaded = true;
                //                Focus();
                TopMost = false;
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


        // Настройки СОМ-порта барометра
        private void setComBarometr_Click(object sender, EventArgs e)
        {
            FormPortSettings newForm = new FormPortSettings();
            newForm.InitPortsettings(Properties.Settings.Default.COMbarometr,
                Properties.Settings.Default.COMbarometr_Speed,
                Properties.Settings.Default.COMbarometr_DataBits,
                Properties.Settings.Default.COMbarometr_StopBits,
                Properties.Settings.Default.COMbarometr_Parity);
            if (newForm.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.COMbarometr = newForm.GetPortName();
                Properties.Settings.Default.COMbarometr_Speed = newForm.GetPortSpeed();
                Properties.Settings.Default.COMbarometr_DataBits = newForm.GetPortDataBits();
                Properties.Settings.Default.COMbarometr_StopBits = newForm.GetPortStopBits();
                Properties.Settings.Default.COMbarometr_Parity = newForm.GetPortParity();
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

        /// <summary>
        /// Включение питания каналов коммутатора
        /// </summary>
        /// <param name="mode"></param>
        private void SetCommutatorChanalPower(int mode)
        {
            if ((Commutator != null)&&(Commutator.Connected))
            {
                Program.txtlog.WriteLineLog("Подключаем питание на линии коммутатора", 0);
                Commutator.SetAllPower();
            }
        }


        // Подключение задатчика МЕНСОРА или ПАСКАЛЯ
        private void btnMensor_Click(object sender, EventArgs e)
        {
            MensorReadError = 0;
            
            switch (selectedPressurer)
            {

                case 0: // подключение Менсора
                    {
                        //Mensor.ListMod.Clear();
                        //Mensor.ListMod.AddRange(new string[] { "[канал А] ДП-1", "[канал А] ДП-2", "[канал А] AutoRange", "[канал B] ДП-1", "[канал B] ДП-2", "[канал B] AutoRange", });        // список подключенных модулей

                        if (Mensor.Connect(Properties.Settings.Default.COMMensor,
                            Properties.Settings.Default.COMMensor_Speed,
                            Properties.Settings.Default.COMMensor_DataBits,
                            Properties.Settings.Default.COMMensor_StopBits,
                            Properties.Settings.Default.COMMensor_Parity) >= 0)
                        {
                            btnMensor.BackColor = Color.Green;
                            btnMensor.Text = "Подключен";
                            Program.txtlog.WriteLineLog("Задатчик давления Mensor подключен", 0);
                            //cbMensorTypeR.Items.Clear();
                            cbMensorTypeR.DataSource = Mensor.ListMod;
                            bMensorMeas.Name = "Измерение";

                            // Устанавливаем состояние барометра подключен 
                            bBarometr.BackColor = Color.Green;
                            bBarometr.Text = "Подключен";

                        }
                        else
                        {
                            btnMensor.BackColor = Color.IndianRed;
                            btnMensor.Text = "Не подключен";
                            Program.txtlog.WriteLineLog("Задатчик давления  Mensor не подключен", 1);
                            //cbMensorTypeR.Items.Clear();
                            cbMensorTypeR.DataSource = Mensor.ListMod;

                            // Устанавливаем состояние барометра Отключен 
                            bBarometr.BackColor = Color.IndianRed;
                            bBarometr.Text = "Не подключен";
                        }
                        break;
                    }

                case 1: // подключение Паскаля
                    {
                        //Pascal.ListMod.Clear();
                        //Pascal.ListMod.AddRange(new string[] { "[внутр] 1", "[внутр] 2", "[внутр] 3", "[внеш] 1", "[внеш] 2", });        // список подключенных модулей

                        if (Pascal.Connect(Properties.Settings.Default.COMMensor,
                            Properties.Settings.Default.COMMensor_Speed,
                            Properties.Settings.Default.COMMensor_DataBits,
                            Properties.Settings.Default.COMMensor_StopBits,
                            Properties.Settings.Default.COMMensor_Parity) >= 0)
                        {
                            btnMensor.BackColor = Color.Green;
                            btnMensor.Text = "Подключен";
                            Program.txtlog.WriteLineLog("Задатчик давления Паскаль подключен", 0);

                            //cbMensorTypeR.Items.Clear();
                            cbMensorTypeR.DataSource = Pascal.ListMod;
                            bMensorMeas.Name = "Обнуление";

                        }
                        else
                        {
                            btnMensor.BackColor = Color.IndianRed;
                            btnMensor.Text = "Не подключен";
                            Program.txtlog.WriteLineLog("Задатчик давления Паскаль не подключен", 1);
                            //cbMensorTypeR.Items.Clear();
                            cbMensorTypeR.DataSource = Pascal.ListMod;

                        }
                        break;
                    }

                case 2: // подключение Элемера (АКД-12К)
                    {
                        

                        if (Elemer.Connect(Properties.Settings.Default.COMMensor,
                            Properties.Settings.Default.COMMensor_Speed,
                            Properties.Settings.Default.COMMensor_DataBits,
                            Properties.Settings.Default.COMMensor_StopBits,
                            Properties.Settings.Default.COMMensor_Parity) >= 0)
                        {
                            btnMensor.BackColor = Color.Green;
                            btnMensor.Text = "Подключен";
                            Program.txtlog.WriteLineLog("Задатчик давления Элемер АКД-12К подключен", 0);

                            //cbMensorTypeR.Items.Clear();
                            cbMensorTypeR.DataSource = Elemer.ListMod;                         
                        }
                        else
                        {
                            btnMensor.BackColor = Color.IndianRed;
                            btnMensor.Text = "Не подключен";
                            Program.txtlog.WriteLineLog("Задатчик давления Элемер АКД-12К не подключен", 1);                           
                            cbMensorTypeR.DataSource = Elemer.ListMod;
                        }
                        break;
                    }

            }
            

        }


        // подключение Барометра
        private void bBarometr_Click(object sender, EventArgs e)
        {
            if (Barometr.Connect(Properties.Settings.Default.COMbarometr,
             Properties.Settings.Default.COMbarometr_Speed,
             Properties.Settings.Default.COMbarometr_DataBits,
             Properties.Settings.Default.COMbarometr_StopBits,
             Properties.Settings.Default.COMbarometr_Parity) >= 0)
            {
                bBarometr.BackColor = Color.Green;
                bBarometr.Text = "Подключен";
                Program.txtlog.WriteLineLog("Барометр подключен", 0);
            }
            else
            {
                bBarometr.BackColor = Color.IndianRed;
                bBarometr.Text = "Не подключен";
                Program.txtlog.WriteLineLog("Барометр не подключен", 1);
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
            if ((Mensor != null) && (selectedPressurer == 0))
            {

                Mensor.MenStartTimer();
                Mensor.ShowDialog();

            }
            //else
            //{
            //    Mensor = new FormMensor();
            //    btnMensor.PerformClick();
            //}
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
                try
                {
                    tbSelChannalNumber.Text = string.Format("Канал {0}", i + 1);
                    tbInfoDesc.Text = sensors.sensor.GetDesc();
                    tbInfoTeg.Text = sensors.sensor.GetTeg();
                    tbInfoMessage.Text = sensors.sensor.GetMes();

                    tbInfoPressureModel.Text = new String(sensors.sensor.PressureModel);
                    tbInfoUp.Text = sensors.sensor.UpLevel.ToString("f6");
                    tbInfoDown.Text = sensors.sensor.DownLevel.ToString("f6");
                    tbInfoSerialNumber.Text = sensors.sensor.SerialNumber.ToString();
                    tbInfoMin.Text = sensors.sensor.MinLevel.ToString("f6");
                    tbInfoMesUnit.Text = sensors.sensor.GetUnit();


                    tbInfoHartVersion.Text = sensors.sensor.v1.ToString();
                    tbInfoSensorVersion.Text = sensors.sensor.v2.ToString();
                    tbInfoSoftVersion.Text = sensors.sensor.v3.ToString();
                    tbInfoHardVersion.Text = sensors.sensor.v4.ToString();

                    tbInfoDeviceAdress.Text = sensors.sensor.Addr.ToString("D2");
                    tbInfoFactoryNumber.Text = sensors.sensor.uni.ToString();
                    tbInfoSoftVersion.Text = sensors.sensor.v3.ToString();
                    cbInfoPreambul.Text = sensors.sensor.pre.ToString();
                    tbInfoSensorType.Text = sensors.sensor.GetdevType();

                    DateTime dt = new DateTime(1900 + (int)(sensors.sensor.data & 0xFF), (int)(sensors.sensor.data >> 8) & 0xFF, (int)((sensors.sensor.data >> 16) & 0xFF));
                    dtpInfoDate.Value = dt;
                    /*            string SelectedSensor = sensors.sensor.Addr.ToString("D2") + " | " + sensors.sensor.GetdevType() + " | " + sensors.sensor.uni;
                                tbCharact.Text = SelectedSensor;
                                tbCoef.Text = SelectedSensor;*/
                    if ((tbInfoSensorType.Text == "ЭНИ-12") || (tbInfoSensorType.Text == "ЭНИ-12М"))
                    {
                        pbSensorImage.BackgroundImage = Properties.Resources.eni_12h_hs_m;
                    }
                    else
                    {
                        pbSensorImage.BackgroundImage = Properties.Resources.eni_100_m;
                    }

                }
                catch
                {
                    Program.txtlog.WriteLineLog(string.Format("Ошибка заполнения информационной панели датчика (Дата: {0}) ", sensors.sensor.data), 1);
                }
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

                    } while ((Math.Abs(I4 - 4.0) > SKO_CURRENT) && (ci < MAX_COUNT_CAP_READ));

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
            List<int> ErrorList = new List<int>();
            pbCHProcess.Maximum = MaxChannalCount;
            pbCHProcess.Minimum = 0;
            pbCHProcess.Value = 0;
            int ci = 0;
            int cc = 0;
            float I4 = 0;
            float I20 = 0;

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
                        if (ProcessStop) return;//прекращаем
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
                            /*DialogResult result = MessageBox.Show(
                                    "Выполнить калибровку?",
                                    "Превышено максимальное отклонение тока ЦАП (4мА)!",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Information,
                                    MessageBoxDefaultButton.Button1);
                            if (result == DialogResult.No)
                            {
                                cc++;
                                continue;
                            }*/
                            Program.txtlog.WriteLineLog("CL: Превышено максимальное отклонение тока ЦАП (4мА)", 1);
                            ErrorList.Add(i);
                            continue;
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
                            /*DialogResult result = MessageBox.Show(
                                    "Выполнить калибровку?",
                                    "Превышено максимальное отклонение тока ЦАП (20мА)!",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Information,
                                    MessageBoxDefaultButton.Button1);
                            if (result == DialogResult.No)
                            {
                                cc++;
                                continue;
                            }*/
                            Program.txtlog.WriteLineLog("CL: Превышено максимальное отклонение тока ЦАП (20мА)", 1);
                            ErrorList.Add(i);
                            continue;
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

                    if ((Math.Abs(I4 - 4.0) > SKO_CALIBRATION_CURRENT) && (Math.Abs(I20 - 20.0) > SKO_CALIBRATION_CURRENT))
                    {
                        Program.txtlog.WriteLineLog("CL: Значение тока ЦАП вне допуска. Калибровка не выполнена!", 1);
                        ErrorList.Add(i);
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
                    ErrorList.Add(i);
                }
                //Commutator.SetConnectors(i, 1);
                seli++;
            }
            Program.txtlog.WriteLineLog("CL: Калибровка ЦАП завершена!", 2);
            for (int ei = 0; ei < ErrorList.Count; ei++)
            {
                Program.txtlog.WriteLineLog("CL: Не выполнена калибровка датчика в канале: " + (ei + 1).ToString(), 1);
            }
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
                int i = Convert.ToInt32(str) - 1;

                //                int i = cbChannalCharakterizator.SelectedIndex;

                if ((i < 0) || !CheckChannalEnable(i)) return;//Если канал не выбран пропускаем обработку

                //if (Commutator.ActivCH != i + 1)
                Commutator.SetConnectors(i, 0);

                if (sensors.SelectSensor(i))//выбор датчика на канале i
                {
                    bool readresult = false;
                    int ch = 0;
                    do
                    {
                        readresult = sensors.SensorValueReadC03();
                        Application.DoEvents();
                        ch++;
                    } while ((!sensors.ValidateSensorParam()) && (ch < sensors.WRITE_COUNT));

                    if (readresult)
                    {

                        /*if (sensors.SensorValueReadC03())
                    {*/
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
            int FinishNumber = MaxChannalCount - 1;   //конечный канал
            int Diapazon;
            if (cbDiapazon1.Text != "")
            {
                Diapazon = Convert.ToInt32(cbDiapazon1.Text);
            }
            else
            {
                Diapazon = 1;
            }

            Program.txtlog.WriteLineLog("CH: Старт операции характеризации для заданного давления ... ", 2);

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
                        readresult = sensors.SensorValueReadC03();
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
                            ResultCH.AddPoint(i, (double)numTermoCameraPoint.Value, Diapazon, (double)numMensorPoint.Value, sensors.sensor.OutVoltage, sensors.sensor.Resistance, 0);
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
            Program.txtlog.WriteLineLog("CH: Операция характеризации для выбранного давления завершена!", 2);
        }

        //расчет коэффициентов и запись в датчики
        private void СaclSensorCoeff()
        {
            //int seli = 0;
            int StartNumber = 0;    //начальный канал
            int FinishNumber = MaxChannalCount - 1;   //конечный канал
            List<int> ErrorList = new List<int>(); 

            Program.txtlog.WriteLineLog("CH: Старт расчета коэффициентов для выбранных датчиков ... ", 2);

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
                    if (!sensors.C253SensorCoefficientNumber())//установка номера набора коэффициентов
                    {
                        Program.txtlog.WriteLineLog("CH: Номер набора коэффициентов не установлен в датчике канала " + (i + 1).ToString(), 1);
                    }

                    double Pmax = sensors.sensor.UpLevel; // - sensors.sensor.DownLevel; // Делим на максимум , а не на диапазон (16.03.2022) 
                    if ((Pmax <= 0) || (Pmax > 1000000))
                    {
                        Program.txtlog.WriteLineLog("CH: Не верные НПИ и ВПИ датчика в канале:" + (i + 1).ToString(), 1);
                        ErrorList.Add(i);
                        continue;
                    }
                    bool sensor_DV = ResultCH.Channal[i].PressureModel[1] == '2';//Датчик ДВ
                    Matrix<double> ResulCoefmtx;
                    if (AlgorithmMNK)
                    {
                        // Матрица давлений
                        Matrix<double> Pmtx = ResultCH.GetPressuerMatrix(i);
                        // Матрица напряжений
                        Matrix<double> Umtx = ResultCH.GetVoltageMatrix(i);
                        // Матрица Сопротивлений
                        Matrix<double> Rmtx = ResultCH.GetRezistansMatrix(i);
                        // Матрица температур
                        Matrix<double> Tmtx = ResultCH.GetTemperatureMatrix(i);
                        // Название датчика
                        string SensName = ResultCH.Channal[i].GetSensorType();//"ЭНИ-12";
                        // Матрица с результатами
                        ResulCoefmtx = CalculationMNK(Rmtx, Umtx, Pmtx, Tmtx, Pmax, sensor_DV, SensName); ;

                    }
                    else
                    {
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
                            mtxP[c_row, c_cols] = ResultCH.Channal[i].Points[j].Pressure;
                            mtxR[c_row, c_cols] = ResultCH.Channal[i].Points[j].Resistance;
                            mtxU[c_row, c_cols] = ResultCH.Channal[i].Points[j].OutVoltage;
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

                        ResulCoefmtx = CalculationMtx.CalculationCoef(Rnew, Unew, Pnew, Pmax, sensor_DV);
                    }


                    double[] db = new double[24];
                    if (ResulCoefmtx.RowCount < 24)
                    {
                        Program.txtlog.WriteLineLog("CH: Недостаточное количество точек при характеризация: " + ResulCoefmtx.RowCount.ToString(), 1);
                        ErrorList.Add(i);
                        continue;
                    }
                    Program.txtlog.WriteLineLog("CH: Расчитанные коэффициенты для датчика в канале " + (i + 1).ToString(), 0);
                    for (int j = 0; j < 24; j++)
                    {
                        Program.txtlog.WriteLineLog("Коэффициент " + (j + 1).ToString() + ": " + ResulCoefmtx.At(j, 0), 0);
                        if (j < 24)
                        {
                            db[j] = ResulCoefmtx.At(j, 0);
                            sensors.sensor.Coefficient[j] = Convert.ToSingle(ResulCoefmtx.At(j, 0));
                        }
                    }


                    // доработка 11.04. Сравнение допустимого отклонения с заданным пороговым значкением перед записью коэффициентов в датчик 
                    if (ResulCoefmtx[24, 0] > TrhDeviation)
                    {
                        Program.txtlog.WriteLineLog(string.Format("CH: Внимание! Рассчитанное допустимое отклонение датчика в канале{0} выше порогового значения!", i + 1), 1);
                        //Program.txtlog.WriteLineLog(string.Format("CH: Коэффициенты хараткеризации не будут записсына в датчик в канале{0}...", i + 1), 1);
                        ErrorList.Add(i);
                        continue;
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog(string.Format("CH: Рассчитанное допустимое отклонение датчика в канале{0} находится в допустимых пределах", i + 1), 2);
                        Program.txtlog.WriteLineLog(string.Format("CH: Старт записи коэффициентов в датчик в канале{0}...", i + 1), 2);
                        if (!sensors.C250SensorCoefficientWrite())//запись коэффициентов в ОЗУ датчика
                        {
                            Program.txtlog.WriteLineLog("CH: Ошибка записи коэффициентов в датчик в канале " + (i + 1).ToString(), 1);
                            ErrorList.Add(i);
                            continue;
                        }
                        else
                        {
                            for (int j = 0; j < 24; j++)
                            {
                                float div = Math.Abs(sensors.sensor.Coefficient[j] - Convert.ToSingle(ResulCoefmtx.At(j, 0)));
                                if (div != 0)
                                {
                                    Program.txtlog.WriteLineLog(string.Format("CH: Запись коэффициента {0} не удалась!", j + 1), 2);
                                    Program.txtlog.WriteLineLog("Считано " + (j + 1).ToString() + ": " + sensors.sensor.Coefficient[j], 0);
                                }
                            }
                        }
                        

                        if (!sensors.C252EEPROMCoefficientWrite())//запись в коэффициентов EEPROM
                        {
                            Program.txtlog.WriteLineLog("CH: Ошибка записи коэффициентов в EEPROM датчика в канале " + (i + 1).ToString(), 1);
                            ErrorList.Add(i);
                            continue;
                        }
                        else
                        {
                            Program.txtlog.WriteLineLog(string.Format("CH: Запись коэффициентов в датчик в канале{0} завершена!", i + 1), 2);
                            ResultCH.AddCoeff(i, db);
                        }
                        if (!sensors.С42SensorReset())//перезагрузка датчика
                        {
                            Program.txtlog.WriteLineLog("CH: Сброс датчика не выполнен! " + (i + 1).ToString(), 1);
                            ErrorList.Add(i);
                        }
                    }
                } else {
                    Program.txtlog.WriteLineLog("CH: Датчик не найден в канале " + (i + 1).ToString(), 1);
                    ErrorList.Add(i);
                }
                //Commutator.SetConnectors(i, 1); // команда отключить датчик с индексом i   
                //seli++;
            }
            Program.txtlog.WriteLineLog("CH: Операция вычисления и записи коэффициентов завершена!", 2);

            for (int ei = 0; ei < ErrorList.Count; ei++)
            {
                Program.txtlog.WriteLineLog("CH: Не выполнена запись коэффициентов датчика в канале: " + (ei + 1).ToString(), 1);
            }
        }


        //верификация датчиков
        //чтение всех измеренных параметров с текущего датчика давления
        private void ReadSensorPressure()
        {
            int seli = 0;
            int StartNumber = 0;    //начальный канал
            int FinishNumber = MaxChannalCount - 1;   //конечный канал
            float VPI, NPI;
            VPI = Convert.ToSingle(nud_VR_VPI.Value);
            NPI = Convert.ToSingle(nud_VR_NPI.Value);

            Program.txtlog.WriteLineLog("VR: Старт операции верификации для выбранного давления ... ", 2);

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
                        ResultVR.AddPoint(i, (double)numTermoCameraPoint.Value, sensors.sensor.NPI, sensors.sensor.VPI, (double)numMensorPoint.Value, sensors.sensor.Pressure, Multimetr.Current, Ir, sensors.sensor.OutVoltage, sensors.sensor.Resistance);

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
            Program.txtlog.WriteLineLog("VR: Операция верификации для выбранного давления завершена ", 2);
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
                    int res = sensors.С35WriteVPI_NPI(VPI, NPI);
                    if (res >= 0)
                    {
                        Program.txtlog.WriteLineLog("VR: Выполнена запись НПИ ВПИ датчика в канале " + (i + 1).ToString(), 0);
                    }
                    else
                    {
                        if (res == -6)
                        {
                            Program.txtlog.WriteLineLog("VR: Неверные знаения НПИ ВПИ датчика. Команда не выполнена!", 1);
                        }
                        else
                        {
                            Program.txtlog.WriteLineLog("VR: Запись НПИ ВПИ датчика не выполнена!", 1);
                        }
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
            if ((!SensorAbsPressuer) && (Math.Abs(Convert.ToDouble(tbMensorData.Text)) > 0.5))
            {
                if (MessageBox.Show("Текущее давление не равно нулю. Продолжить установку нуля?", "Подтверждение команды", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    Program.txtlog.WriteLineLog("Операция прекращена.", 0);
                    return;
                }
            }
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
                    if (SensorAbsPressuer)
                    {
                        float pressure =Convert.ToSingle(numATMpress.Value);
                        if (sensors.С143SetZero(pressure))
                        {
                            Program.txtlog.WriteLineLog("VR: Выполнена установка нуля датчика ДА в канале " + (i + 1).ToString(), 0);
                        }
                        else
                        {
                            Program.txtlog.WriteLineLog("VR: Установка нуля датчика ДА не выполнена!", 1);
                        }
                    }
                    else
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
            if ((ResultCH == null) || (ResultCH.Channal.Count <= i) || (i < 0) || (ResultCH.Channal[i].Points.Count <= 0))
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
                dataGridView2.Rows[j].Cells[7].Value = ResultCH.Channal[i].Points[j].Deviation.ToString("f");
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
                dataGridView3.Rows.Add(j + 1, "", "", "", "", "", "", "", "");
                dataGridView3.Rows[j].Cells[1].Value = ResultVR.Channal[i].Points[j].Datetime.ToString("dd.MM.yyyy HH:mm:ss");      //
                dataGridView3.Rows[j].Cells[2].Value = ResultVR.Channal[i].Points[j].Temperature.ToString();   //
                dataGridView3.Rows[j].Cells[3].Value = ResultVR.Channal[i].Points[j].NPI.ToString();   //
                dataGridView3.Rows[j].Cells[4].Value = ResultVR.Channal[i].Points[j].VPI.ToString();   //
                dataGridView3.Rows[j].Cells[5].Value = ResultVR.Channal[i].Points[j].PressureZ.ToString("f3");
                dataGridView3.Rows[j].Cells[6].Value = ResultVR.Channal[i].Points[j].PressureF.ToString("f3");
                dataGridView3.Rows[j].Cells[7].Value = ResultVR.Channal[i].Points[j].CurrentR.ToString("f4");
                dataGridView3.Rows[j].Cells[8].Value = ResultVR.Channal[i].Points[j].CurrentF.ToString("f4");
                dataGridView3.Rows[j].Cells[9].Value = ResultVR.Channal[i].Points[j].OutVoltage.ToString("f4");
                dataGridView3.Rows[j].Cells[10].Value = ResultVR.Channal[i].Points[j].Resistance.ToString("f4");
            }
            dataGridView3.Sort(dataGridView3.Columns[0], ListSortDirection.Descending);
            dataGridView3.ClearSelection();
            dataGridView3.Rows[0].Selected = true;
            //            dataGridView3.Rows[0].Cells[0].Selected = true;
        }

        //обновляем грид результатов верификации для датчика в канале i
        private void UpDateMetrologGrid(int i)
        {
            dataGridView5.Rows.Clear();

            if ((ResultMET == null) || (ResultMET.Channal.Count <= i) || (i < 0) || (ResultMET.Channal[i].Points.Count <= 0))
            {
                Program.txtlog.WriteLineLog("Result MET: Результаты не сформированы!", 1);
                return;
            }

            for (int j = 0; j < ResultMET.Channal[i].Points.Count; j++)//заполняем грид данными текущего датчика
            {
                dataGridView5.Rows.Add(j + 1, "", "", "", "", "", "", "", "");
                dataGridView5.Rows[j].Cells[1].Value = ResultMET.Channal[i].Points[j].Datetime.ToString("dd.MM.yyyy HH:mm:ss");      //
                dataGridView5.Rows[j].Cells[2].Value = ResultMET.Channal[i].Points[j].Temperature.ToString();   //
                dataGridView5.Rows[j].Cells[3].Value = ResultMET.Channal[i].Points[j].NPI.ToString();   //
                dataGridView5.Rows[j].Cells[4].Value = ResultMET.Channal[i].Points[j].VPI.ToString();   //
                dataGridView5.Rows[j].Cells[5].Value = ResultMET.Channal[i].Points[j].PressureZ.ToString("f3");
                dataGridView5.Rows[j].Cells[6].Value = ResultMET.Channal[i].Points[j].PressureF.ToString("f3");
                dataGridView5.Rows[j].Cells[7].Value = ResultMET.Channal[i].Points[j].CurrentR.ToString("f4");
                dataGridView5.Rows[j].Cells[8].Value = ResultMET.Channal[i].Points[j].CurrentF.ToString("f4");
            }
            dataGridView5.Sort(dataGridView5.Columns[0], ListSortDirection.Descending);
            dataGridView5.ClearSelection();
            dataGridView5.Rows[0].Selected = true;
            //            dataGridView3.Rows[0].Cells[0].Selected = true;
        }
        //обновляем грид калибровки тока для датчика в канале i
        private void UpdateCurrentGrid(int i)
        {
            dataGridView4.Rows.Clear();
            //if ((ResultCI == null) || (ResultCI.Channal.Count <= i))

            if ((ResultCI == null) || (ResultCI.Channal.Count <= i) || (i < 0) || (ResultCI.Channal[i].Points.Count <= 0))
            {
                //Program.txtlog.WriteLineLog("Result CL: Результаты чтения ЦАП не сформированы!", 1);// 05.08.2020 
                return;
            }

            for (int j = 0; j < ResultCI.Channal[i].Points.Count; j++)//заполняем грид данными текущего датчика
            {
                dataGridView4.Rows.Add(j + 1, "", "", "", "", "");
                dataGridView4.Rows[j].Cells[1].Value = ResultCI.Channal[i].Points[j].Datetime.ToString();                 //
                dataGridView4.Rows[j].Cells[2].Value = ResultCI.Channal[i].Points[j].Temperature.ToString("f2");   //
                dataGridView4.Rows[j].Cells[3].Value = ResultCI.Channal[i].Points[j].I4.ToString("f4");
                dataGridView4.Rows[j].Cells[4].Value = ResultCI.Channal[i].Points[j].I20.ToString("f4");
            }
            dataGridView4.Sort(dataGridView4.Columns[0], ListSortDirection.Descending);
            dataGridView4.ClearSelection();
            dataGridView4.Rows[0].Selected = true;
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

                        dataGridView1.Rows[i].Cells[sen].Value = "Нет данных";      //тип датчика
                        dataGridView1.Rows[i].Cells[zn].Value = "Нет данных";       //заводской номер

                        if (!CheckChannalEnable(i))
                        {
                            continue;//Если канал не выбран пропускаем обработку
                        }

                        dataGridView1.Rows[i].Selected = true;
                        dataGridView1.Rows[i].Cells[ch].Selected = true;
                        Application.DoEvents();


                        Program.txtlog.WriteLineLog(string.Format("Поиск датчиков на линии {0} ...", i + 1), 0);

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
                                dataGridView1.Rows[i].Cells[sel].Value = false;
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
                                if (!sensors.C14SensorRead())       //чтение данных с датчика
                                    Program.txtlog.WriteLineLog(string.Format("Параметры ПД датчика на линии {0} не прочитаны!", i + 1), 1);
                                // Program.txtlog.WriteLineLog(string.Format("ВПИ ПД датчика {0}", sensors.sensor.UpLevel), 0);
                                if (!sensors.C140ReadPressureModel())//читаем модель ПД
                                    Program.txtlog.WriteLineLog(string.Format("Модель ПД датчика на линии {0} не прочитана!", i + 1), 1);

                                Thread.Sleep(500);
                                sensors.ParseReadBuffer(500);//ждем завершения операций по датчику в потоке
                                if (!sensors.CheckValidSN())
                                {
                                    Program.txtlog.WriteLineLog("Обнаружено повторение серийного номера!", 1);

                                    if(MessageBox.Show("Прервать операцию поиска?", "Обнаружено повторение серийного номера!", MessageBoxButtons.YesNo)==DialogResult.Yes)
                                        return -2;
                                }
                                UpdateDataGrids(i);         //обновляем информацию по датчику в таблице
                                SensorFind = true;
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
                            dataGridView1.Rows[i].Cells[sel].Value = false;
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
                        btnSensorSeach.Text = "Идет поиск датчиков... Остановить? ";

                        UpdateItemState(1);
                        SeachConnectedSensor();
                    }
                    else
                    {
                        MessageBox.Show("Не выбраны каналы для поиска датчиков", "Операция прервана", MessageBoxButtons.OK);
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
                FormPause formpause = new FormPause();
                if (formpause.ShowDialog() != DialogResult.OK)
                //                if (MessageBox.Show("Для продолжения нажмите 'Да'. Чтобы остановить поиск нажмите 'Нет'", "Поиск поставлен на паузу", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    ProcessStop = true;
                    Program.txtlog.WriteLineLog("Поиск прекращен по команде пользователя", 0);
                }
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Properties.Settings.Default.Save();
            MainTimer.Stop();
            MainTimer.Enabled = false;
            sensors.DisConnect();
            Mensor.DisConnect();
            Pascal.DisConnect();
            Elemer.DisConnect();
            Multimetr.DisConnect();
            Commutator.DisConnect();
            Barometr.DisConnect();
        }




        //**************************************************************************************************
        //
        // СРАБАТЫВАНИЕ ГЛАВНОГО ТАЙМЕРА
        //
        //**************************************************************************************************


        private void MainTimer_Tick(object sender, EventArgs e)
        {
            TimerTickCount++;

            // Опрос датчиков температуры
            if (ThermalCamera.Connected)
            {
                tbTemperature.Text = Convert.ToString(ThermalCamera.ReadData());
            }
            else
            {
                if (btnThermalCamera.BackColor != Color.IndianRed)
                {
                    btnThermalCamera.BackColor = Color.IndianRed;
                }
                else
                {
                    btnThermalCamera.BackColor = Color.Transparent;
                }
            }

            // Опрос состояния коммутатора
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

            // Чтение показаний мультиметра
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

            // Чтение аатм. давления с барометра
            if (Barometr.Connected)
            {
                numATMpress.Value = Convert.ToDecimal(Barometr.amtPress); //обновляем данные
            }
            else
            {
                if (bBarometr.BackColor != Color.IndianRed)
                {
                    bBarometr.BackColor = Color.IndianRed;
                }
                else
                {
                    bBarometr.BackColor = Color.Transparent;
                }
            }





            if (!cb_ManualMode.Checked) // если НЕ выбран ручной режим
            {
                // Чтение показаний задатчика давления
                // Определяем какой из задатчиков используется
                switch (selectedPressurer)
                {
                    case 0: // менсор
                        {
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
                            break;
                        }

                    case 1:
                        {
                            if (Pascal.Connected) // паскаль
                            {
                                ReadPascal(); //обновляем данные с Паскаля
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
                            break;
                        }

                    case 2:
                        {
                            if (Elemer.Connected)
                            {
                                ReadElemer(); //обновляем данные с Элемера
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
                            break;
                        }
                }
            }




            // Четние данных с датчика
            if (!SensorBusy && sensors.IsConnect())
            {
                if ((cbSensorPeriodRead.Checked) && (!isSensorRead))
                {
                    ReadSensor(); //выполняем переодичекое чтение датчика
                }
            }
            else
            {
                cbSensorPeriodRead.Checked = false;
            }

            tbDateTime.Text = DateTime.Now.ToString("HH:mm:ss  dd.MM.yyyy");//часы

            if (!dtpClockTimer.Enabled)//таймер часов
            {
                Console.Beep();
                if (btnClockTimer.BackColor != Color.Yellow)
                {
                    btnClockTimer.BackColor = Color.Yellow;
                }
                else
                {
                    btnClockTimer.BackColor = Color.Transparent;
                }
                try
                {
                    TimerValueSec = new DateTime(TimerValueSec.Ticks - MAIN_TIMER * 10000);//1 тик 10000мсек
                    if (((TimerValueSec.Hour == 0) && (TimerValueSec.Minute == 0) && (TimerValueSec.Second == 0)) || (TimerValueSec.Hour >= 23))//остановка таймера
                    {
                        btnClockTimer.BackColor = Color.Green;
                        dtpClockTimer.Enabled = true;
                        dtpClockTimer.Value = new DateTime(TimerValueSec.Year, TimerValueSec.Month, TimerValueSec.Day, 0, 0, 0);//
                        Console.Beep();
                        Console.Beep();
                    }
                    else
                    {
                        dtpClockTimer.Value = TimerValueSec;
                    }
                }
                catch
                {
                    Program.txtlog.WriteLineLog("Ошибка временной структуры: " + MAIN_TIMER.ToString(), 1);
                }
            }
        }



        //чтение данных с МЕНСОРА
        private void ReadMensor()
        {
            int CH_mensor = Mensor._activCH;    // Получаем номер активного канала (0 значит А,   1 значит B,   -1 = не прочитали )                   
            if (CH_mensor != -1)
            {

                if (SKO_PRESSURE > Math.Abs(Mensor._press - Mensor.UserPoint))//Convert.ToDouble(numMensorPoint.Value)))
                {
                    //Console.Beep();
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
                    if (MensorCountPoint != 0)
                    {
                        Console.Beep();
                        Console.Beep();
                        Console.Beep();
                    }
                    tbMensorData.BackColor = Color.White;
                    MensorCountPoint = 0;
                }

                // Получаем текущее значение давления и обновляем гл. форму 
                tbMensorData.Text = Mensor._press.ToString("f3");

                // Получаем тип давления | 0 - АБС, 1 - ИЗБ
                if (Mensor._tpress == 0)
                {
                    rbPressABS.Checked = true;
                    rbPressIZB.Checked = false;
                }
                else if (Mensor._tpress == 1)
                {
                    rbPressABS.Checked = false;
                    rbPressIZB.Checked = true;
                }
                else if (Mensor._tpress == -1)
                {
                    rbPressABS.Checked = false;
                    rbPressIZB.Checked = false;
                }

                // Получаем тек. значение уставки  и обновляем гл. форму
                //numMensorPoint.Text = Mensor._point.ToString("f2");

                // Получаем и выводим на форму давление встроенного барометра Менсора
                numATMpress.Value = Convert.ToDecimal(Mensor._barometr);


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
                MensorReadError++;
                Program.txtlog.WriteLineLog("Ошибка чтения данных с Менсора. Количество ошибок: " + MensorReadError.ToString(), 1);
            }

            if (MensorReadError >= MAX_ERROR_COUNT)
            {
                /*tbMensorData.Text = "-1";
                numMensorPoint.Text = "-1";
                cbMensorTypeR.SelectedIndex = -1;*/
                Mensor.DisConnect();
                btnMensor.BackColor = Color.IndianRed;
                btnMensor.Text = "Не подключен";
                Program.txtlog.WriteLineLog("Нет данных с задатчика давления. Устройство отключено.", 1);
                //btnMensor.PerformClick();
                btnMensor_Click(null, null);
            }
        }






        //чтение данных с Паскаля
        private void ReadPascal()
        {

            if (Pascal.rangeModule[0] != -1)
            {


                if (SKO_PRESSURE > Math.Abs(Pascal.press - Pascal.UserPoint))//Convert.ToDouble(numMensorPoint.Value)))
                {
                    //Console.Beep();
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
                    if (MensorCountPoint != 0)
                    {
                        Console.Beep();
                        Console.Beep();
                        Console.Beep();
                    }
                    tbMensorData.BackColor = Color.White;
                    MensorCountPoint = 0;
                }


                // Получаем текущее значение давления и обновляем гл. форму 
                tbMensorData.Text = Pascal.press.ToString("f3");


                if (cbMensorTypeR.Items.Count > 0)
                {
                    // Получаем тип преобразователя (удерживаемый диапазон)
                    int typeR = Pascal.rangeModule[1] - 1;  //  
                                                            // Обновляем тип преобзарователя
                    if (Pascal.rangeModule[0] == 1)
                    {
                        cbMensorTypeR.SelectedIndex = typeR; //  по списку
                    }
                    else if (Pascal.rangeModule[0] == 2)
                    {
                        cbMensorTypeR.SelectedIndex = typeR + Pascal.M1num;
                    }
                }

                // Задача
                if (Pascal.modeStart)
                {
                    bMensorControl.BackColor = Color.LightGreen;
                }
                else
                {
                    bMensorControl.BackColor = Color.Transparent;
                }

                // Обнуление
                if (Pascal.modeClearP)
                {
                    bMensorMeas.BackColor = Color.LightGreen;
                }
                else
                {
                    bMensorMeas.BackColor = Color.Transparent;
                }

                // Вентиляция сброс давления
                if (Pascal.modeVent)
                {
                    bMensorVent.BackColor = Color.LightGreen;
                }
                else
                {
                    bMensorVent.BackColor = Color.Transparent;
                }


                MensorReadError = 0;
            }
            else
            {
                MensorReadError++;
                Program.txtlog.WriteLineLog("Ошибка чтения данных с Паскаля. Количество ошибок: " + MensorReadError.ToString(), 1);
            }

            if (MensorReadError >= MAX_ERROR_COUNT)
            {
                /*tbMensorData.Text = "-1";
                numMensorPoint.Text = "-1";
                cbMensorTypeR.SelectedIndex = -1;*/
                Pascal.DisConnect();
                btnMensor.BackColor = Color.IndianRed;
                btnMensor.Text = "Не подключен";
                Program.txtlog.WriteLineLog("Нет данных с задатчика давления. Устройство отключено.", 1);
                btnMensor_Click(null, null);
            }
        }



        //чтение данных с Элемера
        private void ReadElemer()
        {
            if (SKO_PRESSURE > Math.Abs(Elemer.press - Elemer.UserPoint)) 
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
                if (MensorCountPoint != 0)
                {
                    Console.Beep();
                    Console.Beep();
                    Console.Beep();
                }
                tbMensorData.BackColor = Color.White;
                MensorCountPoint = 0;
            }


            // Получаем текущее значение давления и обновляем гл. форму 
            tbMensorData.Text = Elemer.press.ToString("f3");

            // Обновление цвета кнопок в зав-ти от режима            
            // Задача
            if (Elemer.modeStartReg)
            {
                bMensorControl.BackColor = Color.LightGreen;
            }
            else
            {
                bMensorControl.BackColor = Color.Transparent;
            }

            
            // Измерение
            if (Elemer.modeStopReg)
            {
                bMensorMeas.BackColor = Color.LightGreen;
            }
            else
            {
                bMensorMeas.BackColor = Color.Transparent;
            }
            
            // Вентиляция сброс давления
            if (Elemer.modeClearP)
            {
                bMensorVent.BackColor = Color.LightGreen;
            }
            else
            {
                bMensorVent.BackColor = Color.Transparent;
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
        private void PowerComutators(Int64 data32)
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
                if ((System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Control) && (Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[sel].Value) == false))
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
                else if ((System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Control) && (Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[sel].Value) == true))
                {
                    for (int i = e.RowIndex; i >= 0; i--)
                    {
                        if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[sel].Value) == false)
                        {
                            break;
                        }
                        else
                        {
                            dataGridView1.Rows[i].Cells[sel].Value = false;
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
            dataGridView5.Visible = (tabControl1.SelectedIndex == 3);

            // При переключении  tabControl1 необходимо получить модель выбранного датчика
            // для обновления cmbox и др элементов
            string SelectModel;   /// модель выбранного датчика
            string SelectType;   /// тип выбранного датчика
            SelectedLevel = 1;

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    {
                        pUpStatusBar.Visible = false;
                        splitter1.Visible = false;
                        return;
                    }

                // ОКНО - ХАРАКТЕРИЗАЦИЯ    
                case 1:
                    {
                        cbCHlevel.SelectedIndex = SelectedLevel - 1;

                        pUpStatusBar.Visible = true;
                        splitter1.Visible = true;
                        UpDateSelectedChannal();


                        // Занесение данных из ДБ в listview
                        if (SensorsDB._сonnection.State == System.Data.ConnectionState.Open)
                        {
                            int NumOfRange;
                            si = sensors.FindSensorGroup(SelectedLevel);
                            if (si >= 0)
                            {
                                SelectModel = new String(sensors.sensorList[si].PressureModel);
                                SensorAbsPressuer = (SelectModel[1] == '0');
                                SelectType = sensors.sensorList[si].GetdevType();

                                // Определяем количество диапазонов у датчика                          
                                NumOfRange = Convert.ToInt16(SensorsDB.GetDataSensors(SelectType, SelectModel, "NumOfRange"));

                                if ((NumOfRange != 1) && (NumOfRange != 2))
                                {
                                    cbDiapazon1.SelectedIndex = -1;
                                    cbDiapazon1.Enabled = false;
                                }
                                else
                                {
                                    cbDiapazon1.SelectedIndex = 0;
                                    cbDiapazon1.Enabled = (NumOfRange > 1);
                                }
                            }
                        }
                        return;
                    }

                // ОКНО - ВЕРИФИКАЦИЯ
                case 2:
                    {
                        cbVRlevel.SelectedIndex = SelectedLevel - 1;

                        pUpStatusBar.Visible = true;
                        //label1.Visible = true;
                        //tbNumCH.Visible = true;
                        splitter1.Visible = false;
                        UpDateSelectedChannal();


                        // Занесение данных из ДБ в combobox                     
                        if (SensorsDB._сonnection.State == System.Data.ConnectionState.Open)
                        {

                            // УРОВЕНЬ
                            si = sensors.FindSensorGroup(SelectedLevel);
                            if (si >= 0)
                            {
                                SelectModel = new String(sensors.sensorList[si].PressureModel);
                                SensorAbsPressuer = (SelectModel[1] == '0');
                                SelectType = sensors.sensorList[si].GetdevType();
                                cbVRDiapazon1.Items.Clear();


                                // Определяем количество диапазонов у датчика                               
                                //NumOfRange = Convert.ToInt16(SensorsDB.GetDataSensors(SelectType, SelectModel, "NumOfRange"));
                                // Считываем НПИ/ВПИ для трех диапазонов верификации и формируем данные для combobox 
                                string NPI_VPI_diap1 = SensorsDB.GetDataSensors(SelectType, SelectModel, "VerNPIPoint1") + ".." + SensorsDB.GetDataSensors(SelectType, SelectModel, "VerVPIPoint1");
                                if (NPI_VPI_diap1.Length > 3)
                                    cbVRDiapazon1.Items.Add(NPI_VPI_diap1);

                                string NPI_VPI_diap2 = SensorsDB.GetDataSensors(SelectType, SelectModel, "VerNPIPoint2") + ".." + SensorsDB.GetDataSensors(SelectType, SelectModel, "VerVPIPoint2");
                                if (NPI_VPI_diap2.Length > 3)
                                    cbVRDiapazon1.Items.Add(NPI_VPI_diap2);

                                string NPI_VPI_diap3 = SensorsDB.GetDataSensors(SelectType, SelectModel, "VerNPIPoint3") + ".." + SensorsDB.GetDataSensors(SelectType, SelectModel, "VerVPIPoint3");
                                if (NPI_VPI_diap3.Length > 3)
                                    cbVRDiapazon1.Items.Add(NPI_VPI_diap3);


                                if (cbVRDiapazon1.Items.Count <= 0)
                                {
                                    cbVRDiapazon1.SelectedIndex = -1;
                                    cbVRDiapazon1.Enabled = false;
                                }
                                else
                                {
                                    cbVRDiapazon1.SelectedIndex = 0;
                                }

                            }
                        }
                        return;
                    }
                // ОКНО - Сдача Метрологу
                case 3:
                    {
                        // УРОВЕНЬ - 1
                        int NumOfRange = -1;
                        string SensParam;
                        si = sensors.FindSensorGroup(1);
                        if (si >= 0)
                        {
                            SelectModel = new String(sensors.sensorList[si].PressureModel);
                            SensorAbsPressuer = (SelectModel[1] == '0');
                            SelectType = sensors.sensorList[si].GetdevType();
                            cb_MET_Unit.Text = sensors.sensorList[si].GetUnit();

                            // Определяем количество диапазонов у датчика                               
                            NumOfRange = Convert.ToInt16(SensorsDB.GetDataSensors(SelectType, SelectModel, "NumOfRange"));

                            if ((NumOfRange != 1) && (NumOfRange != 2))
                            {
                                cbVRDiapazon1.SelectedIndex = -1;
                                cbVRDiapazon1.Enabled = false;
                            }
                            else
                            {
                                // Занесение данных о диапазоне температур и давлений из БД в listbobox
                                SensParam = SensorsDB.GetDataSensors(SelectType, SelectModel, "VerPressPoint1"); // функция запроса данных из БД по номеру модели и параметру
                                if (SensParam != "")
                                {
                                    string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    lb_MET_PressValue.Items.Clear();
                                    lb_MET_PressValue.Items.AddRange(SPcmbox);
                                    lb_MET_PressValue.SelectedIndex = 0;
                                }
                            }
                        }
                        else
                        {
                            cb_MET_Unit.Text = "кПа";
                        }

                        pUpStatusBar.Visible = true;
                        splitter1.Visible = false;
                        UpDateSelectedChannal();
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
            if (ResultMET != null)
                ResultMET.CloseAll();

            //Подготавливаем заводские номера по каналам
            int[] FN = new int[MaxChannalCount];
            byte[] Type = new byte[MaxChannalCount];
            string[] Model = new string[MaxChannalCount];
            for (int i = 0; i < MaxChannalCount; i++)
            {
                if (sensors.SelectSensor(i))
                {
                    FN[i] = (int)sensors.sensor.uni;
                    Type[i] = sensors.sensor.devType;
                    Model[i] = new string(sensors.sensor.PressureModel);
                }
                else
                {
                    FN[i] = 0;
                    Type[i] = 0;
                    Model[i] = "";
                }
            }

            //***************** создаем файлы результатов характеризации *******************************
            ResultCH = new СResultCH(MaxChannalCount, FN, sensors.COEFF_COUNT, Type, Model);//результаты характеризации датчиков
                                                                                            // ResultCH.SetSensorInfo();
            ResultCH.LoadFromFile();

            //***************** создаем файлы результатов калибровки ***********************************
            ResultCI = new CResultCI(MaxChannalCount, FN);//результаты калибровки датчиков
            ResultCI.LoadFromFile();

            //***************** создаем файлы результатов верификации ***********************************
            ResultVR = new CResultVR(MaxChannalCount, FN, Type, Model);
            ResultVR.LoadFromFile();
            //*******************************************************************************************

            //***************** создаем файлы результатов сдачи метрологу ***********************************
            ResultMET = new CResultMET(MaxChannalCount, FN, Type, Model);
            ResultMET.LoadFromFile();
            //*******************************************************************************************

        }





        // Отработка нажатия на гл. форме МЕНСОР - ЗАДАЧА
        private void bMensorControl_Click(object sender, EventArgs e)
        {

            switch (selectedPressurer)
            {
                case 0: // менсор
                    {
                        if (Mensor._serialPort_M.IsOpen)
                        {
                            // если уставка равна нулю сбрсываем давление, если нет то выполняем команду задача
                            //if (numMensorPoint.Value == 0)
                            //{
                            //    Mensor.SetMode(2);
                            //    bMensorControl.BackColor = Color.LightGreen;
                            //}
                            //else
                            //{
                            Mensor.SetMode(1);
                            bMensorControl.BackColor = Color.LightGreen;
                            //}

                        }
                        else
                        {
                            Program.txtlog.WriteLineLog("Нет Связи. Задатчик давления не подключен", 1);
                        }
                        break;
                    }

                case 1: // паскаль
                    {
                        if (Pascal.Port.IsOpen)
                        {

                            Pascal.SetModeKeyStart();
                            bMensorControl.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            Program.txtlog.WriteLineLog("Нет Связи. Задатчик давления не подключен", 1);
                        }
                        break;
                    }

                case 2: // элемер
                    {
                        if (Elemer.Port.IsOpen)
                        {

                            Elemer.SetModeKeyStart();
                            if (Elemer.modeStartReg)
                            {
                                bMensorControl.BackColor = Color.LightGreen;
                            }
                            else
                            {
                                Program.txtlog.WriteLineLog("Элемер: не удалось включить регулирование давления", 1);
                                bMensorControl.BackColor = Color.Transparent;
                            }
                            
                        }
                        else
                        {
                            Program.txtlog.WriteLineLog("Нет Связи. Задатчик давления не подключен", 1);
                        }
                        break;
                    }

            }

        }


        // Отработка нажатия на гл. форме МЕНСОР - СБРОС
        private void button7_Click(object sender, EventArgs e)
        {

            switch (selectedPressurer)
            {
                case 0: // менсор
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
                        break;
                    }

                case 1: // паскаль
                    {
                        if (Pascal.Port.IsOpen)
                        {
                            bMensorVent.BackColor = Color.LightGreen;
                            Pascal.SetModeVent();

                        }
                        else
                        {
                            Program.txtlog.WriteLineLog("Нет Связи. Задатчик давления не подключен", 1);
                        }
                        break;
                    }

                case 2: // элемер
                    {
                        if (Elemer.Port.IsOpen)
                        {

                            Elemer.SetClearP();                            
                            if (Elemer.modeClearP)
                            {
                                bMensorVent.BackColor = Color.LightGreen;
                            }
                            else
                            {
                                Program.txtlog.WriteLineLog("Элемер: не удалось включить Сброс", 1);
                                bMensorVent.BackColor = Color.Transparent;
                            }

                        }
                        else
                        {
                            Program.txtlog.WriteLineLog("Нет Связи. Задатчик давления не подключен", 1);
                        }
                        break;
                    }

            }
            
        }


        // Отработка нажатия на гл. форме МЕНСОР - ИЗМЕРЕНИЕ/ОБНУЛЕНИЕ
        private void bMensorMeas_Click(object sender, EventArgs e)
        {
            switch (selectedPressurer)
            {
                case 0: // менсор
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
                        break;
                    }

                case 1: // паскаль
                    {
                        if (Pascal.Port.IsOpen)
                        {
                            bMensorMeas.BackColor = Color.LightGreen;
                            Pascal.SetClearP();
                        }
                        else
                        {
                            Program.txtlog.WriteLineLog("Нет Связи. Задатчик давления не подключен", 1);
                        }
                        break;
                    }

                case 2: // элемер
                    {                        

                        if (Elemer.Port.IsOpen)
                        {                                                       
                            Elemer.SetModeKeyStop();
                            if (Elemer.modeClearP)
                            {
                                bMensorVent.BackColor = Color.LightGreen;
                            }
                            else
                            {
                                Program.txtlog.WriteLineLog("Элемер: не удалось отключить регулирование давления", 1);
                                bMensorVent.BackColor = Color.Transparent;
                            }

                        }
                        else
                        {
                            Program.txtlog.WriteLineLog("Нет Связи. Задатчик давления не подключен", 1);
                        }
                        break;
                    }
            }           
        }


        // Отработка выбора на гл.форме ТИПА ПРЕОБРАЗОВАТЕЛЯ МЕНСОРА из списка
        private void cbMensorTypeR_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            if (cb_ManualMode.Checked)
                return;

            if (UseMensor)
            {


                if (!Mensor._serialPort_M.IsOpen)
                {
                    if (cbMensorTypeR.SelectedIndex != -1)
                    {
                        Program.txtlog.WriteLineLog("Нет Связи. Задатчик давления не подключен", 1);
                        cbMensorTypeR.SelectedIndex = -1;
                    }
                    MainTimer.Enabled = true;
                    MainTimer.Start();
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

            else
            {


                if (!Pascal.Port.IsOpen)
                {
                    if (cbMensorTypeR.SelectedIndex != -1)
                    {
                        Program.txtlog.WriteLineLog("Нет Связи. Задатчик давления не подключен", 1);
                        cbMensorTypeR.SelectedIndex = -1;
                    }
                    MainTimer.Enabled = true;
                    MainTimer.Start();
                    return;
                }
                try
                {
                    MainTimer.Stop();
                    MainTimer.Enabled = false;
                                       
                    // Получаем индекс выбранного преобразователя
                    int ind = cbMensorTypeR.SelectedIndex + 1;

                    // Определяем тип канала соответствующего выбранному преобразователю
                    if ((ind > 0) && (ind <= Pascal.M1num))  // 
                    {
                        Pascal.SetModule(1, ind);
                    }
                    else if ((ind > Pascal.M1num) && (ind <= Pascal.M2num)) // 
                    {
                        Pascal.SetModule(2, ind - Pascal.M1num);
                    }

                }
                finally
                {
                    MainTimer.Enabled = true;
                    MainTimer.Start();
                }

            }*/
            
        }

        // Отработка нажания по заданию уставки OK
        private void button6_Click(object sender, EventArgs e)
        {

            if (cb_ManualMode.Checked)
            {
                tbMensorData.Text = Convert.ToString(numMensorPoint.Value);
            }
            else
            {

                switch (selectedPressurer)
                {
                    case 0: // менсор
                        {
                            if (!Mensor._serialPort_M.IsOpen)
                            {
                                Program.txtlog.WriteLineLog("Нет Связи. Задатчик давления не подключен", 1);
                                return;
                            }

                            double Point = (double)numMensorPoint.Value;  // получаем заданное значение уставки
                            Mensor.SetPoint(Point);

                            // если включена задача и уставка равна 0, то включаем режим вентиляции
                            //if ((Point == 0) && (Mensor._mode == 1))
                            //{
                            //    Mensor.SetMode(2);
                            //    bMensorControl.BackColor = Color.LightGreen;
                            //}
                            break;
                        }

                    case 1: // паскаль
                        {
                            if (!Pascal.Port.IsOpen)
                            {
                                Program.txtlog.WriteLineLog("Нет Связи. Задатчик давления не подключен", 1);
                                return;
                            }

                            double Point = (double)numMensorPoint.Value;  // получаем заданное значение уставки

                            // если установлено АБСОЛЮТНОЕ давление, то для Паскаля, от уставки отнимаем атмосферное давление
                            // которое задано в ГПа для перевода его в кПА нужно разделить на 10
                            if (rbPressABS.Checked)
                            {
                                Point = Point - Convert.ToDouble(numATMpress.Value);
                            }

                            Pascal.SetPress(Point);
                            break;
                        }

                    case 2: // элемер
                        {
                            if (!Elemer.Port.IsOpen)
                            {
                                Program.txtlog.WriteLineLog("Нет Связи. Задатчик давления не подключен", 1);
                                return;
                            }

                            double Point = (double)numMensorPoint.Value;  // получаем заданное значение уставки

                            /*
                            // если установлено АБСОЛЮТНОЕ давление, то для Паскаля, от уставки отнимаем атмосферное давление
                            // которое задано в ГПа для перевода его в кПА нужно разделить на 10
                            if (rbPressABS.Checked)
                            {
                                Point = Point - Convert.ToDouble(numATMpress.Value);
                            }
                            */
                           
                            //Elemer.SetPress(Convert.ToInt16(Point).ToString("X"), Point);
                            Elemer.SetPress(Point);
                            if(!Elemer.target)                             
                                Program.txtlog.WriteLineLog("Элемер: уставка в прибор не записана", 1);
                            break;
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






        //Старт характеризации
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

                try
                {

                    btnCHStart.Text = "Выполняется процесс характеризации ... Остановить?";
                    Program.txtlog.WriteLineLog("CH: Старт характеризации!", 2);
                    UpdateItemState(2);
                    if (AutoRegim)
                    {
                        if (lvCHPressureSet.Items.Count <= 0)
                        {
                            if (MessageBox.Show("Отсутсвуют точки давления. Продолжить характеризацию в ручную??", "Подтверждение команды", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                ReadSensorParametrs();
                            }
                        }
                        else
                        {
                            // Раскачка давлением
                            // Получаем Pmax датчика
                            int si = sensors.FindSensorGroup(SelectedLevel);
                            if ((si >= 0)&&(fPushPress))
                            {
                                string SelectModel = new String(sensors.sensorList[si].PressureModel);
                                string SelectType = sensors.sensorList[si].GetdevType();
                                double Pmax_sens = Convert.ToDouble(SensorsDB.GetDataSensors(SelectType, SelectModel, "Pmax"));
                                // Запускаем раскачку
                                PushPress(Pmax_sens);
                                button7_Click(null, null);
                            }


                            for (i = 0; i < lvCHPressureSet.Items.Count; i++)
                            {
                                if (ProcessStop) break;//прекращаем 
                                lvCHPressureSet.Items[i].Selected = true;
                                btnCHPressureSet1_Click(null, null);
                                //btnCHPressureSet1.PerformClick();
                                //Application.DoEvents();
                                ReadSensorParametrs();
                            }
                            bMensorControl_Click(null, null);
                            button7_Click(null, null);
                        }
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("CH: Ожидаем установления давления в датчиках", 0);
                        TimerTickCount = 0;
                        do//ожидаем установления давления
                        {
                            if (ProcessStop) return;//прекращаем
                            Application.DoEvents();
                            Thread.Sleep(100);
                        } while (TimerTickCount < SENSOR_PRESSUER_WAIT / MainTimer.Interval);
                        ReadSensorParametrs();
                    }

                    Program.txtlog.WriteLineLog("CH: Операция характеризации завершена!", 2);

                }

                finally
                {
                    btnCHStart.Text = "Старт характеризации";
                    UpdateItemState(0);
                }
            }
            else
            {
                FormPause formpause = new FormPause();
                if (formpause.ShowDialog() != DialogResult.OK)
                //                    if (MessageBox.Show("Для продолжения нажмите 'Да'. Чтобы остановить характеризацию нажмите 'Нет'", "Характеризация поставлена на паузу", MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No)
                {
                    ProcessStop = true;
                    Program.txtlog.WriteLineLog("Операция прекращена пользователем", 0);
                }
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex == sel) && (dataGridView1.RowCount > 0))//выбор датчиков
            {
                bool CurentSet = Convert.ToBoolean(dataGridView1.Rows[0].Cells[sel].Value);
                for (int i = 0; i <= (dataGridView1.RowCount - 1); i++)
                {
                    dataGridView1.Rows[i].Cells[sel].Value = !CurentSet;
                    //                    dataGridView1.Rows[i].Cells[3].Style.BackColor = Color.White;
                }
            }
            if ((e.ColumnIndex == sen) && (dataGridView1.RowCount > 0))//выбор датчиков
            {
                if (MessageBox.Show("Отчистить список обнаруженных датчиков?", "Подтверждение операции", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    UpdateItems();

            }
        }

        private void cbChannalCharakterizator_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!SensorBusy)
            {
                if (cbChannalCharakterizator.Text == "") return;

                string str = cbChannalCharakterizator.Text.Remove(0, 6);
                int i = Convert.ToInt32(str) - 1;

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

                    label_UpStVoltage.Text = sensors.sensor.OutVoltage.ToString("f4");//4 знака
                    label_UpStResistance.Text = sensors.sensor.Resistance.ToString("f2");//2 знака
                    label_UpStPressure.Text = sensors.sensor.Pressure.ToString("f4");

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
            string strValue = "";
            if (lvCHTermoCamera.SelectedItems.Count > 0)
            {
                strValue = lvCHTermoCamera.SelectedItems[0].Text;
            }
            /*
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
            }*/
            if (strValue == "")
            {
                MessageBox.Show("Выбирите значение температуры", "Не задана температура в термокамере");
                return;
            }

            if (ThermalCamera.Connected)
            {
                numTermoCameraPoint.Text = strValue;
                Program.txtlog.WriteLineLog("Температура задана. Ожидаем завершение стабилизации показаний.", 0);
                TemperatureReady = true;
                MessageBox.Show("Температура установлена.", "Успешное завершение операции");
            }
            else
            {
                Program.txtlog.WriteLineLog("CH: Нет cвязи c термокамерой.", 1);
                //  if (MessageBox.Show("Хотите установить температуру в ручную?", "Нет соединения с термокамерой", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    numTermoCameraPoint.Text = strValue;
                    TemperatureReady = true;
                }
            }
        }


        //Установка давления для характеризации группы 
        private void btnCHPressureSet1_Click(object sender, EventArgs e)
        {
            // bool SenorAbsPressuer = false;
            string strValue = "";
            if (lvCHPressureSet.SelectedItems.Count > 0)
            {
                strValue = lvCHPressureSet.SelectedItems[0].Text;
            }

            if (strValue == "")
            {
                MessageBox.Show("Введите значение давления в кПа", "Не установлено давление в задатчике");
                return;
            }

            if ((Mensor.Connected) || (Pascal.Connected) || (Elemer.Connected))
            {
                double Point = 0;
                double shift = 0;
                try
                {
                    btnCHPressureSet1.Enabled = false;

                    Point = double.Parse(strValue.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                    Program.txtlog.WriteLineLog("CH: Устанавливаем давление в датчиках " + Point.ToString() + "кПа", 0);
                    numMensorPoint.Text = strValue;

                    // если установлено АБСОЛЮТНОЕ давление, то для Паскаля и Элемера, от уставки отнимаем атмосферное давление
                    // которое задано в ГПа для перевода его в кПА нужно разделить на 10
                    if ((selectedPressurer >= 1) && (rbPressABS.Checked))
                    {
                        Point = Point - Convert.ToDouble(numATMpress.Value);
                    }
                     

                    if ((Point == 0) && !SensorAbsPressuer)
                    {
                        button7_Click(null, null);
                    }
                    else
                    {
                        button6_Click(null, null);      //выставляем давление
                        Thread.Sleep(2000);
                        // доработка 09.08.2020 / на зам. №4 от 06.08.2020 
                        // если задача давления включена, то повторно ее не включаем 
                        // UseMensor = true - используем Менсор
                        // UseMensor = false - используем Паскаль
                        // Pascal.modeStart = true/false - задача ВКЛЮЧЕНА/ВЫКЛЮЧЕНА (ПАСКАЛЬ)
                        // Mensor._mode = 0 / 1 / 2 - режимы ИЗМ. / ЗАДАЧА / СБОРС 
                        //

                        //if ((!UseMensor && !Pascal.modeStart) || (UseMensor && Mensor._mode != 1))
                        if (((selectedPressurer == 1) && !Pascal.modeStart) || ((selectedPressurer == 0) && Mensor._mode != 1) || ((selectedPressurer == 2) && !Elemer.modeStartReg))
                        {
                            bMensorControl_Click(null, null);  //запускаем задачу
                        }
                    }

                    TimerTickCount = 0;
                    do//ожидаем установления давления
                    {
                        if (ProcessStop) return;//прекращаем
                        Application.DoEvents();
                        Thread.Sleep(100);
                        shift = Math.Abs(Convert.ToDouble(tbMensorData.Text) - Point);
                    } while ((shift > SKO_PRESSURE) && (TimerTickCount < MENSOR_PRESSUER_WAIT / MainTimer.Interval));
                    if (TimerTickCount >= MENSOR_PRESSUER_WAIT / MainTimer.Interval)
                    {//давление не установлено
                        Program.txtlog.WriteLineLog("CH: Истекло время установки давления в датчиках", 1);
                    }
                    else
                    {//давление установлено
                        TimerTickCount = 0;
                        do//ожидаем установления давления
                        {
                            if (ProcessStop) return;//прекращаем
                            Application.DoEvents();
                            Thread.Sleep(100);
                        } while (TimerTickCount < SENSOR_PRESSUER_WAIT / MainTimer.Interval);
                        PressureReady = true;
                        Program.txtlog.WriteLineLog("CH: Давление в датчиках установлено.", 0);
                    }
                }
                finally
                {
                    btnCHPressureSet1.Enabled = true;
                }
            }
            else
            {

                Program.txtlog.WriteLineLog("CH: Нет cвязи c задатчиком давления.", 1);
                if (MessageBox.Show("Хотите установить давление " + strValue + "кПа в ручную?", "Нет соединения с задатчиком давления", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    numMensorPoint.Text = strValue;
                    PressureReady = true;

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
                        btnReadCAP.Text = "Выполняется процесс чтения ЦАП... Остановить?";
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
                FormPause formpause = new FormPause();
                if (formpause.ShowDialog() != DialogResult.OK)
                //                    if (MessageBox.Show("Отменить текущую операцию?", "Подтверждение команды", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
                    btnCalibrateCurrent.Text = "Выполняется калибровка... Остановить?";
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
                FormPause formpause = new FormPause();
                if (formpause.ShowDialog() != DialogResult.OK)

                //                    if (MessageBox.Show("Отменить текущую операцию?", "Подтверждение команды", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
            if (Settings.ShowDialog() == DialogResult.OK)
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
                MainTimer.Stop();
                MainTimer.Enabled = false;

                // Общие настройки программы
                MAIN_TIMER = Properties.Settings.Default.set_MainTimer;                     // Общий интервал опроса
                MainTimer.Interval = MAIN_TIMER;

                MAX_ERROR_COUNT = Properties.Settings.Default.set_MaxErrorCount;            //Количество ошибок чтения данных с устройств перед отключением
                MIN_SENSOR_CURRENT = Properties.Settings.Default.set_MinSensorCurrent;      //минимльный ток датчика для обнаружения, мА
                MAX_COUNT_CAP_READ = Properties.Settings.Default.set_MaxCountCAPRead;       //максимальное количество циклов чтения тока ЦАП
                SKO_CURRENT = Properties.Settings.Default.set_SKOCurrent;                   //допуск по току ЦАП датчика до калибровки, мА
                SKO_CALIBRATION_CURRENT = Properties.Settings.Default.set_SKOCalibrationCurrent; //допуск по току ЦАП после калибровки, мА            
                Multimetr.REZISTOR = Properties.Settings.Default.set_Rezistor;              // сопротивление резистора
                CCalculation.flag_ObrHod = Properties.Settings.Default.set_flagObrHod;
                CCalculation.flag_MeanR = Properties.Settings.Default.set_MeanR;            // усреднять или нет матрицу сопротивлений
                AutoRegim = Properties.Settings.Default.set_AutoRegim;                      // автоматический режим
                TrhDeviation = Properties.Settings.Default.set_Deviation;                   // порог по отклонению R^2
                fPushPress = Properties.Settings.Default.set_PushPress;


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
                MAX_COUNT_POINT = (Properties.Settings.Default.set_MensorMaxCountPoint * 1000) / MAIN_TIMER + 1;        //ожидание стабилизации давления в датчике, в циклах таймера
                SENSOR_PRESSUER_WAIT = (Properties.Settings.Default.set_MensorMaxCountPoint * 1000);
                SKO_PRESSURE = Properties.Settings.Default.set_MensorSKOPressure;           //(СКО) допуск по давлению, кПа


                sensors.WAIT_TIMEOUT = Properties.Settings.Default.set_SensWaitTimeout;     //таймаут ожидания ответа от датчика
                sensors.WRITE_COUNT = Properties.Settings.Default.set_SensReadCount;        //число попыток записи команд в датчик
                sensors.WRITE_PERIOD = Properties.Settings.Default.set_SensReadPause;       //период выдачи команд

                CCalcMNK.Kf = Properties.Settings.Default.set_Math_Kf;
                CCalcMNK.Kpmax_dop = Properties.Settings.Default.set_Math_Kmax_dop;
                //CCalcMNK.code = Properties.Settings.Default.set_Math_Code;
                CCalcMNK.Amax = Properties.Settings.Default.set_Math_Amax;
                CCalcMNK.Mmax = Properties.Settings.Default.set_Math_Mmax;
                CCalcMNK.Tnku = Properties.Settings.Default.set_Math_Tnku;
                CCalcMNK.KdM = Properties.Settings.Default.set_Math_KdM;
                CCalcMNK.deltaFdop_min = Properties.Settings.Default.set_Math_DFdop_min;
                //CCalcMNK.Fr_min = Properties.Settings.Default.set_Math_Fr_min;
                AlgorithmMNK = Properties.Settings.Default.set_Math_AlgorithmMNK;

                //если изменился задатчик давления после изменения настроек
                if (Properties.Settings.Default.set_selectPressurer != selectedPressurer)
                {
                    selectedPressurer = Properties.Settings.Default.set_selectPressurer;
                    // отключаем подключенный задатчик
                    if (Mensor.Connected)
                        Mensor.DisConnect();
                    if (Pascal.Connected)
                        Pascal.DisConnect();
                    if (Elemer.Connected)
                        Elemer.DisConnect();

                    // подключаем новый задатчик 
                    //btnMensor_Click(null, null);
                }   
               

                //gbBarometr.Visible = !UseMensor;
                if (MaxChannalCount != Commutator.MaxChannal)
                {
                    Commutator.SetMaxChannal(MaxChannalCount);
                    UpdateItems();//обновляем списки визуальных элементов
                }
            }
            catch
            {
                Program.txtlog.WriteLineLog("Не удалось задать настройки программы", 1);
            }
            finally
            {
                MainTimer.Enabled = true;
                MainTimer.Start();
            }
        }








        // ХАРАКТЕРИЗАЦИЯ
        // Выбор диапазона в combobox
        // и отбражение данный по точкам температуры и давлению из ДБ
        private void cbDiapazon1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SensorsDB._сonnection.State == System.Data.ConnectionState.Open)
            {
                lvCHPressureSet.Items.Clear();
                lvCHTermoCamera.Items.Clear();

                if (sensors.sensorList.Count > 0)
                {
                    string SelectModel = new String(sensors.sensorList[0].PressureModel);
                    string SelectType = sensors.sensorList[0].GetdevType();
                    string field = "";
                    string SensParam;
                    if (cbDiapazon1.SelectedIndex == 0)
                    {
                        field = "HarPressPoint1";
                    }
                    if (cbDiapazon1.SelectedIndex == 1)
                    {
                        field = "HarPressPoint2";
                    }

                    if (field != "")
                    {
                        SensParam = SensorsDB.GetDataSensors(SelectType, SelectModel, field); // функция запроса данных из БД по номеру модели и параметру
                        if (SensParam != null)
                        {
                            string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < SPcmbox.Length; i++)
                            {
                                ListViewItem item = new ListViewItem(SPcmbox[i]);
                                lvCHPressureSet.Items.Add(item);
                            }
                        }
                        else
                        {
                            Program.txtlog.WriteLineLog("CH: Точки давления из базы данных не загружены!", 1);
                        }
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("CH: Не задан диапазон характеризации", 1);
                    }

                    SensParam = SensorsDB.GetDataSensors(SelectType, SelectModel, "HarTempPoint1"); // функция запроса данных из БД по номеру модели и параметру
                    if (SensParam != null)
                    {
                        string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < SPcmbox.Length; i++)
                        {
                            ListViewItem item = new ListViewItem(SPcmbox[i]);
                            lvCHTermoCamera.Items.Add(item);
                        }
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("CH: Точки давления из базы данных не загружены!", 1);
                    }
                }
                else
                {
                    Program.txtlog.WriteLineLog("CH: Не выбранны датчики для характеризации", 1);
                }

            }
            else
            {
                Program.txtlog.WriteLineLog("CH: Нет доступа к базе данных!", 1);
            }

        }


        //Установка температуры в термокамере для верификации
        private void btnVRTemperatureSet1_Click(object sender, EventArgs e)
        {
            TemperatureReady = false;
            string strValue = "";
            if (lvVRTermoCamera.SelectedItems.Count > 0)
            {
                strValue = lvVRTermoCamera.SelectedItems[0].Text;
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
            if (lvVRPressureSet.SelectedItems.Count > 0)
            {
                strValue = lvVRPressureSet.SelectedItems[0].Text;
            }

            if (strValue == "")
            {
                MessageBox.Show("Введите значение давления в кПа", "Не задано давление в задатчике");
                return;
            }
            if ((Mensor.Connected) || (Pascal.Connected) || (Elemer.Connected))
            {
                double Point;
                double shift;
                try
                {
                    btnVRPressureSet1.Enabled = false;

                    Point = double.Parse(strValue.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                    Program.txtlog.WriteLineLog("VR: Устанавливаем давление в датчиках " + Point.ToString() + "кПа", 0);
                    //Point = Convert.ToDouble(strValue);// получаем заданное значение уставки
                    numMensorPoint.Text = strValue;

                    // если установлено АБСОЛЮТНОЕ давление, то для Паскаля и Элемера, от уставки отнимаем атмосферное давление
                    // которое задано в ГПа для перевода его в кПА нужно разделить на 10
                    if ((selectedPressurer >= 1) && (rbPressABS.Checked))
                    {
                        Point = Point - Convert.ToDouble(numATMpress.Value);
                    }



                    if ((Point == 0) && !SensorAbsPressuer)
                    {
                        button7_Click(null, null);
                    }
                    else
                    {
                        button6_Click(null, null);      //выставляем давление
                        Thread.Sleep(2000);
                        // доработка 09.08.2020 / на зам. №4 от 06.08.2020 
                        // если задача давления включена, то повторно ее не включаем 
                        // UseMensor = true - используем Менсор
                        // UseMensor = false - используем Паскаль
                        // Pascal.modeStart = true/false - задача ВКЛЮЧЕНА/ВЫКЛЮЧЕНА (ПАСКАЛЬ)
                        // Mensor._mode = 0 / 1 / 2 - режимы ИЗМ. / ЗАДАЧА / СБОРС 

                        //if ((!UseMensor && !Pascal.modeStart) || (UseMensor && Mensor._mode != 1))
                        if (((selectedPressurer == 1) && !Pascal.modeStart) || ((selectedPressurer == 0) && Mensor._mode != 1) || ((selectedPressurer == 2) && !Elemer.modeStartReg))
                        {
                            bMensorControl_Click(null, null);  //запускаем задачу
                        }

                    }

                    TimerTickCount = 0;
                    do//ожидаем установления давления
                    {
                        if (ProcessStop) return;//прекращаем
                        Application.DoEvents();
                        Thread.Sleep(100);
                        shift = Math.Abs(Convert.ToDouble(tbMensorData.Text) - Point);
                    } while ((shift > SKO_PRESSURE) && (TimerTickCount < MENSOR_PRESSUER_WAIT / MainTimer.Interval));
                    if (TimerTickCount >= MENSOR_PRESSUER_WAIT / MainTimer.Interval)
                    {//давление не установлено
                        Program.txtlog.WriteLineLog("VR:Истекло время установки давления в датчиках", 1);
                    }
                    else
                    {//давление установлено
                        TimerTickCount = 0;
                        do//ожидаем установления давления
                        {
                            if (ProcessStop) return;//прекращаем
                            Application.DoEvents();
                            Thread.Sleep(100);
                        } while (TimerTickCount < SENSOR_PRESSUER_WAIT / MainTimer.Interval);
                        PressureReady = true;
                        //                        btnVRParamRead.BackColor = Color.LightGreen;
                        //                        MessageBox.Show("Давление установлено.", "Успешное завершение операции");
                        Program.txtlog.WriteLineLog("VR: Давление установлено", 0);
                    }
                }
                finally
                {
                    btnVRPressureSet1.Enabled = true;
                }
            }
            else
            {
                Program.txtlog.WriteLineLog("VR: Нет cвязи c задатчиком давления.", 1);
                if (MessageBox.Show("Хотите установить давление в ручную?", "Нет соединения с задатчиком давления", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
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

                    btnVRParamRead.Text = "Выполняется процесс верификации ... Остановить?";
                    Program.txtlog.WriteLineLog("VR: Старт верификации!", 2);
                    UpdateItemState(6);
                    //ReadSensorPressure();

                    if (AutoRegim)
                    {
                        if (lvVRPressureSet.Items.Count <= 0)
                        {
                            if (MessageBox.Show("Отсутсвуют точки давления. Продолжить верификацию в ручную??", "Подтверждение команды", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                ReadSensorPressure();
                            }
                        }
                        else
                        {

                            // Раскачка давлением
                            // Получаем Pmax датчика
                            int si = sensors.FindSensorGroup(SelectedLevel);
                            if ((si >= 0)&&(fPushPress))
                            {
                                string SelectModel = new String(sensors.sensorList[si].PressureModel);
                                string SelectType = sensors.sensorList[si].GetdevType();
                                double Pmax_sens = Convert.ToDouble(SensorsDB.GetDataSensors(SelectType, SelectModel, "Pmax"));
                                // Запускаем раскачку
                                PushPress(Pmax_sens);
                                //button7_Click(null,null);//сброс
                            }





                            for (int j = 0; j < cbVRDiapazon1.Items.Count; j++)
                            {
                                button7_Click(null, null);//сброс
                                cbVRDiapazon1.SelectedIndex = j;
                                Program.txtlog.WriteLineLog("VR: Устанавливаем диапазон " + cbVRDiapazon1.Text + "кПа", 0);
                                // Задаем НПИ/ВПИ для трех диапазонов верификации
                                string[] npivpi = cbVRDiapazon1.Text.Split('.');
                                nud_VR_NPI.Value = Convert.ToDecimal(npivpi[0]);
                                nud_VR_VPI.Value = Convert.ToDecimal(npivpi[2]);
                                WriteSensorVPI_NPI();

                                try
                                {
                                    MainTimer.Stop();
                                    MainTimer.Enabled = false;
                                    // если используется Паскаль, то перед задачей выставляем модуль заданный в БД
                                    if ((selectedPressurer == 1) && (cbVRDiapazon1.Items.Count > 0))
                                    {
                                        if (cbMensorTypeR.Items.Count >= numPascaleModule)
                                            cbMensorTypeR.SelectedIndex = numPascaleModule;
                                        //Pascal.rangeModule = numPascaleModule;
                                        Application.DoEvents();
                                    }
                                }
                                finally
                                {
                                    MainTimer.Enabled = true;
                                    MainTimer.Start();
                                }

                                for (i = 0; i < lvVRPressureSet.Items.Count; i++)
                                {
                                    if (ProcessStop) break;//прекращаем 
                                    lvVRPressureSet.Items[i].Selected = true;
                                    btnVRPressureSet1_Click(null, null);
                                    //btnVRPressureSet1.PerformClick();
                                    ReadSensorPressure();
                                }
                            }

                            bMensorControl_Click(null, null);
                            button7_Click(null, null);
                        }
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("VR: Ожидаем установления давления в датчиках", 0);
                        TimerTickCount = 0;
                        do//ожидаем установления давления
                        {
                            if (ProcessStop) return;//прекращаем
                            Application.DoEvents();
                            Thread.Sleep(100);
                        } while (TimerTickCount < SENSOR_PRESSUER_WAIT / MainTimer.Interval);
                        ReadSensorPressure();
                    }
                    Program.txtlog.WriteLineLog("VR:Операция верификации завершена", 2);

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
                FormPause formpause = new FormPause();
                if (formpause.ShowDialog() != DialogResult.OK)
                //                    if (MessageBox.Show("Для продолжения нажмите 'Да'. Чтобы остановить верификацию нажмите 'Нет'", "Верификация поставлена на паузу", MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No)
                {
                    ProcessStop = true;
                    Program.txtlog.WriteLineLog("VR:Операция прекращена пользователем", 0);
                }
            }
        }


        //Загружаем точки давления и температур из БД для верификации
        private void cbVRDiapazon1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SensorsDB._сonnection.State == System.Data.ConnectionState.Open)
            {
                lvVRPressureSet.Items.Clear();
                lvVRTermoCamera.Items.Clear();

                if (sensors.sensorList.Count > 0)
                {
                    string SelectModel = new String(sensors.sensorList[0].PressureModel);
                    string SelectType = sensors.sensorList[0].GetdevType();
                    string field = "";
                    string fieldM = "";
                    if (cbVRDiapazon1.SelectedIndex == 0)
                    {
                        field = "VerPressPoint1";
                        fieldM = "VerModulePoint1";
                    }
                    if (cbVRDiapazon1.SelectedIndex == 1)
                    {
                        field = "VerPressPoint2";
                        fieldM = "VerModulePoint2";
                    }
                    if (cbVRDiapazon1.SelectedIndex == 2)
                    {
                        field = "VerPressPoint3";
                        fieldM = "VerModulePoint3";
                    }




                    if (field != "")
                    {
                        string SensParam = SensorsDB.GetDataSensors(SelectType, SelectModel, field); // функция запроса данных из БД по номеру модели и параметру
                        if (SensParam != null)
                        {
                            string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < SPcmbox.Length; i++)
                            {
                                ListViewItem item = new ListViewItem(SPcmbox[i]);
                                lvVRPressureSet.Items.Add(item);
                            }
                            strPascaleModule = SensorsDB.GetDataSensors(SelectType, SelectModel, fieldM); // функция запроса данных из БД по номеру модели и параметру
                            string[] str = strPascaleModule.Split(':');
                            if (str.Length > 0)
                            {
                                numPascaleModule = Convert.ToInt32(str[0]) - 1;
                            }
                            else
                            {
                                numPascaleModule = 0;
                            }
                        }
                        else
                        {
                            Program.txtlog.WriteLineLog("VR: Точки давления из базы данных не загружены!", 1);
                        }
                        SensParam = SensorsDB.GetDataSensors(SelectType, SelectModel, "VerTempPoint1"); // функция запроса данных из БД по номеру модели и параметру
                        if (SensParam != null)
                        {
                            string[] SPcmbox = SensParam.Split(new char[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < SPcmbox.Length; i++)
                            {
                                ListViewItem item = new ListViewItem(SPcmbox[i]);
                                lvVRTermoCamera.Items.Add(item);
                            }
                        }
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("VR: Не задан диапазон верификации", 1);
                    }
                }
                else
                {
                    Program.txtlog.WriteLineLog("VR: Не выбранны датчики для верификации", 1);
                }
            }
            else
            {
                Program.txtlog.WriteLineLog("VR: Нет доступа к базе данных!", 1);
            }
        }


        private void dataGridView2_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            ToolStripMenuDeleteResult_Click(sender, null);
        }

        private void ToolStripMenuDeleteResult_Click(object sender, EventArgs e)
        {
            if (ResultCH != null)
            {
                if (cbChannalCharakterizator.Text == "") return;

                string str = cbChannalCharakterizator.Text.Remove(0, 6);
                int ii = Convert.ToInt32(str) - 1;

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

                    for (int i = ResultCH.Channal[ii].Points.Count - 1; i >= 0; i--)
                    {
                        for (int j = 0; j < s.Count; j++)
                        {
                            if (i == s[j].Index)
                            {
                                ResultCH.DeletePoint(ii, i);
                                break;
                            }
                        }
                    }

                    dataGridView2.Sort(dataGridView2.Columns[0], ListSortDirection.Descending);
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
                int ii = Convert.ToInt32(str) - 1;

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

                    for (int i = ResultVR.Channal[ii].Points.Count - 1; i >= 0; i--)
                    {
                        for (int j = 0; j < s.Count; j++)
                        {
                            if (i == s[j].Index)
                            {
                                ResultVR.DeletePoint(ii, i);
                                break;
                            }
                        }
                    }
                    /*for (int i = 0; i < s.Count; i++)
                    {
                        ResultVR.DeletePoint(ii, s[i].Index);
                    }*/
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
            if (state == 0)
            {
                gbCommutator.Enabled = true;
                gbMensor.Enabled = true;
                gbMultimetr.Enabled = true;
                gbTermoCamera.Enabled = true;
                gbBarometr.Enabled = true;

                cbCHlevel.Enabled = true;
                cbDiapazon1.Enabled = true;
                cbVRlevel.Enabled = true;
                cbVRDiapazon1.Enabled = true;

                btnCHPressureSet1.Enabled = true;
                btnCHTemperatureSet1.Enabled = true;
                btnVRPressureSet1.Enabled = true;
                btnVRTemperatureSet1.Enabled = true;

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

                btn_MET_NPI_VPI.BackColor = Color.LightGreen;
                btn_MET_NPI_VPI.Enabled = true;
                btn_MET_Start.BackColor = Color.LightGreen;
                btn_MET_Start.Enabled = true;
                btn_MET_DTime.BackColor = Color.LightGreen;
                btn_MET_DTime.Enabled = true;
                btn_MET_Unit.BackColor = Color.LightGreen;
                btn_MET_Unit.Enabled = true;
                btn_MET_SetZero.BackColor = Color.LightGreen;
                btn_MET_SetZero.Enabled = true;

                btnCalculateDeviation.BackColor = Color.LightGreen;
                btnCalculateDeviation.Enabled = true;

                SensorBusy = false;
                ProcessStop = true;

                pbVRProcess.Value = 0;
                pbCHProcess.Value = 0;
            }
            else
            {
                gbCommutator.Enabled = false;
                gbMensor.Enabled = false;
                gbMultimetr.Enabled = false;
                gbTermoCamera.Enabled = false;
                gbBarometr.Enabled = false;

                cbCHlevel.Enabled = false;
                cbDiapazon1.Enabled = false;
                cbVRlevel.Enabled = false;
                cbVRDiapazon1.Enabled = false;

                btnCHPressureSet1.Enabled = false;
                btnCHTemperatureSet1.Enabled = false;
                btnVRPressureSet1.Enabled = false;
                btnVRTemperatureSet1.Enabled = false;

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

                btnVR_SetZero.BackColor = Color.IndianRed;
                btnVR_SetZero.Enabled = false;

                btn_MET_NPI_VPI.BackColor = Color.IndianRed;
                btn_MET_NPI_VPI.Enabled = false;
                btn_MET_Start.BackColor = Color.IndianRed;
                btn_MET_Start.Enabled = false;
                btn_MET_DTime.BackColor = Color.IndianRed;
                btn_MET_DTime.Enabled = false;
                btn_MET_Unit.BackColor = Color.IndianRed;
                btn_MET_Unit.Enabled = false;
                btn_MET_SetZero.BackColor = Color.IndianRed;
                btn_MET_SetZero.Enabled = false;

                btnCalculateDeviation.BackColor = Color.IndianRed;
                btnCalculateDeviation.Enabled = false;

                SensorBusy = true;
                ProcessStop = false;
            }
            switch (state)
            {
                case 1://поиск датчиков
                    btnSensorSeach.Enabled = true;
                    btnSensorSeach.BackColor = Color.LightGreen;
                    break;
                case 2://характеризация
                    btnCHStart.BackColor = Color.LightGreen;
                    btnCHStart.Enabled = true;
                    break;
                case 3://чтение ЦАП
                    btnReadCAP.BackColor = Color.LightGreen;
                    btnReadCAP.Enabled = true;
                    break;
                case 4://калибровка
                    btnCalibrateCurrent.BackColor = Color.LightGreen;
                    btnCalibrateCurrent.Enabled = true;
                    break;
                case 5://расчет коэффициентов
                    btnCalculateCoeff.BackColor = Color.LightGreen;
                    btnCalculateCoeff.Enabled = true;
                    break;
                case 6://верификация
                    btnVRParamRead.BackColor = Color.LightGreen;
                    btnVRParamRead.Enabled = true;
                    break;
                case 7://ВПИ НПИ
                    btnVR_VPI_NPI.BackColor = Color.LightGreen;
                    btnVR_VPI_NPI.Enabled = true;
                    break;
                case 8://Установка нуля
                    btnVR_SetZero.BackColor = Color.LightGreen;
                    btnVR_SetZero.Enabled = true;
                    break;
                case 9://расчет коэффициентов
                    btnCalculateCoeff.BackColor = Color.LightGreen;
                    btnCalculateCoeff.Enabled = true;
                    break;
                case 10://Сдача метрологу
                    btn_MET_Start.BackColor = Color.LightGreen;
                    btn_MET_Start.Enabled = true;
                    break;
                case 11://Установка времени демпфирования
                    btn_MET_DTime.BackColor = Color.LightGreen;
                    btn_MET_DTime.Enabled = true;
                    break;
                case 12://Установка единиц измерения
                    btn_MET_Unit.BackColor = Color.LightGreen;
                    btn_MET_Unit.Enabled = true;
                    break;
                case 13://Установка нуля
                    btn_MET_SetZero.BackColor = Color.LightGreen;
                    btn_MET_SetZero.Enabled = true;
                    break;
                case 14://НПи ВПИ
                    btn_MET_NPI_VPI.BackColor = Color.LightGreen;
                    btn_MET_NPI_VPI.Enabled = true;
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
                    Program.txtlog.WriteLineLog("CH: Не выбраны датчики для расчета коэффициентов. Операция прервана.", 0);
                    return;
                }

                //                if (TemperatureReady && PressureReady)
                //                {
                try
                {
                    btnCalculateCoeff.Text = "Выполняется расчет и запись коэффициентов ... Остановить?";
                    UpdateItemState(9);
                    СaclSensorCoeff();
                }
                catch
                {
                    Program.txtlog.WriteLineLog("CH: Критическая ошибка записи коэффициентов.", 1);
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
                FormPause formpause = new FormPause();
                if (formpause.ShowDialog() != DialogResult.OK)

                //                    if (MessageBox.Show("Отменить текущую операцию?", "Подтверждение команды", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ProcessStop = true;
                    Program.txtlog.WriteLineLog("CH: Операция прекращена пользователем", 0);
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
                    btnVR_VPI_NPI.Text = "Остановить";
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
                FormPause formpause = new FormPause();
                if (formpause.ShowDialog() != DialogResult.OK)

                //                    if (MessageBox.Show("Отменить запись ВПИ НПИ датчиков?", "Подтверждение команды", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
                    btnVR_SetZero.Text = "Остановить";
                    UpdateItemState(8);
                    SetZero();
                }
                finally
                {
                    btnVR_SetZero.Text = "Установка нуля";
                    UpdateItemState(0);

                }
            }
            else
            {
                FormPause formpause = new FormPause();
                if (formpause.ShowDialog() != DialogResult.OK)
                //                    if (MessageBox.Show("Отменить обнуление датчиков?", "Подтверждение команды", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ProcessStop = true;
                    Program.txtlog.WriteLineLog("VR:Операция прекращена пользователем", 0);
                }
            }


        }


        // Обнулить датчик на заданном канале коммутатора
        private void button1_Click(object sender, EventArgs e)
        {
            
            int i = Convert.ToInt16(tbNumCH.Text)-1;

            if (( i != 0) && (i < MaxChannalCount))
            {
                try
                {
                    ProcessStop = false;
                    if (sensors.SelectSensor(i))//выбор датчика на канале i
                    {
                        if (sensors.С43SetZero())
                        {
                            Program.txtlog.WriteLineLog("Выполнена установка нуля датчика в канале " + tbNumCH.Text, 0);
                        }
                        else
                        {
                            Program.txtlog.WriteLineLog("Установка нуля датчика не выполнена!", 1);
                        }
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("Датчик не найден в канале " + (i + 1).ToString(), 1);
                    }
                }
                catch
                {
                    Program.txtlog.WriteLineLog("Установка нуля датчика не выполнена!", 1);
                }
                finally
                {
                    ProcessStop = true;
                }
            }
            else
            {
                Program.txtlog.WriteLineLog("Установка нуля датчика не выполнена! Не выбран канал коммутатора!", 1);
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
                    dataGridView4.Sort(dataGridView4.Columns[0], ListSortDirection.Ascending);

                    for (int i = ResultCI.Channal[ii].Points.Count - 1; i >= 0; i--)
                    {
                        for (int j = 0; j < s.Count; j++)
                        {
                            if (i == s[j].Index)
                            {
                                ResultCI.DeletePoint(ii, i);
                                break;
                            }
                        }
                    }

                    UpdateCurrentGrid(ii);
                    ResultCI.SaveToArhiv(ii);
                }
            }

        }

        private void btnClockTimer_Click(object sender, EventArgs e)
        {
            if (dtpClockTimer.Enabled)
            {
                TimerValueSec = dtpClockTimer.Value;
                dtpClockTimer.Enabled = false;
            }
            else
            {
                dtpClockTimer.Enabled = true;
                btnClockTimer.BackColor = Color.Green;
            }
        }

        private void tsMenuItemAbout_Click(object sender, EventArgs e)
        {
            FormAbout fa = new FormAbout();
            fa.ShowDialog();
        }

        private void tsmiPanelVisible_Click(object sender, EventArgs e)
        {
            gbCommutator.Visible = tsmiPanelCommutator.Checked;
            gbMultimetr.Visible = tsmiPanelMultimetr.Checked;
            gbMensor.Visible = tsmiPanelMensor.Checked;
            gbTermoCamera.Visible = tsmiPanelTermocamera.Checked;
            panelLog.Visible = tsmiPanelLog.Checked;

            Properties.Settings.Default.set_CommutatorVisible = gbCommutator.Visible;
            Properties.Settings.Default.set_MultimetrVisible = gbMultimetr.Visible;
            Properties.Settings.Default.set_MensorVisible = gbMensor.Visible;
            Properties.Settings.Default.set_TermocameraVisible = gbTermoCamera.Visible;
            Properties.Settings.Default.set_PanelLogVisible = panelLog.Visible;

            Properties.Settings.Default.Save();  // Сохраняем переменные.*/
        }

        private void btn_MET_NPI_VPI_Click(object sender, EventArgs e)
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
                    Program.txtlog.WriteLineLog("MET: Не выбраны каналы для записи ВПИ НПИ датчиков. Операция прервана.", 0);
                    return;
                }

                try
                {
                    btn_MET_NPI_VPI.Text = "Остановить";
                    UpdateItemState(14);
                    WriteSensor_MET_VPI_NPI();
                }
                finally
                {
                    btn_MET_NPI_VPI.Text = "Задать";
                    UpdateItemState(0);
                }
            }
            else
            {
                FormPause formpause = new FormPause();
                if (formpause.ShowDialog() != DialogResult.OK)
                //                    if (MessageBox.Show("Отменить запись ВПИ НПИ датчиков?", "Подтверждение команды", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ProcessStop = true;
                    Program.txtlog.WriteLineLog("MET:Операция прекращена пользователем", 0);
                }
            }

        }


        //Установка единц измерения
        private void WriteSensor_MET_MesUnit()
        {
            int StartNumber = 0;    //начальный канал
            int FinishNumber = MaxChannalCount - 1;   //конечный канал
            string unitstr = cb_MET_Unit.Text;

            Program.txtlog.WriteLineLog("MET: Старт записи единицы измерения для выбранных датчиков ... ", 2);
            pbMETProcess.Maximum = FinishNumber - StartNumber;
            pbMETProcess.Minimum = 0;
            pbMETProcess.Value = 0;
            for (int i = StartNumber; i <= FinishNumber; i++)//перебор каналов
            {
                if (ProcessStop) return;//прекращаем  

                pbMETProcess.Value = i - StartNumber;
                Application.DoEvents();
                if (!CheckChannalEnable(i)) continue;//Если канал не выбран пропускаем обработку

                Commutator.SetConnectors(i, 0);
                Thread.Sleep(Commutator.READ_PERIOD);//ждем переключения

                if (sensors.SelectSensor(i))//выбор датчика на канале i
                {
                    if (sensors.С44WriteMesUnit(unitstr))
                    {
                        Program.txtlog.WriteLineLog("MET: Выполнена запись единицы измерения датчика в канале " + (i + 1).ToString(), 0);
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("MET: Запись НПИ ВПИ датчика не выполнена!", 1);
                    }
                }
                else
                {
                    Program.txtlog.WriteLineLog("MET: Датчик не найден в канале " + (i + 1).ToString(), 1);
                }
                //Commutator.SetConnectors(i, 1); // команда отключить датчик с индексом i                    

            }
            Program.txtlog.WriteLineLog("MET: Операция записи единицы измерения завершена", 2);
        }

        //Запись НПИ и ВПИ в выбранные датчики (метролог)
        private void WriteSensor_MET_VPI_NPI()
        {
            int StartNumber = 0;    //начальный канал
            int FinishNumber = MaxChannalCount - 1;   //конечный канал
            float VPI, NPI;
            VPI = Convert.ToSingle(nud_MET_VPI.Value);
            NPI = Convert.ToSingle(nud_MET_NPI.Value);

            Program.txtlog.WriteLineLog("MET: Старт записи НПИ ВПИ для выбранных датчиков ... ", 2);
            pbMETProcess.Maximum = FinishNumber - StartNumber;
            pbMETProcess.Minimum = 0;
            pbMETProcess.Value = 0;
            for (int i = StartNumber; i <= FinishNumber; i++)//перебор каналов
            {
                if (ProcessStop) return;//прекращаем верификацию 

                pbMETProcess.Value = i - StartNumber;
                Application.DoEvents();
                if (!CheckChannalEnable(i)) continue;//Если канал не выбран пропускаем обработку

                Commutator.SetConnectors(i, 0);
                Thread.Sleep(Commutator.READ_PERIOD);//ждем переключения

                if (sensors.SelectSensor(i))//выбор датчика на канале i
                {
                    int res = sensors.С35WriteVPI_NPI(VPI, NPI);
                    if (res >= 0)
                    {
                        Program.txtlog.WriteLineLog("MET: Выполнена запись НПИ ВПИ датчика в канале " + (i + 1).ToString(), 0);
                    }
                    else
                    {
                        if (res == -6)
                        {
                            Program.txtlog.WriteLineLog("MET: Неверные знаения НПИ ВПИ датчика. Команда не выполнена!", 1);
                        }
                        Program.txtlog.WriteLineLog("MET: Запись НПИ ВПИ датчика не выполнена!", 1);
                    }
                }
                else
                {
                    Program.txtlog.WriteLineLog("MET: Датчик не найден в канале " + (i + 1).ToString(), 1);
                }
                //Commutator.SetConnectors(i, 1); // команда отключить датчик с индексом i                    

            }
            Program.txtlog.WriteLineLog("MET: Операция записи НПИ ВПИ завершена", 2);

        }

        private void btn_MET_SetZero_Click(object sender, EventArgs e)
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
                    Program.txtlog.WriteLineLog("MET:Не выбраны каналы для обнуления датчиков. Операция прервана.", 0);
                    return;
                }

                try
                {
                    btn_MET_SetZero.Text = "Остановить";
                    UpdateItemState(13);
                    MET_SetZero();
                }
                finally
                {
                    btn_MET_SetZero.Text = "Установка нуля";
                    UpdateItemState(0);
                }
            }
            else
            {
                FormPause formpause = new FormPause();
                if (formpause.ShowDialog() != DialogResult.OK)

                //                if (MessageBox.Show("Отменить обнуление датчиков?", "Подтверждение команды", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ProcessStop = true;
                    Program.txtlog.WriteLineLog("MET:Операция прекращена пользователем", 0);
                }
            }
        }
        //Установка нуля для датчиков (метролог)
        private void MET_SetZero()
        {
            if ((!SensorAbsPressuer) && (Math.Abs(Convert.ToDouble(tbMensorData.Text)) > 0.5))
            {
                if (MessageBox.Show("Текущее давление не равно нулю. Продолжить установку нуля?", "Подтверждение команды", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    Program.txtlog.WriteLineLog("Операция прекращена.", 0);
                    return;
                }
            }
            int StartNumber = 0;    //начальный канал
            int FinishNumber = MaxChannalCount - 1;   //конечный канал

            Program.txtlog.WriteLineLog("MET: Установка нуля для выбранных датчиков ... ", 2);
            pbMETProcess.Maximum = FinishNumber - StartNumber;
            pbMETProcess.Minimum = 0;
            pbMETProcess.Value = 0;
            for (int i = StartNumber; i <= FinishNumber; i++)//перебор каналов
            {
                if (ProcessStop) return;//прекращаем верификацию 

                pbMETProcess.Value = i - StartNumber;
                Application.DoEvents();
                if (!CheckChannalEnable(i)) continue;//Если канал не выбран пропускаем обработку

                Commutator.SetConnectors(i, 0);
                Thread.Sleep(Commutator.READ_PERIOD);//ждем переключения

                if (sensors.SelectSensor(i))//выбор датчика на канале i
                {
                    if (sensors.С43SetZero())
                    {

                        Program.txtlog.WriteLineLog("MET: Выполнена установка нуля датчика в канале " + (i + 1).ToString(), 0);
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("MET: Установка нуля датчика не выполнена!", 1);
                    }
                }
                else
                {
                    Program.txtlog.WriteLineLog("MET: Датчик не найден в канале " + (i + 1).ToString(), 1);
                }
                //Commutator.SetConnectors(i, 1); // команда отключить датчик с индексом i                    

            }
            Program.txtlog.WriteLineLog("MET: Операция установки нуля завершена", 2);
        }


        //Установка времени демпфирования для датчиков (метролог)
        private void MET_SetDTime()
        {
            int StartNumber = 0;    //начальный канал
            int FinishNumber = MaxChannalCount - 1;   //конечный канал
            float DTime = Convert.ToSingle(nud_MET_DTime.Value);

            Program.txtlog.WriteLineLog("MET: Установка времени демпфирования для выбранных датчиков ... ", 2);
            pbMETProcess.Maximum = FinishNumber - StartNumber;
            pbMETProcess.Minimum = 0;
            pbMETProcess.Value = 0;
            for (int i = StartNumber; i <= FinishNumber; i++)//перебор каналов
            {
                if (ProcessStop) return;//прекращаем верификацию 

                pbMETProcess.Value = i - StartNumber;
                Application.DoEvents();
                if (!CheckChannalEnable(i)) continue;//Если канал не выбран пропускаем обработку

                Commutator.SetConnectors(i, 0);
                Thread.Sleep(Commutator.READ_PERIOD);//ждем переключения

                if (sensors.SelectSensor(i))//выбор датчика на канале i
                {
                    if (sensors.С34WriteDTime(DTime))
                    {

                        Program.txtlog.WriteLineLog("MET: Выполнена установка времени демпфирования в канале " + (i + 1).ToString(), 0);
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("MET: Установка времени демпфирования датчика не выполнена!", 1);
                    }
                }
                else
                {
                    Program.txtlog.WriteLineLog("MET: Датчик не найден в канале " + (i + 1).ToString(), 1);
                }
                //Commutator.SetConnectors(i, 1); // команда отключить датчик с индексом i                    

            }
            Program.txtlog.WriteLineLog("MET: Операция установки времени демпфирования завершена", 2);
        }
        private void btn_MET_Add_Click(object sender, EventArgs e)
        {
            FormInput forminput = new FormInput();
            if (forminput.ShowDialog() == DialogResult.OK)
            {
                lb_MET_PressValue.Items.Add(forminput.Pressuer);
            }
        }

        private void btn_MET_Del_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Удалить выбранное значение из списка?", "Подтверждение операции", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int index = lb_MET_PressValue.SelectedIndex;
                lb_MET_PressValue.Items.RemoveAt(index);
            }
        }

        private void btn_MET_Up_Click(object sender, EventArgs e)
        {
            int index = lb_MET_PressValue.SelectedIndex;
            if ((index > 0) && (index < lb_MET_PressValue.Items.Count))
            {
                int index2 = index - 1;

                string str = lb_MET_PressValue.Items[index].ToString();
                string str2 = lb_MET_PressValue.Items[index2].ToString();

                lb_MET_PressValue.Items.RemoveAt(index);
                lb_MET_PressValue.Items.Insert(index, str2);

                lb_MET_PressValue.Items.RemoveAt(index2);
                lb_MET_PressValue.Items.Insert(index2, str);
                lb_MET_PressValue.SelectedIndex = index2;
            }
        }

        private void btn_MET_Down_Click(object sender, EventArgs e)
        {
            int index = lb_MET_PressValue.SelectedIndex;
            if ((index >= 0) && (index < lb_MET_PressValue.Items.Count - 1))
            {
                int index2 = index + 1;

                string str = lb_MET_PressValue.Items[index].ToString();
                string str2 = lb_MET_PressValue.Items[index2].ToString();

                lb_MET_PressValue.Items.RemoveAt(index);
                lb_MET_PressValue.Items.Insert(index, str2);

                lb_MET_PressValue.Items.RemoveAt(index2);
                lb_MET_PressValue.Items.Insert(index2, str);

                lb_MET_PressValue.SelectedIndex = index2;
            }
        }

        private void btn_MET_DTime_Click(object sender, EventArgs e)
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
                    Program.txtlog.WriteLineLog("MET:Не выбраны каналы для установки времени демпфирования. Операция прервана.", 0);
                    return;
                }

                try
                {
                    btn_MET_DTime.Text = "Остановить";
                    UpdateItemState(11);
                    MET_SetDTime();
                }
                finally
                {
                    btn_MET_DTime.Text = "Задать";
                    UpdateItemState(0);
                }
            }
            else
            {
                FormPause formpause = new FormPause();
                if (formpause.ShowDialog() != DialogResult.OK)

                //                if (MessageBox.Show("Остановить установку времени демпфирования датчиков?", "Подтверждение команды", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ProcessStop = true;
                    Program.txtlog.WriteLineLog("MET:Операция прекращена пользователем", 0);
                }
            }
        }

        private void btn_MET_Start_Click(object sender, EventArgs e)
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
                    Program.txtlog.WriteLineLog("MET: Не выбраны каналы с датчиками. Операция прервана.", 0);
                    return;
                }

                try
                {
                    btn_MET_Start.Text = "Выполняется опрос датчиков ... Остановить?";
                    UpdateItemState(10);

                    if (AutoRegim)
                    {
                        for (int l = 0; l < lb_MET_PressValue.Items.Count; l++)
                        {
                            if (ProcessStop) break;
                            if (MensorSetPressuer(lb_MET_PressValue.Items[l].ToString(), l == 0) == 0)
                                MET_ReadSensorParametrs();
                        }
                        numMensorPoint.Text = "0";
                        bMensorControl_Click(null, null);
                        button7_Click(null, null);
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("MET: Ожидаем установления давления в датчиках", 0);
                        TimerTickCount = 0;
                        do//ожидаем установления давления
                        {
                            if (ProcessStop) return;//прекращаем
                            Application.DoEvents();
                            Thread.Sleep(100);
                        } while (TimerTickCount < SENSOR_PRESSUER_WAIT / MainTimer.Interval);
                        MET_ReadSensorParametrs();
                    }
                }
                finally
                {
                    btn_MET_Start.Text = "Старт";
                    UpdateItemState(0);
                }
            }
            else
            {
                FormPause formpause = new FormPause();
                if (formpause.ShowDialog() != DialogResult.OK)

                //if (MessageBox.Show("Для продолжения нажмите 'Да'. Чтобы остановить операцию нажмите 'Нет'", "Операция поставлена на паузу", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    ProcessStop = true;
                    Program.txtlog.WriteLineLog("MET: Операция прекращена пользователем", 0);
                }
            }
        }

        //Сдача метрологу
        //чтение всех измеренных параметров с выбранных датчиков давления
        private void MET_ReadSensorParametrs()
        {
            int seli = 0;
            int StartNumber = 0;    //начальный канал
            int FinishNumber = MaxChannalCount - 1;   //конечный канал
            float VPI, NPI;
            VPI = Convert.ToSingle(nud_MET_VPI.Value);
            NPI = Convert.ToSingle(nud_MET_NPI.Value);

            Program.txtlog.WriteLineLog("MET: Старт операции для выбранных датчиков ... ", 2);

            pbMETProcess.Maximum = FinishNumber - StartNumber;
            pbMETProcess.Minimum = 0;
            pbMETProcess.Value = 0;
            for (int i = StartNumber; i <= FinishNumber; i++)//перебор каналов
            {
                if (ProcessStop) return;//прекращаем верификацию 

                pbMETProcess.Value = i - StartNumber;
                Application.DoEvents();
                if (!CheckChannalEnable(i)) continue;//Если канал не выбран пропускаем обработку

                Commutator.SetConnectors(i, 0);
                Thread.Sleep(Commutator.READ_PERIOD);//ждем переключения


                if (sensors.SelectSensor(i))//выбор датчика на канале i
                {
                    if (sensors.С15ReadVPI_NPI())
                    {
                        Program.txtlog.WriteLineLog("MET: Выполнено чтение НПИ ВПИ датчика в канале " + (i + 1).ToString(), 0);
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("MET: Ошибка чтения НПИ ВПИ датчика в канале " + (i + 1).ToString(), 1);
                    }


                    if (sensors.SensorValueReadC03())
                    {
                        Thread.Sleep(Multimetr.WAIT_READY);//ждем измерения мультиметром

                        float Ir = 4 + (16 / (sensors.sensor.VPI - sensors.sensor.NPI)) * ((float)numMensorPoint.Value - sensors.sensor.NPI);//расчетный ток
                        ResultMET.AddPoint(i, (double)numTermoCameraPoint.Value, sensors.sensor.NPI, sensors.sensor.VPI, (double)numMensorPoint.Value, sensors.sensor.Pressure, Multimetr.Current, Ir);

                        if (!cbChannalFixMET.Checked)
                        {//если стоит фиксация канал не меняем
                            cbChannalMetrolog.SelectedIndex = seli;
                            UpDateMetrologGrid(i);
                            UpdateUpStatus(i);
                        }
                        else
                        {
                            if (cbChannalMetrolog.SelectedIndex == seli)
                            {
                                UpDateMetrologGrid(i);
                            }
                        }

                        //                        UpDateVerificationGrid(i);
                        Program.txtlog.WriteLineLog("MET: Выполнено чтение параметров датчика в канале " + (i + 1).ToString(), 0);
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("MET: Параметры датчика не прочитаны!", 1);
                    }
                }
                else
                {
                    Program.txtlog.WriteLineLog("MET: Датчик не найден в канале " + (i + 1).ToString(), 1);
                }
                //Commutator.SetConnectors(i, 1); // команда отключить датчик с индексом i                    
                seli++;
            }
            Program.txtlog.WriteLineLog("MET: Операция завершена ... ", 2);
        }


        //Установка давления в Менсоре с ожиданием
        //Вход: строка со значением давления
        private int MensorSetPressuer(string strValue, bool task)
        {
            if (strValue == "")
            {
                Program.txtlog.WriteLineLog("Не задано значение давления", 1);
                return -1;
            }
            if ((Mensor.Connected) || (Pascal.Connected) || (Elemer.Connected))
            {
                double Point = 0;
                double shift = 0;

                Program.txtlog.WriteLineLog("Устанавливаем давление в датчиках " + strValue + "кПА", 0);



                Point = double.Parse(strValue.Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                numMensorPoint.Text = strValue;

                // если установлено АБСОЛЮТНОЕ давление, то для Паскаля и Элемера, от уставки отнимаем атмосферное давление
                // которое задано в ГПа для перевода его в кПА нужно разделить на 10
                if ((selectedPressurer >= 1) && (rbPressABS.Checked))
                {
                    Point = Point - Convert.ToDouble(numATMpress.Value);
                }


                if ((Point == 0) && !SensorAbsPressuer)
                {
                    button7_Click(null, null);
                }
                else
                {
                    button6_Click(null, null);      //выставляем давление
                    Thread.Sleep(2000);

                    // доработка 09.08.2020 / на зам. №4 от 06.08.2020 
                    // если задача давления включена, то повторно ее не включаем 
                    // UseMensor = true - используем Менсор
                    // UseMensor = false - используем Паскаль
                    // Pascal.modeStart = true/false - задача ВКЛЮЧЕНА/ВЫКЛЮЧЕНА (ПАСКАЛЬ)
                    // Mensor._mode = 0 / 1 / 2 - режимы ИЗМ. / ЗАДАЧА / СБОРС 

                    //if ((!UseMensor && !Pascal.modeStart) || (UseMensor && Mensor._mode != 1))
                    if (((selectedPressurer == 1) && !Pascal.modeStart) || ((selectedPressurer == 0) && Mensor._mode != 1) || ((selectedPressurer == 2) && !Elemer.modeStartReg))
                    {
                        bMensorControl_Click(null, null);  //запускаем задачу
                    }
                    /*if (task)
                     {
                         bMensorControl_Click(null,null);  //запускаем задачу
                     }*/
                }
                TimerTickCount = 0;
                do//ожидаем установления давления
                {
                    if (ProcessStop) return -1;//прекращаем
                    Application.DoEvents();
                    Thread.Sleep(100);
                    shift = Math.Abs(Convert.ToDouble(tbMensorData.Text) - Point);
                } while ((shift > SKO_PRESSURE) && (TimerTickCount < MENSOR_PRESSUER_WAIT / MainTimer.Interval));
                if (TimerTickCount >= MENSOR_PRESSUER_WAIT / MainTimer.Interval)
                {//давление не установлено
                    Program.txtlog.WriteLineLog("Истекло время установки давления в датчиках", 1);
                    return -2;
                }
                else
                {//давление установлено
                    TimerTickCount = 0;
                    do//ожидаем установления давления
                    {
                        if (ProcessStop) return -1;//прекращаем
                        Application.DoEvents();
                        Thread.Sleep(100);
                    } while (TimerTickCount < SENSOR_PRESSUER_WAIT / MainTimer.Interval);

                    //                    Thread.Sleep(SENSOR_PRESSUER_WAIT);//ожидаем стабилизации
                    PressureReady = true;
                    Program.txtlog.WriteLineLog("Давление в датчиках установлено.", 0);
                    return 0;
                }
            }
            else
            {
                Program.txtlog.WriteLineLog("Нет cвязи c задатчиком давления.", 1);
                return -3;
            }
        }

        private void cbChannalMetrolog_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!SensorBusy)
            {
                if (cbChannalMetrolog.Text == "") return;

                string str = cbChannalMetrolog.Text.Remove(0, 6);
                int ii = Convert.ToInt32(str) - 1;

                UpDateMetrologGrid(ii);
                UpdateUpStatus(ii);
            }
        }


        //Расчет отклонений по давлению при характеризации датчиков
        private void btnCalculateDeviation_Click(object sender, EventArgs e)
        {
            if (ResultCH == null)
            {
                Program.txtlog.WriteLineLog("CH: Результаты характеризации не сформированы!", 0);
                return;
            }


            for (int i = 0; i < ResultCH.Channal.Count; i++)//
            {
                if (ResultCH.Channal[i].Points.Count <= 0)
                {
                    //Program.txtlog.WriteLineLog("CH: Результаты характеризации не сформированы!", 0);
                    continue;
                    //return;
                }
                else
                {
                    ResultCH.CalcDeviation(i);
                    ResultCH.SaveToArhiv(i);
                    UpDateCharakterizatorGrid(i);
                }
            }

        }

        private void cb_ManualMode_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_ManualMode.Checked)
            {
                // Отключаем если были подключены задатчики 
                // Менсор
                Mensor.DisConnect();
                // Паскаль
                Pascal.DisConnect();

                // Блокируем кнопки управления на форме
                btnMensor.Text = "Режим включен";
                btnMensor.BackColor = Color.Green;
                btnMensor.Enabled = false;
                btnFormMensor.Enabled = false;

                List<string> listManual = new List<string>() { "Ручной режим" };
                cbMensorTypeR.DataSource = listManual;
                cbMensorTypeR.SelectedIndex = 0;
                cbMensorTypeR.Enabled = false;

                bMensorMeas.Enabled = false;
                bMensorControl.Enabled = false;
                bMensorVent.Enabled = false;

                tbMensorData.Text = Convert.ToString(numMensorPoint.Value);
            }
            else
            {
                // Блокируем кнопки управления на форме
                btnMensor.Text = "Не подключен";
                btnMensor.BackColor = Color.IndianRed;
                btnMensor.Enabled = true;
                btnFormMensor.Enabled = true;

                cbMensorTypeR.SelectedIndex = -1;
                cbMensorTypeR.Enabled = true;

                bMensorMeas.Enabled = true;
                bMensorControl.Enabled = true;
                bMensorVent.Enabled = true;
            }

        }

        private void cbMensorTypeR_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cb_ManualMode.Checked)
                return;


            switch (selectedPressurer)
            {
                case 0: // менсор
                    {

                        if (!Mensor._serialPort_M.IsOpen)
                        {
                            if (cbMensorTypeR.SelectedIndex != -1)
                            {
                                Program.txtlog.WriteLineLog("Нет Связи. Задатчик давления не подключен", 1);
                                cbMensorTypeR.SelectedIndex = -1;
                            }
                            MainTimer.Enabled = true;
                            MainTimer.Start();
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
                        break;
                    }

                case 1: // паскаль
                    {
                        if (!Pascal.Port.IsOpen)
                        {
                            if (cbMensorTypeR.SelectedIndex != -1)
                            {
                                Program.txtlog.WriteLineLog("Нет Связи. Задатчик давления не подключен", 1);
                                cbMensorTypeR.SelectedIndex = -1;
                            }
                            MainTimer.Enabled = true;
                            MainTimer.Start();
                            return;
                        }
                        try
                        {
                            MainTimer.Stop();
                            MainTimer.Enabled = false;

                            // сброс давления ВКЛ
                            if (!Pascal.modeVent)
                                Pascal.SetModeVent();

                            while (Pascal.press > 1)
                            {
                                if (ProcessStop) return;//прекращаем
                                Thread.Sleep(500);
                            }
                            // сброс давления ВЫКЛ

                            // сброс давления ВКЛ
                            if (Pascal.modeVent)
                                Pascal.SetModeVent();

                            Thread.Sleep(50);

                            // Получаем индекс выбранного преобразователя
                            int ind = cbMensorTypeR.SelectedIndex + 1;

                            // Определяем тип канала соответствующего выбранному преобразователю
                            if ((ind > 0) && (ind <= Pascal.M1num))  // 
                            {
                                Pascal.SetModule(1, ind);
                            }
                            else if ((ind > Pascal.M1num) && (ind <= Pascal.M1num + Pascal.M2num)) // 
                            {
                                Pascal.SetModule(2, ind - Pascal.M1num);
                            }

                        }
                        finally
                        {
                            MainTimer.Enabled = true;
                            MainTimer.Start();
                        }
                        break;
                    }

                case 2: // элемер
                    {
                        if ((!Elemer.Port.IsOpen)||(cbMensorTypeR.SelectedIndex != -1))
                        {
                            
                                Program.txtlog.WriteLineLog("Нет Связи. Задатчик давления не подключен", 1);
                                //cbMensorTypeR.SelectedIndex = -1;
                            
                            
                        }
                        /*
                        try
                        {
                            MainTimer.Stop();
                            MainTimer.Enabled = false;

                            // сброс давления ВКЛ
                            if (!Pascal.modeVent)
                                Pascal.SetModeVent();

                            while (Pascal.press > 1)
                            {
                                if (ProcessStop) return;//прекращаем
                                Thread.Sleep(500);
                            }
                            // сброс давления ВЫКЛ

                            // сброс давления ВКЛ
                            if (Pascal.modeVent)
                                Pascal.SetModeVent();

                            Thread.Sleep(50);

                            // Получаем индекс выбранного преобразователя
                            int ind = cbMensorTypeR.SelectedIndex + 1;

                            // Определяем тип канала соответствующего выбранному преобразователю
                            if ((ind > 0) && (ind <= Pascal.M1num))  // 
                            {
                                Pascal.SetModule(1, ind);
                            }
                            else if ((ind > Pascal.M1num) && (ind <= Pascal.M1num + Pascal.M2num)) // 
                            {
                                Pascal.SetModule(2, ind - Pascal.M1num);
                            }

                        }
                        finally
                        {
                            MainTimer.Enabled = true;
                            MainTimer.Start();
                        }
                        */
                        break;
                    }
            }
        }



        private void lb_MET_PressValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Delete)
            {
                btn_MET_Del.PerformClick();
            }
            if (System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Control)
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Up)
                {
                    btn_MET_Up.PerformClick();
                    lb_MET_PressValue.SelectedIndex = lb_MET_PressValue.SelectedIndex + 1;
                }
                if (e.KeyCode == System.Windows.Forms.Keys.Down)
                {
                    btn_MET_Down.PerformClick();
                    lb_MET_PressValue.SelectedIndex = lb_MET_PressValue.SelectedIndex - 1;
                }
            }
        }

        private void lb_MET_PressValue_DoubleClick(object sender, EventArgs e)
        {
            FormInput forminput = new FormInput();
            //int index = lb_MET_PressValue.SelectedIndex;
            forminput.Pressuer = lb_MET_PressValue.SelectedItem.ToString();
            if (forminput.ShowDialog() == DialogResult.OK)
            {
                int index = lb_MET_PressValue.SelectedIndex;
                lb_MET_PressValue.Items.RemoveAt(index);
                lb_MET_PressValue.Items.Insert(index, forminput.Pressuer);
            }
        }

        private void btn_MET_Unit_Click(object sender, EventArgs e)
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
                    Program.txtlog.WriteLineLog("MET: Не выбраны каналы для установки ед измерения датчиков. Операция прервана.", 0);
                    return;
                }

                try
                {
                    btn_MET_Unit.Text = "Остановить";
                    UpdateItemState(12);
                    WriteSensor_MET_MesUnit();
                }
                finally
                {
                    btn_MET_Unit.Text = "Задать";
                    UpdateItemState(0);
                }
            }
            else
            {
                FormPause formpause = new FormPause();
                if (formpause.ShowDialog() != DialogResult.OK)
                //if (MessageBox.Show("Отменить установку единицы измерения датчиков?", "Подтверждение команды", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ProcessStop = true;
                    Program.txtlog.WriteLineLog("MET:Операция прекращена пользователем", 0);
                }
            }
        }





        // Установка типа давления ИЗБЫТОЧНОЕ или АБСОЛЮТНОЕ в МЕНСОРЕ
        private void rbPressIZB_CheckedChanged(object sender, EventArgs e)
        {
            if (!cb_ManualMode.Checked) // если НЕ выбран ручной режим
            {

                // Определяем какой из задатчиков используется

                switch (selectedPressurer)
                {
                    case 0: // менсор
                        {
                            if (Mensor.Connected)
                            {
                                if (rbPressIZB.Checked)  //выбрано ИЗБЫТОЧНОЕ давление
                                {
                                    // отправляем команду уcтановить тип давления ИЗБЫТОЧНОЕ
                                    Mensor.SetTypePress(1);
                                }

                                else
                                {
                                    // отправляем команду уcтановить тип давления АБСОЛЮТНОЕ
                                    Mensor.SetTypePress(0);
                                }
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
                            break;
                        }

                    case 1: // паскаль
                        {
                            
                            break;
                        }

                    case 2: // элемер
                        {
                            break;
                        }

                }


                /*
                if (UseMensor) // используется Менсор
                {
                    if (Mensor.Connected)
                    {
                        if (rbPressIZB.Checked)  //выбрано ИЗБЫТОЧНОЕ давление
                        {
                            // отправляем команду уcтановить тип давления ИЗБЫТОЧНОЕ
                            Mensor.SetTypePress(1);
                        }

                        else
                        {
                            // отправляем команду уcтановить тип давления АБСОЛЮТНОЕ
                            Mensor.SetTypePress(0);
                        }
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
                }
                else  // используется Паскаль
                {

                }
                */
            }
        }

        private void cbMensorTypeR_DropDownClosed(object sender, EventArgs e)
        {
            //MainTimer.Enabled = true;
            //MainTimer.Start();
        }

        private void lvCHTermoCamera_MouseDown(object sender, MouseEventArgs e)
        {
            heldDownItem = lvCHTermoCamera.GetItemAt(e.X, e.Y);
            if (heldDownItem != null)
            {
                heldDownPoint = new Point(e.X - heldDownItem.Position.X,
                                          e.Y - heldDownItem.Position.Y);
            }
        }

        private void lvCHTermoCamera_MouseMove(object sender, MouseEventArgs e)
        {
            if (heldDownItem != null)
            {
                //heldDownItem.Position = new Point(e.Location.X - heldDownPoint.X, e.Location.Y - heldDownPoint.Y);
                ListViewItem NewItem = lvCHTermoCamera.GetItemAt(e.X, e.Y);// lvCHTermoCamera.GetItemAt(e.Location.X - heldDownPoint.X, e.Location.Y - heldDownPoint.Y);
                if ((NewItem != null) && (NewItem.Index != heldDownItem.Index))
                {
                    string str = NewItem.Text;
                    NewItem.Text = heldDownItem.Text;
                    heldDownItem.Text = str;
                    heldDownItem = NewItem;
                    heldDownItem.Selected = true;
                }
            }
        }

        private void lvCHTermoCamera_MouseUp(object sender, MouseEventArgs e)
        {
            heldDownItem = null;
        }

        private void lvCHTermoCamera_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem NewItem = lvCHTermoCamera.GetItemAt(e.X, e.Y);
            if (NewItem != null)
            {
                NewItem.BeginEdit();
            }
            else
            {
                NewItem = new ListViewItem("0");
                lvCHTermoCamera.Items.Add(NewItem);
                NewItem.BeginEdit();
            }
        }

        private void tsmCHDeleteTemperItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lvCHTermoCamera.SelectedItems.Count; i++)
            {
                lvCHTermoCamera.SelectedItems[i].Remove();
            }
        }

        private void tsmAddCHTemperItem_Click(object sender, EventArgs e)
        {
            ListViewItem NewItem = new ListViewItem("0");
            if (lvCHTermoCamera.SelectedItems.Count > 0)
            {
                NewItem.Position = lvCHTermoCamera.SelectedItems[0].Position;
            }
            lvCHTermoCamera.Items.Add(NewItem);
            NewItem.BeginEdit();
        }

        private void lvCHPressureSet_MouseDown(object sender, MouseEventArgs e)
        {
            heldDownItem = lvCHPressureSet.GetItemAt(e.X, e.Y);
            if (heldDownItem != null)
            {
                heldDownPoint = new Point(e.X - heldDownItem.Position.X,
                                          e.Y - heldDownItem.Position.Y);
            }
        }

        private void lvCHPressureSet_MouseMove(object sender, MouseEventArgs e)
        {
            if (heldDownItem != null)
            {
                ListViewItem NewItem = lvCHPressureSet.GetItemAt(e.X, e.Y);// lvCHTermoCamera.GetItemAt(e.Location.X - heldDownPoint.X, e.Location.Y - heldDownPoint.Y);
                if ((NewItem != null) && (NewItem.Index != heldDownItem.Index))
                {
                    string str = NewItem.Text;
                    NewItem.Text = heldDownItem.Text;
                    heldDownItem.Text = str;
                    heldDownItem = NewItem;
                    heldDownItem.Selected = true;
                }
            }
        }

        private void lvCHPressureSet_MouseUp(object sender, MouseEventArgs e)
        {
            heldDownItem = null;
        }

        private void lvCHPressureSet_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem NewItem = lvCHPressureSet.GetItemAt(e.X, e.Y);
            if (NewItem != null)
            {
                NewItem.BeginEdit();
            }
            else
            {
                NewItem = new ListViewItem("0");
                lvCHPressureSet.Items.Add(NewItem);
                NewItem.BeginEdit();
            }
        }

        private void tsmCHDeletePressItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lvCHPressureSet.SelectedItems.Count; i++)
            {
                lvCHPressureSet.SelectedItems[i].Remove();
            }
        }

        private void tsmCHAddPressItem_Click(object sender, EventArgs e)
        {
            ListViewItem NewItem = new ListViewItem("0");
            if (lvCHPressureSet.SelectedItems.Count > 0)
            {
                NewItem.Position = lvCHPressureSet.SelectedItems[0].Position;
            }
            lvCHPressureSet.Items.Add(NewItem);
            NewItem.BeginEdit();

        }

        private void lvVRTermoCamera_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem NewItem = lvVRTermoCamera.GetItemAt(e.X, e.Y);
            if (NewItem != null)
            {
                NewItem.BeginEdit();
            }
            else
            {
                NewItem = new ListViewItem("0");
                lvVRTermoCamera.Items.Add(NewItem);
                NewItem.BeginEdit();
            }
        }

        private void lvVRPressureSet_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem NewItem = lvVRPressureSet.GetItemAt(e.X, e.Y);
            if (NewItem != null)
            {
                NewItem.BeginEdit();
            }
            else
            {
                NewItem = new ListViewItem("0");
                lvVRPressureSet.Items.Add(NewItem);
                NewItem.BeginEdit();
            }
        }

        private void lvVRTermoCamera_MouseDown(object sender, MouseEventArgs e)
        {
            heldDownItem = lvVRTermoCamera.GetItemAt(e.X, e.Y);
            if (heldDownItem != null)
            {
                heldDownPoint = new Point(e.X - heldDownItem.Position.X,
                                          e.Y - heldDownItem.Position.Y);
            }

        }

        private void lvVRTermoCamera_MouseMove(object sender, MouseEventArgs e)
        {
            if (heldDownItem != null)
            {
                //heldDownItem.Position = new Point(e.Location.X - heldDownPoint.X, e.Location.Y - heldDownPoint.Y);
                ListViewItem NewItem = lvVRTermoCamera.GetItemAt(e.X, e.Y);// lvCHTermoCamera.GetItemAt(e.Location.X - heldDownPoint.X, e.Location.Y - heldDownPoint.Y);
                if ((NewItem != null) && (NewItem.Index != heldDownItem.Index))
                {
                    string str = NewItem.Text;
                    NewItem.Text = heldDownItem.Text;
                    heldDownItem.Text = str;
                    heldDownItem = NewItem;
                    heldDownItem.Selected = true;
                }
            }
        }

        private void lvVRTermoCamera_MouseUp(object sender, MouseEventArgs e)
        {
            heldDownItem = null;
        }

        private void lvVRPressureSet_MouseDown(object sender, MouseEventArgs e)
        {
            heldDownItem = lvVRPressureSet.GetItemAt(e.X, e.Y);
            if (heldDownItem != null)
            {
                heldDownPoint = new Point(e.X - heldDownItem.Position.X,
                                          e.Y - heldDownItem.Position.Y);
            }
        }

        private void lvVRPressureSet_MouseMove(object sender, MouseEventArgs e)
        {
            if (heldDownItem != null)
            {
                //heldDownItem.Position = new Point(e.Location.X - heldDownPoint.X, e.Location.Y - heldDownPoint.Y);
                ListViewItem NewItem = lvVRPressureSet.GetItemAt(e.X, e.Y);// lvCHTermoCamera.GetItemAt(e.Location.X - heldDownPoint.X, e.Location.Y - heldDownPoint.Y);
                if ((NewItem != null) && (NewItem.Index != heldDownItem.Index))
                {
                    string str = NewItem.Text;
                    NewItem.Text = heldDownItem.Text;
                    heldDownItem.Text = str;
                    heldDownItem = NewItem;
                    heldDownItem.Selected = true;
                }
            }
        }

        private void lvVRPressureSet_MouseUp(object sender, MouseEventArgs e)
        {
            heldDownItem = null;
        }

        private void tsmVRDelTemperItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lvVRTermoCamera.SelectedItems.Count; i++)
            {
                lvVRTermoCamera.SelectedItems[i].Remove();
            }
        }

        private void tsmVRAddTemperItem_Click(object sender, EventArgs e)
        {
            ListViewItem NewItem = new ListViewItem("0");
            if (lvVRTermoCamera.SelectedItems.Count > 0)
            {
                NewItem.Position = lvVRTermoCamera.SelectedItems[0].Position;
            }
            lvVRTermoCamera.Items.Add(NewItem);
            NewItem.BeginEdit();
        }

        private void tsmVRDelPressItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lvVRPressureSet.SelectedItems.Count; i++)
            {
                lvVRPressureSet.SelectedItems[i].Remove();
            }
        }

        private void tsmVRAddPressItem_Click(object sender, EventArgs e)
        {
            ListViewItem NewItem = new ListViewItem("0");
            if (lvVRPressureSet.SelectedItems.Count > 0)
            {
                NewItem.Position = lvVRPressureSet.SelectedItems[0].Position;
            }
            lvVRPressureSet.Items.Add(NewItem);
            NewItem.BeginEdit();
        }

        // Раскачка датчика далением
        private void PushPress(double Pmax)
        {
            Program.txtlog.WriteLineLog("CH:Выполняем раскачу датчиков давлением", 0);
            // Определяем какой из задатчиков используется

            switch (selectedPressurer)
            {
                case 0: // менсор
                    {
                        if (Mensor.Connected)
                        {
                            numMensorPoint.Value = Convert.ToDecimal(Pmax);
                            Mensor.SetPoint(Pmax);

                            //Thread.Sleep(20);
                            // доработка 09.08.2020 / на зам. №4 от 06.08.2020 
                            // если задача давления включена, то повторно ее не включаем 
                            // UseMensor = true - используем Менсор
                            // UseMensor = false - используем Паскаль
                            // Pascal.modeStart = true/false - задача ВКЛЮЧЕНА/ВЫКЛЮЧЕНА (ПАСКАЛЬ)
                            // Mensor._mode = 0 / 1 / 2 - режимы ИЗМ. / ЗАДАЧА / СБОРС 

                            //if ((!UseMensor && !Pascal.modeStart) || (UseMensor && Mensor._mode != 1))
                            //if (((selectedPressurer == 1) && !Pascal.modeStart) || ((selectedPressurer == 0) && Mensor._mode != 1) || ((selectedPressurer == 2) && !Elemer.modeStartReg))
                            if (Mensor._mode != 1)
                            {
                                bMensorControl_Click(null, null);  //запускаем задачу
                            }

                            double shift;
                            TimerTickCount = 0;
                            do//ожидаем установления давления
                            {
                                if (ProcessStop) return;//прекращаем
                                Application.DoEvents();
                                Thread.Sleep(100);
                                shift = Math.Abs(Convert.ToDouble(tbMensorData.Text) - Pmax);
                            } while ((shift > Pmax * 0.05) && (TimerTickCount < MENSOR_PRESSUER_WAIT / MainTimer.Interval));
                            if (TimerTickCount >= MENSOR_PRESSUER_WAIT / MainTimer.Interval)
                            {//давление не установлено
                                Program.txtlog.WriteLineLog("CH:Истекло время установки давления в датчиках", 1);
                            }
                            else
                            {
                                // ждем 3 сек
                                Thread.Sleep(3000);
                            }

                            // Сброс давления
                            Mensor.SetMode(2);
                            Thread.Sleep(500);
                        }
                        break;
                    }

                case 1: // паскаль
                    {
                        numMensorPoint.Value = Convert.ToDecimal(Pmax);
                        if (Pascal.Connected)
                        {
                            //double Point = (double)numMensorPoint.Value;  // получаем заданное значение уставки

                            // если установлено АБСОЛЮТНОЕ давление, то для Паскаля, от уставки отнимаем атмосферное давление
                            // которое задано в ГПа для перевода его в кПА нужно разделить на 10
                            if (rbPressABS.Checked)
                            {
                                Pmax = Pmax - Convert.ToDouble(numATMpress.Value);
                            }

                            Pascal.SetPress(Pmax);

                            Thread.Sleep(2000);
                            // доработка 09.08.2020 / на зам. №4 от 06.08.2020 
                            // если задача давления включена, то повторно ее не включаем 
                            // UseMensor = true - используем Менсор
                            // UseMensor = false - используем Паскаль
                            // Pascal.modeStart = true/false - задача ВКЛЮЧЕНА/ВЫКЛЮЧЕНА (ПАСКАЛЬ)
                            // Mensor._mode = 0 / 1 / 2 - режимы ИЗМ. / ЗАДАЧА / СБОРС 

                            //if ((!UseMensor && !Pascal.modeStart) || (UseMensor && Mensor._mode != 1))
                            if (!Pascal.modeStart)
                            {
                                bMensorControl_Click(null, null);  //запускаем задачу
                            }

                            double shift;
                            TimerTickCount = 0;
                            do//ожидаем установления давления
                            {
                                if (ProcessStop) return;//прекращаем
                                Application.DoEvents();
                                Thread.Sleep(100);
                                shift = Math.Abs(Convert.ToDouble(tbMensorData.Text) - Pmax);
                            } while ((shift > Pmax * 0.05) && (TimerTickCount < MENSOR_PRESSUER_WAIT / MainTimer.Interval));
                            if (TimerTickCount >= MENSOR_PRESSUER_WAIT / MainTimer.Interval)
                            {//давление не установлено
                                Program.txtlog.WriteLineLog("CH:Истекло время установки давления в датчиках", 1);
                            }
                            else
                            {
                                // ждем 3 сек
                                Thread.Sleep(3000);
                            }
                            // Сброс давления
                            Pascal.SetModeVent();
                        }
                        break;
                    }

                case 2: // элемер
                    {
                        numMensorPoint.Value = Convert.ToDecimal(Pmax);
                        if (Pascal.Connected)
                        {
                            // если установлено АБСОЛЮТНОЕ давление, то для Паскаля, от уставки отнимаем атмосферное давление
                            // которое задано в ГПа для перевода его в кПА нужно разделить на 10
                            if (rbPressABS.Checked)
                            {
                                Pmax = Pmax - Convert.ToDouble(numATMpress.Value);
                            }

                            Elemer.SetPress(Pmax);

                            Thread.Sleep(2000);
                        }

                        if (!Elemer.modeStartReg)
                        {
                            bMensorControl_Click(null, null);  //запускаем задачу
                        }

                        double shift;
                        TimerTickCount = 0;
                        do//ожидаем установления давления
                        {
                            if (ProcessStop) return; //прекращаем
                            Application.DoEvents();
                            Thread.Sleep(100);
                            shift = Math.Abs(Convert.ToDouble(tbMensorData.Text) - Pmax);
                        } while ((shift > Pmax * 0.05) && (TimerTickCount < MENSOR_PRESSUER_WAIT / MainTimer.Interval));
                        if (TimerTickCount >= MENSOR_PRESSUER_WAIT / MainTimer.Interval)
                        {//давление не установлено
                            Program.txtlog.WriteLineLog("CH:Истекло время установки давления в датчиках", 1);
                        }
                        else
                        {
                            // ждем 3 сек
                            Thread.Sleep(3000);
                        }
                        // Сброс давления
                        Elemer.SetClearP();
                        break;
                    }

            }

            Program.txtlog.WriteLineLog("CH:Раскачка завершена!", 0);
        }
        /// <summary>
        /// Расчет коэффицентов по МНК
        /// </summary>
        /// <param name="Rmtx"></param>
        /// <param name="Umtx"></param>
        /// <param name="Pmtx"></param>
        /// <param name="Tmtx"></param>
        /// <param name="Pmax"></param>
        /// <param name="sensor_DV"></param>
        /// <param name="SensName"></param>
        /// <returns></returns>
        private Matrix<double> CalculationMNK(Matrix<double> Rmtx, Matrix<double> Umtx, Matrix<double> Pmtx, Matrix<double> Tmtx, double Pmax, bool sensor_DV, string SensName)
        {
            // Матрица с результатами
            Matrix<double> ResulCoefmtx = DenseMatrix.Create(1, 1, -1);       // если размерности не совпадают возвращаем -1

            // Перед расчетом производится
            // Загрузка данных о погрешностях gammaP и gammaТ из текстового файла SensPressTempErrors

            string path = Properties.Settings.Default.FileNameDB;
            string[] rr = Properties.Settings.Default.FileNameDB.Split(new char[] { '\\' });
            int fnum = rr.Length;
            path = path.Replace(rr[fnum - 1], "SensPressTempErrors.txt");

            StreamReader reader;
            string line;
            string[] wordsLine;
            int len;
            Matrix<double> gammaPaTest = DenseMatrix.Create(3, 30, -1); ;
            Matrix<double> gammaTaTest = DenseMatrix.Create(1, 2, 1);
            int fContinue = 0;

            try
            {
                reader = new StreamReader(path);
                while (((line = reader.ReadLine()) != null) & (fContinue == 0))
                {

                    wordsLine = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    // Находим нужный датчик
                    if (String.Compare(wordsLine[0], SensName) == 0)
                    {
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        wordsLine = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        len = wordsLine.Length;
                        gammaPaTest = DenseMatrix.Create(3, len - 1, 0);
                        for (int i = 1; i < len; i++)
                        {
                            gammaPaTest[0, i - 1] = double.Parse(wordsLine[i].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                        }

                        line = reader.ReadLine();
                        wordsLine = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 1; i < len; i++)
                        {
                            gammaPaTest[1, i - 1] = double.Parse(wordsLine[i].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                        }

                        line = reader.ReadLine();
                        wordsLine = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 1; i < len; i++)
                        {
                            gammaPaTest[2, i - 1] = double.Parse(wordsLine[i].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);
                        }

                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        wordsLine = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        gammaTaTest[0, 0] = double.Parse(wordsLine[1].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);

                        line = reader.ReadLine();
                        wordsLine = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        gammaTaTest[0, 1] = double.Parse(wordsLine[1].Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture);

                        fContinue = 1;

                    }
                }
            }

            catch (Exception)
            {
                fContinue = -1;
            }


            // Проверка  загружены ли данные из файла
            switch (fContinue)
            {
                case -1: // не загружены
                    {
                        Program.txtlog.WriteLineLog("Ошибка чтения данных из файла SensPressTempErrors.txt (либо файл не найден, либо не верно заполнен!)");
                        break;
                    }

                case 0: // не загружены
                    {
                        Program.txtlog.WriteLineLog("В файле SensPressTempErrors.txt не обнаружены данные о погрешностях для заданного датчика!");
                        break;
                    }

                case 1: // загружены
                    {
                        try
                        {
                            // Вызов функции расчета коэффициентов методом наименьших квадратов (МНК) 
                            ResulCoefmtx = CCalcMNK.CalcCalibrCoef(Rmtx, Umtx, Pmtx, Tmtx, Pmax, gammaPaTest, gammaTaTest, sensor_DV);

                            break;
                        }
                        catch (Exception)
                        {
                            Program.txtlog.WriteLineLog("MNK: Решение не найдено! Возникла непредвиденная ошибка в алгоритме расчета коэффициентов!", 1);
                            break;
                        }
                    }
            }
            return ResulCoefmtx;
        }


        /// <summary>
        /// Чтение из файла и расчет коэффициентов
        /// </summary>
        private void CalcMNK()
        {
            //if (ResultCH == null) return;
            if (openFileDialogArhiv.ShowDialog() != DialogResult.OK) return;
            string FileName = openFileDialogArhiv.FileName;
            СResultCH ResultCH = new СResultCH(FileName);
            double Pmax = Convert.ToDouble(SensorsDB.GetDataSensors(ResultCH.Channal[0].GetSensorType(), new string(ResultCH.Channal[0].PressureModel), "Pmax"));
            bool sensor_DV = ResultCH.Channal[0].PressureModel[1] == '2';
                        
            if (Pmax <= 0) return;

            int ch = 0;// sensors.sensor.Channal;
           
            // Матрица давлений
            Matrix<double> Pmtx = ResultCH.GetPressuerMatrix(ch);
            // Матрица напряжений
            Matrix<double> Umtx = ResultCH.GetVoltageMatrix(ch);
            // Матрица Сопротивлений
            Matrix<double> Rmtx = ResultCH.GetRezistansMatrix(ch);
            // Матрица температур
            Matrix<double> Tmtx = ResultCH.GetTemperatureMatrix(ch);

            // Название датчика
            string SensName = ResultCH.Channal[0].GetSensorType();//"ЭНИ-12";

            // Матрица с результатами
            Matrix<double> ResulCoefmtx = CalculationMNK(Rmtx, Umtx, Pmtx, Tmtx, Pmax, sensor_DV, SensName);
            // Анализ результатов
            if ((ResulCoefmtx.RowCount == 1) & (ResulCoefmtx.ColumnCount == 1))
            {
                switch (ResulCoefmtx.At(0, 0))
                {
                    case -4:
                        {
                            Program.txtlog.WriteLineLog("MNK: Решение не найдено! Рассчитанная матрица допустимых отклонений Fdop имеет нулевые значения", 1);
                            break;
                        }
                    case -3:
                        {
                            Program.txtlog.WriteLineLog("MNK: Решение не найдено! Маскимальный ВПИ! равен 0", 1);
                            break;
                        }
                    case -2:
                        {
                            Program.txtlog.WriteLineLog("MNK: Решение не найдено! Обнаружено не соответствие входных данных по размеру матриц (P, U, R, T)!", 1);
                            break;
                        }
                    case -1:
                        {
                            Program.txtlog.WriteLineLog("MNK: Решение не найдено! Считаны не верные данные о погрешностях из текстового файла", 1);
                            break;
                        }
                    case 0:
                        {
                            Program.txtlog.WriteLineLog("MNK: Решение не найдено! Не удалось решить матричное уравнение и найти коэффициенты B", 1);
                            break;
                        }
                }
            }
            else
            {
                Program.txtlog.WriteLineLog("MNK: Решение найдено!", 0);
                Program.txtlog.WriteLineLog("MNK: Рассчитанное отклонение (R^2) равно: " + Convert.ToString(ResulCoefmtx[ResulCoefmtx.RowCount - 1, 0]), 0);
                double[] tmp = new double[ResulCoefmtx.RowCount - 1];
                //double[] tmp_dbl = new double[ResulCoefmtx.RowCount - 1];
                for (int i = 0; i < ResulCoefmtx.RowCount - 1; i++)
                {
                    tmp[i] = Convert.ToSingle(ResulCoefmtx[i, 0]);
                }
                ResultCH.AddR2(0, ResulCoefmtx[ResulCoefmtx.RowCount - 1, 0]);
                ResultCH.AddCoeff(0, tmp);
            }
        }


        private void btnThermalCamera_Click_1(object sender, EventArgs e)
        {
            //MultimetrReadError = 0;
            if (ThermalCamera.Connect(Properties.Settings.Default.COMColdCamera,
                Properties.Settings.Default.COMColdCamera_Speed,
                Properties.Settings.Default.COMColdCamera_DataBits,
                Properties.Settings.Default.COMColdCamera_StopBits,
                Properties.Settings.Default.COMColdCamera_Parity) >= 0)
            {
                btnThermalCamera.BackColor = Color.Green;
                btnThermalCamera.Text = "Подключен";
                Program.txtlog.WriteLineLog("Датчик температуры подключен", 0);
            }
            else
            {
                btnThermalCamera.BackColor = Color.IndianRed;
                btnThermalCamera.Text = "Не подключен";
                Program.txtlog.WriteLineLog("Датик температуры не подключен", 1);
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            SetCommutatorChanalPower(0);
        }

        private void tsMenuMetrologDelete_Click(object sender, EventArgs e)
        {
            if (ResultMET != null)
            {
                if (cbChannalMetrolog.Text == "") return;
                string str = cbChannalMetrolog.Text.Remove(0, 6);
                int ii = Convert.ToInt32(str) - 1;

                DataGridViewSelectedRowCollection s = dataGridView5.SelectedRows;
                if (s.Count <= 0) return;

                DialogResult result = MessageBox.Show(
                        "Выбранные записи будут удалены из таблицы и архива данных метролога. Продолжить?",
                        "Подтверждение операции",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    dataGridView5.Sort(dataGridView5.Columns[0], ListSortDirection.Ascending);

                    for (int i = ResultMET.Channal[ii].Points.Count - 1; i >= 0; i--)
                    {
                        for (int j = 0; j < s.Count; j++)
                        {
                            if (i == s[j].Index)
                            {
                                ResultMET.DeletePoint(ii, i);
                                break;
                            }
                        }
                    }
                    /*for (int i = 0; i < s.Count; i++)
                    {
                        ResultMET.DeletePoint(ii, s[i].Index);
                    }*/
                    UpDateMetrologGrid(ii);
                    ResultMET.SaveToArhiv(ii);
                }
            }
        }
    }
}

/*
  switch (selectedPressurer)
            {
                case 0: // менсор
                    {
                        break;
                    }

                case 1: // паскаль
                    {
                        break;
                    }

                case 2: // элемер
                    {
                        break;
                    }

            }
            */