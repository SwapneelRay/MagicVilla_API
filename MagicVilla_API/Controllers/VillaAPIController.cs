using MagicVilla_API.Models;
using MagicVilla_API.Models.Data;
using MagicVilla_API.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace MagicVilla_API.Controllers
{
    // [Route("api/[contoller]")] for directly using contoller name in this case VillaAPI
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _logger;
        private readonly ApplicationDBContext _db;

        public VillaAPIController(ILogger<VillaAPIController> logger, ApplicationDBContext db)
        {
            _logger = logger;
            _db = db;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.LogInformation("getting all villas");
            return Ok(_db.Villas);
        }


        [HttpGet("{id:int}", Name = "GetVilla")]
        //[ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (villa == null) {
                return NotFound();
            }
            else
            {
                return Ok(villa);
            }

        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO)
        {
            /*if(villaDTO == null)
            {
                BadRequest();
            }*/

            if (_db.Villas.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already exist");
                return BadRequest(ModelState);
            }

            if (villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else {
                Villa model = new ()
                    {
                    Id = villaDTO.Id,
                    Details = villaDTO.Details,
                    Amenity = villaDTO.Amenity,
                    ImageUrl = villaDTO.ImageUrl,
                    Name = villaDTO.Name,
                    Occupancy = villaDTO.Occupancy,
                    Rate = villaDTO.Rate,
                    Sqft = villaDTO.Sqft,
                };
                _db.Villas.Add(model);
                _db.SaveChanges();
                return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
            }
        }


        [HttpDelete("{id:int}", Name= "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (villa==null)
            {
                return NoContent();
            }
            else
            {
                _db.Villas.Remove(villa);
                _db.SaveChanges();
                return Ok(villa);
            }
        }

        [HttpPut("{id:int}", Name= "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDTO> UpdateVilla(int id, [FromBody]VillaDTO villaDTO)
        {
            if (villaDTO==null || id != villaDTO.Id)
            {
                return BadRequest();
            }
            else
            {
                Villa model = new()
                {
                    Id = villaDTO.Id,
                    Details = villaDTO.Details,
                    Amenity = villaDTO.Amenity,
                    ImageUrl = villaDTO.ImageUrl,
                    Name = villaDTO.Name,
                    Occupancy = villaDTO.Occupancy,
                    Rate = villaDTO.Rate,
                    Sqft = villaDTO.Sqft,
                };
                _db.Update(model);
                _db.SaveChanges();
                return Ok();
            }
        }
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            if( patchDTO==null || id == 0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.AsNoTracking().FirstOrDefault(u => u.Id == id);

            VillaDTO villaDTO = new()
            {
                Id = villa.Id,
                Details = villa.Details,
                Amenity = villa.Amenity,
                ImageUrl = villa.ImageUrl,
                Name = villa.Name,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft,
            };

            if (villa == null)
            {
                return NoContent();
            }
            else
            {
                patchDTO.ApplyTo(villaDTO, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    Villa model = new()
                    {
                        Id = villaDTO.Id,
                        Details = villaDTO.Details,
                        Amenity = villaDTO.Amenity,
                        ImageUrl = villaDTO.ImageUrl,
                        Name = villaDTO.Name,
                        Occupancy = villaDTO.Occupancy,
                        Rate = villaDTO.Rate,
                        Sqft = villaDTO.Sqft,
                        
                    };
                    _db.Update(model);
                    _db.SaveChanges();
                    return Ok();
                }
                
            }
        }
        
    }
}
