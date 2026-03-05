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
public class CountriesController : GenericController<Country>
{
    private readonly ICountryUnitOfWork _countryUnitOf;
    public CountriesController(IGenericUnitOfWork<Country> unitOfWork, ICountryUnitOfWork countryUnitOf) : base(unitOfWork)
    {
        _countryUnitOf = countryUnitOf;
    }

    [HttpGet("full")]
    public override async Task<IActionResult> GetAsync()
    {
        var response = await _countryUnitOf.GetAsync();

        return response.WasSuccess
            ? Ok(response.Result)
            : BadRequest(response.Message);
    }
    public override async Task<IActionResult> GetAsync(int id)
    {
        var response = await _countryUnitOf.GetAsync(id);

        return response.WasSuccess
            ? Ok(response.Result)
            : BadRequest(response.Message);
    }

    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _countryUnitOf.GetAsync(pagination);

        return response.WasSuccess
            ? Ok(response.Result)
            : BadRequest(response.Message);
    }

    [HttpDelete("full/{id}")]
    public override async Task<IActionResult> DeleteAsync(int id)
    {
        var response = await _countryUnitOf.DeleteAsync(id);

        return response.WasSuccess
            ? Ok(response.Result)
            : BadRequest(response.Message);
    }


    [HttpPost("full")]
    public async Task<IActionResult> PostAsync([FromBody] CountryDTO entity)
    {
        var response = await _countryUnitOf.AddAsync(entity);

        return response.WasSuccess
            ? Ok(response.Result)
            : BadRequest(response.Message);

    }
    [HttpPut("full")]
    public async Task<IActionResult> PustAsync([FromBody] CountryDTO entity)
    {
        var response = await _countryUnitOf.UpdateAsync(entity);

        return response.WasSuccess
            ? Ok(response.Result)
            : BadRequest(response.Message);
    }
    [AllowAnonymous]
    [HttpGet("Combo")]
    public async Task<IActionResult> GetComboAsync()
    {
        var country = await _countryUnitOf.GetComboAsync();

        return Ok(country);
    }
}
