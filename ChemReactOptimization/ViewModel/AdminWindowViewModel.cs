using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ChemReactOptimization.Data;
using ChemReactOptimization.Model;
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
}