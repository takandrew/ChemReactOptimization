using System.Windows;
using System.Windows.Controls;

namespace ChemReactOptimization
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            ((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password;
        }
    }
}
