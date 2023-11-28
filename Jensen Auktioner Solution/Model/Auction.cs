using System.Security.Cryptography;

namespace Jensen_Auktioner_Solution.Model
{
    public class Auction
    {
        public int AuctionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserId { get; set; }

        // Navigation property
        public User User { get; set; }

        // Collection navigation property for bids
        public ICollection<Bid> Bids { get; set; }
    }
}
