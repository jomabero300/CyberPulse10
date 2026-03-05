using CyberPulse10.Backend.Data;
using CyberPulse10.Backend.Helpers;
using CyberPulse10.Backend.Repositories.Interfaces.Gene;
using CyberPulse10.Shared.Entities.Gene;
using CyberPulse10.Shared.EntitiesDTO;
using CyberPulse10.Shared.EntitiesDTO.Gene;
using CyberPulse10.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse10.Backend.Repositories.Implementations.Gene;

public class TaxeRepository : GenericRepository<Taxe>, ITaxeRepository
{
    private readonly ApplicationDbContext _context;
    public TaxeRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<Taxe>> GetAsync(int id)
    {
        var entity = await _context.Taxes
            .AsNoTracking()
             .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return new ActionResponse<Taxe>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<Taxe>
        {
            WasSuccess = true,
            Result = entity
        };
    }
    public override async Task<ActionResponse<PagedResult<Taxe>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = _context.Taxes
            .AsNoTracking()
            .Include(x => x.Statu)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {

            queryable = queryable.Where(c =>
                            EF.Functions.Like(c.Name, $"%{pagination.Filter}%") ||
                            EF.Functions.Like(c.Statu!.Name, $"%{pagination.Filter}%") ||
                            EF.Functions.Like(c.Worth.ToString(), $"%{pagination.Filter}%"));

        }

        var totalRecords = await queryable.CountAsync();

        var items = await queryable
            .OrderBy(x => x.Name)
            .Paginate(pagination)
            .ToListAsync();

        var result = new PagedResult<Taxe>
        {
            Items = items,
            TotalRecords = totalRecords
        };

        return ActionResponse<PagedResult<Taxe>>.Success(result);
    }
    public override async Task<ActionResponse<Taxe>> DeleteAsync(int id)
    {
        var entity = await _context.Taxes.FindAsync(id);

        if (entity == null)
        {
            return new ActionResponse<Taxe>
            {
                WasSuccess = false,
                Message = "ERR001",
            };
        }

        _context.Remove(entity);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Taxe>
            {
                WasSuccess = true,
            };
        }
        catch (Exception)
        {
            return new ActionResponse<Taxe>
            {
                WasSuccess = false,
                Message = "ERR002"
            };
        }
    }


    public async Task<ActionResponse<Taxe>> AddAsync(TaxeDTO entity)
    {
        var model = new Taxe
        {
            Id = entity.Id,
            Name = HtmlUtilities.ToTitleCase(entity.Name.Trim().ToLower()),
            Worth = entity.Worth,
            StatuId = entity.StatuId,
        };

        _context.Add(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Taxe>
            {
                WasSuccess = true,
                Result = model
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Taxe>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Taxe>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }
    public async Task<IEnumerable<Taxe>> GetComboAsync()
    {
        return await _context.Taxes
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();
    }
    public async Task<ActionResponse<Taxe>> UpdateAsync(TaxeDTO entity)
    {
        var model = await _context.Taxes.FindAsync(entity.Id);

        if (model == null)
        {
            return new ActionResponse<Taxe>
            {
                WasSuccess = false,
                Message = "ERR005",
            };
        }

        model.Name = HtmlUtilities.ToTitleCase(entity.Name.Trim().ToLower());
        model.Worth = entity.Worth;
        model.StatuId = entity.StatuId;

        _context.Update(model);

        try
        {
            await _context.SaveChangesAsync();

            return new ActionResponse<Taxe>
            {
                WasSuccess = true,
                Result = model,
            };
        }
        catch (DbUpdateException)
        {
            return new ActionResponse<Taxe>
            {
                WasSuccess = false,
                Message = "ERR003"
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<Taxe>
            {
                WasSuccess = false,
                Message = ex.Message
            };
        }
    }
}