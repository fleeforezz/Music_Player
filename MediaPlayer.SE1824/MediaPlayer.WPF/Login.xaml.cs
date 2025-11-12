using MediaPlayer.BLL.Services;
using MediaPlayer.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MediaPlayer.WPF
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private UserService _userService = new();

        public Login()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordTextBox.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Both email and password are required!", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            User? account = _userService.Login(email, password);
            if (account == null)
            {
                MessageBox.Show("Invalid Email or password", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
