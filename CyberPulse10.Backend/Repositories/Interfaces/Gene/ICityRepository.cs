using CyberPulse10.Shared.Entities.Gene;
using CyberPulse10.Shared.EntitiesDTO;
using CyberPulse10.Shared.EntitiesDTO.Gene;
using CyberPulse10.Shared.Responses;

namespace CyberPulse10.Backend.Repositories.Interfaces.Gene;

public interface ICityRepository
{
    Task<ActionResponse<City>> GetAsync(int id);
    Task<ActionResponse<IEnumerable<City>>> GetAsync();
    Task<ActionResponse<City>> AddAsync(CityDTO entity);
    Task<ActionResponse<City>> UpdateAsync(CityDTO entity);
    Task<ActionResponse<City>> DeleteAsync(int id);
    Task<IEnumerable<City>> GetComboAsync(int id);
    Task<ActionResponse<PagedResult<City>>> GetAsync(PaginationDTO pagination);
}