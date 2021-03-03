using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotHhru.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Ad> Ads { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<WorkType> WorkTypes { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

    }
}
