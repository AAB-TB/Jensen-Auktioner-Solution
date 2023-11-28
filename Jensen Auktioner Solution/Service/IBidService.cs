using Jensen_Auktioner_Solution.Dto.Auction;
using Jensen_Auktioner_Solution.Dto.Bid;
using Jensen_Auktioner_Solution.Model;

namespace Jensen_Auktioner_Solution.Service
{
    public interface IBidService
    {
        Task<IEnumerable<BidDetailsDto>> GetBidsForAuctionAsync(int auctionId);

        Task<bool> PlaceBidAsync(int auctionId, decimal price, int userid);

        Task RemoveBidAsync(int auctionId, int userId);

        Task<AuctionDetailsInfoDto> GetAuctionDetailsWithWinningBidAsync(int auctionId);
    }
}
