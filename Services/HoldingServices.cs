using Microsoft.EntityFrameworkCore;
using ProjectSolamnia;
using SQLitePCL;

namespace ProjectSolamnia { }

public class HoldingService
{
    private readonly ProjectSolamniaDbContext _context;

    public HoldingService(ProjectSolamniaDbContext context)
    {
        _context = context;
    }
    public List<Holding> GetAllHoldings()
    {
        return _context.Holdings.Include(h => h.AssignedCharacters).ToList();
    }
    public Holding? GetHoldingById(int id)
    {
        return _context.Holdings.Include(h => h.AssignedCharacters).FirstOrDefault(h => h.Id == id);
    }
    public void AddHolding(Holding holding)
    {
        _context.Holdings.Add(holding);
        _context.SaveChanges();
    }
    public void UpdateHolding(Holding holding)
    {
        _context.Holdings.Update(holding);
        _context.SaveChanges();
    }
    public void DeleteHolding(int id)
    {
        var holding = _context.Holdings.Find(id);
        if (holding != null)
        {
            _context.Holdings.Remove(holding);
            _context.SaveChanges();
        }
    }
}