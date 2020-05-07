using DatingApp_BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp_BackEnd.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public DbSet<Value> Values {get; set;}

        public DbSet<User> Users {get; set;}
    }
}