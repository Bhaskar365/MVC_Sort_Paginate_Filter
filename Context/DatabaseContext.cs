using Microsoft.EntityFrameworkCore;
using MVC_Sort_Pagination.Models;

namespace MVC_Sort_Pagination.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options):base(options) { }

        public DbSet<Employee> Employee_Table { get; set; }

    }
}
