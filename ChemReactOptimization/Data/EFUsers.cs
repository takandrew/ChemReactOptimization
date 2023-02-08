using ChemReactOptimization.Model;
using System.Collections.Generic;
using System.Linq;

namespace ChemReactOptimization.Data;

public class EFUsers
{
    private readonly ChemReactContext _context;

    public EFUsers(ChemReactContext context)
    {
        _context = context;
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _context.Users.ToList();
    }

    public bool VerifyUser(string login, string password)
    {
        var value = _context.Users.FirstOrDefault(x => (x.Login == login && x.Password == password));
        if (value != null)
            return true;
        else
            return false;
    }

    public User GetById(int id)
    {
        return _context.Users.First(m => m.Id == id);
    }

    public void SaveUser(User user)
    {
        if (user.Id == 0)
            _context.Users.Add(user);
        else
        {
            var dbEntry = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (dbEntry != null)
            {
                dbEntry.Name = user.Name;
                dbEntry.Login = user.Login;
                dbEntry.Password = user.Password;
                dbEntry.Role = user.Role;
            }
        }
        _context.SaveChanges();
    }

    public void DeleteUser(int id)
    {
        var value = _context.Users.Find(id);
        if (value != null)
            _context.Users.Remove(value);
        _context.SaveChanges();
    }
}