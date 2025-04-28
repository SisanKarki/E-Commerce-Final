using Book.Models;

namespace Book.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository1
    {
        void Save();
        void Update(Category obj);
    }
}