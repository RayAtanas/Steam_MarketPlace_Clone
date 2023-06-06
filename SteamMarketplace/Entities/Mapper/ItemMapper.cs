using AutoMapper;
using SteamMarketplace.Entities;
using SteamMarketplace.Entities.DTO;

namespace SteamMarketplace.Entities.Mapper
{
    public class ItemMapper : Profile
    {
        public ItemMapper()
        {
            CreateMap<Item, ItemDTO>();
            CreateMap<ItemDTO, Item>();
        }
    }
}
