using Hotel_Management.UI.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FA.JustBlog.UI.Areas.Identity.Pages.Roles
{
    public class AddUserRoleModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<Hotel_ManagementUIUser> _userManager;

        public AddUserRoleModel(RoleManager<IdentityRole> roleManager,
            UserManager<Hotel_ManagementUIUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;

        }
        public class InputModel
        {
            [Required]
            public string Id { set; get; }
            public string Name { set; get; }

            public string[] RoleNames { set; get; }

        }

        [BindProperty]
        public InputModel Input { set; get; }

        [BindProperty]
        public bool isConfirmed { set; get; }

        [TempData] // Sử dụng Session
        public string StatusMessage { get; set; }

        public IActionResult OnGet() => NotFound("Không thấy");

        public List<string> AllRoles { get; set; } = new List<string>();

        public async Task<IActionResult> OnPost()
        {
            var user = await _userManager.FindByIdAsync(Input.Id);

            if (user == null)
            {
                return NotFound("Not found!");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();

            allRoles.ForEach(r => { AllRoles.Add(r.Name); });

            if (!isConfirmed)
            {
                Input.RoleNames = roles.ToArray();
                isConfirmed = true;
                StatusMessage = "";
                ModelState.Clear();
            }
            else
            {
                // Update add and remove
                StatusMessage = "Vừa cập nhật";
                if (Input.RoleNames == null) Input.RoleNames = new string[] { };
                foreach (var rolename in Input.RoleNames)
                {
                    if (roles.Contains(rolename)) continue;
                    await _userManager.AddToRoleAsync(user, rolename);
                }
                foreach (var rolename in roles)
                {
                    if (Input.RoleNames.Contains(rolename)) continue;
                    await _userManager.RemoveFromRoleAsync(user, rolename);
                }
                return RedirectToPage("./User");
            }

            Input.Name = user.UserName;

            return Page();
        }
    }
}
