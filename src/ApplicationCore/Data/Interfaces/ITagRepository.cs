using ApplicationCore.Data.Entities.Blog;

namespace ApplicationCore.Data.Interfaces
{
    public interface ITagRepository : IRepository<Tag>
    {
        bool NameExists(string name);
    }
}
