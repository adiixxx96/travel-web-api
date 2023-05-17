namespace TravelWebApi.Models;

public class User
{

    public int UserId { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public DateTime CreationDate { get; set; }
    public Boolean Role { get; set; }

    public User()
    {

    }
}