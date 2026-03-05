using CyberPulse10.Backend.Repositories.Interfaces.Gene;
using CyberPulse10.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse10.Shared.EntitiesDTO;
using CyberPulse10.Shared.Responses;

namespace CyberPulse10.Backend.UnitsOfWork.Implementations.Gene;

public class GenericUnitOfWork<T> : IGenericUnitOfWork<T> where T : class
{
    private readonly IGenericRepository<T> _repository;

    public GenericUnitOfWork(IGenericRepository<T> repository)
    {
        _repository = repository;
    }

    public virtual async Task<ActionResponse<T>> AddAsync(T entity) => await _repository.AddAsync(entity);
    public virtual async Task<ActionResponse<T>> DeleteAsync(int id) => await _repository.DeleteAsync(id);
    public virtual async Task<ActionResponse<T>> GetAsync(int id) => await _repository.GetAsync(id);
    public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync() => await _repository.GetAsync();
    public virtual async Task<ActionResponse<PagedResult<T>>> GetAsync(PaginationDTO pagination) => await _repository.GetAsync(pagination);
    public virtual async Task<ActionResponse<T>> UpdateAsync(T entity) => await _repository.UpdateAsync(entity);
}