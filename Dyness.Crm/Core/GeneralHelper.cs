namespace Core
{
    public static class GeneralHelper
    {
        public static string ClearChars(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";

            const string chars = "üğışçö\"é!''£^#+$%½&/{([)]=}?*\\|_~,`’‘;.:@æ\"'“”";
            const string willBeChanged = "ugisco ";

            text = text.Replace(' ', '-');
            text = text.ToLower();

            for (var i = 0; i < chars.Length; i++)
            {
                if (text.IndexOf(chars[i]) != -1)
                {
                    text = text.Replace(chars[i], willBeChanged[i < 6 ? i : 6]);
                }
            }
            text = text.Replace(" ", "");
            text = text.Replace("--", "-");

            return text;
        }
    }
}
