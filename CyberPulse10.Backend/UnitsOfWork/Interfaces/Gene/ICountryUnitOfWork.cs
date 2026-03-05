using CyberPulse10.Shared.Entities.Gene;
using CyberPulse10.Shared.EntitiesDTO;
using CyberPulse10.Shared.EntitiesDTO.Gene;
using CyberPulse10.Shared.Responses;

namespace CyberPulse10.Backend.UnitsOfWork.Interfaces.Gene;

public interface ICountryUnitOfWork
{
    Task<ActionResponse<IEnumerable<Country>>> GetAsync();
    Task<ActionResponse<Country>> GetAsync(int id);
    Task<ActionResponse<Country>> AddAsync(CountryDTO country);
    Task<ActionResponse<Country>> UpdateAsync(CountryDTO country);
    Task<ActionResponse<Country>> GetAsync(int id, bool lb);
    Task<ActionResponse<Country>> DeleteAsync(int id);
    Task<IEnumerable<Country>> GetComboAsync();
    Task<ActionResponse<PagedResult<Country>>> GetAsync(PaginationDTO pagination);
}