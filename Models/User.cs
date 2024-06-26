namespace BuySell.Models;

public class User
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string[] Address { get; set; }

    public User()
    {
        Name = "";
        Phone = "";
        Address = new string[3]{ "", "", "" };
    }
}
