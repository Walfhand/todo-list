using Microsoft.EntityFrameworkCore;

namespace TodoList.Common.EFCore;
public class AppDbContextBase : DbContext, IDbContext
{
    protected AppDbContextBase(DbContextOptions options) : base(options)
    {
    }
}
