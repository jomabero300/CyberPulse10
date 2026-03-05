using CyberPulse10.Frontend.Pages.Auth;
using CyberPulse10.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse10.Frontend.Shared;

public partial class AuthLinks
{
    private string? photoUser;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

    protected override async Task OnParametersSetAsync()
    {
        var authenticationState = await AuthenticationStateTask;
        var claims = authenticationState.User.Claims.ToList();
        var photoClaim = claims.FirstOrDefault(x => x.Type == "Photo");
        var nameClaim = claims.FirstOrDefault(x => x.Type == "Username");
        if (photoClaim is not null)
        {
            photoUser = photoClaim.Value;
        }
    }

    private void EditAction()
    {
        NavigationManager.NavigateTo("/EditUser");
    }

    private void ShowModalLogIn()
    {
        var closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };
        DialogService.ShowAsync<Login>(Localizer["Login"], closeOnEscapeKey);
    }

    private void ShowModalLogOut()
    {
        var closeOnEscapeKey = new DialogOptions { CloseOnEscapeKey = true };
        DialogService.ShowAsync<Logout>(Localizer["Logout"], closeOnEscapeKey);
    }

    private void ShowModalRegister()
    {
        var closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };
        DialogService.ShowAsync<Register>("Registar Usuario", closeOnEscapeKey);
    }
}