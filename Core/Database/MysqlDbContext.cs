using TiendaOnline1.Models;
using Microsoft.EntityFrameworkCore;

namespace TiendaOnline1.Core.Database{
    public class MysqlDbContext : DbContext
    {
        public MysqlDbContext(DbContextOptions<MysqlDbContext> options) : base(options){}

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<SuperUser> SuperUser { get; set; } = null!;
    }
}

