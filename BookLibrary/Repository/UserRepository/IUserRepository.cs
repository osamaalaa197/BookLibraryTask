using BookLibrary.Models;

namespace BookLibrary.Repository.BorrowerRepository
{
    public interface IUserRepository : IRepo<User>
    {
        void Additem(User name);


        void Delete(string id);


        List<User> GetAll();


        User GetByID(string id);
        List<Book> GetUserWihtBook(string userId);
    }
}
