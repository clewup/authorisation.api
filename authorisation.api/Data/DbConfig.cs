namespace authorisation.api.Data;

public class DbConfig
{
    public string ConnectionString = "mongodb+srv://clew:gr7vQS5DP3VGHiFN@clew-network.civ8lz0.mongodb.net/?retryWrites=true&w=majority";
    public string DatabaseName = "bug_tracker";
    public string UserCollectionName = "users";
    public string RoleCollectionName = "roles";
}