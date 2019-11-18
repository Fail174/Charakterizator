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


     public void bOK_Click(object sender, EventArgs e)
     {
        this.DialogResult = DialogResult.OK;  
     }


     public string newTypeSens
     {
         get { return tbTypeSens.Text; }
     }
     public string newModelSens
     {
         get { return tbModelSens.Text; }
     }
        
        
    }


}
