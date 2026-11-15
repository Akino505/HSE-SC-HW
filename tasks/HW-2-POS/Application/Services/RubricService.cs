using HW_2_POS.Domain.Models;
using HW_2_POS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HW_2_POS.Application.Services
{
    public class RubricService(IRubricRepository rubricRepository) : IRubricService
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
    }
}
