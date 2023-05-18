namespace TravelWebApi.Models;

public class Booking
{
    public int BookingId { get; set; }

    public User User { get; set; }

    public Trip Trip { get; set; }

    public DateTime BookingDate { get; set; }

    public decimal FinalPrice { get; set; }

    public Boolean Paid { get; set; }

    public Booking()
    {

    }

}