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



namespace SensorsDataBase
{
    public partial class FormSensorsDB : Form
    {
        OleDbConnection _сonnection;
        OleDbDataReader reader;
        public static string newTypeSens;
        public static string newModelSens;


        public FormSensorsDB()
        {
            InitializeComponent();
            string strFileNameDB = Properties.Settings.Default.FileNameDB;   // получаем путь и имя файла из Settings
            SetConnectionDB(strFileNameDB);                                  // устанавливаем соединение с БД
            GetData();                                                       // получаем список моделей из БД и записываем его в listbox
            // если список моделей из БД не пуст, позиционируемся на первой записи
            if (listModels.Items.Count > 0)
            {
                listModels.SelectedIndex = 0;
            }
        }



        // Функция устанавливает соединение с БД
        private void SetConnectionDB(string strFileNameDB)
        {
            string connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source = " + strFileNameDB + ";";
            // Создаем экземпляр класса OleDBConnection для подключения к БД
            _сonnection = new OleDbConnection(connectString);
            // Открываем подключение к БД
            try
            {
                _сonnection.Open();
                toolStripStatusLabel1.Text = "Соединение с БД установлено...  Файл БД: " + strFileNameDB;
            }
            catch (OleDbException ex)
            {
                //MessageBox.Show(ex.Message);
                MessageBox.Show(ex.Message, "Открытие файла базы данных...", MessageBoxButtons.OK);
                toolStripStatusLabel1.Text = "Соединение с БД не установлено... ";
            }
            finally
            {
                //_сonnection.Close();
            }
        }



        private void GetData()
        {
            if (_сonnection.State == System.Data.ConnectionState.Open)
            {
                // текст запроса
                string query = "SELECT Model FROM TSensors  ORDER BY Type";
                //Тип + Модель string query = "SELECT Type, Model FROM TSensors  ORDER BY Type";

                // создаем объект OleDbCommand для выполнения запроса к БД MS Access
                OleDbCommand command = new OleDbCommand(query, _сonnection);

                // получаем объект OleDbDataReader для чтения табличного результата запроса SELECT
                reader = command.ExecuteReader();

                // очищаем listBox1
                listModels.Items.Clear();

                // в цикле построчно читаем ответ от БД
                while (reader.Read())
                {
                    // выводим данные столбцов текущей строки в listBox1
                    listModels.Items.Add(reader[0].ToString());                           
                }
                // закрываем OleDbDataReader
                reader.Close();
            }
        }




        // Обработчик выбора модели датчика
        private void listModels_SelectedIndexChanged(object sender, EventArgs e)
        {            
            string str = listModels.SelectedItem.ToString();
            SetSensorsData(str);
        }



        // Функция обновления параметров датчика по названию модели
        // вх. данные - имя модели
        private void SetSensorsData(string model)
        {
            // текст запроса
            string query = "SELECT * FROM TSensors WHERE Model = " + "'" + model + "'";
            //Тип + Модель string query = "SELECT Type, Model FROM TSensors  ORDER BY Type";

            // создаем объект OleDbCommand для выполнения запроса к БД MS Access
            OleDbCommand command = new OleDbCommand(query, _сonnection);

            // получаем объект OleDbDataReader для чтения табличного результата запроса SELECT
            reader = command.ExecuteReader();
            reader.Read();

            int cnt = this.Controls.Count;
            //foreach (TextBox tb in this.Controls.OfType<TextBox>())

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

            // закрываем OleDbDataReader
            reader.Close();

        }



