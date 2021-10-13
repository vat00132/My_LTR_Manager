using LTRManager.Model;
using LTRManager.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LTRManager.View
{
    /// <summary>
    /// Логика взаимодействия для TreeCratesControl.xaml
    /// </summary>
    public partial class TreeCratesControl : UserControl
    {
        public TreeCratesControl()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var vm = DataContext as LtrdControlViewModel;

            vm.StatisticsLtrdVisibility = System.Windows.Visibility.Collapsed;
            vm.StatisticsCrateVisibility = System.Windows.Visibility.Collapsed;
            vm.StatisticsLCardVisibility = System.Windows.Visibility.Collapsed;
            vm.SelectModule = false;
            vm.SelectCrate = false;

            var selectedItem = treeView1.SelectedItem as NodeObject;
            //выводим информация взавизимости от выбранного элемента
            if (selectedItem.TypeNode == TypeNode.Ltrd)
            {
                vm.SelectedLtrdServer = selectedItem.Object as LtrdServerObject;
                vm.StatisticsLtrdVisibility = System.Windows.Visibility.Visible;
            }
            else if (selectedItem.TypeNode == TypeNode.Crate)
            {
                vm.SelectedLtrdServer = selectedItem.ParantNode.Object as LtrdServerObject;
                vm.SelectedCrate = selectedItem.Object as CrateObject;
                vm.StatisticsLtrdVisibility = System.Windows.Visibility.Visible;
                vm.StatisticsCrateVisibility = System.Windows.Visibility.Visible;
                vm.SelectCrate = true;
            }
            else if (selectedItem.TypeNode == TypeNode.LCard)
            {
                vm.SelectedLtrdServer = selectedItem.ParantNode.ParantNode.Object as LtrdServerObject;
                vm.SelectedCrate = selectedItem.ParantNode.Object as CrateObject;
                vm.SelectedLCard = selectedItem.Object as LCardObject;
                vm.StatisticsLtrdVisibility = System.Windows.Visibility.Visible;
                vm.StatisticsCrateVisibility = System.Windows.Visibility.Visible;
                vm.StatisticsLCardVisibility = System.Windows.Visibility.Visible;
                vm.SelectModule = true;
            }
        }
    }
}
