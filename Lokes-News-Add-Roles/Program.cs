using Lokes_News_Add_Roles.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using System;

namespace Lokes_News_Add_Roles
{
    internal class Program
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        // Has to be changed if changing to new Db
        private readonly LokesNewsDbv2Context _context;
        private readonly GetIdentityDbContext _getIdentityDbContext;

        public Program(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, LokesNewsDbv2Context context, GetIdentityDbContext getIdentityDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _getIdentityDbContext = getIdentityDbContext;
        }

        static void Main(string[] args)
        {
            //ran: Scaffold-DbContext "Server=tcp:lokesnewsserver.database.windows.net,1433;Initial Catalog=LokesNewsDBv2;Persist Security Info=False;User ID=admin1234;Password=Admin@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Data -Tables AspNetRoles,AspNetUsers,AspNetUserRoles

            var connectionString = "Server=tcp:lokesnewsserver.database.windows.net,1433;Initial Catalog=LokesNewsDBv2;Persist Security Info=False;User ID=admin1234;Password=Admin@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            var services = new ServiceCollection();

            services.AddDbContext<LokesNewsDbv2Context>(
                options => options.UseSqlServer(connectionString));

            services.AddDbContext<GetIdentityDbContext>(
                options => options.UseSqlServer(connectionString));

            services.AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<GetIdentityDbContext>();

            var serviceProvider = services.BuildServiceProvider();

            var program = new Program(
                serviceProvider.GetRequiredService<UserManager<IdentityUser>>(),
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>(),
                serviceProvider.GetRequiredService<LokesNewsDbv2Context>(),
                serviceProvider.GetRequiredService<GetIdentityDbContext>()
            );

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Add methods bellow and call them here from the program object and using .Wait():

            program.AddRolesAsync().Wait();
            program.AddSubscriptionTypesAsync().Wait();
            program.AddCategoriesAsync().Wait();
            program.AddAdminAsync().Wait();

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

        public async Task AddSubscriptionTypesAsync()
        {
            var subTypes = new List<SubscriptionType>
            {
                new SubscriptionType
                {
                    TypeName = "Free",
                    Description = "Stay informed with our regular email newsletters.\nPerfect for casual readers who want custom updates delivered to their inbox.",
                    Price = 0M
                },
                new SubscriptionType
                {
                    TypeName = "Standard",
                    Description = "Receive newsletters and gain access to subscriber-only articles.\nIdeal for readers who want to see our exclusive articles reserved for subscribers.",
                    Price = 49.00M
                },
                new SubscriptionType
                {
                    TypeName = "Premium",
                    Description = "Get newsletters, exclusive articles and full archive access.\nFor dedicated readers that want to explore our complete library of articles",
                    Price = 99.00M
                }
            };
            foreach (var subType in subTypes)
            {
                if (!_context.SubscriptionTypes.Any(st => st.TypeName == subType.TypeName))
                {
                    _context.SubscriptionTypes.Add(subType);
                }
            }
            await _context.SaveChangesAsync();
            return;
        }

        public async Task AddCategoriesAsync()
        {
            var categories = new List<Category>
            {
                new Category { Name = "Sweden" },
                new Category { Name = "Tech" },
                new Category { Name = "Ecomomy" },
                new Category { Name = "Politics" },
                new Category { Name = "Sport" },
                new Category { Name = "Lifestyle" },
                new Category { Name = "Entertainment" }
            };
            foreach (var category in categories)
            {
                if (!_context.Categories.Any(c => c.Name == category.Name))
                {
                    _context.Categories.Add(category);
                }
            }
            await _context.SaveChangesAsync();
            return;
        }

        public async Task AddAdminAsync()
        {
            // Removing FK dependent
            _getIdentityDbContext.UserRoles.RemoveRange(_getIdentityDbContext.UserRoles);
            _getIdentityDbContext.UserClaims.RemoveRange(_getIdentityDbContext.UserClaims);
            _getIdentityDbContext.UserLogins.RemoveRange(_getIdentityDbContext.UserLogins);
            _getIdentityDbContext.UserTokens.RemoveRange(_getIdentityDbContext.UserTokens);
            // Removing the User
            _getIdentityDbContext.Users.RemoveRange(_getIdentityDbContext.Users);
            await _getIdentityDbContext.SaveChangesAsync();
            var adminUser = await _context.AspNetUsers
                .Where(u => u.Email == "admin@gmail.com")
                .FirstOrDefaultAsync();
            if (adminUser == null)
            {
                var user = new IdentityUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                    TwoFactorEnabled = false
                };
                await _userManager.CreateAsync(user, "Admin@1234");
                var idUser = await _getIdentityDbContext.Users.Where(u => u.Email == "admin@gmail.com").FirstOrDefaultAsync();
                await _userManager.AddToRoleAsync(idUser, "Admin");
                var roleClone = await _getIdentityDbContext.UserRoles.Where(ur => ur.UserId == idUser.Id).FirstOrDefaultAsync();
                var newUser = new AspNetUser
                {
                    Id = idUser!.Id,
                    UserName = idUser.UserName,
                    NormalizedUserName = idUser.NormalizedUserName,
                    Email = idUser.Email,
                    NormalizedEmail = idUser.NormalizedEmail,
                    EmailConfirmed = idUser.EmailConfirmed,
                    PasswordHash = idUser.PasswordHash,
                    SecurityStamp = idUser.SecurityStamp,
                    ConcurrencyStamp = idUser.ConcurrencyStamp,
                    PhoneNumber = idUser.PhoneNumber,
                    PhoneNumberConfirmed = idUser.PhoneNumberConfirmed,
                    TwoFactorEnabled = idUser.TwoFactorEnabled,
                    LockoutEnd = idUser.LockoutEnd,
                    LockoutEnabled = idUser.LockoutEnabled,
                    AccessFailedCount = idUser.AccessFailedCount,
                    FirstName = "Admin",
                    LastName = "Admin",
                    Dob = new DateTime(1990, 1, 1)
                }
            ;
                _getIdentityDbContext.Users.Remove(idUser);
                _getIdentityDbContext.SaveChanges();
                await _context.AspNetUsers.AddAsync(newUser);
                await _context.SaveChangesAsync();
                await _getIdentityDbContext.UserRoles.AddAsync(roleClone!);
                _getIdentityDbContext.SaveChanges();
            }
            return;
        }
    }
}