        private void rbRange1_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRange1.Checked)
            {                
                Gain2.Enabled = false;
                Range2_Pmin.Enabled = false;
                Range2_Pmax.Enabled = false;
                HarTempPoint2.Enabled = false;
                HarPressPoint2.Enabled = false;
                VerTempPoint2.Enabled = false;
                VerPressPoint2.Enabled = false;
            }
            else
            {               
                Gain2.Enabled = true;
                Range2_Pmin.Enabled = true;
                Range2_Pmax.Enabled = true;
                HarTempPoint2.Enabled = true;
                HarPressPoint2.Enabled = true;
                VerTempPoint2.Enabled = true;
                VerPressPoint2.Enabled = true;
            }
        }



        // Обработчик добавить запись
        private void bAddLines_Click(object sender, EventArgs e)
        {

            FormAddNewSensorsDB newForm = new FormAddNewSensorsDB();
            newForm.ShowDialog();

            // Добавлем заданную модель в список моделей ListBox
            if (newModelSens != "")
            {
                listModels.Items.Add(newModelSens);

                // текст запроса
                string query = "INSERT INTO Tsensors (Type, Model, NumOfRange) VALUES ('" + newTypeSens + "', '" + newModelSens + "', 2)";

                // создаем объект OleDbCommand для выполнения запроса к БД MS Access
                OleDbCommand command = new OleDbCommand(query, _сonnection);

                // выполняем запрос к MS Access
                command.ExecuteNonQuery();

                listModels.SelectedIndex = listModels.Items.Count - 1;

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
        }



        private void bDeleteLines_Click(object sender, EventArgs e)
        {
            if ((listModels.Items.Count - 1) > 0)
            {
                string str = listModels.SelectedItem.ToString();

                // текст запроса
                string query = "DELETE FROM TSensors WHERE Model = '" + str + "'";

                // создаем объект OleDbCommand для выполнения запроса к БД MS Access
                OleDbCommand command = new OleDbCommand(query, _сonnection);

                // выполняем запрос к MS Access
                command.ExecuteNonQuery();

                // обновляем данные listbox
                GetData();                                

                listModels.SelectedIndex = 0;
            }
            

            else if ((listModels.Items.Count - 1) == 0)
            {
                string str = listModels.SelectedItem.ToString();

                // текст запроса
                string query = "DELETE FROM TSensors WHERE Model = '" + str + "'";

                // создаем объект OleDbCommand для выполнения запроса к БД MS Access
                OleDbCommand command = new OleDbCommand(query, _сonnection);

                // выполняем запрос к MS Access
                command.ExecuteNonQuery();

                GetData();
                listModels.SelectedIndex = -1;

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
            }

            else
            {
                return;
            }                                 
        }




        private void bSaveLines_Click(object sender, EventArgs e)
        {
            //listModels.Items.Add(newModelSens);
            //string strFields = "Type, Model, Serial, Pmin, Pmax, NumOfRange, Gain1, Gain2, Range1_Pmin, Range1_Pmax, Range2_Pmin, Range2_Pmax, DeltaRangeMin," +
            //                    "HarTempPoint1, HarPressPoint1, HarTempPoint2, HarPressPoint2, VerTempPoint1, VerPressPoint1, VerTempPoint2, VerPressPoint2";
            //string strFValues;

            string str = listModels.SelectedItem.ToString();


            string partQuery = "";

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

            // выполняем запрос к MS Access
            command.ExecuteNonQuery();
        }




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
                    Properties.Settings.Default.FileNameDB = filename;
                    Properties.Settings.Default.Save();

                    // Если соединение с БД установлено - то закрываем
                    if (_сonnection.State == System.Data.ConnectionState.Open)
                    {
                        _сonnection.Close();
                    }
                    // очищаем listBox1
                    listModels.Items.Clear();

                    // Устанавливаем соединение с БД
                    SetConnectionDB(filename);

                    // Загружает данные в ListBox)
                    GetData();
                }
                
                catch
                {
                    //MessageBox.Show(ex.Message);
                    MessageBox.Show("Не удалось открыть файл с БД", "Открытие файла базы данных...", MessageBoxButtons.OK);
                }

            }
        }


        // При закрытии формы закрываем соединение с БД
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_сonnection != null)
            {
                _сonnection.Close();
            }
        }


    }

}