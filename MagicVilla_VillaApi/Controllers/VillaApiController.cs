﻿using AutoMapper;
using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.DTO;
using MagicVilla_VillaApi.Repository.IRepository;
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
        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapper;
        public VillaApiController(IVillaRepository dbVilla, IMapper mapper)
        {
            _dbVilla = dbVilla;  
                _mapper = mapper;   
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            //Return Json File key value pair 
            IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync(); 
            return Ok(_mapper.Map<List<VillaDTO>>(villaList));

        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //not found 
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //Bad request
        //[ProducesResponseType(200)]
        //[ProducesResponseType(404)] //not found 
        //[ProducesResponseType(400)] //Bad request
        public async Task<ActionResult<VillaDTO>> GetVillaAsync(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            //linq operation, deal with any kind of database, here is list
            var villa = await _dbVilla.GetAsync(u => u.Id == id);
            if (villa == null)
            {

                return NotFound();
            }
            return Ok(_mapper.Map<VillaDTO>(villa));

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(404)] //not found 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] //Bad request
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO createDTO)
        {

            //if (!ModelState.IsValid) { 

            //    return BadRequest(ModelState);
            //}
            //for customer validation
            if (await _dbVilla.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa Already Exists!!");
                return BadRequest(ModelState);
            }

            if (createDTO == null)
            {

                return BadRequest(createDTO);
            }
            //i want to insert the new record automatically , don't want to specify the id
            //if (villaDTO.Id > 0)
            //{
             
            //    return StatusCode(StatusCodes.Status500InternalServerError);
            //}
            //we are doing this because we are passing values from user to villadto obj not villa database
           Villa model = _mapper.Map<Villa>(createDTO);
           
            await _dbVilla.CreateAsync(model);
            await _dbVilla.SaveAsync();

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
            var villa = await _dbVilla.GetAsync(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            await _dbVilla.RemoveAsync(villa);
            await _dbVilla.SaveAsync();
            return NoContent();

        }
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
        {

            if (updateDTO == null || id != updateDTO.Id)
            {
                return BadRequest();
            }
         

           Villa model= _mapper.Map<Villa>(updateDTO);
            
            await _dbVilla.UpdateAsync(model);
            await _dbVilla.SaveAsync();
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
            var villa = await _dbVilla.GetAsync(u => u.Id == id, tracked:false);
            VillaUpdateDTO VillaDTO =_mapper.Map<VillaUpdateDTO>(villa);

            
            if (villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(VillaDTO, ModelState);
            Villa model = _mapper.Map<Villa>(VillaDTO);
       
            await _dbVilla.UpdateAsync(model);
           await _dbVilla.SaveAsync();

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}
