using ChemReactOptimization.Model;
using System.Collections.Generic;
using System.Linq;

namespace ChemReactOptimization.Data;

public class EFMethods
{
    private readonly ChemReactContext _context;

    public EFMethods(ChemReactContext context)
    {
        _context = context;
    }

    public IEnumerable<Method> GetAllMethods()
    {
        return _context.Methods.ToList();
    }

    public Method GetById(int id)
    {
        return _context.Methods.First(m => m.Id == id);
    }

    public void SaveMethod(Method method)
    {
        if (method.Id == 0)
            _context.Methods.Add(method);
        else
        {
            var dbEntry = _context.Methods.FirstOrDefault(u => u.Id == method.Id);
            if (dbEntry != null)
            {
                dbEntry.Name = method.Name;
            }
        }
        _context.SaveChanges();
    }

    public void DeleteMethod(int id)
    {
        var value = _context.Methods.Find(id);
        if (value != null)
            _context.Methods.Remove(value);
        _context.SaveChanges();
    }
}