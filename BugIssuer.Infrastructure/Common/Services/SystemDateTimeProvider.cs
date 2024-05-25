using BugIssuer.Application.Common.Interfaces;

namespace BugIssuer.Infrastructure.Common.Services;

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTime Now => DateTime.Now; 
}

