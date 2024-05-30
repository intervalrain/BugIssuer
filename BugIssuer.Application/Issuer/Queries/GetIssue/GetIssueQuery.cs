using BugIssuer.Application.Common.Security.Policies;
using BugIssuer.Application.Common.Security.Request;
using BugIssuer.Application.Common.Security.Permissions;
using BugIssuer.Domain;
using ErrorOr;
using MediatR;

namespace BugIssuer.Application.Issuer.Queries.GetIssue;

[Authorize(Permissions = Permission.Issue.Get)]
public record GetIssueQuery(string Applicant, int IssueId) : IRequest<ErrorOr<Issue>>;