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
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
            // считываем данные из Settings и выводим на форму с настройками
            textBox3.Text = Charaterizator.Properties.Settings.Default.set_HoldTimeTemp.ToString();              
        }




        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            ofdDataBase.ShowDialog();
        }


        // 
        private void bSetSettings_Click(object sender, EventArgs e)
        {
            // считываем данные из формы и сохраняем их в Settings
            Charaterizator.Properties.Settings.Default.set_HoldTimeTemp = Convert.ToDouble(textBox3.Text);

        }

       
    }
}
