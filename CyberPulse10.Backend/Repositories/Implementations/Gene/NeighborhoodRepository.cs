using CyberPulse10.Backend.Data;
using CyberPulse10.Backend.Repositories.Interfaces.Gene;
using CyberPulse10.Shared.Entities.Gene;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse10.Backend.Repositories.Implementations.Gene;

public class NeighborhoodRepository : GenericRepository<Neighborhood>, INeighborhoodRepository
{
    private readonly ApplicationDbContext _context;
    public NeighborhoodRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Neighborhood>> GetComboAsync(int id)
    {
        return await _context.Neighborhoods.AsNoTracking().Where(x => x.CityId == id).OrderBy(x => x.Name).ToListAsync();
    }
}