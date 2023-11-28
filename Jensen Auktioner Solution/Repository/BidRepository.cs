using AutoMapper;
using Dapper;
using Jensen_Auktioner_Solution.Data;
using Jensen_Auktioner_Solution.Dto.Bid;
using Jensen_Auktioner_Solution.Model;
using Jensen_Auktioner_Solution.Service;
using System.Data;

namespace Jensen_Auktioner_Solution.Repository
{
    public class BidRepository:IBidService
    {
        private readonly DapperContext _dapperContext;
        private readonly IMapper _mapper;
        private readonly ILogger<BidRepository> _logger;
        private readonly IConfiguration _configuration;
        public BidRepository(
            DapperContext dapperContext,
            IMapper mapper,
            ILogger<BidRepository> logger,
            IConfiguration configuration
            )
        {
            _dapperContext = dapperContext;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
        }

        public async  Task<IEnumerable<Bid>> GetBidsForAuctionAsync(int auctionId)
        {
            try
            {
                using (var connection = _dapperContext.GetDbConnection())
                {
                    connection.Open();

                    // Execute the stored procedure
                    var bids = await connection.QueryAsync<Bid>(
                        "GetBidsForAuction", // Replace with your actual stored procedure name
                        new { AuctionId = auctionId },
                        commandType: CommandType.StoredProcedure
                    );

                    return bids;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetBidsForAuctionAsync: {ex.Message}");
                throw; // Rethrow the exception to the calling code
            }
        }

        public async Task<Bid> GetWinningBidAsync(int auctionId)
        {
            try
            {
                using (var connection = _dapperContext.GetDbConnection())
                {
                    connection.Open();

                    // Execute the stored procedure
                    var winningBid = await connection.QueryFirstOrDefaultAsync<Bid>(
                        "GetWinningBid", // Replace with your actual stored procedure name
                        new { AuctionId = auctionId },
                        commandType: CommandType.StoredProcedure
                    );

                    return winningBid;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetWinningBidAsync: {ex.Message}");
                throw; // Rethrow the exception to the calling code
            }
        }

        public async Task<Bid> PlaceBidAsync(int auctionId, BidDto bidDto)
        {
            try
            {
                using (var connection = _dapperContext.GetDbConnection())
                {
                    connection.Open();

                    // Execute the stored procedure
                    await connection.ExecuteAsync(
                        "PlaceBid", // Replace with your actual stored procedure name
                        new
                        {
                            AuctionId = auctionId,
                            UserId = bidDto.UserId,
                            Price = bidDto.Price
                        },
                        commandType: CommandType.StoredProcedure
                    );

                    // Retrieve and return the placed bid
                    return await GetWinningBidAsync(auctionId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in PlaceBidAsync: {ex.Message}");
                throw; // Rethrow the exception to the calling code
            }
        }

        public async Task RemoveBidAsync(int bidId)
        {
            try
            {
                using (var connection = _dapperContext.GetDbConnection())
                {
                    connection.Open();

                    // Execute the stored procedure
                    await connection.ExecuteAsync(
                        "RemoveBid", // Replace with your actual stored procedure name
                        new { BidId = bidId },
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in RemoveBidAsync: {ex.Message}");
                throw; // Rethrow the exception to the calling code
            }
        }
    }
}
