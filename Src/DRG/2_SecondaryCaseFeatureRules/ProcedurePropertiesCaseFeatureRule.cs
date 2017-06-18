using System.Collections.Generic;
using DRG.Core.Definitions;
using DRG.Core.Features;
using DRG.Interfaces;

namespace DRG.SecondaryCaseFeatureRules
{
    public class ProcedurePropertiesCaseFeatureRule : ICaseFeatureRule
    {
        public void Apply(CaseFeatures caseFeatures, CaseData caseData, DefinitionsDataStore definitions)
        {
            foreach (var procedureCode in caseData.ProcedureCodes)
            {
                List<ProcedureDefinition> found;

                definitions.ProcModels_PROCPR.TryGetValue(procedureCode, out found);

                if (found != null)
                {
                    foreach (var procpr in found)
                    {
                        if (caseFeatures.ProcedureProperties.ContainsKey(procpr.VarVal))
                        {
                            var p = new ProcedureProperty(procpr.VarVal, procedureCode);
                            caseFeatures.ProcedureProperties[procpr.VarVal].Add(p);
                        }
                        else
                        {
                            var p = new ProcedureProperty(procpr.VarVal, procedureCode);
                            caseFeatures.ProcedureProperties.Add(procpr.VarVal, new List<ProcedureProperty> { p } );
                        }
                        /*if (!caseFeatures.ProcedureProperties.ContainsKey(procpr.VarVal))
                        {
                            var p = new ProcedureProperty(procpr.VarVal, procedureCode);
                            caseFeatures.ProcedureProperties.Add(procpr.VarVal, p);
                        }*/
                    }
                }
            }
        }
    }
}