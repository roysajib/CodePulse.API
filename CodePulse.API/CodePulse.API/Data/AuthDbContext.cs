using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "5b572312-6c1a-4d34-8a6a-74c76e1e48ae";
            var writterRoleId = "5f508c2e-b8b2-41e7-ae09-a4f955ab7516";

            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },
                new IdentityRole()
                {
                    Id = writterRoleId,
                    Name = "Writter",
                    NormalizedName = "Writter".ToUpper(),
                    ConcurrencyStamp = writterRoleId
                },
            };
            builder.Entity<IdentityRole>().HasData(roles);

            var adminUserId = "3f89ce59-2159-4716-b78f-428cf79c4e31";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@bs23.com",
                Email = "admin@bs23.com",
                NormalizedEmail = "admin@bs23.com".ToUpper(),
                NormalizedUserName = "admin@bs23.com".ToUpper()
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");

            builder.Entity<IdentityUser>().HasData(admin);

            var adminRoles = new List<IdentityUserRole<string>>
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleId

                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = writterRoleId

                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    }
}
