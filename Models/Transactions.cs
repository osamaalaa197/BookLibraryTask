namespace BookLibrary.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int BookId { get; set; }
        public string UserId { get; set; }
        public Book Book { get; set; }
        public User User { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
