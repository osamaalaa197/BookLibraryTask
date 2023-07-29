using BookLibrary.Context;
using BookLibrary.Models;

namespace BookLibrary.Repository.BookRepository
{
    public class BookRepository : IBookRepository
    {
        private readonly DataBaseContext _context;

        public BookRepository(DataBaseContext dataBaseContext)
        {
            _context = dataBaseContext;
        }

        public void Additem(Book book)
        {
            _context.Book.Add(book);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var book = GetByID(id);
            _context.Book.Remove(book);
            _context.SaveChanges();
        }


        public List<Book> GetAll()
        {
            return _context.Book.ToList();
        }


        public Book GetByID(int id)
        {
            var book = _context.Book.FirstOrDefault(p => p.Id == id);
            return book;
        }

        public List<Book> SearchBook(string search)
        {
            var book=_context.Book.Where(p=>p.Name==search).ToList();
            return book;
        }
    }
}
