namespace Charaterizator
{
    partial class FormAddNewSensorsDB
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbTypeSens = new System.Windows.Forms.TextBox();
            this.tbModelSens = new System.Windows.Forms.TextBox();
            this.bOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(92, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Тип или название датчика:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(118, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Модель датчика:";
            // 
            // tbTypeSens
            // 
            this.tbTypeSens.Location = new System.Drawing.Point(62, 34);
            this.tbTypeSens.Name = "tbTypeSens";
            this.tbTypeSens.Size = new System.Drawing.Size(201, 20);
            this.tbTypeSens.TabIndex = 2;
            // 
            // tbModelSens
            // 
            this.tbModelSens.Location = new System.Drawing.Point(60, 93);
            this.tbModelSens.Name = "tbModelSens";
            this.tbModelSens.Size = new System.Drawing.Size(201, 20);
            this.tbModelSens.TabIndex = 3;
            // 
            // bOK
            // 
            this.bOK.Location = new System.Drawing.Point(124, 133);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(82, 25);
            this.bOK.TabIndex = 4;
            this.bOK.Text = "Принять";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // FormAddNewSensorsDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 172);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.tbModelSens);
            this.Controls.Add(this.tbTypeSens);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormAddNewSensorsDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Добавление нового датчика";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbTypeSens;
        private System.Windows.Forms.TextBox tbModelSens;
        private System.Windows.Forms.Button bOK;
    }
}