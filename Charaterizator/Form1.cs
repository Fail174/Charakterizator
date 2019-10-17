using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ENI100;



namespace Charaterizator
{
    public partial class MainForm : Form
    {
        const int MAX_ERROR_COUNT = 5;

        private readonly Font DrawingFont = new Font(new FontFamily("DS-Digital"), 28.0F);
        private CMultimetr Multimetr = new CMultimetr();
        private ClassEni100 sensors = new ClassEni100();
        private FormSwitch Commutator = new FormSwitch();
        private FormMensor Mensor = new FormMensor();
        private int MaxChannalCount = 30;//максимальное количество каналов коммутаторы

        private StreamWriter writer;//для лога

        private int MultimetrReadError = 0;//число ошибко чтения данных с мультиметра
        private int MensorReadError = 0;//число ошибко чтения данных с задатчика давления
        

        //Инициализация переменных основной программы
        public MainForm()
        {
            InitializeComponent();
            writer = File.CreateText("Charakterizator.log");//создаем лог файл сессии

//            btmMultimetr_Click(null, null);           
//            btnCommutator_Click(null, null);
//            btnMensor_Click(null, null);

            //********************  Цифровой шрифт *********************
            tbMensorData.Font = DrawingFont;
            textBox2.Font = DrawingFont;
            tbMultimetrData.Font = DrawingFont;
            textBox4.Font = DrawingFont;
            textBox5.Font = DrawingFont;
            numericUpDown1.Font = DrawingFont;
            numericUpDown2.Font = DrawingFont;
            //**********************************************************

//            Properties.Settings.Default.Save();  // Сохраняем настройки программы
        }

        //Выполняем при загрузке главной формы
        private void MainForm_Load(object sender, EventArgs e)
        {
            btmMultimetr.PerformClick();
            btnCommutator.PerformClick();
            btnMensor.PerformClick();

            MainTimer.Enabled = true;
            MainTimer.Start();
        }

