using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthWebApi.Data
{
    public class MyIdentityContext : IdentityDbContext<MyUser>
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            

            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SecureDb;Integrated Security=True");

            base.OnConfiguring(optionsBuilder);

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            

            IdentityRole Adminrole = new IdentityRole();
            Adminrole.Id = Guid.NewGuid().ToString();
            Adminrole.NormalizedName = "Admin";
            Adminrole.Name = "Admin";
            Adminrole.ConcurrencyStamp = Guid.NewGuid().ToString();


            IdentityRole DefaultRole = new IdentityRole();
            DefaultRole.Id = Guid.NewGuid().ToString();
            DefaultRole.NormalizedName = "Default";
            DefaultRole.Name = "Default";
            DefaultRole.ConcurrencyStamp = Guid.NewGuid().ToString();


            builder.Entity<IdentityRole>().HasData(Adminrole,DefaultRole);

            base.OnModelCreating(builder);





        }




    }
}
