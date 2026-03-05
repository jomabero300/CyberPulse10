using CyberPulse10.Backend.Data;
using CyberPulse10.Backend.Helpers;
using CyberPulse10.Backend.Repositories.Interfaces.Gene;
using CyberPulse10.Shared.Entities.Gene;
using CyberPulse10.Shared.EntitiesDTO;
using CyberPulse10.Shared.EntitiesDTO.Gene;
using CyberPulse10.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace CyberPulse10.Backend.Repositories.Implementations.Gene;

public class CountryRepository : GenericRepository<Country>, ICountryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public CountryRepository(ApplicationDbContext context, IWebHostEnvironment env) : base(context)
    {
        _context = context;
        _env = env;
    }



    public async Task<ActionResponse<Country>> GetAsync(int id, bool lb)
    {
        var entity = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);

        return entity is null
            ? ActionResponse<Country>.Fail("ERR001")
            : ActionResponse<Country>.Success(entity);
    }
    public async Task<ActionResponse<Country>> AddAsync(CountryDTO entity)
    {
        var country = new Country
        {
            Id = entity.Id,
            Name = entity.Name,
            Image = entity.Image,
        };

        if (!string.IsNullOrWhiteSpace(entity.Image))
        {
            country.Image = await UploadImageHelper.UploadImageAsync(entity.Image, 0, _env.WebRootPath);
        }

        _context.Add(country);

        try
        {
            await _context.SaveChangesAsync();

            return ActionResponse<Country>.Success(country);
        }
        catch (DbUpdateException)
        {
            return ActionResponse<Country>.Fail("ERR003");
        }
        catch (Exception ex)
        {
            return ActionResponse<Country>.Fail(ex.Message);
        }
    }
    public async Task<IEnumerable<Country>> GetComboAsync()
    {
        return await _context.Countries.AsNoTracking().OrderBy(x => x.Name).ToListAsync();
    }

    public async Task<ActionResponse<Country>> UpdateAsync(CountryDTO entity)
    {
        var country = await _context.Countries.FindAsync(entity.Id);

        if (country is null)
            return ActionResponse<Country>.Fail("ERR005");

        country.Name = entity.Name;

        if (!string.IsNullOrWhiteSpace(entity.Image))
        {
            country.Image = await UploadImageHelper.UploadImageAsync(entity.Image, entity.Id, _env.WebRootPath, country.Image!);
        }

        _context.Update(country);

        try
        {
            await _context.SaveChangesAsync();

            return ActionResponse<Country>.Success(country);
        }
        catch (DbUpdateException)
        {
            return ActionResponse<Country>.Fail("ERR003");
        }
        catch (Exception ex)
        {
            return ActionResponse<Country>.Fail(ex.Message);
        }
    }
}
