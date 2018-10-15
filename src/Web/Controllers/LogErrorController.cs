using System.Linq;
using ApplicationCore.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;
using Web.ViewModels.LogErrorViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = "Admins")]
    public class LogErrorController : Controller
    {
        private readonly ILogErrorRepository _logErrorRepository;

        public LogErrorController(ILogErrorRepository logErrorRepository)
        {
            _logErrorRepository = logErrorRepository;
        }

        public IActionResult Index()
        {
            var model = new ListViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAll()
        {
            var errors = _logErrorRepository.GetAll();
            _logErrorRepository.RemoveRange(errors.ToList());
            this.AddMessage("success", "Se ha eliminado el log de errors con éxito");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkAllRead()
        {
            _logErrorRepository.MarkAllChecked();
            this.AddMessage("success", "Todos los errores se marcardo como leídos con éxito");
            return RedirectToAction(nameof(Index));
        }
    }
}
