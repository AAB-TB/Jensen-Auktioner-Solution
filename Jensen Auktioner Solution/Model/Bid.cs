namespace Jensen_Auktioner_Solution.Model
{
    public class Bid
    {
        public int BidId { get; set; }
        public decimal Price { get; set; }
        public int AuctionId { get; set; }
        public int UserId { get; set; }

        // Navigation property
        public Auction Auction { get; set; }

        // Navigation property
        public User User { get; set; }
    }
}
