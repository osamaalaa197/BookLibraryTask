using Microsoft.AspNetCore.Identity;

namespace BookLibrary.Models
{
    public class User : IdentityUser
    {
        public int Age { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
