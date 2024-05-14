using Microsoft.EntityFrameworkCore;
using Volontyor_Hakaton.Models;

namespace Volontyor_Hakaton.Data
{
    public class ApplicationDbContext
   : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Donats> Donats { get; set; }
        public DbSet<Expenses> Expenses { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<User_Project> User_Project { get; set; }

    }
}
