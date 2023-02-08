using ChemReactOptimization.Data;
using ChemReactOptimization.Model;
using System.Text;
using System.Windows;
using WPF_MVVM_Classes;
using ViewModelBase = ChemReactOptimization.Services.ViewModelBase;

public class MethodUpdateWindowViewModel : ViewModelBase
{
    private readonly EFMethods _efMethods;

    private Method _gotMethod;
    private readonly AdminWindowViewModel _adminWindowViewModel;
    private Method _edMethod = new Method();

    private StringBuilder errorStringBuilder = new StringBuilder();

    private string _methodUpdateInsertWindowTitle;

    public MethodUpdateWindowViewModel(EFMethods efMethods, Method gotMethod, AdminWindowViewModel adminWindowViewModel)
    {
        _efMethods = efMethods;

        _gotMethod = gotMethod;
        _adminWindowViewModel = adminWindowViewModel;

        if (gotMethod == null)
        {
            MethodUpdateInsertWindowTitle = "Добавление метода";
        }
        else
        {
            MethodUpdateInsertWindowTitle = "Изменение метода";
            EdMethod = gotMethod;
        }
    }

    public string MethodUpdateInsertWindowTitle
    {
        get => _methodUpdateInsertWindowTitle;
        set
        {
            _methodUpdateInsertWindowTitle = value;
            OnPropertyChanged();
        }
    }

    public Method EdMethod
    {
        get => _edMethod;
        set
        {
            _edMethod = value;
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
                    "Добавление метода", MessageBoxButton.OK, MessageBoxImage.Error);
                errorStringBuilder.Clear();
                return;
            }
            if (_gotMethod == null)
            {
                _efMethods.SaveMethod(EdMethod);
                MessageBox.Show($"Метод успешно добавлен",
                    "Добавление метода", MessageBoxButton.OK, MessageBoxImage.Information);
                _adminWindowViewModel.UpdateMethodList();
                return;
            }
            else
            {
                _efMethods.SaveMethod(EdMethod);
                MessageBox.Show($"Метод успешно изменен",
                    "Изменение метода", MessageBoxButton.OK, MessageBoxImage.Information);
                _adminWindowViewModel.UpdateUserList();
                return;
            }
        });
    }

    private void VerifyData()
    {
        if (string.IsNullOrWhiteSpace(EdMethod.Name))
            errorStringBuilder.Append("Наименование не введено.\n");
    }
}