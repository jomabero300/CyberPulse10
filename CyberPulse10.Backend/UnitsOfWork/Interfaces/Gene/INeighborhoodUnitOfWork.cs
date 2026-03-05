using CyberPulse10.Shared.Entities.Gene;

namespace CyberPulse10.Backend.UnitsOfWork.Interfaces.Gene;

public interface INeighborhoodUnitOfWork
{
    Task<IEnumerable<Neighborhood>> GetComboAsync(int id);
}
