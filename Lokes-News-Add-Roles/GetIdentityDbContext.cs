using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lokes_News_Add_Roles
{
    public class GetIdentityDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public GetIdentityDbContext(DbContextOptions<GetIdentityDbContext> options) : base(options)
        {
            
        }
    }
}
