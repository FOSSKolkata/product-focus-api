using ProductFocus.Domain.Model;

namespace ProductFocus.Domain.Repositories
{
    public interface ITagRepository
    {
        void AddTag(Tag tag);
    }
}
