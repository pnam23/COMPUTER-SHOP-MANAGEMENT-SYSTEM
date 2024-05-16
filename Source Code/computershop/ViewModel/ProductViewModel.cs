using computershop.Model;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static System.Runtime.InteropServices.JavaScript.JSType;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace computershop.ViewModel
{
    public class ProductViewModel : ViewModelBase
    {
        private int totalItems;
        private int currentPage;
        private int totalPages;
        private ObservableCollection<ProductModel> listProducts;
        private string itemsPerPage;
        ObservableCollection<ProductModel> items = new ObservableCollection<ProductModel>();
        ObservableCollection<ProductModel> searchItems = new ObservableCollection<ProductModel>();
        private ObservableCollection<BrandModel> brands;
        private ObservableCollection<string> filters;
        private ObservableCollection<string> productStatus;
        private string nameOfProduct;
        private string errorMessage;
        private BrandModel selectedBrand;
        private string selectedFilter;
        private string newProductName;
        private string newProductDescription;
        private int newProductQuantity;
        private decimal newProductCapital;
        private decimal newProductSalePrice;
        private string selectedImageSource;
        private BrandModel selectedProductBrand;
        private string selectedProductStatus;
        private string currentName;
        private string currentDescription;
        private string currentImageSource;
        private string currentBrand;
        private string currentStatus;
        private int currentQuantity;
        private decimal currentCapital;
        private decimal currentSalePrice;
        private ProductModel selectedItem;


        private readonly string _connectionString = @"
                Server=.;
                Database = LoginDB;
                Trusted_Connection = yes;
                TrustServerCertificate = True
                ";

        
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
        public int CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; OnPropertyChanged(nameof(currentPage)); }
        }

        public string CurrentPageDisplay => (CurrentPage + 1).ToString();


        public int TotalPages
        {
            get { return totalPages; }
            set { totalPages = value; OnPropertyChanged(nameof(totalPages)); }
        }


        public ObservableCollection<ProductModel> ListProducts
        {
            get { return listProducts; }
            set { listProducts = value; OnPropertyChanged(nameof(listProducts)); }
        }
        public ObservableCollection<BrandModel> Brands
        {
            get { return brands; }
            set { brands = value; OnPropertyChanged(nameof(Brands)); }
        }
        public ObservableCollection<string> Filters
        {
            get { return filters; }
            set { filters = value; OnPropertyChanged(nameof(Filters)); }
        }
        public ObservableCollection<string> ProductStatus
        {
            get { return productStatus; }
            set { productStatus = value; OnPropertyChanged(nameof(ProductStatus)); }
        }

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

        public string NameOfProduct
        {
            get { return nameOfProduct; }
            set
            {
                nameOfProduct = value;
                OnPropertyChanged(nameof(NameOfProduct));
            }
        }
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }


        private Visibility _productGridVisibility = Visibility.Visible;
        public Visibility ProductGridVisibility
        {
            get { return _productGridVisibility; }
            set
            {
                _productGridVisibility = value;
                OnPropertyChanged(nameof(ProductGridVisibility));
            }
        }

        private Visibility _addProductGridVisibility = Visibility.Collapsed;
        public Visibility AddProductGridVisibility
        {
            get { return _addProductGridVisibility; }
            set
            {
                _addProductGridVisibility = value;
                OnPropertyChanged(nameof(AddProductGridVisibility));
            }
        }
        private Visibility _updateProductGridVisibility = Visibility.Collapsed;
        public Visibility UpdateProductGridVisibility
        {
            get { return _updateProductGridVisibility; }
            set
            {
                _updateProductGridVisibility = value;
                OnPropertyChanged(nameof(UpdateProductGridVisibility));
            }
        }

        private enum Pages
        {
            Main,
            Actions
        }

        private int _type = (int)Pages.Main;

        public BrandModel SelectedBrand
        {
            get { return selectedBrand; }
            set
            {
                selectedBrand = value;
                OnPropertyChanged(nameof(SelectedBrand));
            }
        }
        public string SelectedFilter
        {
            get { return selectedFilter; }
            set
            {
                selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
            }
        }

        public string NewProductName
        {
            get { return newProductName; }
            set
            {
                newProductName = value;
                OnPropertyChanged(nameof(NewProductName));
            }

        }
        public string NewProductDescription
        {
            get { return newProductDescription; }
            set
            {
                newProductDescription = value;
                OnPropertyChanged(nameof(NewProductDescription));
            }

        }
        public int NewProductQuantity
        {
            get { return newProductQuantity; }
            set
            {
                newProductQuantity = value;
                OnPropertyChanged(nameof(NewProductQuantity));
            }

        }
        public decimal NewProductCapital
        {
            get { return newProductCapital; }
            set
            {
                newProductCapital = value;
                OnPropertyChanged(nameof(NewProductCapital));
            }

        }
        public decimal NewProductSalePrice
        {
            get { return newProductSalePrice; }
            set
            {
                newProductSalePrice = value;
                OnPropertyChanged(nameof(NewProductSalePrice));
            }

        }
        public string SelectedImageSource
        {
            get { return selectedImageSource; }
            set
            {
                selectedImageSource = value;
                OnPropertyChanged(nameof(SelectedImageSource));
            }
        }

        public BrandModel SelectedProductBrand
        {
            get { return selectedProductBrand; }
            set
            {
                selectedProductBrand = value;
                OnPropertyChanged(nameof(SelectedProductBrand));
            }
        }
        public string SelectedProductStatus
        {
            get { return selectedProductStatus; }
            set
            {
                selectedProductStatus = value;
                OnPropertyChanged(nameof(SelectedProductStatus));
            }
        }


        public string CurrentName {
            get
            {
                return currentName;
            }
            set
            {
                currentName = value;
                OnPropertyChanged(nameof(CurrentName));
            }
        }
        public string CurrentDescription
        {
            get
            {
                return currentDescription;
            }
            set
            {
                currentDescription = value;
                OnPropertyChanged(nameof(CurrentDescription));
            }
        }
        public string CurrentImageSource
        {
            get
            {
                return currentImageSource;
            }
            set
            {
                currentImageSource = value;
                OnPropertyChanged(nameof(CurrentImageSource));
            }
        }
        public string CurrentBrand
        {
            get
            {
                return currentBrand;
            }
            set
            {
                currentBrand = value;
                OnPropertyChanged(nameof(CurrentBrand));
            }
        }
        public string CurrentStatus
        {
            get
            {
                return currentStatus;
            }
            set
            {
                currentStatus = value;
                OnPropertyChanged(nameof(CurrentStatus));
            }
        }
        public int CurrentQuantity
        {
            get
            {
                return currentQuantity;
            }
            set
            {
                currentQuantity = value;
                OnPropertyChanged(nameof(CurrentQuantity));
            }
        }
        public decimal CurrentCapital
        {
            get
            {
                return currentCapital;
            }
            set
            {
                currentCapital = value;
                OnPropertyChanged(nameof(CurrentCapital));
            }
        }
        public decimal CurrentSalePrice
        {
            get
            {
                return currentSalePrice;
            }
            set
            {
                currentSalePrice = value;
                OnPropertyChanged(nameof(CurrentSalePrice));
            }
        }

        public ProductModel SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private void LoadBrands()
        {
            Brands = new ObservableCollection<BrandModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [collection]";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BrandModel brand = new BrandModel()
                        {
                            ID = reader.GetInt32(reader.GetOrdinal("collection_id")),
                            Name = reader["collection_name"].ToString(),
                            Description = reader["collection_des"].ToString()
                        };
                        Brands.Add(brand);
                    }
                }
            }
        }

        private void LoadFilters()
        {
            Filters = new ObservableCollection<string>() {
                "Price: Low to High",
                "Price: High to Low",
                "Low Stock",
                "Available",
            };
        }

        private void LoadStatus()
        {
            ProductStatus = new ObservableCollection<string>() {
                "Available",
                "Not Available"
            };
        }

        //Commands
        public ICommand SearchCommand { get; set; }
        public ICommand SelectBrandCommand { get; set; }
        public ICommand SelectFilterCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand ShowAddViewCommand { get; set; }
        public ICommand BrowseImageCommand { get; set; }
        public ICommand AddProductCommand { get; set; }
        public ICommand BackFromAddView { get; set; }
        public ICommand ViewProductDetailCommand { get; set; }
        public ICommand ResetInfoCommand { get; set; }
        public ICommand RemoveProductCommand { get; set; }
        public ICommand UpdateDetailCommand { get; set; }
        public ICommand BackFromUpdateView { get; set; }
        public ICommand ImportDataCommand { get; set; }
        public ICommand PrevPageCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
       

        public ProductViewModel()
        {
            items = LoadData();
            LoadBrands();
            LoadFilters();
            LoadStatus();
            ItemsPerPage = "10";
            UpdateListView(items);

            SearchCommand = new ViewModelCommand(ExecuteSearchCommand);

            SelectBrandCommand = new ViewModelCommand(ExecuteSelectBrandCommand);

            SelectFilterCommand = new ViewModelCommand(ExecuteSelectFilterCommand);

            ResetCommand = new ViewModelCommand(ExecuteResetCommand);

            ShowAddViewCommand = new ViewModelCommand(ExecuteShowAddViewCommand);

            BrowseImageCommand = new ViewModelCommand(ExecuteBrowseImageCommand);

            AddProductCommand = new ViewModelCommand(ExecuteAddProductCommand);

            BackFromAddView = new ViewModelCommand(ExecuteBackFromAddView);

            ViewProductDetailCommand = new ViewModelCommand(ExecuteViewProductDetailCommand);

            ResetInfoCommand = new ViewModelCommand(ExecuteResetInfoCommand);

            RemoveProductCommand = new ViewModelCommand(ExecuteRemoveProductCommand);

            UpdateDetailCommand = new ViewModelCommand(ExecuteUpdateDetailCommand);

            BackFromUpdateView = new ViewModelCommand(ExecuteBackFromUpdateView);

            ImportDataCommand = new ViewModelCommand(ExecuteImportDataCommand);

            PrevPageCommand = new ViewModelCommand(ExecutePrevPageCommand);

            NextPageCommand = new ViewModelCommand(ExecuteNextPageCommand);
        }



        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
        private ObservableCollection<ProductModel> LoadData()
        {
            var ProductList = new ObservableCollection<ProductModel>();
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

                        ProductList.Add(product);
                    }
                }
            }

            return ProductList;
        }



        private void UpdateListView(ObservableCollection<ProductModel> list)
        {
            var itemsToShow = list.Skip(CurrentPage * int.Parse(itemsPerPage)).Take(int.Parse(itemsPerPage)).ToList();
            ListProducts = new ObservableCollection<ProductModel>(itemsToShow);
            TotalPages = (int)Math.Ceiling((double)list.Count() / 10);
            TotalItems = list.Count;
        }


        private bool checkExistedProduct(string name)
        {
            bool existedProduct = false;
            foreach (var items in items)
            {
                if (items.Name.Contains(name))
                {
                    return true;
                }
            }
            return existedProduct;
        }

        private void ExecuteSearchCommand(object obj)
        {
            _type = (int)Pages.Actions;
            ErrorMessage = null;
            searchItems = new ObservableCollection<ProductModel>();

            if (string.IsNullOrEmpty(NameOfProduct))
            {
                _type = (int)Pages.Main;
                UpdateListView(items);
            }
            else if (checkExistedProduct(NameOfProduct))
            {
                
                foreach (var item in items)
                {
                    if (item.Name.Contains(NameOfProduct))
                    {
                        searchItems.Add(item);
                    }
                }
                UpdateListView(searchItems);
            }
            else
            {
                ErrorMessage = $"* No result for product {NameOfProduct}";
            }
        }

        private void ExecuteSelectBrandCommand(object obj)
        {
            UpdateProductList();
        }

        private void ExecuteSelectFilterCommand(object obj)
        {
            UpdateProductList();
        }

        private void UpdateProductList()
        {
            _type = (int)Pages.Actions;
            
            if (SelectedBrand == null)
            {
                searchItems = items;
            }
            else
            {
                searchItems = new ObservableCollection<ProductModel>(); 
                foreach (var item in items)
                {
                    if (SelectedBrand != null && item.Name.Contains(SelectedBrand.Name))
                    {
                        searchItems.Add(item);
                    }
                }
            }

            if (SelectedFilter != null)
            {
                switch (SelectedFilter)
                {
                    case "Price: Low to High":
                        searchItems = new ObservableCollection<ProductModel>(searchItems.OrderBy(product => product.SalePrice).ToList());
                        break;
                    case "Price: High to Low":
                        searchItems = new ObservableCollection<ProductModel>(searchItems.OrderByDescending(product => product.SalePrice).ToList());
                        break;
                    case "Low Stock":
                        searchItems = new ObservableCollection<ProductModel>(searchItems.OrderBy(product => product.Quantity).ToList());
                        break;
                    case "Available":
                        searchItems = new ObservableCollection<ProductModel>(searchItems.Where(product => product.Status == "Available").ToList());
                        break;
                    default:
                        break;
                }
            }

            if (searchItems.Count > 0)
            {
                ErrorMessage = null;
            }
            else
            {
                ErrorMessage = $"* No result for brand {SelectedBrand?.Name ?? "--All--"}";
            }

            UpdateListView(searchItems);
        }

        private void ExecuteResetCommand(object obj)
        {
            SelectedBrand = null;
            SelectedFilter = "Best Seller";
            UpdateListView(items);
        }

        private void ExecuteImportDataCommand(object obj)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var package = new ExcelPackage(new FileInfo("Products.xlsx"));
                ExcelWorksheet workSheet = package.Workbook.Worksheets[0];



                for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                {
                    try
                    {
                        int j = 1;

                        string brand = workSheet.Cells[i, j++].Value.ToString();
                        string name = workSheet.Cells[i, j++].Value.ToString();
                        string description = workSheet.Cells[i, j++].Value.ToString();
                        int quantity = int.Parse(workSheet.Cells[i, j++].Value.ToString());
                        decimal capital = decimal.Parse(workSheet.Cells[i, j++].Value.ToString());
                        decimal saleprice = decimal.Parse(workSheet.Cells[i, j++].Value.ToString());
                        string status = workSheet.Cells[i, j++].Value.ToString();
                        string avatar = workSheet.Cells[i, j++].Value.ToString();



                        ProductModel product = new ProductModel()
                        {
                            Brand = brand,
                            Name = name,
                            Description = description,
                            Quantity = quantity,
                            Capital = capital,
                            SalePrice = saleprice,
                            Status = status,
                            Avatar = avatar
                        };

                        using (var connection = GetConnection())
                        using (var command = new SqlCommand())
                        {
                            connection.Open();
                            command.Connection = connection;
                            command.CommandText = "INSERT INTO [product] VALUES (@productname ,@description, @capitalprice, @saleprice, @status, @stock, @avatar)";
                            command.Parameters.Add("@productname", SqlDbType.NVarChar).Value = product.Brand + " " + product.Name;
                            command.Parameters.Add("@description", SqlDbType.NVarChar).Value = product.Description;
                            command.Parameters.Add("@capitalprice", SqlDbType.Decimal).Value = product.Capital;
                            command.Parameters.Add("@saleprice", SqlDbType.Decimal).Value = product.SalePrice;
                            command.Parameters.Add("@status", SqlDbType.NVarChar).Value = product.Status;
                            command.Parameters.Add("@stock", SqlDbType.Int).Value = product.Quantity;
                            command.Parameters.Add("@avatar", SqlDbType.NVarChar).Value = product.Avatar;
                            command.ExecuteNonQuery();
                        }

                        items.Add(product);
                        UpdateListView(items);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error!");
            }
        }


        private void ExecuteShowAddViewCommand(object obj)
        {
            ProductGridVisibility = Visibility.Collapsed;
            AddProductGridVisibility = Visibility.Visible;
            SelectedImageSource = "/Images/Products/picture.png";
        }


        private void ExecuteAddProductCommand(object obj)
        {
            string name = NewProductName;
            int quantity = NewProductQuantity;
            decimal capital = NewProductCapital;
            decimal saleprice = NewProductSalePrice;
            string description = NewProductDescription;
           

            if (string.IsNullOrEmpty(name) || quantity == null || capital == null || saleprice == null || string.IsNullOrEmpty(description) || SelectedProductStatus == null || SelectedProductBrand == null)
            {
                System.Windows.MessageBox.Show("* Please provide full information to add", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else if (!checkExistedProduct(name))
            {
                ProductModel newProduct = new ProductModel();
                newProduct.Quantity = quantity;
                newProduct.Capital = capital;
                newProduct.Description = description;
                newProduct.SalePrice = saleprice; 
                BrandModel Brand = SelectedProductBrand;
                newProduct.Status = SelectedProductStatus;
                newProduct.Brand = Brand.Name;
                newProduct.Name = Brand.Name + " " + name;
                newProduct.Avatar = SelectedImageSource;

        

                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO [product] VALUES (@productname ,@description, @capitalprice, @saleprice, @status, @stock, @avatar)";
                    command.Parameters.Add("@productname", SqlDbType.NVarChar).Value = Brand.Name + " " + name;
                    command.Parameters.Add("@description", SqlDbType.NVarChar).Value = description;
                    command.Parameters.Add("@capitalprice", SqlDbType.Decimal).Value = capital;
                    command.Parameters.Add("@saleprice", SqlDbType.Decimal).Value = saleprice;
                    command.Parameters.Add("@status", SqlDbType.NVarChar).Value = SelectedProductStatus;
                    command.Parameters.Add("@stock", SqlDbType.Int).Value = quantity;
                    command.Parameters.Add("@avatar", SqlDbType.NVarChar).Value = SelectedImageSource;
                    command.ExecuteNonQuery();
                }

                items.Add(newProduct);
                UpdateListView(items);
                System.Windows.MessageBox.Show("* Product has been added successfully", "Announcement", MessageBoxButton.OK);
                ExecuteBackFromAddView(null);
            }
            else
            {
                System.Windows.MessageBox.Show("* Product has already existed, please try again", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ExecuteBrowseImageCommand(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() != null)
            {
                try
                {
                    SelectedImageSource = openFileDialog.FileName;
                    CurrentImageSource = SelectedImageSource;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExecuteBackFromAddView(object obj)
        {
            ProductGridVisibility = Visibility.Visible;
            AddProductGridVisibility = Visibility.Collapsed;
        }
        private void ExecuteBackFromUpdateView(object obj)
        {
            ProductGridVisibility = Visibility.Visible;
            UpdateProductGridVisibility = Visibility.Collapsed;
        }

        ProductModel oldData = null;
        ProductModel newData = null;
        private void ExecuteViewProductDetailCommand(object obj)
        {
            ProductGridVisibility = Visibility.Collapsed;
            UpdateProductGridVisibility = Visibility.Visible;
            if (obj is ProductModel selectedItem)
            {
                oldData = (ProductModel)selectedItem.Clone();
                newData = (ProductModel)selectedItem.Clone();
                ShowProductDetails(oldData);
            }

        }

        private void ShowProductDetails(ProductModel model)
        {
            CurrentName = model.Name;
            CurrentDescription = model.Description;
            CurrentBrand = model.Brand;
            CurrentQuantity = model.Quantity;
            CurrentCapital = model.Capital;
            CurrentSalePrice = model.SalePrice;
            CurrentImageSource = model.Avatar;
            CurrentStatus = model.Status;
        }


        private void ExecuteResetInfoCommand(object obj)
        {
            if (oldData != null)
            {
                ShowProductDetails(oldData);
            }
        }

        private void ExecuteRemoveProductCommand(object obj)
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
                        command.CommandText = "DELETE FROM PRODUCT WHERE product_id = @id";
                        command.Parameters.Add("@id", SqlDbType.Int).Value = SelectedItem.Id;
                        command.ExecuteNonQuery();
                    }

                    System.Windows.MessageBox.Show("* Product has been removed successfully", "Announcement", (MessageBoxButton)MessageBoxButtons.OK);
                    items = LoadData();
                    UpdateListView(items);
                    ExecuteBackFromUpdateView(null);
                }
            }
        }

        private void ExecuteUpdateDetailCommand(object obj)
        {
            if (obj is ProductModel SelectedItem)
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "UPDATE [product] SET product_des = @description, stock = @quantity, Capital = @capital, SalePrice = @salePrice, avatar = @avatar, [status] = @status WHERE product_name = @ProductName and product_id = @id";
                    command.Parameters.Add("@description", SqlDbType.NVarChar).Value = CurrentDescription;
                    command.Parameters.Add("@quantity", SqlDbType.Int).Value = CurrentQuantity;
                    command.Parameters.Add("@capital", SqlDbType.Float).Value = CurrentCapital;
                    command.Parameters.Add("@saleprice", SqlDbType.Float).Value = CurrentSalePrice;
                    command.Parameters.Add("@productName", SqlDbType.NVarChar).Value = CurrentName;
                    command.Parameters.Add("@id", SqlDbType.Int).Value = SelectedItem.Id;
                    command.Parameters.Add("@avatar", SqlDbType.NVarChar).Value = CurrentImageSource;
                    command.Parameters.Add("@status", SqlDbType.NVarChar).Value = CurrentStatus;
                    command.ExecuteNonQuery();
                }


                items = LoadData();
                UpdateListView(items);
                System.Windows.MessageBox.Show("* Product has been updated successfully", "Announcement", (MessageBoxButton)MessageBoxButtons.OK);
                ExecuteBackFromUpdateView(null);
            }
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
