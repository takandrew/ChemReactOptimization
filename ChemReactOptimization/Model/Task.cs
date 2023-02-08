using System.ComponentModel;

namespace ChemReactOptimization.Model
{
    public class Task
    {
        [DisplayName("Идентификатор")]
        public int Id { get; set; }
        [DisplayName("Название")]
        public string Name { get; set; }
        [DisplayName("α")]
        public double Alpha { get; set; }
        [DisplayName("β")]
        public double Beta { get; set; }
        [DisplayName("μ")]
        public double Mu { get; set; }
        [DisplayName("Δ")]
        public double Delta { get; set; }
        [DisplayName("Расход реакционной массы, кг/ч")]
        public double G { get; set; } // Reaction mass consumption
        [DisplayName("Давление в реакторе, Кпа")]
        public double A { get; set; } // Reactor pressure
        [DisplayName("Количество теплообменных устройств, шт")]
        public int N { get; set; } // Number of heat exchangers
        [DisplayName("Мин. температура Т1, ℃")] 
        public double T1Min { get; set; } // Coil temperature MIN
        [DisplayName("Макс. температура Т1, ℃")] 
        public double T1Max { get; set; } // Coil temperature MAX
        [DisplayName("Мин. температура Т2, ℃")]
        public double T2Min { get; set; } // Diffuser temperature MIN
        [DisplayName("Макс. температура Т2, ℃")]
        public double T2Max { get; set; } // Diffuser temperature MAX
        [DisplayName("Сумма температур, ℃")]
        public double TSumMax { get; set; } // T1+T2 <= TSumMax
    }
}