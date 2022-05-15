using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ChemReactOptimization.Model;
using WPF_MVVM_Classes;
using ViewModelBase = ChemReactOptimization.Services.ViewModelBase;

namespace ChemReactOptimization.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {

        private RelayCommand? _startButtonCommand;
        private IEnumerable _dataList;

        private DataModel _dataModel = new DataModel()
        {
            Alpha = 1,
            Beta = 1,
            Mu = 1, 
            Delta = 1,
            G = 1,
            A = 1, 
            N = 2,
            T1Min = -18,
            T1Max = 7,
            T2Min = -8, 
            T2Max = 8,
            TSumMax = 4
        };

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

        public RelayCommand StartButtonCommand
        {
            get
            {
                return _startButtonCommand ??= new RelayCommand(c =>
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
                        OptimizationMethod.Start(DataModel, out var point3D);
                        DataList = point3D;
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
