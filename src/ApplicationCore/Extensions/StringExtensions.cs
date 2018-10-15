using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ApplicationCore.Extensions
{
    public static class StringExtensions
    {
        public static string Slugify(this string text)
        {
            const string pattern = @"[^a-zA-Z0-9\-]";
            const string replacement = "-";
            var regex = new Regex(pattern);
            var result = regex.Replace(_removeDiacritics(text), replacement)
                .Replace("--", "-")
                .Trim('-');
            return result;
        }

        private static string _removeDiacritics(string text)
        {
            var normalizedString = text.ToLower()
                .Trim()
                .Normalize(NormalizationForm.FormD);

            var stringBuilder = new StringBuilder();
            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
