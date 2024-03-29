﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
//
using System.Collections;
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

        public string selType
        {
            get { return lvwModels.FocusedItem.SubItems[0].Text.ToString(); }
            set { }
        }

        //
        private ListViewColumnSorterExt listViewColumnSorter;




        public FormSensorsDB()
        {
            InitializeComponent();
            listViewColumnSorter = new ListViewColumnSorterExt(lvwModels);
            // формируем списки модулей Паскаля
            VerModulePoint1.Items.AddRange(new string[] { "0:Не задан", "1:Внутренний 1", "2:Внутренний 2", "3:Внешний 1", "4:Внешний 2", "5:Внешний 3" });
            VerModulePoint1.SelectedIndex = 0;
            VerModulePoint2.Items.AddRange(new string[] { "0:Не задан", "1:Внутренний 1", "2:Внутренний 2", "3:Внешний 1", "4:Внешний 2", "5:Внешний 3" });
            VerModulePoint2.SelectedIndex = 0;
            VerModulePoint3.Items.AddRange(new string[] { "0:Не задан", "1:Внутренний 1", "2:Внутренний 2", "3:Внешний 1", "4:Внешний 2", "5:Внешний 3" });
            VerModulePoint3.SelectedIndex = 0;
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
        private void SetSensorsData(string type, string model)
        {
            // текст запроса
            string query = "SELECT * FROM TSensors WHERE Model = '" + model + "' AND Type = '" + type + "'";
            //Тип + Модель string query = "SELECT Type, Model FROM TSensors  ORDER BY Type";

            // создаем объект OleDbCommand для выполнения запроса к БД MS Access
           command = new OleDbCommand(query, _сonnection);


            try
            {
                // получаем объект OleDbDataReader для чтения табличного результата запроса SELECT
                reader = command.ExecuteReader();
                reader.Read();

                // чтение данных
                int cnt = this.Controls.Count;
              
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

                // Чтение модулей Паскаль
                VerModulePoint1.SelectedIndex = VerModulePoint1.FindString(reader[Convert.ToInt32(26)].ToString());
                VerModulePoint2.SelectedIndex = VerModulePoint2.FindString(reader[Convert.ToInt32(27)].ToString());
                VerModulePoint3.SelectedIndex = VerModulePoint3.FindString(reader[Convert.ToInt32(28)].ToString());

                // Чтение количества диапазонов
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
                //VerPressPoint2.Enabled = false;
            }
            else
            {               
                Gain2.Enabled = true;
                Range2_Pmin.Enabled = true;
                Range2_Pmax.Enabled = true;               
                HarPressPoint2.Enabled = true;
                //VerPressPoint2.Enabled = true;
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
                if ((newForm.newModelSens != "")&&(newForm.newTypeSens != ""))
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
                        VerModulePoint1.SelectedIndex = -1;
                        VerModulePoint2.SelectedIndex = -1;
                        VerModulePoint3.SelectedIndex = -1;

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
                    newForm.Close();
                    MessageBox.Show("Не заданы тип или модель датчика", "Добавление записи. Операция прервана", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

               
            }
        }

        
        //-----------------------------------------------------------------------------------------------

        // Обработчик удаления записи
        private void bDeleteLines_Click(object sender, EventArgs e)
        {
            if (lvwModels.SelectedItems.Count <= 0)
            return;

            //var curIndex = lvwModels.SelectedIndices;
            var curIndex = lvwModels.FocusedItem.Index;

            DialogResult result = MessageBox.Show("Вы действительно хотите удалить выбранную запись", "Удаление записи.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
                return;



            string strModel = lvwModels.SelectedItems[0].SubItems[1].Text;
            string strType = lvwModels.SelectedItems[0].SubItems[0].Text;

            // текст запроса
            string query = "DELETE FROM TSensors WHERE Model = '" + strModel + "' AND Type = '" + strType + "'";

            // создаем объект OleDbCommand для выполнения запроса к БД MS Access
            OleDbCommand command = new OleDbCommand(query, _сonnection);

            try
            {
                // выполняем запрос к MS Access
                command.ExecuteNonQuery();
                // обновляем данные listbox
                GetData();

                if(curIndex >=1 )
                {
                    lvwModels.Focus();
                    lvwModels.Select();
                    lvwModels.Items[curIndex - 1].Focused = true;
                    lvwModels.Items[curIndex - 1].Selected = true;
                }
               
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
            string strModel = lvwModels.SelectedItems[0].SubItems[1].Text;
            string strType = lvwModels.SelectedItems[0].SubItems[0].Text;


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
           

            string query = "UPDATE Tsensors SET " +
                            partQuery +
                            ", NumOfRange='" + (Convert.ToInt16(rbRange2.Checked) + 1) + "'" +
                            ", VerModulePoint1='" + (VerModulePoint1.Text) + "'" +
                            ", VerModulePoint2='" + (VerModulePoint2.Text) + "'" +
                            ", VerModulePoint3='" + (VerModulePoint3.Text) + "'" +
                            " WHERE Type = '" + strType + "' AND Model = '" + strModel + "'";
                     
                                 
            // создаем объект OleDbCommand для выполнения запроса к БД MS Access
            OleDbCommand command = new OleDbCommand(query, _сonnection);

            try
            {
                // выполняем запрос к MS Access
                command.ExecuteNonQuery();
                MessageBox.Show("Данные успешно сохранены!", "Сохранение данных.", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
          
            string strModel = lvwModels.SelectedItems[0].SubItems[1].Text;
            string strType = lvwModels.SelectedItems[0].SubItems[0].Text;

            SetSensorsData(strType, strModel);
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


        public string GetDataSensors(string strType, string strModel, int iField)
        {
            string strValue = "";

            if (_сonnection.State == System.Data.ConnectionState.Open)
            {
                try
                {
                  
                    // текст запроса                  
                    string query = "SELECT * FROM TSensors WHERE Type = '" + strType + "' AND Model = '" + strModel + "'";
                    // создаем объект OleDbCommand для выполнения запроса к БД MS Access
                    command = new OleDbCommand(query, _сonnection);
                    // получаем объект OleDbDataReader для чтения табличного результата запроса SELECT
                    reader = command.ExecuteReader();
                    reader.Read();
                    strValue = reader[iField].ToString();                   
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
            //if (strValue == "") strValue = "-1";
            return strValue;
        }

        // Доработанный вариант функции GetDataSensors 
        // при ее вызове необходимо кроме номера модели датчика и названия запрашиваемого параметра, передавать тип датчика
        public string GetDataSensors(string strType, string strModel, string strField)
        {
            string strValue = "";

            if (_сonnection.State == System.Data.ConnectionState.Open)
            {
                try
                {

                    // текст запроса                   
                    string query = "SELECT " + strField + " FROM TSensors WHERE Type = '" + strType + "' AND Model = '" + strModel + "'";

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
                    if (reader != null)
                        reader.Close();
                }
            }
            //if (strValue == "") strValue = "-3";
            return strValue;
        }





        //-----------------------------------------------------------------------------------------------

        // Обработчик - Записать параметры в датчик
        private void bFlashSensor_Click(object sender, EventArgs e)
        {
            if ((eni100 != null)&&(eni100.IsConnect()))
            {
                Program.txtlog.WriteLineLog("Старт записи индивидуальных параметров в датчик...", 0);
                //eni100.sensor.SerialNumber = Convert.ToUInt32(Serial.Text);
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
                //eni100.WriteSerialNumberC49();//серийный номер
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




        // Скопировать запись
        private void bCopyLines_Click(object sender, EventArgs e)
        {
            // получаем названия датчиков из БД
            SensNameList = FormSensNameList(null, null);

            FormAddNewSensorsDB newForm = new FormAddNewSensorsDB();
            //newForm.ShowDialog();
            selType = lvwModels.FocusedItem.SubItems[0].Text.ToString();



            if (newForm.ShowDialog() == DialogResult.OK)
            {
                // Добавлем заданную модель в список моделей ListBox
                if ((newForm.newModelSens != "") && (newForm.newTypeSens != ""))
                {
                    // текст запроса
                    string query = "INSERT INTO Tsensors (Type, Model, NumOfRange) VALUES ('" + newForm.newTypeSens + "', '" + newForm.newModelSens + "', 2)";
                    // создаем объект OleDbCommand для выполнения запроса к БД MS Access
                    OleDbCommand command = new OleDbCommand(query, _сonnection);

                    try
                    {
                        // выполняем запрос к MS Access
                        command.ExecuteNonQuery();
                    }


                    catch
                    {
                        MessageBox.Show("Не удалось добавить заданную модель датчика БД. Возможно, такая модель уже есть в БД", "Добавление записи. Операция прервана", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    finally
                    {
                        //_сonnection.Close();
                    }
                    

                    string strModel = newForm.newModelSens;
                    string strType = newForm.newTypeSens;

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

                    query = "UPDATE Tsensors SET " +
                                    partQuery +
                                    ", NumOfRange = " + (Convert.ToInt16(rbRange2.Checked) + 1) +
                                    ", VerModulePoint1='" + (VerModulePoint1.Text) + "'" +
                                    ", VerModulePoint2='" + (VerModulePoint2.Text) + "'" +
                                    ", VerModulePoint3='" + (VerModulePoint3.Text) + "'" +
                                    " WHERE Type = '" + strType + "' AND Model = '" + strModel + "'";


                    // создаем объект OleDbCommand для выполнения запроса к БД MS Access
                    command = new OleDbCommand(query, _сonnection);

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

                    }
                    catch
                    {
                        MessageBox.Show("Не удалось сохранить данные в файл с БД", "Сохранение данных. Операция прервана.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }                                      

                }
                else
                {
                    newForm.Close();
                    MessageBox.Show("Не заданы тип или модель датчика", "Добавление записи. Операция прервана", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        public class ListViewColumnSorterExt : IComparer
        {
            /// <summary>
            /// Specifies the column to be sorted
            /// </summary>
            private int ColumnToSort;
            /// <summary>
            /// Specifies the order in which to sort (i.e. 'Ascending').
            /// </summary>
            private SortOrder OrderOfSort;
            /// <summary>
            /// Case insensitive comparer object
            /// </summary>
            private CaseInsensitiveComparer ObjectCompare;

            private ListView listView;
            /// <summary>
            /// Class constructor.  Initializes various elements
            /// </summary>
            public ListViewColumnSorterExt(ListView lv)
            {
                listView = lv;
                listView.ListViewItemSorter = this;
                listView.ColumnClick += new ColumnClickEventHandler(listView_ColumnClick);

                // Initialize the column to '0'
                ColumnToSort = 0;

                // Initialize the sort order to 'none'
                OrderOfSort = SortOrder.None;

                // Initialize the CaseInsensitiveComparer object
                ObjectCompare = new CaseInsensitiveComparer();
            }

            private void listView_ColumnClick(object sender, ColumnClickEventArgs e)
            {
                ReverseSortOrderAndSort(e.Column, (ListView)sender);
            }

            /// <summary>
            /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
            /// </summary>
            /// <param name="x">First object to be compared</param>
            /// <param name="y">Second object to be compared</param>
            /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
            public int Compare(object x, object y)
            {
                int compareResult;
                ListViewItem listviewX, listviewY;

                // Cast the objects to be compared to ListViewItem objects
                listviewX = (ListViewItem)x;
                listviewY = (ListViewItem)y;

                // Compare the two items
                compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);

                // Calculate correct return value based on object comparison
                if (OrderOfSort == SortOrder.Ascending)
                {
                    // Ascending sort is selected, return normal result of compare operation
                    return compareResult;
                }
                else if (OrderOfSort == SortOrder.Descending)
                {
                    // Descending sort is selected, return negative result of compare operation
                    return (-compareResult);
                }
                else
                {
                    // Return '0' to indicate they are equal
                    return 0;
                }
            }

            /// <summary>
            /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
            /// </summary>
            private int SortColumn
            {
                set
                {
                    ColumnToSort = value;
                }
                get
                {
                    return ColumnToSort;
                }
            }

            /// <summary>
            /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
            /// </summary>
            private SortOrder Order
            {
                set
                {
                    OrderOfSort = value;
                }
                get
                {
                    return OrderOfSort;
                }
            }

            private void ReverseSortOrderAndSort(int column, ListView lv)
            {
                // Determine if clicked column is already the column that is being sorted.
                if (column == this.SortColumn)
                {
                    // Reverse the current sort direction for this column.
                    if (this.Order == SortOrder.Ascending)
                    {
                        this.Order = SortOrder.Descending;
                    }
                    else
                    {
                        this.Order = SortOrder.Ascending;
                    }
                }
                else
                {
                    // Set the column number that is to be sorted; default to ascending.
                    this.SortColumn = column;
                    this.Order = SortOrder.Ascending;
                }

                // Perform the sort with these new sort options.
                lv.Sort();
            }
        }



    }

}