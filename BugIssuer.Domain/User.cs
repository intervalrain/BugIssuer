namespace BugIssuer.Domain;

public class User
{
    public string UserId { get; }

    public string UserName { get; }

    public string Email { get; } = null!;

    public string Department { get; }

    internal User(string userId, string userName, string email, string department)
    {
        UserId = userId;
        UserName = userName;
        Email = email;
        Department = department;
    }

    public static User Create(string userId, string userName, string email, string department)
    {
        return new User(userId, userName, email, department);
    }
}

