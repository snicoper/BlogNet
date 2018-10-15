using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Data.Entities;
using ApplicationCore.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels.LogEmailViewModels;

namespace Web.Api
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admins")]
    public class LogEmailController : Controller
    {
        private readonly ILogEmailRepository _logEmailRepository;

        public LogEmailController(ILogEmailRepository logEmailRepository)
        {
            _logEmailRepository = logEmailRepository;
        }

        [HttpGet]
        public IQueryable<LogEmail> GelAll()
        {
            return _logEmailRepository.GetAll();
        }

        [HttpGet("paginate")]
        public ListViewModel GelAll(ListViewModel paginate)
        {
            var query = GelAll();

            // Filtros
            if (DateTime.TryParse(paginate.SendAt, out var sendAt))
            {
                query = query.Where(u => u.SendAt.ToShortDateString() == sendAt.ToShortDateString());
            }

            if (!string.IsNullOrEmpty(paginate.From))
            {
                query = query.Where(u => u.From.ToLower().Contains(paginate.From.ToLower()));
            }

            if (!string.IsNullOrEmpty(paginate.To))
            {
                query = query.Where(u => u.To.ToLower().Contains(paginate.To.ToLower()));
            }

            if (!string.IsNullOrEmpty(paginate.Subject))
            {
                query = query.Where(u => u.Subject.ToLower().Contains(paginate.Subject.ToLower()));
            }

            paginate.Paginate(query);
            return paginate;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var email = _logEmailRepository.GetById(id);
            if (email is null)
            {
                return NotFound();
            }

            await _logEmailRepository.RemoveAsync(email);
            return new NoContentResult();
        }
    }
}
