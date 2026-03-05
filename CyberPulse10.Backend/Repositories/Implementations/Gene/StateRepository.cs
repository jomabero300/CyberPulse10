using CyberPulse10.Backend.Data;
using CyberPulse10.Backend.Repositories.Interfaces.Gene;
using CyberPulse10.Shared.Entities.Gene;
using CyberPulse10.Shared.EntitiesDTO;
using CyberPulse10.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse10.Backend.Repositories.Implementations.Gene;

public class StateRepository : GenericRepository<State>, IStateRepository
{
    private readonly ApplicationDbContext _context;
    public StateRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<State>> GetComboAsync(int id)
    {
        return await _context.States
                                    .AsNoTracking()
                                    .Where(x => x.CountryId == id)
                                    .OrderBy(x => x.Name)
                                    .ToListAsync();
    }

    public Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        throw new NotImplementedException();
    }
}