using BugIssuer.Application.Common.Interfaces;
using BugIssuer.Application.Common.Interfaces.Persistence;
using BugIssuer.Domain;

using FluentEmail.Core;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BugIssuer.Infrastructure.Common.Services;

public class EmailBackgroundService : IHostedService, IDisposable
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IFluentEmail _fluentEmail;
    private readonly IUserRepository _repo;
    private Timer _timer;

    public EmailBackgroundService(IServiceScopeFactory serviceScopeFactory, IDateTimeProvider dateTimeProvider, IFluentEmail fluentEmail)
    {
        _dateTimeProvider = dateTimeProvider;
        _fluentEmail = fluentEmail;
        _timer = null;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(SendEmailNotifications, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    private async void SendEmailNotifications(object? state)
    {
        var now = _dateTimeProvider.UtcNow;
        var oneMinute = now.AddMinutes(1);

        var issue = Issue.Create("", "", "", "", "", 1, now); // get new issues or issues subscribe

        var whom = issue.Comments.ConvertAll(comment => comment.AuthorId)
            .Union(new[] { issue.AuthorId }).Distinct();

        var users = await _repo.GetUserByIdsAsync(whom, CancellationToken.None);

        foreach (var user in users)
        {
            await _fluentEmail
                .To(user.Email)
                .Subject($"New issue notification")
                .Body($"""
                       Dear {user.UserName} from the present.

                       Here is the latest issue you have involved:

                       {issue.Title}

                       Link: https://localhost:7037/Home/Issue/{issue.IssueId}

                       Best,
                       {user.UserName} from the past.
                       """)
                .SendAsync();
        }
    }
}