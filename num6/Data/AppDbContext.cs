using Microsoft.EntityFrameworkCore;
using num6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace num6.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<WhiteList> WhiteLists { get; set; }
    }
}
