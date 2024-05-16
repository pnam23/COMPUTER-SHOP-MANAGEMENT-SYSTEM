using computershop.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace computershop.ViewModel
{
    public class CustomerViewModel : ViewModelBase
    {
        private int totalItems;
        private int currentPage;
        private int totalPages;
        private ObservableCollection<CustomerModel> listCustomers;
        private string itemsPerPage;
        private string nameOfCustomer;
        private string errorMessage;
        private string addViewErrorMessage;
        private string addViewSuccessMessage;
        private string newCustomerName;
        private string newCustomerPhone;
        private string newCustomerEmail;
        private string newCustomerAddress;
        private string updateCustomerName;
        private string updateCustomerPhone;
        private string updateCustomerEmail;
        private string updateCustomerAddress;
        private string updateViewErrorMessage;
        private string updateViewSuccessMessage;
        private CustomerModel selectedCustomer;

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


        public ObservableCollection<CustomerModel> ListCustomers
        {
            get { return listCustomers; }
            set { listCustomers = value; OnPropertyChanged(nameof(listCustomers)); }
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

        public string NameOfCustomer
        {
            get { return nameOfCustomer; }
            set
            {
                nameOfCustomer = value;
                OnPropertyChanged(nameof(NameOfCustomer));
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
        public string NewCustomerName
        {
            get { return newCustomerName; }
            set
            {
                newCustomerName = value;
                OnPropertyChanged(nameof(NewCustomerName));
            }
        }
        public string NewCustomerPhone
        {
            get { return newCustomerPhone; }
            set
            {
                newCustomerPhone = value;
                OnPropertyChanged(nameof(NewCustomerPhone));
            }
        }
        public string NewCustomerEmail
        {
            get { return newCustomerEmail; }
            set
            {
                newCustomerEmail = value;
                OnPropertyChanged(nameof(NewCustomerEmail));
            }
        }
        public string NewCustomerAddress
        {
            get { return newCustomerAddress; }
            set
            {
                newCustomerAddress = value;
                OnPropertyChanged(nameof(NewCustomerAddress));
            }
        }
        public string UpdateCustomerName
        {
            get { return updateCustomerName; }
            set
            {
                updateCustomerName = value;
                OnPropertyChanged(nameof(UpdateCustomerName));
            }
        }
        public string UpdateCustomerPhone
        {
            get { return updateCustomerPhone; }
            set
            {
                updateCustomerPhone = value;
                OnPropertyChanged(nameof(UpdateCustomerPhone));
            }
        }
        public string UpdateCustomerEmail
        {
            get { return updateCustomerEmail; }
            set
            {
                updateCustomerEmail = value;
                OnPropertyChanged(nameof(UpdateCustomerEmail));
            }
        }
        public string UpdateCustomerAddress
        {
            get { return updateCustomerAddress; }
            set
            {
                updateCustomerAddress = value;
                OnPropertyChanged(nameof(UpdateCustomerAddress));
            }
        }
        public CustomerModel SelectedCustomer
        {
            get { return selectedCustomer; }
            set
            {
                selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }

        private Visibility _customerGridVisibility = Visibility.Visible;
        public Visibility CustomerGridVisibility
        {
            get { return _customerGridVisibility; }
            set
            {
                _customerGridVisibility = value;
                OnPropertyChanged(nameof(CustomerGridVisibility));
            }
        }

        private Visibility _addCustomerGridVisibility = Visibility.Collapsed;
        public Visibility AddCustomerGridVisibility
        {
            get { return _addCustomerGridVisibility; }
            set
            {
                _addCustomerGridVisibility = value;
                OnPropertyChanged(nameof(AddCustomerGridVisibility));
            }
        }
        private Visibility _updateCustomerGridVisibility = Visibility.Collapsed;
        public Visibility UpdateCustomerGridVisibility
        {
            get { return _updateCustomerGridVisibility; }
            set
            {
                _updateCustomerGridVisibility = value;
                OnPropertyChanged(nameof(UpdateCustomerGridVisibility));
            }
        }


        //Commands
        public ICommand SearchCommand { get; set; }
        public ICommand ShowAddViewCommand { get; set; }
        public ICommand AddNewCustomerCommand { get; set; }
        public ICommand ShowUpdateViewCommand { get; set; }
        public ICommand UpdateCustomerInfoCommand { get; set; }
        public ICommand DeleteCustomerCommand { get; set; }
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

        ObservableCollection<CustomerModel> items = new ObservableCollection<CustomerModel>();


        public CustomerViewModel()
        {
            CustomerGridVisibility = Visibility.Visible;
            AddCustomerGridVisibility = Visibility.Collapsed;
            UpdateCustomerGridVisibility = Visibility.Collapsed;

            items = LoadData();
            ItemsPerPage = "10";
            UpdateListView();
            
            SearchCommand = new ViewModelCommand(ExecuteSearchCommand);
            ShowAddViewCommand = new ViewModelCommand(ExecuteShowAddViewCommand);
            AddNewCustomerCommand = new ViewModelCommand(ExecuteAddNewCustomerCommand);
            ShowUpdateViewCommand = new ViewModelCommand(ExecuteShowUpdateViewCommand);
            UpdateCustomerInfoCommand = new ViewModelCommand(ExecuteUpdateCustomerInfoCommand);
            DeleteCustomerCommand = new ViewModelCommand(ExecuteDeleteCustomerCommand);
            BackFromAddViewCommand = new ViewModelCommand(ExecuteBackFromAddViewCommand);
            BackFromUpdateViewCommand = new ViewModelCommand(ExecuteBackFromUpdateViewCommand);
            PrevPageCommand = new ViewModelCommand(ExecutePrevPageCommand);
            NextPageCommand = new ViewModelCommand(ExecuteNextPageCommand);
        }





        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        private ObservableCollection<CustomerModel> LoadData()
        {
            var CustomerList = new ObservableCollection<CustomerModel>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [customer]";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CustomerModel cus = new CustomerModel()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("customer_id")),
                            CustomerName = reader["customer_name"].ToString(),
                            CustomerPhone = reader["customer_phone"].ToString(),
                            CustomerEmail = reader["customer_email"].ToString(),
                            CustomerAddress = reader["customer_address"].ToString(),
                        };

                        CustomerList.Add(cus);
                    }
                }
            }
            return CustomerList;
        }

        private void UpdateListView()
        {
            items = LoadData();
            var itemsToShow = items.Skip(CurrentPage * int.Parse(itemsPerPage)).Take(int.Parse(itemsPerPage)).ToList();
            ListCustomers = new ObservableCollection<CustomerModel>(itemsToShow);
            TotalPages = (int)Math.Ceiling((double)items.Count() / 10);
            TotalItems = items.Count;
        }

        public bool CheckExistedCustomer(string phone, string email)
        {
            bool existedCustomer = false;
            foreach (var item in items)
            {
                if (phone == item.CustomerPhone || email == item.CustomerEmail)
                {
                    return true;
                }
            }
            return existedCustomer;
        }
        
        public bool CheckExistedCustomerName(string name)
        {
            bool existedCustomer = false;
            foreach (var item in items)
            {
                if (name == item.CustomerName)
                {
                    return true;
                }
            }
            return existedCustomer;
        }

        private void ExecuteSearchCommand(object obj)
        {
            if (string.IsNullOrEmpty(NameOfCustomer))
            {
                UpdateListView();
            }
            else if (CheckExistedCustomerName(NameOfCustomer))
            {
                foreach (var item in items)
                {
                    if (NameOfCustomer == item.CustomerName)
                    {
                        ListCustomers = new ObservableCollection<CustomerModel>() { item };
                        TotalPages = 1;
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show($"* No result for customer '{NameOfCustomer}'", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }

        private void ExecuteShowAddViewCommand(object obj)
        {
            AddViewErrorMessage = null;
            AddViewSuccessMessage = null;
            NewCustomerName = null;
            NewCustomerPhone = null;
            NewCustomerEmail = null;
            NewCustomerAddress = null;
            CustomerGridVisibility = Visibility.Collapsed;
            AddCustomerGridVisibility = Visibility.Visible;
        }
        private void ExecuteBackFromAddViewCommand(object obj)
        {
            CustomerGridVisibility = Visibility.Visible;
            AddCustomerGridVisibility = Visibility.Collapsed;
        }

        private void ExecuteAddNewCustomerCommand(object obj)
        {
            if (string.IsNullOrEmpty(NewCustomerName))
            {
                AddViewErrorMessage = "* Invalid customer, please try again";
            }
            else if (string.IsNullOrEmpty(NewCustomerPhone))
            {
                AddViewErrorMessage = "* Invalid phone number, please try again";
            }
            else if (string.IsNullOrEmpty(NewCustomerEmail))
            {
                AddViewErrorMessage = "* Invalid email, please try again";

            }
            else if (string.IsNullOrEmpty(NewCustomerAddress))
            {
                AddViewErrorMessage = "* Invalid address, please try again";
            }
            else if (!CheckExistedCustomer(NewCustomerPhone, newCustomerEmail))
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO [customer] VALUES (@name,@email, @address, @phone)";
                    command.Parameters.Add("@name", SqlDbType.NVarChar).Value = NewCustomerName;
                    command.Parameters.Add("@phone", SqlDbType.NVarChar).Value = NewCustomerPhone;
                    command.Parameters.Add("@email", SqlDbType.NVarChar).Value = NewCustomerEmail;
                    command.Parameters.Add("@address", SqlDbType.NVarChar).Value = NewCustomerAddress;
                    command.ExecuteNonQuery();
                }

                UpdateListView();
                AddViewErrorMessage = null;
                AddViewSuccessMessage = $"* Customer {NewCustomerName} has been added successfully";
            }
            else
            {

                AddViewErrorMessage = $"* Phone number '{NewCustomerPhone}' or Email '{NewCustomerEmail}' has been used, please try again";
            }
        }

        private void ExecuteShowUpdateViewCommand(object obj)
        {
            UpdateViewErrorMessage = null;
            UpdateViewSuccessMessage = null;
            UpdateCustomerAddress = null;
            UpdateCustomerName = SelectedCustomer.CustomerName;
            UpdateCustomerPhone = SelectedCustomer.CustomerPhone;
            UpdateCustomerEmail = SelectedCustomer.CustomerEmail;
            if (SelectedCustomer != null)
            {
                CustomerGridVisibility = Visibility.Collapsed;
                UpdateCustomerGridVisibility = Visibility.Visible;
            }
            else
            {
                ShowErrorOccurred("Please select a customer to edit.");
            }

        }

        private void ExecuteBackFromUpdateViewCommand(object obj)
        {
            CustomerGridVisibility = Visibility.Visible;
            UpdateCustomerGridVisibility = Visibility.Collapsed;
        }

        private void ExecuteUpdateCustomerInfoCommand(object obj)
        {
            if (string.IsNullOrEmpty(UpdateCustomerAddress))
            {
                UpdateViewErrorMessage = "* Invalid address, please try again";
            }
            else
            {

                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "UPDATE  [customer] SET customer_address = @newaddress WHERE customer_phone = @phone";
                    command.Parameters.Add("@phone", SqlDbType.NVarChar).Value = SelectedCustomer.CustomerPhone;
                    command.Parameters.Add("@newaddress", SqlDbType.NVarChar).Value = UpdateCustomerAddress;
                    command.ExecuteNonQuery();
                }

                UpdateListView();
                UpdateViewSuccessMessage = $"* Customer '{UpdateCustomerName}' has been updated successfully";
            }
        }

        private void ExecuteDeleteCustomerCommand(object obj)
        {
            if (SelectedCustomer != null)
            {
                ShowConfirmation();
            }
            else
            {
                ShowErrorOccurred("Please select a customer to delete.");
            }
        }

        private void ShowConfirmation()
        {
            var name = SelectedCustomer.CustomerName;
            var result = MessageBox.Show($"Are you sure you want to delete customer '{SelectedCustomer.CustomerName}'?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                DeleteCustomer();
                UpdateListView();
                MessageBox.Show($"Customer {name} has been deleted successfully", "Announcement", MessageBoxButton.OK);
            }
        }

        private void ShowErrorOccurred(string s)
        {
            MessageBox.Show(s, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void DeleteCustomer()
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE FROM [cusstomer] WHERE customer_phone = @phone";
                command.Parameters.Add("@phone", SqlDbType.NVarChar).Value = SelectedCustomer.CustomerPhone;
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
