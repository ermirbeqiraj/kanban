using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Issue.Web.IdentityModels
{
    public class UserDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public UserDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Ef core aspnet user configurations
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.Id).UseSqlServerIdentityColumn();

                entity.HasMany(e => e.Claims)
                        .WithOne()
                        .HasForeignKey(e => e.UserId)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Logins)
                        .WithOne()
                        .HasForeignKey(e => e.UserId)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Roles)
                        .WithOne()
                        .HasForeignKey(e => e.UserId)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion
        }
    }
}
