using ChemReactOptimization.Data;
using ChemReactOptimization.Model;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using WPF_MVVM_Classes;
using ViewModelBase = ChemReactOptimization.Services.ViewModelBase;

public class UserUpdateInsertWindowViewModel : ViewModelBase
{
    private readonly EFUsers _efUsers;


    private User _gotUser;
    private readonly AdminWindowViewModel _adminWindowViewModel;
    private User _edUser = new User();

    private StringBuilder errorStringBuilder = new StringBuilder();

    private IEnumerable<string> _roleList = new List<string>
    {
        "Administrator",
        "Researcher"
    };

    private string _userUpdateInsertWindowTitle;

    public UserUpdateInsertWindowViewModel( EFUsers efUsers, User gotUser, AdminWindowViewModel adminWindowViewModel)
    {
        _efUsers = efUsers;

        _gotUser = gotUser;
        _adminWindowViewModel = adminWindowViewModel;

        if (gotUser == null)
        {
            UserUpdateInsertWindowTitle = "Добавление пользователя";
        }
        else
        {
            UserUpdateInsertWindowTitle = "Изменение пользователя";
            EdUser = gotUser;
        }
    }

    public string UserUpdateInsertWindowTitle
    {
        get => _userUpdateInsertWindowTitle;
        set
        {
            _userUpdateInsertWindowTitle = value;
            OnPropertyChanged();
        }
    }

    public User EdUser
    {
        get => _edUser;
        set
        {
            _edUser = value;
            OnPropertyChanged();
        }
    }

    public IEnumerable<string> RoleList
    {
        get => _roleList;
        set
        {
            _roleList = value;
            OnPropertyChanged();
        }
    }

    public RelayCommand DoneCommand
    {
        get => new RelayCommand(x =>
        {
            VerifyData();
            if (errorStringBuilder.Length != 0)
            {
                MessageBox.Show(errorStringBuilder.ToString(),
                    "Добавление пользователя", MessageBoxButton.OK, MessageBoxImage.Error);
                errorStringBuilder.Clear();
                return;
            }
            if (_gotUser == null)
            {
                _efUsers.SaveUser(EdUser);
                MessageBox.Show($"Пользователь успешно добавлен",
                    "Добавление пользователя", MessageBoxButton.OK, MessageBoxImage.Information);
                _adminWindowViewModel.UpdateUserList();
                return;
            }
            else
            {
                _efUsers.SaveUser(EdUser);
                MessageBox.Show($"Пользователь успешно изменен",
                    "Изменение пользователя", MessageBoxButton.OK, MessageBoxImage.Information);
                _adminWindowViewModel.UpdateUserList();
                return;
            }
        });
    }

    private void VerifyData()
    {
        if (string.IsNullOrWhiteSpace(EdUser.Name))
            errorStringBuilder.Append("Имя пользователя не введено.\n");
        if (string.IsNullOrWhiteSpace(EdUser.Role))
            errorStringBuilder.Append("Роль не введена.\n");
        if (string.IsNullOrWhiteSpace(EdUser.Login))
            errorStringBuilder.Append("Логин не введен.\n");
        if (string.IsNullOrWhiteSpace(EdUser.Password))
            errorStringBuilder.Append("Пароль не введен.\n");
    }

}