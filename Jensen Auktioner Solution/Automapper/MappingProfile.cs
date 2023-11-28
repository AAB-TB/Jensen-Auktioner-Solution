using AutoMapper;
using Jensen_Auktioner_Solution.Dto.Auction;
using Jensen_Auktioner_Solution.Dto.Bid;
using Jensen_Auktioner_Solution.Dto.Role;
using Jensen_Auktioner_Solution.Dto.User;
using Jensen_Auktioner_Solution.Model;
using System.Data;

namespace Jensen_Auktioner_Solution.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //mapper.Map<DestinationType>(sourceObject)

            // Model to DTO mappings
            CreateMap<User, UserDto>();  //<source,destination>
            CreateMap<User, UserInfoDto>();
            CreateMap<Role, RoleDto>();
            CreateMap<Auction, AuctionDto>();
            CreateMap<Bid, BidDto>();
            CreateMap<Bid, BidInfoDto>();
            CreateMap<Auction, AuctionDetailsInfoDto>();
            CreateMap<Auction, AuctionInfoDto>().ReverseMap();

        }
    }
}
