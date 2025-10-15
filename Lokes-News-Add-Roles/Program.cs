using Lokes_News_Add_Roles.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Lokes_News_Add_Roles
{
    internal class Program
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public Program(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        static void Main(string[] args)
        {
            //ran: Scaffold-DbContext "Server=(localdb)\mssqllocaldb;Database=LokesNewsDB;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Data -Tables AspNetRoles,AspNetUsers,AspNetUserRoles

            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=LokesNewsDB;Trusted_Connection=True;";
            var services = new ServiceCollection();

            services.AddDbContext<LokesNewsDbContext>(
                options => options.UseSqlServer(connectionString));

            services.AddDbContext<GetIdentityDbContext>(
                options => options.UseSqlServer(connectionString));

            services.AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<GetIdentityDbContext>();

            var serviceProvider = services.BuildServiceProvider();
            var program = new Program(serviceProvider.GetRequiredService<UserManager<IdentityUser>>(), serviceProvider.GetRequiredService<RoleManager<IdentityRole>>());

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Add methods bellow and call them here from the program object and using .Wait():

            //program.AddRolesAsync().Wait();

            // Remember to comment out the called Task after running it once
        }

        public async Task AddRolesAsync()
        {
            var roles = new List<IdentityRole>
            {
                new IdentityRole("Reader"),
                new IdentityRole("Writer"),
                new IdentityRole("Editor"),
                new IdentityRole("Admin")
            };
            foreach(var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role.Name!))
                {
                    await _roleManager.CreateAsync(role);
                }
            }
            return;
        }
    }
}
