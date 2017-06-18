using DRG.Core.Drg;
using DRG.Core.Types;
using DRG.Interfaces;

namespace DRG.DRGGroupingRules
{
    public class PresenceOfValidMainDiagnosisGroupingRule : IDrgGroupingRule
    {
        public bool Apply(CaseFeatures caseFeatures, DrgLogic drgRow)
        {
            /*
            Operator	Conditions for the Criterion to be met	Comment
            ‘+’	
            The Criterion is met when Case.PresenceOfValidMainDiagnosis=1
            
            ‘-’	
            The Criterion is met when Case.PresenceOfValidMainDiagnosis=0	
            
            Blank (NULL)	
            The criterion is always met	
            
            For other Operators: The criterion is always met.
            */

            var op = drgRow.Icd.Operator;
            var caseVal = caseFeatures.PresenceOfValidMainDiagnosis;

            if (op == IcdValue.Plus)
            {
                return caseVal;
            }

            if (op == IcdValue.Minus)
            {
                return caseVal == false;
            }

            return true;
        }
    }
}