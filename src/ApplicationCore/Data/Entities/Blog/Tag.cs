using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Data.Entities.Blog
{
    public class Tag
    {
        public int Id { get; set; }

        [StringLength(256)]
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Name { get; set; }

        [StringLength(256)]
        [Display(Name = "Slug")]
        public string Slug { get; set; }

        [Display(Name = "Imagen")]
        public string Image { get; set; }

        [Display(Name = "Visitas")]
        public int Views { get; set; }

        public ICollection<Article> Articles { get; set; }

        public ICollection<TagArticle> TagArticles { get; set; }
    }
}
