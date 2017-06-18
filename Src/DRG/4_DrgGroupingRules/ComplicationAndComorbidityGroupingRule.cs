using DRG.Core.Drg;
using DRG.Core.Types;
using DRG.Interfaces;

namespace DRG.DRGGroupingRules
{
    public class ComplicationAndComorbidityGroupingRule : IDrgGroupingRule
    {
        public bool Apply(CaseFeatures caseFeatures, DrgLogic drgRow)
        {
            /*
            The following rules apply for the different combinations of Operators and Values

            Blank (NULL)  or “+”	
            Any integer	
            The ComplicationAndComorbidityLevel of the Case must be equal to or larger than the Value

            “>”	
            Any integer	
            The ComplicationAndComorbidityLevel of the Case must be greater than the Value

            “<”	
            Any integer	
            The ComplicationAndComorbidityLevel of the Case must be less than the Value

            “-”	
            Any integer	
            The ComplicationAndComorbidityLevel of the Case can not be equal to the Value

            Blank (NULL)	
            Blank (NULL)	
            The criterion is always met	

            */
            var op = drgRow.Compl.Operator;
            var val = drgRow.Compl.Value;
            var caseVal = caseFeatures.ComplicationAndComorbidityLevel;

            if (op == ComplValue.Blank || op == ComplValue.Plus)
            {
                return caseVal >= val;
            }

            if (op == ComplValue.MoreThan)
            {
                return caseVal > val;
            }

            if (op == ComplValue.LessThan)
            {
                return caseVal < val;
            }

            if (op == ComplValue.Minus)
            {
                return caseVal != val;
            }

            return true;
        }
    }
}