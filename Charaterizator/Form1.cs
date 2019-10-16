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

        //Инициализация переменных основной программы
        public MainForm()
        {
            InitializeComponent();
            writer = File.CreateText("Charakterizator.log");//создаем лог файл сессии

//            btmMultimetr_Click(null, null);           
//            btnCommutator_Click(null, null);
//            btnMensor_Click(null, null);

            //********************  Цифровой шрифт *********************
            textBox1.Font = DrawingFont;
            tbNumCH.Font = DrawingFont;
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

            // btnSensorSeach.PerformClick();
            for (int i = 0; i < MaxChannalCount; i++)
            {
                dataGridView1.Rows.Add(i + 1, "Отсутсвует", "", false, false);
                dataGridView1[3, i].Style.BackColor = Color.Red;
                dataGridView1[4, i].Style.BackColor = Color.Red;
                dataGridView1[5, i].Style.BackColor = Color.Red;
            }







                MainTimer.Enabled = true;
            MainTimer.Start();

        }

        //запись в лог и в окно выводы
        void WriteLineLog(string str, int status=0)
        {
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
            else if (e.ColumnIndex == 4)// Питание датчика - подключение
            {
                if (Convert.ToBoolean(dataGridView1[4, e.RowIndex].Value))
                {
                    Commutator.SetPower(e.RowIndex, 0);     // команда подключить питание датчика
                    dataGridView1[4, e.RowIndex].Style.BackColor = Color.Green;  

                }
                else
                {
                    Commutator.SetPower(e.RowIndex, 1);   // команда отключить питание датчика
                    dataGridView1[4, e.RowIndex].Style.BackColor = Color.Red;

                }
            }


        }

        //Поиск подключенных датчиков
        //Формирует списко датчиков в датагриде
        //Выход: число подключенных датчиков
        private int SeachConnectedSensor()
        {
            WriteLineLog("Старт поиска датчиков...", 0);

            //dataGridView1.Rows.Clear();
            if (sensors.Connect(Properties.Settings.Default.COMSensor,
                Properties.Settings.Default.COMSensor_Speed,
                Properties.Settings.Default.COMSensor_DataBits,
                Properties.Settings.Default.COMSensor_StopBits,
                Properties.Settings.Default.COMSensor_Parity) >= 0)
            {
                for (int i = 0; i < MaxChannalCount; i++)
                {
                    WriteLineLog(string.Format("Поиск датчиков на линии {0}", i),0);
                    //dataGridView1.Rows.Add(i + 1, "Отсутсвует", "", false, false);

                    // коммутируем
                    Commutator.SetConnectors(i, 2); // команда подключить датчик с индексом i

                    if (sensors.SeachSensor())//поиск датчиков
                    {
                        if (sensors.SelectSensor(0))
                        {//датчик найден, обновляем таблицу
                            dataGridView1.Rows[i].Cells[1].Value = sensors.sensor.GetdevType();     //тип датчика
                            dataGridView1.Rows[i].Cells[2].Value = sensors.sensor.Addr.ToString();  //адрес датчика по протоколу HART
                            //dataGridView1.Rows[i].Cells[3].Value = false;                           //датчик подключен к измерительной линии
                            //dataGridView1.Rows[i].Cells[4].Value = false;                           //датчик подключен к измерительной линии
                            dataGridView1.Rows[i].Cells[5].Value = true;                            //исправность датчика
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
                StateComutators(Commutator._StateCH);
                PowerComutators(Commutator._StateCHPower); //обновляем данные с коммутатора
            }

           

            if (Multimetr.Connected)
            {
                ReadMultimetr(); //обновляем данные с мультиметра
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


        // Обновление состояния каналов коммутатора (checkbox)
        private void StateComutators(Int32 data32)
        {
            dataGridView1.Rows[0].Cells[3].Value = (data32 & 0x1); dataGridView1.Rows[15].Cells[3].Value = (data32 & 0x8000) >> 15;
            dataGridView1.Rows[1].Cells[3].Value = (data32 & 0x2) >> 1; dataGridView1.Rows[16].Cells[3].Value = (data32 & 0x10000) >> 16;
            dataGridView1.Rows[2].Cells[3].Value = (data32 & 0x4) >> 2; dataGridView1.Rows[17].Cells[3].Value = (data32 & 0x20000) >> 17;
            dataGridView1.Rows[3].Cells[3].Value = (data32 & 0x8) >> 3; dataGridView1.Rows[18].Cells[3].Value = (data32 & 0x40000) >> 18;
            dataGridView1.Rows[4].Cells[3].Value = (data32 & 0x10) >> 4; dataGridView1.Rows[19].Cells[3].Value = (data32 & 0x80000) >> 19;
            dataGridView1.Rows[5].Cells[3].Value = (data32 & 0x20) >> 5; dataGridView1.Rows[20].Cells[3].Value = (data32 & 0x100000) >> 20;
            dataGridView1.Rows[6].Cells[3].Value = (data32 & 0x40) >> 6; dataGridView1.Rows[21].Cells[3].Value = (data32 & 0x200000) >> 21;
            dataGridView1.Rows[7].Cells[3].Value = (data32 & 0x80) >> 7; dataGridView1.Rows[22].Cells[3].Value = (data32 & 0x400000) >> 22;
            dataGridView1.Rows[8].Cells[3].Value = (data32 & 0x100) >> 8; dataGridView1.Rows[23].Cells[3].Value = (data32 & 0x800000) >> 23;
            dataGridView1.Rows[9].Cells[3].Value = (data32 & 0x200) >> 9; dataGridView1.Rows[24].Cells[3].Value = (data32 & 0x1000000) >> 24;
            dataGridView1.Rows[10].Cells[3].Value = (data32 & 0x400) >> 10; dataGridView1.Rows[25].Cells[3].Value = (data32 & 0x2000000) >> 25;
            dataGridView1.Rows[11].Cells[3].Value = (data32 & 0x800) >> 11; dataGridView1.Rows[26].Cells[3].Value = (data32 & 0x4000000) >> 26;
            dataGridView1.Rows[12].Cells[3].Value = (data32 & 0x1000) >> 12; dataGridView1.Rows[27].Cells[3].Value = (data32 & 0x8000000) >> 27;
            dataGridView1.Rows[13].Cells[3].Value = (data32 & 0x2000) >> 13; dataGridView1.Rows[28].Cells[3].Value = (data32 & 0x10000000) >> 28;
            dataGridView1.Rows[14].Cells[3].Value = (data32 & 0x4000) >> 14; dataGridView1.Rows[29].Cells[3].Value = (data32 & 0x20000000) >> 29;
            
            tbNumCH.Text = Convert.ToString(Commutator._NumOfConnectInputs);
        }


        // Обновление состояния каналов коммутатора (checkbox)
        private void PowerComutators(Int32 data32)
        {
            if((data32 & 0x1)==1)
            {   dataGridView1.Rows[0].Cells[4].Value = 1;
                dataGridView1[4, 0].Style.BackColor = Color.Green;  }
            else
            {   dataGridView1.Rows[0].Cells[4].Value = 0;
                dataGridView1[4, 0].Style.BackColor = Color.Red;    }

            if ((data32 & 0x2) ==2)
            {
                dataGridView1.Rows[1].Cells[4].Value = 1;
                dataGridView1[4, 1].Style.BackColor = Color.Green;
            }
            else
            {
                dataGridView1.Rows[1].Cells[4].Value = 0;
                dataGridView1[4, 1].Style.BackColor = Color.Red;
            }

            /*dataGridView1.Rows[0].Cells[4].Value = (data32 & 0x1); */ dataGridView1.Rows[15].Cells[4].Value = (data32 & 0x8000) >> 15;
            /*dataGridView1.Rows[1].Cells[4].Value = (data32 & 0x2) >> 1;*/ dataGridView1.Rows[16].Cells[4].Value = (data32 & 0x10000) >> 16;
            dataGridView1.Rows[2].Cells[4].Value = (data32 & 0x4) >> 2; dataGridView1.Rows[17].Cells[4].Value = (data32 & 0x20000) >> 17;
            dataGridView1.Rows[3].Cells[4].Value = (data32 & 0x8) >> 3; dataGridView1.Rows[18].Cells[4].Value = (data32 & 0x40000) >> 18;
            dataGridView1.Rows[4].Cells[4].Value = (data32 & 0x10) >> 4; dataGridView1.Rows[19].Cells[4].Value = (data32 & 0x80000) >> 19;
            dataGridView1.Rows[5].Cells[4].Value = (data32 & 0x20) >> 5; dataGridView1.Rows[20].Cells[4].Value = (data32 & 0x100000) >> 20;
            dataGridView1.Rows[6].Cells[4].Value = (data32 & 0x40) >> 6; dataGridView1.Rows[21].Cells[4].Value = (data32 & 0x200000) >> 21;
            dataGridView1.Rows[7].Cells[4].Value = (data32 & 0x80) >> 7; dataGridView1.Rows[22].Cells[4].Value = (data32 & 0x400000) >> 22;
            dataGridView1.Rows[8].Cells[4].Value = (data32 & 0x100) >> 8; dataGridView1.Rows[23].Cells[4].Value = (data32 & 0x800000) >> 23;
            dataGridView1.Rows[9].Cells[4].Value = (data32 & 0x200) >> 9; dataGridView1.Rows[24].Cells[4].Value = (data32 & 0x1000000) >> 24;
            dataGridView1.Rows[10].Cells[4].Value = (data32 & 0x400) >> 10; dataGridView1.Rows[25].Cells[4].Value = (data32 & 0x2000000) >> 25;
            dataGridView1.Rows[11].Cells[4].Value = (data32 & 0x800) >> 11; dataGridView1.Rows[26].Cells[4].Value = (data32 & 0x4000000) >> 26;
            dataGridView1.Rows[12].Cells[4].Value = (data32 & 0x1000) >> 12; dataGridView1.Rows[27].Cells[4].Value = (data32 & 0x8000000) >> 27;
            dataGridView1.Rows[13].Cells[4].Value = (data32 & 0x2000) >> 13; dataGridView1.Rows[28].Cells[4].Value = (data32 & 0x10000000) >> 28;
            dataGridView1.Rows[14].Cells[4].Value = (data32 & 0x4000) >> 14; dataGridView1.Rows[29].Cells[4].Value = (data32 & 0x20000000) >> 29;

        }






    }
}
