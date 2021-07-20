using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProductFocus.Domain.Model;

namespace ProductFocus.Persistence
{
    public class ProductFocusDbContext: DbContext
    {
        public ProductFocusDbContext(DbContextOptions<ProductFocusDbContext> options) : base(options)
        {

        }

        public DbSet<Feature> Features { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
    }


    public class ProductFocusContextFactory : IDesignTimeDbContextFactory<ProductFocusDbContext>
    {
        public ProductFocusDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProductFocusDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS; Database=product-focus; Trusted_Connection=True;");

            return new ProductFocusDbContext(optionsBuilder.Options);
        }
    }
}
