using DRG.Core.Drg;
using DRG.Core.Features;
using DRG.Core.Types;
using DRG.Interfaces;

namespace DRG.DRGGroupingRules
{
    public class DischargeModeGroupingRule : IDrgGroupingRule
    {
        /*
        1
        “=”(equal to) or Blank (NULL)	
        “N”	
        The criterion is met when the Case.DischargeMode is not equal “E”	

        2
        “=”(equal to) or Blank (NULL)	
        Any character (a-Z) except “N”	
        The criterion is met when the Case.DischargeMode is equal to the Value	

        3
        “-”(not equal to)	
        Any character (a-Z)	
        The criterion is met when the Case.DischargeMode is not equal to the Value	

        4
        Any value or Blank (NULL)	
        Blank (NULL)	
        The Criterion is always met
        */
        public bool Apply(CaseFeatures caseFeatures, DrgLogic drgRow)
        {
            var op = drgRow.Disch.Operator;
            var val = drgRow.Disch.Value;
            var caseVal = caseFeatures.DischargeMode;

            // 1 and 2
            if (op == DischValue.Equals || op == DischValue.Blank)
            {
                if (val == DischargeMode.N)
                {
                    return caseVal != DischargeMode.E;
                }

                return val == caseVal;
            }

            // 3
            if (op == DischValue.Minus)
            {
                return val != caseVal;
            }

            // 4 and fallback
            return true;
        }
    }
}