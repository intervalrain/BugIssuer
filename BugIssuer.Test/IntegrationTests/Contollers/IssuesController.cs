using BugIssuer.Test.IntegrationTests.Common;
using BugIssuer.Test.IntegrationTests.Common.WebApplicationFactory;

namespace BugIssuer.Test.IntegrationTests.Contollers;

[TestClass]
public class IssuesController
{
    private AppHttpClient _client;

    [TestInitialize]
    public void InitializeIssueController()
    {
        var webAppFactory = new WebAppFactory();
        _client = webAppFactory.CreateAppHttpClient();
        webAppFactory.ResetDatabase();
    }

}

