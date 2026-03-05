using CurrieTechnologies.Razor.SweetAlert2;
using CyberPulse10.Frontend.Components;
using CyberPulse10.Frontend.Respositories;
using CyberPulseVet10.FrontEnd.Repositories;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(op =>
    {
        op.DetailedErrors = true;
    });

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7084") });
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<ISqlInjValRepository, SqlInjValRepository>();

builder.Services.AddLocalization();
builder.Services.AddSweetAlert2();
builder.Services.AddMudServices();
builder.Services.AddAuthorizationCore();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
