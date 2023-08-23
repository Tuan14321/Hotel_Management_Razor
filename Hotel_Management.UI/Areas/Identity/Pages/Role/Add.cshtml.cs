using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FA.JustBlog.UI.Areas.Identity.Pages.Roles
{
    public class AddModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            public string Id { get; set; }

            [Required]
            [Display(Name = "Role Name")]
            [StringLength(100, ErrorMessage = "{0} length {2} to {1} character.", MinimumLength = 3)]
            public string Name { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public bool IsUpdate { get; set; }

        // Không cho truy cập trang mặc định mà không có handler
        public IActionResult OnGet() => NotFound("Not Found");
        public IActionResult OnPost() => NotFound("Not Found");

        public IActionResult OnPostStartNewRole()
        {
            StatusMessage = "Input new role";
            IsUpdate = false;
            Input.Id = "1";
            ModelState.Clear();

            return Page();
        }

        public async Task<IActionResult> OnPostStartUpdate()
        {
            StatusMessage = null;
            IsUpdate = true;

            if (string.IsNullOrEmpty(Input.Id))
            {
                StatusMessage = "Error: No role";

                return Page();
            }

            var result = await _roleManager.FindByIdAsync(Input.Id);

            if (result != null)
            {
                Input.Name = result.Name;
                ViewData["Title"] = $"Update Role {result.Name}";
                ModelState.Clear();
            }
            else
            {
                StatusMessage = $"Error: Not found {Input.Id}";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddOrUpdate()
        {
            if (!ModelState.IsValid)
            {
                StatusMessage = null;

                return Page();
            }

            if (IsUpdate)
                return await UpdateRole();
            else
                return await AddRole();

        }

        private async Task<IActionResult> UpdateRole()
        {
            if (Input.Id == null)
            {
                StatusMessage = "Error: not found roleId";
                return Page();
            }

            var result = await _roleManager.FindByIdAsync(Input.Id);

            if (result != null)
            {
                result.Name = Input.Name;

                var roleUpdateRs = await _roleManager.UpdateAsync(result);

                if (roleUpdateRs.Succeeded)
                {
                    StatusMessage = "Update Role Success!";
                    return RedirectToPage("./Index");
                }
                else
                {
                    StatusMessage = "Error: ";
                    foreach (var er in roleUpdateRs.Errors)
                    {
                        StatusMessage += er.Description;
                    }
                }
            }
            else
            {
                StatusMessage = "Error: Not found role";
            }

            return Page();
        }

        private async Task<IActionResult> AddRole()
        {
            var newRole = new IdentityRole() { Name = Input.Name };

            var rsNewRole = await _roleManager.CreateAsync(newRole);

            if (rsNewRole.Succeeded)
            {
                StatusMessage = "Add role success!";
                return RedirectToPage("./Index");
            }
            else
            {
                StatusMessage = "Error: ";
                foreach (var er in rsNewRole.Errors)
                {
                    StatusMessage += er.Description;
                }
            }

            return Page();
        }
    }

}
