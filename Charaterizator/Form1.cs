using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ENI100;



namespace Charaterizator
{

    public partial class MainForm : Form
    {
        const int MAX_ERROR_COUNT = 3; //Количество ошибок чтения данных с устройств перед отключением
        const int MaxChannalCount = 30;//максимальное количество каналов коммутаторы

        private readonly Font DrawingFont = new Font(new FontFamily("DS-Digital"), 28.0F);
        private CMultimetr Multimetr = new CMultimetr();
        private ClassEni100 sensors = new ClassEni100();
        private FormSwitch Commutator = new FormSwitch();
        private FormMensor Mensor = new FormMensor();
//        private int MaxChannalCount = 30;//максимальное количество каналов коммутаторы

        private CResultCH ResultCH = new CResultCH(MaxChannalCount);//результаты характеризации датчиков

        private int MultimetrReadError = 0;//число ошибко чтения данных с мультиметра
        private int MensorReadError = 0;//число ошибко чтения данных с менсора
        private bool SensorBusy = false;//Признак обмена данными с датчиками

        //Инициализация переменных основной программы
        public MainForm()
        {
            InitializeComponent();
            Program.txtlog = new CTxtlog(rtbConsole, "Charakterizator.log");//создаем класс лог, с выводов в richtextbox и в файл

            //            writer = File.CreateText("Charakterizator.log");//создаем лог файл сессии

            //            btmMultimetr_Click(null, null);           
            //            btnCommutator_Click(null, null);
            //            btnMensor_Click(null, null);

            //********************  Цифровой шрифт *********************
            tbMensorData.Font = DrawingFont;
            tbNumCH.Font = DrawingFont;
            tbMultimetrData.Font = DrawingFont;
            tbMensorRate.Font = DrawingFont;
            textBox5.Font = DrawingFont;
            numMensorPoint.Font = DrawingFont;
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
                dataGridView1.Rows.Add(i + 1, "Нет данных", "Нет данных", false, false, false);
                dataGridView1[4, i].Style.BackColor = Color.Red;
                dataGridView1[5, i].Style.BackColor = Color.Red;
                dataGridView1[6, i].Style.BackColor = Color.Red;
//                dataGridView2.Rows.Add("", "", "", "", "", "");
//                dataGridView3.Rows.Add("", "", "", "", "");

                cbChannalCharakterizator.Items.Add("Канал " + (i+1).ToString());
            }

