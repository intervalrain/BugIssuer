using BugIssuer.Application.Common.Security.Request;
using BugIssuer.Application.Common.Security.Permissions;

using ErrorOr;

using MediatR;
using BugIssuer.Application.Common.Security.Policies;

namespace BugIssuer.Application.Issuer.Commands.RemoveIssue;

[Authorize(Permissions = Permission.Issue.Remove, Policies = Policy.SelfOrAdmin)]
public record RemoveIssueCommand(string Applicant, int IssueId) : IRequest<ErrorOr<Success>>;