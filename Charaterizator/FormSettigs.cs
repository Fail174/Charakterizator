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

        

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            ofdDataBase.ShowDialog();
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


                //5            
                Properties.Settings.Default.set_SensReadCount = Convert.ToDouble(tbSensReadCount.Text);
                Properties.Settings.Default.set_SensReadPause = Convert.ToDouble(tbSensReadPause.Text);
            }

            catch
            {
                /*tab_FormSettings.SelectedIndex = page_ind;
                TextBox tb = new TextBox();
                tb.Focus();
                bSetSettings.DialogResult = DialogResult.None;*/
                

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

        
    }
}
