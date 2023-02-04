using ChemReactOptimization.Model;
using System.Collections.Generic;
using System.Linq;

namespace ChemReactOptimization.Data;

public class EFTasks
{
    private readonly ChemReactContext _context;

    public EFTasks(ChemReactContext context)
    {
        _context = context;
    }

    public IEnumerable<Task> GetAllTasks()
    {
        return _context.Tasks.ToList();
    }

    public Task GetById(long id)
    {
        return _context.Tasks.First(m => m.Id == id);
    }

    public void SaveTask(Task task)
    {
        _context.Tasks.Add(task);
        _context.SaveChanges();
    }

    public void DeleteTask(long id)
    {
        var value = _context.Tasks.Find(id);
        if (value != null)
            _context.Tasks.Remove(value);
        _context.SaveChanges();
    }
}