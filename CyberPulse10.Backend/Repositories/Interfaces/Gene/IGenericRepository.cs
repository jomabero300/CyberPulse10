using CyberPulse10.Shared.EntitiesDTO;
using CyberPulse10.Shared.Responses;

namespace CyberPulse10.Backend.Repositories.Interfaces.Gene;

public interface IGenericRepository<T> where T : class
{
    Task<ActionResponse<T>> AddAsync(T entity);

    Task<ActionResponse<T>> DeleteAsync(int id);

    Task<ActionResponse<T>> GetAsync(int id);

    Task<ActionResponse<IEnumerable<T>>> GetAsync();

    Task<ActionResponse<PagedResult<T>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<T>> UpdateAsync(T entity);
}