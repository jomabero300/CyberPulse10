using CyberPulse10.Frontend.Repositories;
using CyberPulse10.Frontend.Services;
using CyberPulse10.Shared.EntitiesDTO.Gene;
using CyberPulse10.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse10.Frontend.Pages.Auth;

public partial class Login
{
    private LoginDTO loginDTO = new();
    private bool wasClose;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ILoginService LoginService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;

    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    private async Task ShowModalRecoverPassword()
    {
        var closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraLarge };
        await DialogService.ShowAsync<RecoverPassword>(Localizer["PasswordRecovery"], closeOnEscapeKey);
    }
    private async Task Register()
    {
        var closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };
        await DialogService.ShowAsync<Register>(Localizer["Register"], closeOnEscapeKey);

    }
    private void CloseModal()
    {
        wasClose = true;
        MudDialog.Cancel();
    }

    private async Task ShowModalResendConfirmationEmail()
    {
        var closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true, CloseButton = true, MaxWidth = MaxWidth.ExtraLarge };
        await DialogService.ShowAsync<ResendConfirmationEmailToken>(Localizer["MailForwarding"], closeOnEscapeKey);
    }

    private async Task LoginAsync()
    {
        if (wasClose)
        {
            NavigationManager.NavigateTo("/");
            return;
        }

        var responseHttp = await Repository.PostAsync<LoginDTO, TokenDTO>("/api/accounts/Login", loginDTO);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return;
        }

        await LoginService.LoginAsync(responseHttp.Response!.Token);
        NavigationManager.NavigateTo("/");
    }

}