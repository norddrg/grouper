using System.Collections.Generic;
using System.Linq;
using DRG.Core.Drg;
using DRG.Core.Features;
using DRG.Core.Types;
using DRG.Interfaces;

namespace DRG.DRGGroupingRules
{
    public class ProcedurePropertyGroupingRule : IDrgGroupingRule
    {
        /*
        The Case is defined to meet the set of Procedure Property Criteria when each individual Criterion within the set is met.
        Procedure Properties derived from one distinct Procedure can only be used to fullfill one single Criterion within the set of 
        Procedure Property Criteria. (Implication: For the case to meet the set of Procedure Property Criteria, The Case must 
        contain at least as many Procedures as there are distinct criteria within the set.) 

        For each Criterion the following rules apply for the different combinations of Operators and Values:

        // 1
        Blank (NULL) or “+”	
        Any string value (Non blank)	
        Match between at least one ProcedureProperty of the Case and the Value of the ProcedurePropertyCriterion in question

        // 2
        Blank (NULL)	
        Blank (NULL)	
        The criterion is always met	

        // 3a
        “-“ (Minus sign)	
        Any string value (Non blank)	
        The criterion is fullfilled when the Case has no ProcedureProperties matching the Value of the ProcedurePropertyCriterion in question	
        Infrequent in today’s rules. Occurs in the Secproc1 field.

        // 3b
        “-“ (Minus sign)
        Blank (NULL)	
        The criterion is also fulfilled when  the Case has at least one ProcedureORProperty with FeatureValue=1 if each of the Procedures leading to 
        ProcedureORProperty=1 at the same time lead to at least one ProcedureProperty used as condition in another 
        ProcedurePropertyCriterion in the same set of ProcedurePropertyCriteria.
        */

        public bool Apply(CaseFeatures caseFeatures, DrgLogic drgRow)
        {
            var procOp = drgRow.ProcPro.Operator;
            var procVal = drgRow.ProcPro.Value;

            var secOp = drgRow.SecPro.Operator;
            var secVal = drgRow.SecPro.Value;

            var caseValues = caseFeatures.ProcedureProperties;

            if (secOp == SecProValue.Minus && string.IsNullOrEmpty(secVal))
            {
                // Special case for - (minus) in secproc and no value
                return true;
            }

            if (procOp != ProcProValue.Null && secOp != SecProValue.Null)
            {
                return HasValidProcPro(procOp, procVal, caseValues) && HasValidSecPro(secOp, secVal, caseValues);
            }

            if (procOp != ProcProValue.Null)
            {
                return HasValidProcPro(procOp, procVal, caseValues);
            }

            if (secOp != SecProValue.Null)
            {
                return HasValidSecPro(secOp, secVal, caseValues);
            }

            return true;
        }

        private bool HasValidProcPro(ProcProValue procOp, string procVal, IDictionary<string, List<ProcedureProperty>> caseValues)
        {
            if (procOp == ProcProValue.Blank || procOp == ProcProValue.Plus)
            {
                return caseValues.ContainsKey(procVal);
            }

            if (procOp == ProcProValue.Minus)
            {
                return ! caseValues.ContainsKey(procVal);
            }

            return true;
        }

        private bool HasValidSecPro(SecProValue secOp, string secVal, IDictionary<string, List<ProcedureProperty>> caseValues)
        {
            if (secOp == SecProValue.Blank || secOp == SecProValue.Plus)
            {
                return caseValues.ContainsKey(secVal);
            }

            if (secOp == SecProValue.Minus)
            {
                return ! caseValues.ContainsKey(secVal);
            }

            return true;
        }
    }
}