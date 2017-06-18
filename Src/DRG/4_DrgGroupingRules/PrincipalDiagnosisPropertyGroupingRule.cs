using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DRG.Core.Drg;
using DRG.Core.Types;
using DRG.Interfaces;

namespace DRG.DRGGroupingRules
{
    public class PrincipalDiagnosisPropertyGroupingRule : IDrgGroupingRule
    {
        

        public bool Apply(CaseFeatures caseFeatures, DrgLogic drgRow)
        {
            // 1
            // Operator: Blank(NULL)
            // Value: Any string value (Non blank)
            // Conditions: Match between the PrincipalDiagnosisProperty code of the Case and the Value of the PrincipalDiagnosisPropertyCriterion

            // 2
            // Operator: +
            // Value: Any string value (Non blank)
            // Conditions: Match between the PrincipalDiagnosisProperty code of the Case and the Value of the PrincipalDiagnosisPropertyCriterion

            // 3
            // Operator: blank (NULL)
            // Value: blank (NULL)
            // Conditions: The criterion is always met

            // 4
            // Operator: -
            // Value: Any string value (Non blank)
            // Conditions: The criterion is fullfilled when the DiagnosisCategory code of the Case does not match the Value of the Diagnosis Category Criterion


            var op = drgRow.Pdg.Operator;
            var val = drgRow.Pdg.Value;
            var caseValues = caseFeatures.PrincipalDiagnosisProperties;

            // 1
            if (op == PdgValue.Blank)
            {
                return caseValues.ContainsKey(val);
            }

            // 2
            if (op == PdgValue.Plus)
            {
                return caseValues.ContainsKey(val);
            }

            // 3
            if (op == PdgValue.Null)
            {
                return true;
            }

            // 4
            if (op == PdgValue.Minus)
            {
                return !caseValues.ContainsKey(val);
            }

            // Fallback if no rule is met
            return true;
        }
    }
}
