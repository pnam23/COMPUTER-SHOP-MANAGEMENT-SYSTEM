using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using computershop.Model;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Windows.Media;
using System.Data;
using System.Runtime.InteropServices.JavaScript;
using System.Windows.Forms;

namespace computershop.ViewModel
{
    public class ReportViewModel:ViewModelBase
    {
        private DateTime reportStartDate;
        public DateTime ReportStartDate
        {
            get { return reportStartDate; }
            set
            {
                reportStartDate = value;
                OnPropertyChanged(nameof(ReportStartDate));
            }
        }
        private DateTime reportEndDate;
        public DateTime ReportEndDate
        {
            get { return reportEndDate; }
            set
            {
                reportEndDate = value;
                OnPropertyChanged(nameof(ReportEndDate));
            }
        }

        private SeriesCollection seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get { return seriesCollection; }
            set
            {
                seriesCollection = value;
                OnPropertyChanged(nameof(SeriesCollection));
            }
        }
        private SeriesCollection productSeriesCollection;
        public SeriesCollection ProductSeriesCollection
        {
            get { return productSeriesCollection; }
            set
            {
                productSeriesCollection = value;
                OnPropertyChanged(nameof(ProductSeriesCollection));
            }
        }
        private SeriesCollection allProductsSeriesCollection;
        public SeriesCollection AllProductsSeriesCollection
        {
            get { return allProductsSeriesCollection; }
            set
            {
                allProductsSeriesCollection = value;
                OnPropertyChanged(nameof(AllProductsSeriesCollection));
            }
        }
        

        public Func<object, object> Formatter { get; private set; }

        private string dateInfo;
        public string DateInfo
        {
            get
            {
                return dateInfo;
            }
            set
            {
                dateInfo = value;
                OnPropertyChanged(nameof(DateInfo));
            }
        }

        private Visibility statisticsGridVisibility = Visibility.Visible;
        public Visibility StatisticsGridVisibility
        {
            get { return statisticsGridVisibility; }
            set
            {
                statisticsGridVisibility = value;
                OnPropertyChanged(nameof(StatisticsGridVisibility));
            }
        }
        private Visibility productStatisticsGridVisibility = Visibility.Collapsed;
        public Visibility ProductStatisticsGridVisibility
        {
            get { return productStatisticsGridVisibility; }
            set
            {
                productStatisticsGridVisibility = value;
                OnPropertyChanged(nameof(ProductStatisticsGridVisibility));
            }
        }
        private Visibility allProductStatisticsGridVisibility = Visibility.Collapsed;
        public Visibility AllProductStatisticsGridVisibility
        {
            get { return allProductStatisticsGridVisibility; }
            set
            {
                allProductStatisticsGridVisibility = value;
                OnPropertyChanged(nameof(AllProductStatisticsGridVisibility));
            }
        }

        //private Visibility eachItemChartVisibility = Visibility.Collapsed;
        //public Visibility EachItemChartVisibility
        //{
        //    get { return eachItemChartVisibility; }
        //    set
        //    {
        //        eachItemChartVisibility = value;
        //        OnPropertyChanged(nameof(EachItemChartVisibility));
        //    }
        //}
        //private Visibility allItemsChartVisibility = Visibility.Collapsed;
        //public Visibility AllItemsChartVisibility
        //{
        //    get { return allItemsChartVisibility; }
        //    set
        //    {
        //        allItemsChartVisibility = value;
        //        OnPropertyChanged(nameof(AllItemsChartVisibility));
        //    }
        //}

        private ObservableCollection<string> years;

        public ObservableCollection<string> Years
        {
            get { return years; }
            set { years = value; OnPropertyChanged(nameof(Years)); }
        }

        private ObservableCollection<string> months;
        public ObservableCollection<string> Months
        {
            get { return months; }
            set { months = value; OnPropertyChanged(nameof(Months)); }
        }

        private ObservableCollection<ProductModel> products;
        public ObservableCollection<ProductModel> Products
        {
            get
            {
                return products;
            }
            set
            {
                products = value;
                OnPropertyChanged(nameof(Products));
            }
        }



        private string selectedYear;
        public string SelectedYear
        {
            get { return selectedYear; }
            set
            {
                selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
            }
        }

