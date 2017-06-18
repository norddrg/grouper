using DRG.Core.Drg;
using DRG.Core.Types;
using DRG.Interfaces;

namespace DRG.DRGGroupingRules
{
    public class SexGroupingRule : IDrgGroupingRule
    {
        public bool Apply(CaseFeatures caseFeatures, DrgLogic drgRow)
        {
            /*
            1
            Blank (NULL)	
            “M”	
            The Case.Sex must be equal to  1	
            
            2
            Blank (NULL)	
            “F”	
            The Case.Sex must be equal to 2	
            
            3
            “-“ (Minus)	
            Blank (NULL)	
            The Case.Sex is NULL / missing
            
            4
            Blank (NULL)
            Blank (NULL)	
            The Criterion is always met            
            */

            // 1
            if (drgRow.Sex == Gender.Male)
            {
                return caseFeatures.Gender == Gender.Male;
            }

            // 2
            if (drgRow.Sex == Gender.Female)
            {
                return caseFeatures.Gender == Gender.Female;
            }

            // 3
            if (drgRow.Sex == Gender.Minus)
            {
                return caseFeatures.Gender == Gender.Null;
            }

            // 4 and fallback
            return true;
        }
    }
}