
using Jensen_Auktioner_Solution.Dto.Bid;

namespace Jensen_Auktioner_Solution.Dto.Auction
{
    public class AuctionInfoDto
    {
        public int AuctionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserId { get; set; }

        // Navigation property
        public string AuctionCreator { get; set; }

       
    }
}
