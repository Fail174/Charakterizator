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
            string[] split = FormSensorsDB.SensNameList.Split('$');
            for (int j = 0; j < split.Length - 1; j++)
            {
                cbSetSensName.Items.Add(split[j]);
            }
            if (cbSetSensName.Items.Count > 0)
            {
                cbSetSensName.SelectedIndex = cbSetSensName.FindString(Charaterizator.MainForm.SensorsDB.selType);          
            }

        }


        public void bOK_Click(object sender, EventArgs e)
     {
        this.DialogResult = DialogResult.OK;  
     }


     public string newTypeSens
     {
         get { return cbSetSensName.Text; }
     }
     public string newModelSens
     {
         get { return tbModelSens.Text; }
     }
        
        
    }


}
