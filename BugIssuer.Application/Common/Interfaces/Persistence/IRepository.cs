using System;
namespace BugIssuer.Application.Common.Interfaces.Persistence;

public interface IRepository
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

