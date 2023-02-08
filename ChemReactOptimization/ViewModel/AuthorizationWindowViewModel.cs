using ChemReactOptimization.Data;
using System;
using System.Linq;
using System.Windows;
using WPF_MVVM_Classes;
using ViewModelBase = ChemReactOptimization.Services.ViewModelBase;

namespace ChemReactOptimization.ViewModel
{
    public class AuthorizationWindowViewModel : ViewModelBase
    {
        string _login = String.Empty;
        string _password = String.Empty;

        private readonly EFMethods _efMethods;
        private readonly EFTasks _efTasks; 
        private readonly EFUsers _efUsers;

        public AuthorizationWindowViewModel(EFMethods efMethods, EFTasks efTasks, EFUsers efUsers)
        {
            _efMethods = efMethods;
            _efTasks = efTasks;
            _efUsers = efUsers;
        }

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand LogInCommand
        {
            get
            {
                return new RelayCommand(r =>
                {
                    var userList = _efUsers.GetAllUsers();
                    var user = userList.FirstOrDefault(x => x.Login == Login);
                    if (user == null)
                    {
                        MessageBox.Show($"Введенные логин и (или) пароль некорректны.", "Авторизация",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        if (user.Password != Password)
                        {
                            MessageBox.Show($"Введенные логин и (или) пароль некорректны.", "Авторизация",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        else
                        {
                            if (user.Role == "Researcher")
                            {
                                var mainWindow = new MainWindow();
                                mainWindow.DataContext = new MainWindowViewModel(_efMethods,_efTasks,_efUsers);
                                mainWindow.Show();
                                Application.Current.MainWindow.Close();
                            }
                            else if (user.Role == "Administrator")
                            {
                                var adminWindow = new AdminWindow();
                                adminWindow.DataContext = new AdminWindowViewModel(_efMethods, _efTasks, _efUsers);
                                adminWindow.Show();
                                Application.Current.MainWindow.Close();
                            }
                        }
                    }
                });
            }
        }
    }
}
