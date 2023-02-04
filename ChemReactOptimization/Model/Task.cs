namespace ChemReactOptimization.Model
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public double Alpha { get; set; }
        public double Beta { get; set; }
        public double Mu { get; set; }
        public double Delta { get; set; }

        public double G { get; set; } // Reaction mass consumption
        public double A { get; set; } // Reactor pressure
        public int N { get; set; } // Number of heat exchangers
        public double T1Min { get; set; } // Coil temperature MIN
        public double T1Max { get; set; } // Coil temperature MAX
        public double T2Min { get; set; } // Diffuser temperature MIN
        public double T2Max { get; set; } // Diffuser temperature MAX
        public double TSumMax { get; set; } // T1+T2 <= TSumMax
    }
}