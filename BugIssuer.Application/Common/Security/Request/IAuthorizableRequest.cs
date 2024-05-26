using MediatR;

namespace BugIssuer.Application.Common.Security.Request;

public interface IAuthorizableRequest<T> : IRequest<T>
{
    string UserId { get; }
}

