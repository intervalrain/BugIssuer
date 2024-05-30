using System;
namespace BugIssuer.Application.Common.Security.Permissions;

public static partial class Permission
{
    public static class Issue
    {
        public const string Create = "create:issue";
        public const string Comment= "create:comment";
        public const string Remove = "remove:issue";
        public const string Update = "update:issue";

        public const string Get = "get:issue";
        public const string List = "list:issues";
        public const string ListMy = "list:myIssues";
        public const string Search = "search:issues";
    }
}

