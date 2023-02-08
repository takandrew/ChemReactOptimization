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

    public Task GetById(int id)
    {
        return _context.Tasks.First(m => m.Id == id);
    }

    public void SaveTask(Task task)
    {

        if (task.Id == 0)
            _context.Tasks.Add(task);
        else
        {
            var dbEntry = _context.Tasks.FirstOrDefault(u => u.Id == task.Id);
            if (dbEntry != null)
            {
                dbEntry.Name = task.Name;
                dbEntry.Alpha = task.Alpha;
                dbEntry.Beta = task.Beta;
                dbEntry.Mu = task.Mu;
                dbEntry.Delta = task.Delta;
                dbEntry.G = task.G;
                dbEntry.A = task.A;
                dbEntry.N = task.N;
                dbEntry.T1Min = task.T1Min;
                dbEntry.T1Max = task.T1Max;
                dbEntry.T2Min = task.T2Min;
                dbEntry.T2Max = task.T2Max;
                dbEntry.TSumMax = task.TSumMax;
            }
        }
        _context.SaveChanges();
    }

    public void DeleteTask(int id)
    {
        var value = _context.Tasks.Find(id);
        if (value != null)
            _context.Tasks.Remove(value);
        _context.SaveChanges();
    }
}