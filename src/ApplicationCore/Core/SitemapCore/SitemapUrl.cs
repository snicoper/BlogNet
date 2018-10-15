using System;

namespace ApplicationCore.Core.SitemapCore
{
    public enum ChangeFrequency
    {
        Always,
        Hourly,
        Daily,
        Weekly,
        Monthly,
        Yearly,
        Never
    }

    public class SitemapUrl
    {
        public string Url { get; set; }
        public DateTime? UpdateAt { get; set; }
        public ChangeFrequency ChangeFrequency { get; set; }
        public double Priority { get; set; }
    }
}
