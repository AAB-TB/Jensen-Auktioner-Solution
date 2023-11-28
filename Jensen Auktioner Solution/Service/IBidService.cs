using Jensen_Auktioner_Solution.Dto.Bid;
using Jensen_Auktioner_Solution.Model;

namespace Jensen_Auktioner_Solution.Service
{
    public interface IBidService
    {
        Task<IEnumerable<Bid>> GetBidsForAuctionAsync(int auctionId);

        Task<Bid> PlaceBidAsync(int auctionId, BidDto bidDto);

        Task RemoveBidAsync(int bidId);

        Task<Bid> GetWinningBidAsync(int auctionId);
    }
}
