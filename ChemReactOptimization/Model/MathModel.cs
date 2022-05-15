using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemReactOptimization.Model
{
    public class MathModel
    {
        public static double TargetFunction(DataModel dataModel, double T1, double T2)
        {
            return 10 * (dataModel.Alpha * dataModel.G *
                             (Math.Pow((T2 - dataModel.Beta * dataModel.A), dataModel.N) +
                              dataModel.Mu * Math.Pow(Math.Exp(T1 + T2), dataModel.N) +
                              dataModel.Delta * (T2 - T1)));
        }
    }
}
