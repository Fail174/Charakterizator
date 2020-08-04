namespace SensorProgrammer
{
    partial class FormSensorProgrammer
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.bCommutator = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgwMainWindow = new System.Windows.Forms.DataGridView();
            this.cbModel = new System.Windows.Forms.ComboBox();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.bDataBase = new System.Windows.Forms.Button();
            this.bBurn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dgwSensParam = new System.Windows.Forms.DataGridView();
            this.sensParamName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sensParamValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bSensors = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.bRead = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.rtbConsole = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbAutoNumSerial = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgwChannal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgwSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgwType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgwModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgwSerial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwMainWindow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgwSensParam)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // bCommutator
            // 
            this.bCommutator.BackColor = System.Drawing.Color.ForestGreen;
            this.bCommutator.ForeColor = System.Drawing.SystemColors.Window;
            this.bCommutator.Location = new System.Drawing.Point(164, 24);
            this.bCommutator.Name = "bCommutator";
            this.bCommutator.Size = new System.Drawing.Size(113, 43);
            this.bCommutator.TabIndex = 0;
            this.bCommutator.Text = "Коммутатор: ПОДКЛЮЧЕН";
            this.bCommutator.UseVisualStyleBackColor = false;
            this.bCommutator.Click += new System.EventHandler(this.bCommutator_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgwMainWindow);
            this.panel1.Location = new System.Drawing.Point(12, 106);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(440, 463);
            this.panel1.TabIndex = 1;
            // 
            // dgwMainWindow
            // 
            this.dgwMainWindow.AllowUserToAddRows = false;
            this.dgwMainWindow.AllowUserToDeleteRows = false;
            this.dgwMainWindow.AllowUserToResizeColumns = false;
            this.dgwMainWindow.AllowUserToResizeRows = false;
            this.dgwMainWindow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgwMainWindow.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgwChannal,
            this.dgwSelect,
            this.dgwType,
            this.dgwModel,
            this.dgwSerial});
            this.dgwMainWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgwMainWindow.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgwMainWindow.Location = new System.Drawing.Point(0, 0);
            this.dgwMainWindow.MultiSelect = false;
            this.dgwMainWindow.Name = "dgwMainWindow";
            this.dgwMainWindow.RowHeadersVisible = false;
            this.dgwMainWindow.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgwMainWindow.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgwMainWindow.Size = new System.Drawing.Size(440, 463);
            this.dgwMainWindow.TabIndex = 0;
            this.dgwMainWindow.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgwMainWindow_CellClick);
            this.dgwMainWindow.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgwMainWindow_CellEndEdit);
            this.dgwMainWindow.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgwMainWindow_CellValueChanged);
            this.dgwMainWindow.SelectionChanged += new System.EventHandler(this.dgwMainWindow_SelectionChanged);
            // 
            // cbModel
            // 
            this.cbModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbModel.FormattingEnabled = true;
            this.cbModel.Location = new System.Drawing.Point(290, 20);
            this.cbModel.Name = "cbModel";
            this.cbModel.Size = new System.Drawing.Size(136, 21);
            this.cbModel.TabIndex = 2;
            this.cbModel.SelectedIndexChanged += new System.EventHandler(this.cbModel_SelectedIndexChanged);
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(86, 20);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(130, 21);
            this.cbType.TabIndex = 1;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // bDataBase
            // 
            this.bDataBase.BackColor = System.Drawing.Color.ForestGreen;
            this.bDataBase.ForeColor = System.Drawing.SystemColors.Window;
            this.bDataBase.Location = new System.Drawing.Point(11, 24);
            this.bDataBase.Name = "bDataBase";
            this.bDataBase.Size = new System.Drawing.Size(138, 43);
            this.bDataBase.TabIndex = 2;
            this.bDataBase.Text = "База Данных: ПОДКЛЮЧЕНА";
            this.bDataBase.UseVisualStyleBackColor = false;
            this.bDataBase.Click += new System.EventHandler(this.bDataBase_Click);
            // 
            // bBurn
            // 
            this.bBurn.BackColor = System.Drawing.Color.LightCoral;
            this.bBurn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bBurn.ForeColor = System.Drawing.SystemColors.Window;
            this.bBurn.Location = new System.Drawing.Point(218, 319);
            this.bBurn.Name = "bBurn";
            this.bBurn.Size = new System.Drawing.Size(195, 40);
            this.bBurn.TabIndex = 5;
            this.bBurn.Text = "Записать индивидуальные параметры";
            this.bBurn.UseVisualStyleBackColor = false;
            this.bBurn.Click += new System.EventHandler(this.bBurn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Путь к БД: ";
            // 
            // dgwSensParam
            // 
            this.dgwSensParam.AllowUserToAddRows = false;
            this.dgwSensParam.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgwSensParam.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgwSensParam.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sensParamName,
            this.sensParamValue});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgwSensParam.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgwSensParam.Enabled = false;
            this.dgwSensParam.Location = new System.Drawing.Point(10, 27);
            this.dgwSensParam.Name = "dgwSensParam";
            this.dgwSensParam.ReadOnly = true;
            this.dgwSensParam.RowHeadersVisible = false;
            this.dgwSensParam.Size = new System.Drawing.Size(403, 243);
            this.dgwSensParam.TabIndex = 7;
            // 
            // sensParamName
            // 
            this.sensParamName.HeaderText = "Наименование параметра";
            this.sensParamName.Name = "sensParamName";
            this.sensParamName.ReadOnly = true;
            this.sensParamName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.sensParamName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.sensParamName.Width = 280;
            // 
            // sensParamValue
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.sensParamValue.DefaultCellStyle = dataGridViewCellStyle7;
            this.sensParamValue.HeaderText = "Значение";
            this.sensParamValue.Name = "sensParamValue";
            this.sensParamValue.ReadOnly = true;
            this.sensParamValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.sensParamValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.sensParamValue.Width = 121;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.bSensors);
            this.groupBox1.Controls.Add(this.bDataBase);
            this.groupBox1.Controls.Add(this.bCommutator);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(467, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(421, 105);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Управление подключением ";
            // 
            // bSensors
            // 
            this.bSensors.BackColor = System.Drawing.Color.ForestGreen;
            this.bSensors.ForeColor = System.Drawing.SystemColors.Window;
            this.bSensors.Location = new System.Drawing.Point(295, 25);
            this.bSensors.Name = "bSensors";
            this.bSensors.Size = new System.Drawing.Size(118, 43);
            this.bSensors.TabIndex = 7;
            this.bSensors.Text = "Датчики: ПОДКЛЮЧЕНЫ";
            this.bSensors.UseVisualStyleBackColor = false;
            this.bSensors.Click += new System.EventHandler(this.bSensors_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.bRead);
            this.groupBox2.Controls.Add(this.dgwSensParam);
            this.groupBox2.Controls.Add(this.bBurn);
            this.groupBox2.Location = new System.Drawing.Point(467, 134);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(423, 371);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " Индивидуальные параметры датчика ";
            // 
            // bRead
            // 
            this.bRead.BackColor = System.Drawing.Color.LightCoral;
            this.bRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bRead.ForeColor = System.Drawing.SystemColors.Window;
            this.bRead.Location = new System.Drawing.Point(8, 319);
            this.bRead.Name = "bRead";
            this.bRead.Size = new System.Drawing.Size(195, 40);
            this.bRead.TabIndex = 8;
            this.bRead.Text = "Поиск датчиков в выделенных каналах";
            this.bRead.UseVisualStyleBackColor = false;
            this.bRead.Click += new System.EventHandler(this.bRead_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(464, 543);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(423, 23);
            this.progressBar.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(464, 516);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(328, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Выполняется прошивка индивидуальных параметров датчиков";
            this.label2.Visible = false;
            // 
            // rtbConsole
            // 
            this.rtbConsole.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbConsole.Location = new System.Drawing.Point(339, 47);
            this.rtbConsole.Name = "rtbConsole";
            this.rtbConsole.ReadOnly = true;
            this.rtbConsole.Size = new System.Drawing.Size(87, 22);
            this.rtbConsole.TabIndex = 13;
            this.rtbConsole.Text = "";
            this.rtbConsole.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Тип датчика:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(238, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Модель:";
            // 
            // cbAutoNumSerial
            // 
            this.cbAutoNumSerial.AutoSize = true;
            this.cbAutoNumSerial.Checked = true;
            this.cbAutoNumSerial.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoNumSerial.Location = new System.Drawing.Point(13, 54);
            this.cbAutoNumSerial.Name = "cbAutoNumSerial";
            this.cbAutoNumSerial.Size = new System.Drawing.Size(306, 17);
            this.cbAutoNumSerial.TabIndex = 16;
            this.cbAutoNumSerial.Text = "Включить автонумерацию серийных номеров датчиков";
            this.cbAutoNumSerial.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbAutoNumSerial);
            this.groupBox3.Controls.Add(this.rtbConsole);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.cbModel);
            this.groupBox3.Controls.Add(this.cbType);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(440, 83);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Тип и модель датчика";
            // 
            // dgwChannal
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgwChannal.DefaultCellStyle = dataGridViewCellStyle9;
            this.dgwChannal.HeaderText = "Канал";
            this.dgwChannal.Name = "dgwChannal";
            this.dgwChannal.ReadOnly = true;
            this.dgwChannal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgwChannal.Width = 45;
            // 
            // dgwSelect
            // 
            this.dgwSelect.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dgwSelect.HeaderText = "";
            this.dgwSelect.Name = "dgwSelect";
            this.dgwSelect.Width = 25;
            // 
            // dgwType
            // 
            this.dgwType.HeaderText = "Тип датчика";
            this.dgwType.Name = "dgwType";
            this.dgwType.ReadOnly = true;
            this.dgwType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgwType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgwType.Width = 120;
            // 
            // dgwModel
            // 
            this.dgwModel.HeaderText = "Модель";
            this.dgwModel.Name = "dgwModel";
            this.dgwModel.ReadOnly = true;
            this.dgwModel.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgwModel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgwSerial
            // 
            this.dgwSerial.HeaderText = "Серийный номер";
            this.dgwSerial.Name = "dgwSerial";
            this.dgwSerial.ReadOnly = true;
            this.dgwSerial.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgwSerial.Width = 130;
            // 
            // FormSensorProgrammer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormSensorProgrammer";
            this.Text = "Программа для записи индивидуальных параметров датчиков";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormSensorProgrammer_FormClosed);
            this.Load += new System.EventHandler(this.FormSensorProgrammer_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgwMainWindow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgwSensParam)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bCommutator;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bDataBase;
        private System.Windows.Forms.DataGridView dgwMainWindow;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.ComboBox cbModel;
        private System.Windows.Forms.Button bBurn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgwSensParam;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn sensParamName;
        private System.Windows.Forms.DataGridViewTextBoxColumn sensParamValue;
        private System.Windows.Forms.Button bSensors;
        private System.Windows.Forms.RichTextBox rtbConsole;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbAutoNumSerial;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button bRead;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgwChannal;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgwSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgwType;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgwModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgwSerial;
    }
}

