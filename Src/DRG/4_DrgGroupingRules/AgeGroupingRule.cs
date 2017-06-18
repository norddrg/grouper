using System;
using DRG.Core.Drg;
using DRG.Core.Types;
using DRG.Interfaces;

namespace DRG.DRGGroupingRules
{
    public class AgeGroupingRule : IDrgGroupingRule
    {
        /*
        // 1
        “<”(less than)”	
        Any integer	
        The Case.Age must be less than the Value	

        // 2
        “>”(greater than)”	
        Any integer	
        The Case.Age must be greater than the Value	

        // 3
        Blank (NULL)
        Blank (NULL)
        The Criterion is always met
        */

        public bool Apply(CaseFeatures caseFeatures, DrgLogic drgRow)
        {
            var op = drgRow.Agelim.Operator;
            var val = drgRow.Agelim.Days;
            var caseVal = caseFeatures.Age;

            // 1
            if (op == AgeValue.LessThan)
            {
                return caseVal < val;
            }

            // 2
            if (op == AgeValue.MoreThan)
            {
                return caseVal > val;
            }

            // 3
            if (op == AgeValue.Null)
            {
                return true;
            }

            // Fallback if no rule is met
            return true;
        }
    }
}