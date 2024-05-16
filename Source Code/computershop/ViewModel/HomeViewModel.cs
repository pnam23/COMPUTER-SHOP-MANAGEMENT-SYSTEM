using computershop.Model;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace computershop.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private int totalOrders;
        public int TotalOrders
        {
            get { return totalOrders; }
            set
            {
                totalOrders = value;
                OnPropertyChanged(nameof(TotalOrders));
            }
        }

        private int totalOrdersThisMonth;
        public int TotalOrdersThisMonth
        {
            get { return totalOrdersThisMonth; }
            set
            {
                totalOrdersThisMonth = value;
                OnPropertyChanged(nameof(TotalOrdersThisMonth));
            }
        }

        private int totalCustomers;
        public int TotalCustomers
        {
            get { return totalCustomers; }
            set
            {
                totalCustomers = value;
                OnPropertyChanged(nameof(TotalCustomers));
            }
        }

        private int totalCustomersThisMonth;
        public int TotalCustomersThisMonth
        {
            get { return totalCustomersThisMonth; }
            set
            {
                totalCustomersThisMonth = value;
                OnPropertyChanged(nameof(TotalCustomersThisMonth));
            }
        }

        private decimal totalRevenue;
        public decimal TotalRevenue
        {
            get { return totalRevenue; }
            set
            {
                totalRevenue = value;
                OnPropertyChanged(nameof(TotalRevenue));
            }
        }

        private decimal totalRevenueThisMonth;
        public decimal TotalRevenueThisMonth
        {
            get { return totalRevenueThisMonth; }
            set
            {
                totalRevenueThisMonth = value;
                OnPropertyChanged(nameof(TotalRevenueThisMonth));
            }
        }

        private decimal totalProfit;
        public decimal TotalProfit
        {
            get { return totalProfit; }
            set
            {
                totalProfit = value;
                OnPropertyChanged(nameof(TotalProfit));
            }
        }

        private decimal totalProfitThisMonth;
        public decimal TotalProfitThisMonth
        {
            get { return totalProfitThisMonth; }
            set
            {
                totalProfitThisMonth = value;
                OnPropertyChanged(nameof(TotalProfit));
            }
        }

        public Func<ChartPoint, string> PointLabel => point
            => $"{point.Y} ({point.Participation:P1})";

        public SeriesCollection SeriesDataCollection { get; set; }


        private ObservableCollection<ProductModel> bestSellerList;
        public ObservableCollection<ProductModel> BestSellerList
        {
            get { return bestSellerList; }
            set
            {
                bestSellerList = value;
                OnPropertyChanged(nameof(BestSellerList));
            }
        }

        private readonly string _connectionString = @"
                Server=.;
                Database = LoginDB;
                Trusted_Connection = yes;
                TrustServerCertificate = True
                ";


        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public HomeViewModel()
        {
            ShowTotalOrders();
            ShowTotalOrdersThisMonth();
            ShowTotalCustomers();
            ShowTotalCustomersThisMonth();
            ShowTotalRevenue();
            ShowTotalRevenueThisMonth();
            ShowTotalProfit();
            ShowTotalProfitThisMonth();
            ShowBestSellerList();
            ShowChart();
        }

        private void ShowTotalOrders()
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"
                select count(*) as totalorders from [order]
                "
                ;


                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TotalOrders = reader.GetInt32(reader.GetOrdinal("totalorders"));
                    }
                }
            }
        }
        private void ShowTotalOrdersThisMonth()
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"
                    select count(*) as totalordersthismonth from [order]
                    where Month(createDate) = @thismonth and Year(createDate) = @thisYear"
                ;
                command.Parameters.Add("@thismonth", SqlDbType.Int).Value = DateTime.Now.Month;
                command.Parameters.Add("@thisyear", SqlDbType.Int).Value = DateTime.Now.Year;


                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TotalOrdersThisMonth = reader.GetInt32(reader.GetOrdinal("totalordersthismonth"));
                    }
                }
            }
        }


        private void ShowTotalCustomers()
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"
                select count(*) as totalcustomers from customer
                "
                ;


                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TotalCustomers = reader.GetInt32(reader.GetOrdinal("totalcustomers"));
                    }
                }
            }
        }
        private void ShowTotalCustomersThisMonth()
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"
                select count(distinct cus_id) as totalcustomersthismonth 
                from customer, [order]
                where customer_id = cus_id 
	                and Month(createDate) = @thismonth
	                and Year(createDate) = @thisyear
                "
                ;

                command.Parameters.Add("@thismonth", SqlDbType.Int).Value = DateTime.Now.Month;
                command.Parameters.Add("@thisyear", SqlDbType.Int).Value = DateTime.Now.Year;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TotalCustomersThisMonth = reader.GetInt32(reader.GetOrdinal("totalcustomersthismonth"));
                    }
                }
            }
        }

        private void ShowTotalRevenue()
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"
	                select sum(total) as totalrevenue
                	from [order] "
                ;


                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TotalRevenue = reader.GetDecimal(reader.GetOrdinal("totalrevenue"));
                    }
                }
            }
        }
        private void ShowTotalRevenueThisMonth()
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"
                select sum(total) as totalrevenuethismonth
                    from [order]
	                where Month(createDate) = @thismonth
	                and Year(createDate) = @thisyear        
                "
                ;

                command.Parameters.Add("@thismonth", SqlDbType.Int).Value = DateTime.Now.Month;
                command.Parameters.Add("@thisyear", SqlDbType.Int).Value = DateTime.Now.Year;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TotalRevenueThisMonth = reader.GetDecimal(reader.GetOrdinal("totalrevenuethismonth"));
                    }
                }
            }
        }
        private void ShowTotalProfit()
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"
	                select sum(profit) as totalprofit
                	from [order] "
                ;


                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TotalProfit = reader.GetDecimal(reader.GetOrdinal("totalprofit"));
                    }
                }
            }
        }
        private void ShowTotalProfitThisMonth()
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"
                select sum(profit) as totalprofitthismonth
	                from [order]
                    where Month(createDate) = @thismonth
	                and Year(createDate) = @thisyear
                "
                ;

                command.Parameters.Add("@thismonth", SqlDbType.Int).Value = DateTime.Now.Month;
                command.Parameters.Add("@thisyear", SqlDbType.Int).Value = DateTime.Now.Year;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TotalProfitThisMonth = reader.GetDecimal(reader.GetOrdinal("totalprofitthismonth"));
                    }
                }
            }
        }

        private void ShowBestSellerList()
        {
            BestSellerList = new ObservableCollection<ProductModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = """
                    
                    SELECT TOP 5
                        p.product_name AS ProductName,
                    	p.avatar as Avatar,
                        SUM(o.quantity) AS TotalQuantitySold
                    FROM 
                        orderdetails o
                    INNER JOIN 
                        product p ON o.product_id = p.product_id
                    INNER JOIN
                    	[order] ord on ord.order_id = o.order_id and ord.[status] = 'Paid' 

                    GROUP BY 
                        p.product_name, p.product_id, p.avatar
                    ORDER BY 
                        SUM(o.quantity) DESC
                    """;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ProductModel product = new ProductModel()
                        {
                            Name = reader["ProductName"].ToString(),
                            Avatar = reader["Avatar"].ToString(),
                            Quantity = reader.GetInt32(reader.GetOrdinal("TotalQuantitySold")),
                        };

                        BestSellerList.Add(product);
                    }
                }

            }
        }

        private void ShowChart()
        {
            LoadBrands();
        }


        private void LoadBrands()
        {
            SeriesDataCollection = new SeriesCollection();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = """
                    SELECT c.collection_name AS brand, COUNT(cl.product_id) AS totalproducts
                    FROM [collection] c
                    LEFT JOIN collection_list cl ON c.collection_id = cl.collection_id
                    GROUP BY c.collection_id, c.collection_name;
                    """;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var name = reader["brand"].ToString();
                        var quantity = reader.GetInt32(reader.GetOrdinal("totalproducts"));

                        bool dataLabels = quantity != 0;

                        SeriesDataCollection.Add(new PieSeries
                        {
                            Title = name,
                            Values = new ChartValues<int> { quantity },
                            DataLabels = dataLabels,
                            LabelPoint = PointLabel
                        });
                    }
                }
            }
        }
    }
}

