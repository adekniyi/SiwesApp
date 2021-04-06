using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SiwesApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Data
{
    public class ApplicationDataContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : base(options) 
        {
          
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // establishing the relationship between the user, role and userRole classes
            // users can have many roles, many roles can have many users
            builder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired(false);

                userRole.HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired(false);
            });



            //builder.Entity<Lecturer>()
            //    .HasOne(a => a.User)
            //    .WithOne(b => b.lecturer)
            //    .HasForeignKey<Lecturer>(c => c.UserId)
            //    .IsRequired(false);

            builder.Entity<SiwesAdmin>()
             .HasOne(a => a.User)
             .WithOne(b => b.siwesAdmin)
             .HasForeignKey<SiwesAdmin>(c => c.UserId)
             .IsRequired(false);

         //   builder.Entity<SiwesCoordinator>()
         //  .HasOne(a => a.User)
         //  .WithOne(b => b.siwesCoordinator)
         //  .HasForeignKey<SiwesCoordinator>(c => c.UserId)
         //  .IsRequired(false);

         //   builder.Entity<IndustrialSupervisor>()
         //.HasOne(a => a.User)
         //.WithOne(b => b.industrialSupervisor)
         //.HasForeignKey<IndustrialSupervisor>(c => c.UserId)
         //.IsRequired(false);


         //   builder.Entity<Student>()
         //       .HasOne(a => a.User)
         //       .WithOne(b => b.Student)
         //       .HasForeignKey<Student>(c => c.UserId)
         //       .IsRequired(false);

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<SiwesAdmin> SiwesAdmins { get; set; }
        public DbSet<SiwesCoordinator> SiwesCoordinators { get; set; }
        public DbSet<IndustrialSupervisor> IndustrialSupervisors { get; set; }
    }
}
