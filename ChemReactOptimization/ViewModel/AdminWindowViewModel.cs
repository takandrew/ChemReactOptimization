using ChemReactOptimization;
using ChemReactOptimization.Data;
using ChemReactOptimization.Model;
using System.Collections.Generic;
using System.Windows;
using WPF_MVVM_Classes;
using ViewModelBase = ChemReactOptimization.Services.ViewModelBase;

public class AdminWindowViewModel : ViewModelBase
{
    private readonly EFMethods _efMethods;
    private readonly EFTasks _efTasks;
    private readonly EFUsers _efUsers;

    private IEnumerable<Method> _methodList = new List<Method>();
    private IEnumerable<Task> _taskList = new List<Task>();
    private IEnumerable<User> _userList = new List<User>();
    private Task _selectedTask;
    private Method _selectedMethod;
    private User _selectedUser;

    public AdminWindowViewModel(EFMethods efMethods, EFTasks efTasks, EFUsers efUsers)
    {
        _efMethods = efMethods;
        _efTasks = efTasks;
        _efUsers = efUsers;

        _methodList = efMethods.GetAllMethods();
        _taskList = efTasks.GetAllTasks();
        _userList = efUsers.GetAllUsers();
    }

    public IEnumerable<Method> MethodList
    {
        get => _methodList;
        set
        {
            _methodList = value;
            OnPropertyChanged();
        }
    }
    public IEnumerable<Task> TaskList
    {
        get => _taskList;
        set
        {
            _taskList = value;
            OnPropertyChanged();
        }
    }
    public IEnumerable<User> UserList
    {
        get => _userList;
        set
        {
            _userList = value;
            OnPropertyChanged();
        }
    }
    public Task SelectedTask
    {
        get => _selectedTask;
        set
        {
            _selectedTask = value;
            OnPropertyChanged();
        }
    }
    public Method SelectedMethod
    {
        get => _selectedMethod;
        set
        {
            _selectedMethod = value;
            OnPropertyChanged();
        }
    }
    public User SelectedUser
    {
        get => _selectedUser;
        set
        {
            _selectedUser = value;
            OnPropertyChanged();
        }
    }

    public RelayCommand TaskDataGridInsert
    {
        get => new RelayCommand(x =>
        {
            var taskUpdateInsertWindow = new TaskUpdateInsertWindow();
            taskUpdateInsertWindow.DataContext = new TaskUpdateInsertWindowViewModel(_efTasks, null, this);
            taskUpdateInsertWindow.Show();
        });
    }
    public RelayCommand TaskDataGridUpdate
    {
        get => new RelayCommand(x =>
        {
            if (SelectedTask == null)
            {
                MessageBox.Show($"Выберите состояние, которое необходимо изменить.",
                    "Изменение состояния", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var taskUpdateInsertWindow = new TaskUpdateInsertWindow();
            taskUpdateInsertWindow.DataContext = new TaskUpdateInsertWindowViewModel(_efTasks, SelectedTask, this);
            taskUpdateInsertWindow.Show();
        });
    }
    public RelayCommand TaskDataGridDelete
    {
        get => new RelayCommand(x =>
        {
            if (SelectedTask == null)
            {
                MessageBox.Show($"Выберите состояние, который необходимо удалить.",
                    "Удаление состояния", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (MessageBox.Show($"Вы уверены, что хотите удалить состояние с идентификатором {SelectedTask.Id}?",
                    "Удаление состояния", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                _efTasks.DeleteTask(SelectedTask.Id);
                MessageBox.Show($"Состояние успешно удалено.",
                    "Удаление состояния", MessageBoxButton.OK, MessageBoxImage.Information);
                TaskList = _efTasks.GetAllTasks();
            }
        });
    }
    public RelayCommand MethodDataGridInsert
    {
        get => new RelayCommand(x =>
        {
            var methodUpdateInsertWindow = new MethodUpdateInsertWindow();
            methodUpdateInsertWindow.DataContext = new MethodUpdateWindowViewModel(_efMethods, null, this);
            methodUpdateInsertWindow.Show();
        });
    }
    public RelayCommand MethodDataGridUpdate
    {
        get => new RelayCommand(x =>
        {
            if (SelectedMethod == null)
            {
                MessageBox.Show($"Выберите метод, который необходимо изменить.",
                    "Изменение метода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var methodUpdateInsertWindow = new MethodUpdateInsertWindow();
            methodUpdateInsertWindow.DataContext = new MethodUpdateWindowViewModel(_efMethods, SelectedMethod, this);
            methodUpdateInsertWindow.Show();
        });
    }
    public RelayCommand MethodDataGridDelete
    {
        get => new RelayCommand(x =>
        {
            if (SelectedMethod == null)
            {
                MessageBox.Show($"Выберите метод, который необходимо удалить.",
                    "Удаление метода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (MessageBox.Show($"Вы уверены, что хотите удалить метод с идентификатором {SelectedMethod.Id}?",
                    "Удаление метода", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                _efMethods.DeleteMethod(SelectedMethod.Id);
                MessageBox.Show($"Метод успешно удален.",
                    "Удаление метода", MessageBoxButton.OK, MessageBoxImage.Information);
                MethodList = _efMethods.GetAllMethods();
            }
        });
    }
    public RelayCommand UserDataGridInsert
    {
        get => new RelayCommand(x =>
        {
            var userUpdateInsertWindow = new UserUpdateInsertWindow();
            userUpdateInsertWindow.DataContext = new UserUpdateInsertWindowViewModel(_efUsers, null, this);
            userUpdateInsertWindow.Show();
        });
    }
    public RelayCommand UserDataGridUpdate
    {
        get => new RelayCommand(x =>
        {
            if (SelectedUser == null)
            {
                MessageBox.Show($"Выберите пользователя, которого необходимо изменить.",
                    "Изменение пользователя", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var userUpdateInsertWindow = new UserUpdateInsertWindow();
            userUpdateInsertWindow.DataContext = new UserUpdateInsertWindowViewModel(_efUsers, SelectedUser, this);
            userUpdateInsertWindow.Show();
        });
    }
    public RelayCommand UserDataGridDelete
    {
        get => new RelayCommand(x =>
        {
            if (SelectedUser == null)
            {
                MessageBox.Show($"Выберите пользователя, которого необходимо удалить.",
                    "Удаление пользователя", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (MessageBox.Show($"Вы уверены, что хотите удалить пользователя с идентификатором {SelectedUser.Id}?",
                    "Удаление пользователя", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                _efUsers.DeleteUser(SelectedUser.Id);
                MessageBox.Show($"Пользователь успешно удален.",
                    "Удаление пользователя", MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateUserList();
            }
        });
    }

    public void UpdateUserList()
    {
        UserList = _efUsers.GetAllUsers();
    }
    public void UpdateMethodList()
    {
        MethodList = _efMethods.GetAllMethods();
    }
    public void UpdateTaskList()
    {
        TaskList = _efTasks.GetAllTasks();
    }
}