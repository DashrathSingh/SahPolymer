using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WorkWellPipe.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var defaultPages = new List<ContentPages>();

            defaultPages.Add(new ContentPages() { PageCode = "ABOUT", Description = "ABOUT Standard", PageName="About Us",PageDescription="",CreatedDate=DateTime.Now,UpdatedDate = DateTime.Now });
            defaultPages.Add(new ContentPages() { PageCode = "MISSION", Description = "MISSION Standard", PageName = "Mission", PageDescription = "", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now });
            defaultPages.Add(new ContentPages() { PageCode = "VISION", Description = "VISION Standard", PageName = "Vision", PageDescription = "", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now });

            context.ContentPages.AddRange(defaultPages);

            base.Seed(context);
        }
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("WorkWellPipeContext", throwIfV1Schema: false)
        {
            Database.SetInitializer(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceImages> ServiceImages { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<BrandImages> BrandImages { get; set; }
        public DbSet<SliderImages> SliderImages { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<AlbumImages> AlbumImages { get; set; }
         public DbSet<Company> Companies { get; set; }
        public DbSet<ContentPages> ContentPages { get; set; }
        

    }
}