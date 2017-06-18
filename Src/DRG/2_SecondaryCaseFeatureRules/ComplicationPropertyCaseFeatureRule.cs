using System;
using System.Collections.Generic;
using System.Linq;
using DRG.Core;
using DRG.Core.Definitions;
using DRG.Core.Features;
using DRG.Core.Types;
using DRG.Interfaces;

namespace DRG.SecondaryCaseFeatureRules
{
    public class ComplicationPropertyCaseFeatureRule : ICaseFeatureRule
    {
        public void Apply(CaseFeatures caseFeatures, CaseData caseData, DefinitionsDataStore definitions)
        {
            /*

                ComplicationProperty information is established by applying DiagnoseFeatureRules of the Definition Data on all Diagnoses except Diagnosis no. 1 from the Diagnoses list of the Case Data.

                The folloving rules from the Definition Data table DiagnosisFeatureRules are used:
                •	Rules where FeatureType=COMPL

                // 1
                When any single Code within the Diagnoses of the Case (except Diagnosis no. 1)  matches the DiagnosisCode1 value of the rule, and the rule does not contain a value for DiagnosisCode2.

                // 2
                When two codes within the Diagnoses of the Case (except Diagnosis no. 1), having CodeNumber 1 and 2, match the DiagnosisCode1 and DiagnosisCode2 values of the rule respectively.

           */

            if (caseData.DiagnoseCodes.Count <= 1) return;

            var secondAndOut = GetDefinitions(caseData.DiagnoseCodes.Skip(1), definitions);
            foreach (var diagnosisDefinition in secondAndOut)
            {
                if (!caseFeatures.ComplicationProperties.ContainsKey(diagnosisDefinition.VarVal))
                {
                    caseFeatures.ComplicationProperties.Add(diagnosisDefinition.VarVal,
                        new ComplicationProperty(diagnosisDefinition.VarVal));
                }
            }

            var first = GetDefinitions(caseData.DiagnoseCodes.Take(1), definitions);
            var removables = new List<string>();
            foreach (var diagnosisDefinition in first)
            {
                var complications = caseFeatures.ComplicationProperties.Where(x => x.Value.CompType != ComplicationPropertyType.Null);
                removables.AddRange(from kvp in complications where kvp.Value.WildcardValue == diagnosisDefinition.WildcardVarVal select kvp.Key);
            }

            foreach (var removable in removables)
            {
                caseFeatures.ComplicationProperties.Remove(removable);
            }
        }

        private List<DiagnosisDefinition> GetDefinitions(IEnumerable<DiagnosisPair> diagnosisPairs, DefinitionsDataStore definitions)
        {
            var result = new List<DiagnosisDefinition>(0);

            foreach (var diagnosisPair in diagnosisPairs)
            {
                var diagnosisDefinitions = new List<DiagnosisDefinition>();
                var temp1 = new List<DiagnosisDefinition>();

                definitions.DgModels_COMPL.TryGetValue(diagnosisPair.Code1, out temp1);
                if (temp1 != null)
                    diagnosisDefinitions.AddRange(temp1);

                if (diagnosisPair.IsPair)
                {
                    var temp2 = new List<DiagnosisDefinition>();
                    definitions.DgModels_COMPL.TryGetValue(diagnosisPair.Code2, out temp2);
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

            return result;
        }
    }
}