using System;
using System.Threading.Tasks;
using ApplicationCore.Data.Entities.Blog;
using ApplicationCore.Data.Identity;
using ApplicationCore.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;

namespace Web.Controllers.Blog
{
    public class ArticleSubscribeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ISubscribeArticleRepository _subscribeArticleRepository;

        public ArticleSubscribeController(
            UserManager<AppUser> userManager,
            ISubscribeArticleRepository subscribeArticleRepository)
        {
            _userManager = userManager;
            _subscribeArticleRepository = subscribeArticleRepository;
        }

        /// <summary>
        /// Aprovecha FooterViewModel para el formulario.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe(string emailSubscribe, string returnUrl)
        {
            var isEmailRegister = _subscribeArticleRepository.EmailExists(emailSubscribe);
            var subscribeArticle = new SubscribeArticle
            {
                Email = emailSubscribe
            };

            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user != null && (user.SubscribeArticle != null || isEmailRegister))
                    {
                        this.AddMessage("warning", "Ya estas subscrito para recibir alertas");
                    }
                    else
                    {
                        if (user != null)
                        {
                            subscribeArticle.UserId = user.Id;
                            await _subscribeArticleRepository.SubscribeAsync(subscribeArticle);
                            user.SubscribeArticleId = subscribeArticle.Id;
                            await _userManager.UpdateAsync(user);
                        }
                        else
                        {
                            throw new NullReferenceException();
                        }
                    }
                }
                else
                {
                    // Si el email esta registrado, se notifica
                    if (isEmailRegister)
                    {
                        this.AddMessage("warning", "El email que insertas ya esta registrado");
                    }
                    else
                    {
                        await _subscribeArticleRepository.SubscribeAsync(subscribeArticle);
                    }
                }

                if (subscribeArticle.Token != Guid.Empty)
                {
                    this.AddMessage("success", "Te has subscrito con éxito!");
                }
            }

            return Redirect(returnUrl ?? "/");
        }

        public async Task<IActionResult> UnSubscribe(string token, string returnUrl = "/")
        {
            var subscriberArticle = _subscribeArticleRepository.GetByToken(token);
            if (subscriberArticle is null)
            {
                this.AddMessage("warning", "Email no encontrado");
            }
            else
            {
                // Si es un usuario registrado, el comportamiento de AppUserSubscribeArticleId es
                // DeleteBehavior.SetNull por lo que AppUser.SubscribeArticleId se establecera en null.
                await _subscribeArticleRepository.UnSubscribeByTokenAsync(token);
                this.AddMessage("success", "Te has dado de baja de las notificaciones con éxito");
            }

            return Redirect(returnUrl);
        }
    }
}
