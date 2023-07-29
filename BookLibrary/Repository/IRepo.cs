using BookLibrary.Models;

namespace BookLibrary.Repository
{
    public interface IRepo<T>
    {
        void Additem(T name);
        List<T> GetAll();
    }
}
