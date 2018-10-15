using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using ApplicationCore.Data;
using ApplicationCore.Data.Identity;
using ApplicationCore.Services.EmailServices;
using ApplicationCore.Services.ImageServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Web.Extensions;
using Web.ViewModels.AccountViewModels;

namespace Web.Controllers.Accounts
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserValidator<AppUser> _userValidator;
        private readonly IPasswordValidator<AppUser> _passwordValidator;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _env;

        public AccountController(
            ApplicationDbContext dbContext,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IUserValidator<AppUser> userValidator,
            IPasswordValidator<AppUser> passwordValidator,
            IPasswordHasher<AppUser> passwordHasher,
            IEmailService emailService,
            IConfiguration configuration,
            IHostingEnvironment env)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _userValidator = userValidator;
            _passwordValidator = passwordValidator;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
            _configuration = configuration;
            _env = env;
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _dbContext
                .Users
                .Include(u => u.SubscribeArticle)
                .FirstAsync(u => u.UserName == User.Identity.Name);
            var model = new ProfileViewModel {User = user};
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    var requireConfirmedEmail = _signInManager.Options.SignIn.RequireConfirmedEmail;
                    if (requireConfirmedEmail is false || requireConfirmedEmail && user.EmailConfirmed)
                    {
                        if (user.Active is true)
                        {
                            var result =
                                await _signInManager.PasswordSignInAsync(user, model.Password, model.Remember, false);

                            if (result.Succeeded)
                            {
                                user.LastLogin = DateTime.Now;
                                await _userManager.UpdateAsync(user);
                                return Redirect(returnUrl ?? Url.Action(nameof(Profile)));
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "La cuenta no esta activa, por favor hable con un responsable");
                        }
                    }
                    else
                    {
                        return RedirectToAction(nameof(ConfirmEmail), new {userId = user.Id});
                    }
                }
            }

            ModelState.AddModelError("", "Nombre de usuario o contraseña no valido");
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newUser = new AppUser
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                var validUser = await _userValidator.ValidateAsync(_userManager, newUser);
                if (validUser.Succeeded)
                {
                    var validPassword = await _passwordValidator.ValidateAsync(_userManager, newUser, model.Password);
                    if (validPassword.Succeeded)
                    {
                        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, model.Password);
                        newUser.LastLogin = DateTime.Now;
                        newUser.CreateAt = DateTime.Now;

                        var createResult = await _userManager.CreateAsync(newUser, model.Password);
                        if (createResult.Succeeded)
                        {
                            this.AddMessage("success", "Cuenta creada con éxito");
                            return RedirectToAction(nameof(RegisterConfirmation), new {userId = newUser.Id});
                        }
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

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> RegisterConfirmation(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return NotFound();
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callBack = Url.Action(nameof(ConfirmEmail), "Account", new
            {
                userId = user.Id,
                token
            }, HttpContext.Request.Scheme);

            var model = new RegisterConfirmationViewModel
            {
                CallBack = callBack,
                SiteName = _configuration["Site:SiteName"]
            };

            _emailService.To = new List<MailAddress> {new MailAddress(user.Email)};
            _emailService.Subject = $"Verificación de registro en {_configuration["Site:SiteName"]}";
            await _emailService.SendEmailAsync("AccountRegisterConfirmation", model);
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            string message;
            if (result.Succeeded)
            {
                message = "El email ha sido verificado correctamente";
                this.AddMessage("success", message);
                return RedirectToAction(nameof(Profile));
            }

            message = "Se ha enviado un email para la verificación";
            this.AddMessage("warning", message);
            return RedirectToAction(nameof(RegisterConfirmation), new {userId = user.Id});
        }

        public IActionResult EditPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPassword(EditPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var checkPassword = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
                if (checkPassword)
                {
                    var validNewPassword = _passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                    if (validNewPassword.Result.Succeeded)
                    {
                        var changePasswordResult = await _userManager.ChangePasswordAsync(
                            user,
                            model.CurrentPassword,
                            model.NewPassword
                        );

                        if (changePasswordResult.Succeeded)
                        {
                            this.AddMessage("success", "Contraseña cambiada con éxito");
                            return RedirectToAction(nameof(Profile));
                        }

                        _addErrorsFromResult(changePasswordResult);
                    }
                    else
                    {
                        var message = validNewPassword.Result.Errors.First().Description;
                        ModelState.AddModelError(nameof(model.NewPassword), message);
                    }
                }
                else
                {
                    ModelState.AddModelError(nameof(model.CurrentPassword), "La contraseña actual no es correcta");
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult RecoveryPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecoveryPassword(RecoveryPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var siteName = _configuration["Site:SiteName"];
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var callBack = Url.Action(nameof(ResetPassword), "Account", new
                    {
                        userId = user.Id,
                        token
                    }, HttpContext.Request.Scheme);

                    var modelEmail = new RecoveryPasswordSendViewModel
                    {
                        CallBack = callBack,
                        SiteName = siteName
                    };

                    _emailService.To = new List<MailAddress> {new MailAddress(user.Email)};
                    _emailService.Subject = $"Recuperación de contraseña - {siteName}";
                    await _emailService.SendEmailAsync("AccountRecoveryPassword", modelEmail);
                    this.AddMessage("success", "Se ha enviado un email para recuperar la contraseña");
                    return RedirectToAction(nameof(Login));
                }

                ModelState.AddModelError(nameof(model.Email), "Email no encontrado");
            }

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return NotFound();
            }

            var model = new ResetPasswordViewModel
            {
                UserId = userId,
                Token = token
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user is null)
                {
                    return NotFound();
                }

                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    this.AddMessage("success", "Contraseña establecida con éxito");
                    return RedirectToAction(nameof(Login));
                }

                _addErrorsFromResult(result);
            }

            return View(model);
        }

        public async Task<IActionResult> EditEmail()
        {
            var user = await _userManager.GetUserAsync(User);
            var requireConfirmedEmail = _signInManager.Options.SignIn.RequireConfirmedEmail;
            var model = new EditEmailViewModel
            {
                RequireConfirmedEmail = requireConfirmedEmail
            };

            if (requireConfirmedEmail)
            {
                model.CurrentEmail = user.Email;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmail(EditEmailViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                // Solo se cambia el email para poderlo validar, después se ha poner el que tenia.
                var currentEmail = user.Email;
                user.Email = model.NewEmail;
                var validEmail = await _userValidator.ValidateAsync(_userManager, user);
                user.Email = currentEmail;
                user.TemporalEmailChange = model.NewEmail;

                if (validEmail.Succeeded)
                {
                    if (model.RequireConfirmedEmail is false)
                    {
                        await _userManager.UpdateAsync(user);
                        this.AddMessage("success", "Cambiado email con éxito");
                        return RedirectToAction(nameof(Profile));
                    }

                    await _userManager.UpdateAsync(user);
                    var token = await _userManager.GenerateChangeEmailTokenAsync(user, model.NewEmail);
                    var callBack = Url.Action(nameof(ConfirmEditEmail), "Account", new
                    {
                        userId = user.Id,
                        token
                    }, HttpContext.Request.Scheme);

                    var modelConfirmEmail = new EditEmailConfirmationViewModel
                    {
                        CallBack = callBack,
                        SiteName = _configuration["Site:SiteName"]
                    };

                    _emailService.To = new List<MailAddress> {new MailAddress(user.Email)};
                    _emailService.Subject = $"Confirmación de email {_configuration["Site:SiteName"]}";
                    await _emailService.SendEmailAsync("AccountEditEmailConfirmation", modelConfirmEmail);
                    this.AddMessage("success", "Se ha enviado un email para la confirmación");
                    return RedirectToAction(nameof(Profile), new {userId = user.Id});
                }

                var message = validEmail.Errors.First().Description;
                ModelState.AddModelError(nameof(model.NewEmail), message);
            }

            return View(model);
        }

        public async Task<IActionResult> ConfirmEditEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ChangeEmailAsync(user, user.TemporalEmailChange, token);
            if (result.Succeeded)
            {
                user.TemporalEmailChange = string.Empty;
                await _userManager.UpdateAsync(user);
                this.AddMessage("success", "Email confirmado con éxito");
            }
            else
            {
                this.AddMessage("error", "El email no ha podido ser cambiado, inténtalo de nuevo");
            }

            return RedirectToAction(nameof(Profile));
        }

        public async Task<IActionResult> RemoveTemporalEmail()
        {
            var user = await _userManager.GetUserAsync(User);
            user.TemporalEmailChange = string.Empty;
            await _userManager.UpdateAsync(user);
            this.AddMessage("success", "Eliminado email sin confirmar");
            return RedirectToAction(nameof(Profile));
        }

        public async Task<IActionResult> EditPhoto()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new EditPhotoViewModel
            {
                CurrentPhoto = user.Photo
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPhoto(
            [FromServices] IUploadImageService uploadImageService,
            EditPhotoViewModel model)
        {
            var defaultImageName = "user.png";
            var uploadTo = Path.Combine("accounts", "profiles");
            var maxWidth = 150;
            var maxHeight = 150;
            var user = await _userManager.GetUserAsync(User);
            var oldUserPhoto = user.Photo;
            var mediaPath = _configuration["Images:Path"].Trim('/').Replace('/', Path.DirectorySeparatorChar);

            if (ModelState.IsValid)
            {
                if (model.Restore != true && model.UploadPhoto != null)
                {
                    var result = await uploadImageService.UploadAndResizeAsync(
                        model.UploadPhoto, uploadTo, maxWidth, maxHeight
                    );

                    if (result.Success)
                    {
                        user.Photo = Path.Combine(uploadTo, result.ReturnMessages["filename"]);
                        await _userManager.UpdateAsync(user);
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.UploadPhoto), result.Errors.First());
                        return View(model);
                    }
                }
                else if (model.Restore is true)
                {
                    user.Photo = Path.Combine(uploadTo, defaultImageName);
                    await _userManager.UpdateAsync(user);
                }

                if (model.Restore is true || model.UploadPhoto != null)
                {
                    if (!oldUserPhoto.EndsWith(defaultImageName))
                    {
                        var oldPhotoPath = Path.Combine(_env.WebRootPath, mediaPath, oldUserPhoto);
                        if (System.IO.File.Exists(oldPhotoPath))
                        {
                            System.IO.File.Delete(oldPhotoPath);
                        }
                    }

                    this.AddMessage("success", "Imagen cambiada con éxito");
                }

                return RedirectToAction(nameof(Profile));
            }

            return View(model);
        }

        private void _addErrorsFromResult(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
