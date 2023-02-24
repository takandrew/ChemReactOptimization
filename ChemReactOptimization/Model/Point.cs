namespace ChemReactOptimization.Model;

public class Point
{
    public double X { get; set; }
    public double Y { get; set; }

    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }

    public Point()
    {
        X = 0;
        Y = 0;
    }
}