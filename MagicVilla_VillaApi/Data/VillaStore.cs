using MagicVilla_VillaApi.Models.DTO;

namespace MagicVilla_VillaApi.Data
{
    public static class VillaStore
    {
        //Object Initializer 
        //static field doesn't have to be inisitiaited through an object, just the class name
        public static List<VillaDTO> VillaList = new List<VillaDTO>()
        
            {
                new VillaDTO { Id = 1, Name = "Pool View" },
                new VillaDTO { Id = 2, Name = "Beach View" }
            };
    }
}
