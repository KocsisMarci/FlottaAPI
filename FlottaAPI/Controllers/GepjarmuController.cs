using FlottaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlottaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GepjarmuController : ControllerBase
    {

        private FlottaContext _context;

        public GepjarmuController(FlottaContext context) {
            _context = context;
        }

        /*
         Dependency injection ki kéne vitelezni.
         */


        //GetAll



        [HttpGet("GetAll")]
        public IActionResult GetAll() {
            try
            {
                var jarmuvek = _context.Gepjarmus.ToList();

                return Ok(jarmuvek);
            }

            catch (Exception ex) {
                return StatusCode(500, $"Hiba történt a kérés során, bővebben: {ex.Message}");
            }
        }




        //GetByRendszam


        [HttpGet("GetByRendszam/{rendszam}")]

        public IActionResult GetByRendszam(string rendszam)
        {
            try
            {

                var eredmeny = _context.Gepjarmus.FirstOrDefault(jarmu => jarmu.Rendszam == rendszam);

                if (eredmeny is null)
                {
                    return NotFound("Nincs ilyen rendszámú jármű");
                }
                else
                {
                    return Ok(eredmeny);
                }
                }


            catch (Exception ex) {
                return StatusCode(500, $"Hiba történt a kérés során, bővebben: {ex.Message}");
            }
        }

        //Fel kell vinni járművet


        [HttpPost("UjGepjarmu")]

        public IActionResult UjGepjarmu(Gepjarmu gepjarmu)
        {
            try
            {

                if (gepjarmu == null) {
                    return BadRequest("Hiányzó adat(ok).");
                }
                _context.Gepjarmus.Add(gepjarmu);

                _context.SaveChanges();

                return Ok("Sikeres gépjármű felvitel");
            }

            catch (Exception ex) {
                return StatusCode(500, $"Hiba történt a kérés során, bővebben: {ex.Message}");
            }
        }


        //Adott azonosítójú járművet ki vezette

        [HttpGet("GepJarmuSofor/{id}")]


        public IActionResult GepJarmuSofor(int id)
        {
            try
            {
                /*
                var vanE = _context.Kikuldottjarmus.Where(jarmu => jarmu.Id == id);

                if (vanE is null) {
                    return StatusCode(404, "Nem létező id");
                }*/

                if (_context.Gepjarmus.FirstOrDefault(g => g.Id == id) == null) {
                    return NotFound("Hiányzó gépjármű!");
                }

                List<Sofor> result = _context.Kikuldottjarmus.
                    Include(k => k.SoforNavigation).
                    Include(k => k.Gepjarmu).
                     Where(k => k.GepjarmuId == id).Select(k => k.SoforNavigation).ToList();


                if (result.Count == 0) {
                    return StatusCode(400, "Nincsen jármű");
                }
                return Ok(result);
            }


            catch (Exception ex) {
                return StatusCode(500, $"Hiba történt a kérés során, bővebben: {ex.Message}");
            }
        }

        //Törlés id alapon


        [HttpDelete("DeleteJarmu/{id}")]


        public IActionResult DeleteJarmu(int id)
        {
            try
            {
                var result = _context.Gepjarmus.FirstOrDefault(jarmu => jarmu.Id ==id);

                _context.ChangeTracker.Clear();

                if (result == null) {
                    return NotFound("Nincs ilyen id-jú jármű");
                }

                _context.Gepjarmus.Remove(result);
                /*
                 Vagy
                _context.Gepjarmus.Remove(new GepJarmu{Id=id});
                 */
                _context.SaveChanges();

                return Ok("Sikeres törlés történt");

            }

            catch (Exception ex) {
                return StatusCode(500, $"Hiba történt a kérés során {ex.Message}");
            }
        }
     

    }
}
