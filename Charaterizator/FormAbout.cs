using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Charaterizator
{
    public partial class FormAbout : Form
    {
        public static string AssemblyCopyright()
        {
            AssemblyCopyrightAttribute copyright = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0] as AssemblyCopyrightAttribute;
            return copyright.Copyright;
        }
        public FormAbout()
        {
            InitializeComponent();
          
            lAboutVersion.Text = Application.ProductVersion;
            lAboutAsmVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            lAboutCopyright.Text = AssemblyCopyright();

            lAboutDateTime.Text = File.GetCreationTime(GetType().Assembly.Location).ToString("F");
        }
    }
}
