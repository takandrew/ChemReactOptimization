using ChemReactOptimization.Data;
using ChemReactOptimization.Model;
using System.Text;
using System.Windows;
using WPF_MVVM_Classes;
using ViewModelBase = ChemReactOptimization.Services.ViewModelBase;

public class TaskUpdateInsertWindowViewModel : ViewModelBase
{
    private readonly EFTasks _efTask;

    private Task _gotTask;
    private readonly AdminWindowViewModel _adminWindowViewModel;
    private Task _edTask = new Task();

    private StringBuilder errorStringBuilder = new StringBuilder();

    private string _taskUpdateInsertWindowTitle;

    public TaskUpdateInsertWindowViewModel(EFTasks efTask, Task gotTask, AdminWindowViewModel adminWindowViewModel)
    {
        _efTask = efTask;

        _gotTask = gotTask;
        _adminWindowViewModel = adminWindowViewModel;

        if (gotTask == null)
        {
            TaskUpdateInsertWindowTitle = "Добавление состояния";
        }
        else
        {
            TaskUpdateInsertWindowTitle = "Изменение состояния";
            EdTask = gotTask;
        }
    }

    public string TaskUpdateInsertWindowTitle
    {
        get => _taskUpdateInsertWindowTitle;
        set
        {
            _taskUpdateInsertWindowTitle = value;
            OnPropertyChanged();
        }
    }

    public Task EdTask
    {
        get => _edTask;
        set
        {
            _edTask = value;
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
                    "Добавление состояния", MessageBoxButton.OK, MessageBoxImage.Error);
                errorStringBuilder.Clear();
                return;
            }
            if (_gotTask == null)
            {
                _efTask.SaveTask(EdTask);
                MessageBox.Show($"Состояние успешно добавлено",
                    "Добавление состояния", MessageBoxButton.OK, MessageBoxImage.Information);
                _adminWindowViewModel.UpdateTaskList();
                return;
            }
            else
            {
                _efTask.SaveTask(EdTask);
                MessageBox.Show($"Состояние успешно изменено",
                    "Изменение состояния", MessageBoxButton.OK, MessageBoxImage.Information);
                _adminWindowViewModel.UpdateTaskList();
                return;
            }
        });
    }

    private void VerifyData()
    {
        if (string.IsNullOrWhiteSpace(EdTask.Name))
            errorStringBuilder.Append("Наименование не введено.\n");
        if (EdTask.T1Min > EdTask.T1Max)
            errorStringBuilder.Append("Минимальное значение Т1 не может быть больше максимального.\n");
        if (EdTask.T2Min > EdTask.T2Max)
            errorStringBuilder.Append("Минимальное значение Т2 не может быть больше максимального.\n");
        if (EdTask.TSumMax <= 0)
            errorStringBuilder.Append("Сумма Т1 и Т2 должна быть положительной.\n");
        if (EdTask.N <= 0)
            errorStringBuilder.Append("Количество теплообменных устройств должно быть положительным.\n");
    }
}