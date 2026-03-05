using CyberPulse10.Shared.Entities.Gene;
using CyberPulse10.Shared.EntitiesDTO;
using CyberPulse10.Shared.Responses;

namespace CyberPulse10.Backend.UnitsOfWork.Interfaces.Gene;

public interface IStateUnitOfWork
{
    Task<ActionResponse<State>> GetAsync(int id);
    Task<ActionResponse<IEnumerable<State>>> GetAsync();
    Task<ActionResponse<State>> DeleteAsync(int id);
    Task<IEnumerable<State>> GetComboAsync(int id);
    Task<ActionResponse<PagedResult<State>>> GetAsync(PaginationDTO pagination);
}