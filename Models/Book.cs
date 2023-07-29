using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibrary.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Descrption { get; set; }
        public string Author { get; set; }
        public string Image { get; set; }
        public int Copies_Of_Book { get; set; }
        public ICollection<Transaction> Transactions { get; set; }


    }
}
