using System.Linq;
using DRG.Core.Drg;
using DRG.Core.Types;
using DRG.Interfaces;

namespace DRG.DRGGroupingRules
{
    public class ProcedureORPropertyGroupingRule : IDrgGroupingRule
    {
        /*
        1
        Blank (NULL)  or “+”	
        “S”	
        The Case must have at least one ProcedureORProperty with a value equal to 1
        
        2
        Blank (NULL)  or “+”	
        “P”	
        The Case must have at least one ProcedureORProperty with a value equal to 1 or 2
        
        3
        Blank (NULL)  or “+”	
        “N”	
        The Case can not  have any ProcedureORProperty with a value equal to 1
        
        4
        Blank (NULL)  or “+”	
        “Z”	
        The Case can not  have any ProcedureORProperty with a value equal to 1 or 2
        
        5
        “+”	
        Blank (NULL)	
        The Case must have at least one ProcedureORProperty, regardless of value	
        Same result as the value “P” with today’s configurations
        
        6
        “-”	
        Blank (NULL)	
        The Case can not have any ProcedureORProperty, regardless of value	
        Same result as the value “Z” with today’s configurations
        
        7
        Blank (NULL)	
        Blank (NULL)	
        The criterion is always met	Frequent in today’s rules
        
        8
        “-”	
        Any value (not NULL)	
        The criterion is never met	

        */

        public bool Apply(CaseFeatures caseFeatures, DrgLogic drgRow)
        {
            var op = drgRow.OrProp.Operator;
            var val = drgRow.OrProp.Type;
            var caseValues = caseFeatures.ProcedureORProperties;

            if ((op == OrPropValue.Plus || op == OrPropValue.Blank) && val != OrPropType.Null && val != OrPropType.Other)
            {
                if (val == OrPropType.S)
                {
                    // The Case must have at least one ProcedureORProperty with a value equal to 1
                    return caseValues.ContainsKey("1");
                }

                if (val == OrPropType.P)
                {
                    // The Case must have at least one ProcedureORProperty with a value equal to 1 or 2
                    return caseValues.ContainsKey("1") || caseValues.ContainsKey("2");
                }

                if (val == OrPropType.N)
                {
                    // The Case can not  have any ProcedureORProperty with a value equal to 1
                    return ! caseValues.ContainsKey("1");
                }

                if (val == OrPropType.Z)
                {
                    //The Case can not  have any ProcedureORProperty with a value equal to 1 or 2	Frequent in today’s rules
                    return ! caseValues.ContainsKey("1") && ! caseValues.ContainsKey("2");
                }
            }
            // 5
            if (op == OrPropValue.Plus && val == OrPropType.Null)
            {
                return caseValues.Any();
            }

            // 6
            if (op == OrPropValue.Minus && val == OrPropType.Null)
            {
                return caseValues.Count == 0;
            }

            // 7
            if (op == OrPropValue.Null && val == OrPropType.Null)
            {
                return true;
            }

            // 8
            if (op == OrPropValue.Minus && val == OrPropType.Other)
            {
                return false;
            }

            // Fallback if no rule is met
            return true;
        }
    }
}