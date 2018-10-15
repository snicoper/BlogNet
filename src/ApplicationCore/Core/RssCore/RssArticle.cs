using System;

namespace ApplicationCore.Core.RssCore
{
    public class RssArticle : IRssItem
    {
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
        public string Link { get; set; }
        public string Permalink { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
