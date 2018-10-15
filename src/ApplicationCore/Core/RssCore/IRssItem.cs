using System;

namespace ApplicationCore.Core.RssCore
{
    public interface IRssItem
    {
        string AuthorName { get; set; }
        string AuthorEmail { get; set; }
        string Link { get; set; }
        string Permalink { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        string Body { get; set; }
        DateTime CreateAt { get; set; }
    }
}
