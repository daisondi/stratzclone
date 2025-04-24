// Server/Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using stratzclone.Server.Models;

namespace stratzclone.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
    }
}
