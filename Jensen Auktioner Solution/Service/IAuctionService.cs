using Jensen_Auktioner_Solution.Dto.Auction;
using Jensen_Auktioner_Solution.Dto.Bid;
using Jensen_Auktioner_Solution.Model;
using System.Security.Cryptography;

namespace Jensen_Auktioner_Solution.Service
{
    public interface IAuctionService
    {
        Task<Auction> CreateAuctionAsync(int userid, AuctionDto auctionDto);

        Task<AuctionDetailsInfoDto> UpdateAuctionAsync(int auctionId, AuctionDto auctionDto);

        Task RemoveAuctionAsync(int auctionId);

        Task<IEnumerable<AuctionInfoDto>> GetAllAuctionsAsync();

        Task<AuctionDetailsInfoDto> GetAuctionDetailsAsync(int auctionId);

        Task<IEnumerable<Auction>> SearchAuctionsAsync(string searchQuery);

        
    }
}
