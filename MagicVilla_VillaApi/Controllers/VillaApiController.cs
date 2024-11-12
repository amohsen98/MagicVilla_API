using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.DTO;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //not found 
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //Bad request
        //[ProducesResponseType(200)]
        //[ProducesResponseType(404)] //not found 
        //[ProducesResponseType(400)] //Bad request
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            //linq operation, deal with any kind of database, here is list
            var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {

                return NotFound();
            }
            return Ok(villa);

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(404)] //not found 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] //Bad request
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO)
        {

            //if (!ModelState.IsValid) { 

            //    return BadRequest(ModelState);
            //}
            //for customer validation
            if (VillaStore.VillaList.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa Already Exists!!");
                return BadRequest(ModelState);
            }

            if (villaDTO == null)
            {

                return BadRequest(villaDTO);
            }
            //i want to insert the new record automatically , don't want to specify the id
            if (villaDTO.Id > 0)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villaDTO.Id = VillaStore.VillaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            VillaStore.VillaList.Add(villaDTO);

            //invoking another end point
            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);


        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            VillaStore.VillaList.Remove(villa);
            return NoContent();

        }
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {

            if (villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest();
            }
            var Villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
            Villa.Name = villaDTO.Name;
            Villa.Occupancy = villaDTO.Occupancy;
            Villa.sqft = villaDTO.sqft;
            return NoContent();
        }
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var Villa = VillaStore.VillaList.FirstOrDefault(u => u.Id == id);
            if (Villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(Villa, ModelState);
            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }
            return NoContent();





        }
    }
}
