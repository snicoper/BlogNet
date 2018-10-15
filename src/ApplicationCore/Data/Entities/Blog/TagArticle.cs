namespace ApplicationCore.Data.Entities.Blog
{
    public class TagArticle
    {
        public int TagId { get; set; }
        public Tag Tag { get; set; }

        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
