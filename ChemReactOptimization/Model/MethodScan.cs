using System.Collections.Generic;
using System;
using System.Linq;
using System.Windows;

namespace ChemReactOptimization.Model;

public class MethodScan
{
    private DataModel _dataModel = new DataModel();
    public double _step; 
    public double _k = 10;
    public double _r = 2;
    public double _n = 2;
    public double _epsilon = 0.01;

    private int num_of_functioncalls = 0;

    private bool Conditions(double t1, double t2)
    {
        return (t2 + t1) <= _dataModel.TSumMax;
    }

    public void Calculate(out List<Point3D> points3D)
    {
        double funcMin = double.MaxValue;
        _step = Math.Pow(_k, _r) * _epsilon;
        points3D = new List<Point3D>();
        var p3D = new List<Point3D>();
        List<double> values;
        Point newMin;
        var t1Min = _dataModel.T1Min;
        var t2Min = _dataModel.T2Min;
        var t1Max = _dataModel.T1Min;
        var t2Max = _dataModel.T2Min;

        newMin = SearchMinOnGrid(out p3D, out values);
        t1Min = newMin.X - _step;
        t2Min = newMin.Y - _step;

        t1Max = newMin.X + _step;
        t2Max = newMin.Y + _step;

        _step /= _k;
        points3D.AddRange(p3D);

        while (funcMin > values.Min())
        {
            newMin = SearchMinOnGrid(out p3D, out values);

            t1Min = newMin.X - _step;
            t2Min = newMin.Y - _step;

            t1Max = newMin.X + _step;
            t2Max = newMin.Y + _step;

            _step /= _k;
            funcMin = values.Min();
            points3D.AddRange(p3D);
        }
    }

    private Point SearchMinOnGrid(out List<Point3D> points3D, out List<double> values)
    {
        points3D = new List<Point3D>();

        for (var t1 = _dataModel.T1Min; t1 <= _dataModel.T1Max; t1 += _step)
            for (var t2 = _dataModel.T2Min; t2 <= _dataModel.T2Max; t2 += _step)
            {
                if (!Conditions(t1, t2))
                    continue;
                var value = MathModel.TargetFunction(_dataModel,t1,t2);
                num_of_functioncalls++;

                points3D.Add(new Point3D(Math.Round(t1, 4), Math.Round(t2, 4), Math.Round(value, 4)));

            }

        var valuesListTemp = points3D.Select(item => item.Z).ToList();
        values = valuesListTemp;
        return new Point(points3D.Find(x => x.Z == valuesListTemp.Min()).X,
            points3D.Find(x => x.Z == valuesListTemp.Min()).Y);
    }

    public MethodScan(DataModel dataModel, out List<Point3D> points3D)
    {
        _dataModel = dataModel;
        Calculate(out points3D);
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