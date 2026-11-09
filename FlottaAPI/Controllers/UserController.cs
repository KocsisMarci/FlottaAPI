using FlottaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlottaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private FlottaContext _context;

        public UserController(FlottaContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllUser")]

        public IActionResult GetAllUserr()
        {
            try
            {
                return Ok(_context.Users.ToList());
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Hiba történt a kérés során, bővebben: {ex.Message}");
                //return BadRequest is lehetne persze.
            }
        }

        [HttpGet("GetAsyncUserById/{id}")]

        public async Task<IActionResult> GetAsyncUserById(int id)
        {
            try
            {

                var  userek= await _context.Users.ToListAsync();
                var user = userek.FirstOrDefault(sofor => sofor.Id == id);

                if (user == null)
                {
                    return NotFound("Nincs ilyen id-jú sofőr");
                }

                else
                {
                    return Ok(user);

                }
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Hiba történt a kérés során, bővebben: {ex.Message}");
            }
        }


        [HttpPost("PostUser")]

        public IActionResult PostUser(User user)
        {
            try
            {
                _context.Users.Add(user);

                _context.SaveChanges();

                return Ok("Sikeres post");

            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Hiba történt a kérés során, bővebben: {ex.Message}");

            }
        }

        [HttpPut("PutUser")]

        public IActionResult PutUser(User user)
        {

            try
            {
                if (user is null)
                {
                    return BadRequest("Missing data");
                }

                User usere = _context.Users.FirstOrDefault(us => us.Id == us.Id);


                if (usere is null)
                {
                    return NotFound("Nincs ilyen id-jú user");

                }

                else
                {
                    _context.ChangeTracker.Clear();
                    _context.Update(user);
                    _context.SaveChanges();
                    return Ok("The user data modified");

                }


            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Hiba történt a kérés során, bővebben: {ex.Message}");

            }
        }



        [HttpDelete("DeleteUser/{id}")]

        public IActionResult DeleteUser(int id)
        {

            try
            {
                var toDelete = _context.Users.ToList().FirstOrDefault(u => u.Id == id);

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


    }
}
