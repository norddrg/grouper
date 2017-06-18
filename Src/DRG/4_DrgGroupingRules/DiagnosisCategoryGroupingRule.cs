using DRG.Core.Drg;
using DRG.Core.Types;
using DRG.Interfaces;

namespace DRG.DRGGroupingRules
{
    public class DiagnosisCategoryGroupingRule : IDrgGroupingRule
    {
        /*

        // 1
        Blank (NULL)	
        Any string value (Non blank)
        Match between the DiagnosisCategory code of the Case and the Value of the Diagnosis Category Criterion	Frequent in today’s rules

        // 2
        “+”	(Plus sign)
        Any string value (Non blank)
        Match between the DiagnosisCategory code of the Case and the Value of the Diagnosis Category Criterion	Frequent in today’s rules
        
        // 3
        Blank (NULL)
        Blank (NULL)	
        The criterion is always met
        
        // 4
        “-“ (Minus sign)
        Any string value (Non blank)	
        The criterion is fullfilled when the DiagnosisCategory code of the Case does not match the Value of the Diagnosis Category Criterion	

        */

        public bool Apply(CaseFeatures caseFeatures, DrgLogic drgRow)
        {
            var op = drgRow.DgCat1.Operator;
            var val = drgRow.DgCat1.Value;
            var caseVal = caseFeatures.DiagnosisCategory;

            // 1
            if (op == DgCatValue.Blank)
            {
                if (caseVal == null) return false;
                return val == caseVal.Value;
            }

            // 2
            if (op == DgCatValue.Plus)
            {
                if (caseVal == null) return false;
                return val == caseVal.Value;
            }

            // 3
            if (op == DgCatValue.Null)
            {
                return true;
            }

            // 4
            if (op == DgCatValue.Minus)
            {
                if (caseVal == null) return false;
                return val != caseVal.Value;
            }

            // Fallback if no rule is met
            return true;
        }
    }
}