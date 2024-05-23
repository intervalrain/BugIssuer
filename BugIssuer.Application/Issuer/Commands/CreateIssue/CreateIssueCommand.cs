using BugIssuer.Domain;

using ErrorOr;

using MediatR;

namespace BugIssuer.Application.Issuer.Commands.CreateIssue;

public record CreateIssueCommand(
	string Title,
	string Description,
	string Category,
	string Author) : IRequest<ErrorOr<Issue>>;