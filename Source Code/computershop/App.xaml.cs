using computershop.View;
using computershop.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;
using computershop.View;

namespace computershop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected void ApplicationStart(object sender, EventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            loginWindow.IsVisibleChanged += (s, ev) =>
            {
                if (loginWindow.IsVisible == false && loginWindow.IsLoaded)
                {
                    var dashboardWindow = new DashboardWindow();
                    dashboardWindow.Show();
                    loginWindow.Close();
                }
            };
        }
    }

}
