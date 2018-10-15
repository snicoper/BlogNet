using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ApplicationCore.Data.Entities.Blog;
using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Data.Identity
{
    public class AppUser : IdentityUser
    {
        [Display(Name = "Fecha de creación")]
        public DateTime CreateAt { get; set; }

        [Display(Name = "Fecha ultimo acceso")]
        public DateTime LastLogin { get; set; }

        [Required]
        [Display(Name = "Cuenta activa")]
        public bool Active { get; set; } = true;

        [StringLength(255)]
        [Display(Name = "Email sin confirmar")]
        public string TemporalEmailChange { get; set; }

        [Display(Name = "Imagen")]
        public string Photo { get; set; } = "accounts/profiles/user.png";

        public ICollection<Article> Articles { get; set; }

        public int? SubscribeArticleId { get; set; }
        public SubscribeArticle SubscribeArticle { get; set; }
    }
}