        //запись в лог и в окно выводы
        void WriteLineLog(string str, int status=0)
        {
            str = DateTime.Now + ":" + str;
            writer.WriteLine(str);
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem_MultimetrSetings_Click(object sender, EventArgs e)
        {
            FormPortSettings newForm = new FormPortSettings();
            newForm.InitPortsettings(Properties.Settings.Default.COMMultimetr,
                Properties.Settings.Default.COMMultimetr_Speed,
                Properties.Settings.Default.COMMultimetr_DatabBits,
                Properties.Settings.Default.COMMultimetr_StopBits,
                Properties.Settings.Default.COMMultimetr_Parity);
            if (newForm.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.COMMultimetr = newForm.GetPortName();
                Properties.Settings.Default.COMMultimetr_Speed = newForm.GetPortSpeed();
                Properties.Settings.Default.COMMultimetr_DatabBits = newForm.GetPortDataBits();
                Properties.Settings.Default.COMMultimetr_StopBits = newForm.GetPortStopBits();
                Properties.Settings.Default.COMMultimetr_Parity = newForm.GetPortParity();
                Properties.Settings.Default.Save();  // Сохраняем переменные.
            }
        }

        private void MI_MensorSetings_Click(object sender, EventArgs e)
        {
            FormPortSettings newForm = new FormPortSettings();
            newForm.InitPortsettings(Properties.Settings.Default.COMMensor,
                Properties.Settings.Default.COMMensor_Speed,
                Properties.Settings.Default.COMMensor_DataBits,
                Properties.Settings.Default.COMMensor_StopBits,
                Properties.Settings.Default.COMMensor_Parity);
            if (newForm.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.COMMensor = newForm.GetPortName();
                Properties.Settings.Default.COMMensor_Speed = newForm.GetPortSpeed();
                Properties.Settings.Default.COMMensor_DataBits = newForm.GetPortDataBits();
                Properties.Settings.Default.COMMensor_StopBits = newForm.GetPortStopBits();
                Properties.Settings.Default.COMMensor_Parity = newForm.GetPortParity();
                Properties.Settings.Default.Save();  // Сохраняем переменные.
            }
        }

        private void MI_CommutatorSetings_Click(object sender, EventArgs e)
        {
            FormPortSettings newForm = new FormPortSettings();
            newForm.InitPortsettings(Properties.Settings.Default.COMComutator,
                Properties.Settings.Default.COMComutator_Speed,
                Properties.Settings.Default.COMComutator_DataBits,
                Properties.Settings.Default.COMComutator_StopBits,
                Properties.Settings.Default.COMComutator_Parity);
            if (newForm.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.COMComutator = newForm.GetPortName();
                Properties.Settings.Default.COMComutator_Speed = newForm.GetPortSpeed();
                Properties.Settings.Default.COMComutator_DataBits = newForm.GetPortDataBits();
                Properties.Settings.Default.COMComutator_StopBits = newForm.GetPortStopBits();
                Properties.Settings.Default.COMComutator_Parity = newForm.GetPortParity();
                Properties.Settings.Default.Save();  // Сохраняем переменные.
            }
        }

        private void MI_ColdCameraSetings_Click(object sender, EventArgs e)
        {
            FormPortSettings newForm = new FormPortSettings();
            newForm.InitPortsettings(Properties.Settings.Default.COMColdCamera,
                Properties.Settings.Default.COMColdCamera_Speed,
                Properties.Settings.Default.COMColdCamera_DataBits,
                Properties.Settings.Default.COMColdCamera_StopBits,
                Properties.Settings.Default.COMColdCamera_Parity);
            if (newForm.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.COMColdCamera = newForm.GetPortName();
                Properties.Settings.Default.COMColdCamera_Speed = newForm.GetPortSpeed();
                Properties.Settings.Default.COMColdCamera_DataBits = newForm.GetPortDataBits();
                Properties.Settings.Default.COMColdCamera_StopBits = newForm.GetPortStopBits();
                Properties.Settings.Default.COMColdCamera_Parity = newForm.GetPortParity();
                Properties.Settings.Default.Save();  // Сохраняем переменные.
            }
        }

        private void MI_SensorSetings_Click(object sender, EventArgs e)
        {
            FormPortSettings newForm = new FormPortSettings();
            newForm.InitPortsettings(Properties.Settings.Default.COMSensor,
                Properties.Settings.Default.COMSensor_Speed,
                Properties.Settings.Default.COMSensor_DataBits,
                Properties.Settings.Default.COMSensor_StopBits,
                Properties.Settings.Default.COMSensor_Parity);
            if (newForm.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.COMSensor = newForm.GetPortName();
                Properties.Settings.Default.COMSensor_Speed = newForm.GetPortSpeed();
                Properties.Settings.Default.COMSensor_DataBits = newForm.GetPortDataBits();
                Properties.Settings.Default.COMSensor_StopBits = newForm.GetPortStopBits();
                Properties.Settings.Default.COMSensor_Parity = newForm.GetPortParity();
                Properties.Settings.Default.Save();  // Сохраняем переменные.
            }
        }

        //подключение мультиметра
        private void btmMultimetr_Click(object sender, EventArgs e)
        {
            if (Multimetr.Connect(Properties.Settings.Default.COMMultimetr,
                Properties.Settings.Default.COMMultimetr_Speed,
                Properties.Settings.Default.COMMultimetr_DatabBits,
                Properties.Settings.Default.COMMultimetr_StopBits,
                Properties.Settings.Default.COMMultimetr_Parity) >= 0)
            {
                btmMultimetr.BackColor = Color.Green;
                btmMultimetr.Text = "Подключен";
                WriteLineLog("Мультиметр подключен", 0);
            }
            else
            {
                btmMultimetr.BackColor = Color.Red;
                btmMultimetr.Text = "Не подключен";
                WriteLineLog("Мультиметр не подключен",1);
            }
        }

        // Подключение коммутатора
        private void btnCommutator_Click(object sender, EventArgs e)
        {
            if (Commutator.Connect(Properties.Settings.Default.COMComutator,
               Properties.Settings.Default.COMComutator_Speed,
               Properties.Settings.Default.COMComutator_DataBits,
               Properties.Settings.Default.COMComutator_StopBits,
               Properties.Settings.Default.COMComutator_Parity) >= 0)
            {
                btnCommutator.BackColor = Color.Green;
                btnCommutator.Text = "Подключен";
                WriteLineLog("Коммутатор подключен", 0);
            }
            else
            {
                btnCommutator.BackColor = Color.Red;
                btnCommutator.Text = "Не подключен";
                WriteLineLog("Коммутатор не подключен",1);
            }
        }



        private void btnFormCommutator_Click(object sender, EventArgs e)
        {
            //FormSwitch CommutatorWindow = new FormSwitch();
            if (Commutator != null)
            {
                Commutator.ShowDialog();
                
            }
            else
            {
                Commutator = new FormSwitch();
                btnCommutator.PerformClick();

            }

        }

        private void btnFormMensor_Click(object sender, EventArgs e)
        {
            FormMensor MensorWindow = new FormMensor();
            MensorWindow.ShowDialog();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)//Состояние датчика - подключение
            {
                if (Convert.ToBoolean(dataGridView1[3, e.RowIndex].Value))
                {
                    Commutator.SetConnectors(e.RowIndex, 2); // команда подключить датчик с индексом e.RowIndex
                    dataGridView1[3, e.RowIndex].Style.BackColor = Color.Green;
                }
                else
                {
                    Commutator.SetConnectors(e.RowIndex, 3); // команда отключить датчик с индексом e.RowIndex
                    dataGridView1[3, e.RowIndex].Style.BackColor = Color.Red;
                }
            }
        }

