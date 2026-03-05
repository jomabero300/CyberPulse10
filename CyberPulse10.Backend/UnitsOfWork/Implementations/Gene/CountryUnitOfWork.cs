using CyberPulse10.Backend.Repositories.Interfaces.Gene;
using CyberPulse10.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse10.Shared.Entities.Gene;
using CyberPulse10.Shared.EntitiesDTO;
using CyberPulse10.Shared.EntitiesDTO.Gene;
using CyberPulse10.Shared.Responses;

namespace CyberPulse10.Backend.UnitsOfWork.Implementations.Gene;

public class CountryUnitOfWork : GenericUnitOfWork<Country>, ICountryUnitOfWork
{
    private readonly ICountryRepository _countryRepository;

    public CountryUnitOfWork(IGenericRepository<Country> repository, ICountryRepository countryRepository) : base(repository)
    {
        _countryRepository = countryRepository;
    }

    public override async Task<ActionResponse<IEnumerable<Country>>> GetAsync()=>await _countryRepository.GetAsync();
    public override async Task<ActionResponse<Country>> GetAsync(int id)=>await _countryRepository.GetAsync(id);
    public override async Task<ActionResponse<PagedResult<Country>>> GetAsync(PaginationDTO pagination)=>await _countryRepository.GetAsync(pagination);
    public override async Task<ActionResponse<Country>> DeleteAsync(int id)=>await _countryRepository.DeleteAsync(id);


    public async Task<ActionResponse<Country>> AddAsync(CountryDTO country) => await _countryRepository.AddAsync(country);
    public async Task<ActionResponse<Country>> GetAsync(int id, bool lb) => await _countryRepository.GetAsync(id, lb);
    public async Task<IEnumerable<Country>> GetComboAsync() => await _countryRepository.GetComboAsync();
    public async Task<ActionResponse<Country>> UpdateAsync(CountryDTO country) => await _countryRepository.UpdateAsync(country);
}
