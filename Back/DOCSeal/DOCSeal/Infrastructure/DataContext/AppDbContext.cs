using Microsoft.EntityFrameworkCore;
using DOCSeal.Domain.Entities;

namespace DOCSeal.Infrastructure.DataContext;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        //builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        builder.Entity<UserPosition>(e => e.HasIndex(p => p.UserId));
        builder.Entity<RefreshToken>(e => e.HasIndex(t => t.Token));
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Organisation> Organisations { get; set; }
    public DbSet<OrganisationInviteCode> OrganisationInviteCodes { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<UserPosition>  UserPositions { get; set; }
}