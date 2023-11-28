using Jensen_Auktioner_Solution.Dto.Bid;
using Jensen_Auktioner_Solution.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jensen_Auktioner_Solution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly IBidService _bidService;
        private readonly ILogger<BidController> _logger;
        public BidController(
            IBidService bidService,
            ILogger<BidController> logger
            )
        {
            _bidService = bidService;
            _logger = logger;
        }
        [HttpGet("{auctionId}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetBidsForAuction(int auctionId)
        {
            try
            {
                var bids = await _bidService.GetBidsForAuctionAsync(auctionId);
                return Ok(bids);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetBidsForAuction: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("winning/{auctionId}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetWinningBid(int auctionId)
        {
            try
            {
                var winningBid = await _bidService.GetWinningBidAsync(auctionId);

                if (winningBid != null)
                {
                    return Ok(winningBid);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetWinningBid: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> PlaceBid(int auctionId, [FromBody] BidDto bidDto)
        {
            try
            {
                var placedBid = await _bidService.PlaceBidAsync(auctionId, bidDto);

                if (placedBid != null)
                {
                    return Ok(placedBid);
                }
                else
                {
                    return BadRequest("Bid placement failed due to validation errors.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in PlaceBid: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> RemoveBid(int id)
        {
            try
            {
                await _bidService.RemoveBidAsync(id);
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in RemoveBid: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
