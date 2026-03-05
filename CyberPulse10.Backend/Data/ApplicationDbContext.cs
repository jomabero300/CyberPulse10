using CyberPulse10.Shared.Entities.Gene;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse10.Backend.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<City> Cities { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Neighborhood> Neighborhoods { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<Statu> Status { get; set; }
    public DbSet<Taxe> Taxes { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("Adm");

        builder.Entity<City>().HasIndex(c =>new {c.StateId, c.Name }).IsUnique();
        builder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
        builder.Entity<Neighborhood>().HasIndex(n => n.Name).IsUnique();
        builder.Entity<State>().HasIndex(n => new {n.CountryId, n.Name }).IsUnique();
        builder.Entity<Statu>().HasIndex(n => n.Name).IsUnique();
        builder.Entity<Taxe>().HasIndex(n => n.Name).IsUnique();

        DisableCascadingDelete(builder);
    }

    private void DisableCascadingDelete(ModelBuilder builder)
    {
        var relatioships = builder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys());

        foreach (var relationship in relatioships)
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}
