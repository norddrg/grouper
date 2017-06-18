using System;
using DRG.Core.Drg;
using DRG.Core.Types;
using DRG.Interfaces;

namespace DRG.DRGGroupingRules
{
    public class DurationGroupingRule : IDrgGroupingRule
    {
        /*
        // 1
        “<”(less than)”	
        Any integer	
        The Case.Duration must be less than the Value	

        // 2
        “>”(greater than)”	
        Any integer	
        The Case.Duration must be greater than the Value	

        // 3
        Blank (NULL)
        Blank (NULL)
        The Criterion is always met
        */


        public bool Apply(CaseFeatures caseFeatures, DrgLogic drgRow)
        {
            var op = drgRow.Dur.Operator;
            var val = drgRow.Dur.Days;
            var caseVal = caseFeatures.Duration;

            // 1
            if (op == DurationValue.LessThan)
            {
                return caseVal < val;
            }

            // 2
            if (op == DurationValue.MoreThan)
            {
                return caseVal > val;
            }

            // 3
            if (op == DurationValue.Null)
            {
                return true;
            }

            // Fallback if no rule is met
            return true;
        }
    }
}