using Microsoft.EntityFrameworkCore;
using ProjectSolamnia;
using SQLitePCL;

namespace ProjectSolamnia { }

public class HoldingService
{
    private readonly ProjectSolamniaDbContext _dbContext;

    public HoldingService(ProjectSolamniaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Holding> GetAllHoldings()
    {
        return _dbContext.Holdings.Include(h => h.AssignedCharacters).ToList();
    }

    public Holding? GetHoldingById(int id)
    {
        return _dbContext.Holdings.Include(h => h.AssignedCharacters).FirstOrDefault(h => h.Id == id);
    }

    public void AddHolding(Holding holding)
    {
        _dbContext.Holdings.Add(holding);
        _dbContext.SaveChanges();
    }

    public void UpdateHolding(Holding holding)
    {
        _dbContext.Holdings.Update(holding);
        _dbContext.SaveChanges();
    }

    public void DeleteHolding(int id)
    {
        var holding = _dbContext.Holdings.Find(id);
        if (holding != null)
        {
            _dbContext.Holdings.Remove(holding);
            _dbContext.SaveChanges();
        }
    }
}
