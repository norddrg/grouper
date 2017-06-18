using System.Collections.Generic;
using System.Text.RegularExpressions;
using DRG.Core;
using DRG.Core.Definitions;
using DRG.Core.Features;
using DRG.Core.Types;
using DRG.Interfaces;

namespace DRG.SecondaryCaseFeatureRules
{
    public class RecodeDiagnosisPropertiesByGenderCaseFeatureRule : ICaseFeatureRule
    {
        public void Apply(CaseFeatures caseFeatures, CaseData caseData, DefinitionsDataStore definitions)
        {
            /*
                DiagnosisProperty codes starting with ‘98’ are recoded based on gender information from the Case Data:
                •	If Case.Sex=1, the two first digits of the DiagnosisProperty code (‘98’) are replaced with ‘12’.
                •	If Case.Sex=2, the two first digits of the DiagnosisProperty code (‘98’) are replaced with ‘13’.
                •	If Case.Sex is none of the above, the DiagnosisProperty code remains unchanged.
            */

            var updatedDictionary = new Dictionary<string, DiagnosisProperty>(caseFeatures.DiagnosisProperties.Count);

            foreach (var diagnosisProperty in caseFeatures.DiagnosisProperties)
            {
                if (diagnosisProperty.Value.IsRecodable)
                {
                    var regex = new Regex(@"^98");
                    switch (caseFeatures.Gender)
                    {
                        case Gender.Male:
                            var male = regex.Replace(diagnosisProperty.Value.Value, "12");
                            if (! updatedDictionary.ContainsKey(male))
                                updatedDictionary.Add(male, new DiagnosisProperty(male, diagnosisProperty.Value.DiagnosisPair));
                            break;
                        case Gender.Female:
                            var female = regex.Replace(diagnosisProperty.Value.Value, "13");
                            if (! updatedDictionary.ContainsKey(female))
                                updatedDictionary.Add(female, new DiagnosisProperty(female, diagnosisProperty.Value.DiagnosisPair));
                            break;
                        default:
                            if (! updatedDictionary.ContainsKey(diagnosisProperty.Key))
                                updatedDictionary.Add(diagnosisProperty.Key, new DiagnosisProperty(diagnosisProperty.Value.Value, diagnosisProperty.Value.DiagnosisPair));
                            break;
                    }
                }
                else
                {
                    if (! updatedDictionary.ContainsKey(diagnosisProperty.Key))
                        updatedDictionary.Add(diagnosisProperty.Key, new DiagnosisProperty(diagnosisProperty.Value.Value, diagnosisProperty.Value.DiagnosisPair));
                }
            }

            caseFeatures.DiagnosisProperties = updatedDictionary;
        }


    }
}