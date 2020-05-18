using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwtproject.api.Models
{
    public class DemoContext:DbContext
    {
        public DemoContext(DbContextOptions options):base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
