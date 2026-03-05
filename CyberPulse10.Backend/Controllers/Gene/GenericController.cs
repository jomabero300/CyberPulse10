using Azure;
using CyberPulse10.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse10.Shared.EntitiesDTO;
using Microsoft.AspNetCore.Mvc;

namespace CyberPulse10.Backend.Controllers.Gene;

public class GenericController<T> : Controller where T : class
{
    private readonly IGenericUnitOfWork<T> _unitOfWork;

    public GenericController(IGenericUnitOfWork<T> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    [HttpGet]
    public virtual async Task<IActionResult> GetAsync()
    {
        var response = await _unitOfWork.GetAsync();

        return response.WasSuccess 
            ? Ok(response.Result) 
            : BadRequest(response.Message);   
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetAsync(int id)
    {
        var response = await _unitOfWork.GetAsync(id);

        return response.WasSuccess
            ? Ok(response.Result)
            : BadRequest(response.Message);
    }

    [HttpPost]
    public virtual async Task<IActionResult> PostAsync(T model)
    {
        var response = await _unitOfWork.AddAsync(model);

        return response.WasSuccess
            ? Ok(response.Result)
            : BadRequest(response.Message);
    }

    [HttpPut]
    public virtual async Task<IActionResult> PustAsync(T model)
    {
        var response = await _unitOfWork.UpdateAsync(model);

        return response.WasSuccess
            ? Ok(response.Result)
            : BadRequest(response.Message);
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(int id)
    {
        var response = await _unitOfWork.DeleteAsync(id);

        return response.WasSuccess
            ? Ok(response.Result)
            : BadRequest(response.Message);
    }

    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _unitOfWork.GetAsync(pagination);

        return response.WasSuccess
            ? Ok(response.Result)
            : BadRequest(response.Message);
    }
}