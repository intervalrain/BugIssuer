namespace BugIssuer.Test.Common;

public static class StringGenerator
{
    private static readonly Random _rand = new Random();

    public static string GenerateUserId()
    {
        return "000" + _rand.Next(10000, 100000).ToString("D5");
    }

    public static string GenerateUserName()
    {
        string firstName = GenerateRandomString(2, 5);
        string lastName = GenerateRandomString(2, 5);
        return $"{firstName} {lastName}";
    }

    public static string GenerateEmail(string userName)
    {
        string emailName = userName.ToLower().Replace(" ", "_");
        return emailName + "@umc.com";
    }

    public static string GenerateContent(int length = 10)
    {
        return string.Join(" ", Enumerable.Range(0, length).Select(x => GenerateRandomString(5, 10)));
    }

    public static string GenerateRandomString(int minLength, int maxLength)
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