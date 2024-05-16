using computershop.Model;
using Microsoft.Data.SqlClient;
using OfficeOpenXml.Drawing.Slicer.Style;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace computershop.ViewModel
{
    public class OrdersViewModel:ViewModelBase
    {
        private int totalItems;
        public int TotalItems
        {
            get
            {
                return totalItems;
            }
            set
            {
                totalItems = value;
                OnPropertyChanged(nameof(totalItems));
            }
        }

        private int currentPage;
        public int CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; OnPropertyChanged(nameof(currentPage)); }
        }
        public string CurrentPageDisplay => (CurrentPage + 1).ToString();

        private int totalPages;
        public int TotalPages
        {
            get { return totalPages; }
            set { totalPages = value; OnPropertyChanged(nameof(totalPages)); }
        }

        private string itemsPerPage;

        public string ItemsPerPage
        {
            get { return itemsPerPage; }
            set
            {
                if (itemsPerPage != value)
                {
                    itemsPerPage = value;
                    OnPropertyChanged(nameof(ItemsPerPage));

                    ItemsPerPageTextChanged(value);
                }
            }
        }
        private void ItemsPerPageTextChanged(string newValue)
        {
            if (string.IsNullOrEmpty(newValue))
            {
                ItemsPerPage = "0";
            }
            UpdateListView(items);
        }


        private ObservableCollection<OrderModel> listOrders;
        public ObservableCollection<OrderModel> ListOrders
        {
            get { return listOrders; }
            set { listOrders = value; OnPropertyChanged(nameof(ListOrders)); }
        }


        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
        private DateTime endDate;
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        private Visibility orderGridVisibility = Visibility.Visible;
        public Visibility OrderGridVisibility
        {
            get { return orderGridVisibility; }
            set
            {
                orderGridVisibility = value;
                OnPropertyChanged(nameof(OrderGridVisibility));
            }
        }

        private Visibility addOrderGridVisibility = Visibility.Collapsed;
        public Visibility AddOrderGridVisibility
        {
            get { return addOrderGridVisibility; }
            set
            {
                addOrderGridVisibility = value;
                OnPropertyChanged(nameof(AddOrderGridVisibility));
            }
        }
        private Visibility updateOrderGridVisibility = Visibility.Collapsed;
        public Visibility UpdateOrderGridVisibility
        {
            get { return updateOrderGridVisibility; }
            set
            {
                updateOrderGridVisibility = value;
                OnPropertyChanged(nameof(UpdateOrderGridVisibility));
            }
        }
        private Visibility selectProductGridVisibility = Visibility.Collapsed;
        public Visibility SelectProductGridVisibility
        {
            get { return selectProductGridVisibility; }
            set
            {
                selectProductGridVisibility = value;
                OnPropertyChanged(nameof(SelectProductGridVisibility));
            }
        }
        private Visibility productDetailGridVisibility = Visibility.Collapsed;
        public Visibility ProductDetailGridVisibility
        {
            get { return productDetailGridVisibility; }
            set
            {
                productDetailGridVisibility = value;
                OnPropertyChanged(nameof(ProductDetailGridVisibility));
            }
        }

        private string newCusName;
        public string NewCusName {
            get
            {
                return newCusName;
            }
            set
            {
                newCusName = value;
                OnPropertyChanged(nameof(NewCusName));
            }
        }

        private string newCusPhone;
        public string NewCusPhone
        {
            get
            {
                return newCusPhone;
            }
            set
            {
                newCusPhone = value;
                OnPropertyChanged(nameof(NewCusPhone));
            }
        }

        private string newCusEmail;
        public string NewCusEmail
        {
            get
            {
                return newCusEmail;
            }
            set
            {
                newCusEmail = value;
                OnPropertyChanged(nameof(NewCusEmail));
            }
        }

        private string newCusAddress;
        public string NewCusAddress
        {
            get
            {
                return newCusAddress;
            }
            set
            {
                newCusAddress = value;
                OnPropertyChanged(nameof(NewCusAddress));
            }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                errorMessage = value;
                OnPropertyChanged (nameof(ErrorMessage));
            }
        }
        private string successMessage;
        public string SuccessMessage
        {
            get
            {
                return successMessage;
            }
            set
            {
                successMessage = value;
                OnPropertyChanged (nameof(SuccessMessage));
            }
        }

        private DateTime selectedOrderCreatedDate;
        public DateTime SelectedOrderCreatedDate
        {
            get
            {
                return selectedOrderCreatedDate;
            }
            set
            {
                selectedOrderCreatedDate = value;
                OnPropertyChanged(nameof(SelectedOrderCreatedDate));
                OnPropertyChanged(nameof(IsDateSelected));
            }
        }
        public bool IsDateSelected
        {
            get { return selectedOrderCreatedDate != null; }

        }
    

        private string selectedOrderStatus;
        public string SelectedOrderStatus
        {
            get
            {
                return selectedOrderStatus;
            }
            set
            {
                selectedOrderStatus = value;
                OnPropertyChanged(nameof(SelectedOrderStatus));
            }
        }

        private CustomerModel customer;
        public CustomerModel Customer
        {
            get {
                return Customer;
            }
            set
            {
                Customer = value;
                OnPropertyChanged(nameof(Customer));
            }
        }

        private string existedPhoneMessage;
        public string ExistedPhoneMessage
        {
            get
            {
                return existedPhoneMessage;
            }
            set
            {
                existedPhoneMessage = value;
                OnPropertyChanged(nameof(ExistedPhoneMessage));
            }
        }
        private string existedEmailMessage;
        public string ExistedEmailMessage
        {
            get
            {
                return existedEmailMessage;
            }
            set
            {
                existedEmailMessage = value;
                OnPropertyChanged(nameof(ExistedEmailMessage));
            }
        }

        private OrderModel selectedItem;
        public OrderModel SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                UpdateIsQuantityEnabled();
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        private string selectedOrderDetailStatus;
        public string SelectedOrderDetailStatus
        {
            get
            {
                return selectedOrderDetailStatus;
            }
            set
            {
                selectedOrderDetailStatus = value;
                OnPropertyChanged(nameof(SelectedOrderDetailStatus));
            }
        }

        private int customerId;
        public int CustomerId {
            get
            {
                return customerId;
            }
            set
            {
                customerId = value;
                OnPropertyChanged(nameof(CustomerId));
            }
        }
        private string customerName;
        public string CustomerName
        {
            get
            {
                return customerName;
            }
            set
            {
                customerName = value;
                OnPropertyChanged(nameof(CustomerName));
            }
        }
        private string customerEmail;
        public string CustomerEmail
        {
            get
            {
                return customerEmail;
            }
            set
            {
                customerEmail = value;
                OnPropertyChanged(nameof(CustomerEmail));
            }
        }
        private string customerPhone;
        public string CustomerPhone
        {
            get
            {
                return customerPhone;
            }
            set
            {
                customerPhone = value;
                OnPropertyChanged(nameof(CustomerPhone));
            }
        }
        private string customerAddress;
        public string CustomerAddress
        {
            get
            {
                return customerAddress;
            }
            set
            {
                customerAddress = value;
                OnPropertyChanged(nameof(CustomerAddress));
            }
        }

        public DateTime customerOrderDate;
        public DateTime CustomerOrderDate
        {
            get
            {
                return customerOrderDate;
            }
            set
            {
                customerOrderDate = value;
                OnPropertyChanged(nameof(CustomerOrderDate));
            }
        }
        
        public int orderID;
        public int OrderID
        {
            get
            {
                return orderID;
            }
            set
            {
                orderID = value;
                OnPropertyChanged(nameof(OrderID));
            }
        }


        private ObservableCollection<ProductModel> productsInOrder;
        public ObservableCollection<ProductModel> ProductsInOrder
        {
            get { return productsInOrder; }
            set { productsInOrder = value; OnPropertyChanged(nameof(ProductsInOrder)); }
        }

        private decimal totalPrice;
        public decimal TotalPrice
        {
            get
            {
                return totalPrice;
            }
            set
            {
                totalPrice = value;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }
        private decimal totalProfit;
        public decimal TotalProfit
        {
            get
            {
                return totalProfit;
            }
            set
            {
                totalProfit = value;
                OnPropertyChanged(nameof(TotalProfit));
            }
        }

        private ObservableCollection<ProductModel> listProducts;
        public ObservableCollection<ProductModel> ListProducts
        {
            get { return listProducts; }
            set { listProducts = value; OnPropertyChanged(nameof(ListProducts)); }
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

        private string selectedProductName;
        public string SelectedProductName
        {
            get { return selectedProductName; }
            set
            {
                selectedProductName = value;
                OnPropertyChanged(nameof(SelectedProductName));
            }
        }
        private int selectedProductQuantity;
        public int SelectedProductQuantity
        {
            get { return selectedProductQuantity; }
            set
            {
                selectedProductQuantity = value;
                OnPropertyChanged(nameof(SelectedProductQuantity));
            }
        }
        private decimal selectedProductPrice;
        public decimal SelectedProductPrice
        {
            get { return selectedProductPrice; }
            set
            {
                selectedProductPrice = value;
                OnPropertyChanged(nameof(SelectedProductPrice));
            }
        }
        private string selectedProductImage;
        public string SelectedProductImage
        {
            get { return selectedProductImage; }
            set
            {
                selectedProductImage = value;
                OnPropertyChanged(nameof(SelectedProductImage));
            }
        }

        private string selectedProductStatus;
        public string SelectedProductStatus
        {
            get { return selectedProductStatus; }
            set
            {
                selectedProductStatus = value;
                OnPropertyChanged(nameof(SelectedProductStatus));
            }
        }

        private string productQuantity;
        public string ProductQuantity
        {
            get
            {
                return productQuantity;
            }
            set { 
                productQuantity = value;
                OnPropertyChanged(nameof(ProductQuantity));
            }
        }

        private bool _isQuantityEnabled = true;

        public bool IsQuantityEnabled
        {
            get { return _isQuantityEnabled; }
            set
            {
                _isQuantityEnabled = value;
                OnPropertyChanged(nameof(IsQuantityEnabled)); 
            }
        }
        private void UpdateIsQuantityEnabled()
        {
            IsQuantityEnabled = SelectedItem?.Status != "Not Available";
        }

        private bool _isOrderStatusSelectionEnabled;
        public bool IsOrderStatusSelectionEnabled
        {
            get { return _isOrderStatusSelectionEnabled; }
            set
            {
                if (_isOrderStatusSelectionEnabled != value)
                {
                    _isOrderStatusSelectionEnabled = value;
                    OnPropertyChanged(nameof(IsOrderStatusSelectionEnabled));
                }
            }
        }
        private bool _isPromotionsSelectionEnabled;
        public bool IsPromotionsSelectionEnabled
        {
            get { return _isPromotionsSelectionEnabled; }
            set
            {
                if (_isPromotionsSelectionEnabled != value)
                {
                    _isPromotionsSelectionEnabled = value;
                    OnPropertyChanged(nameof(IsPromotionsSelectionEnabled));
                }
            }
        }

        private bool _isAddButtonEnabled;
        public bool IsAddButtonEnabled
        {
            get { return _isAddButtonEnabled; }
            set
            {
                if (_isAddButtonEnabled != value)
                {
                    _isAddButtonEnabled = value;
                    OnPropertyChanged(nameof(IsAddButtonEnabled));
                }
            }
        }

        ObservableCollection<OrderModel> items = new ObservableCollection<OrderModel>();

        ObservableCollection<CustomerModel> customers = new ObservableCollection<CustomerModel>();

        ObservableCollection<OrderModel> searchItems;

        ObservableCollection<ProductModel> selectedProducts = new ObservableCollection<ProductModel>();

        ObservableCollection<string> orderStatus;

        public ObservableCollection<string> OrderStatus
        {
            get { return orderStatus; }
            set { orderStatus = value; OnPropertyChanged(nameof(OrderStatus)); }
        }
        private void LoadStatus()
        {
            OrderStatus = new ObservableCollection<string>() {
                "Pending",
                "Paid",
                "Cancelled",
            };
        }
        private enum Pages
        {
            Main,
            Actions
        }
        private int _type = (int)Pages.Main;

        private ObservableCollection<CustomerModel> LoadCustomer()
        {
            var CusList = new ObservableCollection<CustomerModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from [customer]";



                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CustomerModel customer = new CustomerModel()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("customer_id")),
                            CustomerName = reader["customer_name"].ToString(),
                            CustomerEmail = reader["customer_email"].ToString(),
                            CustomerPhone = reader["customer_phone"].ToString(),
                            CustomerAddress = reader["customer_address"].ToString(),
                        };

                        CusList.Add(customer);
                    }
                }
            }

            return CusList;
        }



        private readonly string _connectionString = @"
                Server=.;
                Database = LoginDB;
                Trusted_Connection = yes;
                TrustServerCertificate = True
                ";



        //Commands
        public ICommand SearchCommand { get; set; }
        public ICommand ResetDateCommand { get; set; }
        public ICommand ShowAddViewCommand { get; set; }
        public ICommand AddOrderCommand { get; set; }
        public ICommand BackFromAddView { get; set; }
        public ICommand ShowUpdateViewCommand { get; set; }
        public ICommand BackFromUpdateView { get; set; }
        public ICommand ShowAddProductViewCommand { get; set; }
        public ICommand ViewProductDetailCommand { get; set; }
        public ICommand AddProductToOrderCommand { get; set; }
        public ICommand UpdateOrderDetailCommmand { get; set; }
        public ICommand DeleteOrderCommand { get; set; }
        public ICommand DeleteProductsInOrderCommand { get; set; }
        public ICommand BackFromSelectProductView { get; set; }
        public ICommand PrevPageCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
       

        public OrdersViewModel()
        {
            items = LoadData();
            customers = LoadCustomer();
            ItemsPerPage = "10";
            IsOrderStatusSelectionEnabled = true;
            IsPromotionsSelectionEnabled = true;
            IsAddButtonEnabled = true;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            UpdateListView(items);

            

            SearchCommand = new ViewModelCommand(ExecuteSearchCommand);

            ResetDateCommand = new ViewModelCommand(ExecuteResetDateCommand);

            ShowAddViewCommand = new ViewModelCommand(ExecuteShowAddViewCommand);

            AddOrderCommand = new ViewModelCommand(ExecuteAddOrderCommand);

            BackFromAddView = new ViewModelCommand(ExecuteBackFromAddView);

            ShowUpdateViewCommand = new ViewModelCommand(ExecuteShowUpdateViewCommand);

            BackFromUpdateView = new ViewModelCommand(ExecuteBackFromUpdateView);

            ShowAddProductViewCommand = new ViewModelCommand(ExecuteShowAddProductViewCommand);

            ViewProductDetailCommand = new ViewModelCommand(ExecuteViewProductDetailCommand);

            AddProductToOrderCommand = new ViewModelCommand(ExecuteAddProductToOrderCommand);

            UpdateOrderDetailCommmand = new ViewModelCommand(ExecuteUpdateOrderDetailCommmand);

            DeleteOrderCommand = new ViewModelCommand(ExecuteDeleteOrderCommand);

            DeleteProductsInOrderCommand = new ViewModelCommand(ExecuteDeleteProductsInOrderCommand);

            BackFromSelectProductView = new ViewModelCommand(ExecuteBackFromSelectProductView);

            PrevPageCommand = new ViewModelCommand(ExecutePrevPageCommand);

            NextPageCommand = new ViewModelCommand(ExecuteNextPageCommand);
        }

        

        private void ExecuteViewProductDetailCommand(object obj)
        {
            ProductDetailGridVisibility = Visibility.Visible;
            SelectedProductName = SelectedProduct.Name;
            SelectedProductQuantity = SelectedProduct.Quantity;
            SelectedProductPrice = SelectedProduct.SalePrice;
            SelectedProductImage = SelectedProduct.Avatar;
            SelectedProductStatus = SelectedProduct.Status;
            ProductQuantity = "1";

        }


        private void ExecuteAddProductToOrderCommand(object obj)
        {
            try
            {
                if (int.Parse(ProductQuantity) > SelectedProduct.Quantity)
                {
                    System.Windows.MessageBox.Show("* Invalid quantity, please try again", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    ProductModel newItem = new ProductModel()
                    {
                        Name = SelectedProduct.Name,
                        SalePrice = SelectedProduct.SalePrice,
                        Quantity = int.Parse(ProductQuantity),
                        Avatar = SelectedProductImage,
                        Id = SelectedProduct.Id,
                        Capital = SelectedProduct.Capital,
                        Description = SelectedProduct.Description,
                        Brand = SelectedProduct.Brand,
                        Status = SelectedProduct.Status,
                    };


                    foreach (var item in items)
                    {
                        if (item.OrderID == SelectedItem.OrderID)
                        {
                            item.ordProductList.Add(newItem);
                            try
                            {
                                using (var connection = GetConnection())
                                using (var command = new SqlCommand())
                                {
                                    connection.Open();
                                    command.Connection = connection;
                                    command.CommandText = "InsertOrderDetails";
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.Parameters.AddWithValue("@order_id", item.OrderID);
                                    command.Parameters.AddWithValue("@product_id", newItem.Id);
                                    command.Parameters.AddWithValue("@quantity", newItem.Quantity);
                                    command.Parameters.AddWithValue("@saleprice", newItem.SalePrice);
                                    command.ExecuteNonQuery();
                                }
                                System.Windows.MessageBox.Show("* Add to order successfully", "Announcement", MessageBoxButton.OK, MessageBoxImage.Information);
                                RefreshOrderProductList();
                                ExecuteBackFromSelectProductView(SelectedItem);
                            }
                            catch (Exception ex)
                            {
                                System.Windows.MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        private ObservableCollection<OrderModel> LoadData()
        {
            var OrderList  = new ObservableCollection<OrderModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from [order] , [customer]  where cus_id = customer_id";



                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        OrderModel order = new OrderModel()
                        {
                            ordProductList = new ObservableCollection<ProductModel>(),
                            OrderID = reader.GetInt32(reader.GetOrdinal("order_id")),
                            CustomerID = reader.GetInt32(reader.GetOrdinal("cus_id")),
                            CustomerName = reader["customer_name"].ToString(),
                            CustomerPhone = reader["customer_phone"].ToString(),
                            CustomerEmail = reader["customer_email"].ToString(),
                            CustomerAddress = reader["customer_address"].ToString(),
                            CreateDate = reader.GetDateTime(reader.GetOrdinal("createdate")),
                            Total = reader.GetDecimal(reader.GetOrdinal("total")),
                            Profit = reader.GetDecimal(reader.GetOrdinal("profit")),
                            Status = reader["status"].ToString()
                        };

                        OrderList.Add(order);
                    }
                }
            }

            return OrderList;
        }

        private void UpdateListView(ObservableCollection<OrderModel> list)
        {
            var itemsToShow = list.Skip(CurrentPage * int.Parse(itemsPerPage)).Take(int.Parse(itemsPerPage)).ToList();
            ListOrders = new ObservableCollection<OrderModel>(itemsToShow);
            TotalPages = (int)Math.Ceiling((double)list.Count() / 10);
            TotalItems = list.Count;
        }

        private void ExecuteSearchCommand(object obj)
        {
            searchItems = new ObservableCollection<OrderModel>();

            if (startDate == null || endDate == null)
            {
                _type = (int)Pages.Main;

                System.Windows.Forms.MessageBox.Show("* Please select the date and try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
            }

            else if (startDate <= endDate)
            {
                _type = (int)Pages.Actions;

                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from [order], [customer] where cus_id = customer_id and CreateDate between @startDate and @endDate";
                    command.Parameters.Add("@startDate", SqlDbType.DateTime).Value = startDate;
                    command.Parameters.Add("@endDate", SqlDbType.DateTime).Value = endDate;


                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrderModel order = new OrderModel()
                            {
                                OrderID = reader.GetInt32(reader.GetOrdinal("order_id")),
                                CustomerID = reader.GetInt32(reader.GetOrdinal("cus_id")),
                                CustomerName = reader["customer_name"].ToString(),
                                CreateDate = reader.GetDateTime(reader.GetOrdinal("createdate")),
                                Total = reader.GetDecimal(reader.GetOrdinal("total")),
                                Profit = reader.GetDecimal(reader.GetOrdinal("profit")),
                                Status = reader["status"].ToString()
                            };
                            searchItems.Add(order);
                        }
                    }
                }

                UpdateListView(searchItems);
            }

            else
            {
                System.Windows.MessageBox.Show("* Please try again", "Error", (MessageBoxButton)MessageBoxButtons.OK, (MessageBoxImage)MessageBoxIcon.Error);
            }
        }

        private void ExecuteResetDateCommand(object obj)
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            UpdateListView(items);
        }

        private void ExecuteShowAddViewCommand(object obj)
        {
            OrderGridVisibility = Visibility.Collapsed;
            AddOrderGridVisibility = Visibility.Visible;
            LoadStatus();
            SelectedOrderCreatedDate = DateTime.Now;
            ErrorMessage = null;
            SuccessMessage = null;
            NewCusName = null;
            NewCusEmail = null;
            NewCusAddress = null;
            NewCusPhone = null;
        }

        public bool CheckExistedOrder(string cus_name, string cus_phone)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [order], [customer] WHERE cus_id = customer_id AND customer_name LIKE '%' + @cusName + '%' AND status = @orderStatus and customer_phone = @phone";
                command.Parameters.Add("@cusName", SqlDbType.NVarChar).Value = cus_name;
                command.Parameters.Add("@orderStatus", SqlDbType.NVarChar).Value = "Pending";
                command.Parameters.Add("@phone", SqlDbType.NVarChar).Value = cus_phone;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return true;
                    }
                    return false;
                }
            }
        }


        public bool CheckExistedPhoneAndEmail(string cusName, string cusPhone, string cusEmail)
        {
            bool isExisted = false;
            foreach (var item in customers)
            {
                if (item.CustomerPhone == cusPhone)
                {
                    if (item.CustomerName != cusName)
                    {
                        ExistedPhoneMessage = "* This phone number has been used";
                        isExisted = true;
                    }
                }
                if (item.CustomerEmail == cusEmail)
                {
                    if (item.CustomerName != cusName)
                    {
                        ExistedEmailMessage = "* This email has been used";
                        isExisted = true;
                    }
                }
            }
            return isExisted;
        }

        private void ExecuteAddOrderCommand(object obj)
        {
            if (string.IsNullOrEmpty(NewCusName) || string.IsNullOrEmpty(NewCusPhone) || string.IsNullOrEmpty(NewCusEmail) || string.IsNullOrEmpty(NewCusAddress) || SelectedOrderStatus == null || IsDateSelected == false)
            {
                ErrorMessage = "* Please provide full information to add";
            }
            else if (!CheckExistedOrder(NewCusName, NewCusPhone))
            {
                ExistedPhoneMessage = null;
                ExistedEmailMessage = null;

                if (!CheckExistedPhoneAndEmail(NewCusName, NewCusPhone, NewCusEmail))
                {
                    AddNewCustomer();

                    OrderModel newOrder = new OrderModel();
                    foreach (var item in customers)
                    {
                        if (item.CustomerPhone == NewCusPhone)
                        {
                            newOrder.CustomerID = item.Id;
                        }
                    }
                    newOrder.CreateDate = SelectedOrderCreatedDate;
                    newOrder.CustomerName = NewCusName;
                    newOrder.Total = 0;
                    newOrder.Status = SelectedOrderStatus;

                    using (var connection = GetConnection())
                    using (var command = new SqlCommand())
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO [order] VALUES (@cusID, @date, @total, @profit, @status)";
                        command.Parameters.Add("@cusID", SqlDbType.NVarChar).Value = newOrder.CustomerID;
                        command.Parameters.Add("@date", SqlDbType.DateTime).Value = newOrder.CreateDate;
                        command.Parameters.Add("@total", SqlDbType.Decimal).Value = newOrder.Total;
                        command.Parameters.Add("@profit", SqlDbType.Decimal).Value = newOrder.Profit;
                        command.Parameters.Add("@status", SqlDbType.NVarChar).Value = newOrder.Status;
                        command.ExecuteNonQuery();
                    }

                    items.Add(newOrder);
                    items = LoadData();
                    UpdateListView(items);
                    SuccessMessage = $"* Order has been added successfully";
                }
            }
            else
            {
                ErrorMessage = $"* Order is still pending, please try again";
            }
        }

        private void AddNewCustomer()
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO [customer] VALUES (@cusName,@cusEmail,@cusAddress, @cusPhone)";
                command.Parameters.Add("@cusName", SqlDbType.NVarChar).Value = NewCusName;
                command.Parameters.Add("@cusEmail", SqlDbType.NVarChar).Value = NewCusEmail;
                command.Parameters.Add("@cusPhone", SqlDbType.NVarChar).Value = NewCusPhone;
                command.Parameters.Add("@cusAddress", SqlDbType.NVarChar).Value = NewCusAddress;
                command.ExecuteNonQuery();

            }

            customers = LoadCustomer();
        }
        private void ExecuteDeleteOrderCommand(object obj)
        {
            if (SelectedItem != null)
            {
                var result = System.Windows.MessageBox.Show($"Are you sure you want to delete the order #{SelectedItem.OrderID}?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (var connection = GetConnection())
                    using (var command = new SqlCommand())
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = "DeleteOrder";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@order_id", SelectedItem.OrderID);
                        command.ExecuteNonQuery();
                    }
                    System.Windows.MessageBox.Show("* Order has been deleted successfully", "Announcement", (MessageBoxButton)MessageBoxButtons.OK);
                    items = LoadData();
                    UpdateListView(items);

                }
            }
            else
            {
                System.Windows.MessageBox.Show("* Please select an order to delete", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ExecuteDeleteProductsInOrderCommand(object obj)
        {
            if (obj is ProductModel SelectedItem)
            {
                var result = System.Windows.MessageBox.Show($"Are you sure you want to delete the product '{SelectedItem.Name}'?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (var connection = GetConnection())
                    using (var command = new SqlCommand())
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = "DeleteProductAndUpdateOrder";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@order_id", OrderID);
                        command.Parameters.AddWithValue("@product_id", SelectedItem.Id);
                        command.ExecuteNonQuery();
                    }
                    RefreshOrderProductList();

                    items = LoadData();
                    UpdateListView(items);
                }
            }
        }
        private void ExecuteBackFromAddView(object obj)
        {
            OrderGridVisibility = Visibility.Visible;
            AddOrderGridVisibility = Visibility.Collapsed;
        }

        private void ExecuteBackFromUpdateView(object obj)
        {
            OrderGridVisibility = Visibility.Visible;
            UpdateOrderGridVisibility = Visibility.Collapsed;
        }

        private void ExecuteUpdateOrderDetailCommmand(object obj)
        {
            try
            { 
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "UpdateOrderAndCustomer";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@order_id", OrderID);
                    command.Parameters.AddWithValue("@customer_name", CustomerName);
                    command.Parameters.AddWithValue("@customer_email", CustomerEmail);
                    command.Parameters.AddWithValue("@customer_address", CustomerAddress);
                    command.Parameters.AddWithValue("@create_date", CustomerOrderDate);
                    command.Parameters.AddWithValue("@status", SelectedOrderDetailStatus);
                    command.Parameters.AddWithValue("@total", TotalPrice);
                    command.Parameters.AddWithValue("@profit", TotalProfit);
                    command.ExecuteNonQuery();
                }

                items = LoadData();
                UpdateListView(items);
                System.Windows.MessageBox.Show("* Successfully", "Announcement", MessageBoxButton.OK, MessageBoxImage.Information);
                ExecuteBackFromUpdateView(null);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            

        }

        private void RefreshOrderProductList()
        {
            ProductsInOrder = new ObservableCollection<ProductModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = """
                    select * 
                    from [orderdetails] o , [product] p , [collection] c
                    where o.product_id = p.product_id 
                    	and LEFT(p.product_name, CHARINDEX(' ', p.product_name) - 1) = c.collection_name
                    	and o.order_id = @id
                    """;
                command.Parameters.Add("@id", SqlDbType.Int).Value = SelectedItem.OrderID;

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
                            SalePrice = reader.GetDecimal(reader.GetOrdinal("total")),
                            Status = reader["status"].ToString(),
                            Brand = reader["collection_name"].ToString(),
                            Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                            Avatar = reader["avatar"].ToString()
                        };

                        ProductsInOrder.Add(product);
                    }
                }

            }

            decimal total = 0;
            decimal capital = 0;
            foreach (var item in ProductsInOrder)
            {
                total += item.SalePrice;
                capital += item.Quantity*item.Capital;
            }
            TotalPrice = total;
            TotalProfit = total - capital;
        }

        private void ExecuteShowUpdateViewCommand(object obj)
        {

            if (SelectedItem != null)
            {
                LoadStatus();
                OrderGridVisibility = Visibility.Collapsed;
                UpdateOrderGridVisibility = Visibility.Visible;
                OrderID = SelectedItem.OrderID;
                CustomerId = SelectedItem.CustomerID;
                CustomerName = SelectedItem.CustomerName;
                CustomerEmail = SelectedItem.CustomerEmail;
                CustomerPhone = SelectedItem.CustomerPhone;
                CustomerAddress = SelectedItem.CustomerAddress;
                CustomerOrderDate = SelectedItem.CreateDate;
                SelectedOrderDetailStatus = SelectedItem.Status;

                if (SelectedOrderDetailStatus == "Paid")
                {
                    IsOrderStatusSelectionEnabled = false;
                    IsPromotionsSelectionEnabled = false;
                    IsAddButtonEnabled = false;
                }
                else
                {
                    IsOrderStatusSelectionEnabled = true;
                    IsPromotionsSelectionEnabled = true;
                    IsAddButtonEnabled = true;
                }
                RefreshOrderProductList();
                
            }
            else
            {
                System.Windows.MessageBox.Show("* Please select an order to edit", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteShowAddProductViewCommand(object obj)
        {
            UpdateOrderGridVisibility = Visibility.Collapsed;
            SelectProductGridVisibility = Visibility.Visible;
            LoadProducts();
            
        }

        private void LoadProducts()
        {
            ListProducts = new ObservableCollection<ProductModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = """
                    SELECT *
                    FROM 
                        product p
                    JOIN 
                        [Collection] c ON LEFT(p.product_name, CHARINDEX(' ', p.product_name) - 1) = c.collection_name;
                    """;

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
                            Brand = reader["collection_name"].ToString(),
                            Quantity = reader.GetInt32(reader.GetOrdinal("stock")),
                            Avatar = reader["avatar"].ToString()
                        };

                        ListProducts.Add(product);
                    }
                }

            }
        }

        private void ExecuteBackFromSelectProductView(object obj)
        {
            UpdateOrderGridVisibility = Visibility.Visible;
            SelectProductGridVisibility = Visibility.Collapsed;
        }

        private void ExecuteNextPageCommand(object obj)
        {
            if (CurrentPage < TotalPages - 1)
            {
                CurrentPage++;
                if (_type == 0)
                {
                    UpdateListView(items);
                }
                else
                {
                    UpdateListView(searchItems);
                }
            }
        }

        private void ExecutePrevPageCommand(object obj)
        {
            if (CurrentPage > 0)
            {
                CurrentPage--;
                if (_type == 0)
                {
                    UpdateListView(items);
                }
                else
                {
                    UpdateListView(searchItems);
                }
            }
        }
    }
}
