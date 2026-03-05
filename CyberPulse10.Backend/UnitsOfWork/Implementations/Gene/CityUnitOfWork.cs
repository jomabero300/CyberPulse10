using CyberPulse10.Backend.Repositories.Interfaces.Gene;
using CyberPulse10.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse10.Shared.Entities.Gene;
using CyberPulse10.Shared.EntitiesDTO;
using CyberPulse10.Shared.EntitiesDTO.Gene;
using CyberPulse10.Shared.Responses;

namespace CyberPulse10.Backend.UnitsOfWork.Implementations.Gene;

public class CityUnitOfWork : GenericUnitOfWork<City>, ICityUnitOfWork
{
    private readonly ICityRepository _cityRepository;
    public CityUnitOfWork(IGenericRepository<City> repository, ICityRepository cityRepository) : base(repository)
    {
        _cityRepository = cityRepository;
    }

    public override async Task<ActionResponse<IEnumerable<City>>> GetAsync()=>await _cityRepository.GetAsync();
    public override async Task<ActionResponse<City>> GetAsync(int id)=>await _cityRepository.GetAsync(id);
    public override async Task<ActionResponse<PagedResult<City>>> GetAsync(PaginationDTO pagination)=>await _cityRepository.GetAsync(pagination);
    public override async Task<ActionResponse<City>> DeleteAsync(int id)=>await _cityRepository.DeleteAsync(id);

    public async Task<ActionResponse<City>> AddAsync(CityDTO entity)=>await _cityRepository.AddAsync(entity);
    public async Task<IEnumerable<City>> GetComboAsync(int id) => await _cityRepository.GetComboAsync(id);
    public async Task<ActionResponse<City>> UpdateAsync(CityDTO entity)=>await _cityRepository.UpdateAsync(entity);
}
