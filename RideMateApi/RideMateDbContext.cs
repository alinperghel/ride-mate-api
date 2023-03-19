using System;
using Microsoft.EntityFrameworkCore;

namespace RideMateApi
{
	public class RideMateDbContext : DbContext
	{
        public RideMateDbContext(DbContextOptions<RideMateDbContext> options)
        : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Ride> Rides { get; set; }
    }
}

