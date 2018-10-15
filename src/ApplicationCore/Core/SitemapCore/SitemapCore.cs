using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ApplicationCore.Core.SitemapCore
{
    public class SitemapCore
    {
        private readonly XNamespace _xmlsn = "http://www.sitemaps.org/schemas/sitemap/0.9";
        private readonly ChangeFrequency _changeFrequency;
        private readonly double _priority;
        private readonly List<SitemapUrl> _urls;

        public SitemapCore()
        {
            _urls = new List<SitemapUrl>();
            _changeFrequency = ChangeFrequency.Never;
            _priority = 0.5;
        }

        public SitemapCore(ChangeFrequency changeFrequency, double priority)
            : this()
        {
            _changeFrequency = changeFrequency;
            _priority = priority;
        }

        public void AddUrl(string url, DateTime? updateAt = null)
        {
            _urls.Add(new SitemapUrl
            {
                Url = url,
                UpdateAt = updateAt,
                ChangeFrequency = _changeFrequency,
                Priority = _priority
            });
        }

        public XDocument Serialize()
        {
            var sitemap = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(_xmlsn + "urlset",
                    from item in _urls
                    select _createItemElement(item)
                ));

            return sitemap;
        }

        private XElement _createItemElement(SitemapUrl url)
        {
            var itemElement = new XElement(_xmlsn + "url", new XElement(_xmlsn + "loc", url.Url.ToLower()));
            if (url.UpdateAt.HasValue)
            {
                itemElement.Add(new XElement(_xmlsn + "lastmod", url.UpdateAt.Value.ToString("yyyy-MM-dd")));
            }

            itemElement.Add(new XElement(_xmlsn + "changefreq", url.ChangeFrequency.ToString().ToLower()));
            itemElement.Add(new XElement(_xmlsn + "priority", url.Priority));

            return itemElement;
        }
    }
}
