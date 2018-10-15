using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Core.PaginatorCore;
using ApplicationCore.Data.Entities.Blog;
using ApplicationCore.Data.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Web.ViewModels.ArticleAdminViewModels
{
    public enum ActiveChoices
    {
        None = 0,
        Active = 1,
        Desactive = 2
    }

    public class ListViewModel : PaginatorCore<Article>
    {
        public ListViewModel()
        {
            OrderBy = "CreateAt";

            ActiveList = new List<SelectListItem>
            {
                new SelectListItem { Text = "-- Articulo activo --", Value = ActiveChoices.None.ToString() },
                new SelectListItem { Text = "Activo", Value = ActiveChoices.Active.ToString() },
                new SelectListItem { Text = "Desactivo", Value = ActiveChoices.Desactive.ToString() }
            };
        }

        public ListViewModel(
            IArticleRepository articleRepository,
            ITagRepository tagRepository) : this()
        {
            var users = articleRepository
                .GetAll()
                .Include(a => a.Owner)
                .Select(u => new { Text = u.Owner.UserName, Value = u.Owner.Id })
                .Distinct();

            OwnerList = new List<SelectListItem> { new SelectListItem { Text = "-- Autor --", Value = "" } };
            foreach (var user in users)
            {
                OwnerList.Add(new SelectListItem { Text = user.Text, Value = user.Value });
            }

            var tags = tagRepository
                .GetAll()
                .Select(t => new { Text = t.Name, Value = t.Id });

            DefaultTagList = new List<SelectListItem> { new SelectListItem { Text = "-- Etiqueta --", Value = "0" } };
            foreach (var tag in tags)
            {
                DefaultTagList.Add(new SelectListItem { Text = tag.Text, Value = tag.Value.ToString() });
            }
        }

        public string Title { get; set; }

        public string Body { get; set; }

        public ActiveChoices Active { get; set; }

        public string CreateAt { get; set; }

        public string UpdateAt { get; set; }

        public string Owner { get; set; }

        public int DefaultTag { get; set; }

        public List<SelectListItem> ActiveList { get; set; }

        public List<SelectListItem> OwnerList { get; set; }

        public List<SelectListItem> DefaultTagList { get; set; }
    }
}
