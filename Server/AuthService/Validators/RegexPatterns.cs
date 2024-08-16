using System.Text.RegularExpressions;

namespace AuthService.Validators
{
    public static class RegexPatterns
    {
        public static readonly Regex Username = new(@"^[a-zA-Z0-9]{4,24}$\r\n", RegexOptions.Compiled);
        public static readonly Regex Password = new(@"^[a-zA-Z0-9!@#~*]{8,24}$", RegexOptions.Compiled);
        public static readonly Regex Email = new(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled);
    }
}
