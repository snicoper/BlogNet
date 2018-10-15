using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Data.Entities;
using ApplicationCore.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels.LogErrorViewModels;

namespace Web.Api
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admins")]
    public class LogErrorController : Controller
    {
        private readonly ILogErrorRepository _logErrorRepository;

        public LogErrorController(ILogErrorRepository logErrorRepository)
        {
            _logErrorRepository = logErrorRepository;
        }

        [HttpGet]
        public IQueryable<LogError> GetAll()
        {
            return _logErrorRepository.GetAll();
        }

        [HttpGet("paginate")]
        public ListViewModel GetAll(ListViewModel paginate)
        {
            var query = GetAll();

            // Filtros
            if (DateTime.TryParse(paginate.CreateAt, out var createAt))
            {
                query = query.Where(u => u.CreateAt.ToShortDateString() == createAt.ToShortDateString());
            }

            if (!string.IsNullOrEmpty(paginate.Username))
            {
                query = query.Where(u => u.Username.ToLower().Contains(paginate.Username.ToLower()));
            }

            if (!string.IsNullOrEmpty(paginate.Path))
            {
                query = query.Where(u => u.Path.ToLower().Contains(paginate.Path.ToLower()));
            }

            if (!string.IsNullOrEmpty(paginate.Message))
            {
                query = query.Where(u => u.Message.ToLower().Contains(paginate.Message.ToLower()));
            }

            if (paginate.Checked.HasValue)
            {
                query = query.Where(u => u.Checked == paginate.Checked.Value);
            }

            paginate.Paginate(query);
            return paginate;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var log = _logErrorRepository.GetById(id);
            if (log is null)
            {
                return NotFound();
            }

            if (log.Checked is false)
            {
                log.Checked = true;
                await _logErrorRepository.UpdateAsync(log);
            }

            return new ObjectResult(log);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var log = _logErrorRepository.GetById(id);
            if (log is null)
            {
                return NotFound();
            }

            await _logErrorRepository.RemoveAsync(log);
            return new NoContentResult();
        }
    }
}
