using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SportFieldBooking.Data.Model;

namespace SportFieldBooking.Data
{
    public class SQLServerDbContext : DomainDbContext
    {
        public SQLServerDbContext(IConfiguration configuration) : base(configuration) { }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration["DatabaseOptions:ConnectionStrings:SQLServer"]);
        }
    }
}
