namespace CyberPulseVet10.FrontEnd.Repositories;

public interface ISqlInjValRepository
{
    bool HasSqlInjection(string input);
    string SanitizeInput(string input);
}