using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<
                ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=ResourceManagmentSystem;Trusted_Connection=True;TrustServerCertificate=true;");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
