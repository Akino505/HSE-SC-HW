using HW_2_POS.Domain.Models;

namespace HW_2_POS.Domain.Services
{
    public interface IFinalScoreCalculator
    {
        FinalScore Calculate(Rubric rubric, StudentGrades grades);
    }
}
