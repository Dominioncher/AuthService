using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AuthService.DB
{
    public class PostgreSQLContext : DbContext
    {
        public DbSet<DBUser> Users { get; set; }

        public PostgreSQLContext(DbContextOptions<PostgreSQLContext> options): base(options)
        {
        }
    }
}
