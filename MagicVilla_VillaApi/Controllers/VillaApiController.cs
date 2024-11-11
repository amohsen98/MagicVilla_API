using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MagicVilla_VillaApi.Controllers
{
    //[Route("api/[controller]")]

    [Route("api/VillaApi")]
    [ApiController]
    public class VillaApiController : ControllerBase

    {
        [HttpGet]
        public IEnumerable<VillaDTO> GetVillas()
        {
            //Return Json File key value pair 
            return VillaStore.VillaList;

        }

        [HttpGet("{id:int}")]
        public VillaDTO GetVilla(int id)
        {
            //linq operation, deal with any kind of database, here is list
            return VillaStore.VillaList.FirstOrDefault(u => u.Id == id);

        }

    }
}
