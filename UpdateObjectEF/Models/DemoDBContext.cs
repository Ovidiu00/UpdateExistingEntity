using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateObjectEF.Models
{
    public class DemoDBContext : DbContext
    {

        public DemoDBContext(DbContextOptions options) :base(options)
        {

        }
        public DemoDBContext()
        {

        }

        public DbSet<Employee> Employees { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=DemoDB;Trusted_Connection=True;").EnableSensitiveDataLogging().LogTo(Console.WriteLine);
        }
    }
}