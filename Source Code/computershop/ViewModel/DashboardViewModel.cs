using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using computershop.Model;
using computershop.Repository;
using computershop.View;
using FontAwesome.Sharp;

namespace computershop.ViewModel
{
    public class DashboardViewModel : ViewModelBase
    {
        private UserAccountModel currentUserAccount;
        private IUserRepository userRepository;
        private ViewModelBase currentChildView;
        private string caption;
        private IconChar icon;

        public UserAccountModel CurrentUserAccount
        {
            get
            {
                return currentUserAccount;
            }
            set
            {
                currentUserAccount = value;
                OnPropertyChanged(nameof(currentUserAccount));
            }
        }

        public ViewModelBase CurrentChildView
        {
            get
            {
                return currentChildView;
            }
            set
            {
                currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }
        public string Caption {
            get
            {
                return caption;
            }
            set
            {
                caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }
        public IconChar Icon {
            get
            {
                return icon;
            }
            set
            {
                icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }

        private bool _isViewVisible = true;

        public bool IsViewVisible
        {
            get
            {
                return _isViewVisible;
            }
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }

        //Commands
        public ICommand ShowHomeWindowCommand { get; }
        public ICommand ShowBrandWindowCommand { get; }
        public ICommand ShowProductWindowCommand { get; }
        public ICommand ShowOrdersWindowCommand { get; }
        public ICommand ShowReportWindowCommand { get; }
        public ICommand ShowCustomerWindowCommand { get; }
        public ICommand LogOutCommand { get; }

        public DashboardViewModel()
        {
            userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();

            //Initialize commands
            ShowHomeWindowCommand = new ViewModelCommand(ExecuteShowHomeWindowCommand);

            ShowBrandWindowCommand = new ViewModelCommand(ExecuteShowBrandWindowCommand);

            ShowProductWindowCommand = new ViewModelCommand(ExecuteShowProductWindowCommand);

            ShowOrdersWindowCommand = new ViewModelCommand(ExecuteShowOrdersWindowCommand);

            ShowReportWindowCommand = new ViewModelCommand(ExecuteShowReportWindowCommand);

            ShowCustomerWindowCommand = new ViewModelCommand(ExecuteShowCustomerWindowCommand);

            LogOutCommand = new ViewModelCommand(ExecuteLogOutCommand);

            //Default view
            ExecuteShowHomeWindowCommand(null);

            LoadCurrentUserData();
        }

       

        private readonly DashboardWindow _currentWindow;

        private readonly DashboardViewModel _viewModel;

        
        

        private void ExecuteShowHomeWindowCommand(object obj)
        {
            CurrentChildView = new HomeViewModel();
            Caption = "Dashboard";
            Icon = IconChar.Home;
        }
        private void ExecuteShowBrandWindowCommand(object obj)
        {
            CurrentChildView = new BrandViewModel();
            
            Caption = "Brand";
            Icon = IconChar.Book;
        }
        
        private void ExecuteShowProductWindowCommand(object obj)
        {
            CurrentChildView = new ProductViewModel();
            Caption = "Product";
            Icon = IconChar.Computer;
        }
        private void ExecuteShowOrdersWindowCommand(object obj)
        {
            CurrentChildView = new OrdersViewModel();
            Caption = "Orders";
            Icon = IconChar.Receipt;
        }
        private void ExecuteShowReportWindowCommand(object obj)
        {
            CurrentChildView = new ReportViewModel();
            Caption = "Report";
            Icon = IconChar.ChartColumn;
        }

        private void ExecuteShowCustomerWindowCommand(object obj)
        {
            CurrentChildView = new CustomerViewModel();
            Caption = "Customers";
            Icon = IconChar.PeopleGroup;
        }

        private void ExecuteLogOutCommand(object obj)
        {
            LoginWindow login = new LoginWindow();
            IsViewVisible = false;
            login.Show();
        }

        public event EventHandler ShowLoginWindow;


        private void LoadCurrentUserData()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                currentUserAccount.Username = user.Username;
                currentUserAccount.DisplayName = $"{user.Name} {user.lastName}";
            }
            else
            {
                currentUserAccount.DisplayName = "Invalid user, not logged in";
            }
        }
    }
}
