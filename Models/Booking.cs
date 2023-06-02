namespace TravelWebApi.Models;

public class Booking
{
    public int BookingId { get; set; }

    public DateTime BookingDate { get; set; }

    public decimal FinalPrice { get; set; }

    public Boolean Paid { get; set; }

    public int UserId { get; set; }

    public int TripId { get; set; }


    public Booking()
    {

    }

}