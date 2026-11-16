using HW_2_POS.Application.Models;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace HW_2_POS.Infrastructure.Export
{
    internal sealed class JsonResultExporter : ResultExporter
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        public override string Format => "json";
        protected override string Render(IReadOnlyList<StudentResult> results)
        {
            var items = results.Select(r => new
            {
                studentId = r.StudentId,
                studentName = r.StudentName,
                finalScore = r.Score.Value
            });
            return JsonSerializer.Serialize(items, Options);
        }
    }

}
