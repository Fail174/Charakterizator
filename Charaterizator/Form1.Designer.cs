namespace Charaterizator
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.новыйПроектToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.открытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.открытьБДДатчиковToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.мультиметрToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MI_MultimetrSetings = new System.Windows.Forms.ToolStripMenuItem();
            this.параметрыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.задатчикДавленияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MI_MensorSetings = new System.Windows.Forms.ToolStripMenuItem();
            this.параметрыToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.коммутаторToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MI_CommutatorSetings = new System.Windows.Forms.ToolStripMenuItem();
            this.параметрыToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.холодильнаяКамераToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MI_ColdCameraSetings = new System.Windows.Forms.ToolStripMenuItem();
            this.параметрыToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.датчикиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MI_SensorSetings = new System.Windows.Forms.ToolStripMenuItem();
            this.параметрыToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemMainSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.окнаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPanelCommutator = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPanelMultimetr = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPanelMensor = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPanelTermocamera = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiPanelLog = new System.Windows.Forms.ToolStripMenuItem();
            this.инфоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsMenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.помощьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbTermoCamera = new System.Windows.Forms.GroupBox();
            this.bThermalCameraSet = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numTermoCameraPoint = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.tbTemperature = new System.Windows.Forms.TextBox();
            this.btnThermalCamera = new System.Windows.Forms.Button();
            this.splitter5 = new System.Windows.Forms.Splitter();
            this.gbMensor = new System.Windows.Forms.GroupBox();
            this.btnFormMensor = new System.Windows.Forms.Button();
            this.cb_ManualMode = new System.Windows.Forms.CheckBox();
            this.bMensorMeas = new System.Windows.Forms.Button();
            this.bMensorSet = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.numMensorPoint = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.bMensorControl = new System.Windows.Forms.Button();
            this.bMensorVent = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cbMensorTypeR = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbMensorData = new System.Windows.Forms.TextBox();
            this.btnMensor = new System.Windows.Forms.Button();
            this.splitter4 = new System.Windows.Forms.Splitter();
            this.gbMultimetr = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbMultimetrData = new System.Windows.Forms.TextBox();
            this.btnMultimetr = new System.Windows.Forms.Button();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.dtpClockTimer = new System.Windows.Forms.DateTimePicker();
            this.btnClockTimer = new System.Windows.Forms.Button();
            this.tbDateTime = new System.Windows.Forms.TextBox();
            this.gbCommutator = new System.Windows.Forms.GroupBox();
            this.tbNumCH = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnFormCommutator = new System.Windows.Forms.Button();
            this.btnCommutator = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.сCHRecordNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDataTime2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cTempreture2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDiapazon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.сPressure2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cUTemp2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cUPress2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDeviation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmsCharacterizationTable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuDeleteResult = new System.Windows.Forms.ToolStripMenuItem();
            this.panelLog = new System.Windows.Forms.Panel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.rtbConsole = new System.Windows.Forms.RichTextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dataGridView5 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmsVerificationTable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsMenuVerificationDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.dataGridView4 = new System.Windows.Forms.DataGridView();
            this.сCIRecordNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.сIDataTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.сITemperature = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cICurrent4mA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cICurrent20mA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmsCurentTable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmCurrentDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.сVRRecordNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDataTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cTempreture = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cNPI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cVPI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPressureZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPressureF = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.сCurrentR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.сCurrentF = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cVoltageF = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cResistanceF = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.сChannalNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.сSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.сSensor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.сFactoryNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.сPower = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.сWork = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pbSensorSeach = new System.Windows.Forms.ProgressBar();
            this.tbSelChannalNumber = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.tbInfoDeviceAdress = new System.Windows.Forms.TextBox();
            this.tbInfoPressureModel = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.dtpInfoDate = new System.Windows.Forms.DateTimePicker();
            this.cbInfoPreambul = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.tbInfoSerialNumber = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tbInfoFactoryNumber = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tbInfoDesc = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.tbInfoTeg = new System.Windows.Forms.TextBox();
            this.label61 = new System.Windows.Forms.Label();
            this.tbInfoMesUnit = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.tbInfoMin = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.tbInfoUp = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.tbInfoDown = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.tbInfoSoftVersion = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.tbInfoSensorType = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.btnSensorSeach = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnCalculateDeviation = new System.Windows.Forms.Button();
            this.gbCHLevel4 = new System.Windows.Forms.GroupBox();
            this.cbDiapazon4 = new System.Windows.Forms.ComboBox();
            this.label42 = new System.Windows.Forms.Label();
            this.btnCHPressureSet4 = new System.Windows.Forms.Button();
            this.cbCHPressureSet4 = new System.Windows.Forms.ComboBox();
            this.btnCHTemperatureSet4 = new System.Windows.Forms.Button();
            this.label43 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.cbCHTermoCamera4 = new System.Windows.Forms.ComboBox();
            this.gbCHLevel3 = new System.Windows.Forms.GroupBox();
            this.cbDiapazon3 = new System.Windows.Forms.ComboBox();
            this.label38 = new System.Windows.Forms.Label();
            this.btnCHPressureSet3 = new System.Windows.Forms.Button();
            this.cbCHPressureSet3 = new System.Windows.Forms.ComboBox();
            this.btnCHTemperatureSet3 = new System.Windows.Forms.Button();
            this.label39 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.cbCHTermoCamera3 = new System.Windows.Forms.ComboBox();
            this.gbCHLevel2 = new System.Windows.Forms.GroupBox();
            this.cbDiapazon2 = new System.Windows.Forms.ComboBox();
            this.label34 = new System.Windows.Forms.Label();
            this.btnCHPressureSet2 = new System.Windows.Forms.Button();
            this.cbCHPressureSet2 = new System.Windows.Forms.ComboBox();
            this.btnCHTemperatureSet2 = new System.Windows.Forms.Button();
            this.label35 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.cbCHTermoCamera2 = new System.Windows.Forms.ComboBox();
            this.btnCalibrateCurrent = new System.Windows.Forms.Button();
            this.btnCalculateCoeff = new System.Windows.Forms.Button();
            this.btnReadCAP = new System.Windows.Forms.Button();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.cbChannalFix = new System.Windows.Forms.CheckBox();
            this.cbChannalCharakterizator = new System.Windows.Forms.ComboBox();
            this.btnCHStart = new System.Windows.Forms.Button();
            this.pbCHProcess = new System.Windows.Forms.ProgressBar();
            this.gbCHLevel1 = new System.Windows.Forms.GroupBox();
            this.cbDiapazon1 = new System.Windows.Forms.ComboBox();
            this.label41 = new System.Windows.Forms.Label();
            this.btnCHPressureSet1 = new System.Windows.Forms.Button();
            this.cbCHPressureSet1 = new System.Windows.Forms.ComboBox();
            this.btnCHTemperatureSet1 = new System.Windows.Forms.Button();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.cbCHTermoCamera1 = new System.Windows.Forms.ComboBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnVR_SetZero = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnVR_VPI_NPI = new System.Windows.Forms.Button();
            this.label57 = new System.Windows.Forms.Label();
            this.nud_VR_VPI = new System.Windows.Forms.NumericUpDown();
            this.label56 = new System.Windows.Forms.Label();
            this.nud_VR_NPI = new System.Windows.Forms.NumericUpDown();
            this.btnVRParamRead = new System.Windows.Forms.Button();
            this.gbVRLevel4 = new System.Windows.Forms.GroupBox();
            this.cbVRDiapazon4 = new System.Windows.Forms.ComboBox();
            this.label53 = new System.Windows.Forms.Label();
            this.btnVRPressureSet4 = new System.Windows.Forms.Button();
            this.cbVRPressureSet4 = new System.Windows.Forms.ComboBox();
            this.btnVRTemperatureSet4 = new System.Windows.Forms.Button();
            this.label54 = new System.Windows.Forms.Label();
            this.label55 = new System.Windows.Forms.Label();
            this.cbVRTermoCamera4 = new System.Windows.Forms.ComboBox();
            this.gbVRLevel3 = new System.Windows.Forms.GroupBox();
            this.cbVRDiapazon3 = new System.Windows.Forms.ComboBox();
            this.label50 = new System.Windows.Forms.Label();
            this.btnVRPressureSet3 = new System.Windows.Forms.Button();
            this.cbVRPressureSet3 = new System.Windows.Forms.ComboBox();
            this.btnVRTemperatureSet3 = new System.Windows.Forms.Button();
            this.label51 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.cbVRTermoCamera3 = new System.Windows.Forms.ComboBox();
            this.gbVRLevel2 = new System.Windows.Forms.GroupBox();
            this.cbVRDiapazon2 = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnVRPressureSet2 = new System.Windows.Forms.Button();
            this.cbVRPressureSet2 = new System.Windows.Forms.ComboBox();
            this.btnVRTemperatureSet2 = new System.Windows.Forms.Button();
            this.label45 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.cbVRTermoCamera2 = new System.Windows.Forms.ComboBox();
            this.gbVRLevel1 = new System.Windows.Forms.GroupBox();
            this.cbVRDiapazon1 = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnVRPressureSet1 = new System.Windows.Forms.Button();
            this.cbVRPressureSet1 = new System.Windows.Forms.ComboBox();
            this.btnVRTemperatureSet1 = new System.Windows.Forms.Button();
            this.label47 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.cbVRTermoCamera1 = new System.Windows.Forms.ComboBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.cbChannalFixVR = new System.Windows.Forms.CheckBox();
            this.cbChannalVerification = new System.Windows.Forms.ComboBox();
            this.pbVRProcess = new System.Windows.Forms.ProgressBar();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cbChannalFixMET = new System.Windows.Forms.CheckBox();
            this.cbChannalMetrolog = new System.Windows.Forms.ComboBox();
            this.btn_MET_Start = new System.Windows.Forms.Button();
            this.pbMETProcess = new System.Windows.Forms.ProgressBar();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_MET_DTime = new System.Windows.Forms.Button();
            this.nud_MET_DTime = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_MET_Down = new System.Windows.Forms.Button();
            this.btn_MET_Up = new System.Windows.Forms.Button();
            this.btn_MET_Del = new System.Windows.Forms.Button();
            this.btn_MET_Add = new System.Windows.Forms.Button();
            this.lb_MET_PressValue = new System.Windows.Forms.ListBox();
            this.label62 = new System.Windows.Forms.Label();
            this.cb_MET_Unit = new System.Windows.Forms.ComboBox();
            this.btn_MET_SetZero = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_MET_NPI_VPI = new System.Windows.Forms.Button();
            this.label59 = new System.Windows.Forms.Label();
            this.nud_MET_VPI = new System.Windows.Forms.NumericUpDown();
            this.label60 = new System.Windows.Forms.Label();
            this.nud_MET_NPI = new System.Windows.Forms.NumericUpDown();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.MainTimer = new System.Windows.Forms.Timer(this.components);
            this.pUpStatusBar = new System.Windows.Forms.Panel();
            this.label_UpStPressure = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.label_UpStResistance = new System.Windows.Forms.Label();
            this.label_UpStVoltage = new System.Windows.Forms.Label();
            this.label_UpStR = new System.Windows.Forms.Label();
            this.label_UpStV = new System.Windows.Forms.Label();
            this.cbSensorPeriodRead = new System.Windows.Forms.CheckBox();
            this.UpStCh = new System.Windows.Forms.Label();
            this.UpStSerial = new System.Windows.Forms.Label();
            this.UpStModel = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbTermoCamera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTermoCameraPoint)).BeginInit();
            this.gbMensor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMensorPoint)).BeginInit();
            this.gbMultimetr.SuspendLayout();
            this.gbCommutator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.cmsCharacterizationTable.SuspendLayout();
            this.panelLog.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView5)).BeginInit();
            this.cmsVerificationTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).BeginInit();
            this.cmsCurentTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.gbCHLevel4.SuspendLayout();
            this.gbCHLevel3.SuspendLayout();
            this.gbCHLevel2.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.gbCHLevel1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_VR_VPI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_VR_NPI)).BeginInit();
            this.gbVRLevel4.SuspendLayout();
            this.gbVRLevel3.SuspendLayout();
            this.gbVRLevel2.SuspendLayout();
            this.gbVRLevel1.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_MET_DTime)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_MET_VPI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_MET_NPI)).BeginInit();
            this.pUpStatusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Location = new System.Drawing.Point(0, 974);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1447, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.настройкиToolStripMenuItem,
            this.окнаToolStripMenuItem,
            this.инфоToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1447, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.новыйПроектToolStripMenuItem,
            this.открытьToolStripMenuItem,
            this.сохранитьToolStripMenuItem,
            this.toolStripMenuItem1,
            this.открытьБДДатчиковToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // новыйПроектToolStripMenuItem
            // 
            this.новыйПроектToolStripMenuItem.Name = "новыйПроектToolStripMenuItem";
            this.новыйПроектToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.новыйПроектToolStripMenuItem.Text = "Новый проект";
            // 
            // открытьToolStripMenuItem
            // 
            this.открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            this.открытьToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.открытьToolStripMenuItem.Text = "Открыть";
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.сохранитьToolStripMenuItem.Text = "Сохранить";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(189, 6);
            // 
            // открытьБДДатчиковToolStripMenuItem
            // 
            this.открытьБДДатчиковToolStripMenuItem.Name = "открытьБДДатчиковToolStripMenuItem";
            this.открытьБДДатчиковToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.открытьБДДатчиковToolStripMenuItem.Text = "Открыть БД датчиков";
            this.открытьБДДатчиковToolStripMenuItem.Click += new System.EventHandler(this.открытьБДДатчиковToolStripMenuItem_Click);
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.мультиметрToolStripMenuItem,
            this.задатчикДавленияToolStripMenuItem,
            this.коммутаторToolStripMenuItem,
            this.холодильнаяКамераToolStripMenuItem,
            this.датчикиToolStripMenuItem,
            this.toolStripMenuItem2,
            this.MenuItemMainSettings});
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.настройкиToolStripMenuItem.Text = "Настройки";
            // 
            // мультиметрToolStripMenuItem
            // 
            this.мультиметрToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MI_MultimetrSetings,
            this.параметрыToolStripMenuItem});
            this.мультиметрToolStripMenuItem.Name = "мультиметрToolStripMenuItem";
            this.мультиметрToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.мультиметрToolStripMenuItem.Text = "Мультиметр";
            // 
            // MI_MultimetrSetings
            // 
            this.MI_MultimetrSetings.Name = "MI_MultimetrSetings";
            this.MI_MultimetrSetings.Size = new System.Drawing.Size(213, 22);
            this.MI_MultimetrSetings.Text = "Настройки подключения";
            this.MI_MultimetrSetings.Click += new System.EventHandler(this.ToolStripMenuItem_MultimetrSetings_Click);
            // 
            // параметрыToolStripMenuItem
            // 
            this.параметрыToolStripMenuItem.Name = "параметрыToolStripMenuItem";
            this.параметрыToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.параметрыToolStripMenuItem.Text = "Параметры";
            this.параметрыToolStripMenuItem.Click += new System.EventHandler(this.параметрыToolStripMenuItem_Click);
            // 
            // задатчикДавленияToolStripMenuItem
            // 
            this.задатчикДавленияToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MI_MensorSetings,
            this.параметрыToolStripMenuItem1});
            this.задатчикДавленияToolStripMenuItem.Name = "задатчикДавленияToolStripMenuItem";
            this.задатчикДавленияToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.задатчикДавленияToolStripMenuItem.Text = "Задатчик давления";
            // 
            // MI_MensorSetings
            // 
            this.MI_MensorSetings.Name = "MI_MensorSetings";
            this.MI_MensorSetings.Size = new System.Drawing.Size(213, 22);
            this.MI_MensorSetings.Text = "Настройки подключения";
            this.MI_MensorSetings.Click += new System.EventHandler(this.MI_MensorSetings_Click);
            // 
            // параметрыToolStripMenuItem1
            // 
            this.параметрыToolStripMenuItem1.Name = "параметрыToolStripMenuItem1";
            this.параметрыToolStripMenuItem1.Size = new System.Drawing.Size(213, 22);
            this.параметрыToolStripMenuItem1.Text = "Параметры";
            this.параметрыToolStripMenuItem1.Click += new System.EventHandler(this.параметрыToolStripMenuItem1_Click);
            // 
            // коммутаторToolStripMenuItem
            // 
            this.коммутаторToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MI_CommutatorSetings,
            this.параметрыToolStripMenuItem2});
            this.коммутаторToolStripMenuItem.Name = "коммутаторToolStripMenuItem";
            this.коммутаторToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.коммутаторToolStripMenuItem.Text = "Коммутатор";
            // 
            // MI_CommutatorSetings
            // 
            this.MI_CommutatorSetings.Name = "MI_CommutatorSetings";
            this.MI_CommutatorSetings.Size = new System.Drawing.Size(213, 22);
            this.MI_CommutatorSetings.Text = "Настройки подключения";
            this.MI_CommutatorSetings.Click += new System.EventHandler(this.MI_CommutatorSetings_Click);
            // 
            // параметрыToolStripMenuItem2
            // 
            this.параметрыToolStripMenuItem2.Name = "параметрыToolStripMenuItem2";
            this.параметрыToolStripMenuItem2.Size = new System.Drawing.Size(213, 22);
            this.параметрыToolStripMenuItem2.Text = "Параметры";
            this.параметрыToolStripMenuItem2.Click += new System.EventHandler(this.параметрыToolStripMenuItem2_Click);
            // 
            // холодильнаяКамераToolStripMenuItem
            // 
            this.холодильнаяКамераToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MI_ColdCameraSetings,
            this.параметрыToolStripMenuItem3});
            this.холодильнаяКамераToolStripMenuItem.Name = "холодильнаяКамераToolStripMenuItem";
            this.холодильнаяКамераToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.холодильнаяКамераToolStripMenuItem.Text = "Термокамера";
            // 
            // MI_ColdCameraSetings
            // 
            this.MI_ColdCameraSetings.Name = "MI_ColdCameraSetings";
            this.MI_ColdCameraSetings.Size = new System.Drawing.Size(213, 22);
            this.MI_ColdCameraSetings.Text = "Настройки подключения";
            this.MI_ColdCameraSetings.Click += new System.EventHandler(this.MI_ColdCameraSetings_Click);
            // 
            // параметрыToolStripMenuItem3
            // 
            this.параметрыToolStripMenuItem3.Name = "параметрыToolStripMenuItem3";
            this.параметрыToolStripMenuItem3.Size = new System.Drawing.Size(213, 22);
            this.параметрыToolStripMenuItem3.Text = "Параметры";
            this.параметрыToolStripMenuItem3.Click += new System.EventHandler(this.параметрыToolStripMenuItem3_Click);
            // 
            // датчикиToolStripMenuItem
            // 
            this.датчикиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MI_SensorSetings,
            this.параметрыToolStripMenuItem4});
            this.датчикиToolStripMenuItem.Name = "датчикиToolStripMenuItem";
            this.датчикиToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.датчикиToolStripMenuItem.Text = "Датчики";
            // 
            // MI_SensorSetings
            // 
            this.MI_SensorSetings.Name = "MI_SensorSetings";
            this.MI_SensorSetings.Size = new System.Drawing.Size(213, 22);
            this.MI_SensorSetings.Text = "Настройки подключения";
            this.MI_SensorSetings.Click += new System.EventHandler(this.MI_SensorSetings_Click);
            // 
            // параметрыToolStripMenuItem4
            // 
            this.параметрыToolStripMenuItem4.Name = "параметрыToolStripMenuItem4";
            this.параметрыToolStripMenuItem4.Size = new System.Drawing.Size(213, 22);
            this.параметрыToolStripMenuItem4.Text = "Параметры";
            this.параметрыToolStripMenuItem4.Click += new System.EventHandler(this.параметрыToolStripMenuItem4_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(193, 6);
            // 
            // MenuItemMainSettings
            // 
            this.MenuItemMainSettings.Name = "MenuItemMainSettings";
            this.MenuItemMainSettings.Size = new System.Drawing.Size(196, 22);
            this.MenuItemMainSettings.Text = "Основные параметры";
            this.MenuItemMainSettings.Click += new System.EventHandler(this.MenuItemMainSettings_Click);
            // 
            // окнаToolStripMenuItem
            // 
            this.окнаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiPanelCommutator,
            this.tsmiPanelMultimetr,
            this.tsmiPanelMensor,
            this.tsmiPanelTermocamera,
            this.toolStripMenuItem3,
            this.tsmiPanelLog});
            this.окнаToolStripMenuItem.Name = "окнаToolStripMenuItem";
            this.окнаToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.окнаToolStripMenuItem.Text = "Окна";
            // 
            // tsmiPanelCommutator
            // 
            this.tsmiPanelCommutator.Checked = true;
            this.tsmiPanelCommutator.CheckOnClick = true;
            this.tsmiPanelCommutator.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsmiPanelCommutator.Name = "tsmiPanelCommutator";
            this.tsmiPanelCommutator.Size = new System.Drawing.Size(226, 22);
            this.tsmiPanelCommutator.Tag = "0";
            this.tsmiPanelCommutator.Text = "Панель коммутатора";
            this.tsmiPanelCommutator.Click += new System.EventHandler(this.tsmiPanelVisible_Click);
            // 
            // tsmiPanelMultimetr
            // 
            this.tsmiPanelMultimetr.Checked = true;
            this.tsmiPanelMultimetr.CheckOnClick = true;
            this.tsmiPanelMultimetr.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsmiPanelMultimetr.Name = "tsmiPanelMultimetr";
            this.tsmiPanelMultimetr.Size = new System.Drawing.Size(226, 22);
            this.tsmiPanelMultimetr.Tag = "1";
            this.tsmiPanelMultimetr.Text = "Панель мультиметра";
            this.tsmiPanelMultimetr.Click += new System.EventHandler(this.tsmiPanelVisible_Click);
            // 
            // tsmiPanelMensor
            // 
            this.tsmiPanelMensor.Checked = true;
            this.tsmiPanelMensor.CheckOnClick = true;
            this.tsmiPanelMensor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsmiPanelMensor.Name = "tsmiPanelMensor";
            this.tsmiPanelMensor.Size = new System.Drawing.Size(226, 22);
            this.tsmiPanelMensor.Tag = "2";
            this.tsmiPanelMensor.Text = "Панель задатчика давления";
            this.tsmiPanelMensor.Click += new System.EventHandler(this.tsmiPanelVisible_Click);
            // 
            // tsmiPanelTermocamera
            // 
            this.tsmiPanelTermocamera.CheckOnClick = true;
            this.tsmiPanelTermocamera.Name = "tsmiPanelTermocamera";
            this.tsmiPanelTermocamera.Size = new System.Drawing.Size(226, 22);
            this.tsmiPanelTermocamera.Tag = "3";
            this.tsmiPanelTermocamera.Text = "Панель термокамеры";
            this.tsmiPanelTermocamera.Click += new System.EventHandler(this.tsmiPanelVisible_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(223, 6);
            // 
            // tsmiPanelLog
            // 
            this.tsmiPanelLog.Checked = true;
            this.tsmiPanelLog.CheckOnClick = true;
            this.tsmiPanelLog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsmiPanelLog.Name = "tsmiPanelLog";
            this.tsmiPanelLog.Size = new System.Drawing.Size(226, 22);
            this.tsmiPanelLog.Text = "Панель журнала действий";
            this.tsmiPanelLog.Click += new System.EventHandler(this.tsmiPanelVisible_Click);
            // 
            // инфоToolStripMenuItem
            // 
            this.инфоToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsMenuItemAbout,
            this.помощьToolStripMenuItem});
            this.инфоToolStripMenuItem.Name = "инфоToolStripMenuItem";
            this.инфоToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.инфоToolStripMenuItem.Text = "Инфо";
            // 
            // tsMenuItemAbout
            // 
            this.tsMenuItemAbout.Name = "tsMenuItemAbout";
            this.tsMenuItemAbout.Size = new System.Drawing.Size(149, 22);
            this.tsMenuItemAbout.Text = "О программе";
            this.tsMenuItemAbout.Click += new System.EventHandler(this.tsMenuItemAbout_Click);
            // 
            // помощьToolStripMenuItem
            // 
            this.помощьToolStripMenuItem.Name = "помощьToolStripMenuItem";
            this.помощьToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.помощьToolStripMenuItem.Text = "Помощь";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.gbTermoCamera);
            this.panel1.Controls.Add(this.splitter5);
            this.panel1.Controls.Add(this.gbMensor);
            this.panel1.Controls.Add(this.splitter4);
            this.panel1.Controls.Add(this.gbMultimetr);
            this.panel1.Controls.Add(this.splitter3);
            this.panel1.Controls.Add(this.dtpClockTimer);
            this.panel1.Controls.Add(this.btnClockTimer);
            this.panel1.Controls.Add(this.tbDateTime);
            this.panel1.Controls.Add(this.gbCommutator);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1138, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(309, 950);
            this.panel1.TabIndex = 6;
            // 
            // gbTermoCamera
            // 
            this.gbTermoCamera.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.gbTermoCamera.Controls.Add(this.bThermalCameraSet);
            this.gbTermoCamera.Controls.Add(this.label10);
            this.gbTermoCamera.Controls.Add(this.label7);
            this.gbTermoCamera.Controls.Add(this.numTermoCameraPoint);
            this.gbTermoCamera.Controls.Add(this.label6);
            this.gbTermoCamera.Controls.Add(this.tbTemperature);
            this.gbTermoCamera.Controls.Add(this.btnThermalCamera);
            this.gbTermoCamera.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbTermoCamera.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbTermoCamera.Location = new System.Drawing.Point(0, 659);
            this.gbTermoCamera.Name = "gbTermoCamera";
            this.gbTermoCamera.Size = new System.Drawing.Size(307, 198);
            this.gbTermoCamera.TabIndex = 6;
            this.gbTermoCamera.TabStop = false;
            this.gbTermoCamera.Text = "Термокамера";
            this.gbTermoCamera.Visible = false;
            // 
            // bThermalCameraSet
            // 
            this.bThermalCameraSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bThermalCameraSet.Location = new System.Drawing.Point(212, 137);
            this.bThermalCameraSet.Name = "bThermalCameraSet";
            this.bThermalCameraSet.Size = new System.Drawing.Size(81, 49);
            this.bThermalCameraSet.TabIndex = 12;
            this.bThermalCameraSet.Text = "Задача";
            this.bThermalCameraSet.UseVisualStyleBackColor = true;
            this.bThermalCameraSet.Click += new System.EventHandler(this.bThermalCameraSet_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(231, 84);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 42);
            this.label10.TabIndex = 11;
            this.label10.Text = "°С";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(3, 81);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(116, 42);
            this.label7.TabIndex = 8;
            this.label7.Text = "Текущая температура:";
            // 
            // numTermoCameraPoint
            // 
            this.numTermoCameraPoint.DecimalPlaces = 1;
            this.numTermoCameraPoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numTermoCameraPoint.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numTermoCameraPoint.Location = new System.Drawing.Point(99, 137);
            this.numTermoCameraPoint.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numTermoCameraPoint.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numTermoCameraPoint.Name = "numTermoCameraPoint";
            this.numTermoCameraPoint.Size = new System.Drawing.Size(104, 49);
            this.numTermoCameraPoint.TabIndex = 7;
            this.numTermoCameraPoint.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(3, 150);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 20);
            this.label6.TabIndex = 6;
            this.label6.Text = "Уставка:";
            // 
            // tbTemperature
            // 
            this.tbTemperature.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbTemperature.Location = new System.Drawing.Point(119, 81);
            this.tbTemperature.Name = "tbTemperature";
            this.tbTemperature.Size = new System.Drawing.Size(103, 49);
            this.tbTemperature.TabIndex = 3;
            this.tbTemperature.Text = "80,0";
            // 
            // btnThermalCamera
            // 
            this.btnThermalCamera.BackColor = System.Drawing.Color.Green;
            this.btnThermalCamera.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnThermalCamera.Location = new System.Drawing.Point(7, 27);
            this.btnThermalCamera.Name = "btnThermalCamera";
            this.btnThermalCamera.Size = new System.Drawing.Size(107, 46);
            this.btnThermalCamera.TabIndex = 2;
            this.btnThermalCamera.Text = "Подключен";
            this.btnThermalCamera.UseVisualStyleBackColor = false;
            this.btnThermalCamera.Click += new System.EventHandler(this.btnThermalCamera_Click);
            // 
            // splitter5
            // 
            this.splitter5.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter5.Location = new System.Drawing.Point(0, 656);
            this.splitter5.Name = "splitter5";
            this.splitter5.Size = new System.Drawing.Size(307, 3);
            this.splitter5.TabIndex = 19;
            this.splitter5.TabStop = false;
            // 
            // gbMensor
            // 
            this.gbMensor.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.gbMensor.Controls.Add(this.btnFormMensor);
            this.gbMensor.Controls.Add(this.cb_ManualMode);
            this.gbMensor.Controls.Add(this.bMensorMeas);
            this.gbMensor.Controls.Add(this.bMensorSet);
            this.gbMensor.Controls.Add(this.label9);
            this.gbMensor.Controls.Add(this.numMensorPoint);
            this.gbMensor.Controls.Add(this.label4);
            this.gbMensor.Controls.Add(this.bMensorControl);
            this.gbMensor.Controls.Add(this.bMensorVent);
            this.gbMensor.Controls.Add(this.label3);
            this.gbMensor.Controls.Add(this.cbMensorTypeR);
            this.gbMensor.Controls.Add(this.label2);
            this.gbMensor.Controls.Add(this.tbMensorData);
            this.gbMensor.Controls.Add(this.btnMensor);
            this.gbMensor.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbMensor.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbMensor.Location = new System.Drawing.Point(0, 285);
            this.gbMensor.Name = "gbMensor";
            this.gbMensor.Size = new System.Drawing.Size(307, 371);
            this.gbMensor.TabIndex = 3;
            this.gbMensor.TabStop = false;
            this.gbMensor.Text = "Задатчик давления";
            // 
            // btnFormMensor
            // 
            this.btnFormMensor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnFormMensor.Location = new System.Drawing.Point(125, 30);
            this.btnFormMensor.Name = "btnFormMensor";
            this.btnFormMensor.Size = new System.Drawing.Size(166, 27);
            this.btnFormMensor.TabIndex = 4;
            this.btnFormMensor.Text = "Управление";
            this.btnFormMensor.UseVisualStyleBackColor = true;
            this.btnFormMensor.Click += new System.EventHandler(this.btnFormMensor_Click);
            // 
            // cb_ManualMode
            // 
            this.cb_ManualMode.AutoSize = true;
            this.cb_ManualMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cb_ManualMode.Location = new System.Drawing.Point(127, 57);
            this.cb_ManualMode.Name = "cb_ManualMode";
            this.cb_ManualMode.Size = new System.Drawing.Size(121, 20);
            this.cb_ManualMode.TabIndex = 13;
            this.cb_ManualMode.Text = "Ручной режим";
            this.cb_ManualMode.UseVisualStyleBackColor = true;
            this.cb_ManualMode.CheckedChanged += new System.EventHandler(this.cb_ManualMode_CheckedChanged);
            // 
            // bMensorMeas
            // 
            this.bMensorMeas.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bMensorMeas.Location = new System.Drawing.Point(6, 313);
            this.bMensorMeas.Name = "bMensorMeas";
            this.bMensorMeas.Size = new System.Drawing.Size(97, 49);
            this.bMensorMeas.TabIndex = 12;
            this.bMensorMeas.Text = "Измерение";
            this.bMensorMeas.UseVisualStyleBackColor = true;
            this.bMensorMeas.Click += new System.EventHandler(this.bMensorMeas_Click);
            // 
            // bMensorSet
            // 
            this.bMensorSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bMensorSet.Location = new System.Drawing.Point(204, 250);
            this.bMensorSet.Name = "bMensorSet";
            this.bMensorSet.Size = new System.Drawing.Size(90, 49);
            this.bMensorSet.TabIndex = 11;
            this.bMensorSet.Text = "ОК";
            this.bMensorSet.UseVisualStyleBackColor = true;
            this.bMensorSet.Click += new System.EventHandler(this.button6_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(197, 173);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 42);
            this.label9.TabIndex = 10;
            this.label9.Text = "кПа";
            // 
            // numMensorPoint
            // 
            this.numMensorPoint.DecimalPlaces = 2;
            this.numMensorPoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numMensorPoint.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numMensorPoint.Location = new System.Drawing.Point(7, 250);
            this.numMensorPoint.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numMensorPoint.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numMensorPoint.Name = "numMensorPoint";
            this.numMensorPoint.Size = new System.Drawing.Size(189, 49);
            this.numMensorPoint.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(6, 228);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 20);
            this.label4.TabIndex = 4;
            this.label4.Text = "Уставка:";
            // 
            // bMensorControl
            // 
            this.bMensorControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bMensorControl.Location = new System.Drawing.Point(111, 314);
            this.bMensorControl.Name = "bMensorControl";
            this.bMensorControl.Size = new System.Drawing.Size(85, 49);
            this.bMensorControl.TabIndex = 6;
            this.bMensorControl.Text = "Задача";
            this.bMensorControl.UseVisualStyleBackColor = true;
            this.bMensorControl.Click += new System.EventHandler(this.bMensorControl_Click);
            // 
            // bMensorVent
            // 
            this.bMensorVent.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bMensorVent.Location = new System.Drawing.Point(204, 314);
            this.bMensorVent.Name = "bMensorVent";
            this.bMensorVent.Size = new System.Drawing.Size(89, 49);
            this.bMensorVent.TabIndex = 7;
            this.bMensorVent.Text = "Сброс";
            this.bMensorVent.UseVisualStyleBackColor = true;
            this.bMensorVent.Click += new System.EventHandler(this.button7_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(2, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(165, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Выбор калибратора:";
            // 
            // cbMensorTypeR
            // 
            this.cbMensorTypeR.FormattingEnabled = true;
            this.cbMensorTypeR.Items.AddRange(new object[] {
            "[канал A]  ДП-1",
            "[канал А]  ДП-2",
            "[канал А]  AutoRange",
            "[канал В]  ДП-1",
            "[канал В]  ДП-2",
            "[канал В]  AutoRange"});
            this.cbMensorTypeR.Location = new System.Drawing.Point(6, 106);
            this.cbMensorTypeR.Name = "cbMensorTypeR";
            this.cbMensorTypeR.Size = new System.Drawing.Size(285, 33);
            this.cbMensorTypeR.TabIndex = 6;
            this.cbMensorTypeR.DropDown += new System.EventHandler(this.cbMensorTypeR_DropDown);
            this.cbMensorTypeR.SelectedIndexChanged += new System.EventHandler(this.cbMensorTypeR_SelectedIndexChanged);
            this.cbMensorTypeR.SelectedValueChanged += new System.EventHandler(this.cbMensorTypeR_SelectedValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(3, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Текущее давление:";
            // 
            // tbMensorData
            // 
            this.tbMensorData.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbMensorData.Location = new System.Drawing.Point(7, 170);
            this.tbMensorData.Name = "tbMensorData";
            this.tbMensorData.ReadOnly = true;
            this.tbMensorData.Size = new System.Drawing.Size(189, 49);
            this.tbMensorData.TabIndex = 3;
            this.tbMensorData.Text = "0,000";
            // 
            // btnMensor
            // 
            this.btnMensor.BackColor = System.Drawing.Color.Green;
            this.btnMensor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnMensor.Location = new System.Drawing.Point(11, 30);
            this.btnMensor.Name = "btnMensor";
            this.btnMensor.Size = new System.Drawing.Size(108, 46);
            this.btnMensor.TabIndex = 1;
            this.btnMensor.Text = "Подключен";
            this.btnMensor.UseVisualStyleBackColor = false;
            this.btnMensor.Click += new System.EventHandler(this.btnMensor_Click);
            // 
            // splitter4
            // 
            this.splitter4.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter4.Location = new System.Drawing.Point(0, 282);
            this.splitter4.Name = "splitter4";
            this.splitter4.Size = new System.Drawing.Size(307, 3);
            this.splitter4.TabIndex = 18;
            this.splitter4.TabStop = false;
            // 
            // gbMultimetr
            // 
            this.gbMultimetr.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.gbMultimetr.Controls.Add(this.label8);
            this.gbMultimetr.Controls.Add(this.tbMultimetrData);
            this.gbMultimetr.Controls.Add(this.btnMultimetr);
            this.gbMultimetr.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbMultimetr.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.gbMultimetr.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbMultimetr.Location = new System.Drawing.Point(0, 146);
            this.gbMultimetr.Name = "gbMultimetr";
            this.gbMultimetr.Size = new System.Drawing.Size(307, 136);
            this.gbMultimetr.TabIndex = 2;
            this.gbMultimetr.TabStop = false;
            this.gbMultimetr.Text = "Мультиметр";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(159, 78);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 42);
            this.label8.TabIndex = 3;
            this.label8.Text = "мA";
            // 
            // tbMultimetrData
            // 
            this.tbMultimetrData.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbMultimetrData.Location = new System.Drawing.Point(11, 75);
            this.tbMultimetrData.Name = "tbMultimetrData";
            this.tbMultimetrData.Size = new System.Drawing.Size(143, 49);
            this.tbMultimetrData.TabIndex = 2;
            this.tbMultimetrData.Text = "0,0000";
            // 
            // btnMultimetr
            // 
            this.btnMultimetr.BackColor = System.Drawing.Color.Green;
            this.btnMultimetr.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnMultimetr.Location = new System.Drawing.Point(10, 25);
            this.btnMultimetr.Name = "btnMultimetr";
            this.btnMultimetr.Size = new System.Drawing.Size(107, 44);
            this.btnMultimetr.TabIndex = 1;
            this.btnMultimetr.Text = "Подключен";
            this.btnMultimetr.UseVisualStyleBackColor = false;
            this.btnMultimetr.Click += new System.EventHandler(this.btmMultimetr_Click);
            // 
            // splitter3
            // 
            this.splitter3.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter3.Location = new System.Drawing.Point(0, 143);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(307, 3);
            this.splitter3.TabIndex = 17;
            this.splitter3.TabStop = false;
            // 
            // dtpClockTimer
            // 
            this.dtpClockTimer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dtpClockTimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dtpClockTimer.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpClockTimer.Location = new System.Drawing.Point(5, 849);
            this.dtpClockTimer.Name = "dtpClockTimer";
            this.dtpClockTimer.ShowUpDown = true;
            this.dtpClockTimer.Size = new System.Drawing.Size(160, 47);
            this.dtpClockTimer.TabIndex = 16;
            this.dtpClockTimer.Value = new System.DateTime(2020, 3, 5, 0, 1, 0, 0);
            // 
            // btnClockTimer
            // 
            this.btnClockTimer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClockTimer.BackColor = System.Drawing.Color.Green;
            this.btnClockTimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnClockTimer.Location = new System.Drawing.Point(173, 847);
            this.btnClockTimer.Name = "btnClockTimer";
            this.btnClockTimer.Size = new System.Drawing.Size(131, 49);
            this.btnClockTimer.TabIndex = 15;
            this.btnClockTimer.Text = "Таймер";
            this.btnClockTimer.UseVisualStyleBackColor = false;
            this.btnClockTimer.Click += new System.EventHandler(this.btnClockTimer_Click);
            // 
            // tbDateTime
            // 
            this.tbDateTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDateTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbDateTime.Location = new System.Drawing.Point(5, 901);
            this.tbDateTime.Name = "tbDateTime";
            this.tbDateTime.ReadOnly = true;
            this.tbDateTime.Size = new System.Drawing.Size(299, 40);
            this.tbDateTime.TabIndex = 10;
            this.tbDateTime.Text = "00:00:00";
            // 
            // gbCommutator
            // 
            this.gbCommutator.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.gbCommutator.Controls.Add(this.tbNumCH);
            this.gbCommutator.Controls.Add(this.label1);
            this.gbCommutator.Controls.Add(this.btnFormCommutator);
            this.gbCommutator.Controls.Add(this.btnCommutator);
            this.gbCommutator.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbCommutator.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.gbCommutator.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbCommutator.Location = new System.Drawing.Point(0, 0);
            this.gbCommutator.Name = "gbCommutator";
            this.gbCommutator.Size = new System.Drawing.Size(307, 143);
            this.gbCommutator.TabIndex = 1;
            this.gbCommutator.TabStop = false;
            this.gbCommutator.Text = "Коммутатор";
            // 
            // tbNumCH
            // 
            this.tbNumCH.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbNumCH.Location = new System.Drawing.Point(222, 87);
            this.tbNumCH.Name = "tbNumCH";
            this.tbNumCH.Size = new System.Drawing.Size(69, 35);
            this.tbNumCH.TabIndex = 3;
            this.tbNumCH.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(14, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(196, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Номер текущего канала:";
            // 
            // btnFormCommutator
            // 
            this.btnFormCommutator.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnFormCommutator.Location = new System.Drawing.Point(124, 32);
            this.btnFormCommutator.Name = "btnFormCommutator";
            this.btnFormCommutator.Size = new System.Drawing.Size(167, 46);
            this.btnFormCommutator.TabIndex = 1;
            this.btnFormCommutator.Text = "Управление";
            this.btnFormCommutator.UseVisualStyleBackColor = true;
            this.btnFormCommutator.Click += new System.EventHandler(this.btnFormCommutator_Click);
            // 
            // btnCommutator
            // 
            this.btnCommutator.BackColor = System.Drawing.Color.Green;
            this.btnCommutator.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCommutator.Location = new System.Drawing.Point(11, 32);
            this.btnCommutator.Name = "btnCommutator";
            this.btnCommutator.Size = new System.Drawing.Size(107, 46);
            this.btnCommutator.TabIndex = 0;
            this.btnCommutator.Text = "Подключен";
            this.btnCommutator.UseVisualStyleBackColor = false;
            this.btnCommutator.Click += new System.EventHandler(this.btnCommutator_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.BackgroundColor = System.Drawing.SystemColors.InactiveCaption;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.сCHRecordNum,
            this.cDataTime2,
            this.cTempreture2,
            this.cDiapazon,
            this.сPressure2,
            this.cUTemp2,
            this.cUPress2,
            this.cDeviation});
            this.dataGridView2.ContextMenuStrip = this.cmsCharacterizationTable;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(277, 0);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(861, 542);
            this.dataGridView2.TabIndex = 5;
            this.dataGridView2.Visible = false;
            this.dataGridView2.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridView2_RowsRemoved);
            // 
            // сCHRecordNum
            // 
            this.сCHRecordNum.Frozen = true;
            this.сCHRecordNum.HeaderText = "№";
            this.сCHRecordNum.MinimumWidth = 25;
            this.сCHRecordNum.Name = "сCHRecordNum";
            this.сCHRecordNum.ReadOnly = true;
            this.сCHRecordNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.сCHRecordNum.Width = 25;
            // 
            // cDataTime2
            // 
            this.cDataTime2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cDataTime2.HeaderText = "Дата и Время";
            this.cDataTime2.MinimumWidth = 200;
            this.cDataTime2.Name = "cDataTime2";
            this.cDataTime2.ReadOnly = true;
            // 
            // cTempreture2
            // 
            this.cTempreture2.HeaderText = "Температура (°C)";
            this.cTempreture2.MinimumWidth = 200;
            this.cTempreture2.Name = "cTempreture2";
            this.cTempreture2.ReadOnly = true;
            this.cTempreture2.Width = 200;
            // 
            // cDiapazon
            // 
            this.cDiapazon.HeaderText = "Диапазон";
            this.cDiapazon.MinimumWidth = 100;
            this.cDiapazon.Name = "cDiapazon";
            this.cDiapazon.ReadOnly = true;
            // 
            // сPressure2
            // 
            this.сPressure2.HeaderText = "Давление (кПа)";
            this.сPressure2.MinimumWidth = 200;
            this.сPressure2.Name = "сPressure2";
            this.сPressure2.ReadOnly = true;
            this.сPressure2.Width = 200;
            // 
            // cUTemp2
            // 
            this.cUTemp2.HeaderText = "Напряжение (мВ)";
            this.cUTemp2.MinimumWidth = 200;
            this.cUTemp2.Name = "cUTemp2";
            this.cUTemp2.ReadOnly = true;
            this.cUTemp2.Width = 200;
            // 
            // cUPress2
            // 
            this.cUPress2.HeaderText = "Сопротивление (Ом)";
            this.cUPress2.MinimumWidth = 200;
            this.cUPress2.Name = "cUPress2";
            this.cUPress2.ReadOnly = true;
            this.cUPress2.Width = 200;
            // 
            // cDeviation
            // 
            this.cDeviation.HeaderText = "Отклонение";
            this.cDeviation.Name = "cDeviation";
            this.cDeviation.ReadOnly = true;
            // 
            // cmsCharacterizationTable
            // 
            this.cmsCharacterizationTable.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsCharacterizationTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuDeleteResult});
            this.cmsCharacterizationTable.Name = "contextMenuStripCharacterizationTable";
            this.cmsCharacterizationTable.Size = new System.Drawing.Size(168, 26);
            // 
            // ToolStripMenuDeleteResult
            // 
            this.ToolStripMenuDeleteResult.Name = "ToolStripMenuDeleteResult";
            this.ToolStripMenuDeleteResult.Size = new System.Drawing.Size(167, 22);
            this.ToolStripMenuDeleteResult.Text = "Удаление строки";
            this.ToolStripMenuDeleteResult.Click += new System.EventHandler(this.ToolStripMenuDeleteResult_Click);
            // 
            // panelLog
            // 
            this.panelLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLog.Controls.Add(this.richTextBox1);
            this.panelLog.Controls.Add(this.rtbConsole);
            this.panelLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelLog.Location = new System.Drawing.Point(0, 752);
            this.panelLog.Name = "panelLog";
            this.panelLog.Size = new System.Drawing.Size(1138, 222);
            this.panelLog.TabIndex = 7;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBox1.Location = new System.Drawing.Point(527, 5);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(604, 210);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            this.richTextBox1.Visible = false;
            // 
            // rtbConsole
            // 
            this.rtbConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbConsole.Location = new System.Drawing.Point(3, 5);
            this.rtbConsole.Name = "rtbConsole";
            this.rtbConsole.ReadOnly = true;
            this.rtbConsole.Size = new System.Drawing.Size(672, 209);
            this.rtbConsole.TabIndex = 2;
            this.rtbConsole.Text = "";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dataGridView5);
            this.panel3.Controls.Add(this.dataGridView2);
            this.panel3.Controls.Add(this.splitter1);
            this.panel3.Controls.Add(this.dataGridView4);
            this.panel3.Controls.Add(this.dataGridView3);
            this.panel3.Controls.Add(this.dataGridView1);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 24);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1138, 728);
            this.panel3.TabIndex = 8;
            // 
            // dataGridView5
            // 
            this.dataGridView5.AllowUserToAddRows = false;
            this.dataGridView5.AllowUserToDeleteRows = false;
            this.dataGridView5.BackgroundColor = System.Drawing.SystemColors.InactiveCaption;
            this.dataGridView5.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView5.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9});
            this.dataGridView5.ContextMenuStrip = this.cmsVerificationTable;
            this.dataGridView5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView5.Location = new System.Drawing.Point(277, 0);
            this.dataGridView5.Name = "dataGridView5";
            this.dataGridView5.ReadOnly = true;
            this.dataGridView5.RowHeadersVisible = false;
            this.dataGridView5.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView5.Size = new System.Drawing.Size(861, 542);
            this.dataGridView5.TabIndex = 10;
            this.dataGridView5.Visible = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.Frozen = true;
            this.dataGridViewTextBoxColumn1.HeaderText = "№";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 25;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 25;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Дата и Время";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 200;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 200;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Температура (°С)";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 200;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 200;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "НПИ";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "ВПИ";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Pз (кПа)";
            this.dataGridViewTextBoxColumn6.MinimumWidth = 200;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 200;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Pф (кПа)";
            this.dataGridViewTextBoxColumn7.MinimumWidth = 200;
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 200;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "Iр (мА)";
            this.dataGridViewTextBoxColumn8.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Width = 150;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "Iф (мА)";
            this.dataGridViewTextBoxColumn9.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Width = 150;
            // 
            // cmsVerificationTable
            // 
            this.cmsVerificationTable.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsVerificationTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsMenuVerificationDelete});
            this.cmsVerificationTable.Name = "contextMenuStripVerificationTable";
            this.cmsVerificationTable.Size = new System.Drawing.Size(168, 26);
            // 
            // tsMenuVerificationDelete
            // 
            this.tsMenuVerificationDelete.Name = "tsMenuVerificationDelete";
            this.tsMenuVerificationDelete.Size = new System.Drawing.Size(167, 22);
            this.tsMenuVerificationDelete.Text = "Удаление строки";
            this.tsMenuVerificationDelete.Click += new System.EventHandler(this.tsMenuVerificationDetele_Click);
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.splitter1.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(277, 542);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(861, 3);
            this.splitter1.TabIndex = 8;
            this.splitter1.TabStop = false;
            this.splitter1.Visible = false;
            // 
            // dataGridView4
            // 
            this.dataGridView4.AllowUserToAddRows = false;
            this.dataGridView4.AllowUserToDeleteRows = false;
            this.dataGridView4.BackgroundColor = System.Drawing.SystemColors.InactiveCaption;
            this.dataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView4.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.сCIRecordNum,
            this.сIDataTime,
            this.сITemperature,
            this.cICurrent4mA,
            this.cICurrent20mA});
            this.dataGridView4.ContextMenuStrip = this.cmsCurentTable;
            this.dataGridView4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView4.Location = new System.Drawing.Point(277, 545);
            this.dataGridView4.Name = "dataGridView4";
            this.dataGridView4.ReadOnly = true;
            this.dataGridView4.RowHeadersVisible = false;
            this.dataGridView4.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView4.Size = new System.Drawing.Size(861, 183);
            this.dataGridView4.TabIndex = 7;
            this.dataGridView4.Visible = false;
            // 
            // сCIRecordNum
            // 
            this.сCIRecordNum.Frozen = true;
            this.сCIRecordNum.HeaderText = "№";
            this.сCIRecordNum.MinimumWidth = 25;
            this.сCIRecordNum.Name = "сCIRecordNum";
            this.сCIRecordNum.ReadOnly = true;
            this.сCIRecordNum.Width = 25;
            // 
            // сIDataTime
            // 
            this.сIDataTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.сIDataTime.HeaderText = "Дата и Время";
            this.сIDataTime.MinimumWidth = 200;
            this.сIDataTime.Name = "сIDataTime";
            this.сIDataTime.ReadOnly = true;
            // 
            // сITemperature
            // 
            this.сITemperature.HeaderText = "Температура (°C)";
            this.сITemperature.MinimumWidth = 200;
            this.сITemperature.Name = "сITemperature";
            this.сITemperature.ReadOnly = true;
            this.сITemperature.Width = 200;
            // 
            // cICurrent4mA
            // 
            this.cICurrent4mA.HeaderText = "Ток 4мА (мА)";
            this.cICurrent4mA.MinimumWidth = 200;
            this.cICurrent4mA.Name = "cICurrent4mA";
            this.cICurrent4mA.ReadOnly = true;
            this.cICurrent4mA.Width = 200;
            // 
            // cICurrent20mA
            // 
            this.cICurrent20mA.HeaderText = "Ток 20мА (мА)";
            this.cICurrent20mA.MinimumWidth = 200;
            this.cICurrent20mA.Name = "cICurrent20mA";
            this.cICurrent20mA.ReadOnly = true;
            this.cICurrent20mA.Width = 200;
            // 
            // cmsCurentTable
            // 
            this.cmsCurentTable.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsCurentTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmCurrentDelete});
            this.cmsCurentTable.Name = "contextMenuStripVerificationTable";
            this.cmsCurentTable.Size = new System.Drawing.Size(168, 26);
            // 
            // tsmCurrentDelete
            // 
            this.tsmCurrentDelete.Name = "tsmCurrentDelete";
            this.tsmCurrentDelete.Size = new System.Drawing.Size(167, 22);
            this.tsmCurrentDelete.Text = "Удаление строки";
            this.tsmCurrentDelete.Click += new System.EventHandler(this.tsmCurrentDelete_Click);
            // 
            // dataGridView3
            // 
            this.dataGridView3.AllowUserToAddRows = false;
            this.dataGridView3.AllowUserToDeleteRows = false;
            this.dataGridView3.BackgroundColor = System.Drawing.SystemColors.InactiveCaption;
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.сVRRecordNum,
            this.cDataTime,
            this.cTempreture,
            this.cNPI,
            this.cVPI,
            this.cPressureZ,
            this.cPressureF,
            this.сCurrentR,
            this.сCurrentF,
            this.cVoltageF,
            this.cResistanceF});
            this.dataGridView3.ContextMenuStrip = this.cmsVerificationTable;
            this.dataGridView3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView3.Location = new System.Drawing.Point(277, 0);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.ReadOnly = true;
            this.dataGridView3.RowHeadersVisible = false;
            this.dataGridView3.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView3.Size = new System.Drawing.Size(861, 728);
            this.dataGridView3.TabIndex = 6;
            this.dataGridView3.Visible = false;
            this.dataGridView3.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridView3_RowsRemoved);
            // 
            // сVRRecordNum
            // 
            this.сVRRecordNum.Frozen = true;
            this.сVRRecordNum.HeaderText = "№";
            this.сVRRecordNum.MinimumWidth = 25;
            this.сVRRecordNum.Name = "сVRRecordNum";
            this.сVRRecordNum.ReadOnly = true;
            this.сVRRecordNum.Width = 25;
            // 
            // cDataTime
            // 
            this.cDataTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cDataTime.HeaderText = "Дата и Время";
            this.cDataTime.MinimumWidth = 150;
            this.cDataTime.Name = "cDataTime";
            this.cDataTime.ReadOnly = true;
            // 
            // cTempreture
            // 
            this.cTempreture.HeaderText = "Температура (°С)";
            this.cTempreture.MinimumWidth = 150;
            this.cTempreture.Name = "cTempreture";
            this.cTempreture.ReadOnly = true;
            this.cTempreture.Width = 150;
            // 
            // cNPI
            // 
            this.cNPI.HeaderText = "НПИ";
            this.cNPI.MinimumWidth = 100;
            this.cNPI.Name = "cNPI";
            this.cNPI.ReadOnly = true;
            // 
            // cVPI
            // 
            this.cVPI.HeaderText = "ВПИ";
            this.cVPI.MinimumWidth = 100;
            this.cVPI.Name = "cVPI";
            this.cVPI.ReadOnly = true;
            // 
            // cPressureZ
            // 
            this.cPressureZ.HeaderText = "Pз (кПа)";
            this.cPressureZ.MinimumWidth = 100;
            this.cPressureZ.Name = "cPressureZ";
            this.cPressureZ.ReadOnly = true;
            this.cPressureZ.Width = 150;
            // 
            // cPressureF
            // 
            this.cPressureF.HeaderText = "Pф (кПа)";
            this.cPressureF.MinimumWidth = 100;
            this.cPressureF.Name = "cPressureF";
            this.cPressureF.ReadOnly = true;
            this.cPressureF.Width = 150;
            // 
            // сCurrentR
            // 
            this.сCurrentR.HeaderText = "Iр (мА)";
            this.сCurrentR.MinimumWidth = 100;
            this.сCurrentR.Name = "сCurrentR";
            this.сCurrentR.ReadOnly = true;
            this.сCurrentR.Width = 150;
            // 
            // сCurrentF
            // 
            this.сCurrentF.HeaderText = "Iф (мА)";
            this.сCurrentF.MinimumWidth = 100;
            this.сCurrentF.Name = "сCurrentF";
            this.сCurrentF.ReadOnly = true;
            this.сCurrentF.Width = 150;
            // 
            // cVoltageF
            // 
            this.cVoltageF.HeaderText = "V (мВ)";
            this.cVoltageF.MinimumWidth = 100;
            this.cVoltageF.Name = "cVoltageF";
            this.cVoltageF.ReadOnly = true;
            // 
            // cResistanceF
            // 
            this.cResistanceF.HeaderText = "R (Ом)";
            this.cResistanceF.MinimumWidth = 100;
            this.cResistanceF.Name = "cResistanceF";
            this.cResistanceF.ReadOnly = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.InactiveCaption;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.сChannalNum,
            this.сSelect,
            this.сSensor,
            this.сFactoryNumber,
            this.сPower,
            this.сWork});
            this.dataGridView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(277, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(861, 728);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.TabStop = false;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseClick);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // сChannalNum
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.сChannalNum.DefaultCellStyle = dataGridViewCellStyle3;
            this.сChannalNum.Frozen = true;
            this.сChannalNum.HeaderText = "№";
            this.сChannalNum.MinimumWidth = 25;
            this.сChannalNum.Name = "сChannalNum";
            this.сChannalNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.сChannalNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.сChannalNum.Width = 25;
            // 
            // сSelect
            // 
            this.сSelect.HeaderText = "Выбор канала";
            this.сSelect.MinimumWidth = 50;
            this.сSelect.Name = "сSelect";
            this.сSelect.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.сSelect.Width = 50;
            // 
            // сSensor
            // 
            this.сSensor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.сSensor.HeaderText = "Датчик";
            this.сSensor.MinimumWidth = 200;
            this.сSensor.Name = "сSensor";
            this.сSensor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // сFactoryNumber
            // 
            this.сFactoryNumber.HeaderText = "Заводской номер";
            this.сFactoryNumber.MinimumWidth = 200;
            this.сFactoryNumber.Name = "сFactoryNumber";
            this.сFactoryNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.сFactoryNumber.Width = 200;
            // 
            // сPower
            // 
            this.сPower.HeaderText = "Питание";
            this.сPower.MinimumWidth = 100;
            this.сPower.Name = "сPower";
            this.сPower.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // сWork
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Green;
            dataGridViewCellStyle4.NullValue = false;
            this.сWork.DefaultCellStyle = dataGridViewCellStyle4;
            this.сWork.FalseValue = "false";
            this.сWork.HeaderText = "Исправность";
            this.сWork.IndeterminateValue = "null";
            this.сWork.MinimumWidth = 100;
            this.сWork.Name = "сWork";
            this.сWork.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.сWork.TrueValue = "true";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.tabControl1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(277, 728);
            this.panel4.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(277, 728);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pbSensorSeach);
            this.tabPage1.Controls.Add(this.tbSelChannalNumber);
            this.tabPage1.Controls.Add(this.label31);
            this.tabPage1.Controls.Add(this.groupBox9);
            this.tabPage1.Controls.Add(this.btnSensorSeach);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(269, 702);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Поиск датчиков";
            // 
            // pbSensorSeach
            // 
            this.pbSensorSeach.Location = new System.Drawing.Point(6, 64);
            this.pbSensorSeach.Name = "pbSensorSeach";
            this.pbSensorSeach.Size = new System.Drawing.Size(255, 23);
            this.pbSensorSeach.TabIndex = 6;
            // 
            // tbSelChannalNumber
            // 
            this.tbSelChannalNumber.Location = new System.Drawing.Point(118, 106);
            this.tbSelChannalNumber.Name = "tbSelChannalNumber";
            this.tbSelChannalNumber.ReadOnly = true;
            this.tbSelChannalNumber.Size = new System.Drawing.Size(123, 20);
            this.tbSelChannalNumber.TabIndex = 5;
            this.tbSelChannalNumber.Text = "Канал 1";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.CausesValidation = false;
            this.label31.Location = new System.Drawing.Point(12, 109);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(88, 13);
            this.label31.TabIndex = 4;
            this.label31.Text = "Текущий канал:";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.tbInfoDeviceAdress);
            this.groupBox9.Controls.Add(this.tbInfoPressureModel);
            this.groupBox9.Controls.Add(this.label19);
            this.groupBox9.Controls.Add(this.label18);
            this.groupBox9.Controls.Add(this.dtpInfoDate);
            this.groupBox9.Controls.Add(this.cbInfoPreambul);
            this.groupBox9.Controls.Add(this.label17);
            this.groupBox9.Controls.Add(this.label16);
            this.groupBox9.Controls.Add(this.tbInfoSerialNumber);
            this.groupBox9.Controls.Add(this.label15);
            this.groupBox9.Controls.Add(this.tbInfoFactoryNumber);
            this.groupBox9.Controls.Add(this.label14);
            this.groupBox9.Controls.Add(this.textBox12);
            this.groupBox9.Controls.Add(this.label13);
            this.groupBox9.Controls.Add(this.tbInfoDesc);
            this.groupBox9.Controls.Add(this.label20);
            this.groupBox9.Controls.Add(this.tbInfoTeg);
            this.groupBox9.Controls.Add(this.label61);
            this.groupBox9.Controls.Add(this.tbInfoMesUnit);
            this.groupBox9.Controls.Add(this.label21);
            this.groupBox9.Controls.Add(this.tbInfoMin);
            this.groupBox9.Controls.Add(this.label22);
            this.groupBox9.Controls.Add(this.tbInfoUp);
            this.groupBox9.Controls.Add(this.label23);
            this.groupBox9.Controls.Add(this.tbInfoDown);
            this.groupBox9.Controls.Add(this.label24);
            this.groupBox9.Controls.Add(this.tbInfoSoftVersion);
            this.groupBox9.Controls.Add(this.label25);
            this.groupBox9.Controls.Add(this.textBox1);
            this.groupBox9.Controls.Add(this.label26);
            this.groupBox9.Controls.Add(this.textBox4);
            this.groupBox9.Controls.Add(this.label27);
            this.groupBox9.Controls.Add(this.textBox3);
            this.groupBox9.Controls.Add(this.label28);
            this.groupBox9.Controls.Add(this.textBox2);
            this.groupBox9.Controls.Add(this.label29);
            this.groupBox9.Controls.Add(this.tbInfoSensorType);
            this.groupBox9.Controls.Add(this.label30);
            this.groupBox9.Location = new System.Drawing.Point(6, 133);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(257, 556);
            this.groupBox9.TabIndex = 3;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Параметры выбранного датчика";
            // 
            // tbInfoDeviceAdress
            // 
            this.tbInfoDeviceAdress.Location = new System.Drawing.Point(112, 341);
            this.tbInfoDeviceAdress.Name = "tbInfoDeviceAdress";
            this.tbInfoDeviceAdress.ReadOnly = true;
            this.tbInfoDeviceAdress.Size = new System.Drawing.Size(123, 20);
            this.tbInfoDeviceAdress.TabIndex = 60;
            // 
            // tbInfoPressureModel
            // 
            this.tbInfoPressureModel.Location = new System.Drawing.Point(112, 191);
            this.tbInfoPressureModel.Name = "tbInfoPressureModel";
            this.tbInfoPressureModel.ReadOnly = true;
            this.tbInfoPressureModel.Size = new System.Drawing.Size(123, 20);
            this.tbInfoPressureModel.TabIndex = 59;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 527);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(96, 13);
            this.label19.TabIndex = 58;
            this.label19.Text = "Серийный номер:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 501);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(100, 13);
            this.label18.TabIndex = 57;
            this.label18.Text = "Заводской номер:";
            // 
            // dtpInfoDate
            // 
            this.dtpInfoDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpInfoDate.Location = new System.Drawing.Point(112, 367);
            this.dtpInfoDate.Name = "dtpInfoDate";
            this.dtpInfoDate.Size = new System.Drawing.Size(123, 20);
            this.dtpInfoDate.TabIndex = 56;
            // 
            // cbInfoPreambul
            // 
            this.cbInfoPreambul.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInfoPreambul.FormattingEnabled = true;
            this.cbInfoPreambul.Items.AddRange(new object[] {
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20"});
            this.cbInfoPreambul.Location = new System.Drawing.Point(112, 471);
            this.cbInfoPreambul.Name = "cbInfoPreambul";
            this.cbInfoPreambul.Size = new System.Drawing.Size(123, 21);
            this.cbInfoPreambul.TabIndex = 55;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 477);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(69, 13);
            this.label17.TabIndex = 53;
            this.label17.Text = "Преамбулы:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 451);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(54, 13);
            this.label16.TabIndex = 52;
            this.label16.Text = "Заметки:";
            // 
            // tbInfoSerialNumber
            // 
            this.tbInfoSerialNumber.Location = new System.Drawing.Point(112, 523);
            this.tbInfoSerialNumber.Name = "tbInfoSerialNumber";
            this.tbInfoSerialNumber.Size = new System.Drawing.Size(124, 20);
            this.tbInfoSerialNumber.TabIndex = 51;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 425);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(60, 13);
            this.label15.TabIndex = 50;
            this.label15.Text = "Описание:";
            // 
            // tbInfoFactoryNumber
            // 
            this.tbInfoFactoryNumber.Location = new System.Drawing.Point(112, 497);
            this.tbInfoFactoryNumber.Name = "tbInfoFactoryNumber";
            this.tbInfoFactoryNumber.Size = new System.Drawing.Size(123, 20);
            this.tbInfoFactoryNumber.TabIndex = 49;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 399);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(28, 13);
            this.label14.TabIndex = 48;
            this.label14.Text = "Тэг:";
            // 
            // textBox12
            // 
            this.textBox12.Location = new System.Drawing.Point(112, 445);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(123, 20);
            this.textBox12.TabIndex = 47;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 373);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(36, 13);
            this.label13.TabIndex = 46;
            this.label13.Text = "Дата:";
            // 
            // tbInfoDesc
            // 
            this.tbInfoDesc.Location = new System.Drawing.Point(112, 418);
            this.tbInfoDesc.Name = "tbInfoDesc";
            this.tbInfoDesc.Size = new System.Drawing.Size(123, 20);
            this.tbInfoDesc.TabIndex = 45;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(6, 344);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(101, 13);
            this.label20.TabIndex = 44;
            this.label20.Text = "Адрес устройства:";
            // 
            // tbInfoTeg
            // 
            this.tbInfoTeg.Location = new System.Drawing.Point(112, 392);
            this.tbInfoTeg.Name = "tbInfoTeg";
            this.tbInfoTeg.Size = new System.Drawing.Size(123, 20);
            this.tbInfoTeg.TabIndex = 43;
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Location = new System.Drawing.Point(6, 299);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(85, 13);
            this.label61.TabIndex = 42;
            this.label61.Text = "Ед. измерения:";
            // 
            // tbInfoMesUnit
            // 
            this.tbInfoMesUnit.Location = new System.Drawing.Point(112, 296);
            this.tbInfoMesUnit.Name = "tbInfoMesUnit";
            this.tbInfoMesUnit.ReadOnly = true;
            this.tbInfoMesUnit.Size = new System.Drawing.Size(123, 20);
            this.tbInfoMesUnit.TabIndex = 41;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(6, 273);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(85, 13);
            this.label21.TabIndex = 19;
            this.label21.Text = "Мин. диапазон:";
            // 
            // tbInfoMin
            // 
            this.tbInfoMin.Location = new System.Drawing.Point(112, 270);
            this.tbInfoMin.Name = "tbInfoMin";
            this.tbInfoMin.ReadOnly = true;
            this.tbInfoMin.Size = new System.Drawing.Size(123, 20);
            this.tbInfoMin.TabIndex = 18;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(6, 247);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(53, 13);
            this.label22.TabIndex = 17;
            this.label22.Text = "ВПИ ПД:";
            // 
            // tbInfoUp
            // 
            this.tbInfoUp.Location = new System.Drawing.Point(112, 244);
            this.tbInfoUp.Name = "tbInfoUp";
            this.tbInfoUp.ReadOnly = true;
            this.tbInfoUp.Size = new System.Drawing.Size(123, 20);
            this.tbInfoUp.TabIndex = 16;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(6, 221);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(54, 13);
            this.label23.TabIndex = 15;
            this.label23.Text = "НПИ ПД:";
            // 
            // tbInfoDown
            // 
            this.tbInfoDown.Location = new System.Drawing.Point(112, 218);
            this.tbInfoDown.Name = "tbInfoDown";
            this.tbInfoDown.ReadOnly = true;
            this.tbInfoDown.Size = new System.Drawing.Size(123, 20);
            this.tbInfoDown.TabIndex = 13;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(6, 194);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(69, 13);
            this.label24.TabIndex = 12;
            this.label24.Text = "Модель ПД:";
            // 
            // tbInfoSoftVersion
            // 
            this.tbInfoSoftVersion.Location = new System.Drawing.Point(112, 150);
            this.tbInfoSoftVersion.Name = "tbInfoSoftVersion";
            this.tbInfoSoftVersion.ReadOnly = true;
            this.tbInfoSoftVersion.Size = new System.Drawing.Size(123, 20);
            this.tbInfoSoftVersion.TabIndex = 11;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(6, 153);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(66, 13);
            this.label25.TabIndex = 10;
            this.label25.Text = "Версия ПО:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(112, 124);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(123, 20);
            this.textBox1.TabIndex = 9;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(6, 127);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(94, 13);
            this.label26.TabIndex = 8;
            this.label26.Text = "Физ. интерфейс:";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(112, 98);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(123, 20);
            this.textBox4.TabIndex = 7;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(6, 101);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(90, 13);
            this.label27.TabIndex = 6;
            this.label27.Text = "Версия датчика:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(112, 72);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(123, 20);
            this.textBox3.TabIndex = 5;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(6, 75);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(80, 13);
            this.label28.TabIndex = 4;
            this.label28.Text = "Версия HART:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(112, 46);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(123, 20);
            this.textBox2.TabIndex = 3;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(6, 49);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(92, 13);
            this.label29.TabIndex = 2;
            this.label29.Text = "Модель датчика:";
            // 
            // tbInfoSensorType
            // 
            this.tbInfoSensorType.Location = new System.Drawing.Point(112, 20);
            this.tbInfoSensorType.Name = "tbInfoSensorType";
            this.tbInfoSensorType.ReadOnly = true;
            this.tbInfoSensorType.Size = new System.Drawing.Size(123, 20);
            this.tbInfoSensorType.TabIndex = 1;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(6, 23);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(72, 13);
            this.label30.TabIndex = 0;
            this.label30.Text = "Тип датчика:";
            // 
            // btnSensorSeach
            // 
            this.btnSensorSeach.BackColor = System.Drawing.Color.LightGreen;
            this.btnSensorSeach.Location = new System.Drawing.Point(6, 20);
            this.btnSensorSeach.Name = "btnSensorSeach";
            this.btnSensorSeach.Size = new System.Drawing.Size(255, 38);
            this.btnSensorSeach.TabIndex = 1;
            this.btnSensorSeach.Text = "Поиск датчиков";
            this.btnSensorSeach.UseVisualStyleBackColor = false;
            this.btnSensorSeach.Click += new System.EventHandler(this.btnSensorSeach_Click_1);
            // 
            // tabPage2
            // 
            this.tabPage2.AutoScroll = true;
            this.tabPage2.Controls.Add(this.btnCalculateDeviation);
            this.tabPage2.Controls.Add(this.gbCHLevel4);
            this.tabPage2.Controls.Add(this.gbCHLevel3);
            this.tabPage2.Controls.Add(this.gbCHLevel2);
            this.tabPage2.Controls.Add(this.btnCalibrateCurrent);
            this.tabPage2.Controls.Add(this.btnCalculateCoeff);
            this.tabPage2.Controls.Add(this.btnReadCAP);
            this.tabPage2.Controls.Add(this.groupBox10);
            this.tabPage2.Controls.Add(this.btnCHStart);
            this.tabPage2.Controls.Add(this.pbCHProcess);
            this.tabPage2.Controls.Add(this.gbCHLevel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(269, 702);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Характеризация";
            // 
            // btnCalculateDeviation
            // 
            this.btnCalculateDeviation.BackColor = System.Drawing.Color.IndianRed;
            this.btnCalculateDeviation.Location = new System.Drawing.Point(144, 656);
            this.btnCalculateDeviation.Name = "btnCalculateDeviation";
            this.btnCalculateDeviation.Size = new System.Drawing.Size(124, 53);
            this.btnCalculateDeviation.TabIndex = 17;
            this.btnCalculateDeviation.Text = "Расчет отклонений";
            this.btnCalculateDeviation.UseVisualStyleBackColor = false;
            this.btnCalculateDeviation.Click += new System.EventHandler(this.btnCalculateDeviation_Click);
            // 
            // gbCHLevel4
            // 
            this.gbCHLevel4.BackColor = System.Drawing.Color.Transparent;
            this.gbCHLevel4.Controls.Add(this.cbDiapazon4);
            this.gbCHLevel4.Controls.Add(this.label42);
            this.gbCHLevel4.Controls.Add(this.btnCHPressureSet4);
            this.gbCHLevel4.Controls.Add(this.cbCHPressureSet4);
            this.gbCHLevel4.Controls.Add(this.btnCHTemperatureSet4);
            this.gbCHLevel4.Controls.Add(this.label43);
            this.gbCHLevel4.Controls.Add(this.label44);
            this.gbCHLevel4.Controls.Add(this.cbCHTermoCamera4);
            this.gbCHLevel4.Enabled = false;
            this.gbCHLevel4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbCHLevel4.Location = new System.Drawing.Point(8, 401);
            this.gbCHLevel4.Name = "gbCHLevel4";
            this.gbCHLevel4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.gbCHLevel4.Size = new System.Drawing.Size(255, 105);
            this.gbCHLevel4.TabIndex = 16;
            this.gbCHLevel4.TabStop = false;
            this.gbCHLevel4.Tag = "4";
            this.gbCHLevel4.Text = "Уровень 4";
            this.gbCHLevel4.Enter += new System.EventHandler(this.gbCHLevel1_Enter);
            // 
            // cbDiapazon4
            // 
            this.cbDiapazon4.AutoCompleteCustomSource.AddRange(new string[] {
            "1",
            "2"});
            this.cbDiapazon4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDiapazon4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbDiapazon4.FormattingEnabled = true;
            this.cbDiapazon4.Items.AddRange(new object[] {
            "1",
            "2"});
            this.cbDiapazon4.Location = new System.Drawing.Point(83, 17);
            this.cbDiapazon4.Name = "cbDiapazon4";
            this.cbDiapazon4.Size = new System.Drawing.Size(66, 24);
            this.cbDiapazon4.TabIndex = 9;
            this.cbDiapazon4.SelectedIndexChanged += new System.EventHandler(this.cbDiapazon4_SelectedIndexChanged);
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(3, 22);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(61, 13);
            this.label42.TabIndex = 8;
            this.label42.Text = "Диапазон:";
            // 
            // btnCHPressureSet4
            // 
            this.btnCHPressureSet4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCHPressureSet4.Location = new System.Drawing.Point(151, 77);
            this.btnCHPressureSet4.Name = "btnCHPressureSet4";
            this.btnCHPressureSet4.Size = new System.Drawing.Size(98, 25);
            this.btnCHPressureSet4.TabIndex = 7;
            this.btnCHPressureSet4.Tag = "4";
            this.btnCHPressureSet4.Text = "Установить";
            this.btnCHPressureSet4.UseVisualStyleBackColor = true;
            this.btnCHPressureSet4.Click += new System.EventHandler(this.btnCHPressureSet1_Click);
            // 
            // cbCHPressureSet4
            // 
            this.cbCHPressureSet4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbCHPressureSet4.FormattingEnabled = true;
            this.cbCHPressureSet4.Location = new System.Drawing.Point(83, 77);
            this.cbCHPressureSet4.Name = "cbCHPressureSet4";
            this.cbCHPressureSet4.Size = new System.Drawing.Size(66, 24);
            this.cbCHPressureSet4.TabIndex = 6;
            // 
            // btnCHTemperatureSet4
            // 
            this.btnCHTemperatureSet4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCHTemperatureSet4.Location = new System.Drawing.Point(151, 47);
            this.btnCHTemperatureSet4.Name = "btnCHTemperatureSet4";
            this.btnCHTemperatureSet4.Size = new System.Drawing.Size(98, 25);
            this.btnCHTemperatureSet4.TabIndex = 5;
            this.btnCHTemperatureSet4.Tag = "4";
            this.btnCHTemperatureSet4.Text = "Установить";
            this.btnCHTemperatureSet4.UseVisualStyleBackColor = true;
            this.btnCHTemperatureSet4.Click += new System.EventHandler(this.btnCHTemperatureSet1_Click);
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(3, 82);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(61, 13);
            this.label43.TabIndex = 3;
            this.label43.Text = "Давление:";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(3, 52);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(77, 13);
            this.label44.TabIndex = 2;
            this.label44.Text = "Температура:";
            // 
            // cbCHTermoCamera4
            // 
            this.cbCHTermoCamera4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbCHTermoCamera4.FormattingEnabled = true;
            this.cbCHTermoCamera4.Location = new System.Drawing.Point(83, 47);
            this.cbCHTermoCamera4.Name = "cbCHTermoCamera4";
            this.cbCHTermoCamera4.Size = new System.Drawing.Size(66, 24);
            this.cbCHTermoCamera4.TabIndex = 0;
            // 
            // gbCHLevel3
            // 
            this.gbCHLevel3.BackColor = System.Drawing.Color.Transparent;
            this.gbCHLevel3.Controls.Add(this.cbDiapazon3);
            this.gbCHLevel3.Controls.Add(this.label38);
            this.gbCHLevel3.Controls.Add(this.btnCHPressureSet3);
            this.gbCHLevel3.Controls.Add(this.cbCHPressureSet3);
            this.gbCHLevel3.Controls.Add(this.btnCHTemperatureSet3);
            this.gbCHLevel3.Controls.Add(this.label39);
            this.gbCHLevel3.Controls.Add(this.label40);
            this.gbCHLevel3.Controls.Add(this.cbCHTermoCamera3);
            this.gbCHLevel3.Enabled = false;
            this.gbCHLevel3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbCHLevel3.Location = new System.Drawing.Point(8, 290);
            this.gbCHLevel3.Name = "gbCHLevel3";
            this.gbCHLevel3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.gbCHLevel3.Size = new System.Drawing.Size(255, 105);
            this.gbCHLevel3.TabIndex = 15;
            this.gbCHLevel3.TabStop = false;
            this.gbCHLevel3.Tag = "3";
            this.gbCHLevel3.Text = "Уровень 3";
            this.gbCHLevel3.Enter += new System.EventHandler(this.gbCHLevel1_Enter);
            // 
            // cbDiapazon3
            // 
            this.cbDiapazon3.AutoCompleteCustomSource.AddRange(new string[] {
            "1",
            "2"});
            this.cbDiapazon3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDiapazon3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbDiapazon3.FormattingEnabled = true;
            this.cbDiapazon3.Items.AddRange(new object[] {
            "1",
            "2"});
            this.cbDiapazon3.Location = new System.Drawing.Point(83, 17);
            this.cbDiapazon3.Name = "cbDiapazon3";
            this.cbDiapazon3.Size = new System.Drawing.Size(66, 24);
            this.cbDiapazon3.TabIndex = 9;
            this.cbDiapazon3.SelectedIndexChanged += new System.EventHandler(this.cbDiapazon3_SelectedIndexChanged);
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(3, 22);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(61, 13);
            this.label38.TabIndex = 8;
            this.label38.Text = "Диапазон:";
            // 
            // btnCHPressureSet3
            // 
            this.btnCHPressureSet3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCHPressureSet3.Location = new System.Drawing.Point(151, 77);
            this.btnCHPressureSet3.Name = "btnCHPressureSet3";
            this.btnCHPressureSet3.Size = new System.Drawing.Size(98, 25);
            this.btnCHPressureSet3.TabIndex = 7;
            this.btnCHPressureSet3.Tag = "3";
            this.btnCHPressureSet3.Text = "Установить";
            this.btnCHPressureSet3.UseVisualStyleBackColor = true;
            this.btnCHPressureSet3.Click += new System.EventHandler(this.btnCHPressureSet1_Click);
            // 
            // cbCHPressureSet3
            // 
            this.cbCHPressureSet3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbCHPressureSet3.FormattingEnabled = true;
            this.cbCHPressureSet3.Location = new System.Drawing.Point(83, 77);
            this.cbCHPressureSet3.Name = "cbCHPressureSet3";
            this.cbCHPressureSet3.Size = new System.Drawing.Size(66, 24);
            this.cbCHPressureSet3.TabIndex = 6;
            // 
            // btnCHTemperatureSet3
            // 
            this.btnCHTemperatureSet3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCHTemperatureSet3.Location = new System.Drawing.Point(151, 47);
            this.btnCHTemperatureSet3.Name = "btnCHTemperatureSet3";
            this.btnCHTemperatureSet3.Size = new System.Drawing.Size(98, 25);
            this.btnCHTemperatureSet3.TabIndex = 5;
            this.btnCHTemperatureSet3.Tag = "3";
            this.btnCHTemperatureSet3.Text = "Установить";
            this.btnCHTemperatureSet3.UseVisualStyleBackColor = true;
            this.btnCHTemperatureSet3.Click += new System.EventHandler(this.btnCHTemperatureSet1_Click);
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(3, 82);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(61, 13);
            this.label39.TabIndex = 3;
            this.label39.Text = "Давление:";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(3, 52);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(77, 13);
            this.label40.TabIndex = 2;
            this.label40.Text = "Температура:";
            // 
            // cbCHTermoCamera3
            // 
            this.cbCHTermoCamera3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbCHTermoCamera3.FormattingEnabled = true;
            this.cbCHTermoCamera3.Location = new System.Drawing.Point(83, 47);
            this.cbCHTermoCamera3.Name = "cbCHTermoCamera3";
            this.cbCHTermoCamera3.Size = new System.Drawing.Size(66, 24);
            this.cbCHTermoCamera3.TabIndex = 0;
            // 
            // gbCHLevel2
            // 
            this.gbCHLevel2.BackColor = System.Drawing.Color.Transparent;
            this.gbCHLevel2.Controls.Add(this.cbDiapazon2);
            this.gbCHLevel2.Controls.Add(this.label34);
            this.gbCHLevel2.Controls.Add(this.btnCHPressureSet2);
            this.gbCHLevel2.Controls.Add(this.cbCHPressureSet2);
            this.gbCHLevel2.Controls.Add(this.btnCHTemperatureSet2);
            this.gbCHLevel2.Controls.Add(this.label35);
            this.gbCHLevel2.Controls.Add(this.label37);
            this.gbCHLevel2.Controls.Add(this.cbCHTermoCamera2);
            this.gbCHLevel2.Enabled = false;
            this.gbCHLevel2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbCHLevel2.Location = new System.Drawing.Point(8, 179);
            this.gbCHLevel2.Name = "gbCHLevel2";
            this.gbCHLevel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.gbCHLevel2.Size = new System.Drawing.Size(255, 105);
            this.gbCHLevel2.TabIndex = 14;
            this.gbCHLevel2.TabStop = false;
            this.gbCHLevel2.Tag = "2";
            this.gbCHLevel2.Text = "Уровень 2";
            this.gbCHLevel2.Enter += new System.EventHandler(this.gbCHLevel1_Enter);
            // 
            // cbDiapazon2
            // 
            this.cbDiapazon2.AutoCompleteCustomSource.AddRange(new string[] {
            "1",
            "2"});
            this.cbDiapazon2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDiapazon2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbDiapazon2.FormattingEnabled = true;
            this.cbDiapazon2.Items.AddRange(new object[] {
            "1",
            "2"});
            this.cbDiapazon2.Location = new System.Drawing.Point(83, 17);
            this.cbDiapazon2.Name = "cbDiapazon2";
            this.cbDiapazon2.Size = new System.Drawing.Size(66, 24);
            this.cbDiapazon2.TabIndex = 9;
            this.cbDiapazon2.SelectedIndexChanged += new System.EventHandler(this.cbDiapazon2_SelectedIndexChanged);
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(3, 22);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(61, 13);
            this.label34.TabIndex = 8;
            this.label34.Text = "Диапазон:";
            // 
            // btnCHPressureSet2
            // 
            this.btnCHPressureSet2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCHPressureSet2.Location = new System.Drawing.Point(151, 76);
            this.btnCHPressureSet2.Name = "btnCHPressureSet2";
            this.btnCHPressureSet2.Size = new System.Drawing.Size(98, 25);
            this.btnCHPressureSet2.TabIndex = 7;
            this.btnCHPressureSet2.Tag = "2";
            this.btnCHPressureSet2.Text = "Установить";
            this.btnCHPressureSet2.UseVisualStyleBackColor = true;
            this.btnCHPressureSet2.Click += new System.EventHandler(this.btnCHPressureSet1_Click);
            // 
            // cbCHPressureSet2
            // 
            this.cbCHPressureSet2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbCHPressureSet2.FormattingEnabled = true;
            this.cbCHPressureSet2.Location = new System.Drawing.Point(83, 76);
            this.cbCHPressureSet2.Name = "cbCHPressureSet2";
            this.cbCHPressureSet2.Size = new System.Drawing.Size(66, 24);
            this.cbCHPressureSet2.TabIndex = 6;
            // 
            // btnCHTemperatureSet2
            // 
            this.btnCHTemperatureSet2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCHTemperatureSet2.Location = new System.Drawing.Point(151, 47);
            this.btnCHTemperatureSet2.Name = "btnCHTemperatureSet2";
            this.btnCHTemperatureSet2.Size = new System.Drawing.Size(98, 25);
            this.btnCHTemperatureSet2.TabIndex = 5;
            this.btnCHTemperatureSet2.Tag = "2";
            this.btnCHTemperatureSet2.Text = "Установить";
            this.btnCHTemperatureSet2.UseVisualStyleBackColor = true;
            this.btnCHTemperatureSet2.Click += new System.EventHandler(this.btnCHTemperatureSet1_Click);
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(3, 81);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(61, 13);
            this.label35.TabIndex = 3;
            this.label35.Text = "Давление:";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(3, 52);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(77, 13);
            this.label37.TabIndex = 2;
            this.label37.Text = "Температура:";
            // 
            // cbCHTermoCamera2
            // 
            this.cbCHTermoCamera2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbCHTermoCamera2.FormattingEnabled = true;
            this.cbCHTermoCamera2.Location = new System.Drawing.Point(83, 47);
            this.cbCHTermoCamera2.Name = "cbCHTermoCamera2";
            this.cbCHTermoCamera2.Size = new System.Drawing.Size(66, 24);
            this.cbCHTermoCamera2.TabIndex = 0;
            // 
            // btnCalibrateCurrent
            // 
            this.btnCalibrateCurrent.BackColor = System.Drawing.Color.LightGreen;
            this.btnCalibrateCurrent.Location = new System.Drawing.Point(144, 568);
            this.btnCalibrateCurrent.Name = "btnCalibrateCurrent";
            this.btnCalibrateCurrent.Size = new System.Drawing.Size(119, 48);
            this.btnCalibrateCurrent.TabIndex = 13;
            this.btnCalibrateCurrent.Text = "Калибровка тока    (4 и 20 мА)";
            this.btnCalibrateCurrent.UseVisualStyleBackColor = false;
            this.btnCalibrateCurrent.Click += new System.EventHandler(this.btnCalibrateCurrent_Click);
            // 
            // btnCalculateCoeff
            // 
            this.btnCalculateCoeff.BackColor = System.Drawing.Color.IndianRed;
            this.btnCalculateCoeff.Location = new System.Drawing.Point(8, 656);
            this.btnCalculateCoeff.Name = "btnCalculateCoeff";
            this.btnCalculateCoeff.Size = new System.Drawing.Size(124, 53);
            this.btnCalculateCoeff.TabIndex = 12;
            this.btnCalculateCoeff.Text = "Расчет коэффициентов";
            this.btnCalculateCoeff.UseVisualStyleBackColor = false;
            this.btnCalculateCoeff.Click += new System.EventHandler(this.btnCalculateCoeff_Click);
            // 
            // btnReadCAP
            // 
            this.btnReadCAP.BackColor = System.Drawing.Color.LightGreen;
            this.btnReadCAP.Location = new System.Drawing.Point(8, 568);
            this.btnReadCAP.Name = "btnReadCAP";
            this.btnReadCAP.Size = new System.Drawing.Size(124, 48);
            this.btnReadCAP.TabIndex = 9;
            this.btnReadCAP.Text = "Чтение параметров ЦАП";
            this.btnReadCAP.UseVisualStyleBackColor = false;
            this.btnReadCAP.Click += new System.EventHandler(this.btnReadCAP_Click);
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.cbChannalFix);
            this.groupBox10.Controls.Add(this.cbChannalCharakterizator);
            this.groupBox10.Location = new System.Drawing.Point(8, 13);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(255, 47);
            this.groupBox10.TabIndex = 5;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Текущий канал";
            // 
            // cbChannalFix
            // 
            this.cbChannalFix.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbChannalFix.Location = new System.Drawing.Point(223, 18);
            this.cbChannalFix.MaximumSize = new System.Drawing.Size(20, 20);
            this.cbChannalFix.MinimumSize = new System.Drawing.Size(20, 20);
            this.cbChannalFix.Name = "cbChannalFix";
            this.cbChannalFix.Size = new System.Drawing.Size(20, 20);
            this.cbChannalFix.TabIndex = 10;
            this.cbChannalFix.UseVisualStyleBackColor = true;
            // 
            // cbChannalCharakterizator
            // 
            this.cbChannalCharakterizator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbChannalCharakterizator.FormattingEnabled = true;
            this.cbChannalCharakterizator.Location = new System.Drawing.Point(6, 18);
            this.cbChannalCharakterizator.Name = "cbChannalCharakterizator";
            this.cbChannalCharakterizator.Size = new System.Drawing.Size(211, 21);
            this.cbChannalCharakterizator.TabIndex = 0;
            this.cbChannalCharakterizator.SelectedIndexChanged += new System.EventHandler(this.cbChannalCharakterizator_SelectedIndexChanged);
            // 
            // btnCHStart
            // 
            this.btnCHStart.BackColor = System.Drawing.Color.LightGreen;
            this.btnCHStart.Location = new System.Drawing.Point(8, 515);
            this.btnCHStart.Name = "btnCHStart";
            this.btnCHStart.Size = new System.Drawing.Size(255, 47);
            this.btnCHStart.TabIndex = 4;
            this.btnCHStart.Text = "Старт характеризации";
            this.btnCHStart.UseVisualStyleBackColor = false;
            this.btnCHStart.Click += new System.EventHandler(this.button10_Click);
            // 
            // pbCHProcess
            // 
            this.pbCHProcess.Location = new System.Drawing.Point(8, 622);
            this.pbCHProcess.Name = "pbCHProcess";
            this.pbCHProcess.Size = new System.Drawing.Size(255, 23);
            this.pbCHProcess.TabIndex = 3;
            // 
            // gbCHLevel1
            // 
            this.gbCHLevel1.BackColor = System.Drawing.Color.LightGreen;
            this.gbCHLevel1.Controls.Add(this.cbDiapazon1);
            this.gbCHLevel1.Controls.Add(this.label41);
            this.gbCHLevel1.Controls.Add(this.btnCHPressureSet1);
            this.gbCHLevel1.Controls.Add(this.cbCHPressureSet1);
            this.gbCHLevel1.Controls.Add(this.btnCHTemperatureSet1);
            this.gbCHLevel1.Controls.Add(this.label33);
            this.gbCHLevel1.Controls.Add(this.label32);
            this.gbCHLevel1.Controls.Add(this.cbCHTermoCamera1);
            this.gbCHLevel1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbCHLevel1.Location = new System.Drawing.Point(8, 68);
            this.gbCHLevel1.Name = "gbCHLevel1";
            this.gbCHLevel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.gbCHLevel1.Size = new System.Drawing.Size(255, 105);
            this.gbCHLevel1.TabIndex = 0;
            this.gbCHLevel1.TabStop = false;
            this.gbCHLevel1.Tag = "1";
            this.gbCHLevel1.Text = "Уровень 1";
            this.gbCHLevel1.Enter += new System.EventHandler(this.gbCHLevel1_Enter);
            // 
            // cbDiapazon1
            // 
            this.cbDiapazon1.AutoCompleteCustomSource.AddRange(new string[] {
            "1",
            "2"});
            this.cbDiapazon1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDiapazon1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbDiapazon1.FormattingEnabled = true;
            this.cbDiapazon1.Items.AddRange(new object[] {
            "1",
            "2"});
            this.cbDiapazon1.Location = new System.Drawing.Point(83, 17);
            this.cbDiapazon1.Name = "cbDiapazon1";
            this.cbDiapazon1.Size = new System.Drawing.Size(66, 24);
            this.cbDiapazon1.TabIndex = 9;
            this.cbDiapazon1.SelectedIndexChanged += new System.EventHandler(this.cbDiapazon1_SelectedIndexChanged);
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(3, 22);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(61, 13);
            this.label41.TabIndex = 8;
            this.label41.Text = "Диапазон:";
            // 
            // btnCHPressureSet1
            // 
            this.btnCHPressureSet1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCHPressureSet1.Location = new System.Drawing.Point(151, 77);
            this.btnCHPressureSet1.Name = "btnCHPressureSet1";
            this.btnCHPressureSet1.Size = new System.Drawing.Size(98, 25);
            this.btnCHPressureSet1.TabIndex = 7;
            this.btnCHPressureSet1.Tag = "1";
            this.btnCHPressureSet1.Text = "Установить";
            this.btnCHPressureSet1.UseVisualStyleBackColor = true;
            this.btnCHPressureSet1.Click += new System.EventHandler(this.btnCHPressureSet1_Click);
            // 
            // cbCHPressureSet1
            // 
            this.cbCHPressureSet1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbCHPressureSet1.FormattingEnabled = true;
            this.cbCHPressureSet1.Location = new System.Drawing.Point(83, 77);
            this.cbCHPressureSet1.Name = "cbCHPressureSet1";
            this.cbCHPressureSet1.Size = new System.Drawing.Size(66, 24);
            this.cbCHPressureSet1.TabIndex = 6;
            // 
            // btnCHTemperatureSet1
            // 
            this.btnCHTemperatureSet1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCHTemperatureSet1.Location = new System.Drawing.Point(151, 47);
            this.btnCHTemperatureSet1.Name = "btnCHTemperatureSet1";
            this.btnCHTemperatureSet1.Size = new System.Drawing.Size(98, 25);
            this.btnCHTemperatureSet1.TabIndex = 5;
            this.btnCHTemperatureSet1.Tag = "1";
            this.btnCHTemperatureSet1.Text = "Установить";
            this.btnCHTemperatureSet1.UseVisualStyleBackColor = true;
            this.btnCHTemperatureSet1.Click += new System.EventHandler(this.btnCHTemperatureSet1_Click);
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(3, 82);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(61, 13);
            this.label33.TabIndex = 3;
            this.label33.Text = "Давление:";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(3, 52);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(77, 13);
            this.label32.TabIndex = 2;
            this.label32.Text = "Температура:";
            // 
            // cbCHTermoCamera1
            // 
            this.cbCHTermoCamera1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbCHTermoCamera1.FormattingEnabled = true;
            this.cbCHTermoCamera1.Location = new System.Drawing.Point(83, 47);
            this.cbCHTermoCamera1.Name = "cbCHTermoCamera1";
            this.cbCHTermoCamera1.Size = new System.Drawing.Size(66, 24);
            this.cbCHTermoCamera1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.AutoScroll = true;
            this.tabPage3.BackColor = System.Drawing.Color.Transparent;
            this.tabPage3.Controls.Add(this.btnVR_SetZero);
            this.tabPage3.Controls.Add(this.groupBox5);
            this.tabPage3.Controls.Add(this.btnVRParamRead);
            this.tabPage3.Controls.Add(this.gbVRLevel4);
            this.tabPage3.Controls.Add(this.gbVRLevel3);
            this.tabPage3.Controls.Add(this.gbVRLevel2);
            this.tabPage3.Controls.Add(this.gbVRLevel1);
            this.tabPage3.Controls.Add(this.groupBox11);
            this.tabPage3.Controls.Add(this.pbVRProcess);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(269, 702);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Верификация";
            // 
            // btnVR_SetZero
            // 
            this.btnVR_SetZero.BackColor = System.Drawing.Color.LightGreen;
            this.btnVR_SetZero.Location = new System.Drawing.Point(8, 672);
            this.btnVR_SetZero.Name = "btnVR_SetZero";
            this.btnVR_SetZero.Size = new System.Drawing.Size(255, 30);
            this.btnVR_SetZero.TabIndex = 16;
            this.btnVR_SetZero.Tag = "4";
            this.btnVR_SetZero.Text = "Установка нуля";
            this.btnVR_SetZero.UseVisualStyleBackColor = false;
            this.btnVR_SetZero.Click += new System.EventHandler(this.btnVR_SetZero_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnVR_VPI_NPI);
            this.groupBox5.Controls.Add(this.label57);
            this.groupBox5.Controls.Add(this.nud_VR_VPI);
            this.groupBox5.Controls.Add(this.label56);
            this.groupBox5.Controls.Add(this.nud_VR_NPI);
            this.groupBox5.Location = new System.Drawing.Point(8, 596);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(255, 70);
            this.groupBox5.TabIndex = 15;
            this.groupBox5.TabStop = false;
            // 
            // btnVR_VPI_NPI
            // 
            this.btnVR_VPI_NPI.BackColor = System.Drawing.Color.LightGreen;
            this.btnVR_VPI_NPI.Location = new System.Drawing.Point(174, 14);
            this.btnVR_VPI_NPI.Name = "btnVR_VPI_NPI";
            this.btnVR_VPI_NPI.Size = new System.Drawing.Size(75, 46);
            this.btnVR_VPI_NPI.TabIndex = 4;
            this.btnVR_VPI_NPI.Text = "Задать";
            this.btnVR_VPI_NPI.UseVisualStyleBackColor = false;
            this.btnVR_VPI_NPI.Click += new System.EventHandler(this.btn_VR_VPI_NPI_Click);
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(7, 17);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(30, 13);
            this.label57.TabIndex = 3;
            this.label57.Text = "ВПИ";
            // 
            // nud_VR_VPI
            // 
            this.nud_VR_VPI.DecimalPlaces = 1;
            this.nud_VR_VPI.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_VR_VPI.Location = new System.Drawing.Point(46, 14);
            this.nud_VR_VPI.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nud_VR_VPI.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.nud_VR_VPI.Name = "nud_VR_VPI";
            this.nud_VR_VPI.Size = new System.Drawing.Size(115, 20);
            this.nud_VR_VPI.TabIndex = 2;
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(7, 42);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(31, 13);
            this.label56.TabIndex = 1;
            this.label56.Text = "НПИ";
            // 
            // nud_VR_NPI
            // 
            this.nud_VR_NPI.DecimalPlaces = 1;
            this.nud_VR_NPI.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_VR_NPI.Location = new System.Drawing.Point(46, 40);
            this.nud_VR_NPI.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nud_VR_NPI.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.nud_VR_NPI.Name = "nud_VR_NPI";
            this.nud_VR_NPI.Size = new System.Drawing.Size(115, 20);
            this.nud_VR_NPI.TabIndex = 0;
            // 
            // btnVRParamRead
            // 
            this.btnVRParamRead.BackColor = System.Drawing.Color.LightGreen;
            this.btnVRParamRead.Location = new System.Drawing.Point(8, 515);
            this.btnVRParamRead.Name = "btnVRParamRead";
            this.btnVRParamRead.Size = new System.Drawing.Size(255, 46);
            this.btnVRParamRead.TabIndex = 13;
            this.btnVRParamRead.Tag = "4";
            this.btnVRParamRead.Text = "Старт верификации";
            this.btnVRParamRead.UseVisualStyleBackColor = false;
            this.btnVRParamRead.Click += new System.EventHandler(this.btnVRParamRead_Click);
            // 
            // gbVRLevel4
            // 
            this.gbVRLevel4.BackColor = System.Drawing.Color.Transparent;
            this.gbVRLevel4.Controls.Add(this.cbVRDiapazon4);
            this.gbVRLevel4.Controls.Add(this.label53);
            this.gbVRLevel4.Controls.Add(this.btnVRPressureSet4);
            this.gbVRLevel4.Controls.Add(this.cbVRPressureSet4);
            this.gbVRLevel4.Controls.Add(this.btnVRTemperatureSet4);
            this.gbVRLevel4.Controls.Add(this.label54);
            this.gbVRLevel4.Controls.Add(this.label55);
            this.gbVRLevel4.Controls.Add(this.cbVRTermoCamera4);
            this.gbVRLevel4.Enabled = false;
            this.gbVRLevel4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbVRLevel4.Location = new System.Drawing.Point(8, 401);
            this.gbVRLevel4.Name = "gbVRLevel4";
            this.gbVRLevel4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.gbVRLevel4.Size = new System.Drawing.Size(255, 105);
            this.gbVRLevel4.TabIndex = 12;
            this.gbVRLevel4.TabStop = false;
            this.gbVRLevel4.Tag = "4";
            this.gbVRLevel4.Text = "Уровень 4";
            this.gbVRLevel4.Enter += new System.EventHandler(this.gbVRLevel1_Enter);
            // 
            // cbVRDiapazon4
            // 
            this.cbVRDiapazon4.AutoCompleteCustomSource.AddRange(new string[] {
            "1",
            "2"});
            this.cbVRDiapazon4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVRDiapazon4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbVRDiapazon4.FormattingEnabled = true;
            this.cbVRDiapazon4.Items.AddRange(new object[] {
            "1",
            "2"});
            this.cbVRDiapazon4.Location = new System.Drawing.Point(83, 17);
            this.cbVRDiapazon4.Name = "cbVRDiapazon4";
            this.cbVRDiapazon4.Size = new System.Drawing.Size(66, 24);
            this.cbVRDiapazon4.TabIndex = 9;
            this.cbVRDiapazon4.SelectedIndexChanged += new System.EventHandler(this.cbVRDiapazon4_SelectedIndexChanged);
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(3, 22);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(61, 13);
            this.label53.TabIndex = 8;
            this.label53.Text = "Диапазон:";
            // 
            // btnVRPressureSet4
            // 
            this.btnVRPressureSet4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnVRPressureSet4.Location = new System.Drawing.Point(151, 77);
            this.btnVRPressureSet4.Name = "btnVRPressureSet4";
            this.btnVRPressureSet4.Size = new System.Drawing.Size(98, 25);
            this.btnVRPressureSet4.TabIndex = 7;
            this.btnVRPressureSet4.Tag = "4";
            this.btnVRPressureSet4.Text = "Установить";
            this.btnVRPressureSet4.UseVisualStyleBackColor = true;
            this.btnVRPressureSet4.Click += new System.EventHandler(this.btnVRPressureSet1_Click);
            // 
            // cbVRPressureSet4
            // 
            this.cbVRPressureSet4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbVRPressureSet4.FormattingEnabled = true;
            this.cbVRPressureSet4.Location = new System.Drawing.Point(83, 77);
            this.cbVRPressureSet4.Name = "cbVRPressureSet4";
            this.cbVRPressureSet4.Size = new System.Drawing.Size(66, 24);
            this.cbVRPressureSet4.TabIndex = 6;
            // 
            // btnVRTemperatureSet4
            // 
            this.btnVRTemperatureSet4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnVRTemperatureSet4.Location = new System.Drawing.Point(151, 47);
            this.btnVRTemperatureSet4.Name = "btnVRTemperatureSet4";
            this.btnVRTemperatureSet4.Size = new System.Drawing.Size(98, 25);
            this.btnVRTemperatureSet4.TabIndex = 5;
            this.btnVRTemperatureSet4.Tag = "4";
            this.btnVRTemperatureSet4.Text = "Установить";
            this.btnVRTemperatureSet4.UseVisualStyleBackColor = true;
            this.btnVRTemperatureSet4.Click += new System.EventHandler(this.btnVRTemperatureSet1_Click);
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(3, 82);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(61, 13);
            this.label54.TabIndex = 3;
            this.label54.Text = "Давление:";
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(3, 52);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(77, 13);
            this.label55.TabIndex = 2;
            this.label55.Text = "Температура:";
            // 
            // cbVRTermoCamera4
            // 
            this.cbVRTermoCamera4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbVRTermoCamera4.FormattingEnabled = true;
            this.cbVRTermoCamera4.Location = new System.Drawing.Point(83, 47);
            this.cbVRTermoCamera4.Name = "cbVRTermoCamera4";
            this.cbVRTermoCamera4.Size = new System.Drawing.Size(66, 24);
            this.cbVRTermoCamera4.TabIndex = 0;
            // 
            // gbVRLevel3
            // 
            this.gbVRLevel3.BackColor = System.Drawing.Color.Transparent;
            this.gbVRLevel3.Controls.Add(this.cbVRDiapazon3);
            this.gbVRLevel3.Controls.Add(this.label50);
            this.gbVRLevel3.Controls.Add(this.btnVRPressureSet3);
            this.gbVRLevel3.Controls.Add(this.cbVRPressureSet3);
            this.gbVRLevel3.Controls.Add(this.btnVRTemperatureSet3);
            this.gbVRLevel3.Controls.Add(this.label51);
            this.gbVRLevel3.Controls.Add(this.label52);
            this.gbVRLevel3.Controls.Add(this.cbVRTermoCamera3);
            this.gbVRLevel3.Enabled = false;
            this.gbVRLevel3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbVRLevel3.Location = new System.Drawing.Point(8, 290);
            this.gbVRLevel3.Name = "gbVRLevel3";
            this.gbVRLevel3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.gbVRLevel3.Size = new System.Drawing.Size(255, 105);
            this.gbVRLevel3.TabIndex = 11;
            this.gbVRLevel3.TabStop = false;
            this.gbVRLevel3.Tag = "3";
            this.gbVRLevel3.Text = "Уровень 3";
            this.gbVRLevel3.Enter += new System.EventHandler(this.gbVRLevel1_Enter);
            // 
            // cbVRDiapazon3
            // 
            this.cbVRDiapazon3.AutoCompleteCustomSource.AddRange(new string[] {
            "1",
            "2"});
            this.cbVRDiapazon3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVRDiapazon3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbVRDiapazon3.FormattingEnabled = true;
            this.cbVRDiapazon3.Items.AddRange(new object[] {
            "1",
            "2"});
            this.cbVRDiapazon3.Location = new System.Drawing.Point(83, 17);
            this.cbVRDiapazon3.Name = "cbVRDiapazon3";
            this.cbVRDiapazon3.Size = new System.Drawing.Size(66, 24);
            this.cbVRDiapazon3.TabIndex = 9;
            this.cbVRDiapazon3.SelectedIndexChanged += new System.EventHandler(this.cbVRDiapazon3_SelectedIndexChanged);
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(3, 22);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(61, 13);
            this.label50.TabIndex = 8;
            this.label50.Text = "Диапазон:";
            // 
            // btnVRPressureSet3
            // 
            this.btnVRPressureSet3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnVRPressureSet3.Location = new System.Drawing.Point(151, 77);
            this.btnVRPressureSet3.Name = "btnVRPressureSet3";
            this.btnVRPressureSet3.Size = new System.Drawing.Size(98, 25);
            this.btnVRPressureSet3.TabIndex = 7;
            this.btnVRPressureSet3.Tag = "3";
            this.btnVRPressureSet3.Text = "Установить";
            this.btnVRPressureSet3.UseVisualStyleBackColor = true;
            this.btnVRPressureSet3.Click += new System.EventHandler(this.btnVRPressureSet1_Click);
            // 
            // cbVRPressureSet3
            // 
            this.cbVRPressureSet3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbVRPressureSet3.FormattingEnabled = true;
            this.cbVRPressureSet3.Location = new System.Drawing.Point(83, 77);
            this.cbVRPressureSet3.Name = "cbVRPressureSet3";
            this.cbVRPressureSet3.Size = new System.Drawing.Size(66, 24);
            this.cbVRPressureSet3.TabIndex = 6;
            // 
            // btnVRTemperatureSet3
            // 
            this.btnVRTemperatureSet3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnVRTemperatureSet3.Location = new System.Drawing.Point(151, 47);
            this.btnVRTemperatureSet3.Name = "btnVRTemperatureSet3";
            this.btnVRTemperatureSet3.Size = new System.Drawing.Size(98, 25);
            this.btnVRTemperatureSet3.TabIndex = 5;
            this.btnVRTemperatureSet3.Tag = "3";
            this.btnVRTemperatureSet3.Text = "Установить";
            this.btnVRTemperatureSet3.UseVisualStyleBackColor = true;
            this.btnVRTemperatureSet3.Click += new System.EventHandler(this.btnVRTemperatureSet1_Click);
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(3, 82);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(61, 13);
            this.label51.TabIndex = 3;
            this.label51.Text = "Давление:";
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(3, 52);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(77, 13);
            this.label52.TabIndex = 2;
            this.label52.Text = "Температура:";
            // 
            // cbVRTermoCamera3
            // 
            this.cbVRTermoCamera3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbVRTermoCamera3.FormattingEnabled = true;
            this.cbVRTermoCamera3.Location = new System.Drawing.Point(83, 47);
            this.cbVRTermoCamera3.Name = "cbVRTermoCamera3";
            this.cbVRTermoCamera3.Size = new System.Drawing.Size(66, 24);
            this.cbVRTermoCamera3.TabIndex = 0;
            // 
            // gbVRLevel2
            // 
            this.gbVRLevel2.BackColor = System.Drawing.Color.Transparent;
            this.gbVRLevel2.Controls.Add(this.cbVRDiapazon2);
            this.gbVRLevel2.Controls.Add(this.label12);
            this.gbVRLevel2.Controls.Add(this.btnVRPressureSet2);
            this.gbVRLevel2.Controls.Add(this.cbVRPressureSet2);
            this.gbVRLevel2.Controls.Add(this.btnVRTemperatureSet2);
            this.gbVRLevel2.Controls.Add(this.label45);
            this.gbVRLevel2.Controls.Add(this.label49);
            this.gbVRLevel2.Controls.Add(this.cbVRTermoCamera2);
            this.gbVRLevel2.Enabled = false;
            this.gbVRLevel2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbVRLevel2.Location = new System.Drawing.Point(8, 179);
            this.gbVRLevel2.Name = "gbVRLevel2";
            this.gbVRLevel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.gbVRLevel2.Size = new System.Drawing.Size(255, 105);
            this.gbVRLevel2.TabIndex = 10;
            this.gbVRLevel2.TabStop = false;
            this.gbVRLevel2.Tag = "2";
            this.gbVRLevel2.Text = "Уровень 2";
            this.gbVRLevel2.Enter += new System.EventHandler(this.gbVRLevel1_Enter);
            // 
            // cbVRDiapazon2
            // 
            this.cbVRDiapazon2.AutoCompleteCustomSource.AddRange(new string[] {
            "1",
            "2"});
            this.cbVRDiapazon2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVRDiapazon2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbVRDiapazon2.FormattingEnabled = true;
            this.cbVRDiapazon2.Items.AddRange(new object[] {
            "1",
            "2"});
            this.cbVRDiapazon2.Location = new System.Drawing.Point(83, 17);
            this.cbVRDiapazon2.Name = "cbVRDiapazon2";
            this.cbVRDiapazon2.Size = new System.Drawing.Size(66, 24);
            this.cbVRDiapazon2.TabIndex = 9;
            this.cbVRDiapazon2.SelectedIndexChanged += new System.EventHandler(this.cbVRDiapazon2_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 22);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(61, 13);
            this.label12.TabIndex = 8;
            this.label12.Text = "Диапазон:";
            // 
            // btnVRPressureSet2
            // 
            this.btnVRPressureSet2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnVRPressureSet2.Location = new System.Drawing.Point(151, 77);
            this.btnVRPressureSet2.Name = "btnVRPressureSet2";
            this.btnVRPressureSet2.Size = new System.Drawing.Size(98, 25);
            this.btnVRPressureSet2.TabIndex = 7;
            this.btnVRPressureSet2.Tag = "2";
            this.btnVRPressureSet2.Text = "Установить";
            this.btnVRPressureSet2.UseVisualStyleBackColor = true;
            this.btnVRPressureSet2.Click += new System.EventHandler(this.btnVRPressureSet1_Click);
            // 
            // cbVRPressureSet2
            // 
            this.cbVRPressureSet2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbVRPressureSet2.FormattingEnabled = true;
            this.cbVRPressureSet2.Location = new System.Drawing.Point(83, 77);
            this.cbVRPressureSet2.Name = "cbVRPressureSet2";
            this.cbVRPressureSet2.Size = new System.Drawing.Size(66, 24);
            this.cbVRPressureSet2.TabIndex = 6;
            // 
            // btnVRTemperatureSet2
            // 
            this.btnVRTemperatureSet2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnVRTemperatureSet2.Location = new System.Drawing.Point(151, 47);
            this.btnVRTemperatureSet2.Name = "btnVRTemperatureSet2";
            this.btnVRTemperatureSet2.Size = new System.Drawing.Size(98, 25);
            this.btnVRTemperatureSet2.TabIndex = 5;
            this.btnVRTemperatureSet2.Tag = "2";
            this.btnVRTemperatureSet2.Text = "Установить";
            this.btnVRTemperatureSet2.UseVisualStyleBackColor = true;
            this.btnVRTemperatureSet2.Click += new System.EventHandler(this.btnVRTemperatureSet1_Click);
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(3, 82);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(61, 13);
            this.label45.TabIndex = 3;
            this.label45.Text = "Давление:";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(3, 52);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(77, 13);
            this.label49.TabIndex = 2;
            this.label49.Text = "Температура:";
            // 
            // cbVRTermoCamera2
            // 
            this.cbVRTermoCamera2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbVRTermoCamera2.FormattingEnabled = true;
            this.cbVRTermoCamera2.Location = new System.Drawing.Point(83, 47);
            this.cbVRTermoCamera2.Name = "cbVRTermoCamera2";
            this.cbVRTermoCamera2.Size = new System.Drawing.Size(66, 24);
            this.cbVRTermoCamera2.TabIndex = 0;
            // 
            // gbVRLevel1
            // 
            this.gbVRLevel1.BackColor = System.Drawing.Color.LightGreen;
            this.gbVRLevel1.Controls.Add(this.cbVRDiapazon1);
            this.gbVRLevel1.Controls.Add(this.label11);
            this.gbVRLevel1.Controls.Add(this.btnVRPressureSet1);
            this.gbVRLevel1.Controls.Add(this.cbVRPressureSet1);
            this.gbVRLevel1.Controls.Add(this.btnVRTemperatureSet1);
            this.gbVRLevel1.Controls.Add(this.label47);
            this.gbVRLevel1.Controls.Add(this.label48);
            this.gbVRLevel1.Controls.Add(this.cbVRTermoCamera1);
            this.gbVRLevel1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbVRLevel1.Location = new System.Drawing.Point(8, 68);
            this.gbVRLevel1.Name = "gbVRLevel1";
            this.gbVRLevel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.gbVRLevel1.Size = new System.Drawing.Size(255, 105);
            this.gbVRLevel1.TabIndex = 9;
            this.gbVRLevel1.TabStop = false;
            this.gbVRLevel1.Tag = "1";
            this.gbVRLevel1.Text = "Уровень 1";
            this.gbVRLevel1.Enter += new System.EventHandler(this.gbVRLevel1_Enter);
            // 
            // cbVRDiapazon1
            // 
            this.cbVRDiapazon1.AutoCompleteCustomSource.AddRange(new string[] {
            "1",
            "2"});
            this.cbVRDiapazon1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVRDiapazon1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbVRDiapazon1.FormattingEnabled = true;
            this.cbVRDiapazon1.Items.AddRange(new object[] {
            "1",
            "2"});
            this.cbVRDiapazon1.Location = new System.Drawing.Point(83, 17);
            this.cbVRDiapazon1.Name = "cbVRDiapazon1";
            this.cbVRDiapazon1.Size = new System.Drawing.Size(66, 24);
            this.cbVRDiapazon1.TabIndex = 9;
            this.cbVRDiapazon1.SelectedIndexChanged += new System.EventHandler(this.cbVRDiapazon1_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(61, 13);
            this.label11.TabIndex = 8;
            this.label11.Text = "Диапазон:";
            // 
            // btnVRPressureSet1
            // 
            this.btnVRPressureSet1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnVRPressureSet1.Location = new System.Drawing.Point(151, 77);
            this.btnVRPressureSet1.Name = "btnVRPressureSet1";
            this.btnVRPressureSet1.Size = new System.Drawing.Size(98, 25);
            this.btnVRPressureSet1.TabIndex = 7;
            this.btnVRPressureSet1.Tag = "1";
            this.btnVRPressureSet1.Text = "Установить";
            this.btnVRPressureSet1.UseVisualStyleBackColor = true;
            this.btnVRPressureSet1.Click += new System.EventHandler(this.btnVRPressureSet1_Click);
            // 
            // cbVRPressureSet1
            // 
            this.cbVRPressureSet1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbVRPressureSet1.FormattingEnabled = true;
            this.cbVRPressureSet1.Location = new System.Drawing.Point(83, 77);
            this.cbVRPressureSet1.Name = "cbVRPressureSet1";
            this.cbVRPressureSet1.Size = new System.Drawing.Size(66, 24);
            this.cbVRPressureSet1.TabIndex = 6;
            // 
            // btnVRTemperatureSet1
            // 
            this.btnVRTemperatureSet1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnVRTemperatureSet1.Location = new System.Drawing.Point(151, 47);
            this.btnVRTemperatureSet1.Name = "btnVRTemperatureSet1";
            this.btnVRTemperatureSet1.Size = new System.Drawing.Size(98, 25);
            this.btnVRTemperatureSet1.TabIndex = 5;
            this.btnVRTemperatureSet1.Tag = "1";
            this.btnVRTemperatureSet1.Text = "Установить";
            this.btnVRTemperatureSet1.UseVisualStyleBackColor = true;
            this.btnVRTemperatureSet1.Click += new System.EventHandler(this.btnVRTemperatureSet1_Click);
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(3, 82);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(61, 13);
            this.label47.TabIndex = 3;
            this.label47.Text = "Давление:";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(3, 52);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(77, 13);
            this.label48.TabIndex = 2;
            this.label48.Text = "Температура:";
            // 
            // cbVRTermoCamera1
            // 
            this.cbVRTermoCamera1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbVRTermoCamera1.FormattingEnabled = true;
            this.cbVRTermoCamera1.Location = new System.Drawing.Point(83, 47);
            this.cbVRTermoCamera1.Name = "cbVRTermoCamera1";
            this.cbVRTermoCamera1.Size = new System.Drawing.Size(66, 24);
            this.cbVRTermoCamera1.TabIndex = 0;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.cbChannalFixVR);
            this.groupBox11.Controls.Add(this.cbChannalVerification);
            this.groupBox11.Location = new System.Drawing.Point(8, 13);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(255, 47);
            this.groupBox11.TabIndex = 6;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Текущий канал";
            // 
            // cbChannalFixVR
            // 
            this.cbChannalFixVR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbChannalFixVR.Location = new System.Drawing.Point(223, 18);
            this.cbChannalFixVR.MaximumSize = new System.Drawing.Size(20, 20);
            this.cbChannalFixVR.MinimumSize = new System.Drawing.Size(20, 20);
            this.cbChannalFixVR.Name = "cbChannalFixVR";
            this.cbChannalFixVR.Size = new System.Drawing.Size(20, 20);
            this.cbChannalFixVR.TabIndex = 11;
            this.cbChannalFixVR.UseVisualStyleBackColor = true;
            // 
            // cbChannalVerification
            // 
            this.cbChannalVerification.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbChannalVerification.FormattingEnabled = true;
            this.cbChannalVerification.Location = new System.Drawing.Point(6, 18);
            this.cbChannalVerification.Name = "cbChannalVerification";
            this.cbChannalVerification.Size = new System.Drawing.Size(211, 21);
            this.cbChannalVerification.TabIndex = 0;
            this.cbChannalVerification.SelectedIndexChanged += new System.EventHandler(this.cbChannalVerification_SelectedIndexChanged);
            // 
            // pbVRProcess
            // 
            this.pbVRProcess.Location = new System.Drawing.Point(8, 567);
            this.pbVRProcess.Name = "pbVRProcess";
            this.pbVRProcess.Size = new System.Drawing.Size(255, 23);
            this.pbVRProcess.TabIndex = 4;
            // 
            // tabPage4
            // 
            this.tabPage4.AutoScroll = true;
            this.tabPage4.Controls.Add(this.groupBox4);
            this.tabPage4.Controls.Add(this.btn_MET_Start);
            this.tabPage4.Controls.Add(this.pbMETProcess);
            this.tabPage4.Controls.Add(this.groupBox3);
            this.tabPage4.Controls.Add(this.groupBox2);
            this.tabPage4.Controls.Add(this.btn_MET_SetZero);
            this.tabPage4.Controls.Add(this.groupBox1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(269, 702);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Сдача метрологу";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cbChannalFixMET);
            this.groupBox4.Controls.Add(this.cbChannalMetrolog);
            this.groupBox4.Location = new System.Drawing.Point(6, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(255, 47);
            this.groupBox4.TabIndex = 23;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Текущий канал";
            // 
            // cbChannalFixMET
            // 
            this.cbChannalFixMET.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbChannalFixMET.Location = new System.Drawing.Point(223, 18);
            this.cbChannalFixMET.MaximumSize = new System.Drawing.Size(20, 20);
            this.cbChannalFixMET.MinimumSize = new System.Drawing.Size(20, 20);
            this.cbChannalFixMET.Name = "cbChannalFixMET";
            this.cbChannalFixMET.Size = new System.Drawing.Size(20, 20);
            this.cbChannalFixMET.TabIndex = 11;
            this.cbChannalFixMET.UseVisualStyleBackColor = true;
            // 
            // cbChannalMetrolog
            // 
            this.cbChannalMetrolog.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbChannalMetrolog.FormattingEnabled = true;
            this.cbChannalMetrolog.Location = new System.Drawing.Point(6, 18);
            this.cbChannalMetrolog.Name = "cbChannalMetrolog";
            this.cbChannalMetrolog.Size = new System.Drawing.Size(211, 21);
            this.cbChannalMetrolog.TabIndex = 0;
            this.cbChannalMetrolog.SelectedIndexChanged += new System.EventHandler(this.cbChannalMetrolog_SelectedIndexChanged);
            // 
            // btn_MET_Start
            // 
            this.btn_MET_Start.BackColor = System.Drawing.Color.LightGreen;
            this.btn_MET_Start.Location = new System.Drawing.Point(6, 570);
            this.btn_MET_Start.Name = "btn_MET_Start";
            this.btn_MET_Start.Size = new System.Drawing.Size(255, 46);
            this.btn_MET_Start.TabIndex = 22;
            this.btn_MET_Start.Text = "Старт";
            this.btn_MET_Start.UseVisualStyleBackColor = false;
            this.btn_MET_Start.Click += new System.EventHandler(this.btn_MET_Start_Click);
            // 
            // pbMETProcess
            // 
            this.pbMETProcess.Location = new System.Drawing.Point(6, 541);
            this.pbMETProcess.Name = "pbMETProcess";
            this.pbMETProcess.Size = new System.Drawing.Size(255, 23);
            this.pbMETProcess.TabIndex = 21;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_MET_DTime);
            this.groupBox3.Controls.Add(this.nud_MET_DTime);
            this.groupBox3.Location = new System.Drawing.Point(6, 429);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(255, 70);
            this.groupBox3.TabIndex = 20;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Время демпфирования (сек)";
            // 
            // btn_MET_DTime
            // 
            this.btn_MET_DTime.BackColor = System.Drawing.Color.LightGreen;
            this.btn_MET_DTime.Location = new System.Drawing.Point(174, 14);
            this.btn_MET_DTime.Name = "btn_MET_DTime";
            this.btn_MET_DTime.Size = new System.Drawing.Size(75, 46);
            this.btn_MET_DTime.TabIndex = 4;
            this.btn_MET_DTime.Text = "Задать";
            this.btn_MET_DTime.UseVisualStyleBackColor = false;
            this.btn_MET_DTime.Click += new System.EventHandler(this.btn_MET_DTime_Click);
            // 
            // nud_MET_DTime
            // 
            this.nud_MET_DTime.DecimalPlaces = 1;
            this.nud_MET_DTime.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_MET_DTime.Location = new System.Drawing.Point(10, 29);
            this.nud_MET_DTime.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nud_MET_DTime.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.nud_MET_DTime.Name = "nud_MET_DTime";
            this.nud_MET_DTime.Size = new System.Drawing.Size(151, 20);
            this.nud_MET_DTime.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_MET_Down);
            this.groupBox2.Controls.Add(this.btn_MET_Up);
            this.groupBox2.Controls.Add(this.btn_MET_Del);
            this.groupBox2.Controls.Add(this.btn_MET_Add);
            this.groupBox2.Controls.Add(this.lb_MET_PressValue);
            this.groupBox2.Controls.Add(this.label62);
            this.groupBox2.Controls.Add(this.cb_MET_Unit);
            this.groupBox2.Location = new System.Drawing.Point(6, 132);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(255, 291);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Установка ряда давлений";
            // 
            // btn_MET_Down
            // 
            this.btn_MET_Down.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_MET_Down.Location = new System.Drawing.Point(200, 224);
            this.btn_MET_Down.Name = "btn_MET_Down";
            this.btn_MET_Down.Size = new System.Drawing.Size(48, 48);
            this.btn_MET_Down.TabIndex = 6;
            this.btn_MET_Down.Text = "<";
            this.btn_MET_Down.UseVisualStyleBackColor = true;
            this.btn_MET_Down.Click += new System.EventHandler(this.btn_MET_Down_Click);
            // 
            // btn_MET_Up
            // 
            this.btn_MET_Up.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_MET_Up.Location = new System.Drawing.Point(200, 170);
            this.btn_MET_Up.Name = "btn_MET_Up";
            this.btn_MET_Up.Size = new System.Drawing.Size(48, 48);
            this.btn_MET_Up.TabIndex = 5;
            this.btn_MET_Up.Text = ">";
            this.btn_MET_Up.UseVisualStyleBackColor = true;
            this.btn_MET_Up.Click += new System.EventHandler(this.btn_MET_Up_Click);
            // 
            // btn_MET_Del
            // 
            this.btn_MET_Del.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_MET_Del.Location = new System.Drawing.Point(200, 116);
            this.btn_MET_Del.Name = "btn_MET_Del";
            this.btn_MET_Del.Size = new System.Drawing.Size(48, 48);
            this.btn_MET_Del.TabIndex = 4;
            this.btn_MET_Del.Text = "-";
            this.btn_MET_Del.UseVisualStyleBackColor = true;
            this.btn_MET_Del.Click += new System.EventHandler(this.btn_MET_Del_Click);
            // 
            // btn_MET_Add
            // 
            this.btn_MET_Add.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_MET_Add.Location = new System.Drawing.Point(200, 62);
            this.btn_MET_Add.Name = "btn_MET_Add";
            this.btn_MET_Add.Size = new System.Drawing.Size(48, 48);
            this.btn_MET_Add.TabIndex = 3;
            this.btn_MET_Add.Text = "+";
            this.btn_MET_Add.UseVisualStyleBackColor = true;
            this.btn_MET_Add.Click += new System.EventHandler(this.btn_MET_Add_Click);
            // 
            // lb_MET_PressValue
            // 
            this.lb_MET_PressValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lb_MET_PressValue.FormattingEnabled = true;
            this.lb_MET_PressValue.ItemHeight = 24;
            this.lb_MET_PressValue.Items.AddRange(new object[] {
            "0",
            "50",
            "100",
            "150",
            "200"});
            this.lb_MET_PressValue.Location = new System.Drawing.Point(10, 62);
            this.lb_MET_PressValue.Name = "lb_MET_PressValue";
            this.lb_MET_PressValue.Size = new System.Drawing.Size(184, 220);
            this.lb_MET_PressValue.TabIndex = 2;
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Location = new System.Drawing.Point(7, 28);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(114, 13);
            this.label62.TabIndex = 1;
            this.label62.Text = "Единицы измерения:";
            // 
            // cb_MET_Unit
            // 
            this.cb_MET_Unit.DisplayMember = "0";
            this.cb_MET_Unit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_MET_Unit.FormattingEnabled = true;
            this.cb_MET_Unit.Items.AddRange(new object[] {
            "кПа",
            "МПа",
            "бар",
            "psi"});
            this.cb_MET_Unit.Location = new System.Drawing.Point(127, 25);
            this.cb_MET_Unit.Name = "cb_MET_Unit";
            this.cb_MET_Unit.Size = new System.Drawing.Size(121, 21);
            this.cb_MET_Unit.TabIndex = 0;
            this.cb_MET_Unit.SelectedIndexChanged += new System.EventHandler(this.cb_MET_Unit_SelectedIndexChanged);
            // 
            // btn_MET_SetZero
            // 
            this.btn_MET_SetZero.BackColor = System.Drawing.Color.LightGreen;
            this.btn_MET_SetZero.Location = new System.Drawing.Point(6, 505);
            this.btn_MET_SetZero.Name = "btn_MET_SetZero";
            this.btn_MET_SetZero.Size = new System.Drawing.Size(255, 30);
            this.btn_MET_SetZero.TabIndex = 18;
            this.btn_MET_SetZero.Tag = "4";
            this.btn_MET_SetZero.Text = "Установка нуля";
            this.btn_MET_SetZero.UseVisualStyleBackColor = false;
            this.btn_MET_SetZero.Click += new System.EventHandler(this.btn_MET_SetZero_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_MET_NPI_VPI);
            this.groupBox1.Controls.Add(this.label59);
            this.groupBox1.Controls.Add(this.nud_MET_VPI);
            this.groupBox1.Controls.Add(this.label60);
            this.groupBox1.Controls.Add(this.nud_MET_NPI);
            this.groupBox1.Location = new System.Drawing.Point(6, 56);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(255, 70);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            // 
            // btn_MET_NPI_VPI
            // 
            this.btn_MET_NPI_VPI.BackColor = System.Drawing.Color.LightGreen;
            this.btn_MET_NPI_VPI.Location = new System.Drawing.Point(174, 14);
            this.btn_MET_NPI_VPI.Name = "btn_MET_NPI_VPI";
            this.btn_MET_NPI_VPI.Size = new System.Drawing.Size(75, 46);
            this.btn_MET_NPI_VPI.TabIndex = 4;
            this.btn_MET_NPI_VPI.Text = "Задать";
            this.btn_MET_NPI_VPI.UseVisualStyleBackColor = false;
            this.btn_MET_NPI_VPI.Click += new System.EventHandler(this.btn_MET_NPI_VPI_Click);
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Location = new System.Drawing.Point(7, 17);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(30, 13);
            this.label59.TabIndex = 3;
            this.label59.Text = "ВПИ";
            // 
            // nud_MET_VPI
            // 
            this.nud_MET_VPI.DecimalPlaces = 1;
            this.nud_MET_VPI.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_MET_VPI.Location = new System.Drawing.Point(46, 14);
            this.nud_MET_VPI.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nud_MET_VPI.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.nud_MET_VPI.Name = "nud_MET_VPI";
            this.nud_MET_VPI.Size = new System.Drawing.Size(115, 20);
            this.nud_MET_VPI.TabIndex = 2;
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Location = new System.Drawing.Point(7, 42);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(31, 13);
            this.label60.TabIndex = 1;
            this.label60.Text = "НПИ";
            // 
            // nud_MET_NPI
            // 
            this.nud_MET_NPI.DecimalPlaces = 1;
            this.nud_MET_NPI.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_MET_NPI.Location = new System.Drawing.Point(46, 40);
            this.nud_MET_NPI.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nud_MET_NPI.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.nud_MET_NPI.Name = "nud_MET_NPI";
            this.nud_MET_NPI.Size = new System.Drawing.Size(115, 20);
            this.nud_MET_NPI.TabIndex = 0;
            // 
            // MainTimer
            // 
            this.MainTimer.Interval = 1000;
            this.MainTimer.Tick += new System.EventHandler(this.MainTimer_Tick);
            // 
            // pUpStatusBar
            // 
            this.pUpStatusBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pUpStatusBar.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.pUpStatusBar.Controls.Add(this.label_UpStPressure);
            this.pUpStatusBar.Controls.Add(this.label58);
            this.pUpStatusBar.Controls.Add(this.label_UpStResistance);
            this.pUpStatusBar.Controls.Add(this.label_UpStVoltage);
            this.pUpStatusBar.Controls.Add(this.label_UpStR);
            this.pUpStatusBar.Controls.Add(this.label_UpStV);
            this.pUpStatusBar.Controls.Add(this.cbSensorPeriodRead);
            this.pUpStatusBar.Controls.Add(this.UpStCh);
            this.pUpStatusBar.Controls.Add(this.UpStSerial);
            this.pUpStatusBar.Controls.Add(this.UpStModel);
            this.pUpStatusBar.Controls.Add(this.label46);
            this.pUpStatusBar.Controls.Add(this.label36);
            this.pUpStatusBar.Controls.Add(this.label5);
            this.pUpStatusBar.Location = new System.Drawing.Point(277, 1);
            this.pUpStatusBar.Name = "pUpStatusBar";
            this.pUpStatusBar.Size = new System.Drawing.Size(1169, 22);
            this.pUpStatusBar.TabIndex = 9;
            this.pUpStatusBar.Visible = false;
            // 
            // label_UpStPressure
            // 
            this.label_UpStPressure.AutoSize = true;
            this.label_UpStPressure.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_UpStPressure.Location = new System.Drawing.Point(1080, 3);
            this.label_UpStPressure.Name = "label_UpStPressure";
            this.label_UpStPressure.Size = new System.Drawing.Size(23, 15);
            this.label_UpStPressure.TabIndex = 16;
            this.label_UpStPressure.Text = "    ";
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label58.Location = new System.Drawing.Point(966, 4);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(113, 15);
            this.label58.TabIndex = 15;
            this.label58.Text = "|  Давление (кПа) :";
            // 
            // label_UpStResistance
            // 
            this.label_UpStResistance.AutoSize = true;
            this.label_UpStResistance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_UpStResistance.Location = new System.Drawing.Point(902, 4);
            this.label_UpStResistance.Name = "label_UpStResistance";
            this.label_UpStResistance.Size = new System.Drawing.Size(23, 15);
            this.label_UpStResistance.TabIndex = 14;
            this.label_UpStResistance.Text = "    ";
            // 
            // label_UpStVoltage
            // 
            this.label_UpStVoltage.AutoSize = true;
            this.label_UpStVoltage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_UpStVoltage.Location = new System.Drawing.Point(706, 4);
            this.label_UpStVoltage.Name = "label_UpStVoltage";
            this.label_UpStVoltage.Size = new System.Drawing.Size(23, 15);
            this.label_UpStVoltage.TabIndex = 13;
            this.label_UpStVoltage.Text = "    ";
            // 
            // label_UpStR
            // 
            this.label_UpStR.AutoSize = true;
            this.label_UpStR.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_UpStR.Location = new System.Drawing.Point(759, 4);
            this.label_UpStR.Name = "label_UpStR";
            this.label_UpStR.Size = new System.Drawing.Size(143, 15);
            this.label_UpStR.TabIndex = 10;
            this.label_UpStR.Text = "|   Сопротивление(Ом) :";
            // 
            // label_UpStV
            // 
            this.label_UpStV.AutoSize = true;
            this.label_UpStV.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_UpStV.Location = new System.Drawing.Point(582, 4);
            this.label_UpStV.Name = "label_UpStV";
            this.label_UpStV.Size = new System.Drawing.Size(127, 15);
            this.label_UpStV.TabIndex = 9;
            this.label_UpStV.Text = "|   Напряжение (мВ): ";
            // 
            // cbSensorPeriodRead
            // 
            this.cbSensorPeriodRead.AutoSize = true;
            this.cbSensorPeriodRead.Location = new System.Drawing.Point(1185, 3);
            this.cbSensorPeriodRead.Name = "cbSensorPeriodRead";
            this.cbSensorPeriodRead.Size = new System.Drawing.Size(142, 17);
            this.cbSensorPeriodRead.TabIndex = 8;
            this.cbSensorPeriodRead.Text = "Периодическое чтение";
            this.cbSensorPeriodRead.UseVisualStyleBackColor = true;
            this.cbSensorPeriodRead.CheckedChanged += new System.EventHandler(this.cbSensorPeriodRead_CheckedChanged);
            // 
            // UpStCh
            // 
            this.UpStCh.AutoSize = true;
            this.UpStCh.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.UpStCh.Location = new System.Drawing.Point(129, 4);
            this.UpStCh.Name = "UpStCh";
            this.UpStCh.Size = new System.Drawing.Size(23, 15);
            this.UpStCh.TabIndex = 7;
            this.UpStCh.Text = "    ";
            // 
            // UpStSerial
            // 
            this.UpStSerial.AutoSize = true;
            this.UpStSerial.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.UpStSerial.Location = new System.Drawing.Point(312, 4);
            this.UpStSerial.Name = "UpStSerial";
            this.UpStSerial.Size = new System.Drawing.Size(23, 15);
            this.UpStSerial.TabIndex = 6;
            this.UpStSerial.Text = "    ";
            // 
            // UpStModel
            // 
            this.UpStModel.AutoSize = true;
            this.UpStModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.UpStModel.Location = new System.Drawing.Point(475, 3);
            this.UpStModel.Name = "UpStModel";
            this.UpStModel.Size = new System.Drawing.Size(23, 15);
            this.UpStModel.TabIndex = 5;
            this.UpStModel.Text = "    ";
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label46.Location = new System.Drawing.Point(186, 4);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(127, 15);
            this.label46.TabIndex = 4;
            this.label46.Text = "|   Заводской номер: ";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label36.Location = new System.Drawing.Point(408, 4);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(71, 15);
            this.label36.TabIndex = 2;
            this.label36.Text = "|   Модель: ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(3, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(128, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "      |   Текущий канал: ";
            // 
            // splitter2
            // 
            this.splitter2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 749);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(1138, 3);
            this.splitter2.TabIndex = 10;
            this.splitter2.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1447, 996);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.pUpStatusBar);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panelLog);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Программа характеризации датчиков давления";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbTermoCamera.ResumeLayout(false);
            this.gbTermoCamera.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTermoCameraPoint)).EndInit();
            this.gbMensor.ResumeLayout(false);
            this.gbMensor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMensorPoint)).EndInit();
            this.gbMultimetr.ResumeLayout(false);
            this.gbMultimetr.PerformLayout();
            this.gbCommutator.ResumeLayout(false);
            this.gbCommutator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.cmsCharacterizationTable.ResumeLayout(false);
            this.panelLog.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView5)).EndInit();
            this.cmsVerificationTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).EndInit();
            this.cmsCurentTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.gbCHLevel4.ResumeLayout(false);
            this.gbCHLevel4.PerformLayout();
            this.gbCHLevel3.ResumeLayout(false);
            this.gbCHLevel3.PerformLayout();
            this.gbCHLevel2.ResumeLayout(false);
            this.gbCHLevel2.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.gbCHLevel1.ResumeLayout(false);
            this.gbCHLevel1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_VR_VPI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_VR_NPI)).EndInit();
            this.gbVRLevel4.ResumeLayout(false);
            this.gbVRLevel4.PerformLayout();
            this.gbVRLevel3.ResumeLayout(false);
            this.gbVRLevel3.PerformLayout();
            this.gbVRLevel2.ResumeLayout(false);
            this.gbVRLevel2.PerformLayout();
            this.gbVRLevel1.ResumeLayout(false);
            this.gbVRLevel1.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nud_MET_DTime)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_MET_VPI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_MET_NPI)).EndInit();
            this.pUpStatusBar.ResumeLayout(false);
            this.pUpStatusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem новыйПроектToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem открытьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem открытьБДДатчиковToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem мультиметрToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem задатчикДавленияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem коммутаторToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem холодильнаяКамераToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem MenuItemMainSettings;
        private System.Windows.Forms.ToolStripMenuItem инфоToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsMenuItemAbout;
        private System.Windows.Forms.ToolStripMenuItem помощьToolStripMenuItem;
        private System.Windows.Forms.GroupBox gbMultimetr;
        private System.Windows.Forms.TextBox tbMultimetrData;
        private System.Windows.Forms.Button btnMultimetr;
        private System.Windows.Forms.GroupBox gbCommutator;
        private System.Windows.Forms.Button btnFormCommutator;
        private System.Windows.Forms.Button btnCommutator;
        private System.Windows.Forms.GroupBox gbMensor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbMensorTypeR;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnFormMensor;
        private System.Windows.Forms.TextBox tbMensorData;
        private System.Windows.Forms.Button btnMensor;
        private System.Windows.Forms.Button bMensorVent;
        private System.Windows.Forms.Button bMensorControl;
        private System.Windows.Forms.GroupBox gbTermoCamera;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numTermoCameraPoint;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbTemperature;
        private System.Windows.Forms.Button btnThermalCamera;
        private System.Windows.Forms.NumericUpDown numMensorPoint;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ToolStripMenuItem MI_MultimetrSetings;
        private System.Windows.Forms.ToolStripMenuItem MI_MensorSetings;
        private System.Windows.Forms.ToolStripMenuItem MI_CommutatorSetings;
        private System.Windows.Forms.ToolStripMenuItem MI_ColdCameraSetings;
        private System.Windows.Forms.ToolStripMenuItem датчикиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MI_SensorSetings;
        private System.Windows.Forms.Panel panelLog;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnSensorSeach;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer MainTimer;
        private System.Windows.Forms.RichTextBox rtbConsole;
        private System.Windows.Forms.Button bThermalCameraSet;
        public System.Windows.Forms.TextBox tbNumCH;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.GroupBox gbCHLevel1;
        private System.Windows.Forms.ComboBox cbCHTermoCamera1;
        private System.Windows.Forms.ProgressBar pbCHProcess;
        private System.Windows.Forms.Button bMensorSet;
        private System.Windows.Forms.Button btnCHStart;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.DateTimePicker dtpInfoDate;
        private System.Windows.Forms.ComboBox cbInfoPreambul;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox tbInfoSerialNumber;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tbInfoFactoryNumber;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbInfoDesc;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox tbInfoTeg;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.TextBox tbInfoMesUnit;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox tbInfoMin;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox tbInfoUp;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox tbInfoDown;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox tbInfoSoftVersion;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox tbInfoSensorType;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.TextBox tbSelChannalNumber;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.ProgressBar pbSensorSeach;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.ComboBox cbChannalCharakterizator;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Button btnCHPressureSet1;
        private System.Windows.Forms.ComboBox cbCHPressureSet1;
        private System.Windows.Forms.Button btnCHTemperatureSet1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.DataGridView dataGridView4;
        private System.Windows.Forms.Button btnReadCAP;
        private System.Windows.Forms.TextBox tbDateTime;
        private System.Windows.Forms.ToolStripMenuItem параметрыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem параметрыToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem параметрыToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem параметрыToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem параметрыToolStripMenuItem4;
        private System.Windows.Forms.TextBox tbInfoPressureModel;
        private System.Windows.Forms.TextBox tbInfoDeviceAdress;
        private System.Windows.Forms.Button btnCalibrateCurrent;
        private System.Windows.Forms.Button btnCalculateCoeff;
        private System.Windows.Forms.ComboBox cbDiapazon1;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.GroupBox gbCHLevel4;
        private System.Windows.Forms.ComboBox cbDiapazon4;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Button btnCHPressureSet4;
        private System.Windows.Forms.ComboBox cbCHPressureSet4;
        private System.Windows.Forms.Button btnCHTemperatureSet4;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.ComboBox cbCHTermoCamera4;
        private System.Windows.Forms.GroupBox gbCHLevel3;
        private System.Windows.Forms.ComboBox cbDiapazon3;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Button btnCHPressureSet3;
        private System.Windows.Forms.ComboBox cbCHPressureSet3;
        private System.Windows.Forms.Button btnCHTemperatureSet3;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.ComboBox cbCHTermoCamera3;
        private System.Windows.Forms.GroupBox gbCHLevel2;
        private System.Windows.Forms.ComboBox cbDiapazon2;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Button btnCHPressureSet2;
        private System.Windows.Forms.ComboBox cbCHPressureSet2;
        private System.Windows.Forms.Button btnCHTemperatureSet2;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.ComboBox cbCHTermoCamera2;
        private System.Windows.Forms.ProgressBar pbVRProcess;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.ComboBox cbChannalVerification;
        private System.Windows.Forms.GroupBox gbVRLevel1;
        private System.Windows.Forms.ComboBox cbVRDiapazon1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnVRPressureSet1;
        private System.Windows.Forms.ComboBox cbVRPressureSet1;
        private System.Windows.Forms.Button btnVRTemperatureSet1;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.ComboBox cbVRTermoCamera1;
        private System.Windows.Forms.Button btnVRParamRead;
        private System.Windows.Forms.GroupBox gbVRLevel4;
        private System.Windows.Forms.ComboBox cbVRDiapazon4;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.Button btnVRPressureSet4;
        private System.Windows.Forms.ComboBox cbVRPressureSet4;
        private System.Windows.Forms.Button btnVRTemperatureSet4;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.ComboBox cbVRTermoCamera4;
        private System.Windows.Forms.GroupBox gbVRLevel3;
        private System.Windows.Forms.ComboBox cbVRDiapazon3;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Button btnVRPressureSet3;
        private System.Windows.Forms.ComboBox cbVRPressureSet3;
        private System.Windows.Forms.Button btnVRTemperatureSet3;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.ComboBox cbVRTermoCamera3;
        private System.Windows.Forms.GroupBox gbVRLevel2;
        private System.Windows.Forms.ComboBox cbVRDiapazon2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnVRPressureSet2;
        private System.Windows.Forms.ComboBox cbVRPressureSet2;
        private System.Windows.Forms.Button btnVRTemperatureSet2;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.ComboBox cbVRTermoCamera2;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel pUpStatusBar;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label UpStCh;
        private System.Windows.Forms.Label UpStSerial;
        public System.Windows.Forms.Label UpStModel;
        private System.Windows.Forms.Button bMensorMeas;
        private System.Windows.Forms.ContextMenuStrip cmsCharacterizationTable;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuDeleteResult;
        private System.Windows.Forms.CheckBox cbChannalFix;
        private System.Windows.Forms.ContextMenuStrip cmsVerificationTable;
        private System.Windows.Forms.ToolStripMenuItem tsMenuVerificationDelete;
        private System.Windows.Forms.CheckBox cbSensorPeriodRead;
        private System.Windows.Forms.CheckBox cbChannalFixVR;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnVR_VPI_NPI;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.NumericUpDown nud_VR_VPI;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.NumericUpDown nud_VR_NPI;
        private System.Windows.Forms.Button btnVR_SetZero;
        private System.Windows.Forms.Label label_UpStR;
        private System.Windows.Forms.Label label_UpStV;
        private System.Windows.Forms.Label label_UpStResistance;
        private System.Windows.Forms.Label label_UpStVoltage;
        private System.Windows.Forms.Label label_UpStPressure;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.ContextMenuStrip cmsCurentTable;
        private System.Windows.Forms.ToolStripMenuItem tsmCurrentDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn сChannalNum;
        private System.Windows.Forms.DataGridViewCheckBoxColumn сSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn сSensor;
        private System.Windows.Forms.DataGridViewTextBoxColumn сFactoryNumber;
        private System.Windows.Forms.DataGridViewCheckBoxColumn сPower;
        private System.Windows.Forms.DataGridViewCheckBoxColumn сWork;
        private System.Windows.Forms.DataGridViewTextBoxColumn сCIRecordNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn сIDataTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn сITemperature;
        private System.Windows.Forms.DataGridViewTextBoxColumn cICurrent4mA;
        private System.Windows.Forms.DataGridViewTextBoxColumn cICurrent20mA;
        private System.Windows.Forms.Button btnClockTimer;
        private System.Windows.Forms.DateTimePicker dtpClockTimer;
        private System.Windows.Forms.Splitter splitter5;
        private System.Windows.Forms.Splitter splitter4;
        private System.Windows.Forms.Splitter splitter3;
        private System.Windows.Forms.ToolStripMenuItem окнаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiPanelCommutator;
        private System.Windows.Forms.ToolStripMenuItem tsmiPanelMultimetr;
        private System.Windows.Forms.ToolStripMenuItem tsmiPanelMensor;
        private System.Windows.Forms.ToolStripMenuItem tsmiPanelTermocamera;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem tsmiPanelLog;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.ComboBox cb_MET_Unit;
        private System.Windows.Forms.Button btn_MET_SetZero;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_MET_NPI_VPI;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.NumericUpDown nud_MET_VPI;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.NumericUpDown nud_MET_NPI;
        private System.Windows.Forms.Button btn_MET_Down;
        private System.Windows.Forms.Button btn_MET_Up;
        private System.Windows.Forms.Button btn_MET_Del;
        private System.Windows.Forms.Button btn_MET_Add;
        private System.Windows.Forms.ListBox lb_MET_PressValue;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_MET_DTime;
        private System.Windows.Forms.NumericUpDown nud_MET_DTime;
        private System.Windows.Forms.ProgressBar pbMETProcess;
        private System.Windows.Forms.Button btn_MET_Start;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox cbChannalFixMET;
        private System.Windows.Forms.ComboBox cbChannalMetrolog;
        private System.Windows.Forms.DataGridViewTextBoxColumn сCHRecordNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDataTime2;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTempreture2;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDiapazon;
        private System.Windows.Forms.DataGridViewTextBoxColumn сPressure2;
        private System.Windows.Forms.DataGridViewTextBoxColumn cUTemp2;
        private System.Windows.Forms.DataGridViewTextBoxColumn cUPress2;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDeviation;
        private System.Windows.Forms.DataGridViewTextBoxColumn сVRRecordNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDataTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTempreture;
        private System.Windows.Forms.DataGridViewTextBoxColumn cNPI;
        private System.Windows.Forms.DataGridViewTextBoxColumn cVPI;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPressureZ;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPressureF;
        private System.Windows.Forms.DataGridViewTextBoxColumn сCurrentR;
        private System.Windows.Forms.DataGridViewTextBoxColumn сCurrentF;
        private System.Windows.Forms.DataGridViewTextBoxColumn cVoltageF;
        private System.Windows.Forms.DataGridViewTextBoxColumn cResistanceF;
        private System.Windows.Forms.DataGridView dataGridView5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.Button btnCalculateDeviation;
        private System.Windows.Forms.CheckBox cb_ManualMode;
    }
}

