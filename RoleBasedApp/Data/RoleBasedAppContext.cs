using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoleBasedApp.Models;

namespace RoleBasedApp.Data
{
    public class RoleBasedAppContext : DbContext
    {
        public RoleBasedAppContext()
        {
            
        }
        public RoleBasedAppContext (DbContextOptions<RoleBasedAppContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blog { get; set; }
    }
}
