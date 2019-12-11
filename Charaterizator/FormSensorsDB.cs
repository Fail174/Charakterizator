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
//using ENI100;



namespace Charaterizator
{
    public partial class FormSensorsDB : Form
    {
        OleDbDataReader reader;
        OleDbCommand command;
        public OleDbConnection _сonnection;       
        public static string newTypeSens;
        public static string newModelSens;
        public static string SensNameList;
        public ClassEni100 eni100=null;
        public int SelectInd = -1;

        public FormSensorsDB()
        {
            InitializeComponent();               
        }

        
        //-----------------------------------------------------------------------------------------------

        // Функция устанавливает соединение с БД
        public void SetConnectionDB(string strFileNameDB)
        {
            string connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source = " + strFileNameDB + ";";
            // Создаем экземпляр класса OleDBConnection для подключения к БД
            _сonnection = new OleDbConnection(connectString);
            // Открываем подключение к БД
            try
            {
                _сonnection.Open();
                toolStripStatusLabel1.Text = "Соединение с БД установлено...  Файл БД: " + strFileNameDB;
                Program.txtlog.WriteLineLog("Соединение с БД установлено.", 0);
                Program.txtlog.WriteLineLog("Файл БД: " + strFileNameDB, 0);
            }
            catch (OleDbException ex)
            {
                //MessageBox.Show(ex.Message);
                MessageBox.Show(ex.Message, "Открытие файла базы данных. Операция прервана.", MessageBoxButtons.OK, MessageBoxIcon.Warning);               
                toolStripStatusLabel1.Text = "Соединение с БД не установлено... ";
                Program.txtlog.WriteLineLog("Соединение с БД не установлено.", 1);
                Program.txtlog.WriteLineLog("Подключите файл с БД вручную, с помощью меню: Файл - Открыть БД датчиков.", 1);

            }
            finally
            {
                //_сonnection.Close();
            }
        }                
        

        //-----------------------------------------------------------------------------------------------

