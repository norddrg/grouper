using System.Collections.Generic;
using System.Linq;
using DRG.Core.Definitions;
using DRG.Core.Features;
using DRG.Interfaces;

namespace DRG.SecondaryCaseFeatureRules
{
    public class ProcedureCCPropertyCaseFeatureRules : ICaseFeatureRule
    {

        /*
            ProcedureCCProperty information is obtained by applying ProcedureFeatureRules of the Definition Data on all Procedure information of the Case Data.
            •	Rules where FeatureType = CC
            The FeatureValue of a given rule is assigned to the Case one or many times, in the following situations:
            •	When any single Code within the Procedures of the Case matches the ProcedureCode1 value of the rule.

*/

        public void Apply(CaseFeatures caseFeatures, CaseData caseData, DefinitionsDataStore definitions)
        {
            foreach (var procedureCode in caseData.ProcedureCodes)
            {
                List<ProcedureDefinition> found;

                definitions.ProcModels_CC.TryGetValue(procedureCode, out found);

                if (found != null)
                {
                    foreach (var procpr in found)
                    {
                        if (!caseFeatures.ProcedureCCProperties.ContainsKey(procpr.VarVal))
                        {
                            var p = new ProcedureCCProperty(procpr.VarVal, procedureCode);
                            caseFeatures.ProcedureCCProperties.Add(procpr.VarVal, p);
                        }
                    }
                }
            }
        }
    }
}