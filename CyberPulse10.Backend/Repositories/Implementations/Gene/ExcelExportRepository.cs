using ClosedXML.Excel;
using CyberPulse10.Backend.Data;
using CyberPulse10 .Backend.Repositories.Interfaces.Gene;

namespace CyberPulse10.Backend.Repositories.Implementations.Gene;

public class ExcelExportRepository : IExcelExportRepository
{
    private readonly ApplicationDbContext _context;

    public ExcelExportRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(int sheetsProcessed, int rowsProcessed)> ProcessExcelAsync(Stream fileStream)
    {
        int totalSheets = 0;
        int totalRows = 0;

        return (totalSheets, totalRows);
    }
}