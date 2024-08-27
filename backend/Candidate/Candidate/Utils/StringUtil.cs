using System.Globalization;
using System.Text;

namespace Candidate.Utils
{
    public class StringUtil
    {
        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
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
        public static string FormatEventName(string eventName)
        {
            // Loại bỏ dấu tiếng Việt
            var nameWithoutDiacritics = RemoveDiacritics(eventName);

            // Chuyển toàn bộ ký tự thành chữ thường
            var lowerCaseName = nameWithoutDiacritics.ToLower();

            // Thay khoảng trắng bằng dấu gạch ngang
            var formattedName = lowerCaseName.Replace(' ', '-');

            return formattedName;
        }
    }
}
