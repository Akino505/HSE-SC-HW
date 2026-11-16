using HW_2_POS.Domain.Models;
using HW_2_POS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HW_2_POS.Application.Services
{
    public class RubricService(IRubricRepository rubricRepository, IGradesRepository gradesRepository) : IRubricService
    {
        public IReadOnlyList<Criterion> List()
        {
            return rubricRepository.Get().Criteria;
        }
        public Criterion AddCriterion(string name, decimal maxScore, decimal weight)
        {
            var rubric = rubricRepository.Get();
            var criterion = rubric.AddCriterion(name, maxScore, weight);
            rubricRepository.Save(rubric);
            return criterion;
        }
        public int DeleteCriterion(Guid criterionId)
        {
            var rubric = rubricRepository.Get();
            rubric.RemoveCriterion(criterionId);
            var removedGrades = 0;
            foreach (var sheet in gradesRepository.List())
            {
                if (!sheet.CancelIfPresent(criterionId))
                    continue;
                gradesRepository.Save(sheet);
                removedGrades++;
            }
            rubricRepository.Save(rubric);
            return removedGrades;
        }
    }
}
