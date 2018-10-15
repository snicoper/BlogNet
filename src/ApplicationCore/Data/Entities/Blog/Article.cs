using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ApplicationCore.Data.Identity;

namespace ApplicationCore.Data.Entities.Blog
{
    public class Article
    {
        public int Id { get; set; }

        [StringLength(256)]
        [Display(Name = "Titulo")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Title { get; set; }

        [StringLength(256)]
        [Display(Name = "Slug")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Slug { get; set; }

        [Display(Name = "Imagen en cabecera")]
        public string ImageHeader { get; set; }

        [Display(Name = "Articulo")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Body { get; set; }

        [Display(Name = "Visitas")]
        public int Views { get; set; }

        [Display(Name = "Likes")]
        public int Likes { get; set; }

        [Display(Name = "Activo")]
        public bool Active { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime CreateAt { get; set; }

        [Display(Name = "Ultima modificación")]
        public DateTime UpdateAt { get; set; }

        [Display(Name = "Owner")]
        public string OwnerId { get; set; }
        public AppUser Owner { get; set; }

        [Display(Name = "Etiqueta principal")]
        public int DefaultTagId { get; set; }
        public Tag DefaultTag { get; set; }

        [Display(Name = "Tabla de contenidos")]
        public bool TableOfContents { get; set; }

        public ICollection<TagArticle> TagArticles { get; set; }
    }
}
