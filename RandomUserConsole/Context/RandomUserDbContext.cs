using RandomUserConsole.Models;
using System.Data.Entity;

namespace RandomUserConsole.Context
{
    class RandomUserDbContext: DbContext
    {
        public RandomUserDbContext() : base ("name=RandomUserContext")
        { }
        public DbSet<RandomUser> RandomUsers { get; set; }
    }
}
