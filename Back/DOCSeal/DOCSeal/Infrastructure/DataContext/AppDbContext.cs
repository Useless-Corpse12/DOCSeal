using Microsoft.EntityFrameworkCore;
using DOCSeal.Domain.Entities;

namespace DOCSeal.Infrastructure.DataContext;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Organisation> Organisations { get; set; }
    public DbSet<Document> Documents { get; set; }
}