using DMS.Domain.Models;
using DMS.Infrastructure.ModelsConfiguration;
using DMS.Infrastructure.Seed;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.DataContext
{
    public class DMSContext:IdentityDbContext<AppUser>
    {
        public DMSContext()
        { }
        public DMSContext(DbContextOptions<DMSContext> options):base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AppUserConfig).Assembly);
           
            builder.Seed();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=DMS;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");
        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<SharedItem> SharedItems { get; set; }

    }
}
