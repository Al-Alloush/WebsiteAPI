using Core.Models.Identity;
using Core.Models.Uploads;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public static class InitializeSeedData
    {
        static CultureInfo MyCultureInfo = new CultureInfo("de-DE");

        public static async Task AddSeedUsersData(IServiceProvider services)
        {


            // Get UserManager Service to Application
            UserManager<AppUser> userManager = services.GetRequiredService<UserManager<AppUser>>();
            var context = services.GetRequiredService<AppDbContext>();
            Random random = new Random();

            // create Users list
            for (int i = 1; i <= 100; i++)
            {
                var Confirmed = true;
                var name = "visitor";
                var role = "Visitor";
                var AuPass = false;
                var lang = "de,";

                if (i >= 20 && i < 25)
                {
                    AuPass = true;
                }

                if (i >= 25 && i < 30)
                {
                    name = "admin";
                    role = "Admin";
                }
                if (i >= 30 && i < 35)
                {
                    name = "editor";
                    role = "Editor";
                }
                if (i >= 40 && i < 50)
                {
                    Confirmed = false;
                }

                if (i % 2 == 0)
                    lang = "de,ar,";

                if (i % 7 == 0)
                    lang = "ar,";

                if (i % 8 == 1)
                    lang = "en,fr,";

                var userS = new AppUser
                {
                    Email = name + "_" + i + "@al-alloush.com",
                    EmailConfirmed = Confirmed,
                    UserName = name + "_" + i,
                    FirstName = "First " + name + " Name",
                    LastName = "Last " + name + " Name",
                    SelectedLanguages = lang,
                    RegisterDate = DateTime.Now,
                    Birthday = DateTime.Parse("15/6/1978", MyCultureInfo),
                    PhoneNumber = "17328547" + i,
                    PhoneNumberConfirmed = Confirmed,
                    AutomatedPassword = AuPass,
                    Address = new Address
                    {
                        Street = "street_" + i,
                        BuildingNum = "12",
                        Flore = "L 3",
                        City = "city_" + i,
                        Zipcode = "874" + i,
                        Country = "German"
                    }
                };
                IdentityResult createS = await userManager.CreateAsync(userS, "!QA1qa");
                if (createS.Succeeded)
                    userManager.AddToRoleAsync(userS, role).Wait();
            }

            var users = await context.Users.ToArrayAsync();
            for (int i = 1; i < users.Length; i++)
            {
                var imgNum = random.Next(1, 52);
                var upload = new Upload
                {
                    Name = "user (" + imgNum + "): " + random.Next(1, 100000),
                    Path = "/Uploads/Images/user (" + imgNum + ").jpg",
                    AddedDateTime = DateTime.Now,
                    UserId = users[i].Id
                };
                await context.Upload.AddAsync(upload);
                await context.SaveChangesAsync();
            }

            for (int i = 1; i < users.Length; i++)
            {
                var userUploads = await context.Upload.FirstOrDefaultAsync(u => u.UserId == users[i].Id);

                var userImage = new UploadUserImagesList
                {
                    UploadId = userUploads.Id,
                    UserId = users[i].Id,
                    Default = true,
                    UploadTypeId = 1
                };
                await context.UploadUserImagesList.AddAsync(userImage);
                await context.SaveChangesAsync();
            }
        }
    }
}
