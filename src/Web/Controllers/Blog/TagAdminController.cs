using System.IO;
using System.Threading.Tasks;
using ApplicationCore.Data.Interfaces;
using ApplicationCore.Services.ImageServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Web.Extensions;
using Web.ViewModels.TagAdminViewModels;

namespace Web.Controllers.Blog
{
    [Authorize(Roles = "Admins")]
    public class TagAdminController : Controller
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly ITagRepository _tagRepository;
        private readonly IUploadImageService _uploadImageService;

        private readonly int _maxWidth;
        private readonly int _maxHeight;
        private readonly string _uploadTo;

        public TagAdminController(
            IHostingEnvironment env,
            IConfiguration configuration,
            ITagRepository tagRepository,
            IUploadImageService uploadImage)
        {
            _env = env;
            _configuration = configuration;
            _tagRepository = tagRepository;
            _uploadImageService = uploadImage;

            _uploadTo = Path.Combine("blog", "tags");
            _maxWidth = 450;
            _maxHeight = 250;
        }

        public IActionResult List()
        {
            var model = new ListViewModel();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_tagRepository.NameExists(model.Tag.Name))
                {
                    var message = $"Ya existe una etiqueta con el nombre {model.Tag.Name}";
                    ModelState.AddModelError("Tag.Name", message);
                    return View(model);
                }

                var result = await _uploadImageService.UploadAndResizeAsync(
                    model.Image, _uploadTo, _maxWidth, _maxHeight
                );
                if (result.Success)
                {
                    var filename = result.ReturnMessages["filename"];
                    model.Tag.Image = Path.Combine(_uploadTo, filename);
                    await _tagRepository.CreateAsync(model.Tag);
                    this.AddMessage("success", "Etiqueta creada con éxito");
                    return RedirectToAction(nameof(List));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var tag = _tagRepository.GetById(id);
            if (tag is null)
            {
                return NotFound();
            }

            var model = new EditViewModel
            {
                Tag = tag,
                OldImage = tag.Image
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tag = await _tagRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == model.Tag.Id);

                if (string.CompareOrdinal(tag.Name, model.Tag.Name) != 0)
                {
                    if (_tagRepository.NameExists(model.Tag.Name))
                    {
                        ModelState.AddModelError("Tag.Name", $"El nombre {model.Tag.Name} ya existe");
                        return View(model);
                    }
                }

                if (model.Image != null)
                {
                    var result = await _uploadImageService.UploadAndResizeAsync(
                        model.Image, _uploadTo, _maxWidth, _maxHeight
                    );
                    if (result.Success)
                    {
                        var filename = result.ReturnMessages["filename"];
                        model.Tag.Image = Path.Combine(_uploadTo, filename);
                        if (model.OldImage != model.Tag.Image)
                        {
                            var oldImageFullPath = _configuration["Images:Path"]
                                .Trim('/')
                                .Replace('/', Path.AltDirectorySeparatorChar);
                            oldImageFullPath = Path.Combine(_env.WebRootPath, oldImageFullPath, model.OldImage);

                            if (System.IO.File.Exists(oldImageFullPath))
                            {
                                System.IO.File.Delete(oldImageFullPath);
                            }
                        }
                    }
                }

                await _tagRepository.UpdateAsync(model.Tag);
                this.AddMessage("success", "Etiqueta modificada con éxito");
                return RedirectToAction(nameof(List));
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await _tagRepository.GetAll()
                .Include(a => a.Articles)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (model is null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag is null)
            {
                return NotFound();
            }

            await _tagRepository.RemoveAsync(tag);
            this.AddMessage("success", "Etiqueta eliminada con éxito");
            return RedirectToAction(nameof(List));
        }
    }
}
