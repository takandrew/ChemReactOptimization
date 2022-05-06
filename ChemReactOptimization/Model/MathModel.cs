using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemReactOptimization.Model
{
    public class MathModel
    {
        public double TargetFunction(DataModel dataModel)
        {
            return 10 * (dataModel.Alpha * dataModel.G *
                             (Math.Pow((dataModel.T2 - dataModel.Beta * dataModel.A), dataModel.N) +
                              dataModel.Mu * Math.Pow(Math.Exp(dataModel.T1 + dataModel.T2), dataModel.N) +
                              dataModel.Delta * (dataModel.T2 - dataModel.T1)));
        }
    }
}
