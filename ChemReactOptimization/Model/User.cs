using System.ComponentModel;

namespace ChemReactOptimization.Model;

public class User
{
    [DisplayName("Идентификатор")]
    public int Id { get; set; }
    [DisplayName("Имя")]
    public string Name { get; set; }
    [DisplayName("Роль")]
    public string Role { get; set; }
    [DisplayName("Логин")]
    public string Login { get; set; }
    [DisplayName("Пароль")]
    public string Password { get; set; }
}