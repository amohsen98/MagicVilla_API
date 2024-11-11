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
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {

            //Return Json File key value pair 
            return Ok(VillaStore.VillaList);

        }

        [HttpGet("{id:int}")]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0) {
                return BadRequest();
            }
            //linq operation, deal with any kind of database, here is list
            var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
            if (villa == null) {

                return NotFound();
            }
            return Ok(villa);

        }

    }
}
