using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MagicVilla_VillaApi.Controllers
{
    //[Route("api/[controller]")]

    [Route("api/VillaApi")]
    [ApiController] //for validation
    public class VillaApiController : ControllerBase

    {
        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {

            //Return Json File key value pair 
            return Ok(VillaStore.VillaList);

        }

        [HttpGet("{id:int}", Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //not found 
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //Bad request
        //[ProducesResponseType(200)]
        //[ProducesResponseType(404)] //not found 
        //[ProducesResponseType(400)] //Bad request
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(404)] //not found 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] //Bad request
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO) {

            //if (!ModelState.IsValid) { 

            //    return BadRequest(ModelState);
            //}
            //for customer validation
            if (VillaStore.VillaList.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa Already Exists!!");
                return BadRequest(ModelState);
            }

            if (villaDTO == null) {

                return BadRequest(villaDTO);
            }
            if(villaDTO.Id > 0)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villaDTO.Id = VillaStore.VillaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            VillaStore.VillaList.Add(villaDTO);
            
            //invoking another end point
            return CreatedAtRoute("GetVilla", new {id = villaDTO.Id}  ,villaDTO);
        
        
        }    

    }
}
