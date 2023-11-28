namespace Jensen_Auktioner_Solution.Dto.Bid
{
    public class BidDetailsDto
    {
        
        public int AuctionId { get; set; }
        public string AuctionTitle { get; set; }
        public int BidderId { get; set; }
        public string BidderName { get; set; }
        public decimal Price { get; set; }
    }
}
