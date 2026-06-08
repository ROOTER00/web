using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Kurdixane.Web.Helpers;

public static class SlugHelper
{
    private static readonly Dictionary<char, string> TurkishMap = new()
    {
        ['ç'] = "c", ['Ç'] = "c", ['ğ'] = "g", ['Ğ'] = "g", ['ı'] = "i", ['İ'] = "i",
        ['ö'] = "o", ['Ö'] = "o", ['ş'] = "s", ['Ş'] = "s", ['ü'] = "u", ['Ü'] = "u"
    };

    public static string Generate(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        var sb = new StringBuilder();
        foreach (char c in input.Trim())
            sb.Append(TurkishMap.TryGetValue(c, out var rep) ? rep : c);

        string normalized = sb.ToString().ToLowerInvariant();
        normalized = RemoveDiacritics(normalized);
        normalized = Regex.Replace(normalized, @"[^a-z0-9\s-]", "");
        normalized = Regex.Replace(normalized, @"\s+", "-").Trim('-');
        normalized = Regex.Replace(normalized, @"-+", "-");
        return normalized;
    }

    private static string RemoveDiacritics(string text)
    {
        var formD = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (char c in formD)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
