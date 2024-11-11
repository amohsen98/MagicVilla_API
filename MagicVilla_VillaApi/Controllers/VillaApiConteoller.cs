using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaApi.Controllers
{
    //[Route("api/[controller]")]

    [Route("api/VillaApi")]
    [ApiController]
    public class VillaApiConteoller : ControllerBase

    {
        [HttpGet]
        public IEnumerable<VillaDTO> GetVillas()
        {
            //Return Json File key value pair 
            return new List<VillaDTO>()
            {

                new VillaDTO { Id = 1, Name = "Pool View" },
                new VillaDTO { Id = 2, Name = "Beach View" }
            };

        }

    }
}
