using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VCore.LTRManager;

namespace LTRManager.View
{
    /// <summary>
    /// Логика взаимодействия для CrateSyncSettingsWindow.xaml
    /// </summary>
    public partial class CrateSyncSettingsWindow : Window
    {
        public CrateSyncSettingsWindow()
        {
            InitializeComponent();
        }

        public LTRManagerSyncCrateSettings ShowDialog(LTRManagerSyncCrateSettings setting, string[] digout, string[] startMark, string[] secondMark)
        {
            OutputResolution.IsChecked = setting.OutputResolution;
            DIGOUT1_ComboBox.ItemsSource = digout;
            DIGOUT2_ComboBox.ItemsSource = digout;
            DIGOUT1_ComboBox.SelectedIndex = setting.DIGOUT1;
            DIGOUT2_ComboBox.SelectedIndex = setting.DIGOUT2;
            StartMark_ComboBox.ItemsSource = startMark;
            SecondMark_ComboBox.ItemsSource = secondMark;
            StartMark_ComboBox.SelectedIndex = setting.StartMarkMode;
            SecondMark_ComboBox.SelectedIndex = setting.SecondMarkMode;

            bool showResult = base.ShowDialog() ?? false;
            if (showResult)
            {
                setting.OutputResolution = OutputResolution.IsChecked.Value;
                setting.DIGOUT1 = DIGOUT1_ComboBox.SelectedIndex;
                setting.DIGOUT2 = DIGOUT2_ComboBox.SelectedIndex;
                setting.StartMarkMode = StartMark_ComboBox.SelectedIndex;
                setting.SecondMarkMode = SecondMark_ComboBox.SelectedIndex;

                return setting;
            }

            return null;
        }

        /*private void InstallStartMark_Button_Click(object sender, RoutedEventArgs e)
        {
            Setting.StartMarkMode = StartMark_ComboBox.SelectedIndex;
        }

        

        private void InstallDIGOUT_Button_Click(object sender, RoutedEventArgs e)
        {
            Setting.OutputResolution = OutputResolution.IsChecked.Value;
            Setting.DIGOUT1 = DIGOUT1_ComboBox.SelectedIndex;
            Setting.DIGOUT2 = DIGOUT2_ComboBox.SelectedIndex;
        }

        private void InstallSecondMark_Button_Click(object sender, RoutedEventArgs e)
        {
            Setting.SecondMarkMode = SecondMark_ComboBox.SelectedIndex;
        }*/

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
