using MagicVilla_VillaApi.Models.DTO;

namespace MagicVilla_VillaApi.Data
{
    public static class VillaStore
    {
        //Object Initializer 
        public static List<VillaDTO> VillaList = new List<VillaDTO>()
        
            {
                new VillaDTO { Id = 1, Name = "Pool View" },
                new VillaDTO { Id = 2, Name = "Beach View" }
            };
    }
}
