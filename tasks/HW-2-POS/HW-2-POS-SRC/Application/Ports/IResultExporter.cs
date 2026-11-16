using HW_2_POS.Application.Models;

namespace HW_2_POS.Application.Ports
{
    public interface IResultExporter
    {
        string Format {  get; }
        void Export(IReadOnlyList<StudentResult> results, string path);
    }
}
