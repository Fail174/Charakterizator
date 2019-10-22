using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charaterizator
{
    public class CTxtlog
    {
        private System.Windows.Forms.RichTextBox rtbConsole;
        private StreamWriter writer;//для лога
        string LogFileName;

        public CTxtlog(System.Windows.Forms.RichTextBox rtb, string lfn)
        {
            rtbConsole = rtb;
            LogFileName = lfn;
            writer = File.CreateText(LogFileName);//создаем лог файл сессии
        }
        ~CTxtlog()
        {
           // writer.Close();
        }

        public void WriteLineLog(string str, int status = 0)
        {
            str = DateTime.Now + ": " + str;
            writer.WriteLine(str);
            writer.Flush();
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
        }

    }
}
