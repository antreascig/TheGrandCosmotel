using WebGames;
using WebGames.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebGames.Libs
{
    public class SecurityManager
    {
        public static List<IdentityRole> Roles { get; set; }

        public static void Init()
        {
            // Initialize roles

            using (var context = ApplicationDbContext.Create())
            {
                var RolesNames = new string[] { "sysadmin", "admin", "player" };
                var saveChanges = false;
                foreach (var roleName in RolesNames)
                {
                    if (context.Roles.FirstOrDefault(r => r.Name == roleName) == null)
                    {
                        context.Roles.Add(new IdentityRole { Name = roleName });
                        saveChanges = true;
                    }
                }
                // Save Roles
                if (saveChanges)
                {
                    context.SaveChanges();
                    saveChanges = false;
                }

                // initialize the Roles
                Roles = context.Roles.ToList();

                // Initialize persistent users
                var config = SettingsManager.GetConfig();

                foreach (var persUser in config.security.persistentUsers)
                {
                    var user = context.Users.FirstOrDefault(u => u.UserName == persUser.username);

                    //context.Users.Remove(user);
                    //context.SaveChanges();
                    //user = null;

                    if (user == null)
                    {
                        user = new ApplicationUser()
                        {
                            UserName = persUser.username,
                            Email = persUser.email,
                            FullName = persUser.username
                        };

                        using (var UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context)))
                        {
                            // Configure validation logic for usernames
                            UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager)
                            {
                                AllowOnlyAlphanumericUserNames = false,
                                RequireUniqueEmail = true
                            };
                            var res = UserManager.CreateAsync(user, persUser.password);
                            if (!res.Result.Succeeded)
                            {
                                return;
                            }
                        }
                        foreach ( var roleName in persUser.roles)
                        {
                            var role = WebGames.Libs.SecurityManager.Roles.FirstOrDefault(m => m.Name == roleName);
                            user.Roles.Add(new IdentityUserRole { RoleId = role.Id });
                            saveChanges = true;
                        }
                    }
                }

                if (saveChanges)
                {
                    context.SaveChanges();
                }

            }

        }
    }
}