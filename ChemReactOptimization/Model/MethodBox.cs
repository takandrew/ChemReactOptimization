using System.Collections.Generic;
using System.Linq;
using System;
using System.Windows;

namespace ChemReactOptimization.Model;

public class MethodBox
{
    private DataModel _dataModel = new DataModel();

    private int num_of_functioncalls = 0;

    /// <summary>
    /// Хранение значений комплекса для вывода на экран
    /// </summary>
    public struct Complex
    {
        /// <summary>
        /// номер комплекса
        /// </summary>
        public int NumberComplex;

        /// <summary>
        /// точки в комплекса 
        /// </summary>
        public double PointX;

        public double PointY;

        /// <summary>
        /// Значения точек
        /// </summary>
        public double Func;
    }

    public List<Complex> Complices;

    /// <summary>
    /// тип данных для хранения наилучшей и наихудшей вершины
    /// </summary>
    struct ExtrPoint
    {
        /// <summary>
        /// // 0 - если плохая вершина, 1 - если вершина хорошая
        /// </summary>
        public int Flag;

        /// <summary>
        /// индекс в массиве точек комплекса
        /// </summary>
        public int Index;

        /// <summary>
        /// значение функции в этой точке
        /// </summary>
        public double ValueFunc;

        /// <summary>
        /// координаты точки
        /// </summary>
        public Point ValuePoint;
    }

    /// <summary>
    /// массив для хранения исходных вершин
    /// </summary>
    private static Point[] _StartPoints { get; set; }

    /// <summary>
    /// массив для хранения активного комплекса
    /// </summary>
    private static Point[] _ComplexPoints { get; set; }

    /// <summary>
    /// массив для хранения точек, не выполняющих условия
    /// </summary>
    private Point[] _ErrorPoints { get; set; }

    private double[] _ValuesFunc { get; set; }

    private void SearchPoints(ref bool flag, ref int countErrorPoints, ref int countComplexPoints, int countPoint)
    {
        countComplexPoints = 0;
        countErrorPoints = 0;
        Random random = new Random();
        Complices = new List<Complex>();

        // находим начальные точки
        for (int i = 0; i < countPoint; i++)
        {
            _StartPoints[i] = new Point(
                _dataModel.T1Min + random.NextDouble() * (_dataModel.T1Max - _dataModel.T1Min),
                _dataModel.T2Min + random.NextDouble() * (_dataModel.T2Max - _dataModel.T2Min));
        }

        flag = true; // false если хотя бы одна вершина удовлетворяет условиям

        for (int i = 0; i < countPoint; i++)
        {

            // проверяем что найденная вершина удовлетворяет ограничениям второго рода
            if (_StartPoints[i].X + _StartPoints[i].Y <= _dataModel.TSumMax)
            {
                _ComplexPoints[countComplexPoints] = new Point(_StartPoints[i].X, _StartPoints[i].Y);

                flag = false;
                countComplexPoints++;
            }
            else
            {
                _ErrorPoints[countErrorPoints] = new Point(_StartPoints[i].X, _StartPoints[i].Y);

                countErrorPoints++;
            }
        }
    }

