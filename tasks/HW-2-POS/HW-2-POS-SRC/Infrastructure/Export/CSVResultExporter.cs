using HW_2_POS.Application.Models;
using System.Globalization;
using System.Text;

namespace HW_2_POS.Infrastructure.Export
{
    internal sealed class CSVResultExporter : ResultExporter
    {
        public override string Format => "csv";
        protected override string Render(IReadOnlyList<StudentResult> results)
        {
            var builder = new StringBuilder();
            builder.AppendLine("StudentId;StudentName;FinalScore");
            foreach (var result in results)
            {
                var score = result.Score.Value.ToString(CultureInfo.InvariantCulture);
                builder.AppendLine($"{result.StudentId};{Escape(result.StudentName)};{score}");
            }
            return builder.ToString();
        }
        private static string Escape(string value)
        {
            var needsQuotes = value.IndexOfAny(new[] { ';', '"', '\n', '\r' }) >= 0;
            return needsQuotes ? "\"" + value.Replace("\"", "\"\"") + "\"" : value;
        }
    }
}
