using BugIssuer.Application.Common.Security.Users;

namespace BugIssuer.Test.Common;

public static class CurrentUserFactory
{
    private static readonly Random _rand = new Random(); 

    public static CurrentUser Create(List<string>? permissions = null, List<string>? roles = null)
    {
        string userId = GenerateUserId();
        string userName = GenerateUserName();
        string email = GenerateEmail(userName);

        return new CurrentUser
        {
            UserId = userId,
            UserName = userName,
            Email = email,
            Permissions = (permissions ?? new List<string>()).AsReadOnly(),
            Roles = (roles ?? new List<string>()).AsReadOnly()
        };
    }

    private static string GenerateUserId()
    {
        return "000" + _rand.Next(10000, 100000).ToString("D5");
    }

    private static string GenerateUserName()
    {
        string firstName = GenerateRandomString(2, 5);
        string lastName = GenerateRandomString(2, 5);
        return $"{firstName} {lastName}";
    }

    private static string GenerateEmail(string userName)
    {
        string emailName = userName.ToLower().Replace(" ", "_");
        return emailName + "@umc.com";
    }


    private static string GenerateRandomString(int minLength, int maxLength)
    {
        int length = _rand.Next(minLength, maxLength + 1);
        const string vowels = "aeiouy";
        const string consonants = "bcdfghjklmnpqrstvwxz";
        const string chars = "abcdefghijklmnopqrstuvwxyz";

        char[] str = new char[length];
        str[0] = chars[_rand.Next(chars.Length)];

        for (int i = 1; i < length; i++)
        {
            if (vowels.Contains(str[i - 1]))
            {
                if (i > 1 && vowels.Contains(str[i - 2]))
                {
                    str[i] = consonants[_rand.Next(consonants.Length)];
                }
                else
                {
                    str[i] = chars[_rand.Next(chars.Length)];
                }
            }
            else
            {
                str[i] = vowels[_rand.Next(vowels.Length)];
            }
        }

        var result = new string(str); 
        return char.ToUpper(result[0]) + result.Substring(1);
    }
}

