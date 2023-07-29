using BookLibrary.Models;

namespace BookLibrary.Repository.BookRepository
{
    public interface IBookRepository : IRepo<Book>
    {
        void Additem(Book name);


        void Delete(int id);


        List<Book> GetAll();


        Book GetByID(int id);
        List<Book> SearchBook(string search);
    }
}
