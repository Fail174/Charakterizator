using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TxtLog
{
    public class CTxtlog
    {
        private System.Windows.Forms.RichTextBox rtbConsole=null;
        private StreamWriter writer=null;//для лога
        public string LogFileName;

        public CTxtlog(System.Windows.Forms.RichTextBox rtb, string lfn)
        {
            rtbConsole = rtb;
            LogFileName = lfn;
            writer = File.CreateText(LogFileName);//создаем лог файл сессии
        }
        ~CTxtlog()
        {
//            if(writer!=null)
//           writer.Close();
        }

        public void WriteLineLog(string str, int status = 0)
        {
            try
            {
                if (rtbConsole != null)
                {

                    switch (status)
                    {
                        case 0:
                            rtbConsole.SelectionColor = Color.Black;
                            break;
                        case 1:
                            rtbConsole.SelectionColor = Color.Red;
                            Console.Beep();
                            break;
                        case 2:
                            rtbConsole.SelectionColor = Color.DarkGreen;
                            break;
                        default:
                            rtbConsole.SelectionColor = Color.Black;
                            break;
                    }
                    str = DateTime.Now + ": " + str;
                    rtbConsole.AppendText(str + Environment.NewLine);
                    rtbConsole.ScrollToCaret();

                    if (writer != null)
                    {
                        writer.WriteLine(str);
                        writer.Flush();
                    }
                }
            }
            catch
            {
                Console.WriteLine("Ошибка записи в лог!");
            }
        }

    }
}
