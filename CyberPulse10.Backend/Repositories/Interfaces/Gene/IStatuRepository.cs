using CyberPulse10.Shared.Entities.Gene;
using CyberPulse10.Shared.EntitiesDTO;
using CyberPulse10.Shared.EntitiesDTO.Gene;
using CyberPulse10.Shared.Responses;

namespace CyberPulse10.Backend.Repositories.Interfaces.Gene;

public interface IStatuRepository
{
    Task<ActionResponse<Statu>> GetAsync(int id);
    Task<ActionResponse<Statu>> GetAsync(string name, int nivel);
    Task<ActionResponse<IEnumerable<Statu>>> GetAsync();
    Task<ActionResponse<Statu>> AddAsync(StatuDTO entity);
    Task<ActionResponse<Statu>> UpdateAsync(StatuDTO entity);
    Task<ActionResponse<Statu>> DeleteAsync(int id);
    Task<IEnumerable<Statu>> GetComboAsync(int id);
    Task<ActionResponse<PagedResult<Statu>>> GetAsync(PaginationDTO pagination);
}