using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;

namespace NattfrostBackend.Data
{
    public class AppDbContext : DbContext
    {
        //constructor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        //DbSet for Subscribers  
        public DbSet<Entities.Subscriber> Subscribers { get; set; }  
    }
}
