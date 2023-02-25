using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ChemReactOptimization.Data;
using ChemReactOptimization.Model;
using WPF_MVVM_Classes;
using ViewModelBase = ChemReactOptimization.Services.ViewModelBase;
using Excel = Microsoft.Office.Interop.Excel;

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
        private IEnumerable<Point3D> _dataList;

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

        public IEnumerable<Point3D> DataList
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
                        var errString = new StringBuilder();
                        if (DataModel.Alpha <= 0)
                            errString.Append("α должен быть положительным.\n");
                        if (DataModel.Beta <= 0)
                            errString.Append("β должен быть положительным.\n");
                        if (DataModel.Mu <= 0)
                            errString.Append("μ должен быть положительным.\n");
                        if (DataModel.Delta <= 0)
                            errString.Append("Δ должен быть положительным.\n");
                        if (DataModel.G <= 0)
                            errString.Append("G должен быть положительным.\n");
                        if (DataModel.A <= 0)
                            errString.Append("A должен быть положительным.\n");
                        if (DataModel.N <= 0)
                            errString.Append("N должен быть положительным.\n");
                        if (DataModel.T1Min > DataModel.T1Max)
                            errString.Append("Минимальное значение Т1 не может быть больше максимального.\n");
                        if (DataModel.T2Min > DataModel.T2Max)
                            errString.Append("Минимальное значение Т2 не может быть больше максимального.\n");
                        if (DataModel.TSumMax <= 0)
                            errString.Append("Сумма Т1 и Т2 должна быть положительной.");
                        if (errString.Length > 0)
                        {
                            MessageBox.Show(errString.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            errString.Clear();
                        }
                        else
                        {
                            if (MethodSelected.Id == MethodList.FirstOrDefault(x => x.Name.Contains("Нелдер")).Id)
                            {
                                MethodNelderMead.Start(DataModel, out var point3D);
                                DataList = point3D;
                            }
                            else if (MethodSelected.Id == MethodList.FirstOrDefault(x => x.Name.Contains("Бокс")).Id)
                            {
                                var methodbox = new MethodBox(DataModel, out var point3D);
                                DataList = point3D;
                            }
                            else if (MethodSelected.Id == MethodList.FirstOrDefault(x => x.Name.Contains("скан")).Id)
                            {
                                var methodscan = new MethodScan(DataModel, out var point3D);
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

        public RelayCommand ExportCommand
        {
            get
            {
                return new RelayCommand(r =>
                {
                    if (DataList is null || !DataList.Any())
                    {
                        MessageBox.Show("Для экспорта отчета необходимо произвести расчеты.", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var data = DataList as List<Point3D>;
                    var temp = new List<double>();

                    foreach (var item in data)
                    {
                        temp.Add(item.Z);
                    }

                    var excelApp = new Excel.Application
                    {
                        SheetsInNewWorkbook = 1
                    };

                    var workBook = excelApp.Workbooks.Add();
                    var workSheet = (Excel.Worksheet)workBook.Worksheets.Item[1];
                    workSheet.Name = "Результаты";

                    workSheet.Cells[1, 3] = "Температура в змеевике, °C";
                    workSheet.Cells[1, 4] = "Температура в диффузоре, °C";
                    workSheet.Cells[1, 5] = "Себестоимость продукта, у.е.";

                    workSheet.Cells[1, 1] = "Входные данные"; workSheet.Cells[1, 1].Font.Bold = true;
                    workSheet.Cells[1, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 2]].Merge();

                    workSheet.Cells[2, 1] = "Задание:";
                    workSheet.Cells[2, 1].Font.Bold = true;
                    workSheet.Cells[2, 2] = TaskSelected.Name;

                    workSheet.Cells[4, 1] = "Нормирующий множитель α";
                    workSheet.Cells[4, 2] = DataModel.Alpha;
                    workSheet.Cells[5, 1] = "Нормирующий множитель β";
                    workSheet.Cells[5, 2] = DataModel.Beta;
                    workSheet.Cells[6, 1] = "Нормирующий множитель μ";
                    workSheet.Cells[6, 2] = DataModel.Mu;
                    workSheet.Cells[7, 1] = "Нормирующий множитель Δ";
                    workSheet.Cells[7, 2] = DataModel.Delta;
                    workSheet.Cells[8, 1] = "Расход реакционной массы, кг/ч";
                    workSheet.Cells[8, 2] = DataModel.G;
                    workSheet.Cells[9, 1] = "Давление в реакторе, КПа";
                    workSheet.Cells[9, 2] = DataModel.A;
                    workSheet.Cells[10, 1] = "Количество теплообменных устройств , шт";
                    workSheet.Cells[10, 2] = DataModel.N;

                    workSheet.Cells[11, 1] = "Ограничения";
                    workSheet.Cells[11, 1].Font.Bold = true;
                    workSheet.Range[workSheet.Cells[10, 1], workSheet.Cells[10, 2]].Merge();

                    workSheet.Cells[12, 1] = "Минимальная температура в змеевике Т1, °C";
                    workSheet.Cells[12, 2] = DataModel.T1Min;
                    workSheet.Cells[13, 1] = "Максимальная температура в змеевике Т1, °C";
                    workSheet.Cells[13, 2] = DataModel.T1Max;

                    workSheet.Cells[14, 1] = "Минимальная температура в диффузоре Т2, °C";
                    workSheet.Cells[14, 2] = DataModel.T2Min;
                    workSheet.Cells[15, 1] = "Максимальная температура в диффузоре Т2, °C";
                    workSheet.Cells[15, 2] = DataModel.T2Max;

                    workSheet.Cells[16, 1] = "Разница температур Т2-Т1, °C";
                    workSheet.Cells[16, 2] = DataModel.TSumMax;

                    workSheet.Cells[17, 1] = "Себестоимость 1 кг. компонента, у.е.";
                    workSheet.Cells[17, 2] = 10;

                    workSheet.Cells[18, 1] = "Точность решения, у.е.";
                    workSheet.Cells[18, 2] = 0.01;

                    workSheet.Cells[19, 1] = "Выбранный метод решения";
                    workSheet.Cells[19, 1].Font.Bold = true;
                    workSheet.Cells[19, 2] = MethodSelected.Name;

                    workSheet.Cells[20, 1] = "Результаты расчета";
                    workSheet.Cells[20, 1].Font.Bold = true;
                    workSheet.Cells[20, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    workSheet.Range[workSheet.Cells[20, 1], workSheet.Cells[20, 2]].Merge();

                    workSheet.Cells[21, 1] = "Температура в змеевике Т1, °C";
                    workSheet.Cells[21, 2] = data.Find(x => x.Z == temp.Min()).X;
                    workSheet.Cells[22, 1] = "Температура в змеевике Т1, °C";
                    workSheet.Cells[22, 2] = data.Find(x => x.Z == temp.Min()).Y;
                    workSheet.Cells[23, 1] = "Минимальная себестоимость, у.е.";
                    workSheet.Cells[23, 2] = temp.Min();

                    for (int i = 2; i <= data.Count + 1; i++)
                    {
                        workSheet.Cells[i, 3] = data[i - 2].X;
                        workSheet.Cells[i, 4] = data[i - 2].Y;
                        workSheet.Cells[i, 5] = data[i - 2].Z;
                    }

                    workSheet.Columns.AutoFit();
                    excelApp.Visible = true;
                });
            }
        }
    }
}
