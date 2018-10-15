using System;
using ApplicationCore.Data.Identity;

namespace ApplicationCore.Data.Entities.Blog
{
    public class SubscribeArticle
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public Guid Token { get; set; }
        public DateTime SubscribeAt { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
