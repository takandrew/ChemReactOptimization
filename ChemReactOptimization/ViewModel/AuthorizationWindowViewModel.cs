using ChemReactOptimization.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_MVVM_Classes;
using ViewModelBase = ChemReactOptimization.Services.ViewModelBase;

namespace ChemReactOptimization.ViewModel
{
    public class AuthorizationWindowViewModel : ViewModelBase
    {
        string _login = String.Empty;
        string _password = String.Empty;

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
                    
                });
            }
        }
    }
}
