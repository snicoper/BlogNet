using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Data;
using ApplicationCore.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels.AccountAdminViewModels;

namespace Web.Api
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admins")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(
            ApplicationDbContext dbContext,
            UserManager<AppUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet]
        public IQueryable<AppUser> GetAll()
        {
            return _dbContext.Users;
        }

        [HttpGet("paginate")]
        public ListViewModel GetAll(ListViewModel paginate)
        {
            var query = GetAll();

            // Filtros
            if (!string.IsNullOrEmpty(paginate.UserName))
            {
                query = query.Where(u => u.UserName.ToLower().Contains(paginate.UserName.ToLower()));
            }

            if (!string.IsNullOrEmpty(paginate.Email))
            {
                query = query.Where(u => u.Email.ToLower().Contains(paginate.Email.ToLower()));
            }

            if (!string.IsNullOrEmpty(paginate.PhoneNumber))
            {
                query = query.Where(u => u.PhoneNumber.Contains(paginate.PhoneNumber));
            }

            if (DateTime.TryParse(paginate.CreateAt, out var createAt))
            {
                query = query.Where(u => u.CreateAt.ToShortDateString() == createAt.ToShortDateString());
            }

            if (DateTime.TryParse(paginate.LastLogin, out var lastLogin))
            {
                query = query.Where(u => u.LastLogin.ToShortDateString() == lastLogin.ToShortDateString());
            }

            switch (paginate.Active)
            {
                case AccountStatus.Active:
                    query = query.Where(u => u.Active);
                    break;
                case AccountStatus.Disable:
                    query = query.Where(u => u.Active == false);
                    break;
                case AccountStatus.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (paginate.EmailConfirmed)
            {
                case EmailConfirmed.Yes:
                    query = query.Where(u => u.EmailConfirmed);
                    break;
                case EmailConfirmed.No:
                    query = query.Where(u => u.EmailConfirmed == false);
                    break;
                case EmailConfirmed.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            paginate.Paginate(query);
            return paginate;
        }

        [HttpGet("search/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return NotFound();
            }

            return new ObjectResult(user);
        }

        [HttpGet("search/username")]
        public IActionResult FilterByUserName(string term)
        {
            var users = GetAll()
                .Where(u => u.UserName.ToLower().Contains(term.ToLower()))
                .Select(u => new
                {
                    u.Id,
                    Text = u.UserName
                });
            return new ObjectResult(users);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] AppUser user)
        {
            if (user is null || user.Id != id)
            {
                return NotFound();
            }

            await _userManager.UpdateAsync(user);
            return new NoContentResult();
        }
    }
}
