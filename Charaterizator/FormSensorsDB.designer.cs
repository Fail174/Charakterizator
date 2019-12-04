namespace Charaterizator
{
    partial class FormSensorsDB
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
            this.bOpenFile = new System.Windows.Forms.Button();
            this.bAddLines = new System.Windows.Forms.Button();
            this.bFlashSensor = new System.Windows.Forms.Button();
            this.bDeleteLines = new System.Windows.Forms.Button();
            this.bSaveLines = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Range2_Pmax = new System.Windows.Forms.NumericUpDown();
            this.Range2_Pmin = new System.Windows.Forms.NumericUpDown();
            this.Gain2 = new System.Windows.Forms.NumericUpDown();
            this.Range1_Pmax = new System.Windows.Forms.NumericUpDown();
            this.Range1_Pmin = new System.Windows.Forms.NumericUpDown();
            this.Gain1 = new System.Windows.Forms.NumericUpDown();
            this.DeltaRangeMin = new System.Windows.Forms.NumericUpDown();
            this.Pmax = new System.Windows.Forms.NumericUpDown();
            this.Pmin = new System.Windows.Forms.NumericUpDown();
            this.Serial = new System.Windows.Forms.NumericUpDown();
            this.textBox16 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.textBox14 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rbRange2 = new System.Windows.Forms.RadioButton();
            this.rbRange1 = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.HarPressPoint2 = new System.Windows.Forms.TextBox();
            this.textBox17 = new System.Windows.Forms.TextBox();
            this.HarPressPoint1 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.HarTempPoint1 = new System.Windows.Forms.TextBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.VerPressPoint2 = new System.Windows.Forms.TextBox();
            this.textBox18 = new System.Windows.Forms.TextBox();
            this.VerPressPoint1 = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.VerTempPoint1 = new System.Windows.Forms.TextBox();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lvwModels = new System.Windows.Forms.ListView();
            this.lvwType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvwModel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Range2_Pmax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Range2_Pmin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Gain2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Range1_Pmax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Range1_Pmin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Gain1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeltaRangeMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pmax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pmin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Serial)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bOpenFile
            // 
            this.bOpenFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bOpenFile.Location = new System.Drawing.Point(28, 23);
            this.bOpenFile.Name = "bOpenFile";
            this.bOpenFile.Size = new System.Drawing.Size(210, 27);
            this.bOpenFile.TabIndex = 0;
            this.bOpenFile.Text = "Открыть файл БД";
            this.bOpenFile.UseVisualStyleBackColor = true;
            this.bOpenFile.Click += new System.EventHandler(this.bOpenFile_Click);
            // 
            // bAddLines
            // 
            this.bAddLines.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bAddLines.Location = new System.Drawing.Point(28, 363);
            this.bAddLines.Name = "bAddLines";
            this.bAddLines.Size = new System.Drawing.Size(210, 28);
            this.bAddLines.TabIndex = 1;
            this.bAddLines.Text = "Добавить запись";
            this.bAddLines.UseVisualStyleBackColor = true;
            this.bAddLines.Click += new System.EventHandler(this.bAddLines_Click);
            // 
            // bFlashSensor
            // 
            this.bFlashSensor.BackColor = System.Drawing.Color.RosyBrown;
            this.bFlashSensor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bFlashSensor.Location = new System.Drawing.Point(28, 487);
            this.bFlashSensor.Name = "bFlashSensor";
            this.bFlashSensor.Size = new System.Drawing.Size(210, 32);
            this.bFlashSensor.TabIndex = 2;
            this.bFlashSensor.Text = "Записать данные в датчик";
            this.bFlashSensor.UseVisualStyleBackColor = false;
            this.bFlashSensor.Click += new System.EventHandler(this.bFlashSensor_Click);
            // 
            // bDeleteLines
            // 
            this.bDeleteLines.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bDeleteLines.Location = new System.Drawing.Point(28, 403);
            this.bDeleteLines.Name = "bDeleteLines";
            this.bDeleteLines.Size = new System.Drawing.Size(210, 28);
            this.bDeleteLines.TabIndex = 3;
            this.bDeleteLines.Text = "Удалить запись";
            this.bDeleteLines.UseVisualStyleBackColor = true;
            this.bDeleteLines.Click += new System.EventHandler(this.bDeleteLines_Click);
            // 
            // bSaveLines
            // 
            this.bSaveLines.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bSaveLines.Location = new System.Drawing.Point(28, 442);
            this.bSaveLines.Name = "bSaveLines";
            this.bSaveLines.Size = new System.Drawing.Size(210, 28);
            this.bSaveLines.TabIndex = 4;
            this.bSaveLines.Text = "Сохранить изменения";
            this.bSaveLines.UseVisualStyleBackColor = true;
            this.bSaveLines.Click += new System.EventHandler(this.bSaveLines_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Range2_Pmax);
            this.groupBox1.Controls.Add(this.Range2_Pmin);
            this.groupBox1.Controls.Add(this.Gain2);
            this.groupBox1.Controls.Add(this.Range1_Pmax);
            this.groupBox1.Controls.Add(this.Range1_Pmin);
            this.groupBox1.Controls.Add(this.Gain1);
            this.groupBox1.Controls.Add(this.DeltaRangeMin);
            this.groupBox1.Controls.Add(this.Pmax);
            this.groupBox1.Controls.Add(this.Pmin);
            this.groupBox1.Controls.Add(this.Serial);
            this.groupBox1.Controls.Add(this.textBox16);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.textBox12);
            this.groupBox1.Controls.Add(this.textBox14);
            this.groupBox1.Controls.Add(this.textBox8);
            this.groupBox1.Controls.Add(this.textBox7);
            this.groupBox1.Controls.Add(this.textBox6);
            this.groupBox1.Controls.Add(this.textBox5);
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(264, 23);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(600, 260);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Индивидуальные параметры";
            // 
            // Range2_Pmax
            // 
            this.Range2_Pmax.DecimalPlaces = 1;
            this.Range2_Pmax.Location = new System.Drawing.Point(365, 222);
            this.Range2_Pmax.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Range2_Pmax.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.Range2_Pmax.Name = "Range2_Pmax";
            this.Range2_Pmax.Size = new System.Drawing.Size(217, 21);
            this.Range2_Pmax.TabIndex = 11;
            this.Range2_Pmax.Tag = "11";
            // 
            // Range2_Pmin
            // 
            this.Range2_Pmin.DecimalPlaces = 1;
            this.Range2_Pmin.Location = new System.Drawing.Point(365, 202);
            this.Range2_Pmin.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Range2_Pmin.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.Range2_Pmin.Name = "Range2_Pmin";
            this.Range2_Pmin.Size = new System.Drawing.Size(217, 21);
            this.Range2_Pmin.TabIndex = 10;
            this.Range2_Pmin.Tag = "10";
            // 
            // Gain2
            // 
            this.Gain2.DecimalPlaces = 1;
            this.Gain2.Location = new System.Drawing.Point(365, 182);
            this.Gain2.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Gain2.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.Gain2.Name = "Gain2";
            this.Gain2.Size = new System.Drawing.Size(217, 21);
            this.Gain2.TabIndex = 9;
            this.Gain2.Tag = "7";
            // 
            // Range1_Pmax
            // 
            this.Range1_Pmax.DecimalPlaces = 1;
            this.Range1_Pmax.Location = new System.Drawing.Point(365, 162);
            this.Range1_Pmax.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Range1_Pmax.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.Range1_Pmax.Name = "Range1_Pmax";
            this.Range1_Pmax.Size = new System.Drawing.Size(217, 21);
            this.Range1_Pmax.TabIndex = 8;
            this.Range1_Pmax.Tag = "9";
            // 
            // Range1_Pmin
            // 
            this.Range1_Pmin.DecimalPlaces = 1;
            this.Range1_Pmin.Location = new System.Drawing.Point(365, 142);
            this.Range1_Pmin.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Range1_Pmin.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.Range1_Pmin.Name = "Range1_Pmin";
            this.Range1_Pmin.Size = new System.Drawing.Size(217, 21);
            this.Range1_Pmin.TabIndex = 7;
            this.Range1_Pmin.Tag = "8";
            // 
            // Gain1
            // 
            this.Gain1.DecimalPlaces = 1;
            this.Gain1.Location = new System.Drawing.Point(365, 122);
            this.Gain1.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Gain1.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.Gain1.Name = "Gain1";
            this.Gain1.Size = new System.Drawing.Size(217, 21);
            this.Gain1.TabIndex = 6;
            this.Gain1.Tag = "6";
            // 
            // DeltaRangeMin
            // 
            this.DeltaRangeMin.DecimalPlaces = 1;
            this.DeltaRangeMin.Location = new System.Drawing.Point(365, 82);
            this.DeltaRangeMin.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.DeltaRangeMin.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.DeltaRangeMin.Name = "DeltaRangeMin";
            this.DeltaRangeMin.Size = new System.Drawing.Size(217, 21);
            this.DeltaRangeMin.TabIndex = 4;
            this.DeltaRangeMin.Tag = "12";
            // 
            // Pmax
            // 
            this.Pmax.DecimalPlaces = 1;
            this.Pmax.Location = new System.Drawing.Point(365, 62);
            this.Pmax.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Pmax.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.Pmax.Name = "Pmax";
            this.Pmax.Size = new System.Drawing.Size(217, 21);
            this.Pmax.TabIndex = 3;
            this.Pmax.Tag = "4";
            // 
            // Pmin
            // 
            this.Pmin.DecimalPlaces = 1;
            this.Pmin.Location = new System.Drawing.Point(365, 42);
            this.Pmin.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Pmin.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.Pmin.Name = "Pmin";
            this.Pmin.Size = new System.Drawing.Size(217, 21);
            this.Pmin.TabIndex = 2;
            this.Pmin.Tag = "3";
            // 
            // Serial
            // 
            this.Serial.Location = new System.Drawing.Point(365, 22);
            this.Serial.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.Serial.Name = "Serial";
            this.Serial.Size = new System.Drawing.Size(217, 21);
            this.Serial.TabIndex = 1;
            this.Serial.Tag = "2";
            // 
            // textBox16
            // 
            this.textBox16.Location = new System.Drawing.Point(19, 82);
            this.textBox16.Name = "textBox16";
            this.textBox16.ReadOnly = true;
            this.textBox16.Size = new System.Drawing.Size(347, 21);
            this.textBox16.TabIndex = 22;
            this.textBox16.Text = "Минимально допустимый диапазон настройки датика";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(19, 222);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(347, 21);
            this.textBox2.TabIndex = 29;
            this.textBox2.Text = "Верхний предел измерения для 2- го диапазона, кПа";
            // 
            // textBox12
            // 
            this.textBox12.Location = new System.Drawing.Point(19, 202);
            this.textBox12.Name = "textBox12";
            this.textBox12.ReadOnly = true;
            this.textBox12.Size = new System.Drawing.Size(347, 21);
            this.textBox12.TabIndex = 28;
            this.textBox12.Text = "Нижний предел измерения для 2- го диапазона, кПа";
            // 
            // textBox14
            // 
            this.textBox14.Location = new System.Drawing.Point(19, 182);
            this.textBox14.Name = "textBox14";
            this.textBox14.ReadOnly = true;
            this.textBox14.Size = new System.Drawing.Size(347, 21);
            this.textBox14.TabIndex = 27;
            this.textBox14.Text = "Коэффициент усиления 2- го диапазона";
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(19, 162);
            this.textBox8.Name = "textBox8";
            this.textBox8.ReadOnly = true;
            this.textBox8.Size = new System.Drawing.Size(347, 21);
            this.textBox8.TabIndex = 26;
            this.textBox8.Text = "Верхний предел измерения для 1- го диапазона, кПа";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(19, 142);
            this.textBox7.Name = "textBox7";
            this.textBox7.ReadOnly = true;
            this.textBox7.Size = new System.Drawing.Size(347, 21);
            this.textBox7.TabIndex = 25;
            this.textBox7.Text = "Нижний предел измерения для 1- го диапазона, кПа";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(19, 122);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(347, 21);
            this.textBox6.TabIndex = 24;
            this.textBox6.Text = "Коэффициент усиления 1- го диапазона";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(19, 102);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(347, 21);
            this.textBox5.TabIndex = 23;
            this.textBox5.Text = "Количество диапазонов характеризации ";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(19, 62);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(347, 21);
            this.textBox4.TabIndex = 21;
            this.textBox4.Text = "Максимальный верхний предел датчика, кПа";
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox3.Location = new System.Drawing.Point(19, 42);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(347, 21);
            this.textBox3.TabIndex = 20;
            this.textBox3.Text = "Минимальный нижний предел датчика, кПа";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox1.Location = new System.Drawing.Point(19, 22);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(347, 21);
            this.textBox1.TabIndex = 19;
            this.textBox1.Text = "Заводской номер";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox4.Controls.Add(this.rbRange2);
            this.groupBox4.Controls.Add(this.rbRange1);
            this.groupBox4.Location = new System.Drawing.Point(365, 94);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(217, 30);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            // 
            // rbRange2
            // 
            this.rbRange2.AutoSize = true;
            this.rbRange2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbRange2.Location = new System.Drawing.Point(115, 9);
            this.rbRange2.Name = "rbRange2";
            this.rbRange2.Size = new System.Drawing.Size(100, 17);
            this.rbRange2.TabIndex = 15;
            this.rbRange2.Text = "два диапазона";
            this.rbRange2.UseVisualStyleBackColor = true;
            // 
            // rbRange1
            // 
            this.rbRange1.AutoSize = true;
            this.rbRange1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbRange1.Location = new System.Drawing.Point(6, 9);
            this.rbRange1.Name = "rbRange1";
            this.rbRange1.Size = new System.Drawing.Size(108, 17);
            this.rbRange1.TabIndex = 14;
            this.rbRange1.Text = "один диапазон  |";
            this.rbRange1.UseVisualStyleBackColor = true;
            this.rbRange1.CheckedChanged += new System.EventHandler(this.rbRange1_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.HarPressPoint2);
            this.groupBox2.Controls.Add(this.textBox17);
            this.groupBox2.Controls.Add(this.HarPressPoint1);
            this.groupBox2.Controls.Add(this.textBox9);
            this.groupBox2.Controls.Add(this.HarTempPoint1);
            this.groupBox2.Controls.Add(this.textBox11);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(264, 298);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(600, 102);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Параметры характеризации (данные вводить через пробел или точку с запятой)";
            // 
            // HarPressPoint2
            // 
            this.HarPressPoint2.Location = new System.Drawing.Point(364, 65);
            this.HarPressPoint2.Name = "HarPressPoint2";
            this.HarPressPoint2.Size = new System.Drawing.Size(217, 21);
            this.HarPressPoint2.TabIndex = 14;
            this.HarPressPoint2.Tag = "15";
            this.HarPressPoint2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HarTempPoint1_KeyPress);
            // 
            // textBox17
            // 
            this.textBox17.Location = new System.Drawing.Point(20, 65);
            this.textBox17.Name = "textBox17";
            this.textBox17.ReadOnly = true;
            this.textBox17.Size = new System.Drawing.Size(347, 21);
            this.textBox17.TabIndex = 32;
            this.textBox17.Text = "Значения точек по давлению 2- го диапазона";
            // 
            // HarPressPoint1
            // 
            this.HarPressPoint1.Location = new System.Drawing.Point(364, 46);
            this.HarPressPoint1.Name = "HarPressPoint1";
            this.HarPressPoint1.Size = new System.Drawing.Size(217, 21);
            this.HarPressPoint1.TabIndex = 13;
            this.HarPressPoint1.Tag = "14";
            this.HarPressPoint1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HarTempPoint1_KeyPress);
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(20, 46);
            this.textBox9.Name = "textBox9";
            this.textBox9.ReadOnly = true;
            this.textBox9.Size = new System.Drawing.Size(347, 21);
            this.textBox9.TabIndex = 31;
            this.textBox9.Text = "Значения точек по давлению 1- го диапазона";
            // 
            // HarTempPoint1
            // 
            this.HarTempPoint1.Location = new System.Drawing.Point(364, 27);
            this.HarTempPoint1.Name = "HarTempPoint1";
            this.HarTempPoint1.Size = new System.Drawing.Size(217, 21);
            this.HarTempPoint1.TabIndex = 12;
            this.HarTempPoint1.Tag = "13";
            this.HarTempPoint1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HarTempPoint1_KeyPress);
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(20, 27);
            this.textBox11.Name = "textBox11";
            this.textBox11.ReadOnly = true;
            this.textBox11.Size = new System.Drawing.Size(347, 21);
            this.textBox11.TabIndex = 30;
            this.textBox11.Text = "Значения точек по температуре";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.VerPressPoint2);
            this.groupBox3.Controls.Add(this.textBox18);
            this.groupBox3.Controls.Add(this.VerPressPoint1);
            this.groupBox3.Controls.Add(this.textBox10);
            this.groupBox3.Controls.Add(this.VerTempPoint1);
            this.groupBox3.Controls.Add(this.textBox13);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox3.Location = new System.Drawing.Point(264, 418);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(600, 103);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Параметры верификации (данные вводить через пробел или точку с запятой)";
            // 
            // VerPressPoint2
            // 
            this.VerPressPoint2.Location = new System.Drawing.Point(364, 66);
            this.VerPressPoint2.Name = "VerPressPoint2";
            this.VerPressPoint2.Size = new System.Drawing.Size(217, 21);
            this.VerPressPoint2.TabIndex = 17;
            this.VerPressPoint2.Tag = "18";
            this.VerPressPoint2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HarTempPoint1_KeyPress);
            // 
            // textBox18
            // 
            this.textBox18.Location = new System.Drawing.Point(20, 66);
            this.textBox18.Name = "textBox18";
            this.textBox18.ReadOnly = true;
            this.textBox18.Size = new System.Drawing.Size(347, 21);
            this.textBox18.TabIndex = 35;
            this.textBox18.Text = "Значения точек по давлению 2- го диапазона";
            // 
            // VerPressPoint1
            // 
            this.VerPressPoint1.Location = new System.Drawing.Point(364, 47);
            this.VerPressPoint1.Name = "VerPressPoint1";
            this.VerPressPoint1.Size = new System.Drawing.Size(217, 21);
            this.VerPressPoint1.TabIndex = 16;
            this.VerPressPoint1.Tag = "17";
            this.VerPressPoint1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HarTempPoint1_KeyPress);
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(20, 47);
            this.textBox10.Name = "textBox10";
            this.textBox10.ReadOnly = true;
            this.textBox10.Size = new System.Drawing.Size(347, 21);
            this.textBox10.TabIndex = 34;
            this.textBox10.Text = "Значения точек по давлению 1- го диапазона";
            // 
            // VerTempPoint1
            // 
            this.VerTempPoint1.Location = new System.Drawing.Point(364, 28);
            this.VerTempPoint1.Name = "VerTempPoint1";
            this.VerTempPoint1.Size = new System.Drawing.Size(217, 21);
            this.VerTempPoint1.TabIndex = 15;
            this.VerTempPoint1.Tag = "16";
            this.VerTempPoint1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HarTempPoint1_KeyPress);
            // 
            // textBox13
            // 
            this.textBox13.Location = new System.Drawing.Point(20, 28);
            this.textBox13.Name = "textBox13";
            this.textBox13.ReadOnly = true;
            this.textBox13.Size = new System.Drawing.Size(347, 21);
            this.textBox13.TabIndex = 33;
            this.textBox13.Text = "Значения точек по температуре";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 556);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(895, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "три кита";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // lvwModels
            // 
            this.lvwModels.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvwType,
            this.lvwModel});
            this.lvwModels.FullRowSelect = true;
            this.lvwModels.GridLines = true;
            this.lvwModels.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvwModels.HideSelection = false;
            this.lvwModels.Location = new System.Drawing.Point(28, 64);
            this.lvwModels.MultiSelect = false;
            this.lvwModels.Name = "lvwModels";
            this.lvwModels.Size = new System.Drawing.Size(210, 279);
            this.lvwModels.TabIndex = 25;
            this.lvwModels.UseCompatibleStateImageBehavior = false;
            this.lvwModels.View = System.Windows.Forms.View.Details;
            this.lvwModels.SelectedIndexChanged += new System.EventHandler(this.lvwModels_SelectedIndexChanged);
            // 
            // lvwType
            // 
            this.lvwType.Text = "Название";
            this.lvwType.Width = 115;
            // 
            // lvwModel
            // 
            this.lvwModel.Text = "Модель";
            this.lvwModel.Width = 90;
            // 
            // FormSensorsDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 578);
            this.Controls.Add(this.bSaveLines);
            this.Controls.Add(this.bDeleteLines);
            this.Controls.Add(this.bAddLines);
            this.Controls.Add(this.lvwModels);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bFlashSensor);
            this.Controls.Add(this.bOpenFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormSensorsDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Программа для редактирования базы данных датчиков";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Range2_Pmax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Range2_Pmin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Gain2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Range1_Pmax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Range1_Pmin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Gain1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeltaRangeMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pmax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pmin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Serial)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bOpenFile;
        private System.Windows.Forms.Button bAddLines;
        private System.Windows.Forms.Button bFlashSensor;
        private System.Windows.Forms.Button bDeleteLines;
        private System.Windows.Forms.Button bSaveLines;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox HarPressPoint1;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.TextBox HarTempPoint1;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox VerPressPoint1;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.TextBox VerTempPoint1;
        private System.Windows.Forms.TextBox textBox13;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rbRange2;
        private System.Windows.Forms.RadioButton rbRange1;
        private System.Windows.Forms.TextBox textBox16;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.TextBox textBox14;
        private System.Windows.Forms.TextBox HarPressPoint2;
        private System.Windows.Forms.TextBox textBox17;
        private System.Windows.Forms.TextBox VerPressPoint2;
        private System.Windows.Forms.TextBox textBox18;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ListView lvwModels;
        private System.Windows.Forms.ColumnHeader lvwType;
        private System.Windows.Forms.ColumnHeader lvwModel;
        private System.Windows.Forms.NumericUpDown Serial;
        private System.Windows.Forms.NumericUpDown Range2_Pmax;
        private System.Windows.Forms.NumericUpDown Range2_Pmin;
        private System.Windows.Forms.NumericUpDown Gain2;
        private System.Windows.Forms.NumericUpDown Range1_Pmax;
        private System.Windows.Forms.NumericUpDown Range1_Pmin;
        private System.Windows.Forms.NumericUpDown Gain1;
        private System.Windows.Forms.NumericUpDown DeltaRangeMin;
        private System.Windows.Forms.NumericUpDown Pmax;
        private System.Windows.Forms.NumericUpDown Pmin;
    }
}

