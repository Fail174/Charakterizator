using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace Charaterizator
{
    public class CTxtlog
    {
        private System.Windows.Forms.RichTextBox rtbConsole;
        private StreamWriter writer;//для лога
        public string LogFileName;

        public CTxtlog(System.Windows.Forms.RichTextBox rtb, string lfn)
        {
            rtbConsole = rtb;
            LogFileName = lfn;
            writer = File.CreateText(LogFileName);//создаем лог файл сессии
            WriteLineLog(Application.ProductName + "  v." + Application.ProductVersion.ToString(),0);
            WriteLineLog("Версия сборки:" + (FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location)).FileVersion.ToString(), 0);

        }
        ~CTxtlog()
        {
//           writer.Close();
        }

        public void WriteLineLog(string str, int status = 0)
        {
            str = DateTime.Now + ": " + str;
            writer.WriteLine(str);
            writer.Flush();
            //Thread.Sleep(10);
            if (status == 0)
            {
                //                rtbConsole.ForeColor = Color.Black;
                rtbConsole.SelectionColor = Color.Black;
            }
            else
            {
                //                rtbConsole.ForeColor = Color.Red;
                rtbConsole.SelectionColor = Color.Red;
            }
            rtbConsole.AppendText(str + Environment.NewLine);
            rtbConsole.ScrollToCaret();
        }

    }
}