    public List<Point3D> Calc(out List<Point3D> points)
    {
        points = new List<Point3D>();

        // определяем количество вершин комплекса
        var countPoint = 0;
        if (_dataModel.N <= 5)
            countPoint = (int)_dataModel.N * 2;
        else
            countPoint = (int)_dataModel.N + 1;

        _StartPoints = new Point[countPoint]; // массив исходных точек
        _ComplexPoints = new Point[countPoint];
        _ErrorPoints = new Point[countPoint];
        _ValuesFunc = new double[countPoint];



        bool flag = true; // false - если хотя бы одна вершина удовлетворяет условиям
        int countErrorPoints = 0;
        int countComplexPoints = 0;

        while (flag)
        {
            SearchPoints(ref flag, ref countErrorPoints, ref countComplexPoints, countPoint);

        }

        double sumComplexPointsX = 0;
        double sumComplexPointsY = 0;

        //считаем сумму значений по каждой координате
        for (int i = 0; i < countComplexPoints; i++)
        {
            sumComplexPointsX += _ComplexPoints[i].X;
            sumComplexPointsY += _ComplexPoints[i].Y;
        }

        // исправление вершин, которые не выполняют ограничения
        for (int i = 0; i < countErrorPoints; i++)
        {
            _ErrorPoints[i].X = 0.5 * (_ErrorPoints[i].X + (1 / (countComplexPoints)) * sumComplexPointsX);
            _ErrorPoints[i].Y = 0.5 * (_ErrorPoints[i].Y + (1 / (countComplexPoints)) * sumComplexPointsY);

            if (_ErrorPoints[i].X + _ErrorPoints[i].Y <=
                _dataModel.TSumMax) // проверяем что в найденной вершине выполняются ограничения второго рода
            {
                _ComplexPoints[countComplexPoints] = new Point(_ErrorPoints[i].X, _ErrorPoints[i].Y);

                countComplexPoints++;
            }

            else
            {
                i -= 1;
            }
        }

        // вычисление значений функции в вершинах комплекса
        for (int i = 0; i < _ComplexPoints.Length; i++)
        {
            _ValuesFunc[i] = MathModel.TargetFunction(_dataModel, _ComplexPoints[i].X, _ComplexPoints[i].Y);
            num_of_functioncalls++;
        }

        int number = 0;

        while (true)
        {
            for (int i = 0; i < _ComplexPoints.Length; i++)
            {
                Complices.Add(new Complex
                {
                    NumberComplex = number,
                    PointX = _ComplexPoints[i].X,
                    PointY = _ComplexPoints[i].Y,
                    Func = _ValuesFunc[i]
                });
            }

            number++;

            double[] sortValuesFunc = new double[_ValuesFunc.Length];

            for (int i = 0; i < _ValuesFunc.Length; i++)
            {
                sortValuesFunc[i] = _ValuesFunc[i];
            }

            Array.Sort(sortValuesFunc);

            var extrPoint = new ExtrPoint[2]; // массив для хранения самой "хорошей" и самой "плохой" вершины

            // запоминаем точки самой "хорошей" и самой "плохой" вершины
            for (int i = 0; i < _ValuesFunc.Length; i++)
            {
                if (_ValuesFunc[i] == sortValuesFunc[0])
                {
                    extrPoint[0].Flag = 1;
                    extrPoint[0].Index = i;
                    extrPoint[0].ValueFunc = _ValuesFunc[i];
                    extrPoint[0].ValuePoint = _ComplexPoints[i];
                }

                else if (_ValuesFunc[i] == sortValuesFunc[sortValuesFunc.Length - 1])
                {
                    extrPoint[extrPoint.Length - 1].Flag = 0;
                    extrPoint[extrPoint.Length - 1].Index = i;
                    extrPoint[extrPoint.Length - 1].ValueFunc = _ValuesFunc[i];
                    extrPoint[extrPoint.Length - 1].ValuePoint = _ComplexPoints[i];
                }
            }

            var centerPoint = new Point(); // координата центра комплекса

            double
                sumMinusExtrValX =
                    0; // хранение суммы значений вершин по Х минус худшая вершина для промежуточных вычислений
            double
                sumMinusExtrValY =
                    0; // хранение суммы значений вершин по Х минус худшая вершина для промежуточных вычислений


            // вычисление промежуточной суммы для координаты центра комплекса
            for (int i = 0; i < _ComplexPoints.Length; i++)
            {
                sumMinusExtrValX += _ComplexPoints[i].X;
                sumMinusExtrValY += _ComplexPoints[i].Y;
            }

            // координаты центра комплекса
            centerPoint.X = 1.0 / (countPoint - 1) *
                            (sumMinusExtrValX - extrPoint.Last(x => x.Flag == 0).ValuePoint.X);
            centerPoint.Y = 1.0 / (countPoint - 1) *
                            (sumMinusExtrValY - extrPoint.Last(x => x.Flag == 0).ValuePoint.Y);

            double sumB = 0; // хранение суммы для проверки окончания поиска

            sumB += Math.Abs((centerPoint.X - extrPoint.Last(x => x.Flag == 0).ValuePoint.X)) +
                    Math.Abs((centerPoint.X - extrPoint.Last(x => x.Flag == 1).ValuePoint.X));
            sumB += Math.Abs((centerPoint.Y - extrPoint.Last(x => x.Flag == 0).ValuePoint.Y)) +
                    Math.Abs((centerPoint.Y - extrPoint.Last(x => x.Flag == 1).ValuePoint.Y));

            double B = 1.0 / (2 * _dataModel.N) * sumB;



            if (B < 0.1)
            {
                var point = new Point3D
                {
                    X = Math.Round(centerPoint.X, 4),
                    Y = Math.Round(centerPoint.Y, 4),
                    Z = Math.Round(MathModel.TargetFunction(_dataModel, centerPoint.X, centerPoint.Y), 4)
                };
                num_of_functioncalls++;
                points.Add(point);
                return points;
            }

            else
            {
                var newPoint = new Point
                {
                    X = 2.3 * centerPoint.X - 1.3 * extrPoint.Last(x => x.Flag == 0).ValuePoint.X,
                    Y = 2.3 * centerPoint.Y - 1.3 * extrPoint.Last(x => x.Flag == 0).ValuePoint.Y
                }; // новая координата взамен наихудшей

                // проверям ограничений первого рода                
                if (_dataModel.T1Min > newPoint.X)
                {
                    newPoint.X = _dataModel.T1Min + 0.1;
                }
                else if (newPoint.X > _dataModel.T2Max)
                {
                    newPoint.X = _dataModel.T1Min - 0.1;
                }

                if (_dataModel.T1Max > newPoint.Y)
                {
                    newPoint.Y = _dataModel.T2Max + 0.1;
                }
                else if (newPoint.Y > _dataModel.T2Max)
                {
                    newPoint.Y = _dataModel.T2Max - 0.1;
                }

                // проверка ограничений второго рода
                // пока ограничение не выполняется смещаем координату к центру
                while ((newPoint.X + newPoint.Y) > _dataModel.TSumMax)
                {
                    newPoint.X = 0.5 * (newPoint.X + centerPoint.X);
                    newPoint.Y = 0.5 * (newPoint.Y + centerPoint.Y);
                }

                // вычисляем значение функции в новой точке
                double newPointF = MathModel.TargetFunction(_dataModel, newPoint.X, newPoint.Y);
                num_of_functioncalls++;

                while (newPointF > extrPoint.Last(x => x.Flag == 0).ValueFunc)
                {
                    newPoint.X = 0.5 * (newPoint.X + extrPoint.Last(x => x.Flag == 1).ValuePoint.X);
                    newPoint.Y = 0.5 * (newPoint.Y + extrPoint.Last(x => x.Flag == 1).ValuePoint.Y);
                    newPointF = MathModel.TargetFunction(_dataModel, newPoint.X, newPoint.Y);
                    num_of_functioncalls++;
                }

                // записываем значения новой точке в массив вершин Комплекса
                _ComplexPoints[extrPoint.Last(x => x.Flag == 0).Index] = newPoint;
                _ValuesFunc[extrPoint.Last(x => x.Flag == 0).Index] = newPointF;
                points.Add(new Point3D(Math.Round(newPoint.X, 4), Math.Round(newPoint.Y, 4), Math.Round(newPointF, 4)));
            }
        }
    }

    public MethodBox(DataModel dataModel, out List<Point3D> points3D)
    {
        _dataModel = dataModel;
        points3D = Calc(out points3D);
        var temp = new List<double>();

        foreach (var item in points3D)
        {
            temp.Add(item.Z);
        }

        MessageBox.Show($"Т1: {points3D.Find(x => x.Z == temp.Min()).X}\n" +
                        $"Т2: {points3D.Find(x => x.Z == temp.Min()).Y}\n" +
                        $"Минимум: {temp.Min()}\n" +
                        $"Количество вызовов функции: {num_of_functioncalls}\n");

        num_of_functioncalls = 0;
    }
}