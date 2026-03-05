using CyberPulse10.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse10.Shared.Entities.Gene;
using CyberPulse10.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse10.Backend.Data;

public class SeedDb(ApplicationDbContext context, IUserUnitOfWork userUnitOf)
{
    private readonly ApplicationDbContext _context = context;
    private readonly IUserUnitOfWork _usersUnitOf = userUnitOf;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.MigrateAsync(cancellationToken);
        await CheckStatusAsync(cancellationToken);
        await CheckCountriesAsync();
        await CheckStatesAsync();
        await CheckCitiesAsync();
        await CheckRolesAsync();
        await CheckUserAsync("Marcos", "Suarez", "1000001", "marcos301234@gmail.com", "3133670740", UserType.Admi);
    }


    private async Task CheckStatusAsync(CancellationToken cancellationToken)
    {
        if (await _context.Status.AnyAsync(cancellationToken))
            return;

        List<Statu> status =
            [
                new Statu { Name = "Activo", Level = 0 },
                new Statu { Name = "Inactivo", Level = 0 },
                new Statu { Name = "Suspendido", Level = 0 },
                new Statu { Name = "Creada", Level = 1 },
                new Statu { Name = "Registrada", Level = 1 },
                new Statu { Name = "Abierto", Level = 2 },
                new Statu { Name = "Cerrado", Level = 2 }
            ];

        _context.Status.AddRange(status);

        await _context.SaveChangesAsync(cancellationToken);

    }
    private async Task CheckRolesAsync()
    {
        await _usersUnitOf.CheckRoleAsync(UserType.Admi.ToString());
        await _usersUnitOf.CheckRoleAsync(UserType.User.ToString());
    }
    private async Task CheckCountriesAsync()
    {
        if (!_context.Countries.Any())
        {
            var statesSqlScript = File.ReadAllText("Data\\Scripts\\Countries.sql");

            await _context.Database.ExecuteSqlRawAsync(statesSqlScript);

        }
    }
    private async Task CheckStatesAsync()
    {
        if (!_context.States.Any())
        {
            var statesSqlScript = File.ReadAllText("Data\\Scripts\\States.sql");

            await _context.Database.ExecuteSqlRawAsync(statesSqlScript);

        }
    }
    private async Task CheckCitiesAsync()
    {
        if (!_context.Cities.Any())
        {
            var statesSqlScript = File.ReadAllText("Data\\Scripts\\Cities.sql");

            await _context.Database.ExecuteSqlRawAsync(statesSqlScript);

        }
    }

    private async Task<User> CheckUserAsync(string firstName, string lastName, string DocumenteId, string email, string phone, UserType userType)
    {
        var user = await _usersUnitOf.GetUserAsync(email);

        if (user == null)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(x => x.Name == "Colombia");

            user = new User
            {
                DocumentId = DocumenteId,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email,
                PhoneNumber = phone,
                Country = country!,
                UserType = userType,
            };

            await _usersUnitOf.AddUserASync(user, "Mbel123.");

            await _usersUnitOf.AddUserToRoleAsync(user, userType.ToString());

            var token = await _usersUnitOf.GenerateEmailConfirmationTokenAsync(user);
            await _usersUnitOf.ConfirmEmailAsync(user, token);
        }

        return user;
    }
}