using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Context
{
    public class MyContext : DbContext 
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) 
        {
            
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Profiling> Profilings { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<University> Universities { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(a => a.Accounts)
                .WithOne(b => b.Employees)
                .HasForeignKey<Account>(b => b.NIK);

            modelBuilder.Entity<Account>()
                .HasOne(c => c.Profilings)
                .WithOne(d => d.Accounts)
                .HasForeignKey<Profiling>(d => d.NIK);

            modelBuilder.Entity<Profiling>()
                .HasOne(c => c.Educations)
                .WithMany(e => e.Profilings);

            modelBuilder.Entity<Education>()
                .HasOne(c => c.Universities)
                .WithMany(e => e.Educations);

            modelBuilder.Entity<AccountRole>()
                .HasKey(ar => new { ar.NIK, ar.RoleId });
            modelBuilder.Entity<AccountRole>()
                .HasOne(f => f.Role)
                .WithMany(g=>g.AccountRole)
                .HasForeignKey(f => f.RoleId);
            modelBuilder.Entity<AccountRole>()
                .HasOne(f => f.Account)
                .WithMany(g => g.AccountRole)
                .HasForeignKey(f => f.NIK);
        }

    }
    
}
