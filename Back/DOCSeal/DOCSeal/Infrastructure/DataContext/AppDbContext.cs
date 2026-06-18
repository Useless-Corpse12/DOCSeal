using Microsoft.EntityFrameworkCore;
using DOCSeal.Domain.Entities.Users;
using DOCSeal.Domain.Entities.Organisations;
using DOCSeal.Domain.Entities.Documents;

namespace DOCSeal.Infrastructure.DataContext;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Organisation> Organisations { get; set; }
    public DbSet<Document> Documents { get; set; }
}