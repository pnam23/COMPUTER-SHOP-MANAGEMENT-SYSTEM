using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using computershop.Model;
using computershop.Repository;
using computershop.View;
using Microsoft.VisualBasic.ApplicationServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace computershop.ViewModel
{
    class LoginViewModel : ViewModelBase
    {

        private string _userName;
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible = true;

        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        public SecureString Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
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

        private bool rememberMe;

        public bool RememberMe
        {
            get { return rememberMe; }
            set
            {
                rememberMe = value;
                OnPropertyChanged(nameof(RememberMe));
            }
        }
        public ICommand LoginCommand { get; }
        public ICommand RecoveryPasswordCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand ToggleRememberPasswordCommand { get; }

        private IUserRepository userRepository;

        public LoginViewModel()
        {
            userRepository = new UserRepository();
            var username = ConfigurationManager.AppSettings["username"];
            var password = ConfigurationManager.AppSettings["password"];
            var entropy = ConfigurationManager.AppSettings["entropy"];

            //if (DecryptPassword(password, entropy)!= null)
            //{
            //    ShowUsernameAndPassword(username, password, entropy);
            //}

            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
        }

        private void ShowUsernameAndPassword(string username, string password, string entropy)
        { 
            UserName = username;
            Password = ConvertToSecureString(DecryptPassword(password, entropy));
        }

        private void EncryptPassword()
        {
            string passwordString = ConvertSecureStringToString(Password);
            var passwordInBytes = Encoding.UTF8.GetBytes(passwordString);
            var entropy = new byte[20];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(entropy);
            }
            var cypherText = ProtectedData.Protect(passwordInBytes, entropy, DataProtectionScope.CurrentUser);

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["password"].Value = Convert.ToBase64String(cypherText);
            config.AppSettings.Settings["entropy"].Value = Convert.ToBase64String(entropy);
            config.AppSettings.Settings["username"].Value = UserName;

            config.Save(ConfigurationSaveMode.Minimal);
            ConfigurationManager.RefreshSection("appSettings");
        }
        private string ConvertSecureStringToString(SecureString secureString)
        {
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(secureString);
            try
            {
                return System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
        }
        private  SecureString ConvertToSecureString(string plainString)
        {
            if (plainString == null)
                throw new ArgumentNullException(nameof(plainString));

            var secureString = new SecureString();
            foreach (char c in plainString)
            {
                secureString.AppendChar(c);
            }
            secureString.MakeReadOnly();
            return secureString;
        }

        public static string DecryptPassword(string storedEncryptedPassword, string storedEntropy)
        {

            if (string.IsNullOrEmpty(storedEncryptedPassword) || string.IsNullOrEmpty(storedEntropy))
            {
                return null;
            }

            byte[] encryptedPasswordBytes = Convert.FromBase64String(storedEncryptedPassword);
            byte[] entropyBytes = Convert.FromBase64String(storedEntropy);

            try
            {
                byte[] decryptedPasswordBytes = ProtectedData.Unprotect(encryptedPasswordBytes, entropyBytes, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(decryptedPasswordBytes);
            }
            catch (CryptographicException ex)
            {
                return null;
            }
        }


        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(UserName) || UserName.Length < 3 ||
                Password == null || Password.Length < 3)
            {
                validData = false;
            }
            else validData = true;
            return validData;
        }

        private void ExecuteLoginCommand(object obj)
        {
            var isValidUser = userRepository.AuthenticateUser(new System.Net.NetworkCredential(UserName, Password));

            if (isValidUser)
            {
                if (RememberMe)
                {
                    EncryptPassword();
                }

                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(UserName),null);
                IsViewVisible = false;
                
                
               
            }
            else
            {
                ErrorMessage = "* Invalid username or password";
            }
        }

        

    }

}
