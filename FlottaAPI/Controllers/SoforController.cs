using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlottaAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace FlottaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoforController : ControllerBase
    {


        private FlottaContext _context;

        public SoforController(FlottaContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllSofor")]

        public IActionResult GetAllSofor()
        {
            try
            {
                return Ok(_context.Sofors.ToList());
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Hiba történt a kérés során, bővebben: {ex.Message}");
                //return BadRequest is lehetne persze.
            }
        }

        [HttpGet("GetAsyncSoforById/{id}")]

        public async Task<IActionResult> GetAsyncSoforById(int id)
        {
            try
            {

                var soforok = await _context.Sofors.ToListAsync();
                var sofor = soforok.FirstOrDefault(sofor => sofor.Id == id);

                if (sofor == null)
                {
                    return NotFound("Nincs ilyen id-jú sofőr");
                }

                else
                {
                    return Ok(sofor);

                }
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Hiba történt a kérés során, bővebben: {ex.Message}");
            }
        }


        [HttpPost("PostSofor")]

        public IActionResult PostSofor(Sofor sofor)
        {
            try
            {
                _context.Sofors.Add(sofor);

                _context.SaveChanges();

                return Ok("Sikeres post");

            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Hiba történt a kérés során, bővebben: {ex.Message}");

            }
        }

        [HttpPut("PutSofor")]

        public IActionResult PutSofor(Sofor sofor)
        {

            try
            {
                if (sofor is null) {
                    return BadRequest("Missing data");
                }

                Sofor soforunk = _context.Sofors.FirstOrDefault(sof => sof.Id==sofor.Id);


                if (soforunk is null) {
                    return NotFound("Nincs ilyen id-jú sofőr");

                }

                else
                {
                    _context.ChangeTracker.Clear();
                    _context.Update(sofor);
                    _context.SaveChanges();
                    return Ok("The driver data modified");

                }


            }

            catch (Exception ex) {
                return StatusCode(500, $"Hiba történt a kérés során, bővebben: {ex.Message}");

            }
        }



        [HttpDelete("DeleteSofor/{id}")]

        public IActionResult DeleteSofor(int id)
        {

            try
            {
                var toDelete = _context.Sofors.ToList().FirstOrDefault(sofor => sofor.Id == id);

                _context.ChangeTracker.Clear();
                if (toDelete != null)
                {
                    _context.Remove(toDelete);
                    _context.SaveChanges();
                    return Ok("Sikeres delete");
                }

                else
                {
                    return NotFound("Nincs ilyen id-jú sofőr");

                }


            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Hiba történt a kérés során, bővebben: {ex.Message}");

            }
        }

        [HttpGet("SoforJarmu/{id}")]

        public IActionResult GetSoforJarmu(int id)
        {
            try
            {
                Sofor vane = _context.Sofors.FirstOrDefault(sofor => sofor.Id == id);

                if (vane == null)
                {
                    return NotFound("Hiányzó soför!");
                }

                var result = _context.Kikuldottjarmus.
                    Where(j => j.SoforNavigation.Id == id).
                    Select(j => j.Gepjarmu).ToList();


                return Ok(result);

            }

            catch (Exception ex) {
                return BadRequest("Hiba !!!" + ex.Message);
            }
        }

    }
}