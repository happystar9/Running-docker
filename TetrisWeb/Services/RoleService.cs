

using Microsoft.AspNetCore.Identity;
using TetrisWeb.AuthData;

public class RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager) {
public async Task CreateRoleAsync()
{
    // var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

   string roleName = "Admin";
    if (!await roleManager.RoleExistsAsync(roleName))
    {
        await roleManager.CreateAsync(new IdentityRole { Name = roleName, NormalizedName = roleName.ToUpper() });
    }
}

public async Task AssignRoleAsync(string userEmail)
{
    var user = await userManager.FindByNameAsync(userEmail);
    if (user != null)
    {
        string roleName = "Admin";
        if (!await userManager.IsInRoleAsync(user, roleName))
        {
            await userManager.AddToRoleAsync(user, roleName);
        }
    }
}

}