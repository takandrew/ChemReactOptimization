using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using ChemReactOptimization.Model;
using WPF_MVVM_Classes;
using ViewModelBase = ChemReactOptimization.Services.ViewModelBase;

namespace ChemReactOptimization.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {

        //private RelayCommand? _calculateCommand;
        //private IEnumerable _dataList;
        //private List<Point3D> _point3D = new();

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

        //public IEnumerable DataList
        //{
        //    get => _dataList;
        //    set
        //    {
        //        _dataList = value;
        //        OnPropertyChanged();
        //    }
        //}

        //public RelayCommand CalculateCommand
        //{
        //    get
        //    {
        //        return _calculateCommand ??= new RelayCommand(c =>
        //        {
                    
        //        });
        //    }
        //}

    }
}
