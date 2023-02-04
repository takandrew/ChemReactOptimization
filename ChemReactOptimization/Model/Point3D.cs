using System.ComponentModel;

namespace ChemReactOptimization.Model
{
    public class Point3D
    {
        [DisplayName("Температура в змеевике")]
        public double X { get; set; }
        [DisplayName("Температура в диффузоре")]
        public double Y { get; set; }
        [DisplayName("Себестоимость продукта")]
        public double Z { get; set; }

        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
