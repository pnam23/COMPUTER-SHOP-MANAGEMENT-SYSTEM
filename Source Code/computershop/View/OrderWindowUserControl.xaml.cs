using computershop.Model;
using computershop.ViewModel;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
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
    /// Interaction logic for OrderWindowUserControl.xaml
    /// </summary>
    public partial class OrderWindowUserControl : System.Windows.Controls.UserControl
    {
        public OrderWindowUserControl()
        {
            InitializeComponent();
            DataContext = new OrdersViewModel();
            //_connectionString = @"
            //    Server=.;
            //    Database = LoginDB;
            //    Trusted_Connection = yes;
            //    TrustServerCertificate = True
            //    ";
            //items = LoadData();
            //txtCurrentPage.Text = (currentPage + 1).ToString();
            //UpdateListView(items);
            //LoadOrderStatus();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as System.Windows.Controls.ListView;
            if (DataContext is OrdersViewModel viewModel)
            {
                viewModel.ViewProductDetailCommand.Execute(listView.SelectedItem);
            }
        }


        private void lvProductsInOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as System.Windows.Controls.ListView;
            if (DataContext is OrdersViewModel viewModel)
            {
                viewModel.DeleteProductsInOrderCommand.Execute(listView.SelectedItem);
            }
        }







        //private SqlConnection GetConnection()
        //{
        //    return new SqlConnection(_connectionString);
        //}

        //private BindingList<OrderModel> LoadData()
        //{
        //    Orders.Clear();
        //    using (var connection = GetConnection())
        //    using (var command = new SqlCommand())
        //    {
        //        connection.Open();
        //        command.Connection = connection;
        //        command.CommandText = "select * from [order], [customer] where cus_id = customer_id";

        //        using (var reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                OrderModel order = new OrderModel()
        //                {
        //                    OrderID = reader.GetInt32(reader.GetOrdinal("order_id")),
        //                    CustomerName = reader["customer_name"].ToString(),
        //                    CreateDate = reader.GetDateTime(reader.GetOrdinal("createdate")),
        //                    Total = reader.GetDecimal(reader.GetOrdinal("total")),
        //                    Profit = reader.GetDecimal(reader.GetOrdinal("profit")),
        //                    Status = reader["status"].ToString()
        //                };
        //                Orders.Add(order);
        //            }
        //        }
        //    }
        //    return Orders;
        //}

        //private void UpdateListView(BindingList<OrderModel> list)
        //{
        //    items = LoadData();
        //    var itemsToShow = list.Skip(currentPage * itemsPerPage).Take(itemsPerPage).ToList();
        //    totalPages = (int)Math.Ceiling((double)items.Count() / 10);
        //    if (currentPage >= totalPages)
        //    {
        //        currentPage--;
        //        txtCurrentPage.Text = (currentPage + 1).ToString();
        //    }
        //    txtTotalPages.Text = totalPages.ToString();
        //    txtNumItem.Text = itemsToShow.Count().ToString();
        //    lvOrder.ItemsSource = itemsToShow;
        //    txtTotal.Text = items.Count().ToString();
        //}

        //private void LoadOrderStatus()
        //{
        //    List<string> orderStatus = new List<string>()
        //    {
        //        "Pending",
        //        "Paid",
        //        "Cancelled",
        //    };
        //    cbbOrderStatus.SelectedIndex = 0;
        //    cbbOrderStatus.ItemsSource = orderStatus;
        //}

        //private void btnSearchDate_Click(object sender, RoutedEventArgs e)
        //{

        //    var startDate = dpStartDate.SelectedDate;
        //    var endDate = dpEndDate.SelectedDate;
        //    _searchOrders.Clear();

        //    if(startDate == null || endDate == null)
        //    {
        //        _type = (int)Functions.Main;

        //        System.Windows.Forms.MessageBox.Show("* Please select the date and try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
        //    }

        //    else if(startDate <= endDate)
        //    {
        //        _type = (int)Functions.Search;

        //        using (var connection = GetConnection())
        //        using (var command = new SqlCommand())
        //        {
        //            connection.Open();
        //            command.Connection = connection;
        //            command.CommandText = "select * from [order], [customer] where cus_id = customer_id and CreateDate between @startDate and @endDate";
        //            command.Parameters.Add("@startDate", SqlDbType.DateTime).Value = startDate;
        //            command.Parameters.Add("@endDate", SqlDbType.DateTime).Value = endDate;


        //            using (var reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    OrderModel order = new OrderModel()
        //                    {
        //                        OrderID = reader.GetInt32(reader.GetOrdinal("order_id")),
        //                        CustomerName = reader["customer_name"].ToString(),
        //                        CreateDate = reader.GetDateTime(reader.GetOrdinal("createdate")),
        //                        Total = reader.GetDecimal(reader.GetOrdinal("total")),
        //                        Profit = reader.GetDecimal(reader.GetOrdinal("profit"))
        //                    };
        //                    _searchOrders.Add(order);
        //                }
        //            }
        //        }

        //        UpdateListView(_searchOrders);
        //    }

        //    else
        //    {
        //        System.Windows.Forms.MessageBox.Show("* Please try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
        //    }


        //}

        //private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        //{
        //    orderMainGrid.Visibility = Visibility.Collapsed;
        //    addOrderGrid.Visibility = Visibility.Visible;
        //}

        //private void btnResetDate_Click(object sender, RoutedEventArgs e)
        //{
        //    dpStartDate.SelectedDate = null;
        //    dpEndDate.SelectedDate = null;
        //    UpdateListView(items);
        //}



        //private void btnEdit_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void btnDelete_Click(object sender, RoutedEventArgs e)
        //{
        //    if (lvOrder.SelectedItem != null)
        //    {
        //        var selectedOrder = (OrderModel)lvOrder.SelectedItem;
        //        var id = selectedOrder.OrderID;

        //        var result = System.Windows.MessageBox.Show($"Are you sure you want to delete the order #'{id}'?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

        //        if (result == MessageBoxResult.Yes)
        //        {

        //            using (var connection = GetConnection())
        //            using (var command = new SqlCommand())
        //            {
        //                connection.Open();
        //                command.Connection = connection;
        //                command.CommandText = "DELETE FROM [order] WHERE order_id = @id";
        //                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
        //                command.ExecuteNonQuery();
        //            }

        //            UpdateListView(items);

        //            System.Windows.MessageBox.Show("* Order has been removed successfully", "Announcement", MessageBoxButton.OK);
        //        }

        //    }

        //    else
        //    {
        //        System.Windows.MessageBox.Show("* Please select an order to delete", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        //private void btnPrevPage_Click(object sender, RoutedEventArgs e)
        //{
        //    if (currentPage > 0)
        //    {
        //        currentPage--;
        //        txtCurrentPage.Text = (currentPage + 1).ToString();
        //        if (_type == 0)
        //        {
        //            UpdateListView(items);
        //        }
        //        else
        //        {
        //            UpdateListView(_searchOrders);
        //        }
        //    }
        //}

        //private void btnNextPage_Click(object sender, RoutedEventArgs e)
        //{
        //    if (currentPage < totalPages - 1)
        //    {
        //        currentPage++;
        //        txtCurrentPage.Text = (currentPage + 1).ToString();
        //        if (_type == 0)
        //        {
        //            UpdateListView(items);
        //        }
        //        else
        //        {
        //            UpdateListView(_searchOrders);
        //        }
        //    }
        //}

        //private void btnBack_Click(object sender, RoutedEventArgs e)
        //{
        //    orderMainGrid.Visibility = Visibility.Visible;
        //    addOrderGrid.Visibility = Visibility.Collapsed;
        //}

        //private void btnAddNewOrder_Click(object sender, RoutedEventArgs e)
        //{

        //}
    }
}
