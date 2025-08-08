using DIY_IMPULSE_API_PROJECT.DAL;
using Microsoft.EntityFrameworkCore;

namespace DIY_IMPULSE_API_PROJECT.DBENGINE
{
    public class DBEngine : DbContext
    {
        public DBEngine(DbContextOptions<DBEngine> options) : base(options)
        {
            
        }

        public DbSet<Tbl_Users> Tbl_Users { get; set; }
        public DbSet<Tbl_RefreshToken> Tbl_RefreshToken { get; set; }

    }
}
