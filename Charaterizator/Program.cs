using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TxtLog;


namespace Charaterizator
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainwnd = new MainForm();
            Application.Run(mainwnd);
            //            Application.Run(new MainForm());
        }
        public static CTxtlog txtlog;
        public static MainForm mainwnd;
    }
}
