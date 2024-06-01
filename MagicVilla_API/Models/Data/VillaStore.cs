using MagicVilla_API.Models.Dto;

namespace MagicVilla_API.Models.Data
{
    public static class VillaStore
    {
       public static List<VillaDTO> Villalist = new List<VillaDTO>
            { new VillaDTO { Id = 1, Name = "Marriott" },
            new VillaDTO { Id = 2,Name ="Hyatt"}
            };
    }
}
