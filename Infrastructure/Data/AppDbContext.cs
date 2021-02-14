using Core.Models.Blogs;
using Core.Models.Identity;
using Core.Models.Settings;
using Core.Models.Uploads;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    /*
    if we want customise IdentityUser class we need to create new class inherent from IdentityUser and pass it :
        IdentityDbContext<AppUser>
    IdentityDbContext inherits from DbContext for that we just need this AppDbContext class to work with database
     */

    public class AppDbContext : IdentityDbContext<AppUser>
    {
        // this Constructor is required
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // for all Tables of EntityFrameworkCore like (Users, Roles, ...) initialize here by default, IdentityDbContext took care of all of the work to create tables and relational between them.
        public DbSet<Address> Address { get; set; }
        public DbSet<UploadType> UploadType { get; set; }
        public DbSet<UploadUserImagesList> UploadUserImagesList { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<UserSelectedLanguages> UserSelectedLanguages { get; set; }
        
        //
        public DbSet<BlogSourceCategoryName> BlogSourceCategoryName { get; set; }
        public DbSet<Blog> Blog { get; set; }
        public DbSet<BlogCategory> BlogCategory { get; set; }
        public DbSet<BlogCategoryList> BlogCategoryList { get; set; }
        public DbSet<BlogLike> BlogLike { get; set; }
        public DbSet<BlogComment> BlogComment { get; set; }
        public DbSet<UploadBlogImagesList> UploadBlogImagesList { get; set; }
    }
}
