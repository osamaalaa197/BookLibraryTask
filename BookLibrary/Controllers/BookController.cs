using BookLibrary.Context;
using BookLibrary.Models;
using BookLibrary.Repository.BookRepository;
using BookLibrary.Repository.BorrowerRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository _BookRepository;
        private readonly IUserRepository _UserRepository;
        private readonly DataBaseContext _context;

        public BookController(IBookRepository bookRepository,IUserRepository UserRepository, DataBaseContext dataBaseContext)
        {
            _BookRepository = bookRepository;
            _UserRepository = UserRepository;
            _context = dataBaseContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAllBook()
        {
            var books = _BookRepository.GetAll();
            return View(books);
        }
        [Authorize]
        public IActionResult Details(int id)
        {
            var book = _BookRepository.GetByID(id);
            return View(book);
        }

        [HttpPost]
        public IActionResult Borrow(int BookID,string UserID)
        {
            var user = _UserRepository.GetByID(UserID);
            var book = _BookRepository.GetByID(BookID);
            if (book == null)
            {
                return BadRequest("Failed to borrow the book.");
            }
            else
            {
                int borrowedCopies = _context.Transactions.Count(t => t.UserId == UserID && t.ReturnDate == null);
                if (book.Copies_Of_Book==0)
                {
                    return BadRequest("You have already borrowed the maximum number of copies.");
                }
                book.Copies_Of_Book--;
                _context.Transactions.Add(new Models.Transaction
                {
                    BookId = BookID,
                    UserId= UserID
                });
                _context.SaveChanges();
                return RedirectToAction("GetAllBook", "Book");

            }
           
        }
        [HttpPost]
        public IActionResult Return(int BookID, string UserID)
        {
            var transaction = _context.Transactions.Where(p => p.UserId == UserID && p.BookId == BookID).FirstOrDefault();
            //var user = _UserRepository.GetByID(UserID);
            //var book = _BookRepository.GetByID(BookID);
            if (transaction == null)
            {
                return BadRequest("Failed to return the book.");

            }
            var book = _BookRepository.GetByID(transaction.BookId);
            if(book == null)
            {
                return BadRequest("Failed to return the book.");

            }

            book.Copies_Of_Book++;
            transaction.ReturnDate = DateTime.Now;
            _context.Transactions.Remove(transaction);
            _context.SaveChanges();
            return RedirectToAction("GetAllBook", "Book");

        }

        public IActionResult SearchBook(string search)
        {
            var book = _BookRepository.SearchBook(search);
            return View(book);
        }
    }
}
