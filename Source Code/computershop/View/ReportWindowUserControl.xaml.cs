using computershop.ViewModel;
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

namespace computershop.View
{
    /// <summary>
    /// Interaction logic for ReportWindowUserControl.xaml
    /// </summary>
    public partial class ReportWindowUserControl : UserControl
    {
        public ReportWindowUserControl()
        {
            InitializeComponent();
            DataContext = new ReportViewModel();
        }

        private void cmbYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as System.Windows.Controls.ComboBox;
            if (comboBox != null && comboBox.SelectedItem != null)
            {
                var viewModel = DataContext as ReportViewModel;
                viewModel?.SelectYearCommand.Execute(comboBox.SelectedItem);
            }
        }

     
        private void cmbProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as System.Windows.Controls.ComboBox;
            if (comboBox != null && comboBox.SelectedItem != null)
            {
                var viewModel = DataContext as ReportViewModel;
                viewModel?.SelectProductCommand.Execute(comboBox.SelectedItem);
            }
        }
    }
}
