﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalBlog.Data;
using PersonalBlog.Models;

namespace PersonalBlog.Utilites
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context; 
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) { 
            _context= context;  
            _userManager= userManager;
            _roleManager= roleManager;
        }

        public void Initialize()
        {
            if (!_roleManager.RoleExistsAsync(WebsiteRoles.Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.Author)).GetAwaiter().GetResult();
                _userManager.CreateAsync(new ApplicationUser()
                {
                    UserName = "admin",
                    Email= "admin@gmail.com",
                    FirstName = "super",
                    LastName = "admin",
                },"Admin@001").Wait();
            }

            var appUser = _context.ApplicationUsers.FirstOrDefault(x => x.Email == "admin@gmail.com");
            if (appUser != null)
            { 
                _userManager.AddToRoleAsync(appUser,WebsiteRoles.Admin).GetAwaiter().GetResult();
            }
            var AboutPage = new Page()
            {
                Title = "About us",
                Slug = "about"
            };

            var ContactPage = new Page()
            {
                Title = "Contact us",
                Slug = "contact"
            };

            var PrivacyPolicyPage = new Page()
            {
                Title = "Privacy Policy",
                Slug = "privacy"
            };

            _context.Pages.Add(AboutPage);
            _context.Pages.Add(ContactPage);
            _context.Pages.Add(PrivacyPolicyPage);
            _context.SaveChanges();

            var ListOfPages = new List<Page>()
            {
                new Page()
                {
                    Title = "About Us",
                    Slug = "about"
                },
                new Page()
                {
                Title = "Contact us",
                Slug = "contact"
                },

                new Page()
                {
                    Title = "Privacy Policy",
                    Slug = "privacy"
                }
            };

            _context.Pages.AddRange(ListOfPages);
            _context.SaveChanges();
        }
    }
}
    