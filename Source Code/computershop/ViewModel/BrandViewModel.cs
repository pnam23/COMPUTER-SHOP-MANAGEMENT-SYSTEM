using computershop.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace computershop.ViewModel
{
    public class BrandViewModel:ViewModelBase
    {
        private int totalItems;
        private int currentPage;
        private int totalPages;
        private ObservableCollection<BrandModel> listBrands;
        private string itemsPerPage;
        private string nameOfBrand;
        private string errorMessage;
        private string addViewErrorMessage;
        private string addViewSuccessMessage;
        private string newBrandName;
        private string newBrandDescription;
        private string updateBrandName;
        private string updateBrandDescription;
        private string updateViewErrorMessage;
        private string updateViewSuccessMessage;
        private BrandModel selectedBrand;

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
        public int CurrentPage {
            get {  return currentPage; }
            set { currentPage = value; OnPropertyChanged(nameof(currentPage));}
        }

        public string CurrentPageDisplay => (CurrentPage + 1).ToString();


        public int TotalPages {
            get { return totalPages; }
            set { totalPages = value; OnPropertyChanged(nameof(totalPages));}
        }


        public ObservableCollection<BrandModel> ListBrands { 
            get { return listBrands; }
            set { listBrands = value; OnPropertyChanged(nameof(listBrands)); }
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
            UpdateListView();
        }

        public string NameOfBrand
        {
            get { return nameOfBrand; }
            set
            {
                nameOfBrand = value;
                OnPropertyChanged(nameof(NameOfBrand));
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

        public string AddViewErrorMessage
        {
            get { return addViewErrorMessage; }
            set
            {
                addViewErrorMessage = value;
                OnPropertyChanged(nameof(AddViewErrorMessage));
            }
        }
        public string AddViewSuccessMessage
        {
            get { return addViewSuccessMessage; }
            set
            {
                addViewSuccessMessage = value;
                OnPropertyChanged(nameof(AddViewSuccessMessage));
            }
        }
        public string UpdateViewErrorMessage
        {
            get { return updateViewErrorMessage; }
            set
            {
                updateViewErrorMessage = value;
                OnPropertyChanged(nameof(UpdateViewErrorMessage));
            }
        }
        public string UpdateViewSuccessMessage
        {
            get { return updateViewSuccessMessage; }
            set
            {
                updateViewSuccessMessage = value;
                OnPropertyChanged(nameof(UpdateViewSuccessMessage));
            }
        }
        public string NewBrandName
        {
            get { return newBrandName; }
            set
            {
                newBrandName = value;
                OnPropertyChanged(nameof(NewBrandName));
            }
        }
        public string NewBrandDescription
        {
            get { return newBrandDescription; }
            set
            {
                newBrandDescription = value;
                OnPropertyChanged(nameof(NewBrandDescription));
            }
        }
        public string UpdateBrandName
        {
            get { return updateBrandName; }
            set
            {
                updateBrandName = value;
                OnPropertyChanged(nameof(UpdateBrandName));
            }
        }
        public string UpdateBrandDescription
        {
            get { return updateBrandDescription; }
            set
            {
                updateBrandDescription = value;
                OnPropertyChanged(nameof(UpdateBrandDescription));
            }
        }
        public BrandModel SelectedBrand
        {
            get { return selectedBrand; }
            set
            {
                selectedBrand = value;
                OnPropertyChanged(nameof(SelectedBrand));
            }
        }

        private Visibility _brandGridVisibility = Visibility.Visible;
        public Visibility BrandGridVisibility
        {
            get { return _brandGridVisibility; }
            set
            {
                _brandGridVisibility = value;
                OnPropertyChanged(nameof(BrandGridVisibility));
            }
        }

        private Visibility _addBrandGridVisibility = Visibility.Collapsed;
        public Visibility AddBrandGridVisibility
        {
            get { return _addBrandGridVisibility; }
            set
            {
                _addBrandGridVisibility = value;
                OnPropertyChanged(nameof(AddBrandGridVisibility));
            }
        }
        private Visibility _updateBrandGridVisibility = Visibility.Collapsed;
        public Visibility UpdateBrandGridVisibility
        {
            get { return _updateBrandGridVisibility; }
            set
            {
                _updateBrandGridVisibility = value;
                OnPropertyChanged(nameof(UpdateBrandGridVisibility));
            }
        }


        //Commands
        public ICommand SearchCommand { get; set; }
        public ICommand ShowAddViewCommand { get; set; }
        public ICommand AddNewBrandCommand { get; set; }
        public ICommand ShowUpdateViewCommand { get; set; }
        public ICommand UpdateBrandDetailCommand { get; set; }
        public ICommand DeleteBrandCommand { get; set; }
        public ICommand BackFromAddViewCommand { get; set; }
        public ICommand BackFromUpdateViewCommand { get; set; }
        public ICommand PrevPageCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
        


        private readonly string _connectionString = @"
                Server=.;
                Database = LoginDB;
                Trusted_Connection = yes;
                TrustServerCertificate = True
                ";

        ObservableCollection<BrandModel> items = new ObservableCollection<BrandModel>();
        

        public BrandViewModel() {

            items = LoadData();
            ItemsPerPage = "10";
            UpdateListView();
            SearchCommand = new ViewModelCommand(ExecuteSearchCommand);
            ShowAddViewCommand = new ViewModelCommand(ExecuteShowAddViewCommand);
            AddNewBrandCommand = new ViewModelCommand(ExecuteAddNewBrandCommand);
            ShowUpdateViewCommand = new ViewModelCommand(ExecuteShowUpdateViewCommand);
            UpdateBrandDetailCommand = new ViewModelCommand(ExecuteUpdateBrandDetailCommand);
            DeleteBrandCommand = new ViewModelCommand(ExecuteDeleteBrandCommand);
            BackFromAddViewCommand = new ViewModelCommand(ExecuteBackFromAddViewCommand);
            BackFromUpdateViewCommand = new ViewModelCommand(ExecuteBackFromUpdateViewCommand);
            PrevPageCommand = new ViewModelCommand(ExecutePrevPageCommand);
            NextPageCommand = new ViewModelCommand(ExecuteNextPageCommand);
        }

        

        

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        private ObservableCollection<BrandModel> LoadData()
        {
            var BrandList = new ObservableCollection<BrandModel>();
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

                        BrandList.Add(brand);
                    }
                }
            }
            return BrandList;
        }

        private void UpdateListView()
        {
            items = LoadData();
            var itemsToShow = items.Skip(CurrentPage * int.Parse(itemsPerPage)).Take(int.Parse(itemsPerPage)).ToList();
            ListBrands = new ObservableCollection<BrandModel>(itemsToShow);
            TotalPages = (int)Math.Ceiling((double)items.Count() / 10);
            TotalItems = items.Count;
        }

        public bool CheckExistedBrand(string name)
        {
            bool existedBrand = false;
            foreach (var item in items)
            {
                if (name == item.Name)
                {
                    return true;
                }
            }
            return existedBrand;
        }

        private void ExecuteSearchCommand(object obj)
        {
            if (string.IsNullOrEmpty(NameOfBrand))
            {
                UpdateListView();
            }
            else if (CheckExistedBrand(NameOfBrand))
            {
                foreach (var item in items)
                {
                    if (NameOfBrand == item.Name)
                    {
                        ListBrands = new ObservableCollection<BrandModel>() { item };
                        TotalPages = 1;
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show( $"* No result for brand {NameOfBrand}", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);               
            }
        }

        private void ExecuteShowAddViewCommand(object obj)
        {
            AddViewErrorMessage = null;
            AddViewSuccessMessage = null;
            NewBrandName = null;
            NewBrandDescription = null;
            BrandGridVisibility = Visibility.Collapsed;
            AddBrandGridVisibility = Visibility.Visible;
        }
        private void ExecuteBackFromAddViewCommand(object obj)
        {
            BrandGridVisibility = Visibility.Visible;
            AddBrandGridVisibility = Visibility.Collapsed;
        }

        private void ExecuteAddNewBrandCommand(object obj)
        {
            if (string.IsNullOrEmpty(NewBrandName))
            {
                AddViewErrorMessage = "* Invalid brand, please try again";
            }
            else if (string.IsNullOrEmpty(NewBrandDescription))
            {
                AddViewErrorMessage = "* Invalid description, please try again";
            }
            else if (!CheckExistedBrand(NewBrandName))
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO [collection] VALUES (@brandname,@description)";
                    command.Parameters.Add("@brandname", SqlDbType.NVarChar).Value = NewBrandName;
                    command.Parameters.Add("@description", SqlDbType.NVarChar).Value = NewBrandDescription;
                    command.ExecuteNonQuery();
                }

                UpdateListView();
                AddViewErrorMessage = null;
                AddViewSuccessMessage = $"* Brand {NewBrandName} has been added successfully";
            }
            else
            {
                AddViewErrorMessage = $"* Brand {NewBrandName} has already existed, please try again";
            }
        }

        private void ExecuteShowUpdateViewCommand(object obj)
        {
            UpdateViewErrorMessage = null;
            UpdateViewSuccessMessage = null;
            UpdateBrandDescription = null;
            UpdateBrandName = SelectedBrand.Name;
            if (SelectedBrand != null)
            {
                BrandGridVisibility = Visibility.Collapsed;
                UpdateBrandGridVisibility = Visibility.Visible;
            }
            else
            {
                ShowErrorOccurred("Please select a brand to edit.");
            }

        }

        private void ExecuteBackFromUpdateViewCommand(object obj)
        {
            BrandGridVisibility = Visibility.Visible;
            UpdateBrandGridVisibility = Visibility.Collapsed;
        }

        private void ExecuteUpdateBrandDetailCommand(object obj)
        {
            if (string.IsNullOrEmpty(UpdateBrandDescription))
            {
                UpdateViewErrorMessage = "* Invalid description, please try again";
            }
            else
            {
              
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "UPDATE  [COLLECTION] SET COLLECTION_DES = @newDescription WHERE COLLECTION_NAME = @brandname";
                    command.Parameters.Add("@brandname", SqlDbType.NVarChar).Value = SelectedBrand.Name;
                    command.Parameters.Add("@newDescription", SqlDbType.NVarChar).Value = UpdateBrandDescription;
                    command.ExecuteNonQuery();
                }

                UpdateListView();
                UpdateViewSuccessMessage = $"* Brand {UpdateBrandName} has been updated successfully";
            }
        }

        private void ExecuteDeleteBrandCommand(object obj)
        {
            if (SelectedBrand != null)
            {
                ShowConfirmation();
            }
            else {
                ShowErrorOccurred( "Please select a brand to delete.");
            }
        }

        private void ShowConfirmation()
        {
            var name = SelectedBrand.Name;
            var result = MessageBox.Show($"Are you sure you want to delete the brand '{SelectedBrand.Name}'?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                DeleteBrand();
                UpdateListView();
                MessageBox.Show($"Brand {name} has been deleted successfully", "Announcement", MessageBoxButton.OK);
            }
        }

        private void ShowErrorOccurred(string s)
        {
            MessageBox.Show(s, "Error", MessageBoxButton.OK,MessageBoxImage.Error);
        }

        public void DeleteBrand()
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE FROM [collection] WHERE collection_name = @name";
                command.Parameters.Add("@name", SqlDbType.NVarChar).Value = SelectedBrand.Name;
                command.ExecuteNonQuery();
            }
        }

        private void ExecuteNextPageCommand(object obj)
        {
            if (CurrentPage < TotalPages - 1)
            {
                CurrentPage++;
                UpdateListView();
            }
        }

        private void ExecutePrevPageCommand(object obj)
        {
            if (CurrentPage > 0)
            {
                CurrentPage--;
                UpdateListView();
            }
        }

        
    }
}
