using CyberPulse10.Shared.Entities.Gene;

namespace CyberPulse10.Backend.Repositories.Interfaces.Gene;

public interface INeighborhoodRepository
{
    Task<IEnumerable<Neighborhood>> GetComboAsync(int id);
}