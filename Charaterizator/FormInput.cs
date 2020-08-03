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
    public partial class FormInput : Form
    {
        private string _Pressuer;
        public string Pressuer//ток в мА
        {
            get { return _Pressuer; }
            set
            {
                _Pressuer = value;
                nud_Pressuer.Value = Convert.ToInt32(_Pressuer);
            }
        }
        public FormInput()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           _Pressuer = nud_Pressuer.Value.ToString();
        }
    }
}
