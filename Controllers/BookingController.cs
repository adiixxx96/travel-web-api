using Microsoft.AspNetCore.Mvc;
using TravelWebApi.Data;
using TravelWebApi.Models;

namespace videogames.Controllers;

[ApiController]
public class BookingController : ControllerBase
{
    private readonly DataContext _context;
    private static List<Booking> Bookings = new List<Booking> { };
    public BookingController(DataContext context)
    {
        _context = context;
    }


    //Obtengo todas las reservas de todos los usuarios
    [HttpGet]
    [Route("bookings")]
    public ActionResult<List<Booking>> Get()
    {
        return Ok(_context.Bookings);
    }

    //Obtengo los viajes reservados por un usuario
    [HttpGet]
    [Route("bookings/user/{UserId}")]
    public ActionResult<List<Booking>> GetByUserId(int UserId)
    {
        var Bookings = _context.Bookings.Where(x => x.UserId == UserId).ToList();
        var trips = new List<Trip>();
        foreach (var item in Bookings)
        {
            var trip = _context.Trips.Find(item.TripId);
            if (trip != null)
            {
                trips.Add(trip);
            }
        }
        return trips.Count == 0 ? NotFound("This user has no bookings yet") : Ok(trips);
    }

    //Obtener los usuarios que han reservado algún viaje
    [HttpGet]
    [Route("bookings/trip/{TripId}")]
    public ActionResult<List<Booking>> GetByTripId(int TripId)
    {
        var Bookings = _context.Bookings.Where(x => x.TripId == TripId).ToList();
        var users = new List<User>();
        foreach (var item in Bookings)
        {
            var user = _context.Users.Find(item.UserId);
            if (user != null)
            {
                users.Add(user);
            }
        }
        return users.Count == 0 ? NotFound("El viaje no ha sido comprado por ningún usuario") : Ok(users);
    }

    //Resevar un viaje
    [HttpPost]
    [Route("bookings/{UserId}/{TripId}")]
    public ActionResult NewBooking(int UserId, int TripId)
    {
        var existingBooking = _context.Bookings.SingleOrDefault(x => x.UserId == UserId && x.TripId == TripId);
        if (existingBooking != null)
        {
            return Conflict("You have already booked this trip. Try with another one.");
        }
        else
        {

            Booking booking = new Booking();
            var trip = _context.Trips.Find(TripId);
            if (trip.AvailableSpots > 0)
            {
                trip.AvailableSpots = trip.AvailableSpots - 1;
            }
            if (trip.AvailableSpots == 0)
            {
                trip.IsFull = true;
            }
            _context.SaveChanges();
            booking.UserId = UserId;
            booking.TripId = TripId;
            booking.BookingDate = DateTime.Now;
            booking.FinalPrice = trip.Price;
            _context.Bookings.Add(booking);
            _context.SaveChanges();
            var resourceUrl = Request.Path.ToString() + "/" + booking.BookingId;
            return Created(resourceUrl, booking);
        }
    }

    //Un usuario cancela una reserva
    [HttpDelete]
    [Route("bookings/{UserId}/{TripId}")]
    public ActionResult DeleteBooking(int UserId, int TripId)
    {
        var existingBooking = _context.Bookings.SingleOrDefault(x => x.UserId == UserId && x.TripId == TripId);
        if (existingBooking == null)
        {
            return NotFound("Booking does not exist");
        }
        else
        {
            var trip = _context.Trips.Find(existingBooking.TripId);
            trip.AvailableSpots = trip.AvailableSpots + 1;
            if (trip.AvailableSpots > 0)
            {
                trip.IsFull = false;
            }
            _context.SaveChanges();

            _context.Bookings.Remove(existingBooking);
            _context.SaveChanges();
            return NoContent();
        }
    }
}