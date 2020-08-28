using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Threading;
using TxtLog;
using Charaterizator;



namespace SensorProgrammer
{
    public partial class FormSensorProgrammer : Form
    {
        // Переменные для работы с БД
        OleDbDataReader reader;
        OleDbCommand command;
        public OleDbConnection _сonnection;

        // Объекты классов из characterizator 
        private FormSwitch Commutator = new FormSwitch();
        private ClassEni100 sensors = new ClassEni100(15);       

        // Структура с параметрами датчика из БД
        struct SensParam
        {
            public string Type;
            public string Model;
            //public string Serial;
            public string Pmin;
            public string Pmax;
            public string NumOfRange;
            public string DeltaRange;
            public string Gain1;
            public string Range1Pmin;
            public string Range1Pmax;
            public string Gain2;
            public string Range2Pmin;
            public string Range2Pmax;                       
        }
        SensParam sensParam;


            public FormSensorProgrammer()
        {
            Program.txtlog = new CTxtlog(rtbConsole, "DB.log");//создаем класс лог, с выводов в richtextbox и в файл
            InitializeComponent();
        }


        

        // Операции выполняемые при загрузке формы...
        // устанавливаем соединение с коммутатором (с параметрами из настроек settings)
        // устанавливаем связь с БД (путь файла к БД из настроек settings)
        // учтанавливаем связь с датчиками
        private void FormSensorProgrammer_Load(object sender, EventArgs e)
        {
            // Подключение коммутатора
            SetCOMconnect();
            
            // Подключение БД
            string FileName = Properties.Settings.Default.DBfileName;  // получаем путь и имя файла из Settings
            SetConectDB(FileName);                                     // устанавливаем соединение с БД    
            // Если соединение с БД установлено 
            if (_сonnection.State == System.Data.ConnectionState.Open)
            {
                SetdgwMainWindow(); // первоначальное заполенение dgwMainWindow
                FilldwgSensParam(); // отображение окна с параметрами датчиков
            }
            // Подключаем датчики
            SetSensorsConnect();
            
        }

        

        //********************************* РАБОТА С БД ******************************************

        // Установка соединения с БД
        void SetConectDB(string strFileNameDB)
        {
            string connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source = " + strFileNameDB + ";";            
            _сonnection = new OleDbConnection(connectString); // Создаем экземпляр класса OleDBConnection для подключения к БД
            // Открываем подключение к БД
            try
            {
                _сonnection.Open();
                bDataBase.BackColor = Color.ForestGreen;
                bDataBase.Text = "База Данных: ПОДКЛЮЧЕНА";
                label1.Text = "Путь к БД: " + strFileNameDB;
            }
            catch (OleDbException ex)
            {
                //MessageBox.Show(ex.Message);   
                bDataBase.BackColor = Color.IndianRed;
                bDataBase.Text = "База Данных: НЕ ПОДКЛЮЧЕНА!";
                label1.Text = "Не выполнено подключение к БД. Файл с базой данных не найден!" ;
            }
            finally
            {            
            }            
        }



