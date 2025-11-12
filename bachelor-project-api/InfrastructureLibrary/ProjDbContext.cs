using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ApplicationCoreLibrary.Entities;

namespace InfrastructureLibrary
{
    public class ProjDbContext : IdentityDbContext<User, Role, int,
                                    IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
                                    IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ProjDbContext(DbContextOptions options) : base(options) { }

        public DbSet<UrlReport> UrlReports { get; set; }
        public DbSet<SessionToken> SessionTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UrlReport>()
                .ToTable(x => x.HasTrigger("UrlReportInsert"))
                .ToTable(x => x.HasTrigger("UrlReportUpdate"));

            modelBuilder.Entity<UrlReport>()
                .HasOne(x => x.User)
                .WithMany(x => x.UrlReports);

            modelBuilder.Entity<UserRole>(ur =>
            {
                ur.HasKey(ur => new { ur.UserId, ur.RoleId });

                ur.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId);
                ur.HasOne(ur => ur.User).WithMany(u => u.UserRoles).HasForeignKey(ur => ur.UserId);
            });
        }
    }
}
