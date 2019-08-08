using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StreetLegal.Models;
using StreetLegal.Models.CarModels;

namespace StreetLegal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
           
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Engine> Engines { get; set; }

        public DbSet<Car> Cars { get; set; }

        public DbSet<Tyres> Tyres { get; set; }

        public DbSet<Driver> Drivers { get; set; }
    }
}
