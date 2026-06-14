using Microsoft.EntityFrameworkCore;
using DOCSeal.Domain.Entities.Documents;
using DOCSeal.Domain.Entities.Organisations;
using DOCSeal.Domain.Entities.Users;

namespace DOCSeal.Infrastructure.DbContexts;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Organisation> Organisations { get; set; }
    public DbSet<Document> Documents { get; set; }
}