        // ПЕРЕПОДКЛЮЧИТЬ БД по нажатию кнопки
        private void bDataBase_Click(object sender, EventArgs e)
        {
            OpenFileDialog _openFileDialog = new OpenFileDialog();
            _openFileDialog.Filter = "access files (*.mdb)|*.mdb";

            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // получаем выбранный файл
                    string FileName = _openFileDialog.FileName;
                    // Если соединение с БД установлено - то закрываем
                    if (_сonnection.State == System.Data.ConnectionState.Open)
                    {
                        _сonnection.Close();
                    }
                    // Устанавливаем соединение с БД
                    SetConectDB(FileName);
                    // Сохраняем новый путь к БД
                    Properties.Settings.Default.DBfileName = FileName;
                    Properties.Settings.Default.Save();
                    // Очищаем dgwMainWindow и combobox
                    dgwMainWindow.Rows.Clear();
                    cbType.Items.Clear();
                    cbModel.Items.Clear();
                    // Перезаполняем dgwMainWindow
                    SetdgwMainWindow();
                }
                catch
                {
                    //MessageBox.Show(ex.Message);
                    MessageBox.Show("Не удалось открыть файл с базой данных!", "Открытие файла БД. Операция прервана", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }



        // Получение списка всех моделей из БД
        void GetTypeDB()
        {
            // текст запроса              
            string query = "SELECT DISTINCT Type FROM TSensors ORDER BY Type";
            // создаем объект OleDbCommand для выполнения запроса к БД MS Access
            OleDbCommand command = new OleDbCommand(query, _сonnection);
            try
            { // получаем объект OleDbDataReader для чтения табличного результата запроса SELECT
                reader = command.ExecuteReader();
                // в цикле построчно читаем ответ от БД
                while (reader.Read())
                {
                    cbType.Items.Add(reader[0].ToString());
                }
            }
            catch
            {
                MessageBox.Show("Не удалось прочитать типы датчиков из БД. Возможно файл с БД пуст или не соответствует требуемой структуре", "Чтение данных из БД. Операция прервана.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                reader.Close();   // закрываем OleDbDataReader
            }
        }



        // Чтение из БД параметров датчика по заданным ТИПУ и МОДЕЛИ
        private void SetSensorsData(string type, string model)
        {
            // текст запроса
            string query = "SELECT * FROM TSensors WHERE Type = '" + type + "' AND Model = '" + model + "'";

            // создаем объект OleDbCommand для выполнения запроса к БД MS Access
            command = new OleDbCommand(query, _сonnection);
            try
            {
                // получаем объект OleDbDataReader для чтения табличного результата запроса SELECT
                reader = command.ExecuteReader();
                reader.Read();

                sensParam.Type = type;
                sensParam.Model = model;

                sensParam.Pmin = reader[3].ToString();
                dgwSensParam.Rows[0].Cells[1].Value = reader[3].ToString();
                sensParam.Pmax = reader[4].ToString();
                dgwSensParam.Rows[1].Cells[1].Value = reader[4].ToString();
                sensParam.DeltaRange = reader[12].ToString();
                dgwSensParam.Rows[2].Cells[1].Value = reader[12].ToString();
                sensParam.NumOfRange = reader[5].ToString();
                dgwSensParam.Rows[3].Cells[1].Value = reader[5].ToString();
                sensParam.Gain1 = reader[6].ToString();
                dgwSensParam.Rows[4].Cells[1].Value = reader[6].ToString();
                sensParam.Range1Pmin = reader[8].ToString();
                dgwSensParam.Rows[5].Cells[1].Value = reader[8].ToString();
                sensParam.Range1Pmax = reader[9].ToString();
                dgwSensParam.Rows[6].Cells[1].Value = reader[9].ToString();
                sensParam.Gain2 = reader[7].ToString();
                dgwSensParam.Rows[7].Cells[1].Value = reader[7].ToString();
                sensParam.Range2Pmin = reader[10].ToString();
                dgwSensParam.Rows[8].Cells[1].Value = reader[10].ToString();
                sensParam.Range2Pmax = reader[11].ToString();
                dgwSensParam.Rows[9].Cells[1].Value = reader[11].ToString();
            }

            catch
            {
                MessageBox.Show("Не удалось прочитать параметры датчиков из БД. Возможно файл с БД пуст или не соответствует требуемой структуре", "Чтение данных из БД. Операция прервана.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            finally
            {
                // закрываем OleDbDataReader
                reader.Close();
            }
        }





        //**************************** РАБОТА С КОММУТАТОРОМ ****************************************

        // Обработчик нажать на кнопку подулючить коммутатор    
        private void bCommutator_Click(object sender, EventArgs e)
        {
            FormPortSettings newForm = new FormPortSettings();
            newForm.InitPortsettings(Properties.Settings.Default.COMname,
                    Properties.Settings.Default.COMspeed,
                    Properties.Settings.Default.COMdataBits,
                    Properties.Settings.Default.COMstopBits,
                    Properties.Settings.Default.COMparity);


            if (newForm.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.COMname = newForm.GetPortName();
                Properties.Settings.Default.COMspeed = newForm.GetPortSpeed();
                Properties.Settings.Default.COMdataBits = newForm.GetPortDataBits();
                Properties.Settings.Default.COMstopBits = newForm.GetPortStopBits();
                Properties.Settings.Default.COMparity = newForm.GetPortParity();
                Properties.Settings.Default.Save();  // Сохраняем переменные.
            }
            // Подключение коммутатора
            if (Commutator.Connect(Properties.Settings.Default.COMname,
               Properties.Settings.Default.COMspeed,
               Properties.Settings.Default.COMdataBits,
               Properties.Settings.Default.COMstopBits,
               Properties.Settings.Default.COMparity, 1) >= 0)
            {
                bCommutator.BackColor = Color.ForestGreen;
                bCommutator.Text = "Коммутатор: ПОДКЛЮЧЕН";
            }
            else
            {
                bCommutator.BackColor = Color.IndianRed;
                bCommutator.Text = "Коммутатор: НЕ ПОДКЛЮЧЕН";
            }
        }



        // Установка соединения с коммутатором
        private void SetCOMconnect()
        {
            // Подключение коммутатора
            if (Commutator.Connect(Properties.Settings.Default.COMname,
               Properties.Settings.Default.COMspeed,
               Properties.Settings.Default.COMdataBits,
               Properties.Settings.Default.COMstopBits,
               Properties.Settings.Default.COMparity, 1) >= 0)
            {
                bCommutator.BackColor = Color.ForestGreen;
                bCommutator.Text = "Коммутатор: ПОДКЛЮЧЕН";
            }
            else
            {
                bCommutator.BackColor = Color.IndianRed;
                bCommutator.Text = "Коммутатор: НЕ ПОДКЛЮЧЕН";
            }

        }





        //**************************** РАБОТА С ДАТЧИКАМИ ****************************************
        // Установка соединения с датчиками
        private void SetSensorsConnect()
        {
            // Подключение датчиков
            if (sensors.Connect(Properties.Settings.Default.COMSensor,
                   Properties.Settings.Default.COMSensor_Speed,
                   Properties.Settings.Default.COMSensor_DataBits,
                   Properties.Settings.Default.COMSensor_StopBits,
                   Properties.Settings.Default.COMSensor_Parity) >= 0)
            {
                bSensors.BackColor = Color.Green;
                bSensors.Text = "Датчики: ПОДКЛЮЧЕНЫ";
            }

            else
            {
                bSensors.BackColor = Color.IndianRed;
                bSensors.Text = "Датчики: НЕ ПОДКЛЮЧЕНЫ";
            }

        }


        //Подключение датчиков
        private void bSensors_Click(object sender, EventArgs e)
        {
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

            if (sensors.Connect(Properties.Settings.Default.COMSensor,
                  Properties.Settings.Default.COMSensor_Speed,
                  Properties.Settings.Default.COMSensor_DataBits,
                  Properties.Settings.Default.COMSensor_StopBits,
                  Properties.Settings.Default.COMSensor_Parity) >= 0)
            {
                bSensors.BackColor = Color.Green;
                bSensors.Text = "Датчики: ПОДКЛЮЧЕНЫ";
            }

            else
            {
                bSensors.BackColor = Color.IndianRed;
                bSensors.Text = "Датчики: НЕ ПОДКЛЮЧЕНЫ";
            }

        }







        //**************************** РАБОТА С ФОРМОЙ ****************************************

        // Первоначальное заполенеие dgwMainWindow
        void SetdgwMainWindow()
        {
            // поле 1
            // Заполняем DataGrid - dgwMainWindow
            dgwMainWindow.Rows.Clear();
            for (int i = 0; i < 30; i++)
            {
                dgwMainWindow.Rows.Add();
                dgwMainWindow.Rows[i].Cells[0].Value = Convert.ToString(i + 1);
            }
            dgwMainWindow.ClearSelection();

            // поле 2
            // Заполняем DataGrid - dgwMainWindow2
            dgwMainWindow2.Rows.Clear();
            for (int i = 0; i < 30; i++)
            {
                dgwMainWindow2.Rows.Add();
                dgwMainWindow2.Rows[i].Cells[0].Value = Convert.ToString(i + 31);
            }
            dgwMainWindow2.ClearSelection();
            
            // Получаем список типов и датчиков из БД
            GetTypeDB();
        }
        
                

        // Обработчик события при выборе ТИПА датчика (в combobox)
        // по выбранному типу датчика, получаем список моделей и заполняем столбец с ТИПОМ датчика
        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbModel.Items.Clear();
            string SelectType = cbType.Text;
            // текст запроса              
            string query = "SELECT DISTINCT Model FROM TSensors WHERE Type = '" + SelectType + "'";
            // создаем объект OleDbCommand для выполнения запроса к БД MS Access
            OleDbCommand command = new OleDbCommand(query, _сonnection);
            try
            { // получаем объект OleDbDataReader для чтения табличного результата запроса SELECT
                reader = command.ExecuteReader();
                // в цикле построчно читаем ответ от БД
                while (reader.Read())
                {
                    cbModel.Items.Add(reader[0].ToString());
                }
            }
            catch
            {
                MessageBox.Show("Не удалось прочитать модели датчиков из БД. Возможно файл с БД пуст или не соответствует требуемой структуре", "Сообщение об ошибке. Операция прервана.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                reader.Close();   
                FilldgwMainWindow(2, SelectType);  // заполняем столбец ТИП до конца таблицы
                FilldgwMainWindow(3, null);        // обнуляем столбец МОДЕЛЬ до конца таблицы                   
                FilldwgSensParam();

            }           
        }
        


        // Обработчик события при выборе МОДЕЛИ датчика
        // Заполняем столбец МОДЕЛЬ - выбранной из combobox моделью
        private void cbModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SelectType = cbType.Text;
            string SelectModel = cbModel.Text;
            FilldgwMainWindow(3, SelectModel);
            FilldwgSensParam();
            SetSensorsData(SelectType, SelectModel);
        }




        // Функция - Автозаполнение dgwMainWindow при выборе в combobox ТИПА и МОДЕЛИ датчика
        void FilldgwMainWindow(int NumId, string Data)
        {
            for (int i = 0; i < 30; i++)
            {
                if (Convert.ToBoolean(dgwMainWindow.Rows[i].Cells[1].Value) == true)
                    dgwMainWindow.Rows[i].Cells[NumId].Value = Convert.ToString(Data);
            }
            dgwMainWindow.ClearSelection();

            for (int i = 0; i < 30; i++)
            {
                if (Convert.ToBoolean(dgwMainWindow2.Rows[i].Cells[1].Value) == true)
                    dgwMainWindow2.Rows[i].Cells[NumId].Value = Convert.ToString(Data);
            }
            dgwMainWindow2.ClearSelection();




        }



        // Обработчик события завершения редактирования ячейки  dgwMainWindow
        private void dgwMainWindow_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            /* ИСКЛЮЧЕНО серийные номера читаются с датчиков как заводские
            // редактирование СЕРИЙНОГО НОМЕРА
            if ((e.ColumnIndex == 4)&&(cbAutoNumSerial.Checked))
            {
                int i;
                // Проверка введенного значения на то что это число
                try
                {
                    Int64 serial = Convert.ToInt64(dgwMainWindow.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    if (serial != 0)  // если задали сер.номер в первой ячейки прописываем его инкрементацией во все остальные строки
                    {
                        for (i = e.RowIndex; i < 30; i++)
                        {
                            if (Convert.ToBoolean(dgwMainWindow.Rows[i].Cells[1].Value) == true)
                            {
                                //dgwMainWindow.Rows[i].Cells[4].Value = Convert.ToString(serial++);
                                dgwMainWindow.Rows[i].Cells[4].Value = (serial++).ToString("0000000");
                            }
                        }
                        dgwMainWindow.ClearSelection();
                    }
                }
                catch
                {
                    MessageBox.Show("Данные в этой ячейке не являются числом.", "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dgwMainWindow.Rows[e.RowIndex].Cells[4].Value = null;
                }          
            }
            */
        }



        // Разрешаем/Запрещаем редактирование серийного номера датчика - при установке checkBox - выбор канала коммутатора 
        private void dgwMainWindow_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {           
           /* ИСКЛЮЧЕНО
            
            // редактирование НОМЕРА КАНАЛА
            if ((e.ColumnIndex == 1) && (e.RowIndex >= 0))  //&& (e.ColumnIndex >= 0)
            {
                if (Convert.ToBoolean(dgwMainWindow.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) == true)
                {
                    dgwMainWindow.Rows[e.RowIndex].Cells[4].ReadOnly = false;
                    dgwMainWindow.Rows[e.RowIndex].Cells[2].Value = cbType.SelectedItem;
                    dgwMainWindow.Rows[e.RowIndex].Cells[3].Value = cbModel.SelectedItem;
                    
                   /*
                    // подключаем соответствующий канал коммутатора
                    Commutator.SetConnectors(e.RowIndex, 0);

                    if (sensors.SeachSensor(e.RowIndex))//поиск датчиков по HART
                    {
                        dgwMainWindow.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Green;
                    }
                    else
                    {
                        dgwMainWindow.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.IndianRed;
                    }
                    */
       /*         }
                else
                {
                    dgwMainWindow.Rows[e.RowIndex].Cells[4].Value = null;
                    dgwMainWindow.Rows[e.RowIndex].Cells[2].Value = null;
                    dgwMainWindow.Rows[e.RowIndex].Cells[3].Value = null;

                    dgwMainWindow.Rows[e.RowIndex].Cells[4].ReadOnly = true;
                    dgwMainWindow.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
            }           
*/
        }
        
              



        private void FilldwgSensParam()
        {
            // Заполняем DataGrid - dgwMainWindow
            dgwSensParam.Rows.Clear();
            dgwSensParam.Rows.Add(); dgwSensParam.Rows[0].Cells[0].Value = Convert.ToString("Минимальный нижний предел датчика, кПа");
            dgwSensParam.Rows.Add(); dgwSensParam.Rows[1].Cells[0].Value = Convert.ToString("Максимальный верхний предел датчика, кПа");

            dgwSensParam.Rows.Add(); dgwSensParam.Rows[2].Cells[0].Value = Convert.ToString("Минимально допустимый диапазон настройки датчика");
            dgwSensParam.Rows.Add(); dgwSensParam.Rows[3].Cells[0].Value = Convert.ToString("Количество диапазонов характеризации");

            dgwSensParam.Rows.Add(); dgwSensParam.Rows[4].Cells[0].Value = Convert.ToString("Коэффициент усиления 1-го диапазона");
            dgwSensParam.Rows.Add(); dgwSensParam.Rows[5].Cells[0].Value = Convert.ToString("Нижний предел измерения для 1-го диапазона, кПа");
            dgwSensParam.Rows.Add(); dgwSensParam.Rows[6].Cells[0].Value = Convert.ToString("Верхний предел измерения для 1-го диапазона, кПа");

            dgwSensParam.Rows.Add(); dgwSensParam.Rows[7].Cells[0].Value = Convert.ToString("Коэффициент усиления 2-го диапазона");
            dgwSensParam.Rows.Add(); dgwSensParam.Rows[8].Cells[0].Value = Convert.ToString("Нижний предел измерения для 2-го диапазона, кПа");
            dgwSensParam.Rows.Add(); dgwSensParam.Rows[9].Cells[0].Value = Convert.ToString("Верхний предел измерения для 2-го диапазона, кПа");

            dgwSensParam.ClearSelection();      
        }
        


        // Снимаем выделение с dgwMainWindow
        private void dgwMainWindow_SelectionChanged(object sender, EventArgs e)
        {
            dgwMainWindow.ClearSelection();
        }

        private void dgwMainWindow2_SelectionChanged(object sender, EventArgs e)
        {
            dgwMainWindow2.ClearSelection();
        }



        // Поле1
        private void dgwMainWindow_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if ((System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Control) && (Convert.ToBoolean(dgwMainWindow.Rows[e.RowIndex].Cells[1].Value) == false))
                {
                    for (int i = e.RowIndex; i >= 0; i--)
                    {
                        if (Convert.ToBoolean(dgwMainWindow.Rows[i].Cells[1].Value) == true)
                        {
                            break;
                        }
                        else
                        {
                            dgwMainWindow.Rows[i].Cells[1].Value = true;
                            //dgwMainWindow.Rows[i].Cells[4].ReadOnly = false;
                            dgwMainWindow.Rows[i].Cells[2].Value = cbType.SelectedItem;
                            dgwMainWindow.Rows[i].Cells[3].Value = cbModel.SelectedItem;

                        }
                    }
                }
                else if ((System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Control) && (Convert.ToBoolean(dgwMainWindow.Rows[e.RowIndex].Cells[1].Value) == true))
                {
                    for (int i = e.RowIndex; i >= 0; i--)
                    {
                        if (Convert.ToBoolean(dgwMainWindow.Rows[i].Cells[1].Value) == false)
                        {
                            break;
                        }
                        else
                        {
                            dgwMainWindow.Rows[i].Cells[1].Value = false;
                            //dgwMainWindow.Rows[i].Cells[4].ReadOnly = true;
                            dgwMainWindow.Rows[i].Cells[4].Value = null;
                            dgwMainWindow.Rows[i].Cells[2].Value = null;
                            dgwMainWindow.Rows[i].Cells[3].Value = null;

                        }
                    }
                }

                if (System.Windows.Forms.Control.ModifierKeys != System.Windows.Forms.Keys.Control)
                {

                    if (Convert.ToBoolean(dgwMainWindow.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) == false)
                    {
                        dgwMainWindow.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
                       // dgwMainWindow.Rows[e.RowIndex].Cells[4].ReadOnly = false;
                        dgwMainWindow.Rows[e.RowIndex].Cells[2].Value = cbType.SelectedItem;
                        dgwMainWindow.Rows[e.RowIndex].Cells[3].Value = cbModel.SelectedItem;

                    }
                    else
                    {
                        dgwMainWindow.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;

                        dgwMainWindow.Rows[e.RowIndex].Cells[4].Value = null;
                        dgwMainWindow.Rows[e.RowIndex].Cells[2].Value = null;
                        dgwMainWindow.Rows[e.RowIndex].Cells[3].Value = null;
                        //dgwMainWindow.Rows[e.RowIndex].Cells[4].ReadOnly = true;
                        dgwMainWindow.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
        }



        // Поле2
        private void dgwMainWindow2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if ((System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Control) && (Convert.ToBoolean(dgwMainWindow2.Rows[e.RowIndex].Cells[1].Value) == false))
                {
                    for (int i = e.RowIndex; i >= 0; i--)
                    {
                        if (Convert.ToBoolean(dgwMainWindow2.Rows[i].Cells[1].Value) == true)
                        {
                            break;
                        }
                        else
                        {
                            dgwMainWindow2.Rows[i].Cells[1].Value = true;
                            //dgwMainWindow2.Rows[i].Cells[4].ReadOnly = false;
                            dgwMainWindow2.Rows[i].Cells[2].Value = cbType.SelectedItem;
                            dgwMainWindow2.Rows[i].Cells[3].Value = cbModel.SelectedItem;

                        }
                    }
                }
                else if ((System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Control) && (Convert.ToBoolean(dgwMainWindow2.Rows[e.RowIndex].Cells[1].Value) == true))
                {
                    for (int i = e.RowIndex; i >= 0; i--)
                    {
                        if (Convert.ToBoolean(dgwMainWindow2.Rows[i].Cells[1].Value) == false)
                        {
                            break;
                        }
                        else
                        {
                            dgwMainWindow2.Rows[i].Cells[1].Value = false;
                            //dgwMainWindow2.Rows[i].Cells[4].ReadOnly = true;
                            dgwMainWindow2.Rows[i].Cells[4].Value = null;
                            dgwMainWindow2.Rows[i].Cells[2].Value = null;
                            dgwMainWindow2.Rows[i].Cells[3].Value = null;

                        }
                    }
                }

                if (System.Windows.Forms.Control.ModifierKeys != System.Windows.Forms.Keys.Control)
                {

                    if (Convert.ToBoolean(dgwMainWindow2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) == false)
                    {
                        dgwMainWindow2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
                        //dgwMainWindow2.Rows[e.RowIndex].Cells[4].ReadOnly = false;
                        dgwMainWindow2.Rows[e.RowIndex].Cells[2].Value = cbType.SelectedItem;
                        dgwMainWindow2.Rows[e.RowIndex].Cells[3].Value = cbModel.SelectedItem;

                    }
                    else
                    {
                        dgwMainWindow2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;

                        dgwMainWindow2.Rows[e.RowIndex].Cells[4].Value = null;
                        dgwMainWindow2.Rows[e.RowIndex].Cells[2].Value = null;
                        dgwMainWindow2.Rows[e.RowIndex].Cells[3].Value = null;
                        //dgwMainWindow2.Rows[e.RowIndex].Cells[4].ReadOnly = true;
                        dgwMainWindow2.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
        }





        //********************************* ПРОШИТЬ ДАТЧИКИ *******************************************

        // обработчик нажатия на кнопку ПРОШИТЬ ДАТЧИКИ
        private void bBurn_Click(object sender, EventArgs e)
        {
            bool checkCH = false;
            bool checkSerial = true;
            int res;

            label2.Visible = true;
            label2.Text = "";


            // Проверка готовности к прошивке
            // проверка связи с коммутатором
            if (Commutator.Connected != true)
            {
                label2.Visible = true;
                label2.Text = "Операция не может быть выполнена. Нет связи с коммутатором!";
                return;
            }
            // проверка связи с БД
            if (_сonnection.State != System.Data.ConnectionState.Open)
            {
                label2.Visible = true;
                label2.Text = "Операция не может быть выполнена. Нет связи Базой данных!";
                return;
            }
            // проверка выбран ли тип датчика
            if (cbType.SelectedIndex < 0)
            {
                label2.Visible = true;
                label2.Text = "Операция не может быть выполнена. Не задан тип датчика!";
                return;
            }
            // проверка выбрана ли модель датчика
            if (cbModel.SelectedIndex < 0)
            {
                label2.Visible = true;
                label2.Text = "Операция не может быть выполнена. Не указана модель датчика!";
                return;
            }
            // Заданы ли каналы коммутатора           
            for (int i = 0; i < 30; i++)
            {
                if(Convert.ToBoolean(dgwMainWindow.Rows[i].Cells[1].Value) == true)
                {
                    checkCH = true;                 
                    if (Convert.ToInt64(dgwMainWindow.Rows[i].Cells[4].Value) <= 0)
                    {
                        checkSerial = false;
                    }
                }
            }
            for (int i = 0; i < 30; i++)
            {
                if (Convert.ToBoolean(dgwMainWindow2.Rows[i].Cells[1].Value) == true)
                {
                    checkCH = true;
                    if (Convert.ToInt64(dgwMainWindow2.Rows[i].Cells[4].Value) <= 0)
                    {
                        checkSerial = false;
                    }
                }
            }

            if (checkCH == false)
            {
                label2.Visible = true;
                label2.Text = "Операция не может быть выполнена. Не выбраны каналы коммутатора!";
                return;
            }
            if (checkSerial == false)
            {
                label2.Visible = true;
                label2.Text = "Операция не может быть выполнена. Не задан серийный номер!";
                return;
            }

            
            // Если все проверки пройденны         
                        
            // Запрос на подтверждение
            DialogResult result = MessageBox.Show(
            "Начать запись индивидуальных параметров в датчики?",
            "Подтверждение...",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button1,
            MessageBoxOptions.DefaultDesktopOnly);

            if (result == DialogResult.No)
            {
                return;
            }


            Application.OpenForms[0].Activate(); // восстанавливаем фокус главного окна
            dgwMainWindow.Enabled = false;       // делаем недоступным для редактирования главное окно поле1
            dgwMainWindow2.Enabled = false;       // делаем недоступным для редактирования главное окно поле2

            // Настраиваем прогрессБар
            int progressMax = 0;            
            for (int i = 0; i < 30; i++)
            {
                if (Convert.ToBoolean(dgwMainWindow.Rows[i].Cells[1].Value) == true)
                    progressMax++;
            }
            for (int i = 0; i < 30; i++)
            {
                if (Convert.ToBoolean(dgwMainWindow2.Rows[i].Cells[1].Value) == true)
                    progressMax++;
            }
            
            progressBar.Minimum = 0;
            progressBar.Maximum = progressMax;       
            progressBar.Value = 0;
            progressBar.Step = 1;
            Boolean resBurn = true;
            //progressBar.PerformStep();

          
            // Опрос подключенных к коммутатору датчиков и запись индивидуальных параметров
            for (int i = 0; i < 60; i++)
            {
                label2.Text = "Выполняется запись индивидуальных параметров...";

                if (i < 30)
                {

                    if (Convert.ToBoolean(dgwMainWindow.Rows[i].Cells[1].Value) == true)
                    {
                        // подключаем соответствующий канал коммутатора
                        Commutator.SetConnectors(i, 0);

                        string Serial = dgwMainWindow.Rows[i].Cells[4].Value.ToString();

                        if (sensors.SeachSensor(i))//поиск датчиков по HART
                        {
                            if (sensors.SelectSensor(i))//выбор обнаруженного датчика
                            {//датчик найден, обновляем таблицу

                                labelCH.Text = "Запись данных в датчик в канале " + Convert.ToString(i + 1);
                                BurnSensors(Serial, sensParam.Pmin, sensParam.Pmax, sensParam.DeltaRange, sensParam.Model);
                                res = BurnSensors(Serial, sensParam.Pmin, sensParam.Pmax, sensParam.DeltaRange, sensParam.Model);
                                switch (res)
                                {
                                    case 1:
                                        dgwMainWindow.Rows[i].DefaultCellStyle.BackColor = Color.Green;

                                        if (sensors.C14SensorRead())       //чтение данных с датчика
                                        {
                                            if (Convert.ToUInt32(Serial) != sensors.sensor.SerialNumber)
                                            {
                                                resBurn = false;
                                                label2.Text = "Не удалось записать серийный номер датчика в канале: " + (i + 1);
                                                tbResult.Text = "Канал: " + (i + 1) + " - Не удалось записать серийный номер";
                                                dgwMainWindow.Rows[i].DefaultCellStyle.BackColor = Color.IndianRed;
                                            }
                                        }
                                        if (!sensors.C140ReadPressureModel())
                                        {//читаем модель ПД
                                            //MessageBox.Show("Модель ПД датчика не считана", "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            label2.Text = "Не удалось считать модель датчика в канале: " + (i + 1);
                                            tbResult.Text = "Канал: " + (i + 1) + " - Модель ПД датчика не считана";
                                            dgwMainWindow.Rows[i].DefaultCellStyle.BackColor = Color.IndianRed;
                                        }
                                        else
                                        {
                                            for (int j = 0; j < sensParam.Model.Length; j++)
                                            {
                                                if (sensParam.Model[j] != sensors.sensor.PressureModel[j])
                                                {
                                                    //MessageBox.Show("Модель ПД датчика не записана", "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    label2.Text = "Не удалось записать модель датчика в канале: " + (i + 1);
                                                    tbResult.Text = "Канал: " + (i + 1) + " - Не удалось записать модель датчика";
                                                    dgwMainWindow.Rows[i].DefaultCellStyle.BackColor = Color.IndianRed;
                                                    //break;
                                                }

                                            }
                                        }
                                        if (dgwMainWindow.Rows[i].DefaultCellStyle.BackColor == Color.Green)
                                        {
                                            label2.Text = "Параметры успешно записаны в датчик в канале: " + (i + 1);
                                            tbResult.Text = "Канал: " + (i + 1) + " - Данные успешно записаны в датчик";
                                        }
                                       

                                        break;
                                    case 0:
                                        label2.Text = "Не удалось записать параметры. Нет подключения к датчику в канале " + (i + 1);
                                        tbResult.Text = "Канал: " + (i + 1) + " - Нет подключения к датчику";
                                        dgwMainWindow.Rows[i].DefaultCellStyle.BackColor = Color.IndianRed;
                                        //MessageBox.Show("Не удалось записать параметры. Нет подключения к датчику в канале " + (i + 1), "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);                                      
                                        resBurn = false;
                                        break;
                                    case -1:
                                    case -2:
                                    case -3:
                                    case -4:
                                        label2.Text = "Команды записи в датчик не выполнены: " + res;
                                        tbResult.Text = "Канал: " + (i + 1) + " - Команды записи в датчик не выполнены";
                                        dgwMainWindow.Rows[i].DefaultCellStyle.BackColor = Color.IndianRed;
                                        //MessageBox.Show("Команды записи в датчик не выполнены в канале: " + (i + 1), "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        resBurn = false;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            label2.Text = "Датчик не обнаружен в канале: " + (i + 1);
                            MessageBox.Show("Датчик не обнаружен в канале: " + (i + 1), "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            resBurn = false;
                        }
                        progressBar.PerformStep();
                    }

                }
                else
                {

                    if (Convert.ToBoolean(dgwMainWindow2.Rows[i-30].Cells[1].Value) == true)
                    {
                        // подключаем соответствующий канал коммутатора
                        Commutator.SetConnectors(i, 0);

                        string Serial = dgwMainWindow2.Rows[i-30].Cells[4].Value.ToString();

                        if (sensors.SeachSensor(i))//поиск датчиков по HART
                        {
                            if (sensors.SelectSensor(i))//выбор обнаруженного датчика
                            {//датчик найден, обновляем таблицу

                                labelCH.Text = "Запись данных в датчик в канале " + Convert.ToString(i + 1);
                                BurnSensors(Serial, sensParam.Pmin, sensParam.Pmax, sensParam.DeltaRange, sensParam.Model);
                                res = BurnSensors(Serial, sensParam.Pmin, sensParam.Pmax, sensParam.DeltaRange, sensParam.Model);
                                switch (res)
                                {
                                    case 1:

                                        dgwMainWindow.Rows[i].DefaultCellStyle.BackColor = Color.Green;

                                        if (sensors.C14SensorRead())       //чтение данных с датчика
                                        {
                                            if (Convert.ToUInt32(Serial) != sensors.sensor.SerialNumber)
                                            {
                                                resBurn = false;
                                                label2.Text = "Не удалось записать серийный номер датчика в канале: " + (i + 1);
                                                tbResult.Text = "Канал: " + (i + 1) + " - Не удалось записать серийный номер";
                                                dgwMainWindow.Rows[i].DefaultCellStyle.BackColor = Color.IndianRed;                                             
                                            }
                                        }
                                        if (!sensors.C140ReadPressureModel())
                                        {//читаем модель ПД
                                         //MessageBox.Show("Модель ПД датчика не считана", "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            label2.Text = "Не удалось считать модель датчика в канале: " + (i + 1);
                                            tbResult.Text = "Канал: " + (i + 1) + " - Модель ПД датчика не считана";
                                            dgwMainWindow.Rows[i].DefaultCellStyle.BackColor = Color.IndianRed;
                                        }
                                        else
                                        {
                                            for (int j = 0; j < sensParam.Model.Length; j++)
                                            {
                                                if (sensParam.Model[j] != sensors.sensor.PressureModel[j])
                                                {
                                                    label2.Text = "Не удалось записать модель датчика в канале: " + (i + 1);
                                                    tbResult.Text = "Канал: " + (i + 1) + " - Не удалось записать модель датчика";
                                                    dgwMainWindow.Rows[i].DefaultCellStyle.BackColor = Color.IndianRed;
                                                    //MessageBox.Show("Модель ПД датчика не записана", "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    //break;
                                                }

                                            }
                                        }
                                        if (dgwMainWindow.Rows[i].DefaultCellStyle.BackColor == Color.Green)
                                        {
                                            label2.Text = "Параметры успешно записаны в датчик в канале: " + (i + 1);
                                            tbResult.Text = "Канал: " + (i + 1) + " - Данные успешно записаны в датчик";
                                        }

                                        break;
                                    case 0:
                                        label2.Text = "Не удалось записать параметры. Нет подключения к датчику в канале " + (i + 1);
                                        tbResult.Text = "Канал: " + (i + 1) + " - Нет подключения к датчику";
                                        dgwMainWindow.Rows[i].DefaultCellStyle.BackColor = Color.IndianRed;
                                        //MessageBox.Show("Не удалось записать параметры. Нет подключения к датчику в канале " + (i + 1), "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        resBurn = false;
                                        break;
                                    case -1:
                                    case -2:
                                    case -3:
                                    case -4:
                                        label2.Text = "Команды записи в датчик не выполнены: " + res;
                                        tbResult.Text = "Канал: " + (i + 1) + " - Команды записи в датчик не выполнены";
                                        dgwMainWindow.Rows[i].DefaultCellStyle.BackColor = Color.IndianRed;
                                        //MessageBox.Show("Команды записи в датчик не выполнены в канале: " + (i + 1), "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        resBurn = false;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            label2.Text = "Датчик не обнаружен в канале: " + (i + 1);
                            tbResult.Text = "Канал: " + (i + 1) + " - Датчик не обнаружен";
                            dgwMainWindow.Rows[i].DefaultCellStyle.BackColor = Color.IndianRed;
                            //MessageBox.Show("Датчик не обнаружен в канале: " + (i + 1), "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            resBurn = false;
                        }
                        progressBar.PerformStep();
                    }


                }                               
            }
            dgwMainWindow.Enabled = true;
            dgwMainWindow2.Enabled = true;
            labelCH.Text = "";



            if (resBurn == true)
            {
                label2.Text = "Индивидуальные параметры успешно записаны!";
                tbResult.Text = "Индивидуальные параметры успешно записаны!";
                MessageBox.Show("Индивидуальные параметры успешно записаны!", "Сообщение о завершении", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            else
            {
                label2.Text = "Запись индивидуальных параметров завершена!";
                tbResult.Text = "Запись индивидуальных параметров завершена!";
                MessageBox.Show("Запись индивидуальных параметров завершена!", "Сообщение о завершении", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                progressBar.Value = 0;
                label2.Text = "";
            }           
        }



        // Функция записи параметров в датчик
        int BurnSensors(string Serial, string Pmin, string Pmax, string DeltaRangeMin, string Model)
        {
            int result=1;
            try
            {
                if ((sensors != null) && (sensors.IsConnect()))
                {
                    sensors.EnterServis();
                    //sensors.TegRead();          //читаем информацию о датчике
                    if (!sensors.C14SensorRead())       //чтение данных с датчика
                        MessageBox.Show("Датчик не прочитан: ", "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (!sensors.C140ReadPressureModel())//читаем модель ПД
                        MessageBox.Show("Модель ПД датчика не считана", "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                    Thread.Sleep(500);
                    sensors.ParseReadBuffer(500);//ждем завершения операций по датчику в потоке

                    sensors.sensor.SerialNumber = Convert.ToUInt32(Serial);
                    sensors.sensor.DownLevel = Convert.ToSingle(Pmin);
                    sensors.sensor.UpLevel = Convert.ToSingle(Pmax);
                    sensors.sensor.MinLevel = Convert.ToSingle(DeltaRangeMin);

                    char[] str = Model.ToCharArray();
                    for (int i = 0; i < sensors.sensor.PressureModel.Length; i++)
                    {
                        if (str.Length > i)
                        {
                            sensors.sensor.PressureModel[i] = str[i];
                        }
                        else
                        {
                            sensors.sensor.PressureModel[i] = ' ';
                        }
                    }
                    if (!sensors.EnterServis())
                        result = -1;
                    Thread.Sleep(100);
                    if (!sensors.WriteSerialNumberC49())
                        result = -2;
                    Thread.Sleep(100);
                    if (!sensors.UpDownWriteC249())
                        result = -3;
                    Thread.Sleep(100);
                    if (!sensors.C241WritePressureModel())
                        result = -4;
                    return result;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }

        }



        // Поиск датчиков в выделенных каналах
        private void bRead_Click(object sender, EventArgs e)
        {
            bool flag_select_CH = false;

            if ((Commutator.Connected == true) && (sensors.IsConnect()))
            {
                
                label2.Text = "Выполняется чтение данных с датчиков...";

                // Настраиваем прогрессБар
                int progressMax = 0;
                for (int i = 0; i < 30; i++)
                {
                    if (Convert.ToBoolean(dgwMainWindow.Rows[i].Cells[1].Value) == true)
                        progressMax++;
                }
                for (int i = 0; i < 30; i++)
                {
                    if (Convert.ToBoolean(dgwMainWindow2.Rows[i].Cells[1].Value) == true)
                        progressMax++;
                }

                progressBar.Minimum = 0;
                progressBar.Maximum = progressMax;
                progressBar.Value = 0;
                progressBar.Step = 1;
                //progressBar.PerformStep();


                for (int i = 0; i < 60; i++)
                {

                    if (i < 30)
                    {
                        if (Convert.ToBoolean(dgwMainWindow.Rows[i].Cells[1].Value) == true)
                        {
                            flag_select_CH = true;

                            try
                            {
                                // подключаем соответствующий канал коммутатора
                                Commutator.SetConnectors(i, 0);
                                labelCH.Text = "Чтение данных датчика в канале " + Convert.ToString(i + 1);                                


                                if (sensors.SeachSensor(i))//поиск датчиков по HART
                                {                                    
                                    sensors.SelectSensor(i);
                                    dgwMainWindow.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                                    dgwMainWindow.Rows[i].Cells[4].Value = sensors.sensor.uni;
                                }
                                else
                                {
                                    dgwMainWindow.Rows[i].DefaultCellStyle.BackColor = Color.IndianRed;
                                    dgwMainWindow.Rows[i].Cells[1].Value = false;
                                }
                                progressBar.PerformStep();
                            }

                            catch
                            {
                                MessageBox.Show("Не удалось связаться с датчиками: ", "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                progressBar.Value = 0;
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToBoolean(dgwMainWindow2.Rows[i-30].Cells[1].Value) == true)
                        {
                            flag_select_CH = true;

                            try
                            {
                                // подключаем соответствующий канал коммутатора
                                Commutator.SetConnectors(i, 0);
                                labelCH.Text = "Чтение данных датчика в канале " + Convert.ToString(i + 1);

                                if (sensors.SeachSensor(i))//поиск датчиков по HART
                                {
                                    sensors.SelectSensor(i);
                                    dgwMainWindow2.Rows[i-30].DefaultCellStyle.BackColor = Color.Green;
                                    dgwMainWindow2.Rows[i-30].Cells[4].Value = sensors.sensor.uni;
                                }
                                else
                                {
                                    dgwMainWindow2.Rows[i-30].DefaultCellStyle.BackColor = Color.IndianRed;
                                    dgwMainWindow2.Rows[i-30].Cells[1].Value = false;
                                }
                                progressBar.PerformStep();
                            }

                            catch
                            {
                                MessageBox.Show("Не удалось связаться с датчиками: ", "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                progressBar.Value = 0;
                                break;
                            }
                        }
                    }                                                           
                   
                }
                if (!flag_select_CH)
                {
                    //label2.Text = "Не выбран ни один канал";
                    MessageBox.Show("Не выбран ни один канал", "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                //label2.Text = "Не подключен коммутатор или нет связи с датчиками";
                MessageBox.Show("Не подключен коммутатор или нет связи с датчиками", "Сообщение об ошибке", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            labelCH.Text = "";
            label2.Text = "";
        }









        // При закрытии формы отключаем соединения с БД, коммутатором и датчиками
        private void FormSensorProgrammer_FormClosed(object sender, FormClosedEventArgs e)
        {           
            if (Commutator.Connected == true)
            {
                Commutator.DisConnect();
            }

            if (_сonnection.State == System.Data.ConnectionState.Open)
            {
                _сonnection.Close();
            }

            if (sensors.IsConnect())
            {
                sensors.DisConnect();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetdgwMainWindow(); // первоначальное заполенение dgwMainWindow
            FilldwgSensParam(); // отображение окна с параметрами датчиков
        }
    }      
}
