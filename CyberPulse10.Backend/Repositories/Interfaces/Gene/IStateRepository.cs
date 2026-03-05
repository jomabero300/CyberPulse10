using CyberPulse10.Shared.Entities.Gene;
using CyberPulse10.Shared.EntitiesDTO;
using CyberPulse10.Shared.Responses;

namespace CyberPulse10.Backend.Repositories.Interfaces.Gene;

public interface IStateRepository
{
    Task<ActionResponse<State>> GetAsync(int id);
    Task<ActionResponse<IEnumerable<State>>> GetAsync();
    Task<ActionResponse<State>> DeleteAsync(int id);
    Task<IEnumerable<State>> GetComboAsync(int id);
    Task<ActionResponse<PagedResult<State>>> GetAsync(PaginationDTO pagination);
}