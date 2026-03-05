using CyberPulse10.Backend.Data;
using CyberPulse10.Backend.Helpers;
using CyberPulse10.Backend.Repositories.Interfaces.Gene;
using CyberPulse10.Shared.Entities.Gene;
using CyberPulse10.Shared.EntitiesDTO;
using CyberPulse10.Shared.EntitiesDTO.Gene;
using CyberPulse10.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse10.Backend.Repositories.Implementations.Gene;

public class StatuRepository : GenericRepository<Statu>, IStatuRepository
{
    private readonly ApplicationDbContext _context;

    public StatuRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public override async Task<ActionResponse<IEnumerable<Statu>>> GetAsync()
    {
        var result=await _context.Status.AsNoTracking().ToListAsync();

        return new ActionResponse<IEnumerable<Statu>>
        {
            WasSuccess = true,
            Result = (IEnumerable<Statu>)result
        };
    }
    public override async Task<ActionResponse<Statu>> GetAsync(int id)
    {
        var response =await _context.Status.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();

        if(response is null)
        {
            return new ActionResponse<Statu>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        return new ActionResponse<Statu>
        {
            WasSuccess = true,
            Result = response,
        };
    }
    public override async Task<ActionResponse<PagedResult<Statu>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Status.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => 
                        EF.Functions.Like(x.Name, $"%{pagination.Filter}%"));
        }

        var totalRecords = await queryable.CountAsync();

        var items = await queryable
            .OrderBy(x => x.Name)
            .Paginate(pagination)
            .ToListAsync();

        var result = new PagedResult<Statu>
        {
            Items = items,
            TotalRecords = totalRecords
        };

        return ActionResponse<PagedResult<Statu>>.Success(result);
    }
    public override async Task<ActionResponse<Statu>> DeleteAsync(int id)
    {
        var entity = await _context.Status.FindAsync(id);

        if (entity == null)
        {
            return new ActionResponse<Statu>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        _context.Remove(entity);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Statu>
            {
                WasSuccess = true,
            };
        }
        catch (Exception)
        {
            return new ActionResponse<Statu>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }

    }

    public async Task<ActionResponse<Statu>> GetAsync(string name, int lavel)
    {
        var response = await _context.Status.AsNoTracking().Where(x => x.Name.ToLower() == name.ToLower() && x.Level == lavel).FirstOrDefaultAsync();



        if (response == null)
        {
            return new ActionResponse<Statu>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        return new ActionResponse<Statu>
        {
            WasSuccess = true,
            Result = response,
        };
    }

    public async Task<ActionResponse<Statu>> AddAsync(StatuDTO entity)
    {
        var statu=new Statu{
            Name = entity.Name,
            Level = entity.Level
        };

        _context.Status.Add(statu);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Statu>
            {
                WasSuccess = true,
                Result = statu
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Statu>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Statu>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }
    public async Task<IEnumerable<Statu>> GetComboAsync(int id)
    {
        var query = _context.Status.AsNoTracking().AsQueryable();

        if (id >= 0)
        {
            query = query.Where(x => x.Level == id);
        }


        return await query.OrderBy(x => x.Name).ToListAsync();
    }
    public async Task<ActionResponse<Statu>> UpdateAsync(StatuDTO entity)
    {
        var statu = await _context.Status.Where(x => x.Id == entity.Id).FirstOrDefaultAsync();

        if (statu is null)
        {
            return new ActionResponse<Statu>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }


        statu.Name = entity.Name;

        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<Statu>
            {
                WasSuccess = true,
                Result = statu
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Statu>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Statu>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }

    }

}