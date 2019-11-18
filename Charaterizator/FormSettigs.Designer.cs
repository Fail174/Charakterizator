namespace Charaterizator
{
    partial class FormSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.ofdDataBase = new System.Windows.Forms.OpenFileDialog();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.textBox14 = new System.Windows.Forms.TextBox();
            this.textBox15 = new System.Windows.Forms.TextBox();
            this.textBox16 = new System.Windows.Forms.TextBox();
            this.textBox17 = new System.Windows.Forms.TextBox();
            this.textBox18 = new System.Windows.Forms.TextBox();
            this.textBox19 = new System.Windows.Forms.TextBox();
            this.textBox20 = new System.Windows.Forms.TextBox();
            this.textBox21 = new System.Windows.Forms.TextBox();
            this.textBox23 = new System.Windows.Forms.TextBox();
            this.textBox25 = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnOpenFile);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(17, 316);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(598, 78);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "База данных датчиков";
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(499, 34);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(75, 23);
            this.btnOpenFile.TabIndex = 2;
            this.btnOpenFile.Text = "Выбрать";
            this.btnOpenFile.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(158, 35);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(333, 20);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Путь к файлу базы данных:";
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.Location = new System.Drawing.Point(281, 414);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 35);
            this.button2.TabIndex = 4;
            this.button2.Text = "Закрыть";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // ofdDataBase
            // 
            this.ofdDataBase.FileName = "Sensor.mdb";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(17, 27);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(475, 20);
            this.textBox2.TabIndex = 5;
            this.textBox2.Text = "Время выдержки после выхода камеры на температурный режим, [мин]";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(491, 27);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(124, 20);
            this.textBox3.TabIndex = 6;
            this.textBox3.Text = "5";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(491, 46);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(124, 20);
            this.textBox4.TabIndex = 8;
            this.textBox4.Text = "5";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(17, 46);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(475, 20);
            this.textBox5.TabIndex = 7;
            this.textBox5.Text = "Время выдержки после выхода на заданное давление, [мин]";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(491, 65);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(124, 20);
            this.textBox6.TabIndex = 10;
            this.textBox6.Text = "2";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(17, 65);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(475, 20);
            this.textBox7.TabIndex = 9;
            this.textBox7.Text = "Время выдержки после сброса давления (при раскачке давлением), [мин]";
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(491, 84);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(124, 20);
            this.textBox8.TabIndex = 12;
            this.textBox8.Text = "1";
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(17, 84);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(475, 20);
            this.textBox9.TabIndex = 11;
            this.textBox9.Text = "Диапазон в котором должна поддерживаться температура [град]";
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(17, 160);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(475, 20);
            this.textBox11.TabIndex = 19;
            this.textBox11.Text = "Проводить  характеризацию/верификацию при отсутствии подключения  к задатчику";
            // 
            // textBox12
            // 
            this.textBox12.Location = new System.Drawing.Point(491, 141);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(124, 20);
            this.textBox12.TabIndex = 18;
            this.textBox12.Text = "1";
            // 
            // textBox13
            // 
            this.textBox13.Location = new System.Drawing.Point(17, 141);
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new System.Drawing.Size(475, 20);
            this.textBox13.TabIndex = 17;
            this.textBox13.Text = "Общий интервал опроса приборов и датчиков,  [сек]";
            // 
            // textBox14
            // 
            this.textBox14.Location = new System.Drawing.Point(491, 122);
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new System.Drawing.Size(124, 20);
            this.textBox14.TabIndex = 16;
            this.textBox14.Text = "100";
            // 
            // textBox15
            // 
            this.textBox15.Location = new System.Drawing.Point(17, 122);
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new System.Drawing.Size(475, 20);
            this.textBox15.TabIndex = 15;
            this.textBox15.Text = "Сопротивление тензомоста сенсора датчика, [Ом]";
            // 
            // textBox16
            // 
            this.textBox16.Location = new System.Drawing.Point(491, 103);
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new System.Drawing.Size(124, 20);
            this.textBox16.TabIndex = 14;
            this.textBox16.Text = "10";
            // 
            // textBox17
            // 
            this.textBox17.Location = new System.Drawing.Point(17, 103);
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new System.Drawing.Size(475, 20);
            this.textBox17.TabIndex = 13;
            this.textBox17.Text = "Диапазон в котором должно поддерживаться давление [Па]";
            // 
            // textBox18
            // 
            this.textBox18.Location = new System.Drawing.Point(491, 236);
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new System.Drawing.Size(124, 20);
            this.textBox18.TabIndex = 28;
            // 
            // textBox19
            // 
            this.textBox19.Location = new System.Drawing.Point(17, 236);
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new System.Drawing.Size(475, 20);
            this.textBox19.TabIndex = 27;
            // 
            // textBox20
            // 
            this.textBox20.Location = new System.Drawing.Point(491, 217);
            this.textBox20.Name = "textBox20";
            this.textBox20.Size = new System.Drawing.Size(124, 20);
            this.textBox20.TabIndex = 26;
            // 
            // textBox21
            // 
            this.textBox21.Location = new System.Drawing.Point(17, 217);
            this.textBox21.Name = "textBox21";
            this.textBox21.Size = new System.Drawing.Size(475, 20);
            this.textBox21.TabIndex = 25;
            // 
            // textBox23
            // 
            this.textBox23.Location = new System.Drawing.Point(17, 198);
            this.textBox23.Name = "textBox23";
            this.textBox23.Size = new System.Drawing.Size(475, 20);
            this.textBox23.TabIndex = 23;
            this.textBox23.Text = "Обратный ход при характеризации и верификации [есть / нет]";
            // 
            // textBox25
            // 
            this.textBox25.Location = new System.Drawing.Point(17, 179);
            this.textBox25.Name = "textBox25";
            this.textBox25.Size = new System.Drawing.Size(475, 20);
            this.textBox25.TabIndex = 21;
            this.textBox25.Text = "Проводить  характеризацию/верификацию при отсутствии подключения  к задатчику";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Да",
            "Нет"});
            this.comboBox1.Location = new System.Drawing.Point(491, 159);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(124, 21);
            this.comboBox1.TabIndex = 29;
            this.comboBox1.Text = "Да/Нет";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Да",
            "Нет"});
            this.comboBox2.Location = new System.Drawing.Point(491, 179);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(124, 21);
            this.comboBox2.TabIndex = 30;
            this.comboBox2.Text = "Да/Нет";
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "Да",
            "Нет"});
            this.comboBox3.Location = new System.Drawing.Point(491, 198);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(124, 21);
            this.comboBox3.TabIndex = 31;
            this.comboBox3.Text = "Есть/Нет";
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 461);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.textBox18);
            this.Controls.Add(this.textBox19);
            this.Controls.Add(this.textBox20);
            this.Controls.Add(this.textBox21);
            this.Controls.Add(this.textBox23);
            this.Controls.Add(this.textBox25);
            this.Controls.Add(this.textBox11);
            this.Controls.Add(this.textBox12);
            this.Controls.Add(this.textBox13);
            this.Controls.Add(this.textBox14);
            this.Controls.Add(this.textBox15);
            this.Controls.Add(this.textBox16);
            this.Controls.Add(this.textBox17);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки программы";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.OpenFileDialog ofdDataBase;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.TextBox textBox13;
        private System.Windows.Forms.TextBox textBox14;
        private System.Windows.Forms.TextBox textBox15;
        private System.Windows.Forms.TextBox textBox16;
        private System.Windows.Forms.TextBox textBox17;
        private System.Windows.Forms.TextBox textBox18;
        private System.Windows.Forms.TextBox textBox19;
        private System.Windows.Forms.TextBox textBox20;
        private System.Windows.Forms.TextBox textBox21;
        private System.Windows.Forms.TextBox textBox23;
        private System.Windows.Forms.TextBox textBox25;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox3;
    }
}