using System.Text.RegularExpressions;
using DRG.Core;
using DRG.Core.Definitions;
using DRG.Core.Features;
using DRG.Core.Types;
using DRG.Interfaces;

namespace DRG.SecondaryCaseFeatureRules
{
    public class RecodeMainDiagnosisByGenderCaseFeatureRule : ICaseFeatureRule
    {
        public void Apply(CaseFeatures caseFeatures, CaseData caseData, DefinitionsDataStore definitions)
        {
            /*
                DiagnosisCategory codes starting with ‘98’ are recoded based on gender information from the Case Data:
                •	If Case.Sex=1, the two first digits of the DiagnosisCategory code (‘98’) are replaced with ‘12’.
                •	If Case.Sex=2, the two first digits of the DiagnosisCategory code (‘98’) are replaced with ‘13’.
                •	If Case.Sex is none of the above, the DiagnosisCategory code remains unchanged.
            */
            if (caseFeatures.DiagnosisCategory != null && caseFeatures.DiagnosisCategory.IsRecodable)
            {
                var regex = new Regex(@"^98");
                string varVal;
                switch (caseFeatures.Gender)
                {
                    case Gender.Male:
                        varVal = regex.Replace(caseFeatures.DiagnosisCategory.Value, "12");
                        caseFeatures.DiagnosisCategory = new DiagnosisCategory(varVal);
                        break;
                    case Gender.Female:
                        varVal = regex.Replace(caseFeatures.DiagnosisCategory.Value, "13");
                        caseFeatures.DiagnosisCategory = new DiagnosisCategory(varVal);
                        break;
                }
            }
        }
    }
}