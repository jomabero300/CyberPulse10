using CyberPulse10.Backend.Data;
using CyberPulse10.Backend.Helpers;
using CyberPulse10.Backend.UnitsOfWork.Implementations.Gene;
using CyberPulse10.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse10.Shared.Entities.Gene;
using CyberPulse10.Shared.EntitiesDTO;
using CyberPulse10.Shared.EntitiesDTO.Gene;
using CyberPulse10.Shared.Enums;
using CyberPulse10.Shared.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace CyberPulse10.Backend.Controllers.Gene;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IUserUnitOfWork _userUnitOfWork;
    private readonly IConfiguration _configuration;
    private readonly IMailHelper _mailHelper;
    private readonly IWebHostEnvironment _env;
    private readonly ICountryUnitOfWork _countryUnitOfWork;
    private readonly ApplicationDbContext _context;

    public AccountsController(ApplicationDbContext context, ICountryUnitOfWork countryUnitOfWork, IWebHostEnvironment env, IMailHelper mailHelper, IConfiguration configuration, IUserUnitOfWork usersUnitOfWork)
    {
        _context = context;
        _countryUnitOfWork = countryUnitOfWork;
        _env = env;
        _mailHelper = mailHelper;
        _configuration = configuration;
        _userUnitOfWork = usersUnitOfWork;
    }
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("paginated")]
    public async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _userUnitOfWork.GetAsync(pagination);

        return response.WasSuccess
            ? Ok(response.Result)
            : BadRequest(response.Message);
    }

    [HttpPost("RecoverPassword")]
    public async Task<IActionResult> RecoverPasswordAsync([FromBody] EmailDTO model)
    {
        var user = await _userUnitOfWork.GetUserAsync(model.Email);

        if (user == null)
        {
            return NotFound();
        }

        var response = await SendRecoverEmailAsync(user, model.Language);

        if (response.WasSuccess)
        {
            return NoContent();
        }

        return BadRequest(response.Message);
    }

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordDTO model)
    {
        var user = await _userUnitOfWork.GetUserAsync(model.Email);
        if (user == null)
        {
            return NotFound();
        }

        var result = await _userUnitOfWork.ResetPasswordAsync(user, model.Token, model.NewPassword);

        if (result.Succeeded)
        {
            return NoContent();
        }

        return BadRequest(result.Errors.FirstOrDefault()!.Description);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut]
    public async Task<IActionResult> PutAsync(User user)
    {
        try
        {
            var result = await _userUnitOfWork.UpdateUserAsync(user);

            if (result.Succeeded)
            {
                var updatedUser = await _userUnitOfWork.GetUserAsync(user.Email!);

                return Ok(BuildToken(updatedUser));
            }

            return BadRequest(result.Errors.FirstOrDefault());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut("UserType")]
    public async Task<IActionResult> PutAsync(string userId, UserType userType)
    {
        await _userUnitOfWork.UpdateUserAsync(userId, userType);
        return BadRequest();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("UserType")]
    public async Task<IActionResult> UserTypeAsync(string id)
    {
        var result = await _userUnitOfWork.GetUserAsync(new Guid(id));

        if (result != null)
        {
            return Ok(result);
        }

        return BadRequest();
    }


    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var result = await _userUnitOfWork.GetUserAsync(User.Identity!.Name!);

        return Ok(result);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var user = User.Identity!.Name!;

        if (user == null)
        {
            return NotFound();
        }

        var result = await _userUnitOfWork.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.FirstOrDefault()!.Description);
        }

        return NoContent();
    }

    [HttpPost("ResedToken")]
    public async Task<IActionResult> ResedTokenAsync([FromBody] EmailDTO model)
    {
        var user = await _userUnitOfWork.GetUserAsync(model.Email);
        if (user == null)
        {
            return NotFound();
        }

        var response = await SendConfirmationEmailAsync(user, model.Language);
        if (response.WasSuccess)
        {
            return NoContent();
        }

        return BadRequest(response.Message);
    }

    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] UserDTO model)
    {
        var country = await _countryUnitOfWork.GetAsync(model.CountryId, true);

        if (country == null)
        {
            return BadRequest("ERR004");
        }

        User user = model;

        user.Country = country.Result!;

        var result = await _userUnitOfWork.AddUserASync(model, model.Password);

        if (result.Succeeded)
        {
            await _userUnitOfWork.AddUserToRoleAsync(user, user.UserType.ToString());

            var response = await SendConfirmationEmailAsync(user, model.Language);

            if (response.WasSuccess)
            {
                return NoContent();
            }

            return BadRequest(response.Message);
        }

        return BadRequest(result.Errors.FirstOrDefault());
    }

    [HttpGet("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmailAsync(string userId, string token)
    {
        token = token.Replace(" ", "+");

        var user = await _userUnitOfWork.GetAsync(new Guid(userId));

        if (user == null)
        {
            return NotFound();
        }
        if (user.EmailConfirmed == false)
        {
            var result = await _userUnitOfWork.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault());
            }

            return NoContent();
        }

        return BadRequest("ERR009");
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDTO model)
    {
        var result = await _userUnitOfWork.LoginAsync(model);

        if (result.Succeeded)
        {
            var user = await _userUnitOfWork.GetUserAsync(model.Email);

            if (user != null)
            {
                await _userUnitOfWork.ResetAccessFailedCountAsync(user);
            }

            return Ok(BuildToken(user!));
        }

        if (result.IsLockedOut)
        {
            return BadRequest("ERR007");
        }

        if (result.IsNotAllowed)
        {
            return BadRequest("ERR008");
        }

        return BadRequest("ERR006");
    }

    [HttpGet("Combo")]
    public async Task<IActionResult> GetComboAsync(UserType userType)
    {
        var result = await _userUnitOfWork.GetComboAsync(userType);

        return Ok(result);
    }

    [HttpGet("UserTypeUp")]
    public async Task<IActionResult> UserTypeAsync(string id, UserType userType)
    {
        await _userUnitOfWork.UpdateUserAsync(id, userType);

        return NoContent();
    }
    private async Task<ActionResponse<string>> SendConfirmationEmailAsync(User user, string language)
    {
        var myToken = await _userUnitOfWork.GenerateEmailConfirmationTokenAsync(user);

        var tokenLink = Url.Action("ConfirmEmail", "accounts", new
        {
            userid = user.Id,
            token = HttpUtility.UrlEncode(myToken)
        }, HttpContext.Request.Scheme, _configuration["Url Frontend"]);

        var subject = "Mail:SubjectConfirmationEs";
        var body = "Mail:BodyConfirmationEs";

        if (language != "es")
        {
            subject = "Mail:SubjectConfirmationEn";
            body = "Mail:BodyConfirmationEn";
        }

        return await _mailHelper.SendMailAsync(user.FullName, user.Email!, _configuration[subject]!, string.Format(_configuration[body]!, tokenLink), language);
    }
    private async Task<ActionResponse<string>> SendRecoverEmailAsync(User user, string language)
    {
        var myToken = await _userUnitOfWork.GeneratePasswordResetTokenAsync(user);

        var tokenLink = Url.Action("ResetPassword", "accounts", new
        {
            userid = user.Id,
            token = myToken
        }, HttpContext.Request.Scheme, _configuration["Url Frontend"]);

        if (language == "es")
        {
            return await _mailHelper.SendMailAsync(user.FullName, user.Email!, _configuration["Mail:SubjectRecoveryEs"]!, string.Format(_configuration["Mail:BodyRecoveryEs"]!, tokenLink), language);
        }

        return await _mailHelper.SendMailAsync(user.FullName, user.Email!, _configuration["Mail:SubjectRecoveryEn"]!, string.Format(_configuration["Mail:BodyRecoveryEn"]!, tokenLink), language);
    }
    private TokenDTO BuildToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name,user.Email!),
            new Claim(ClaimTypes.Role,user.UserType.ToString()),
            new Claim("FrstName",user.FirstName),
            new Claim("LastName",user.LastName),
            new Claim("Photo",user.Photo ?? string.Empty),
            new Claim("CountryId",user.Country.Id.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwMBELtKey"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddHours(5);
        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);
        return new TokenDTO
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration,
        };

    }
}