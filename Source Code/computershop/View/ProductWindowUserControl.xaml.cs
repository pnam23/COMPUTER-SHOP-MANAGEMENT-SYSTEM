using computershop.Model;
using computershop.ViewModel;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace computershop.View
{
    /// <summary>
    /// Interaction logic for ProductWindowUserControl.xaml
    /// </summary>
    public partial class ProductWindowUserControl : System.Windows.Controls.UserControl
    {   
        public ProductWindowUserControl() {
            InitializeComponent();
            DataContext = new ProductViewModel();

        }

        private void cmbBrand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as System.Windows.Controls.ComboBox;
            if (comboBox != null && comboBox.SelectedItem != null)
            {
                var viewModel = DataContext as ProductViewModel;
                viewModel?.SelectBrandCommand.Execute(comboBox.SelectedItem);
            }
        }
        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as System.Windows.Controls.ComboBox;
            if (comboBox != null && comboBox.SelectedItem != null)
            {
                var viewModel = DataContext as ProductViewModel;
                viewModel?.SelectFilterCommand.Execute(comboBox.SelectedItem);
            }
        }
        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as System.Windows.Controls.ListView;
            if (DataContext is ProductViewModel viewModel)
            {
                viewModel.ViewProductDetailCommand.Execute(listView.SelectedItem);
            }
        }

    }
}
