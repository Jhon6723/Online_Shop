using Microsoft.EntityFrameworkCore;

namespace TiendaOnline1.Core.Database{
    public class MysqlDbContext : DbContext
    {
        public MysqlDbContext(DbContextOptions<MysqlDbContext> options) : base(options){}

        public DbSet<Models.Product> Products { get; set; } = null!;
        public DbSet<Models.User> Users { get; set; } = null!;
    }
}

