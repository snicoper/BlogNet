using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.ViewModels.ArticleAdminViewModels
{
    public class CreateEditViewModel
    {
        public int Id { get; set; }

        [StringLength(256)]
        [Display(Name = "Titulo")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Title { get; set; }

        [StringLength(256)]
        [Display(Name = "Slug")]
        public string Slug { get; set; }

        [Display(Name = "Imagen en cabecera")]
        public IFormFile ImageHeader { get; set; }

        public string CurrentImageHeader { get; set; }

        [Display(Name = "Articulo")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Body { get; set; }

        [Display(Name = "Visitas")]
        public int Views { get; set; }

        [Display(Name = "Activo")]
        public bool Active { get; set; } = true;

        [Display(Name = "Likes")]
        public int Likes { get; set; }

        [Display(Name = "Fecha de creación")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm}")]
        public DateTime CreateAt { get; set; }

        [Display(Name = "Ultima modificación")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm}")]
        public DateTime UpdateAt { get; set; }

        [Display(Name = "Autor")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string OwnerId { get; set; }

        public string OwnerUserName { get; set; }

        [Display(Name = "Etiqueta principal")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int DefaultTagId { get; set; }

        [Display(Name = "Etiquetas")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int[] Tags { get; set; }

        [Display(Name = "Tabla de contenidos")]
        public bool TableOfContents { get; set; }

        public List<SelectListItem> TagList { get; } = new List<SelectListItem>();
    }
}
