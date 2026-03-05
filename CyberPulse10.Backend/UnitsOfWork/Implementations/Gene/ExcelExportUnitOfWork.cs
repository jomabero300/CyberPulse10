using CyberPulse10.Backend.Repositories.Interfaces.Gene;
using CyberPulse10.Backend.UnitsOfWork.Interfaces.Gene;

namespace CyberPulse10.Backend.UnitsOfWork.Implementations.Gene;

public class ExcelExportUnitOfWork : IExcelExportUnitOfWork
{
    private readonly IExcelExportRepository _excelExportRepository;

    public ExcelExportUnitOfWork(IExcelExportRepository excelExportRepository)
    {
        _excelExportRepository = excelExportRepository;
    }

    public async Task<(int sheetsProcessed, int rowsProcessed)> ProcessExcelAsync(Stream fileStream) => await _excelExportRepository.ProcessExcelAsync(fileStream);
}
