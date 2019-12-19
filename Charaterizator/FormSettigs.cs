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
            Properties.Settings.Default.Reload();
            ReadSettings();
            //ReadSettings(MainForm.SettingsSelIndex);
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
                Properties.Settings.Default.set_HoldTimeTemp = Convert.ToInt32(tbHoldTimeTemp.Value);
                Properties.Settings.Default.set_HoldTimePress = Convert.ToInt32(tbHoldTimePress.Value);
                Properties.Settings.Default.set_HoldTimeAfReset = Convert.ToInt32(tbHoldTimeAfReset.Value);
                Properties.Settings.Default.set_DeltaPress = Convert.ToInt32(tbDeltaPress.Value);
                Properties.Settings.Default.set_DeltaTemp = Convert.ToInt32(tbDeltaTemp.Value);
                Properties.Settings.Default.set_MainTimer = Convert.ToInt32(tbMainTimer.Value);
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


                // 1 - Коммутатор
                Properties.Settings.Default.set_CommReadCH = Convert.ToInt32(tbCommReadCH.Value); //Convert.ToInt16(tbCommReadCH.Text);
                Properties.Settings.Default.set_CommMaxSetCH = Convert.ToInt32(tbCommMaxSetCH.Value);
                Properties.Settings.Default.set_CommReadPeriod = Convert.ToInt32(tbCommReadPeriod.Value);
                Properties.Settings.Default.set_CommReadPause = Convert.ToInt32(tbCommReadPause.Value);

                //2 - Мультиметр
                Properties.Settings.Default.set_MultimReadCount = Convert.ToInt32(tbMultimReadCount.Value);
                Properties.Settings.Default.set_MultimReadPeriod = Convert.ToInt32(tbMultimReadPeriod.Value);
                Properties.Settings.Default.set_MultimReadTimeout = Convert.ToInt32(tbMultimReadTimeOut.Value);
                Properties.Settings.Default.set_MultimDataReady = Convert.ToInt32(tbMultimWaitReady.Value);

                //3 - МЕНСОР
                Properties.Settings.Default.set_MensorReadPeriod = Convert.ToInt32(tbMensorReadPeriod.Value);
                Properties.Settings.Default.set_MensorReadPause = Convert.ToInt32(tbMensorReadPause.Value);
                if (cbMensorSetZero.SelectedIndex == 1)
                {
                    Properties.Settings.Default.set_MensorSetZero = true;
                }
                else
                {
                    Properties.Settings.Default.set_MensorSetZero = false;
                }

                //4
                Properties.Settings.Default.set_TCameraReadPeriod = Convert.ToInt32(tbTCameraReadPeriod.Value);

                //5 - Датчики
                Properties.Settings.Default.set_SensReadCount = Convert.ToInt32(tbSensReadCount.Value);
                Properties.Settings.Default.set_SensReadPause = Convert.ToInt32(tbSensReadPause.Value);
                Properties.Settings.Default.set_SensWaitTimeout = Convert.ToInt32(tbSensWaitPause.Value);
                //tbSensWaitPause.Value;

                //Сохраняем настройки
                //Properties.Settings.Default.Save();
                Program.txtlog.WriteLineLog("Настройки программы успешно сохранены!", 0);
            }
            catch
            {
                Program.txtlog.WriteLineLog("Непредвиденная ошибка сохранения настроек программы!", 1);
            }
            finally
            {
            }
        }
      

        // Обработчик пререключений между окнами настроек
        private void tab_FormSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int ind = tab_FormSettings.SelectedIndex;
            //ReadSettings(ind);
        }


        // Обновление данных выбранной вкладки с настройками
        private void ReadSettings()
        {
           /* switch (ind)
            {
                case 0:*/
                    {
                        // считываем данные из Settings и выводим на форму с настройками
                        tbHoldTimeTemp.Value = Properties.Settings.Default.set_HoldTimeTemp;
                        tbHoldTimePress.Value = Properties.Settings.Default.set_HoldTimePress;
                        tbHoldTimeAfReset.Value = Properties.Settings.Default.set_HoldTimeAfReset;
                        tbDeltaTemp.Value = Properties.Settings.Default.set_DeltaTemp;
                        tbDeltaPress.Value = Properties.Settings.Default.set_DeltaPress;                        
                        tbMainTimer.Value = Properties.Settings.Default.set_MainTimer;
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

          //              return;
                    }
               // case 1:
                    {
                        tbCommReadCH.Value = Properties.Settings.Default.set_CommReadCH;
                        tbCommMaxSetCH.Value = Properties.Settings.Default.set_CommMaxSetCH;
                        tbCommReadPeriod.Value = Properties.Settings.Default.set_CommReadPeriod;
                        tbCommReadPause.Value = Properties.Settings.Default.set_CommReadPause;
         //               return;
                    }
               // case 2:
                    {
                        tbMultimReadCount.Value = Properties.Settings.Default.set_MultimReadCount;
                        tbMultimReadPeriod.Value = Properties.Settings.Default.set_MultimReadPeriod;
                        tbMultimReadTimeOut.Value = Properties.Settings.Default.set_MultimReadTimeout;
                        tbMultimWaitReady.Value = Properties.Settings.Default.set_MultimDataReady;
         //               return;
                    }
              //  case 3:
                    {
                        tbMensorReadPeriod.Value = Properties.Settings.Default.set_MensorReadPeriod;
                        tbMensorReadPause.Value = Properties.Settings.Default.set_MensorReadPause;
                        if (Properties.Settings.Default.set_MensorSetZero)
                        {
                            cbMensorSetZero.SelectedIndex = 1;
                        }
                        else
                        {
                            cbMensorSetZero.SelectedIndex = 0;
                        }
                        
          //              return;
                    }
              //  case 4:
                    {
                        tbTCameraReadPeriod.Value = Properties.Settings.Default.set_TCameraReadPeriod;                     
         //               return;
                    }
              //  case 5:
                    {
                        tbSensReadCount.Value = Properties.Settings.Default.set_SensReadCount;
                        tbSensReadPause.Value = Properties.Settings.Default.set_SensReadPause;
                        tbSensWaitPause.Value = Properties.Settings.Default.set_SensWaitTimeout;
           //             return;
                    }           
          //  }            
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
