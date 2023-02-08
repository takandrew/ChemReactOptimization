using System.ComponentModel;

namespace ChemReactOptimization.Model;

public class Method
{
    [DisplayName("Идентификатор")]
    public int Id { get; set; }
    [DisplayName("Название")]
    public string Name { get; set; }
}