using AutoMapper;
using Dapper;
using Jensen_Auktioner_Solution.Data;
using Jensen_Auktioner_Solution.Dto.Auction;
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

        public async  Task<IEnumerable<BidDetailsDto>> GetBidsForAuctionAsync(int auctionId)
        {
            try
            {
                using (var connection = _dapperContext.GetDbConnection())
                {
                    connection.Open();

                    // Execute the stored procedure
                    var bids = await connection.QueryAsync<BidDetailsDto>(
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

        public async Task<AuctionDetailsInfoDto> GetAuctionDetailsWithWinningBidAsync(int auctionId)
        {
            try
            {
                using (var connection = _dapperContext.GetDbConnection())
                {
                    connection.Open();

                    var result = await connection.QueryMultipleAsync("GetAuctionDetailsWithWinningBid", new { AuctionId = auctionId }, commandType: CommandType.StoredProcedure);

                    // Read auction details
                    var auctionDetails = (await result.ReadAsync<AuctionDetailsInfoDto>()).FirstOrDefault();

                    if (auctionDetails != null)
                    {
                        // Check if the auction is open to read all bids
                        if (await result.ReadAsync<BidInfoDto>() is var bidInfoDtos && bidInfoDtos.Any())
                        {
                            // Auction is open, read all bids
                            auctionDetails.Bids = bidInfoDtos.ToList();
                        }
                        else
                        {
                            // Auction is closed, read only the highest bid
                            var highestBid = (await result.ReadAsync<BidInfoDto>()).FirstOrDefault();
                            auctionDetails.Bids = highestBid != null ? new List<BidInfoDto> { highestBid } : new List<BidInfoDto>();
                        }
                    }

                    return auctionDetails;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving auction details.");
                // You can rethrow the exception if needed or handle it according to your application's requirements.
                throw;
            }
        }

        public async Task<bool> PlaceBidAsync(int auctionId, decimal price, int userid )
        {
            using (var connection = _dapperContext.GetDbConnection())
            {
                try
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@AuctionId", auctionId, DbType.Int32);
                    parameters.Add("@UserId", userid, DbType.Int32);
                    parameters.Add("@Price", price, DbType.Decimal, precision: 18, scale: 2);

                    // Execute the stored procedure
                    await connection.ExecuteAsync("PlaceBid", parameters, commandType: CommandType.StoredProcedure);

                    // If the execution reaches here, the bid placement was successful
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error placing bid");
                    // Log the error or handle it based on your application's requirements
                    return false; // Return false to indicate that bid placement failed
                }
            }
        }

        public async Task RemoveBidAsync(int auctionId, int userId)
        {
            try
            {
                using (var connection = _dapperContext.GetDbConnection())
                {
                    connection.Open();

                    // Execute the stored procedure
                    await connection.ExecuteAsync(
                        "RemoveBid", // Replace with your actual stored procedure name
                        new { AuctionId = auctionId, UserId = userId },
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
