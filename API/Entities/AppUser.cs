namespace API.Entities;


// AppUser entity class representing a user in the application
public class AppUser
{

    // Entitiy class properties that map to the database columns

    // Id property as primary key
    public string Id { get; set; } = Guid.NewGuid().ToString();

    // DisplayName property, display name of the user
    public required string DisplayName { get; set; }

    // Email property, email address of the user
    public required string Email { get; set; }


}