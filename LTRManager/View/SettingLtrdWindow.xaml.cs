using LTRManager.Model;
using System;
using System.Collections.Generic;
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

namespace LTRManager.View
{
    /// <summary>
    /// Логика взаимодействия для SettingLtrdWindow.xaml
    /// </summary>
    public partial class SettingLtrdWindow : Window
    {
        public SettingLtrdWindow()
        {
            InitializeComponent();
        }
        public SettingLtrd ShowDialog(SettingLtrd setting)
        {
            TimeSurvey_TextBox.Text = setting.TimeSurvey.ToString();
            TimeoutForCommandExecution_TextBox.Text = setting.TimeoutForCommandExecution.ToString();
            ConnectionEstablishmentTimeout_TextBox.Text = setting.ConnectionEstablishmentTimeout.ToString();
            TimeRecconect_TextBox.Text = setting.TimeRecconect.ToString();
            SizeBufferTransfer_TextBox.Text = setting.SizeBufferTransfer.ToString();
            SizeBufferReceive_TextBox.Text = setting.SizeBufferReceive.ToString();
            TransferWithoutDelay_CheckBox.IsChecked = setting.TransferWithoutDelay;

            bool showResult = base.ShowDialog() ?? false;
            if (showResult)
            {
                try
                {
                    setting.TimeSurvey = int.Parse(TimeSurvey_TextBox.Text);
                    setting.TimeoutForCommandExecution = int.Parse(TimeoutForCommandExecution_TextBox.Text);
                    setting.ConnectionEstablishmentTimeout = int.Parse(ConnectionEstablishmentTimeout_TextBox.Text);
                    setting.TimeRecconect = int.Parse(TimeRecconect_TextBox.Text);
                    setting.SizeBufferTransfer = int.Parse(SizeBufferTransfer_TextBox.Text);
                    setting.SizeBufferReceive = int.Parse(SizeBufferReceive_TextBox.Text);
                    setting.TransferWithoutDelay = TransferWithoutDelay_CheckBox.IsChecked.Value;

                    return setting;
                }
                catch
                {

                }
            }
            return null;
        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void TimeSurvey_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string num = "";
            foreach (var item in TimeSurvey_TextBox.Text)
            {
                if (item >= '0' && item <= '9') num += item;
            }
            TimeSurvey_TextBox.Text = num;
        }

        private void TimeoutForCommandExecution_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string num = "";
            foreach (var item in TimeoutForCommandExecution_TextBox.Text)
            {
                if (item >= '0' && item <= '9') num += item;
            }
            TimeoutForCommandExecution_TextBox.Text = num;
        }

        private void ConnectionEstablishmentTimeout_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string num = "";
            foreach (var item in ConnectionEstablishmentTimeout_TextBox.Text)
            {
                if (item >= '0' && item <= '9') num += item;
            }
            ConnectionEstablishmentTimeout_TextBox.Text = num;
        }

        private void TimeRecconect_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string num = "";
            foreach (var item in TimeRecconect_TextBox.Text)
            {
                if (item >= '0' && item <= '9') num += item;
            }
            TimeRecconect_TextBox.Text = num;
        }

        private void SizeBufferTransfer_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string num = "";
            foreach (var item in SizeBufferTransfer_TextBox.Text)
            {
                if (item >= '0' && item <= '9') num += item;
            }
            SizeBufferTransfer_TextBox.Text = num;
        }

        private void SizeBufferReceive_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string num = "";
            foreach (var item in SizeBufferReceive_TextBox.Text)
            {
                if (item >= '0' && item <= '9') num += item;
            }
            SizeBufferReceive_TextBox.Text = num;
        }
    }
}
