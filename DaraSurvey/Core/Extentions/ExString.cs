namespace DaraSurvey.Core
{
    public static class ExString
    {
        public static string UppercaseFirst(this string str)
        {
            return (string.IsNullOrEmpty(str))
                ? string.Empty
                : char.ToUpper(str[0]) + str.Substring(1);
        }
    }
}
