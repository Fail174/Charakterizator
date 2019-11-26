using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Charaterizator
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();           
            tab_FormSettings.SelectedIndex = MainForm.SettingsSelIndex;
            ReadSettings(MainForm.SettingsSelIndex);
        }

        

       


        // Сохранение данных из формы в settings
        private void bSetSettings_Click(object sender, EventArgs e)
        {
            //int tag = 0;
            //int page_ind = 0;
            try
            {
                // считываем данные из формы и сохраняем их в Settings 
                // 0
                //page_ind = 0;
                //tag = Convert.ToInt32(tbHoldTimeTemp.Tag);
                Properties.Settings.Default.set_HoldTimeTemp = Convert.ToDouble(tbHoldTimeTemp.Text);
                Properties.Settings.Default.set_HoldTimePress = Convert.ToDouble(tbHoldTimePress.Text);
                Properties.Settings.Default.set_HoldTimeAfReset = Convert.ToDouble(tbHoldTimeAfReset.Text);
                Properties.Settings.Default.set_DeltaPress = Convert.ToDouble(tbDeltaPress.Text);
                Properties.Settings.Default.set_DeltaTemp = Convert.ToDouble(tbDeltaTemp.Text);
                Properties.Settings.Default.set_MainTimer = Convert.ToDouble(tbMainTimer.Text);
                if (cbHandlePress.SelectedIndex == 1)
                {
                    Properties.Settings.Default.set_HandleContrPress = true;
                }
                else
                {
                    Properties.Settings.Default.set_HandleContrPress = false;
                }
                if (cbHandleMultimetr.SelectedIndex == 1)
                {
                    Properties.Settings.Default.set_HandleContrMultimetr = true;
                }
                else
                {
                    Properties.Settings.Default.set_HandleContrMultimetr = false;
                }
                Properties.Settings.Default.FileNameDB = tbPathFile.Text;
                       

                // 1
                Properties.Settings.Default.set_CommReadCH = Convert.ToInt16(tbCommReadCH.Text);
                Properties.Settings.Default.set_CommMaxSetCH = Convert.ToInt16(tbCommMaxSetCH.Text);
                Properties.Settings.Default.set_CommReadPeriod = Convert.ToDouble(tbCommReadPeriod.Text);
                Properties.Settings.Default.set_CommReadPause = Convert.ToDouble(tbCommReadPause.Text);

                //2
                Properties.Settings.Default.set_MultimReadCount = Convert.ToInt16(tbMultimReadCount.Text);
                Properties.Settings.Default.set_MultimReadPeriod = Convert.ToDouble(tbMultimReadPeriod.Text);

                //3
                Properties.Settings.Default.set_MensorReadPeriod = Convert.ToDouble(tbMensorReadPeriod.Text);
                Properties.Settings.Default.set_MensorReadPause = Convert.ToDouble(tbMensorReadPause.Text);
                if (cbMensorSetZero.SelectedIndex == 1)
                {
                    Properties.Settings.Default.set_MensorSetZero = true;
                }
                else
                {
                    Properties.Settings.Default.set_MensorSetZero = false;
                }

                //4
                Properties.Settings.Default.set_TCameraReadPeriod = Convert.ToDouble(tbTCameraReadPeriod.Text);

                //5            
                Properties.Settings.Default.set_SensReadCount = Convert.ToDouble(tbSensReadCount.Text);
                Properties.Settings.Default.set_SensReadPause = Convert.ToDouble(tbSensReadPause.Text);

                //
                Properties.Settings.Default.Save();   
            }

            catch
            {
               
            }

            finally
            {

            }
          
        }
      

        // Обработчик пререключений между окнами настроек
        private void tab_FormSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ind = tab_FormSettings.SelectedIndex;
            ReadSettings(ind);
        }


        // Обновление данных выбранной вкладки с настройками
        private void ReadSettings(int ind)
        {
            switch (ind)
            {
                case 0:
                    {
                        // считываем данные из Settings и выводим на форму с настройками
                        tbHoldTimeTemp.Text = Properties.Settings.Default.set_HoldTimeTemp.ToString();
                        tbHoldTimePress.Text = Properties.Settings.Default.set_HoldTimePress.ToString();
                        tbHoldTimeAfReset.Text = Properties.Settings.Default.set_HoldTimeAfReset.ToString();
                        tbDeltaTemp.Text = Properties.Settings.Default.set_DeltaTemp.ToString();
                        tbDeltaPress.Text = Properties.Settings.Default.set_DeltaPress.ToString();                        
                        tbMainTimer.Text = Properties.Settings.Default.set_MainTimer.ToString();
                        if (Properties.Settings.Default.set_HandleContrPress)
                        {
                            cbHandlePress.SelectedIndex = 1;                          
                        }
                        else
                        {
                            cbHandlePress.SelectedIndex = 0;                           
                        }
                        if (Properties.Settings.Default.set_HandleContrMultimetr)
                        {
                            cbHandleMultimetr.SelectedIndex = 1;
                        }
                        else
                        {
                            cbHandleMultimetr.SelectedIndex = 0;
                        }
                        tbPathFile.Text = Properties.Settings.Default.FileNameDB;

                        return;
                    }
                case 1:
                    {
                        tbCommReadCH.Text = Properties.Settings.Default.set_CommReadCH.ToString();
                        tbCommMaxSetCH.Text = Properties.Settings.Default.set_CommMaxSetCH.ToString();
                        tbCommReadPeriod.Text = Properties.Settings.Default.set_CommReadPeriod.ToString();
                        tbCommReadPause.Text = Properties.Settings.Default.set_CommReadPause.ToString();
                        return;
                    }
                case 2:
                    {
                        tbMultimReadCount.Text = Properties.Settings.Default.set_MultimReadCount.ToString();
                        tbMultimReadPeriod.Text = Properties.Settings.Default.set_MultimReadPeriod.ToString();
                        return;
                    }
                case 3:
                    {
                        tbMensorReadPeriod.Text = Properties.Settings.Default.set_MensorReadPeriod.ToString();
                        tbMensorReadPause.Text = Properties.Settings.Default.set_MensorReadPause.ToString();
                        if (Properties.Settings.Default.set_MensorSetZero)
                        {
                            cbMensorSetZero.SelectedIndex = 1;
                        }
                        else
                        {
                            cbMensorSetZero.SelectedIndex = 0;
                        }
                        
                        return;
                    }
                case 4:
                    {
                        tbTCameraReadPeriod.Text = Properties.Settings.Default.set_TCameraReadPeriod.ToString();                     
                        return;
                    }
                case 5:
                    {
                        tbSensReadCount.Text = Properties.Settings.Default.set_SensReadCount.ToString();
                        tbSensReadPause.Text = Properties.Settings.Default.set_SensReadPause.ToString();
                        return;
                    }           
            }            
        }

        // Обработчик открыть файл с БД
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog _openFileDialog = new OpenFileDialog();
            _openFileDialog.Filter = "access files (*.mdb)|*.mdb";

            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // получаем выбранный файл
                    string filename = _openFileDialog.FileName;
                    //Properties.Settings.Default.FileNameDB = filename;
                    //Properties.Settings.Default.Save();

                    // Если соединение с БД установлено - то закрываем
                    if (MainForm.SensorsDB._сonnection.State == System.Data.ConnectionState.Open)
                    {
                        MainForm.SensorsDB._сonnection.Close();
                    }

                    // Устанавливаем соединение с БД
                    MainForm.SensorsDB.SetConnectionDB(filename);

                    // выводим путь к файлу на форму
                    tbPathFile.Text = filename;

                    // Загружает данные в ListBox)
                    MainForm.SensorsDB.GetData();




                }

                catch
                {
                    //MessageBox.Show(ex.Message);
                    MessageBox.Show("Не удалось открыть файл с базой данных!", "Открытие файла БД. Операция прервана", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }






    }
}
