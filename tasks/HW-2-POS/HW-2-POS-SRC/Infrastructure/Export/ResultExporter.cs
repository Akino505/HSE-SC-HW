using HW_2_POS.Application.Models;
using HW_2_POS.Application.Ports;
using System.Text;

namespace HW_2_POS.Infrastructure.Export
{
    internal abstract class ResultExporter : IResultExporter
    {
        public abstract string Format { get; }
        public void Export(IReadOnlyList<StudentResult> results, string path)
        {
            var prepared = Prepare(results);
            var content = Render(prepared);
            Write(path, content);
        }
        protected virtual IReadOnlyList<StudentResult> Prepare(IReadOnlyList<StudentResult> results)
        {
            return results.OrderBy(r => r.StudentName).ToList();
        }
        protected abstract string Render(IReadOnlyList<StudentResult> results);
        protected virtual void Write(string path, string content)
        {
            var directory = Path.GetDirectoryName(Path.GetFullPath(path));
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);
            File.WriteAllText(path, content, new UTF8Encoding(true));
        }
    }
}
