using Microsoft.AspNetCore.Identity;
using VideoCall.Identity.Models;

namespace VideoCall.Settings
{
    public static class ExtensionFunctions
    {
        private static IServiceProvider _serviceProvider;

        public static void RegisterServices(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }
        public async static Task<bool> AddToRoleIfNotExists(this UserManager<ApplicationUser> manager,ApplicationUser user,string rolename)
        {
            var rolemanager=GetService<RoleManager<ApplicationRole>>();
            if (rolemanager != null)
            {
                var exists =await rolemanager.RoleExistsAsync(rolename);
                if (!exists)
                {
                    var role = new ApplicationRole();
                    role.Name = rolename;
                    await rolemanager.CreateAsync(role);
                }
                var result=await manager.AddToRoleAsync(user, rolename);
                if (result!.Succeeded)
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