        // Функция получает список моделей из БД и выводит его в ListView
        public void GetData()
        {
            if (_сonnection.State == System.Data.ConnectionState.Open)
            {
                // текст запроса               
                string query = "SELECT Type, Model FROM TSensors  ORDER BY Type";
                // создаем объект OleDbCommand для выполнения запроса к БД MS Access
                OleDbCommand command = new OleDbCommand(query, _сonnection);

                try
                { // получаем объект OleDbDataReader для чтения табличного результата запроса SELECT
                    reader = command.ExecuteReader();
                    // очищаем listView
                    lvwModels.Items.Clear();

                    //**
                    int i = 0;
                    // в цикле построчно читаем ответ от БД
                    while (reader.Read())
                    {
                        // выводим данные столбцов текущей строки в listView                   
                        lvwModels.Items.Add(reader[0].ToString());
                        //lvwModels.Items.Add(reader[1].ToString());
                        lvwModels.Items[i].SubItems.Add(reader[1].ToString());
                        i++;
                    }
                }

                catch
                {
                    MessageBox.Show("Не удалось прочитать названия и модели датчиков из БД. Возможно файл с БД пуст или не соответствует требуемой структуре", "Чтение данных из БД. Операция прервана.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                finally
                {
                    // закрываем OleDbDataReader
                    reader.Close();
                }
                              
                
                // если список моделей из БД не пуст, позиционируемся на первой записи
                if (lvwModels.Items.Count > 0)
                {                  
                    lvwModels.Focus();
                    lvwModels.Select();
                    lvwModels.Items[0].Focused = true;
                    lvwModels.Items[0].Selected = true;
                }
                else  //в случае удалении последней записи, список ListView будет пуст - очищаем textbox-ы
                {
                    foreach (var gb in this.Controls.OfType<GroupBox>())
                    {
                        foreach (var tb in gb.Controls.OfType<TextBox>())
                        {
                            if ((tb is TextBox) && (tb.Tag != null))
                            {
                                tb.Text = "";
                            }
                        }
                    }
                    MessageBox.Show("База данных пуста!", "Чтение файлов из БД...          ", MessageBoxButtons.OK);
                    return;
                }
            }
        }

        
        //-----------------------------------------------------------------------------------------------

        // Функция обновления параметров датчика по названию модели
        // вх. данные - имя модели
        private void SetSensorsData(string model)
        {
            // текст запроса
            string query = "SELECT * FROM TSensors WHERE Model = " + "'" + model + "'";
            //Тип + Модель string query = "SELECT Type, Model FROM TSensors  ORDER BY Type";

            // создаем объект OleDbCommand для выполнения запроса к БД MS Access
           command = new OleDbCommand(query, _сonnection);


            try
            {
                // получаем объект OleDbDataReader для чтения табличного результата запроса SELECT
                reader = command.ExecuteReader();
                reader.Read();

                int cnt = this.Controls.Count;
                //foreach (TextBox tb in this.Controls.OfType<TextBox>())

                foreach (var gb in this.Controls.OfType<GroupBox>())
                {
                    foreach (var tb in gb.Controls.OfType<NumericUpDown>())
                    {
                        if ((tb is NumericUpDown) && (tb.Tag != null))
                        {
                            tb.Text = reader[Convert.ToInt32(tb.Tag)].ToString();
                        }
                    }
                }

                
                foreach (var gb in this.Controls.OfType<GroupBox>())
                {
                    foreach (var tb in gb.Controls.OfType<TextBox>())
                    {
                        if ((tb is TextBox) && (tb.Tag != null))
                        {
                            tb.Text = reader[Convert.ToInt32(tb.Tag)].ToString();
                        }
                    }
                }                              


                int numofrange = Convert.ToInt32(reader[5].ToString());
                if (numofrange == 1)
                {
                    rbRange1.Checked = true;
                    rbRange2.Checked = false;
                }
                else
                {
                    rbRange1.Checked = false;
                    rbRange2.Checked = true;
                }
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


        //-----------------------------------------------------------------------------------------------
        
        private void rbRange1_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRange1.Checked)
            {                
                Gain2.Enabled = false;
                Range2_Pmin.Enabled = false;
                Range2_Pmax.Enabled = false;               
                HarPressPoint2.Enabled = false;              
                VerPressPoint2.Enabled = false;
            }
            else
            {               
                Gain2.Enabled = true;
                Range2_Pmin.Enabled = true;
                Range2_Pmax.Enabled = true;               
                HarPressPoint2.Enabled = true;
                VerPressPoint2.Enabled = true;
            }
        }


        //-----------------------------------------------------------------------------------------------

        // Подсказка - отображает типовые названия датчиков (из БД) при добавлении нового
        private string FormSensNameList(object sender, EventArgs e)
        {
            string str = null; 
            // получаем названия датчиков из БД и сохраняем их в SensNameList           
            string query = "SELECT DISTINCT Type FROM TSensors ORDER BY Type";
            // создаем объект OleDbCommand для выполнения запроса к БД MS Access
            OleDbCommand command = new OleDbCommand(query, _сonnection);
            try
            { // получаем объект OleDbDataReader для чтения табличного результата запроса SELECT
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    str = str + reader[0].ToString() + "$";
                }
            }
            catch
            {

            }

            finally
            {
                reader.Close();               
            }
            return str;

        }

        
        //-----------------------------------------------------------------------------------------------

        // Обработчик добавить запись в БД
        private void bAddLines_Click(object sender, EventArgs e)
        {
            // получаем названия датчиков из БД
            SensNameList = FormSensNameList(null, null);

            FormAddNewSensorsDB newForm = new FormAddNewSensorsDB();
            //newForm.ShowDialog();

            if (newForm.ShowDialog() == DialogResult.OK)
            {
                // Добавлем заданную модель в список моделей ListBox
                if (newForm.newModelSens != "")
                {
                    // текст запроса
                    string query = "INSERT INTO Tsensors (Type, Model, NumOfRange) VALUES ('" + newForm.newTypeSens + "', '" + newForm.newModelSens + "', 2)";
                    // создаем объект OleDbCommand для выполнения запроса к БД MS Access
                    OleDbCommand command = new OleDbCommand(query, _сonnection);

                    try
                    {
                        // выполняем запрос к MS Access
                        command.ExecuteNonQuery();
                        lvwModels.Items.Add(newForm.newTypeSens);
                        lvwModels.Items[lvwModels.Items.Count - 1].SubItems.Add(newForm.newModelSens);
                       
                        lvwModels.Focus();
                        lvwModels.Select();
                        lvwModels.Items[lvwModels.Items.Count - 1].Focused = true;
                        lvwModels.Items[lvwModels.Items.Count - 1].Selected = true;
                        
                        foreach (var gb in this.Controls.OfType<GroupBox>())
                        {
                            foreach (var tb in gb.Controls.OfType<TextBox>())
                            {
                                if ((tb is TextBox) && (tb.Tag != null))
                                {
                                    tb.Text = "";
                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Не удалось добавить заданную модель датчика БД. Возможно, такая модель уже есть в БД", "Добавление записи. Операция прервана", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    finally
                    {
                        //_сonnection.Close();
                    }


                }
            else
            {                
            }

               
            }
        }

        
        //-----------------------------------------------------------------------------------------------

        // Обработчик удаления записи
        private void bDeleteLines_Click(object sender, EventArgs e)
        {
            if (lvwModels.SelectedItems.Count <= 0)
            return;


            string str = lvwModels.SelectedItems[0].SubItems[1].Text;

            // текст запроса
            string query = "DELETE FROM TSensors WHERE Model = '" + str + "'";

            // создаем объект OleDbCommand для выполнения запроса к БД MS Access
            OleDbCommand command = new OleDbCommand(query, _сonnection);

            try
            {
                // выполняем запрос к MS Access
                command.ExecuteNonQuery();
                // обновляем данные listbox
                GetData();
            }
            catch
            {
                MessageBox.Show("Не удалось удалить выбранную запись из БД", "Удаление записи. Операция прервана.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }     
        }


        //-----------------------------------------------------------------------------------------------

        // Сохранение параметров модели
        private void bSaveLines_Click(object sender, EventArgs e)
        {
            if (lvwModels.SelectedItems.Count <= 0)
                return;
            string str = lvwModels.SelectedItems[0].SubItems[1].Text;



            string partQuery = "";

            foreach (var gb in this.Controls.OfType<GroupBox>())
            {
                foreach (var tb in gb.Controls.OfType<NumericUpDown>())
                {
                    if ((tb is NumericUpDown) && (tb.Tag != null))
                    {
                        partQuery = partQuery + tb.Name + "=" + "'" + tb.Text + "'" + ",";
                    }
                }
            }

            foreach (var gb in this.Controls.OfType<GroupBox>())
            {
                foreach (var tb in gb.Controls.OfType<TextBox>())
                {
                    if ((tb is TextBox) && (tb.Tag != null))
                    {
                        partQuery = partQuery + tb.Name + "=" + "'" + tb.Text + "'" + ",";
                    }
                }
            }


            partQuery = partQuery.TrimEnd(new char[] { ',' });
            // текст запроса
            /*    string query = "UPDATE Tsensors SET " +
                                "Serial = " + Serial.Text + 
                                ", Pmin = " + tbPmin.Text +
                                ", NumOfRange = " + (Convert.ToInt16(rbRange2.Checked) + 1) +
                                " WHERE Model = '" + str + "'";*/

            string query = "UPDATE Tsensors SET " +
                            partQuery +
                            ", NumOfRange = " + (Convert.ToInt16(rbRange2.Checked) + 1) +
                            " WHERE Model = '" + str + "'";


            // создаем объект OleDbCommand для выполнения запроса к БД MS Access
            OleDbCommand command = new OleDbCommand(query, _сonnection);

            try
            {
                // выполняем запрос к MS Access
                command.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Не удалось сохранить данные в файл с БД", "Сохранение данных. Операция прервана.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        //-----------------------------------------------------------------------------------------------

        // Диалог открытия файла с БД по нажатию кнопки "Открыть БД"
        private void bOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog _openFileDialog = new OpenFileDialog();
            _openFileDialog.Filter = "access files (*.mdb)|*.mdb";

            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // получаем выбранный файл
                    string filename = _openFileDialog.FileName;
                    Charaterizator.Properties.Settings.Default.FileNameDB = filename;
                    Charaterizator.Properties.Settings.Default.Save();

                    // Если соединение с БД установлено - то закрываем
                    if (_сonnection.State == System.Data.ConnectionState.Open)
                    {
                        _сonnection.Close();
                    }                   

                    // Устанавливаем соединение с БД
                    SetConnectionDB(filename);
                    // Загружает данные в ListBox)
                    GetData();
                }
                
                catch
                {
                    //MessageBox.Show(ex.Message);
                    MessageBox.Show("Не удалось открыть файл с базой данных!", "Открытие файла БД. Операция прервана", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
        }


        //-----------------------------------------------------------------------------------------------

        // При закрытии формы закрываем соединение с БД
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }


        //-----------------------------------------------------------------------------------------------

        // Обработчик выбора датчика из списка  ListBox
        private void lvwModels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwModels.SelectedItems.Count <= 0) return;
          
            string str = lvwModels.SelectedItems[0].SubItems[1].Text;
            SetSensorsData(str);
        }



        //-----------------------------------------------------------------------------------------------      
        // Функция получить данные из поля заданного поля БД по номеру ID (Model)

        // Описание полей:
        //    Type              - Название
        //    Model             - Модель
        //    Serial            - Зав. номер
        //    Pmin              - Мин. верхний предел давл. кПа
        //    Pmax              - Макс. верхний предел давл. кПа
        //    NumOfRange        - Кол- во диапазонов
        //    Gain1             - КУ 1-го диап.
        //    Gain2             - КУ 2-го диап.
        //    Range1_Pmin       - Нижний предел измерения 1-го диап.
        //    Range1_Pmax       - Верхний предел измерения 1-го диап.
        //    Range2_Pmin       - Нижний предел измерения 2-го диап.
        //    Range2_Pmax       - Верхний предел измерения 2-го диап.
        //    DeltaRangeMin     - Минимально-допустимы диапазон настройки датчика, кПа
        //    HarTempPoint1     - Массив точек по темп. 1-го диапазона - для характеризации
        //    HarPressPoint1    - Массив точек по давл. 1-го диапазона - для характеризации
       
        //    HarPressPoint2    - Массив точек по давл. 2-го диапазона - для характеризации
        //    VerTempPoint1     - Массив точек по темп. 1-го диапазона - для верификации
        //    VerPressPoint1    - Массив точек по давл. 1-го диапазона - для верификации
       
        //    VerPressPoint2    - Массив точек по давл. 2-го диапазона - для верификации


        public string GetDataSensors(string strModel, string strField)
        {
            string strValue = null;

            if (_сonnection.State == System.Data.ConnectionState.Open)
            {
                try
                {
                  
                    // текст запроса
                    string query = "SELECT " + strField + " FROM TSensors WHERE Model = " + "'" + strModel + "'";
                    // создаем объект OleDbCommand для выполнения запроса к БД MS Access
                    command = new OleDbCommand(query, _сonnection);
                    // получаем объект OleDbDataReader для чтения табличного результата запроса SELECT
                    reader = command.ExecuteReader();
                    reader.Read();
                    strValue = reader[0].ToString();                   
                }

                catch
                {
                    //MessageBox.Show(ex.Message);
                    //MessageBox.Show("Не удалось считать данные из БД", MessageBoxButtons.OK);
                }
                finally
                {
                    // закрываем OleDbDataReader
                    if (reader!=null)
                    reader.Close();
                }        
            }
            return strValue;
        }
                

        //-----------------------------------------------------------------------------------------------

        // Обработчик - Записать параметры в датчик
        private void bFlashSensor_Click(object sender, EventArgs e)
        {
            if ((eni100 != null)&&(eni100.IsConnect()))
            {
                Program.txtlog.WriteLineLog("Старт записи индивидуальных параметров в датчик...", 0);
                eni100.sensor.SerialNumber = Convert.ToUInt32(Serial.Text);
                eni100.sensor.DownLevel = Convert.ToSingle(Pmin.Text);
                eni100.sensor.UpLevel = Convert.ToSingle(Pmax.Text);
                eni100.sensor.MinLevel = Convert.ToSingle(DeltaRangeMin.Text);
                char[] str = lvwModels.SelectedItems[0].SubItems[1].Text.ToCharArray(); ;
                for (int i = 0; i < eni100.sensor.PressureModel.Length; i++)
                {
                    if (str.Length > i)
                    {
                        eni100.sensor.PressureModel[i] = str[i];
                    }
                    else
                    {
                        eni100.sensor.PressureModel[i] = ' ';
                    }
                }

                eni100.EnterServis();
                eni100.WriteSerialNumberC49();//серийный номер
                eni100.UpDownWriteC249();//дипазон 
                eni100.C241WritePressureModel();//модель ПД

                MessageBox.Show("Запись индивидуальных параметров в датчик успешно закончена. ", "Завершение операции", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Program.txtlog.WriteLineLog("Запись индивидуальных параметров в датчик успешно закончена.", 0);
            }
            else
            {
                MessageBox.Show("Нет подключения к датчику!","Подключение к датчику. Операция прервана",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                Program.txtlog.WriteLineLog("Нет подключения к датчику!", 1);
            }
        }

        // Проверка на корректность вводимы значений диапазонов температур и давлений
        private void HarTempPoint1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8 && number != 44 && number != 45 && number != 59 && number != 127 && number != 32) 
            {
                e.Handled = true;
            }
        }
    }

}