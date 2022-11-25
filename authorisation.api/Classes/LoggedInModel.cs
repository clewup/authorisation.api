namespace authorisation.api.Classes;

public class LoggedInModel
{
    public string AccessToken { get; set; } = "";
    public UserModel User { get; set; }
}