        //Поиск подключенных датчиков
        //Формирует списко датчиков в датагриде
        //Выход: число подключенных датчиков
        private int SeachConnectedSensor()
        {
            WriteLineLog("Старт поиска датчиков...", 0);

            dataGridView1.Rows.Clear();
            if (sensors.Connect(Properties.Settings.Default.COMSensor,
                Properties.Settings.Default.COMSensor_Speed,
                Properties.Settings.Default.COMSensor_DataBits,
                Properties.Settings.Default.COMSensor_StopBits,
                Properties.Settings.Default.COMSensor_Parity) >= 0)
            {
                for (int i = 0; i < MaxChannalCount; i++)
                {
                    WriteLineLog(string.Format("Поиск датчиков на линии {0}", i),0);
                    dataGridView1.Rows.Add(i + 1, "Отсутсвует", "", false, false);

                    // коммутируем
                    Commutator.SetConnectors(i, 2); // команда подключить датчик с индексом i

                    if (sensors.SeachSensor())//поиск датчиков
                    {
                        if (sensors.SelectSensor(0))
                        {//датчик найден, обновляем таблицу
                            dataGridView1.Rows[i].Cells[1].Value = sensors.sensor.GetdevType();     //тип датчика
                            dataGridView1.Rows[i].Cells[2].Value = sensors.sensor.Addr.ToString();  //адрес датчика по протоколу HART
                            dataGridView1.Rows[i].Cells[3].Value = false;                           //датчик подключен к измерительной линии
                            dataGridView1.Rows[i].Cells[4].Value = true;                            //исправность датчика
                            WriteLineLog("Датчик обнаружен", 0);
                        }
                    }
                    Commutator.SetConnectors(i, 3); // команда отключить датчик с индексом i
                }
            }
            else {
                WriteLineLog("Нет соединения с датчиками. Проверте подключение коммутатора.", 1);
            }
            return 0;
        }

        private void btnSensorSeach_Click_1(object sender, EventArgs e)
        {
            SeachConnectedSensor();
        }


        private void cbTest_CheckedChanged(object sender, EventArgs e)
        {

            if (cbTest.Checked)
            {


                Commutator.SetPower(10, 0);

                int qi = Commutator.CalcNumOfConnectInputs(Commutator._StateCHPower);
                textBox2.Text = Convert.ToString(qi);

               
            }
            else
            {

                Commutator.SetPower(10, 1);

                int qi = Commutator.CalcNumOfConnectInputs(Commutator._StateCHPower);
                textBox2.Text = Convert.ToString(qi);

            }

           
        }

        private void btnMensor_Click(object sender, EventArgs e)
        {
            if (Mensor.Connect(Properties.Settings.Default.COMMensor,
              Properties.Settings.Default.COMMensor_Speed,
              Properties.Settings.Default.COMMensor_DataBits,
              Properties.Settings.Default.COMMensor_StopBits,
              Properties.Settings.Default.COMMensor_Parity) >= 0)
            {
                btnMensor.BackColor = Color.Green;
                btnMensor.Text = "Подключен";
                WriteLineLog("Задатчик давления подключен", 0);
            }
            else
            {
                btnMensor.BackColor = Color.Red;
                btnMensor.Text = "Не подключен";
                WriteLineLog("Задатчик давления не подключен", 1);
            }
        }





        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            MainTimer.Stop();
            MainTimer.Enabled = false;
            writer.Close();//закрываем лог
        }



        private void MainTimer_Tick(object sender, EventArgs e)
        {
            if (Commutator.Connected)
            {
                int qi = Commutator.CalcNumOfConnectInputs(Commutator._StateCH);
                textBox2.Text = Convert.ToString(qi);
            }
            if (Multimetr.Connected)
            {
                ReadMultimetr();//обновляем данные с мультиметра
            }
            if (Mensor.Connected)
            {
                ReadMensor();//обновляем данные с задатчика давления
            }
        }
        
        //чтение данных с менсора
        private void ReadMensor()
        {
            float mData = Multimetr.ReadData();//чтение давления с Менсора
            if (mData != 0)
            {
                tbMensorData.Text = mData.ToString();
                MensorReadError = 0;
            }
            else
            {
                tbMensorData.Text = "";
                MensorReadError++;
            }
            if (MensorReadError >= MAX_ERROR_COUNT)
            {
                Mensor.DisConnect();
                btnMensor.BackColor = Color.Red;
                btnMensor.Text = "Не подключен";
                WriteLineLog("Нет данных с задатчика давления. Устройство отключено.", 1);
            }
        }

        //чтение данных с мультиметра
        private void ReadMultimetr()
        {
            float mData = Multimetr.ReadData();
            if (mData != 0)
            {
                tbMultimetrData.Text = mData.ToString();
                MultimetrReadError = 0;
            }
            else
            {
                tbMultimetrData.Text = "";
                MultimetrReadError++;
            }
            if (MultimetrReadError >= MAX_ERROR_COUNT)
            {
                Multimetr.DisConnect();
                btmMultimetr.BackColor = Color.Red;
                btmMultimetr.Text = "Не подключен";
                WriteLineLog("Нет данных с мультиметра. Устройство отключено.", 1);
            }
        }
    }
}
