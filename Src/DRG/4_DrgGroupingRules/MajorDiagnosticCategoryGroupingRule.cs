using System;
using DRG.Core;
using DRG.Core.Drg;
using DRG.Core.Types;
using DRG.Interfaces;

namespace DRG.DRGGroupingRules
{
    public class MajorDiagnosticCategoryGroupingRule : IDrgGroupingRule
    {
        public bool Apply(CaseFeatures caseFeatures, DrgLogic drgRow)
        {
            // 1
            // Operator: blank (NULL)
            // Value: Any string value (Non blank)
            // Conditions: Match between the MajorDiagnosticCategory code of the Case and the Value of the Major Diagnostic Category Criterion

            // 2
            // Operator: +
            // Value: Any string value (Non blank)
            // Conditions: Match between the MajorDiagnosticCategory code of the Case and the Value of the Major Diagnostic Category Criterion

            // 3
            // Operator: blank (NULL)
            // Value: blank (NULL)
            // Conditions: The criterion is always met

            // 4
            // Operator: -
            // Value: Any string value (Non blank)
            // The criterion is fullfilled when the MajorDiagnosticCategory code of the Case does not match the Value of the Major Diagnostic Category Criterion

            var op = drgRow.Mdc.Operator;
            var val = drgRow.Mdc.Value;
            var caseVal = caseFeatures.MajorDiagnosticCategory;

            // 1
            if (op == MdcValue.Blank)
            {
                return val == caseVal;
            }

            // 2
            if (op == MdcValue.Plus)
            {
                return val == caseVal;
            }

            // 3
            if (op == MdcValue.Null)
            {
                return true;
            }

            // 4
            if (op == MdcValue.Minus)
            {
                return val != caseVal;
            }

            // Fallback if no rule is met
            return true;
        }
    }
}