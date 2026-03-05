using CyberPulse10.Backend.Repositories.Interfaces.Gene;
using CyberPulse10.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse10.Shared.Entities.Gene;
using CyberPulse10.Shared.EntitiesDTO;
using CyberPulse10.Shared.EntitiesDTO.Gene;
using CyberPulse10.Shared.Enums;
using CyberPulse10.Shared.Responses;
using Microsoft.AspNetCore.Identity;

namespace CyberPulse10.Backend.UnitsOfWork.Implementations.Gene;

public class UserUnitOfWork : IUserUnitOfWork
{
    private readonly IUserRepository _userRepository;

    public UserUnitOfWork(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IdentityResult> AddUserASync(User user, string password)=>await _userRepository.AddUserAsync(user, password);

    public async Task AddUserToRoleAsync(User user, string roleName)=>await _userRepository.AddUserToRoleAsync(user,roleName);

    public async Task<IdentityResult> ChangePasswordAsync(string email, string currentPassword, string newPassword)=>await _userRepository.ChangePasswordAsync(email,currentPassword,newPassword);

    public async Task CheckRoleAsync(string roleName)=>await _userRepository.CheckRoleAsync(roleName);

    public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)=>await _userRepository.ConfirmEmailAsync(user,token);

    public async Task<string> GenerateEmailConfirmationTokenAsync(User user)=>await _userRepository.GenerateEmailConfirmationTokenAsync(user);

    public async Task<string> GeneratePasswordResetTokenAsync(User user) => await _userRepository.GeneratePasswordResetTokenAsync(user);

    public async Task<IEnumerable<User>> GetAsync(UserType userType)=>await _userRepository.GetAsync(userType);

    public async Task<IEnumerable<User>> GetAsync(string id)=>await _userRepository.GetAsync(id);

    public async Task<ActionResponse<PagedResult<User>>> GetAsync(PaginationDTO pagination)=>await _userRepository.GetAsync(pagination);

    public async Task<User> GetAsync(Guid userId)=>await _userRepository.GetAsync(userId);

    public async Task<IEnumerable<User>> GetComboAsync(UserType userType)=>await _userRepository.GetComboAsync(userType);

    public async Task<User> GetUserAsync(Guid userId) => await _userRepository.GetUserAsync(userId);

    public async Task<User> GetUserAsync(string email)=>await _userRepository.GetUserAsync(email);

    public async Task<ActionResponse<User>> GetUserAsync(string userDocument, UserType userType)=>await _userRepository.GetUserAsync(userDocument, userType);

    public async Task<bool> IsUserInRoleAsync(User user, string roleName)=>await _userRepository.IsUserInRoleAsync(user, roleName);

    public async Task<SignInResult> LoginAsync(LoginDTO model) => await _userRepository.LoginAsync(model);

    public async Task LogoutAsync()=>await _userRepository.LogoutAsync();

    public async Task ResetAccessFailedCountAsync(User user)=>await _userRepository.ResetAccessFailedCountAsync(user);

    public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)=>await _userRepository.ResetPasswordAsync(user,token,password);

    public async Task<IdentityResult> UpdateUserAsync(User user)=>await _userRepository.UpdateUserAsync(user);

    public async Task UpdateUserAsync(string userId, UserType userType) => await _userRepository.UpdateUserAsync(userId,userType);
}
