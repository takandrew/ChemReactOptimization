using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChemReactOptimization.Model
{
    public class OptimizationMethod
    {

        private static DataModel _optDataModel = new DataModel();
        private static List<Point3D> resultList = new List<Point3D>();

        public static DataModel OptDataModel
        {
            get => _optDataModel;
            set
            {
                _optDataModel = value;
            }
        }

        public static int num_of_functioncalls = 0;

        private static bool ConditionChecking(double t1, double t2)
        {
            if ((OptDataModel.T1Min <= t1) &&
                (t1 <= OptDataModel.T1Max) &&
                (OptDataModel.T2Min <= t2) &&
                (t2 <= OptDataModel.T2Max) &&
                (t1 + t2 <= OptDataModel.TSumMax))
                return true;
            else
                return false;
        }

        public static double CalculateFuncValue(double[] x)
        {
            num_of_functioncalls++;
            double targetFuncValue = 10 * (OptDataModel.Alpha * OptDataModel.G *
                                    (Math.Pow((x[1] - OptDataModel.Beta * OptDataModel.A), OptDataModel.N) +
                                     OptDataModel.Mu * Math.Pow(Math.Exp(x[0] + x[1]), OptDataModel.N) +
                                     OptDataModel.Delta * (x[1] - x[0])));
            if (ConditionChecking(x[0],x[1])) 
                resultList.Add(new Point3D(Math.Round(x[0],4), Math.Round(x[1], 4), Math.Round(targetFuncValue, 4)));
            return targetFuncValue;
        }


        public static double[] optimizer(Func<double[], double> function, int N)
        {
            Random rnd = new Random();
            double[][] simplex = new double[N + 1][];

            // Generate N + 1 initial arrays.
            for (int array = 0; array <= N; array++)
            {
                simplex[array] = new double[N];
                for (int index = 0; index < N; index++)
                {
                    simplex[array][index] = rnd.NextDouble() * 20 - 10;
                }
            }
            const double alpha = 1;
            const double gamma = 2;
            const double rho = 0.5;
            const double sigma = 0.5;

            // Infinite loop until convergence
            while (true)
            {
                // Evaluation
                double[] functionValues = new double[N + 1];
                int[] indices = new int[N + 1];
                for (int vertex_of_simplex = 0; vertex_of_simplex <= N; vertex_of_simplex++)
                {
                    functionValues[vertex_of_simplex] = function(simplex[vertex_of_simplex]);
                    indices[vertex_of_simplex] = vertex_of_simplex;
                }

                // Order
                Array.Sort(functionValues, indices);


                // Find centroid of the simplex excluding the vertex with highest functionvalue.
                double[] centroid = new double[N];

                for (int index = 0; index < N; index++)
                {
                    centroid[index] = 0;
                    for (int vertex_of_simplex = 0; vertex_of_simplex <= N; vertex_of_simplex++)
                    {
                        if (vertex_of_simplex != indices[N])
                        {
                            centroid[index] += simplex[vertex_of_simplex][index] / N;
                        }
                    }
                }

                // Check for convergence
                if (Math.Abs(function(centroid) - functionValues[0]) < 1e-5)
                {
                    break;
                }



                //Reflection
                double[] reflection_point = new double[N];
                for (int index = 0; index < N; index++)
                {
                    reflection_point[index] = centroid[index] + alpha * (centroid[index] - simplex[indices[N]][index]);
                }

                double reflection_value = function(reflection_point);

                if (reflection_value >= functionValues[0] & reflection_value < functionValues[N - 1])
                {
                    simplex[indices[N]] = reflection_point;
                    continue;
                }


                // Expansion
                if (reflection_value < functionValues[0])
                {
                    double[] expansion_point = new double[N];
                    for (int index = 0; index < N; index++)
                    {
                        expansion_point[index] = centroid[index] + gamma * (reflection_point[index] - centroid[index]);
                    }
                    double expansion_value = function(expansion_point);

                    if (expansion_value < reflection_value)
                    {
                        simplex[indices[N]] = expansion_point;
                    }
                    else
                    {
                        simplex[indices[N]] = reflection_point;
                    }
                    continue;
                }

                // Contraction
                double[] contraction_point = new double[N];
                for (int index = 0; index < N; index++)
                {
                    contraction_point[index] = centroid[index] + rho * (simplex[indices[N]][index] - centroid[index]);
                }

                double contraction_value = function(contraction_point);

                if (contraction_value < functionValues[N])
                {
                    simplex[indices[N]] = contraction_point;
                    continue;
                }

                //Shrink
                double[] best_point = simplex[indices[0]];
                for (int vertex_of_simplex = 0; vertex_of_simplex <= N; vertex_of_simplex++)
                {
                    for (int index = 0; index < N; index++)
                    {

                        simplex[vertex_of_simplex][index] = best_point[index] + sigma * (simplex[vertex_of_simplex][index] - best_point[index]);

                    }
                }

            }

            return simplex[0];
        }

        public static void Start(DataModel dataModel, out List<Point3D> points3D)
        {
            points3D = new List<Point3D>();
            resultList.Clear();
            OptDataModel = dataModel;
            double[] result = new double[OptDataModel.N];
            result = optimizer(CalculateFuncValue, dataModel.N);
            foreach (var item in resultList)
            {
                points3D.Add(item);
            }
            string toBeShown = $"Т1: {Math.Round(result[0], 4)}  Т2: {Math.Round(result[1], 4)}\nКоличество вызовов функции: {num_of_functioncalls}";
            var minValue = points3D.FirstOrDefault(x => x.X == Math.Round(result[0], 4) && x.Y == Math.Round(result[1], 4));
            toBeShown += $"\nМинимум: {minValue.Z}";
            MessageBox.Show(toBeShown);

            num_of_functioncalls = 0;

        }
    }
}
