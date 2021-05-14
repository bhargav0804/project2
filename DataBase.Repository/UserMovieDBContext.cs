using DataBase.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Repository
{
    public class UserMovieDBContext : DbContext
    {
        public DbSet<UserInfo> Users { get; set; }
        public DbSet<MoviesInfo> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<StarCast> StarCasts { get; set; }
        protected override void OnModelCreating(DbModelBuilder ModelBuilder)
        {
            ModelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
        }
    }
}
