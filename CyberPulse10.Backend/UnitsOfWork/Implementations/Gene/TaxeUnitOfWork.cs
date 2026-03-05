using CyberPulse10.Backend.Repositories.Interfaces.Gene;
using CyberPulse10.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse10.Shared.Entities.Gene;
using CyberPulse10.Shared.EntitiesDTO;
using CyberPulse10.Shared.EntitiesDTO.Gene;
using CyberPulse10.Shared.Responses;

namespace CyberPulse10.Backend.UnitsOfWork.Implementations.Gene;

public class TaxeUnitOfWork : GenericUnitOfWork<Taxe>, ITaxeUnitOfWork
{
    private readonly ITaxeRepository _taxeRepository;
    public TaxeUnitOfWork(IGenericRepository<Taxe> repository, ITaxeRepository taxeRepository) : base(repository)
    {
        _taxeRepository = taxeRepository;
    }


    public override async Task<ActionResponse<IEnumerable<Taxe>>> GetAsync()=>await _taxeRepository.GetAsync();
    public override async Task<ActionResponse<Taxe>> GetAsync(int id)=>await _taxeRepository.GetAsync(id);
    public override async Task<ActionResponse<PagedResult<Taxe>>> GetAsync(PaginationDTO pagination)=>await _taxeRepository.GetAsync(pagination);
    public override async Task<ActionResponse<Taxe>> DeleteAsync(int id)=>await _taxeRepository.DeleteAsync(id);

    public async Task<ActionResponse<Taxe>> AddAsync(TaxeDTO entity)=>await _taxeRepository.AddAsync(entity);
    public async Task<IEnumerable<Taxe>> GetComboAsync()=>await _taxeRepository.GetComboAsync();
    public async Task<ActionResponse<Taxe>> UpdateAsync(TaxeDTO entity)=>await _taxeRepository.UpdateAsync(entity);
}
