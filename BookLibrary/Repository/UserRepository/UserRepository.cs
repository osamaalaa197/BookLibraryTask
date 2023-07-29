using BookLibrary.Context;
using BookLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookLibrary.Repository.BorrowerRepository
{
    public class UserRepository: IUserRepository
    {
        private readonly DataBaseContext _context;

        public UserRepository(DataBaseContext dataBaseContext)
        {
            _context = dataBaseContext;
        }

        public void Additem(User user)
        {
            _context.Add(user);
            _context.SaveChanges();
        }

        public void Delete(string id)
        {
            var Borrower = GetByID(id);
            _context.Users.Remove(Borrower);
            _context.SaveChanges();
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetByID(string id)
        {
            var user = _context.Users.FirstOrDefault(p => p.Id == id);
            return user;
        }




        public List<Book> GetUserWihtBook(string userId)
        {
            var borrowedBooks = _context.Transactions
            .Where(t => t.UserId == userId)
            .Include(t => t.Book)
            .Select(t => t.Book)
            .ToList();

            return borrowedBooks;
        }
    }
}