            MainTimer.Enabled = true;
            MainTimer.Start();

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
                Program.txtlog.WriteLineLog("Мультиметр подключен", 0);
            }
            else
            {
                btmMultimetr.BackColor = Color.Red;
                btmMultimetr.Text = "Не подключен";
                Program.txtlog.WriteLineLog("Мультиметр не подключен", 1);
            }
        }



        // Подключение КОММУТАТОРА
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
                Program.txtlog.WriteLineLog("Коммутатор подключен", 0);
            }
            else
            {
                btnCommutator.BackColor = Color.Red;
                btnCommutator.Text = "Не подключен";
                Program.txtlog.WriteLineLog("Коммутатор не подключен", 1);
            }
        }



        // Подключение МЕНСОРА
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
                Program.txtlog.WriteLineLog("Задатчик давления подключен", 0);
            }
            else
            {
                btnMensor.BackColor = Color.Red;
                btnMensor.Text = "Не подключен";
                Program.txtlog.WriteLineLog("Задатчик давления не подключен", 1);
            }
        }



        // Обработка нажатия кнопки управления КОММУТАТОРОМ
        // Открываем окно с интерфейсом коммутатора
        private void btnFormCommutator_Click(object sender, EventArgs e)
        {
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



        // Обработка нажатия кнопки управления МЕНСОРОМ
        // Открываем окно с интерфейсом МЕНСОРА
        private void btnFormMensor_Click(object sender, EventArgs e)
        {
            if (Mensor != null)
            {
                Mensor.ShowDialog();
            }
            else
            {
                Mensor = new FormMensor();
                btnMensor.PerformClick();
            }
        }

        private void UpdateDataGrids(int i)
        {
            dataGridView1.Rows[i].Cells[1].Value = sensors.sensor.GetdevType();     //тип датчика
            dataGridView1.Rows[i].Cells[2].Value = sensors.sensor.uni.ToString();   //заводской номер
            dataGridView1.Rows[i].Cells[3].Value = true;   //Добавлен в список
//            dataGridView1.Rows[i].Cells[3].Style.BackColor = Color.Green;
            dataGridView1.Rows[i].Cells[6].Style.BackColor = Color.Green;
            dataGridView1.Rows[i].Cells[6].Value = true;                            //исправность датчика                            
        }
        private void UpdateSensorInfoPanel(int i)
        {
            if (sensors.SelectSensor(i))
            {
                tbSelChannalNumber.Text = string.Format("Канал {0}" , i+1);
                tbInfoDesc.Text = sensors.sensor.GetDesc();
                tbInfoTeg.Text = sensors.sensor.GetTeg();
                tbInfoUp.Text = sensors.sensor.UpLevel.ToString("f");
                tbInfoDown.Text = sensors.sensor.DownLevel.ToString("f");
                tbInfoSerialNumber.Text = sensors.sensor.SerialNumber.ToString();
                tbInfoMin.Text = sensors.sensor.MinLevel.ToString("f");
                tbInfoMesUnit.Text = sensors.sensor.GetUnit();
//                DateTime dt = new DateTime(1900 + (int)(sensors.sensor.data & 0xFF), (int)(sensors.sensor.data >> 8) & 0xFF, (int)((sensors.sensor.data >> 16) & 0xFF));
//                dtpInfoDate.Value = dt;

                cbInfoDevAddr.Text = sensors.sensor.Addr.ToString("D2");
                tbInfoFactoryNumber.Text = sensors.sensor.uni.ToString();
                tbInfoSoftVersion.Text = sensors.sensor.v3.ToString();
                cbInfoPreambul.Text = sensors.sensor.pre.ToString();
                tbInfoSensorType.Text = sensors.sensor.GetdevType();
                /*            string SelectedSensor = sensors.sensor.Addr.ToString("D2") + " | " + sensors.sensor.GetdevType() + " | " + sensors.sensor.uni;
                            tbCharact.Text = SelectedSensor;
                            tbCoef.Text = SelectedSensor;*/
            }
            else
            {
//                Program.txtlog.WriteLineLog("Датчик на выбранной линии не обнаружен!", 1);
                DialogResult result = MessageBox.Show(
                        "Выполнить поиск датчика?",
                        "Датчик на выбранной линии не обнаружен!",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.Yes)
                {
                    // коммутируем
                    Commutator.SetConnectors(i, 2); // команда подключить датчик с индексом i

                    if (sensors.SeachSensor(i))//поиск датчиков
                    {
                        Thread.Sleep(100);
                        if (sensors.SelectSensor(i))//выбор обнаруженного датчика
                        {//датчик найден, обновляем таблицу
                            Program.txtlog.WriteLineLog("Датчик обнаружен! Выполняем чтение параметров датчика по HART.", 0);
                            sensors.TegRead();          //читаем инфомацию о датчике
                            sensors.SensorRead();       //чтение данных с датчика
                            UpdateDataGrids(i);         //обновляем информацию по датчику в таблице
                        }
                        else
                        {
                            Program.txtlog.WriteLineLog(string.Format("Датчик на линии {0} обнаружен. Ошибка подключения к датчику!", i + 1), 1);
                        }
                    }
                    else
                    {
                        //                            Program.txtlog.WriteLineLog(string.Format("Нет подключения! Поиск датчиков на линии {0} не выполнен!",i+1), 1);
                        Program.txtlog.WriteLineLog(string.Format("Датчики на линии {0} не обнаружены!", i + 1), 1);
                    }

                    Commutator.SetConnectors(i, 3); // команда отключить датчик с индексом i                    
                }
            }
        }


        //чтение всех измеренных параметров с текущего датчика давления
        private void ReadSensorParametrs()
        {
            Program.txtlog.WriteLineLog("Старт операции характеризации для выбранных датчиков ... ", 0);

            for (int i = 0; i < MaxChannalCount; i++)//перебор каналов
            {
                Commutator.SetConnectors(i, 2);

                if (sensors.SelectSensor(i))//выбор датчика на канале i
                {
                    if (sensors.SensorValueReadC03())
                    {
                        ResultCH.AddPoint(i, sensors.sensor.Temperature, sensors.sensor.Pressure, sensors.sensor.OutVoltage, sensors.sensor.Resistance, sensors.sensor.OutCurrent);
                        cbChannalCharakterizator.SelectedIndex = i;
                        //UpDateCharakterizatorGrid(i);
                        Program.txtlog.WriteLineLog("Выполнено чтение параметров датчика в канале " + (i + 1).ToString(), 0);
                    }
                    else
                    {
                        Program.txtlog.WriteLineLog("Параметры датчика не прочитаны!", 1);
                    }
                }
                else
                {
                    Program.txtlog.WriteLineLog("Датчик не найден в канале " + (i + 1).ToString(), 1);
                }
            }
        }

        //обновляем грид для датчика в канале i
        private void UpDateCharakterizatorGrid(int i)
        {
            dataGridView2.Rows.Clear();
/*            while (dataGridView2.RowCount > 0)//удаляем записи предыдущего датчика
            {
                dataGridView2.Rows.RemoveAt(0);
            }*/
            for (int j=0;j<ResultCH.Channal[i].PointsCount;j++)//заполняем грид данными текущего датчика
            {
                dataGridView2.Rows.Add("", "", "", "", "", "");
                dataGridView2.Rows[j].Cells[0].Value = ResultCH.Channal[i].Points[j].Datetime.ToString();                 //
                dataGridView2.Rows[j].Cells[1].Value = ResultCH.Channal[i].Points[j].Temperature.ToString();   //
                dataGridView2.Rows[j].Cells[2].Value = ResultCH.Channal[i].Points[j].Pressure.ToString("f");   //
                dataGridView2.Rows[j].Cells[3].Value = ResultCH.Channal[i].Points[j].OutVoltage.ToString("f");
                dataGridView2.Rows[j].Cells[4].Value = ResultCH.Channal[i].Points[j].Resistance.ToString("f");
                dataGridView2.Rows[j].Cells[5].Value = ResultCH.Channal[i].Points[j].OutCurrent.ToString("f");
            }
        }

        //Поиск подключенных датчиков
        //Формирует списко датчиков в датагриде
        //Выход: число подключенных датчиков
        private int SeachConnectedSensor()
        {
            SensorBusy = true;
            try
            {
                Program.txtlog.WriteLineLog("Старт поиска датчиков...", 0);
                pbSensorSeach.Maximum = MaxChannalCount;
                pbSensorSeach.Minimum = 0;
                pbSensorSeach.Value = 0;

                sensors.sensorList.Clear();
                if (sensors.Connect(Properties.Settings.Default.COMSensor,
                    Properties.Settings.Default.COMSensor_Speed,
                    Properties.Settings.Default.COMSensor_DataBits,
                    Properties.Settings.Default.COMSensor_StopBits,
                    Properties.Settings.Default.COMSensor_Parity) >= 0)
                {
                    for (int i = 0; i < MaxChannalCount; i++)
                    {
                        Program.txtlog.WriteLineLog(string.Format("Поиск датчиков на линии {0} ...", i+1), 0);

                        // коммутируем
                        Commutator.SetConnectors(i, 2); // команда подключить датчик с индексом i

                        if (sensors.SeachSensor(i))//поиск датчиков
                        {
                            Thread.Sleep(100);
                            if (sensors.SelectSensor(i))//выбор обнаруженного датчика
                            {//датчик найден, обновляем таблицу
                                Program.txtlog.WriteLineLog("Датчик обнаружен! Выполняем чтение параметров датчика по HART.", 0);
                                sensors.TegRead();          //читаем инфомацию о датчике
                                sensors.SensorRead();       //чтение данных с датчика
                                UpdateDataGrids(i);         //обновляем информацию по датчику в таблице
                            }
                            else
                            {
                                Program.txtlog.WriteLineLog(string.Format("Датчик на линии {0} обнаружен. Ошибка подключения к датчику!", i+1), 1);
                            }
                        }
                        else
                        {
//                            Program.txtlog.WriteLineLog(string.Format("Нет подключения! Поиск датчиков на линии {0} не выполнен!",i+1), 1);
                            Program.txtlog.WriteLineLog(string.Format("Датчики на линии {0} не обнаружены!", i + 1), 1);
                        }
                        Commutator.SetConnectors(i, 3); // команда отключить датчик с индексом i                    
                        pbSensorSeach.Value = i;
                    }
                }
                else
                {
                    Program.txtlog.WriteLineLog("Нет соединения с датчиками. Проверте подключение коммутатора.", 1);

                }
            }
            finally
            {
                SensorBusy = false;
                pbSensorSeach.Value = 0;
            }

            return 0;
        }
        private void btnSensorSeach_Click_1(object sender, EventArgs e)
        {
            if (!SensorBusy)
            {
                try
                {
                    btnSensorSeach.Enabled = false;
                    SeachConnectedSensor();
                }
                finally
                {
                    btnSensorSeach.Enabled = true;
                }
            }
            else
            {
                Program.txtlog.WriteLineLog("Команда не выполнена. Идет обмен данными с датчиками.",0);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            MainTimer.Stop();
            MainTimer.Enabled = false;
            sensors.DisConnect();
            Mensor.DisConnect();
            Multimetr.DisConnect();
            Commutator.DisConnect();
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


            if (Mensor.Connected)
            {
                ReadMensor(); //обновляем данные с Менсора
            }
        }


        //чтение данных с МЕНСОРА
        private void ReadMensor()
        {
            int CH_mensor = Mensor._activCH;    // Получаем номер активного канала (0 значит А,   1 значит B,   -1 = не прочитали )                    

            if (CH_mensor != -1)
            {
                // Получаем текущее значение давления и обновляем гл. форму 
                tbMensorData.Text = Mensor._press.ToString();
                // Получаем тек. значение уставки  и обновляем гл. форму
                numMensorPoint.Text = Mensor._point.ToString();
                // Получаем тек. значение скорости  и обновляем гл. форму
                tbMensorRate.Text = Mensor._rate.ToString();

                // Получаем тип преобразователя (удерживаемый диапазон)
                int typeR = Mensor._typeR;  // 0-Д1П1,  1-Д2П1,  2-AutoRange 
                // Обновляем тип преобзарователя
                if (CH_mensor == 0)
                {
                    cbMensorTypeR.SelectedIndex = typeR; // от 0 до 2-х по списку
                }
                else if (CH_mensor == 1)
                {
                    cbMensorTypeR.SelectedIndex = typeR + 3; // от 3 до 5-ти по списку
                }

                MensorReadError = 0;
            }
            else
            {
                tbMensorData.Text = "";
                tbMensorRate.Text = "";
                numMensorPoint.Text = "";
                cbMensorTypeR.SelectedIndex = -1;
                MensorReadError++;
            }

            if (MensorReadError >= MAX_ERROR_COUNT)
            {
                Mensor.DisConnect();
                btnMensor.BackColor = Color.Red;
                btnMensor.Text = "Не подключен";
                Program.txtlog.WriteLineLog("Нет данных с задатчика давления. Устройство отключено.", 1);
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
                Program.txtlog.WriteLineLog("Нет данных с мультиметра. Устройство отключено.", 1);
            }
        }



        // Обновление на главной форме состояния каналов коммутатора (checkbox)
        private void StateComutators(Int32 data32)
        {

            for (int i = 0; i < MaxChannalCount; i++)
            {
                if (((data32 >> i) & 01) == 1)
                {
                    dataGridView1.Rows[i].Cells[4].Value = 1;
                    dataGridView1[4, i].Style.BackColor = Color.Green;
                }
                else
                {
                    dataGridView1.Rows[i].Cells[4].Value = 0;
                    dataGridView1[4, i].Style.BackColor = Color.Red;
                }
            }

            tbNumCH.Text = Convert.ToString(Commutator._NumOfConnectInputs);
        }


        // Обновление на главной форме состояния питания коммутатора (checkbox)
        private void PowerComutators(Int32 data32)
        {
            for (int i = 0; i < MaxChannalCount; i++)
            {
                if (((data32 >> i) & 01) == 1)
                {
                    dataGridView1.Rows[i].Cells[5].Value = true;
                    dataGridView1[5, i].Style.BackColor = Color.Green;
                }
                else
                {
                    dataGridView1.Rows[i].Cells[5].Value = false;
                    dataGridView1[5, i].Style.BackColor = Color.Red;
                }
            }
        }




        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!Commutator.Connected)
            {
                MessageBox.Show("Нет подключения к коммутатору!", "Операция прервана", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (e.ColumnIndex <= 2)//выбор датчика
            {
                UpdateSensorInfoPanel(e.RowIndex);
            }

            if (e.ColumnIndex == 4)//Состояние датчика - подключение
            {
                dataGridView1.Rows[e.RowIndex].Cells[4].Value = !Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[4].Value);

                if (Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[4].Value))
                {
                    Commutator.SetConnectors(e.RowIndex, 2); // команда подключить датчик с индексом e.RowIndex
                    dataGridView1[4, e.RowIndex].Style.BackColor = Color.Green;

                }
                else
                {
                    Commutator.SetConnectors(e.RowIndex, 3); // команда отключить датчик с индексом e.RowIndex
                    dataGridView1[4, e.RowIndex].Style.BackColor = Color.Red;

                }

            }

            else if (e.ColumnIndex == 5)// Питание датчика - подключение
            {
                dataGridView1.Rows[e.RowIndex].Cells[5].Value = !Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[5].Value);


                if (Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[5].Value))
                {
                    Commutator.SetPower(e.RowIndex, 0);     // команда подключить питание датчика
                    dataGridView1[5, e.RowIndex].Style.BackColor = Color.Green;
                }
                else
                {
                    Commutator.SetPower(e.RowIndex, 1);   // команда отключить питание датчика
                    dataGridView1[5, e.RowIndex].Style.BackColor = Color.Red;
                }
            }

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Visible = (tabControl1.SelectedIndex == 0);
            dataGridView2.Visible = (tabControl1.SelectedIndex == 1);
            dataGridView4.Visible = (tabControl1.SelectedIndex == 1);
            dataGridView3.Visible = (tabControl1.SelectedIndex == 2);
        }


        // Отработка нажатия на гл. форме МЕНСОР - ЗАДАЧА
        private void bMensorControl_Click(object sender, EventArgs e)
        {
            if (Mensor._serialPort_M.IsOpen)
            {
                Mensor.SetMode(1);
            }
            else
            {
                Program.txtlog.WriteLineLog("Нет Связи. Задатчик давления не подключен", 1);
            }
        }


        // Отработка нажатия на гл. форме МЕНСОР - СБРОС
        private void button7_Click(object sender, EventArgs e)
        {
            if (Mensor._serialPort_M.IsOpen)
            {
                Mensor.SetMode(2);
            }
            else
            {
                Program.txtlog.WriteLineLog("Нет Связи. Задатчик давления не подключен", 1);
            }

        }


        // Отработка выбора на гл.форме ТИПА ПРЕОБРАЗОВАТЕЛЯ МЕНСОРА из списка
        private void cbMensorTypeR_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!Mensor._serialPort_M.IsOpen)
            {
                if (cbMensorTypeR.SelectedIndex != -1)
                {
                    Program.txtlog.WriteLineLog("Нет Связи. Задатчик давления не подключен", 1);
                    cbMensorTypeR.SelectedIndex = -1;
                }
                return;
            }

            try
            {
                MainTimer.Stop();
                MainTimer.Enabled = false;

                // Получаем индекс выбранного преобразователя
                int ind = cbMensorTypeR.SelectedIndex;

                // Определяем тип канала соответствующего выбранному преобразователю
                if ((ind >= 0) && (ind <= 2))  // активный канал А
                {

                    Mensor.ChannelSet("A");   // устанвливаем активным канал A
                    Thread.Sleep(100);
                    Mensor.SetTypeRange(ind);     // Устанавливаем тип выбранного преобразователя
                }
                else if ((ind >= 0) && (ind <= 2)) // активный канал B
                {
                    Mensor.ChannelSet("B");   // устанвливаем активным канал B
                    Thread.Sleep(100);
                    Mensor.SetTypeRange(ind - 3);     // Устанавливаем тип выбранного преобразователя
                }

            }

            finally
            {
                MainTimer.Enabled = true;
                MainTimer.Start();
            }


        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!Mensor._serialPort_M.IsOpen)
            {
                Program.txtlog.WriteLineLog("Нет Связи. Задатчик давления не подключен", 1);
                return;
            }

            double Point = (double)numMensorPoint.Value;  // получаем заданное значение уставки
            Mensor.SetPoint(Point);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            btnCHStart.BackColor = Color.IndianRed;
            btnCHStart.Text = "Стоп"; 
            ReadSensorParametrs();
            btnCHStart.Text = "Старт";
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex == 3)&&(dataGridView1.RowCount>0))//выбор датчиков
            {
                bool CurentSet = Convert.ToBoolean(dataGridView1.Rows[0].Cells[3].Value);
                for (int i = 0; i <= (dataGridView1.RowCount - 1); i++)
                {
                    dataGridView1.Rows[i].Cells[3].Value = !CurentSet;
//                    dataGridView1.Rows[i].Cells[3].Style.BackColor = Color.White;
                }
            }
        }

        private void cbChannalCharakterizator_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpDateCharakterizatorGrid(cbChannalCharakterizator.SelectedIndex);
        }

        private void gbCHLevel1_Enter(object sender, EventArgs e)
        {
            gbCHLevel1.BackColor = Color.LightGreen;
            gbCHLevel2.BackColor = Color.Transparent;
        }

        private void gbCHLevel2_Enter(object sender, EventArgs e)
        {
            gbCHLevel2.BackColor = Color.LightGreen;
            gbCHLevel1.BackColor = Color.Transparent;
        }

        private void btnNextStep1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void btnNextStep2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void btnCHPressureSet1_Click(object sender, EventArgs e)
        {
            btnCHStart.BackColor = Color.LightGreen;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            btnCHStart.BackColor = Color.LightGreen;
        }

        private void MenuItemMainSettings_Click(object sender, EventArgs e)
        {
            FormSettings Settings = new FormSettings();
            Settings.ShowDialog();
        }
    }
}

