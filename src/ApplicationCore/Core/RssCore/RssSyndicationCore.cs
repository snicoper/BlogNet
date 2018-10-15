using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ApplicationCore.Core.RssCore
{
    public class RssSyndicationCore
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }

        public ICollection<IRssItem> RssItems { get; set; } = new List<IRssItem>();

        public XDocument Serialize()
        {
            var document = new XDocument(new XElement("rss"));
            document.Root.Add(new XAttribute("version", "2.0"));

            var channel = new XElement("channel");
            channel.Add(new XElement("title", Title));
            channel.Add(new XElement("link", Link));
            channel.Add(new XElement("description", Description));
            document.Root.Add(channel);

            foreach (var item in RssItems)
            {
                var itemElement = new XElement("item");
                itemElement.Add(new XElement("title", item.Title));
                itemElement.Add(new XElement("link", item.Link));
                itemElement.Add(new XElement("description", item.Body));

                if (item.AuthorName != null)
                {
                    itemElement.Add(new XElement("author", $"{item.AuthorEmail} ({item.AuthorName})"));
                }

                if (!string.IsNullOrWhiteSpace(item.Permalink))
                {
                    itemElement.Add(new XElement("guid", item.Permalink));
                }

                var dateFormat = item.CreateAt.ToString("r");
                if (item.CreateAt != DateTime.MinValue)
                {
                    itemElement.Add(new XElement("pubDate", dateFormat));
                }

                channel.Add(itemElement);
            }

            return document;
        }
    }
}
