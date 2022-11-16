namespace authorisation.api.Classes;

public class UserModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
   
    public string? LineOne { get; set; }
    public string? LineTwo { get; set; }
    public string? LineThree { get; set; }
    public string? Postcode { get; set; }
    public string? City { get; set; }
    public string? County { get; set; }
    public string? Country { get; set; }
}