using TodoItems.Application.DTOs;
using TodoItems.Domain.Entities;

namespace TodoItems.Application.Mappings;

public static class ItemMappingProfile
{
    public static Item ToEntity(ItemDto dto)
    {
        return new Item(
            dto.Id,
            dto.Title,
            dto.Description,
            dto.Category);
    }
}
