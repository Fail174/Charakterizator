using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace Charaterizator
{
    public partial class FormPortSettings : Form
    {
        public FormPortSettings()
        {
            InitializeComponent();
            // Посмотрим есть ли в системе порты
            bool res = SerialPort.GetPortNames().Length <= 0 ? false : true;
            // Если портов нет выходим если есть пробуем подключится и сканируем
            if (!res)
            {
                return;
            }
            // Обновим список доступных портов в системе
            string [] _PortNames = SerialPort.GetPortNames();
            cbPortName.Items.AddRange(_PortNames);
            cbPortName.Text = _PortNames[0];

        }

        public void InitPortsettings(string PortName, int Speed, int DataBit, int StopBit, int Parity)
        {
            cbPortName.Text = PortName;
            cbSpeed.Text = Speed.ToString();
            cbStopBit.Text = StopBit.ToString();
            cbParity.SelectedIndex = Parity;
            cbDataBit.Text = DataBit.ToString();
        }
        private void label3_Click(object sender, EventArgs e)
        {

        }

        //Возвращает имя выбранного порта
        public string GetPortName()
        {
            return cbPortName.Text;
        }

        //Возвращает скорость порта
        public int GetPortSpeed()
        {
            return Convert.ToInt32(cbSpeed.Text);
        }

        //Возвращает количество стоп битов
        public int GetPortStopBits()
        {
            return Convert.ToInt32(cbStopBit.Text);
        }

        //Возвращает количество битов данных
        public int GetPortDataBits()
        {
            return Convert.ToInt32(cbDataBit.Text);
        }

        //Возвращает четность
        public int GetPortParity()
        {
            return cbParity.SelectedIndex;
        }
    }
}
