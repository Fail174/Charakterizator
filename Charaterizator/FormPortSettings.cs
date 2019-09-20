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
    public partial class FormPortSettings : Form
    {
        public FormPortSettings()
        {
            InitializeComponent();
        }

        public void InitPortsettings(string PortName, int Speed, int DataBit, int StopBit, int Parity)
        {
            cbPortName.Text = PortName;
            cbSpeed.Text = Speed.ToString();
            cbStopBit.Text = StopBit.ToString();
            cbParity.Text = Parity.ToString();
            cbDataBit.Text = DataBit.ToString();
        }
        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
