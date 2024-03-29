﻿using System;
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
        public delegate void DelegatCalcMNK();
        static public DelegatCalcMNK EventCalcMNK;
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
                Properties.Settings.Default.set_Rezistor = Convert.ToDouble(tbRezistor.Value);
                Properties.Settings.Default.set_flagObrHod = cb_FlagObrHod.Checked;
                Properties.Settings.Default.set_MeanR = cb_meanR.Checked;
                Properties.Settings.Default.set_AutoRegim = cb_AutoRegim.Checked;
                Properties.Settings.Default.set_Deviation = Convert.ToDouble(tb_Deviation.Value);
                Properties.Settings.Default.set_PushPress = cb_PushPress.Checked;

                // 1 - Коммутатор-1
                Properties.Settings.Default.set_CommReadCH = Convert.ToInt32(tbCommReadCH.Value);                       //
                Properties.Settings.Default.set_CommMaxSetCH = Convert.ToInt32(tbCommMaxSetCH.Value);                   //
                Properties.Settings.Default.set_CommReadPeriod = Convert.ToInt32(tbCommReadPeriod.Value);               //
                Properties.Settings.Default.set_CommReadPause = Convert.ToInt32(tbCommReadPause.Value);                 //
                Properties.Settings.Default.set_CommMaxLevelCount = Convert.ToInt32(tbCommMaxLevelCount.Value);

                // 1 - Коммутатор-2
                Properties.Settings.Default.set_CommReadCH2 = Convert.ToInt32(tbCommReadCH2.Value);                       //
                Properties.Settings.Default.set_CommMaxSetCH2 = Convert.ToInt32(tbCommMaxSetCH2.Value);                   //
                Properties.Settings.Default.set_CommReadPeriod2 = Convert.ToInt32(tbCommReadPeriod2.Value);               //
                Properties.Settings.Default.set_CommReadPause2 = Convert.ToInt32(tbCommReadPause2.Value);                 //
                Properties.Settings.Default.set_CommMaxLevelCount2 = Convert.ToInt32(tbCommMaxLevelCount2.Value);


                //2 - Мультиметр
                Properties.Settings.Default.set_MultimReadCount = Convert.ToInt32(tbMultimReadCount.Value);             //
                Properties.Settings.Default.set_MultimReadPeriod = Convert.ToInt32(tbMultimReadPeriod.Value);           //
                Properties.Settings.Default.set_MultimReadTimeout = Convert.ToInt32(tbMultimReadTimeOut.Value);         //  
                Properties.Settings.Default.set_MultimDataReady = Convert.ToInt32(tbMultimWaitReady.Value);             //
                Properties.Settings.Default.set_UseMultimAgilent = rb_useMultimAgilent.Checked;

                //3 - МЕНСОР
                Properties.Settings.Default.set_MensorReadPeriod = Convert.ToInt32(tbMensorReadPeriod.Value);           //
                Properties.Settings.Default.set_MensorReadPause = Convert.ToInt32(tbMensorReadPause.Value);             //
                Properties.Settings.Default.set_MensorSKOPressure = Convert.ToDouble(tbMensorSKOPressure.Value);        // допуск по давлению
                Properties.Settings.Default.set_MensorMaxCountPoint = Convert.ToInt32(tbMensorMaxCountPoint.Value);     // время стабилизации давления
                if (rb_useMensor.Checked)
                    Properties.Settings.Default.set_selectPressurer = 0;
                else if (rb_usePascal.Checked)
                    Properties.Settings.Default.set_selectPressurer = 1;
                else if (rb_useElemer.Checked)
                    Properties.Settings.Default.set_selectPressurer = 2;
                
                //4 - ТЕРМОКАМЕРА
                Properties.Settings.Default.set_TCameraReadPeriod = Convert.ToInt32(tbTCameraReadPeriod.Value);         //

                //5 - Датчики
                Properties.Settings.Default.set_SensReadCount = Convert.ToInt32(tbSensReadCount.Value);                 //
                Properties.Settings.Default.set_SensReadPause = Convert.ToInt32(tbSensReadPause.Value);                 //
                Properties.Settings.Default.set_SensWaitTimeout = Convert.ToInt32(tbSensWaitPause.Value);               //           

                //5 - математика - параметры рассчета
                Properties.Settings.Default.set_Math_Kf = Convert.ToInt32(tbMath_Kf.Value);
                Properties.Settings.Default.set_Math_Kmax_dop = Convert.ToInt32(tbMath_Kmax_dop.Value);
                Properties.Settings.Default.set_Math_Amax = Convert.ToInt32(tbMath_Amax.Value);
                Properties.Settings.Default.set_Math_Mmax = Convert.ToInt32(tbMath_Mmax.Value);
                Properties.Settings.Default.set_Math_Tnku = Convert.ToDouble(tbMath_Tnku.Value);
                Properties.Settings.Default.set_Math_KdM = Convert.ToDouble(tbMath_KdM.Value);
                Properties.Settings.Default.set_Math_DFdop_min = Convert.ToDouble(tbMath_DFdop_min.Value);
                Properties.Settings.Default.set_Math_Res_count_max = Convert.ToInt32(tbMath_Fr_min.Value);
                Properties.Settings.Default.set_Math_AlgorithmMNK = rbMNK.Checked;

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
            tbRezistor.Value = Convert.ToDecimal(Properties.Settings.Default.set_Rezistor);
            cb_FlagObrHod.Checked = Properties.Settings.Default.set_flagObrHod;
            cb_meanR.Checked = Properties.Settings.Default.set_MeanR;
            cb_AutoRegim.Checked = Properties.Settings.Default.set_AutoRegim;
            tb_Deviation.Value = Convert.ToDecimal(Properties.Settings.Default.set_Deviation);
            cb_PushPress.Checked = Properties.Settings.Default.set_PushPress;



            // 1 - Коммутатор-1                 
            tbCommReadCH.Value = Properties.Settings.Default.set_CommReadCH;
            tbCommMaxSetCH.Value = Properties.Settings.Default.set_CommMaxSetCH;
            tbCommReadPeriod.Value = Properties.Settings.Default.set_CommReadPeriod;
            tbCommReadPause.Value = Properties.Settings.Default.set_CommReadPause;
            tbCommMaxLevelCount.Value = Properties.Settings.Default.set_CommMaxLevelCount;

            // 1 - Коммутатор-2                 
            tbCommReadCH2.Value = Properties.Settings.Default.set_CommReadCH2;
            tbCommMaxSetCH2.Value = Properties.Settings.Default.set_CommMaxSetCH2;
            tbCommReadPeriod2.Value = Properties.Settings.Default.set_CommReadPeriod2;
            tbCommReadPause2.Value = Properties.Settings.Default.set_CommReadPause2;
            tbCommMaxLevelCount2.Value = Properties.Settings.Default.set_CommMaxLevelCount2;

            //2 - Мультиметр
            tbMultimReadCount.Value = Properties.Settings.Default.set_MultimReadCount;
            tbMultimReadPeriod.Value = Properties.Settings.Default.set_MultimReadPeriod;
            tbMultimReadTimeOut.Value = Properties.Settings.Default.set_MultimReadTimeout;
            tbMultimWaitReady.Value = Properties.Settings.Default.set_MultimDataReady;
            rb_useMultimAgilent.Checked = Properties.Settings.Default.set_UseMultimAgilent;
            rb_useMultimEni201.Checked = !(Properties.Settings.Default.set_UseMultimAgilent);

            //3 - Менсор                   
            tbMensorReadPeriod.Value = Properties.Settings.Default.set_MensorReadPeriod;
            tbMensorReadPause.Value = Properties.Settings.Default.set_MensorReadPause;
            tbMensorSKOPressure.Value = Convert.ToDecimal(Properties.Settings.Default.set_MensorSKOPressure);
            tbMensorMaxCountPoint.Value = Properties.Settings.Default.set_MensorMaxCountPoint;
            switch (Properties.Settings.Default.set_selectPressurer)
            {
                case 0:
                    rb_useMensor.Checked = true;
                    rb_usePascal.Checked = false;
                    rb_useElemer.Checked = false;
                    break;

                case 1:
                    rb_useMensor.Checked = false;
                    rb_usePascal.Checked = true;
                    rb_useElemer.Checked = false;
                    break;

                case 2:
                    rb_useMensor.Checked = false;
                    rb_usePascal.Checked = false;
                    rb_useElemer.Checked = true;
                    break;
            }
            

            // 4 - Термокамера
            tbTCameraReadPeriod.Value = Properties.Settings.Default.set_TCameraReadPeriod;


            // 5 - Датчики                   
            tbSensReadCount.Value = Properties.Settings.Default.set_SensReadCount;
            tbSensReadPause.Value = Properties.Settings.Default.set_SensReadPause;
            tbSensWaitPause.Value = Properties.Settings.Default.set_SensWaitTimeout;

            //5 - математика - параметры рассчета
            tbMath_Kf.Value = Properties.Settings.Default.set_Math_Kf;
            tbMath_Kmax_dop.Value = Properties.Settings.Default.set_Math_Kmax_dop;
            tbMath_Amax.Value = Properties.Settings.Default.set_Math_Amax;
            tbMath_Mmax.Value = Properties.Settings.Default.set_Math_Mmax;
            tbMath_Tnku.Value = Convert.ToDecimal(Properties.Settings.Default.set_Math_Tnku);
            tbMath_KdM.Value = Convert.ToDecimal(Properties.Settings.Default.set_Math_KdM);
            tbMath_DFdop_min.Value = Convert.ToDecimal(Properties.Settings.Default.set_Math_DFdop_min);
            tbMath_Fr_min.Value = Convert.ToInt32(Properties.Settings.Default.set_Math_Res_count_max);
            rbMNK.Checked = Properties.Settings.Default.set_Math_AlgorithmMNK;
            rbLinear.Checked = !(Properties.Settings.Default.set_Math_AlgorithmMNK);
            rbMNK_CheckedChanged(null, null);

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

        private void rbMNK_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMNK.Checked)
            {
                tbMath_Kf.Enabled = true;
                tbMath_Kmax_dop.Enabled = true;
                tbMath_Amax.Enabled = true;
                tbMath_Mmax.Enabled = true;
                tbMath_Tnku.Enabled = true;
                tbMath_KdM.Enabled = true;
                tbMath_DFdop_min.Enabled = true;
                tbMath_Fr_min.Enabled = true;
                btnCalcMNK.Enabled = true;
            }
            else
            {
                tbMath_Kf.Enabled = false;
                tbMath_Kmax_dop.Enabled = false;
                tbMath_Amax.Enabled = false;
                tbMath_Mmax.Enabled = false;
                tbMath_Tnku.Enabled = false;
                tbMath_KdM.Enabled = false;
                tbMath_DFdop_min.Enabled = false;
                tbMath_Fr_min.Enabled = false;
                btnCalcMNK.Enabled = false;
            }

        }

        private void bRestoreParamMNK_Click(object sender, EventArgs e)
        {
            // МНК - параметры рассчета по умолчанию              
            // Kf - коэффициент для формирования расчетной границы
            int Kf = 1;
            //Kpmax_dop - максимально допускаемое значение коэффициента перенастройки
            int Kpmax_dop = 10;
            // Максимальное кол-во циклов поиска
            int Amax = 250;
            // Максимальное значение веса
            int Mmax = 1000;
            // Температура НКУ
            double Tnku = 23;
            // Коэффициент регулировки при расчете шага веса 
            double KdM = 0.01;
            // Допускаемый минимальный шаг расчетной границы
            double deltaFdop_min = 0.005;
            // Минимальная расчетная граница
            int ResCount_max = 200;

            Properties.Settings.Default.set_Math_Kf = Kf;
            Properties.Settings.Default.set_Math_Kmax_dop = Kpmax_dop;
            Properties.Settings.Default.set_Math_Amax = Amax;
            Properties.Settings.Default.set_Math_Mmax = Mmax;
            Properties.Settings.Default.set_Math_Tnku = Tnku;
            Properties.Settings.Default.set_Math_KdM = KdM;
            Properties.Settings.Default.set_Math_DFdop_min = deltaFdop_min;
            Properties.Settings.Default.set_Math_Res_count_max = ResCount_max;
            Properties.Settings.Default.set_Math_AlgorithmMNK = rbMNK.Checked;

            Properties.Settings.Default.Save();  // Сохраняем переменные в settings.
            Program.txtlog.WriteLineLog("Настройки программы успешно сохранены!", 0);

            // Обновляем поле с настройками (читаем из dettings и записываем в поле)
            tbMath_Kf.Value = Properties.Settings.Default.set_Math_Kf;
            tbMath_Kmax_dop.Value = Properties.Settings.Default.set_Math_Kmax_dop;
            tbMath_Amax.Value = Properties.Settings.Default.set_Math_Amax;
            tbMath_Mmax.Value = Properties.Settings.Default.set_Math_Mmax;
            tbMath_Tnku.Value = Convert.ToDecimal(Properties.Settings.Default.set_Math_Tnku);
            tbMath_KdM.Value = Convert.ToDecimal(Properties.Settings.Default.set_Math_KdM);
            tbMath_DFdop_min.Value = Convert.ToDecimal(Properties.Settings.Default.set_Math_DFdop_min);
            tbMath_Fr_min.Value = Convert.ToInt32(Properties.Settings.Default.set_Math_Res_count_max);
            rbMNK.Checked = Properties.Settings.Default.set_Math_AlgorithmMNK;
            rbLinear.Checked = !(Properties.Settings.Default.set_Math_AlgorithmMNK);
            rbMNK_CheckedChanged(null, null);
        }

        private void btnCalcMNK_Click(object sender, EventArgs e)
        {
            EventCalcMNK();
        }
    }
}
