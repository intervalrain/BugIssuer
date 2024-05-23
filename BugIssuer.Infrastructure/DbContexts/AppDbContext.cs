using BugIssuer.Domain;
using Microsoft.EntityFrameworkCore;

namespace BugIssuer.Infrastructure.DbContexts;

public class AppDbContext : DbContext
{
    public DbSet<Issue> Issues { get; set; }
    public DbSet<Comment> Comments  { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
		: base(options)
	{
	}
}

