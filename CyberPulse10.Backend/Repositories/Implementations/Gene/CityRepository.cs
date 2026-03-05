using CyberPulse10.Backend.Data;
using CyberPulse10.Backend.Helpers;
using CyberPulse10.Backend.Repositories.Interfaces.Gene;
using CyberPulse10.Shared.Entities.Gene;
using CyberPulse10.Shared.EntitiesDTO;
using CyberPulse10.Shared.EntitiesDTO.Gene;
using CyberPulse10.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse10.Backend.Repositories.Implementations.Gene;

public class CityRepository : GenericRepository<City>, ICityRepository
{
    private readonly ApplicationDbContext _context;

    public CityRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<IEnumerable<City>>> GetAsync()
    {
        var entity = await _context.Cities.AsNoTracking()
                                        .Include(x => x.State)
                                        .OrderBy(x => x.StateId).ThenBy(x => x.Name)
                                        .ToListAsync();
        return entity is null
            ? ActionResponse<IEnumerable<City>>.Fail("ERR001")
            : ActionResponse<IEnumerable<City>>.Success(entity);

    }
    public override async Task<ActionResponse<City>> GetAsync(int id)
    {
        var entity = await _context.Cities.AsNoTracking()
                                        .Include(x => x.State)
                                        .FirstOrDefaultAsync(x => x.Id == id);
        return entity is null
            ? ActionResponse<City>.Fail("ERR001")
            : ActionResponse<City>.Success(entity);
    }
    public override async Task<ActionResponse<PagedResult<City>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Cities
            .AsNoTracking()
            .Include(x => x.State).AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(c =>
                EF.Functions.Like(c.Name, $"%{pagination.Filter}%"));
        }

        var totalRecords = await queryable.CountAsync();

        var items = await queryable
            .OrderBy(x => x.StateId).ThenBy(x => x.Name)
            .Paginate(pagination)
            .ToListAsync();

        var result = new PagedResult<City>
        {
            Items=items,
            TotalRecords= totalRecords
        };

        return ActionResponse<PagedResult<City>>.Success(result);
    }
    public override async Task<ActionResponse<City>> DeleteAsync(int id)
    {
        var entity = await _context.Cities.FindAsync(id);

        if (entity is null)
            return ActionResponse<City>.Fail("ERR001");

        _context.Remove(entity);

        try
        {
            await _context.SaveChangesAsync();

            return ActionResponse<City>.Success(entity);
        }
        catch (Exception)
        {
            return ActionResponse<City>.Fail("ERR002");
        }
    }


    public async Task<ActionResponse<City>> AddAsync(CityDTO entity)
    {
        var model = new City
        {
            Id = entity.Id,
            Name = entity.Name,
            StateId = entity.StateId
        };

        _context.Add(model);

        try
        {
            await _context.SaveChangesAsync();
            return ActionResponse<City>.Success(model);
        }
        catch (DbUpdateException)
        {
            return ActionResponse<City>.Fail("ERR003");
        }
        catch (Exception ex)
        {
            return ActionResponse<City>.Fail(ex.Message);
        }
    }

    public async Task<IEnumerable<City>> GetComboAsync(int id)
    {
        return await _context.Cities.AsNoTracking().Where(x => x.StateId == id).OrderBy(x => x.Name).ToListAsync();
    }

    public async Task<ActionResponse<City>> UpdateAsync(CityDTO entity)
    {
        var model = await _context.Cities.FindAsync(entity.Id);

        if (model is null)
        {
            return ActionResponse<City>.Fail("ERR005");
        }

        model.Name = entity.Name;
        model.StateId = entity.StateId;

        try
        {
            await _context.SaveChangesAsync();
            return ActionResponse<City>.Success(model);
        }
        catch (DbUpdateException)
        {
            return ActionResponse<City>.Fail("ERR003");
        }
        catch (Exception ex)
        {
            return ActionResponse<City>.Fail(ex.Message);
        }
    }
}