using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Charaterizator
{
    public partial class MainForm : Form
    {
        private readonly Font DrawingFont = new Font(new FontFamily("DS-Digital"), 28.0F);
        public MainForm()
        {
            InitializeComponent();
            textBox1.Font = DrawingFont;
            textBox2.Font = DrawingFont;
            textBox3.Font = DrawingFont;
            textBox4.Font = DrawingFont;
            textBox5.Font = DrawingFont;
            numericUpDown1.Font = DrawingFont;
            numericUpDown2.Font = DrawingFont;

            Properties.Settings.Default.Save();  // Сохраняем переменные.
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
