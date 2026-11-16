using System;
using System.Collections.Generic;
using System.Text;

namespace HW_2_POS.Application.Models
{
    public sealed record GradeLine(
    Guid CriterionId,
    string CriterionName,
    decimal Score,
    decimal MaxScore,
    string? Comment,
    DateTimeOffset Date);
}
