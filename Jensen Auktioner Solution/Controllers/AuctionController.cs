using Jensen_Auktioner_Solution.Dto.Auction;
using Jensen_Auktioner_Solution.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jensen_Auktioner_Solution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionService _auctionService;
        private readonly ILogger<AuctionController> _logger;
        public AuctionController(
            IAuctionService auctionService,
            ILogger<AuctionController> logger
            )
        {
            _auctionService = auctionService;
            _logger = logger;
        }
        [HttpPost("CreateAuction")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> CreateAuction([FromBody] AuctionDto auctionDto)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized("User not authenticated.");
                }

                var createdAuction = await _auctionService.CreateAuctionAsync(userId, auctionDto); 

                if (createdAuction != null)
                {
                    return Ok("Auction Created Successfully!");
                }
                else
                {
                    return BadRequest("Auction creation failed due to validation errors.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CreateAuction: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("GetAllAuctions")]
        
        public async Task<IActionResult> GetAllAuctions()
        {

            try
            {
                var auctions = await _auctionService.GetAllAuctionsAsync();
                return Ok(auctions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllAuctions: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("SpeceficAuction/{auctionid}")]
        public async Task<IActionResult> GetAuctionDetails(int auctionid)
        {
            try
            {
                var auction = await _auctionService.GetAuctionDetailsAsync(auctionid);

                if (auction != null)
                {
                    return Ok(auction);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAuctionDetails: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete("{auctionid}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> RemoveAuction(int auctionid)
        {
            try
            {
                await _auctionService.RemoveAuctionAsync(auctionid);
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in RemoveAuction: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("search")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> SearchAuctions([FromQuery] string query)
        {
            try
            {
                var auctions = await _auctionService.SearchAuctionsAsync(query);
                return Ok(auctions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in SearchAuctions: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPut("{auctionId}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> UpdateAuction(int auctionId, [FromBody] AuctionDto auctionDto)
        {
            try
            {
                var updatedAuction = await _auctionService.UpdateAuctionAsync(auctionId, auctionDto);

                if (updatedAuction != null)
                {
                    return Ok(updatedAuction);
                }
                else
                {
                    // Depending on the nature of validation errors, you might want to provide more specific details
                    return BadRequest("Auction update failed due to validation errors or existing bids.");
                }
            }
            catch (ApplicationException appEx)
            {
                // Handle application-specific exceptions (e.g., custom validation)
                _logger.LogError($"Error in UpdateAuction: {appEx.Message}");
                return BadRequest(appEx.Message);
            }
            catch (Exception ex)
            {
                // Log unexpected errors
                _logger.LogError($"Error in UpdateAuction: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


    }
}
