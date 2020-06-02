namespace BookStore.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToMySqlLikeSyntax(this string value)
        {
            return $"%{value}%";
        }

        public static string ApplyFormatForUrl(this string value)
        {
            return string.IsNullOrEmpty(value) ? value : value.Trim().Replace(" ", "_");
        }
    }
}