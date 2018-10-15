using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;
using Web.ViewModels.AccountAdminViewModels;

namespace Web.Controllers.Accounts
{
    [Authorize(Roles = "Admins")]
    public class AccountAdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserValidator<AppUser> _userValidator;
        private readonly IPasswordValidator<AppUser> _passwordValidator;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountAdminController(
            UserManager<AppUser> userManager,
            IUserValidator<AppUser> userValidator,
            IPasswordValidator<AppUser> passwordValidator,
            IPasswordHasher<AppUser> passwordHasher,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userValidator = userValidator;
            _passwordValidator = passwordValidator;
            _passwordHasher = passwordHasher;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var model = new ListViewModel();
            return View(model);
        }

        public async Task<IActionResult> EditAccount(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return NotFound();
            }

            var model = new EditViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                CreateAt = user.CreateAt,
                LastLogin = user.LastLogin,
                Active = user.Active,
                EmailConfirmed =  user.EmailConfirmed,
                Roles = new List<Role>()
            };
            await _setRolesInModelView(model, user);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccount(EditViewModel model)
        {
            var result = true;
            var user = await _userManager.FindByIdAsync(model.Id);
            model.Roles = new List<Role>();

            if (ModelState.IsValid)
            {
                user.Active = model.Active;
                user.PhoneNumber = model.PhoneNumber;
                user.EmailConfirmed = model.EmailConfirmed;

                // Password
                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    var validNewPassword = _passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                    if (validNewPassword.Result.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
                    }
                    else
                    {
                        var message = validNewPassword.Result.Errors.First().Description;
                        ModelState.AddModelError(nameof(model.NewPassword), message);
                        result = false;
                    }
                }

                // Roles
                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in userRoles)
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }

                if (model.IdRolesToAdd != null)
                {
                    foreach (var role in model.IdRolesToAdd)
                    {
                        await _userManager.AddToRoleAsync(user, role);
                    }
                }

                // Usuario
                if (user.UserName != model.UserName || user.Email != model.Email)
                {
                    var copyUserName = user.UserName;
                    var copyEmail = user.Email;

                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    var validUser = await _userValidator.ValidateAsync(_userManager, user);

                    if (!validUser.Succeeded)
                    {
                        result = false;
                        user.UserName = copyUserName;
                        user.Email = copyEmail;
                        _addErrorsFromResult(validUser);
                    }
                }

                if (result is true)
                {
                    await _userManager.UpdateAsync(user);
                    this.AddMessage("success", "Datos guardados con éxito");
                    return RedirectToAction(nameof(Index));
                }
            }
            await _setRolesInModelView(model, user);
            return View(model);
        }

        public IActionResult Create()
        {
            var model = new CreateViewModel
            {
                Roles = _roleManager.Roles.ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newUser = new AppUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = model.EmailConfirmed,
                    PhoneNumber = model.PhoneNumber,
                    CreateAt = DateTime.Now
                };

                var validUser = await _userValidator.ValidateAsync(_userManager, newUser);
                if (validUser.Succeeded)
                {
                    var validPassword = await _passwordValidator.ValidateAsync(_userManager, newUser, model.Password);
                    if (validPassword.Succeeded)
                    {
                        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, model.Password);

                        var createResult = await _userManager.CreateAsync(newUser);
                        if (createResult.Succeeded)
                        {
                            if (model.IdRolesToAdd != null && model.IdRolesToAdd.Length > 0)
                            {
                                foreach (var roleToAdd in model.IdRolesToAdd)
                                {
                                    await _userManager.AddToRoleAsync(newUser, roleToAdd);
                                }
                            }
                            this.AddMessage("success", "Usuario creado con éxito");
                            return RedirectToAction(nameof(Index));
                        }
                        _addErrorsFromResult(createResult);
                    }
                    else
                    {
                        _addErrorsFromResult(validPassword);
                    }
                }
                else
                {
                    _addErrorsFromResult(validUser);
                }
            }
            model.Roles = _roleManager.Roles.ToList();
            return View(model);
        }

        private void _addErrorsFromResult(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        private async Task _setRolesInModelView(EditViewModel model, AppUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles;
            foreach (var role in allRoles)
            {
                model.Roles.Add(new Role
                {
                    Key = role.Id,
                    Value = role.Name,
                    Selected = userRoles.Contains(role.Name) ? "checked" : ""
                });
            }
        }
    }
}
