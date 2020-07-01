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
            // Properties.Settings.Default.Reload();
            try
            {
                ReadSettings();
                //ReadSettings(MainForm.SettingsSelIndex);
            }
            catch
            {
                Properties.Settings.Default.Reset();
            }
        }

        
        // Сохранение данных из формы в settings
        private void bSetSettings_Click(object sender, EventArgs e)
        {           
            try
            {
                // считываем данные из формы и сохраняем их в Settings               
                
                // 0 - Общие настройки программы
                Properties.Settings.Default.set_HoldTimeTemp = Convert.ToInt32(tbHoldTimeTemp.Value);               //
                Properties.Settings.Default.set_DeltaTemp = Convert.ToDouble(tbDeltaTemp.Value);                    //
                Properties.Settings.Default.set_MainTimer = Convert.ToInt32(tbMainTimer.Value);                     //
                Properties.Settings.Default.FileNameDB = tbPathFile.Text;                                           //
                // Проводить характ/вериф при отсутствии подключения к задатчику или мультиметру
                // пока не используется
                /*
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
                }*/
                    
                Properties.Settings.Default.set_MaxErrorCount = Convert.ToInt32(tbMaxErrorCount.Value);
                Properties.Settings.Default.set_MinSensorCurrent = Convert.ToDouble(tbMinSensorCurrent.Value);
                Properties.Settings.Default.set_MaxCountCAPRead = Convert.ToInt32(tbMaxCountCAPRead.Value);
                Properties.Settings.Default.set_SKOCurrent = Convert.ToDouble(tbSKOCurrent.Value);
                Properties.Settings.Default.set_SKOCalibrationCurrent = Convert.ToDouble(tb_SKOCalibrationCurrent.Value);
                Properties.Settings.Default.set_Rezistor = Convert.ToInt32(tbRezistor.Value);
                Properties.Settings.Default.set_flagObrHod = cb_FlagObrHod.Checked;
                Properties.Settings.Default.set_MeanR = cb_meanR.Checked;

                // 1 - Коммутатор
                Properties.Settings.Default.set_CommReadCH = Convert.ToInt32(tbCommReadCH.Value);                       //
                Properties.Settings.Default.set_CommMaxSetCH = Convert.ToInt32(tbCommMaxSetCH.Value);                   //
                Properties.Settings.Default.set_CommReadPeriod = Convert.ToInt32(tbCommReadPeriod.Value);               //
                Properties.Settings.Default.set_CommReadPause = Convert.ToInt32(tbCommReadPause.Value);                 //
                Properties.Settings.Default.set_CommMaxLevelCount = Convert.ToInt32(tbCommMaxLevelCount.Value);

                //2 - Мультиметр
                Properties.Settings.Default.set_MultimReadCount = Convert.ToInt32(tbMultimReadCount.Value);             //
                Properties.Settings.Default.set_MultimReadPeriod = Convert.ToInt32(tbMultimReadPeriod.Value);           //
                Properties.Settings.Default.set_MultimReadTimeout = Convert.ToInt32(tbMultimReadTimeOut.Value);         //  
                Properties.Settings.Default.set_MultimDataReady = Convert.ToInt32(tbMultimWaitReady.Value);             //

                //3 - МЕНСОР
                Properties.Settings.Default.set_MensorReadPeriod = Convert.ToInt32(tbMensorReadPeriod.Value);           //
                Properties.Settings.Default.set_MensorReadPause = Convert.ToInt32(tbMensorReadPause.Value);             //
                Properties.Settings.Default.set_MensorSKOPressure = Convert.ToDouble(tbMensorSKOPressure.Value);        // допуск по давлению
                Properties.Settings.Default.set_MensorMaxCountPoint = Convert.ToInt32(tbMensorMaxCountPoint.Value);     // время стабилизации давления
                Properties.Settings.Default.set_UseMensor = rb_useMensor.Checked;


               //4 - ТЕРМОКАМЕРА
                Properties.Settings.Default.set_TCameraReadPeriod = Convert.ToInt32(tbTCameraReadPeriod.Value);         //

                //5 - Датчики
                Properties.Settings.Default.set_SensReadCount = Convert.ToInt32(tbSensReadCount.Value);                 //
                Properties.Settings.Default.set_SensReadPause = Convert.ToInt32(tbSensReadPause.Value);                 //
                Properties.Settings.Default.set_SensWaitTimeout = Convert.ToInt32(tbSensWaitPause.Value);               //                

                Properties.Settings.Default.Save();  // Сохраняем переменные.*/
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
         
                   
            // считываем данные из Settings и выводим на форму с настройками
            tbHoldTimeTemp.Value = Properties.Settings.Default.set_HoldTimeTemp;                     
            tbDeltaTemp.Value = Convert.ToDecimal(Properties.Settings.Default.set_DeltaTemp);      
            tbMainTimer.Value = Properties.Settings.Default.set_MainTimer;
            tbPathFile.Text = Properties.Settings.Default.FileNameDB;

            // Проводить характ/вериф при отсутствии подключения к задатчику или мультиметру
            // пока не используется
            /*       if (Properties.Settings.Default.set_HandleContrPress)
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
                   }*/


            tbMaxErrorCount.Value = Properties.Settings.Default.set_MaxErrorCount;
            tbMinSensorCurrent.Value = Convert.ToDecimal(Properties.Settings.Default.set_MinSensorCurrent);
            tbMaxCountCAPRead.Value = Properties.Settings.Default.set_MaxCountCAPRead;
            tbSKOCurrent.Value = Convert.ToDecimal(Properties.Settings.Default.set_SKOCurrent);
            tb_SKOCalibrationCurrent.Value = Convert.ToDecimal(Properties.Settings.Default.set_SKOCalibrationCurrent);
            tbRezistor.Value = Properties.Settings.Default.set_Rezistor;
            cb_FlagObrHod.Checked = Properties.Settings.Default.set_flagObrHod;
            cb_meanR.Checked = Properties.Settings.Default.set_MeanR; 

            // 1 - Коммутатор                 
            tbCommReadCH.Value = Properties.Settings.Default.set_CommReadCH;
            tbCommMaxSetCH.Value = Properties.Settings.Default.set_CommMaxSetCH;
            tbCommReadPeriod.Value = Properties.Settings.Default.set_CommReadPeriod;
            tbCommReadPause.Value = Properties.Settings.Default.set_CommReadPause;
            tbCommMaxLevelCount.Value = Properties.Settings.Default.set_CommMaxLevelCount;

             //2 - Мультиметр
             tbMultimReadCount.Value = Properties.Settings.Default.set_MultimReadCount;
             tbMultimReadPeriod.Value = Properties.Settings.Default.set_MultimReadPeriod;
             tbMultimReadTimeOut.Value = Properties.Settings.Default.set_MultimReadTimeout;
             tbMultimWaitReady.Value = Properties.Settings.Default.set_MultimDataReady;
                   
              //3 - Менсор                   
             tbMensorReadPeriod.Value = Properties.Settings.Default.set_MensorReadPeriod;
             tbMensorReadPause.Value = Properties.Settings.Default.set_MensorReadPause;
             tbMensorSKOPressure.Value = Convert.ToDecimal(Properties.Settings.Default.set_MensorSKOPressure);
             tbMensorMaxCountPoint.Value = Properties.Settings.Default.set_MensorMaxCountPoint;

            if (Properties.Settings.Default.set_UseMensor)
            {
                rb_useMensor.Checked = true;
                rb_usePascal.Checked = false;
            }
            else
            {
                rb_useMensor.Checked = false;
                rb_usePascal.Checked = true;
            }
            

             // 4 - Термокамера
            tbTCameraReadPeriod.Value = Properties.Settings.Default.set_TCameraReadPeriod;                     
       
                   
             // 5 - Датчики                   
             tbSensReadCount.Value = Properties.Settings.Default.set_SensReadCount;
             tbSensReadPause.Value = Properties.Settings.Default.set_SensReadPause;
             tbSensWaitPause.Value = Properties.Settings.Default.set_SensWaitTimeout;                         
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

        private void cb_FlagObrHod_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}
