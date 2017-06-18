using DRG.Core;
using DRG.Core.Definitions;
using DRG.Interfaces;

namespace DRG.TertiaryCaseFeatureRules
{
    public class PresenceOfValidMainDiagnosisFeatureRule : ICaseFeatureRule
    {
        public  void Apply(CaseFeatures caseFeatures, CaseData casdeata, DefinitionsDataStore data)
        {
            /*
            The PresenceOfValidMainDiagnosis of the Case is a flag for telling if the Case has a main diagnosis that can be used for grouping purposes or not.The PresenceOfValidMainDiagnosis of the Case is determined by the DiagnosisCategory of the Case:
            •	If the Case has a DiagnosisCategory feature: Case.PresenceOfValidMainDiagnosis = true
            •	If the Case does not have a DiagnosisCategory feature: Case.PresenceOfValidMainDiagnosis = false
            */

            if (caseFeatures.DiagnosisCategory != null)
            {
                caseFeatures.PresenceOfValidMainDiagnosis = true;
            }
        }
    }
}


