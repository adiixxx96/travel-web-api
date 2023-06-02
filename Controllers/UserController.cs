using Microsoft.AspNetCore.Mvc;
using TravelWebApi.Data;
using TravelWebApi.Models;

namespace TravelWebApi.Controllers;

[ApiController]
public class UserController : ControllerBase {
    private readonly DataContext _context;
    private static List<User> Users = new List<User> { };

    public UserController(DataContext context) {
        _context = context;
    }

    //Obtengo todos los usuarios
    [HttpGet]
    [Route("users")]
    public ActionResult<List<User>> Get() {
        return Ok(_context.Users);
    }

    //Registrar un nuevo usuario
    [HttpPost]
    [Route("users")]
    public ActionResult Register(User user) {
        var existingUsername = _context.Users.FirstOrDefault(x => x.Username == user.Username);
        if (existingUsername != null) {
            return Conflict("Username already exists. Try with another one.");
        }
        else {
            _context.Users.Add(user);
            _context.SaveChanges();
            var resourceUrl = Request.Path.ToString() + "/" + user.UserId;
            return Created(resourceUrl, user);
        }
    }

    //Login usuario
    [HttpPost]
    [Route("login/{Username}/{Password}")]
    public ActionResult<User> Login(string Username, string Password) {
        User user = _context.Users.SingleOrDefault(x => x.Username == Username && x.Password == Password);
        return user == null ? NotFound(null) : Ok(user);
    }

    //Editar usuario
    [HttpPut]
    [Route("users")]
    public ActionResult Edit(User user) {
        var existingUser = _context.Users.Find(user.UserId);
        if (existingUser == null) {
            return NotFound("User does not exist");
        } else {
            existingUser.Name = user.Name;
            existingUser.Username = user.Username;
            existingUser.Password = user.Password;
            _context.SaveChanges();
            return Ok();
        }
    }

    //Borrar usuario
    [HttpDelete]
    [Route("users/{UserId}")]
    public ActionResult<User> Delete(int UserId) {
        var existingUser = _context.Users.Find(UserId);
        if (existingUser == null)
        {
            return NotFound("User does not exist");
        }
        else
        {
            //Borrar reservas del usuario
            var bookings = _context.Bookings.Where(x => x.UserId == existingUser.UserId).ToList();
            if (bookings != null)
            {
                foreach (var booking in bookings)
                {
                    _context.Bookings.Remove(booking);
                    _context.SaveChanges();
                }
            }

            //Borrar usuario
            _context.Users.Remove(existingUser);
            _context.SaveChanges();
            return NoContent();
        }
    }
}