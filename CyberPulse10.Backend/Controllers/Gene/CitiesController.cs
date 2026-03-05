using CyberPulse10.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse10.Shared.Entities.Gene;
using CyberPulse10.Shared.EntitiesDTO;
using CyberPulse10.Shared.EntitiesDTO.Gene;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyberPulse10.Backend.Controllers.Gene;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class CitiesController : GenericController<City>
{
    private readonly ICityUnitOfWork _cityUnitOfWork;
    public CitiesController(IGenericUnitOfWork<City> unitOfWork, ICityUnitOfWork cityUnitOfWork) : base(unitOfWork)
    {
        _cityUnitOfWork = cityUnitOfWork;
    }

    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _cityUnitOfWork.GetAsync(pagination);

        return response.WasSuccess
            ? Ok(response.Result)
            : BadRequest(response.Message);

    }

    [HttpDelete("full/{id}")]
    public override async Task<IActionResult> DeleteAsync(int id)
    {
        var response = await _cityUnitOfWork.DeleteAsync(id);

        return response.WasSuccess
            ? Ok(response.Result)
            : BadRequest(response.Message);

    }

    [HttpPost("full")]
    public async Task<IActionResult> PostAsync(CityDTO model)
    {
        var response = await _cityUnitOfWork.AddAsync(model);

        return response.WasSuccess
            ? Ok(response.Result)
            : BadRequest(response.Message);

    }

    [HttpPut("full")]
    public async Task<IActionResult> PustAsync(CityDTO model)
    {
        var response = await _cityUnitOfWork.UpdateAsync(model);

        return response.WasSuccess
            ? Ok(response.Result)
            : BadRequest(response.Message);

    }

    //[AllowAnonymous]
    [HttpGet("Combo/{id}")]
    public async Task<IActionResult> GetComboAsync(int id)
    {
        return Ok(await _cityUnitOfWork.GetComboAsync(id));
    }
}