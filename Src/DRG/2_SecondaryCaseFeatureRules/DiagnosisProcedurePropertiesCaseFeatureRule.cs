using System.Collections.Generic;
using System.Linq;
using DRG.Core;
using DRG.Core.Definitions;
using DRG.Core.Features;
using DRG.Interfaces;

namespace DRG.SecondaryCaseFeatureRules
{
    public class DiagnosisProcedurePropertiesCaseFeatureRule : ICaseFeatureRule
    {
        public void Apply(CaseFeatures caseFeatures, CaseData caseData, DefinitionsDataStore definitions)
        {
            /*
            The folloving rules from the Definition Data table DiagnosisFeatureRules are used:
            •	Rules where FeatureType=PROCR
            The FeatureValue of a given rule is assigned to the Case one or many times, in the following situations:
            •	When any single Code within the Diagnoses of the Case matches the DiagnosisCode1 value of the rule , and the rule does not contain a value for DiagnosisCode2.
            •	When two codes within the Diagnoses of the Case, having CodeNumber 1 and 2, match the DiagnosisCode2 and DiagnosisCode1 values of the rule respectively.

            */

            if (caseData.DiagnoseCodes.Count <= 0) return;
            var result = new List<DiagnosisDefinition>();

            foreach (var diagnosisPair in caseData.DiagnoseCodes)
            {
                var diagnosisDefinitions = new List<DiagnosisDefinition>();
                var temp1 = new List<DiagnosisDefinition>();

                definitions.DgModels_PROCPR.TryGetValue(diagnosisPair.Code1, out temp1);
                if (temp1 != null)
                    diagnosisDefinitions.AddRange(temp1);

                if (diagnosisPair.IsPair)
                {
                    var temp2 = new List<DiagnosisDefinition>();
                    definitions.DgModels_PROCPR.TryGetValue(diagnosisPair.Code2, out temp2);
                    if (temp2 != null)
                        diagnosisDefinitions.AddRange(temp2);
                }

                foreach (var diagnosisDefinition in diagnosisDefinitions)
                {
                    if (!diagnosisDefinition.HasCode2)
                    {
                        //rule 1.
                        if (diagnosisPair.Code1 == diagnosisDefinition.Code1 || diagnosisPair.Code2 == diagnosisDefinition.Code1)
                        {
                            result.Add(diagnosisDefinition);
                        }
                    }
                    else
                    {
                        //rule 2.
                        if (diagnosisPair.Code1 == diagnosisDefinition.Code2 && diagnosisPair.Code2 == diagnosisDefinition.Code1)
                        {
                            result.Add(diagnosisDefinition);
                        }
                    }
                }
            }

            foreach (var dgmodel in result)
            {
                if (caseFeatures.ProcedureProperties.ContainsKey(dgmodel.VarVal))
                {
                    var p = new ProcedureProperty(dgmodel.VarVal, null);
                    caseFeatures.ProcedureProperties[dgmodel.VarVal].Add(p);
                }
                else
                {
                    var p = new ProcedureProperty(dgmodel.VarVal, null);
                    caseFeatures.ProcedureProperties.Add(dgmodel.VarVal, new List<ProcedureProperty> {p});
                }
            }
        }
    }
}