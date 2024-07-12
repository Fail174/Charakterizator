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
        private System.Windows.Forms.RichTextBox rtbConsoleErrors = null; // добавлена доп. консоль для отдельного вывода информации об ошибках 12.07.2024
        private StreamWriter writer=null;//для лога
        public string LogFileName;

        public CTxtlog(System.Windows.Forms.RichTextBox rtb, string lfn)
        {
            rtbConsole = rtb;
            rtbConsoleErrors = rtb;     // добавлена доп. консоль для отдельного вывода информации об ошибках 12.07.2024
            LogFileName = lfn;
            writer = File.CreateText(LogFileName);//создаем лог файл сессии
        }
        ~CTxtlog()
        {
//            if(writer!=null)
//           writer.Close();
        }

        // 12.07.2024 добавлен параметр outInfo_in_consoleErrors
        // если outInfo_in_consoleErrors = true, то дублируем сообщение в rtbConsoleErrors
        public void WriteLineLog(string str, int status = 0, bool outInfo_in_consoleErrors = false)
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

                    if (outInfo_in_consoleErrors) // если outInfo_in_consoleErrors = true, то дополнительно выводим сообщение в rtbConsoleErrors 
                    {
                        rtbConsoleErrors.AppendText(str + Environment.NewLine);
                        rtbConsoleErrors.ScrollToCaret();
                    }

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

        // добавлена функция очистки rtbConsoleErrors /12.070.2024
        public void clear_rtbConsoleErrors()
        {
            rtbConsoleErrors.Clear();
        }

    }
}
