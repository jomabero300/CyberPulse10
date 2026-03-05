using Azure;
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
public class StatusController : GenericController<Statu>
{
    private readonly IStatuUnitOfWork _statu;
    public StatusController(IGenericUnitOfWork<Statu> unitOfWork, IStatuUnitOfWork statu) : base(unitOfWork)
    {
        _statu = statu;
    }

    [HttpGet("full")]
    public override async Task<IActionResult> GetAsync()
    {
        var response =await _statu.GetAsync();

        return response.WasSuccess
            ? Ok(response.Result)
            : BadRequest(response.Message);
    }
    public override async Task<IActionResult> GetAsync(int id)
    {
        var response = await _statu.GetAsync(id);

        return response.WasSuccess
            ? Ok(response.Result)
            : BadRequest(response.Message);
    }

    [HttpGet("paginated")]
    public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _statu.GetAsync(pagination);

        return response.WasSuccess
            ? Ok(response.Result)
            : BadRequest(response.Message);
    }

    [HttpDelete("full/{id}")]
    public override async Task<IActionResult> DeleteAsync(int id)
    {
        var response = await _statu.DeleteAsync(id);

        return response.WasSuccess
            ? Ok(response.Result)
            : BadRequest(response.Message);
    }


    [HttpPost("full")]
    public async Task<IActionResult> PostAsync([FromBody] StatuDTO entity)
    {
        var response = await _statu.AddAsync(entity);

        return response.WasSuccess
            ? Ok(response.Result)
            : BadRequest(response.Message);

    }
    [HttpPut("full")]
    public async Task<IActionResult> PustAsync([FromBody] StatuDTO entity)
    {
        var response = await _statu.UpdateAsync(entity);

        return response.WasSuccess
            ? Ok(response.Result)
            : BadRequest(response.Message);
    }

    [HttpGet("Combo/{id}")]
    public async Task<IActionResult> GetComboAsync(int id)
    {
        return Ok(await _statu.GetComboAsync(id));
    }
}