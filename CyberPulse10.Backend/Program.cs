using CyberPulse10.Backend.Data;
using CyberPulse10.Backend.Helpers;
using CyberPulse10.Backend.Repositories.Implementations.Gene;
using CyberPulse10.Backend.Repositories.Interfaces.Gene;
using CyberPulse10.Backend.UnitsOfWork.Implementations.Gene;
using CyberPulse10.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse10.Shared.Entities.Gene;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

const string CorsPolicy = "AllowSpecificOrigen";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
    .AddDataAnnotationsLocalization()
    .AddViewLocalization();

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy,
        builder =>
        {
            builder.WithOrigins("https://localhost:7296", "http://localhost:5210")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Seeder

builder.Services.AddScoped<SeedDb>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericUnitOfWork<>), typeof(GenericUnitOfWork<>));

builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICityUnitOfWork, CityUnitOfWork>();

builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICountryUnitOfWork, CountryUnitOfWork>();

builder.Services.AddScoped<INeighborhoodRepository, NeighborhoodRepository>();
builder.Services.AddScoped<INeighborhoodUnitOfWork, NeighborhoodUnitOfWork>();

builder.Services.AddScoped<IStateRepository, StateRepository>();
builder.Services.AddScoped<IStateUnitOfWork, StateUnitOfWork>();

builder.Services.AddScoped<IStatuRepository, StatuRepository>();
builder.Services.AddScoped<IStatuUnitOfWork, StatuUnitOfWork>();

builder.Services.AddScoped<ITaxeRepository, TaxeRepository>();
builder.Services.AddScoped<ITaxeUnitOfWork, TaxeUnitOfWork>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserUnitOfWork, UserUnitOfWork>();

builder.Services.AddScoped<IMailHelper, MailHelper>();

builder.Services.AddIdentity<User, IdentityRole>(x =>
{
    x.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
    x.SignIn.RequireConfirmedEmail = true;
    x.User.RequireUniqueEmail = true;
    x.Password.RequireDigit = false;
    x.Password.RequiredUniqueChars = 0;
    x.Password.RequireLowercase = true;
    x.Password.RequireNonAlphanumeric = true;
    x.Password.RequireUppercase = true;
    x.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    x.Lockout.MaxFailedAccessAttempts = 3;
    x.Lockout.AllowedForNewUsers = true;
    x.Password.RequiredLength = 8;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddUserValidator<CustomEmailValidator>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x => x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwMBELtKey"]!)),
        ClockSkew = TimeSpan.Zero
    });


builder.Services.AddLocalization(option => option.ResourcesPath = "Resources");


var app = builder.Build();


app.UseRouting();
app.UseCors(CorsPolicy);
             
await SeedDataAsync(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseFileServer();
app.UseAuthorization();

app.MapControllers();

app.Run();

static async Task SeedDataAsync(WebApplication app)
{
    await using var scope = app.Services.CreateAsyncScope();

    var seedDb = scope.ServiceProvider.GetRequiredService<SeedDb>();

    await seedDb.SeedAsync();
}
