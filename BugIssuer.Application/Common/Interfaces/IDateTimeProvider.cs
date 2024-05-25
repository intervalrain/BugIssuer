namespace BugIssuer.Application.Common.Interfaces;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
    public DateTime Now { get; }
}

