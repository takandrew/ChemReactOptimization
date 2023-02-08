using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ChemReactOptimization.Data;
using ChemReactOptimization.Model;
using WPF_MVVM_Classes;
using ViewModelBase = ChemReactOptimization.Services.ViewModelBase;

namespace ChemReactOptimization.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly EFMethods _efMethods;
        private readonly EFTasks _efTasks;
        private readonly EFUsers _efUsers;

        private IEnumerable<Method> _methodList = new List<Method>();
        private IEnumerable<Task> _taskList = new List<Task>();

        public MainWindowViewModel(EFMethods efMethods, EFTasks efTasks, EFUsers efUsers)
        {
            _efMethods = efMethods;
            _efTasks = efTasks;
            _efUsers = efUsers;

            _methodList = efMethods.GetAllMethods();
            _taskList = efTasks.GetAllTasks();
        }

        private RelayCommand? _startButtonCommand;
        private IEnumerable _dataList;

        private Method _methodSelected = new Method();
        private Task _taskSelected = new Task();

        private DataModel _dataModel = new DataModel();

        public DataModel DataModel
        {
            get => _dataModel;
            set
            {
                _dataModel = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable DataList
        {
            get => _dataList;
            set
            {
                _dataList = value;
                OnPropertyChanged();
            }
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

        public Method MethodSelected
        {
            get => _methodSelected;
            set
            {
                _methodSelected = value;
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

        public Task TaskSelected
        {
            get => _taskSelected;
            set
            {
                _taskSelected = value;
                DataModel = new DataModel
                {
                    Alpha = TaskSelected.Alpha,
                    Beta = TaskSelected.Beta,
                    Mu = TaskSelected.Mu,
                    Delta = TaskSelected.Delta,
                    G = TaskSelected.G,
                    A = TaskSelected.A,
                    N = TaskSelected.N,
                    T1Min = TaskSelected.T1Min,
                    T1Max = TaskSelected.T1Max,
                    T2Min = TaskSelected.T2Min,
                    T2Max = TaskSelected.T2Max,
                    TSumMax = TaskSelected.TSumMax
                };
                OnPropertyChanged();
            }
        }

        public RelayCommand StartButtonCommand
        {
            get
            {
                return _startButtonCommand ??= new RelayCommand(c =>
                {
                    if (TaskSelected.Name != null)
                    {
                        string errString = String.Empty;
                        if (DataModel.T1Min > DataModel.T1Max)
                            errString += "Минимальное значение Т1 не может быть больше максимального\n";
                        if (DataModel.T2Min > DataModel.T2Max)
                            errString += "Минимальное значение Т2 не может быть больше максимального\n";
                        if (DataModel.TSumMax <= 0)
                            errString += "Сумма Т1 и Т2 должна быть положительной";
                        if (errString.Length > 0)
                            MessageBox.Show(errString, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                        {
                            if (MethodSelected.Id == MethodList.FirstOrDefault(x => x.Name.Contains("Нелдер")).Id)
                            {
                                MethodNelderMead.Start(DataModel, out var point3D);
                                DataList = point3D;
                            }
                            else
                            {
                                MessageBox.Show("Данный метод еще не реализован в программном комплексе", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Выберите состояние.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });
            }
        }

        public RelayCommand Chart2DCommand
        {
            get
            {
                return new RelayCommand(r =>
                {
                    var test = new Chart2DWindow(DataList as List<Point3D>, DataModel);
                    test.Show();
                });
            }
        }

        public RelayCommand Chart3DCommand
        {
            get
            {
                return new RelayCommand(r =>
                {
                    var test = new Chart3DWindow(DataList as List<Point3D>, DataModel);
                    test.Show();
                });
            }
        }

    }
}
