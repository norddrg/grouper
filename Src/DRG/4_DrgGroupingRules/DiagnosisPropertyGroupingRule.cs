using System.Collections.Generic;
using System.Linq;
using DRG.Core.Drg;
using DRG.Core.Types;
using DRG.Interfaces;

namespace DRG.DRGGroupingRules
{
    public class DiagnosisPropertyGroupingRule : IDrgGroupingRule
    {

        /*
        The Case is defined to meet the set of Diagnosis Property Criteria when each individual Criterion within the set is met.
        Multiple Diagnosis Properties derived from the same Diagnosis or Procedure can be used to fullfill one ore more Criteria within the total set of Diagnosis Property Criteria. 
        For each Criterion the following rules apply for the different combinations of Operators and Values:

        1
        Blank (NULL)  or “+”	
        Any string value (Non blank)	
        Match between at least one DiagnosisProperty of the Case and the Value of the DiagnosisPropertyCriterion in question

        2
        Blank (NULL)	
        Blank (NULL)	
        The criterion is always met
        
        3
        “-“ (Minus sign)	
        Any string value (Non blank)	
        The criterion is fullfilled when the Case has no DiagnosisProperties matching the Value of the DiagnosisPropertyCriterion in question	
        */

        public bool Apply(CaseFeatures caseFeatures, DrgLogic drgRow)
        {
            var dg1Op = drgRow.Dgprop1.Operator;
            var dg2Op = drgRow.Dgprop2.Operator;
            var dg3Op = drgRow.Dgprop3.Operator;
            var dg4Op = drgRow.Dgprop4.Operator;
            var caseValues = caseFeatures.DiagnosisProperties;

            // 2
            if (dg1Op == DgPropValue.Null && 
                dg2Op == DgPropValue.Null && 
                dg3Op == DgPropValue.Null &&
                dg4Op == DgPropValue.Null)
            {
                return true;
            }

            // 1 and 3
            var dgPropsInUse = new List<DgProp>();

            if (dg1Op != DgPropValue.Null)
                dgPropsInUse.Add(drgRow.Dgprop1);

            if (dg2Op != DgPropValue.Null)
                dgPropsInUse.Add(drgRow.Dgprop2);

            if (dg3Op != DgPropValue.Null)
                dgPropsInUse.Add(drgRow.Dgprop3);

            if (dg4Op != DgPropValue.Null)
                dgPropsInUse.Add(drgRow.Dgprop4);

            foreach (var dgProp in dgPropsInUse)
            {
                if (dgProp.Operator == DgPropValue.Blank || dgProp.Operator == DgPropValue.Plus)
                {
                    if (!caseValues.ContainsKey(dgProp.Value))
                        return false;
                }
                else if (dgProp.Operator == DgPropValue.Minus)
                {
                    if (caseValues.ContainsKey(dgProp.Value))
                    {
                        return false;
                    }
                }
            }

            // Optimistic ending!
            return true;
        }
    }
}