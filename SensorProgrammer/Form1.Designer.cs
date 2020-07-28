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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            this.bCommutator = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbModel = new System.Windows.Forms.ComboBox();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.dgwMainWindow = new System.Windows.Forms.DataGridView();
            this.dgwChannal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgwSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgwType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgwModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgwSerial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bDataBase = new System.Windows.Forms.Button();
            this.bBurn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dgwSensParam = new System.Windows.Forms.DataGridView();
            this.sensParamName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sensParamValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.rtbConsole = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwMainWindow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgwSensParam)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // bCommutator
            // 
            this.bCommutator.BackColor = System.Drawing.Color.ForestGreen;
            this.bCommutator.ForeColor = System.Drawing.SystemColors.Window;
            this.bCommutator.Location = new System.Drawing.Point(164, 19);
            this.bCommutator.Name = "bCommutator";
            this.bCommutator.Size = new System.Drawing.Size(113, 43);
            this.bCommutator.TabIndex = 0;
            this.bCommutator.Text = "Коммутатор: ПОДКЛЮЧЕН";
            this.bCommutator.UseVisualStyleBackColor = false;
            this.bCommutator.Click += new System.EventHandler(this.bCommutator_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbModel);
            this.panel1.Controls.Add(this.cbType);
            this.panel1.Controls.Add(this.dgwMainWindow);
            this.panel1.Location = new System.Drawing.Point(12, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(440, 462);
            this.panel1.TabIndex = 1;
            // 
            // cbModel
            // 
            this.cbModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbModel.FormattingEnabled = true;
            this.cbModel.Location = new System.Drawing.Point(212, 22);
            this.cbModel.Name = "cbModel";
            this.cbModel.Size = new System.Drawing.Size(89, 21);
            this.cbModel.TabIndex = 2;
            this.cbModel.SelectedIndexChanged += new System.EventHandler(this.cbModel_SelectedIndexChanged);
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(92, 22);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(119, 21);
            this.cbType.TabIndex = 1;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
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
            this.dgwMainWindow.Size = new System.Drawing.Size(440, 462);
            this.dgwMainWindow.TabIndex = 0;
            this.dgwMainWindow.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgwMainWindow_CellClick);
            this.dgwMainWindow.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgwMainWindow_CellEndEdit);
            this.dgwMainWindow.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgwMainWindow_CellValueChanged);
            this.dgwMainWindow.SelectionChanged += new System.EventHandler(this.dgwMainWindow_SelectionChanged);
            // 
            // dgwChannal
            // 
            dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgwChannal.DefaultCellStyle = dataGridViewCellStyle25;
            this.dgwChannal.HeaderText = "Канал";
            this.dgwChannal.Name = "dgwChannal";
            this.dgwChannal.ReadOnly = true;
            this.dgwChannal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgwChannal.Width = 45;
            // 
            // dgwSelect
            // 
            this.dgwSelect.HeaderText = "Выбор";
            this.dgwSelect.Name = "dgwSelect";
            this.dgwSelect.Width = 45;
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
            this.dgwModel.Width = 90;
            // 
            // dgwSerial
            // 
            this.dgwSerial.HeaderText = "Серийный номер";
            this.dgwSerial.Name = "dgwSerial";
            this.dgwSerial.ReadOnly = true;
            this.dgwSerial.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgwSerial.Width = 120;
            // 
            // bDataBase
            // 
            this.bDataBase.BackColor = System.Drawing.Color.ForestGreen;
            this.bDataBase.ForeColor = System.Drawing.SystemColors.Window;
            this.bDataBase.Location = new System.Drawing.Point(11, 19);
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
            this.bBurn.Location = new System.Drawing.Point(10, 269);
            this.bBurn.Name = "bBurn";
            this.bBurn.Size = new System.Drawing.Size(403, 40);
            this.bBurn.TabIndex = 5;
            this.bBurn.Text = "Записать индивидуальные параметры";
            this.bBurn.UseVisualStyleBackColor = false;
            this.bBurn.Click += new System.EventHandler(this.bBurn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 72);
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
            dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle27.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle27.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle27.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle27.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle27.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle27.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgwSensParam.DefaultCellStyle = dataGridViewCellStyle27;
            this.dgwSensParam.Enabled = false;
            this.dgwSensParam.Location = new System.Drawing.Point(10, 21);
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
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.sensParamValue.DefaultCellStyle = dataGridViewCellStyle26;
            this.sensParamValue.HeaderText = "Значение";
            this.sensParamValue.Name = "sensParamValue";
            this.sensParamValue.ReadOnly = true;
            this.sensParamValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.sensParamValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.sensParamValue.Width = 121;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.bDataBase);
            this.groupBox1.Controls.Add(this.bCommutator);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(467, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(421, 94);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Управление подключением ";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.button1.ForeColor = System.Drawing.SystemColors.Window;
            this.button1.Location = new System.Drawing.Point(295, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 43);
            this.button1.TabIndex = 7;
            this.button1.Text = "Датчики: ПОДКЛЮЧЕНЫ";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgwSensParam);
            this.groupBox2.Controls.Add(this.bBurn);
            this.groupBox2.Location = new System.Drawing.Point(467, 110);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(423, 317);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " Индивидуальные параметры датчика ";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(467, 454);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(423, 23);
            this.progressBar.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(467, 433);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(190, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Не дышите, датчики прошиваются...";
            this.label2.Visible = false;
            // 
            // rtbConsole
            // 
            this.rtbConsole.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbConsole.Location = new System.Drawing.Point(12, 483);
            this.rtbConsole.Name = "rtbConsole";
            this.rtbConsole.ReadOnly = true;
            this.rtbConsole.Size = new System.Drawing.Size(878, 45);
            this.rtbConsole.TabIndex = 13;
            this.rtbConsole.Text = "";
            // 
            // FormSensorProgrammer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 540);
            this.Controls.Add(this.rtbConsole);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn dgwChannal;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgwSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgwType;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgwModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgwSerial;
        private System.Windows.Forms.Button bBurn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgwSensParam;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn sensParamName;
        private System.Windows.Forms.DataGridViewTextBoxColumn sensParamValue;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox rtbConsole;
    }
}

