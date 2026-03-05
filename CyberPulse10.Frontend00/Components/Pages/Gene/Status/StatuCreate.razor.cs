using CyberPulse10.Frontend.Respositories;
using CyberPulse10.Shared.EntitiesDTO.Gene;
using CyberPulse10.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse10.Frontend.Components.Pages.Gene.Status;

public partial class StatuCreate
{
    private StatuForm? statuForm;
    private StatuDTO statuDTO = new();

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    private async Task CreateAsync()
    {

        var responseHttp = await Repository.PostAsync("/api/status/full", statuDTO);

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return;
        }

        Return();

        Snackbar.Add(Localizer["RecordCreateOk"], Severity.Success);

    }
    private void Return()
    {
        statuForm!.FormPostedSuccessfully = true;
        //NavigationManager.NavigateTo("/status");
        MudDialog.Close(DialogResult.Ok(true));
    }
}