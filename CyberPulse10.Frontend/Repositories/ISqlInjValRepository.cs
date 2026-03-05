namespace CyberPulse10.Frontend.Repositories;

public interface ISqlInjValRepository
{
    bool HasSqlInjection(string input);
    string SanitizeInput(string input);
}