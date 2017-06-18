using System.Collections.Generic;
using System.Linq;
using DRG.Core;
using DRG.Core.Definitions;
using DRG.Core.Features;
using DRG.Interfaces;

namespace DRG.SecondaryCaseFeatureRules
{
    public class PrincipalDiagnosisPropertyCaseFeatureRule : ICaseFeatureRule
    {
        public void Apply(CaseFeatures caseFeatures, CaseData caseData, DefinitionsDataStore definitions)
        {
            /*
            The FeatureValue of a given rule is assigned to the Case one or many times, in the following situations:
            
            - When any single Code within Diagnosis no. 1 of the Case  matches the DiagnosisCode1 value of the rule, 
            and the rule does not contain a value for DiagnosisCode2.
             
            - When two codes within Diagnosis no. 1 of the Case, having CodeNumber 1 and 2, match the 
            DiagnosisCode2 and DiagnosisCode1 values of the rule respectively.

            */
            if (caseData.DiagnoseCodes.Count <= 0) return;

            var diagnosisNoOne = caseData.DiagnoseCodes.First();
            var diagnosisDefinitions = new List<DiagnosisDefinition>();
            var temp1 = new List<DiagnosisDefinition>();

            definitions.DgModels_PDGPRO.TryGetValue(diagnosisNoOne.Code1, out temp1);
            if (temp1 != null)
                diagnosisDefinitions.AddRange(temp1);

            if (diagnosisNoOne.IsPair)
            {
                var temp2 = new List<DiagnosisDefinition>();
                definitions.DgModels_PDGPRO.TryGetValue(diagnosisNoOne.Code2, out temp2);
                if (temp2 != null)
                    diagnosisDefinitions.AddRange(temp2);
            }

            if (!diagnosisDefinitions.Any())
                return;

            var result = new List<DiagnosisDefinition>();
            foreach (var diagnosisDefinition in diagnosisDefinitions)
            {
                if (!diagnosisDefinition.HasCode2)
                {
                    //rule 1.
                    if (diagnosisNoOne.Code1 == diagnosisDefinition.Code1 || diagnosisNoOne.Code2 == diagnosisDefinition.Code1)
                    {
                        result.Add(diagnosisDefinition);
                    }
                }
                else
                {
                    //rule 2.
                    if (diagnosisNoOne.Code1 == diagnosisDefinition.Code2 && diagnosisNoOne.Code2 == diagnosisDefinition.Code1)
                    {
                        result.Add(diagnosisDefinition);
                    }
                }
            }

            // If there is a match based on the tests above, the Case gets a DiagnosisCategory.
            foreach (var diagnosisDefinition in result)
            {
                if (!caseFeatures.PrincipalDiagnosisProperties.ContainsKey(diagnosisDefinition.VarVal))
                {
                    caseFeatures.PrincipalDiagnosisProperties.Add(diagnosisDefinition.VarVal, new PrincipalDiagnosisProperty(diagnosisDefinition.VarVal));
                }
            }
        }
    }
}