using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MagicVilla_VillaApi.Controllers
{
    //[Route("api/[controller]")]

    [Route("api/VillaApi")]
    [ApiController] //for validation
    public class VillaApiController : ControllerBase

    {
        private readonly ApplicationDbContext _context;
        public VillaApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            //Return Json File key value pair 
            return Ok(await _context.Villas.ToListAsync());

        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //not found 
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //Bad request
        //[ProducesResponseType(200)]
        //[ProducesResponseType(404)] //not found 
        //[ProducesResponseType(400)] //Bad request
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            //linq operation, deal with any kind of database, here is list
            var villa = await _context.Villas.FirstOrDefaultAsync(u => u.Id == id);
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
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO villaDTO)
        {

            //if (!ModelState.IsValid) { 

            //    return BadRequest(ModelState);
            //}
            //for customer validation
            if (await _context.Villas.FirstOrDefaultAsync(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa Already Exists!!");
                return BadRequest(ModelState);
            }

            if (villaDTO == null)
            {

                return BadRequest(villaDTO);
            }
            //i want to insert the new record automatically , don't want to specify the id
            //if (villaDTO.Id > 0)
            //{

            //    return StatusCode(StatusCodes.Status500InternalServerError);
            //}
            //we are doing this because we are passing values from user to villadto obj not villa database
            Villa model = new()
            {
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                Rate = villaDTO.Rate,   
                Occupancy = villaDTO.Occupancy, 
                Sqft        = villaDTO.sqft,
                ImageUrl = villaDTO.ImageUrl,   
                Amenity = villaDTO.Amenity, 


            };
            await _context.Villas.AddAsync(model);
            await _context.SaveChangesAsync();

            //invoking another end point
            return CreatedAtRoute("GetVilla", new { id = model.Id }, model);


        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = await _context.Villas.FirstOrDefaultAsync(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            _context.Villas.Remove(villa);
            await _context.SaveChangesAsync();
            return NoContent();

        }
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaDTO)
        {

            if (villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest();
            }
            //var Villa = _context.Villas.FirstOrDefault(v => v.Id == id);
            //Villa.Name = villaDTO.Name;
            //Villa.Occupancy = villaDTO.Occupancy;
            //Villa.Sqft = villaDTO.sqft;
            Villa model = new()
            {
                Id = villaDTO.Id,
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                Rate = villaDTO.Rate,
                Occupancy = villaDTO.Occupancy,
                Sqft = villaDTO.sqft,
                ImageUrl = villaDTO.ImageUrl,
                Amenity = villaDTO.Amenity,


            };
            _context.Villas.Update(model);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villa = await _context.Villas.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            VillaUpdateDTO villaDTO = new()
            {
                Id = villa.Id,
                Name = villa.Name,
                Details = villa.Details,
                Rate = villa.Rate,
                Occupancy = villa.Occupancy,
                sqft = villa.Sqft,
                ImageUrl = villa.ImageUrl,
                Amenity = villa.Amenity,


            };
            if (villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaDTO, ModelState);
            Villa model = new ()
            {
                Id = villaDTO.Id,
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                Rate = villaDTO.Rate,
                Occupancy = villaDTO.Occupancy,
                Sqft = villaDTO.sqft,
                ImageUrl = villaDTO.ImageUrl,
                Amenity = villaDTO.Amenity,


            };
            _context.Update(model);
           await _context.SaveChangesAsync();

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }


            return NoContent();





        }
    }
}
