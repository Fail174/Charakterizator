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
    public partial class FormAddNewSensorsDB : Form
    {
        public FormAddNewSensorsDB()
        {
            InitializeComponent();
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            FormSensorsDB.newTypeSens = tbTypeSens.Text;
            FormSensorsDB.newModelSens = tbModelSens.Text;
            this.Close();
        }
    }
}
