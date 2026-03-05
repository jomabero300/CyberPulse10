namespace CyberPulse10.Backend.Repositories.Interfaces.Gene;

public interface ISqlInjeValRepository
{
    bool HasSqlInjection(string input);
    string SanitizeInput(string input);
}
