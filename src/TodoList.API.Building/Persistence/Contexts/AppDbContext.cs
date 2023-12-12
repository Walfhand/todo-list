using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TodoList.Common.EFCore;

namespace TodoList.API.Building.Persistence.Contexts;

public class AppDbContext : AppDbContextBase
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
