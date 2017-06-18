using System.Collections.Generic;
using System.Linq;
using DRG.Core;
using DRG.Core.Definitions;
using DRG.Core.Features;
using DRG.Interfaces;

namespace DRG.SecondaryCaseFeatureRules
{
    public class DiagnosisCategoryCaseFeatureRule : ICaseFeatureRule
    {
        public void Apply(CaseFeatures caseFeatures, CaseData caseData, DefinitionsDataStore definitions)
        {

            if (caseData.DiagnoseCodes.Count <= 0) return;

            DiagnosisDefinition found = null;

            // Diagnosis no. 1 in Case
            var diagnosisNoOne = caseData.DiagnoseCodes.First();

            var diagnosisDefinitions = new List<DiagnosisDefinition>();
            var temp1 = new List<DiagnosisDefinition>();

            definitions.DgModels_DGCAT.TryGetValue(diagnosisNoOne.Code1, out temp1);
            if (temp1 != null)
                diagnosisDefinitions.AddRange(temp1);

            if (diagnosisNoOne.IsPair)
            {
                var temp2 = new List<DiagnosisDefinition>();
                definitions.DgModels_DGCAT.TryGetValue(diagnosisNoOne.Code2, out temp2);
                if (temp2 != null)
                    diagnosisDefinitions.AddRange(temp2);

            }

            if (!diagnosisDefinitions.Any())
                return;

            if (diagnosisNoOne.IsPair)
            {
                // If the Diagnosis no.1 of a Case contains two Codes, and Code1 1 and 2 match the DiagnosisCode1 and DiagnosisCode2 values of a rule respectively, this rule is selected.
                found =
                    diagnosisDefinitions.FirstOrDefault(
                        x => x.Code1 == diagnosisNoOne.Code2 && x.Code2 == diagnosisNoOne.Code1);

                // If the Diagnosis no.1 of a Case contains two Codes, and Code1 2 match the DiagnosisCode1 values of a rule, this rule is selected.
                found = found ?? diagnosisDefinitions.FirstOrDefault(x => x.Code1 == diagnosisNoOne.Code2);

                // If the Diagnosis no.1 of a Case contains two or more Codes, and Code1 1 match the DiagnosisCode1 values of a rule, this rule is selected.
                found = found ?? diagnosisDefinitions.FirstOrDefault(x => x.Code1 == diagnosisNoOne.Code1);
            }
            else
            {
                // If the Diagnosis no.1 of a Case contains just one Code1, and this Code1 match the DiagnosisCode1 values of a rule, this rule is selected.
                found = diagnosisDefinitions.FirstOrDefault(x => x.Code1 == diagnosisNoOne.Code1);
            }


            // If there is a match based on the tests above, the Case gets a DiagnosisCategory.
            if (found != null)
            {
                caseFeatures.DiagnosisCategory = new DiagnosisCategory(found.VarVal);
            }
        }
    }
}