        private ProductModel selectedProduct;
        public ProductModel SelectedProduct
        {
            get
            {
                return selectedProduct;
            }
            set
            {
                selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }

        private string productErrorMessage;
        public string ProductErrorMessage
        {
            get { return productErrorMessage; }
            set
            {
                productErrorMessage = value;
                OnPropertyChanged(nameof(ProductErrorMessage));
            }
        }

        private List<string> brands;
        public List<string> Brands
        {
            get { return brands; }
            set
            {
                brands = value;
                OnPropertyChanged(nameof(Brands));
            }
        }


        //Commands
        public ICommand SearchCommand {  get; set; }
        public ICommand ResetDateCommand { get; set; }
        public ICommand ShowProductStatisticsCommand { get; set; }
        public ICommand ShowAllProductStatisticsCommand { get; set; }
        public ICommand BackFromProductStatisticsCommand { get; set; }
        public ICommand BackFromAllProductStatisticsCommand { get; set; }
        public ICommand SelectYearCommand { get; set; }
        public ICommand SelectProductCommand { get; set; }

        private void LoadYears()
        {
            Years = new ObservableCollection<string>();
            for (int year = 2000; year <= 2030; year++)
            {
                Years.Add(year.ToString());
            }
        }
        private void LoadMonths()
        {
            Months = new ObservableCollection<string>()
            {
                "Jan",
                "Feb",
                "Mar",
                "Apr",
                "May",
                "Jun",
                "Jul",
                "Aug",
                "Sep",
                "Oct",
                "Nov",
                "Dec"
            };

        }

        private void LoadProducts()
        {
            Products = new ObservableCollection<ProductModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [product]";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ProductModel product = new ProductModel()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("product_id")),
                            Name = reader["product_name"].ToString(),
                            Description = reader["product_des"].ToString(),
                            Capital = reader.GetDecimal(reader.GetOrdinal("capital")),
                            SalePrice = reader.GetDecimal(reader.GetOrdinal("saleprice")),
                            Status = reader["status"].ToString(),
                            Quantity = reader.GetInt32(reader.GetOrdinal("stock")),
                            Avatar = reader["avatar"].ToString()
                        };
                        Products.Add(product);
                    }
                }
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

        public ReportViewModel()
        {
            ReportStartDate = DateTime.Now;
            ReportEndDate = DateTime.Now;

            LoadYears();
            LoadMonths();
            LoadProducts();

           

            DateInfo = "* Please select a year to see the revenue & profit in that year"; 
            ProductErrorMessage = "* Please select a product to see the statistic of that product"; 

            SearchCommand = new ViewModelCommand(ExecuteSearchCommand);

            ResetDateCommand = new ViewModelCommand(ExecuteResetDateCommand);

            ShowProductStatisticsCommand = new ViewModelCommand(ExecuteShowProductStatisticsCommand);

            ShowAllProductStatisticsCommand = new ViewModelCommand(ExecuteShowAllProductStatisticsCommand);

            BackFromProductStatisticsCommand = new ViewModelCommand(ExecuteBackFromProductStatisticsCommand);

            BackFromAllProductStatisticsCommand = new ViewModelCommand(ExecuteBackFromAllProductStatisticsCommand);

            SelectYearCommand = new ViewModelCommand(ExecuteSelectYearCommand);

            SelectProductCommand = new ViewModelCommand(ExecuteSelectProductCommand);
        }

        private void ExecuteBackFromAllProductStatisticsCommand(object obj)
        {
            AllProductStatisticsGridVisibility = Visibility.Collapsed;
            ProductStatisticsGridVisibility = Visibility.Visible;
        }

        private void ExecuteShowAllProductStatisticsCommand(object obj)
        {
            AllProductStatisticsGridVisibility = Visibility.Visible;
            ProductStatisticsGridVisibility = Visibility.Collapsed;

            SelectedProduct = null;
            UpdateProductStatisticsChart();
        }

        private void ExecuteSelectYearCommand(object obj)
        {
            DateInfo = null;
            if (StatisticsGridVisibility == Visibility.Visible)
            {
                UpdaterChart();
            }

            if(ProductStatisticsGridVisibility == Visibility.Visible || AllProductStatisticsGridVisibility == Visibility.Visible)
            {
                UpdateProductStatisticsChart();
            }
        }

        private void UpdaterChart()
        {
           
            List<decimal> revenueValues = Enumerable.Repeat(0m, 12).ToList();
            List<decimal> profitValues = Enumerable.Repeat(0m, 12).ToList();
            List<int> months = Enumerable.Repeat(0, 12).ToList();

            var s_year = 0;
            var e_year = 0;
            var s_day = 1;
            var e_day = 31;
            var s_month = 1;
            var e_month = 12;


            if(SelectedYear == null && ReportStartDate == DateTime.Now)
            {
                s_year = 0 ;
                e_year = 0 ;
            }
            else if(SelectedYear != null)
            {
                s_year = int.Parse(SelectedYear);
                e_year = s_year;
                s_day = 1;
                e_day = 31;
                s_month = 1;
                e_month = 12;
            }
            else if (ReportEndDate != null && ReportStartDate != null)
            {
                s_year = ReportStartDate.Year;
                e_year = ReportEndDate.Year;
                s_day = ReportStartDate.Day;
                e_day = ReportEndDate.Day;
                s_month = ReportStartDate.Month;
                e_month = ReportEndDate.Month;
            }

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"
                                    SELECT 
                                        YEAR(ord.createDate) AS Year,
                                        MONTH(ord.createDate) AS Month,
                                        DAY(ord.createDate) AS Day,
                                        SUM(ord.total) AS TotalRevenue,
                                        SUM(ord.profit) AS TotalProfit
                                    FROM 
                                        [order] ord
                                    INNER JOIN 
                                        orderdetails o ON ord.order_id = o.order_id 
	                                    AND ord.[status] = 'Paid'
	                                    AND DAY(ord.createDate) between @startday and @endday
	                                    AND MONTH(ord.createDate) between @startmonth and @endmonth
	                                    AND YEAR (ord.createDate) between @startyear and @endyear
                                    GROUP BY 
                                        YEAR(ord.createDate),
                                        MONTH(ord.createDate),
                                        DAY(ord.createDate)
                                    ORDER BY 
                                        YEAR(ord.createDate),
                                        MONTH(ord.createDate),
                                        DAY(ord.createDate);
                                    ";
                command.Parameters.Add("@startday", SqlDbType.Int).Value = s_day;
                command.Parameters.Add("@endday", SqlDbType.Int).Value = e_day;
                command.Parameters.Add("@startmonth", SqlDbType.Int).Value = s_month;
                command.Parameters.Add("@endmonth", SqlDbType.Int).Value = e_month;
                command.Parameters.Add("@startyear", SqlDbType.Int).Value = s_year;
                command.Parameters.Add("@endyear", SqlDbType.Int).Value = e_year;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var revenue = reader.GetDecimal(reader.GetOrdinal("TotalRevenue"));
                        var profit = reader.GetDecimal(reader.GetOrdinal("TotalProfit"));
                        var month = reader.GetInt32(reader.GetOrdinal("Month"));
                        var year = reader.GetInt32(reader.GetOrdinal("Year"));


                        months[month - 1] = month;
                        foreach (var item in months)
                        {
                            if (item != 0)
                            {
                                revenueValues[item - 1] = revenue;
                                profitValues[item - 1] = profit;
                            }
                        }
                    }
                }

                SeriesCollection = new SeriesCollection()
                {
                    new LineSeries
                    {
                        Title = "Revenue",
                        Values = new ChartValues<decimal>(revenueValues),
                        Stroke = new SolidColorBrush(Colors.Cyan),
                        StrokeThickness = 1
                    },

                    new LineSeries
                    {
                        Title = "Profit",
                        Values = new ChartValues<decimal>(profitValues),
                        Stroke = new SolidColorBrush(Colors.DeepPink),
                        StrokeThickness = 1
                    }
                };
            }


        }


        private void ExecuteSelectProductCommand(object obj)
        {
            ProductErrorMessage = null;
            UpdateProductStatisticsChart();
        }

        private void UpdateProductStatisticsChart()
        {
            if (SelectedProduct != null)
            {
                List<decimal> revenueValues = Enumerable.Repeat(0m, 12).ToList();
                List<decimal> profitValues = Enumerable.Repeat(0m, 12).ToList();
                List<int> productsSold = Enumerable.Repeat(0, 12).ToList();
                List<int> months = Enumerable.Repeat(0, 12).ToList();


                var s_year = 0;
                var e_year = 0;
                var s_day = 1;
                var e_day = 31;
                var s_month = 1;
                var e_month = 12;


                if (SelectedYear == null && ReportStartDate.Day == DateTime.Now.Day && ReportEndDate.Day == DateTime.Now.Day)
                {
                    s_year = 0;
                    e_year = 0;
                }
                else if (SelectedYear != null)
                {
                    s_year = int.Parse(SelectedYear);
                    e_year = s_year;
                    s_day = 1;
                    e_day = 31;
                    s_month = 1;
                    e_month = 12;
                }
                else if (ReportEndDate != null && ReportStartDate != null)
                {
                    s_year = ReportStartDate.Year;
                    e_year = ReportEndDate.Year;
                    s_day = ReportStartDate.Day;
                    e_day = ReportEndDate.Day;
                    s_month = ReportStartDate.Month;
                    e_month = ReportEndDate.Month;
                }



                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = @"
                SELECT 
                    YEAR(ord.createDate) AS Year,
                    MONTH(ord.createDate) AS Month,
                    DAY(ord.createDate) AS Day,
                    p.product_name AS ProductName,
                    SUM(o.quantity) AS QuantitySold,
                    SUM(ord.total) AS TotalRevenue,
                    SUM(ord.profit) AS TotalProfit
                FROM 
                    [order] ord
                INNER JOIN 
                    orderdetails o ON ord.order_id = o.order_id 
	                INNER JOIN 
	                product p ON o.product_id = p.product_id
	                AND ord.[status] = 'Paid'
	                AND p.product_name = @productname
	                AND DAY(ord.createDate) between @startday and @endday
	                AND MONTH(ord.createDate) between @startmonth and @endmonth
	                AND YEAR (ord.createDate) between @startyear and @endyear
                GROUP BY 
                    YEAR(ord.createDate),
                    MONTH(ord.createDate),
                    DAY(ord.createDate),
                    p.product_name
                ORDER BY 
                    YEAR(ord.createDate),
                    MONTH(ord.createDate),
                    DAY(ord.createDate);

                ";


                    command.Parameters.Add("@startday", SqlDbType.Int).Value = s_day;
                    command.Parameters.Add("@endday", SqlDbType.Int).Value = e_day;
                    command.Parameters.Add("@startmonth", SqlDbType.Int).Value = s_month;
                    command.Parameters.Add("@endmonth", SqlDbType.Int).Value = e_month;
                    command.Parameters.Add("@startyear", SqlDbType.Int).Value = s_year;
                    command.Parameters.Add("@endyear", SqlDbType.Int).Value = e_year;
                    command.Parameters.Add("@productname", SqlDbType.NVarChar).Value = SelectedProduct.Name;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var revenue = reader.GetDecimal(reader.GetOrdinal("TotalRevenue"));
                            var profit = reader.GetDecimal(reader.GetOrdinal("TotalProfit"));
                            var month = reader.GetInt32(reader.GetOrdinal("Month"));
                            var sold = reader.GetInt32(reader.GetOrdinal("QuantitySold"));

                            months[month - 1] = month;
                            foreach (var item in months)
                            {
                                if (item != 0)
                                {
                                    revenueValues[item - 1] = revenue;
                                    profitValues[item - 1] = profit;
                                    productsSold[item - 1] = sold;
                                }
                            }
                        }
                    }

                    ProductSeriesCollection = new SeriesCollection()
                    {
                        new LineSeries
                        {
                            Title = "Revenue",
                            Values = new ChartValues<decimal>(revenueValues),
                            Stroke = new SolidColorBrush(Colors.Cyan),
                            StrokeThickness = 1
                        },

                        new LineSeries
                        {
                            Title = "Profit",
                            Values = new ChartValues<decimal>(profitValues),
                            Stroke = new SolidColorBrush(Colors.DeepPink),
                            StrokeThickness = 1
                        },

                        new LineSeries
                        {
                            Title = "Sold",
                            Values = new ChartValues<int>(productsSold),
                            Stroke = new SolidColorBrush(Colors.GreenYellow),
                            StrokeThickness = 1
                        }
                    };
                }
            }
            else
            {
                List<int> productsSold = new List<int>();
                List<string> Brands = new List<string>();

                var s_year = 0;
                var e_year = 0;
                var s_day = 1;
                var e_day = 31;
                var s_month = 1;
                var e_month = 12;


                if (SelectedYear == null && ReportStartDate == DateTime.Now)
                {
                    s_year = 0;
                    e_year = 0;
                }
                else if (SelectedYear != null)
                {
                    s_year = int.Parse(SelectedYear);
                    e_year = s_year;
                    s_day = 1;
                    e_day = 31;
                    s_month = 1;
                    e_month = 12;
                }
                else if (ReportEndDate != null && ReportStartDate != null)
                {
                    s_year = ReportStartDate.Year;
                    e_year = ReportEndDate.Year;
                    s_day = ReportStartDate.Day;
                    e_day = ReportEndDate.Day;
                    s_month = ReportStartDate.Month;
                    e_month = ReportEndDate.Month;
                }



                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = @"
                    SELECT c.collection_name AS brand, COALESCE(SUM(od.quantity), 0) AS total_sold
                    FROM [collection] c
                    LEFT JOIN Collection_List cl ON c.collection_id = cl.collection_id
                    LEFT JOIN OrderDetails od ON cl.product_id = od.product_id
                    LEFT JOIN [Order] o ON od.order_id = o.order_id
                    WHERE o.[status] = 'Paid'
                        AND DAY(o.createDate) BETWEEN @startday AND @endday
                        AND MONTH(o.createDate) BETWEEN @startmonth AND @endmonth
                        AND YEAR(o.createDate) BETWEEN @startyear AND @endyear
                    GROUP BY c.collection_name
                    ORDER BY total_sold desc

                ";


                    command.Parameters.Add("@startday", SqlDbType.Int).Value = s_day;
                    command.Parameters.Add("@endday", SqlDbType.Int).Value = e_day;
                    command.Parameters.Add("@startmonth", SqlDbType.Int).Value = s_month;
                    command.Parameters.Add("@endmonth", SqlDbType.Int).Value = e_month;
                    command.Parameters.Add("@startyear", SqlDbType.Int).Value = s_year;
                    command.Parameters.Add("@endyear", SqlDbType.Int).Value = e_year;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var sold = reader.GetInt32(reader.GetOrdinal("total_sold"));
                            var brand = reader["brand"].ToString();

                            Brands.Add(brand);
                            productsSold.Add(sold);

                        }
                    }



                    AllProductsSeriesCollection = new SeriesCollection();

                    for (int i = 0; i < Brands.Count; i++)
                    {
                        var columnSeries = new ColumnSeries
                        {
                            Title = Brands[i],
                            Values = new ChartValues<int> { productsSold[i] },
                            StrokeThickness = 1,
                            Stroke = new SolidColorBrush(Colors.White),
                            Fill = new SolidColorBrush(GetRandomColor())
                        };
                        AllProductsSeriesCollection.Add(columnSeries);
                    }
                }
            }
        }

        private System.Windows.Media.Color GetRandomColor()
        {
            Random random = new Random();
            return System.Windows.Media.Color.FromArgb(
                255,
                (byte)random.Next(0, 256),
                (byte)random.Next(0, 256),
                (byte)random.Next(0, 256)
            );
        }

        private void ExecuteBackFromProductStatisticsCommand(object obj)
        {
            StatisticsGridVisibility = Visibility.Visible;
            ProductStatisticsGridVisibility = Visibility.Collapsed;
        }

        private void ExecuteShowProductStatisticsCommand(object obj)
        {
            StatisticsGridVisibility = Visibility.Collapsed;
            ProductStatisticsGridVisibility = Visibility.Visible;
        }

        private void ExecuteResetDateCommand(object obj)
        {
            ReportStartDate = DateTime.Now;
            ReportEndDate = DateTime.Now;
            SelectedYear = null;
            SelectedProduct = null;
            DateInfo = " * Please select a year to see the revenue & profit in that year";
            ProductErrorMessage = "* Please select a product to see the statistic of that product";


            if (StatisticsGridVisibility == Visibility.Visible)
            {
                UpdaterChart();

            }
            if (ProductStatisticsGridVisibility == Visibility.Visible)
            {
                UpdateProductStatisticsChart();
            }
            
        }

        private void ExecuteSearchCommand(object obj)
        {
            DateInfo = $"* From {ReportStartDate} to {ReportEndDate}";
            if (ReportStartDate == null || ReportEndDate == null)
            {
                System.Windows.Forms.MessageBox.Show("* Please select the date and try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
            }
            else if(ReportStartDate > ReportEndDate)
            {
                System.Windows.MessageBox.Show("* Please try again", "Error", (MessageBoxButton)MessageBoxButtons.OK, (MessageBoxImage)MessageBoxIcon.Error);

            }
            else
            {
                if (StatisticsGridVisibility == Visibility.Visible)
                {
                    UpdaterChart();
                }
                if(ProductStatisticsGridVisibility == Visibility.Visible || AllProductStatisticsGridVisibility ==Visibility.Visible)
                {
                    UpdateProductStatisticsChart();
                }
            }
        }
    }
}
