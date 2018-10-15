using System.Threading.Tasks;
using ApplicationCore.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;
using Web.ViewModels.LogEmailViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = "Admins")]
    public class LogEmailController : Controller
    {
        private readonly ILogEmailRepository _logEmailRepository;

        public LogEmailController(
            ILogEmailRepository logEmailRepository)
        {
            _logEmailRepository = logEmailRepository;
        }

        public IActionResult Index()
        {
            var model = new ListViewModel();
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var model = _logEmailRepository.GetById(id);
            if (model is null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var email = _logEmailRepository.GetById(id);
            if (email is null)
            {
                return NotFound();
            }

            await _logEmailRepository.RemoveAsync(email);
            this.AddMessage("success", "Registro de email eliminado con éxito");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAll()
        {
            var emails = _logEmailRepository.GetAll();
            _logEmailRepository.RemoveRange(emails);
            this.AddMessage("success", "Se han eliminado todos los registros con éxito");
            return RedirectToAction(nameof(Index));
        }
    }
}
