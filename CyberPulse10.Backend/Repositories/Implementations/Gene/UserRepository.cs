using CyberPulse10.Backend.Data;
using CyberPulse10.Backend.Helpers;
using CyberPulse10.Backend.Repositories.Interfaces.Gene;
using CyberPulse10.Shared.Entities.Gene;
using CyberPulse10.Shared.EntitiesDTO;
using CyberPulse10.Shared.EntitiesDTO.Gene;
using CyberPulse10.Shared.Enums;
using CyberPulse10.Shared.Responses;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse10.Backend.Repositories.Implementations.Gene;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _env;
    public UserRepository(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, IConfiguration configuration, IWebHostEnvironment env) : base(context)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _env = env;
    }

    public async Task<IdentityResult> AddUserAsync(User user, string password)
    {
        if (!string.IsNullOrWhiteSpace(user.Photo))
        {
            user.Photo = await HtmlUtilities.UploadImageAsync(user.Photo, $"{_env.WebRootPath}{_configuration["FolderUsers"]}");
        }

        return await _userManager.CreateAsync(user, password);
    }
    public async Task AddUserToRoleAsync(User user, string roleName)
    {
        await _userManager.AddToRoleAsync(user, roleName);
    }
    public async Task<IdentityResult> ChangePasswordAsync(string email, string currentPassword, string newPassword)
    {
        var entity = await _userManager.FindByEmailAsync(email);

        return await _userManager.ChangePasswordAsync(entity!, currentPassword, newPassword);
    }
    public async Task CheckRoleAsync(string roleName)
    {
        var rolExiste = await _roleManager.RoleExistsAsync(roleName);

        if (!rolExiste)
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
        }
    }
    public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
    {
        var entity = await _userManager.FindByEmailAsync(user.Email!);

        return await _userManager.ConfirmEmailAsync(entity!, token);
    }
    public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
    {
        return await _userManager.GenerateEmailConfirmationTokenAsync(user);
    }
    public async Task<string> GeneratePasswordResetTokenAsync(User user)
    {
        return await _userManager.GeneratePasswordResetTokenAsync(user);
    }
    public async Task<IEnumerable<User>> GetAsync(UserType userType)
    {
        var resul = await _context.Users.AsNoTracking().Where(x => x.UserType == userType).OrderBy(x => x.FirstName).ToListAsync();

        return resul;
    }
    public async Task<IEnumerable<User>> GetAsync(string id)
    {
        return await _context.Users
                                .AsNoTracking()
                                .Where(x => x.Id == id)
                                .OrderBy(x => x.FirstName)
                                .ToListAsync();
    }

    public async Task<User> GetAsync(Guid userId)
    {
        var user = await _context.Users
                                .Include(x => x.Country)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.Id == userId.ToString());
        return user!;
    }

    public async Task<IEnumerable<User>> GetComboAsync(UserType userType)
    {
        var result = userType == UserType.User
                        ? await _context.Users
                            .AsNoTracking()
                            .Include(x => x.Country)
                            .OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
                            .ToListAsync()
                        : await _context.Users
                            .AsNoTracking()
                            .Include(x => x.Country)
                            .Where(x => x.UserType == userType)
                            .OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
                            .ToListAsync();
        return result;
    }

    public async Task<User> GetUserAsync(Guid userId)
    {
        var user = await _context.Users
                                .Include(x => x.Country)
                                .FirstOrDefaultAsync(x => x.Id == userId.ToString());
        if (user != null && !string.IsNullOrWhiteSpace(user!.Photo))
        {
            user.Photo = $"{_configuration["UrlBackend"]}{_configuration["FolderUsers"]}{user.Photo}";
        }

        return user!;
    }
    public async Task<ActionResponse<User>> GetUserAsync(string userDocument, UserType userType)
    {
        var entity = userDocument.Length < 15 ?
            await _context.Users.AsNoTracking().Where(x => x.DocumentId == userDocument && x.UserType == userType).FirstOrDefaultAsync() :
            await _context.Users.AsNoTracking().Where(x => x.Id == userDocument && x.UserType == userType).FirstOrDefaultAsync();

        if (entity == null)
        {
            return new ActionResponse<User>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<User>
        {
            WasSuccess = true,
            Result = entity
        };
    }
    public async Task<User> GetUserAsync(string email)
    {
        var user = await _context.Users
                        .AsNoTracking()
                        .Include(c => c.Country)
                        .FirstOrDefaultAsync(x => x.Email == email);

        if (user != null && !string.IsNullOrWhiteSpace(user!.Photo))
        {
            user.Photo = $"{_configuration["UrlBackend"]}{_configuration["FolderUsers"]}{user.Photo}";
        }

        return user!;
    }   
    public async Task<bool> IsUserInRoleAsync(User user, string roleName)
    {
        return await _userManager.IsInRoleAsync(user, roleName);
    }
    public async Task<SignInResult> LoginAsync(LoginDTO model)
    {
        return await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, true);
    }
    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
    public async Task ResetAccessFailedCountAsync(User user)
    {
        await _userManager.ResetAccessFailedCountAsync(user);
    }

    public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);

        return await _userManager.ResetPasswordAsync(entity!, token, password);
    }

    public async Task<IdentityResult> UpdateUserAsync(User user)
    {
        var userOld = await _context.Users
                                .Include(x => x.Country)
                                .FirstOrDefaultAsync(x => x.Email == user.Email);

        if (userOld == null)
            return IdentityResult.Failed(new IdentityError { Description = "ERR020" });

        // 2️⃣ Actualizar SOLO las propiedades necesarias
        userOld.FirstName = user.FirstName;
        userOld.LastName = user.LastName;
        userOld.PhoneNumber = user.PhoneNumber;


        if (!string.IsNullOrWhiteSpace(user.Photo) && !user.Photo.Contains(_configuration["FolderUsers"]!))
        {
            var imageUrl = !string.IsNullOrWhiteSpace(userOld.Photo) ? $"{_env.WebRootPath}{_configuration["FolderUsers"]!}{userOld.Photo}" : "";

            userOld.Photo = await HtmlUtilities.UploadImageAsync(user.Photo, $"{_env.WebRootPath}{_configuration["FolderUsers"]!}", imageUrl);
        }

        return await _userManager.UpdateAsync(userOld);
    }

    public async Task UpdateUserAsync(string userId, UserType userType)
    {
        var entity = await _context.Users.AsNoTracking().Where(x => x.Id == userId).FirstOrDefaultAsync();

        if (entity == null) return;

        _context.Database.ExecuteSql($"UPDATE Admi.AspNetUsers SET UserType={userType} WHERE Id={userId}");
    }
}
