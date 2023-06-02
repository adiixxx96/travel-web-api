using Microsoft.AspNetCore.Mvc;
using TravelWebApi.Data;
using TravelWebApi.Models;
using TravelWebApi.Utils;

namespace TravelWebApi.Controllers;

[ApiController]
public class TripController : ControllerBase
{
    private readonly DataContext _context;
    private static List<Trip> Trips = new List<Trip> { };
    public TripController(DataContext context)
    {
        _context = context;
    }
    
    //Obtengo todos los viajes con parámetros de búsqueda y de ordenación
    [HttpGet]
    [Route("trips")]
    public async Task<ActionResult<GenericFilter<Trip>>> Get(string search, string order = "TripId", string orderType = "ASC")
    {
        List<Trip> _Trips;
        GenericFilter<Trip> _FilterTrip;

        // Obtener todos los viajes
        _Trips = _context.Trips.ToList();

        // Filtrar viajes por el texto de búsqueda
       if (!string.IsNullOrEmpty(search))
        {
            foreach (var item in search.ToLower().Split(new char[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries))
            {
                _Trips = _context.Trips.Where(x => x.Destination.ToLower().Contains(item)).ToList();
            }

        }

        // Ordenar viajes según el campo
        switch (order)
        {
            case "TripId":
                if (orderType.ToLower() == "desc")
                    _Trips = _Trips.OrderByDescending(x => x.TripId).ToList();
                else if (orderType.ToLower() == "asc")
                    _Trips = _Trips.OrderBy(x => x.TripId).ToList();
                break;

            case "Destination":
                if (orderType.ToLower() == "desc")
                    _Trips = _Trips.OrderByDescending(x => x.Destination).ToList();
                else if (orderType.ToLower() == "asc")
                    _Trips = _Trips.OrderBy(x => x.Destination).ToList();
                break;

            case "StartDate":
                if (orderType.ToLower() == "desc")
                    _Trips = _Trips.OrderByDescending(x => x.StartDate).ToList();
                else if (orderType.ToLower() == "asc")
                    _Trips = _Trips.OrderBy(x => x.StartDate).ToList();
                break;

            case "Price":
                if (orderType.ToLower() == "desc")
                    _Trips = _Trips.OrderByDescending(x => x.Price).ToList();
                else if (orderType.ToLower() == "asc")
                    _Trips = _Trips.OrderBy(x => x.Price).ToList();
                break;
            
            default:
                if (orderType.ToLower() == "desc")
                    _Trips = _Trips.OrderByDescending(x => x.TripId).ToList();
                else if (orderType.ToLower() == "asc")
                    _Trips = _Trips.OrderBy(x => x.TripId).ToList();
                break;
        };
         _FilterTrip = new GenericFilter<Trip>() {
            CurrentSearch = search,
            CurrentOrder = order,
            CurrentOrderType = orderType,
            Result = _Trips
         };

         return _FilterTrip;
    }

    //Obtener un viaje
    [HttpGet]
    [Route("trips/{TripId}")]
    public ActionResult<List<Trip>> GetTrip(int TripId)
    {
        var trip = _context.Trips.Find(TripId);
        return trip == null ? NotFound() : Ok(trip);
    }

    //Añadir un nuevo viaje
    [HttpPost]
    [Route("trips")]
    public ActionResult Add(Trip trip)
    {
        var existingTrip = _context.Trips.FirstOrDefault(x => x.TripId == trip.TripId);
        if (existingTrip != null)
        {
            return Conflict("This trip id is already used.");
        }
        else
        {
            _context.Trips.Add(trip);
            _context.SaveChanges();
            var resourceUrl = Request.Path.ToString() + "/" + trip.TripId;
            return Created(resourceUrl, trip);
        }
    }

    //Editar viaje
    [HttpPut]
    [Route("trips")]
    public ActionResult Edit(Trip trip)
    {
        var existingTrip = _context.Trips.Find(trip.TripId);
        if (existingTrip == null)
        {
            return NotFound("Trip does not exist");
        }
        else
        {
            existingTrip.Destination = trip.Destination;
            existingTrip.StartDate = trip.StartDate;
            existingTrip.EndDate = trip.EndDate;
            existingTrip.Price = trip.Price;
            existingTrip.AvailableSpots = trip.AvailableSpots;
            _context.SaveChanges();
            return Ok();
        }
    }

    //Borrar viaje
    [HttpDelete]
    [Route("trips/{TripId}")]
    public ActionResult<Trip> Delete(int TripId)
    {
        var existingTrip = _context.Trips.Find(TripId);
        if (existingTrip == null)
        {
            return NotFound("Trip does not exist");
        }
        else
        {
            //Borrar reservas del viaje
            var bookings = _context.Bookings.Where(x => x.TripId == existingTrip.TripId).ToList();
            if (bookings != null)
            {
                foreach (var booking in bookings)
                {
                    _context.Bookings.Remove(booking);
                    _context.SaveChanges();
                }
            }

            //Borrar viaje
            _context.Trips.Remove(existingTrip);
            _context.SaveChanges();
            return NoContent();
        }
    }
}