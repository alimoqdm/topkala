using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MvcMovie.Models;

namespace DataLayer
{
    public class TopKala3Contex : IdentityDbContext<ApplicationUser>
    {
        public TopKala3Contex()
        {
        }

        public TopKala3Contex(DbContextOptions<TopKala3Contex> options)
            : base(options)
        {
            
        }

    
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductPathImage> ProductPathImages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Properties> Properties { get; set; }
        public DbSet<AttributeGroup> AttibuteGroups { get; set; }
        public DbSet<Attributee> Attributes { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<SilderImage> SilderImage { get; set; }
        public DbSet<Brand> Brands { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            var cascadex = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(k => !k.IsOwnership && k.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var item in cascadex)
                item.DeleteBehavior = DeleteBehavior.Restrict;
        }


    

        }
    }
