namespace TravelWebApi.Models;

public class Trip
{

    public int TripId { get; set; }

    public string Destination { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal Price { get; set; }

    public int AvailableSpots { get; set; }

    public Boolean IsFull { get; set; }

    public string Image { get; set; }

    public Trip()
    {

    }
}