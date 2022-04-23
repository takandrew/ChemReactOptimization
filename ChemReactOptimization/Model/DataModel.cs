namespace ChemReactOptimization.Model
{
    public class DataModel
    {
        #region NormalizingFactors

        public double Alpha { get; set; }
        public double Beta { get; set; }
        public double Mu { get; set; }
        public double Delta { get; set; }

        #endregion
        public double G { get; set; } // Reaction mass consumption
        public double A { get; set; } // Reactor pressure
        public double N { get; set; } // Number of heat exchangers
        public double T1 { get; set; } // Coil temperature
        public double T2 { get; set; } // Diffuser temperature

    }
}
