using CyberPulse10.Shared.Entities.Gene;
using CyberPulse10.Shared.EntitiesDTO;
using CyberPulse10.Shared.EntitiesDTO.Gene;
using CyberPulse10.Shared.Responses;

namespace CyberPulse10.Backend.Repositories.Interfaces.Gene;

public interface ITaxeRepository
{
    Task<ActionResponse<Taxe>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<Taxe>>> GetAsync();

    Task<ActionResponse<Taxe>> AddAsync(TaxeDTO entity);

    Task<ActionResponse<Taxe>> UpdateAsync(TaxeDTO entity);

    Task<ActionResponse<Taxe>> DeleteAsync(int id);

    Task<IEnumerable<Taxe>> GetComboAsync();

    Task<ActionResponse<PagedResult<Taxe>>> GetAsync(PaginationDTO pagination);
}