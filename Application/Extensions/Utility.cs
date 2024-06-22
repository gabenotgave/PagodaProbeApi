namespace Application.Extensions
{
    public static class Utility
    {
        public static string CapitalizeFirstLetter(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            if (value.Length == 1)
                return value.ToUpper();

            return value.Substring(0, 1).ToUpper() + value.Substring(1);
        }

        public static string RemoveCommasAndSemicolons(this string value)
        {
            return value.Replace(";", "").Replace(",", "");
        }
    }
}
