using System.Linq;
using DRG.Core;
using DRG.Core.Definitions;
using DRG.Core.Features;
using DRG.Interfaces;

namespace DRG.TertiaryCaseFeatureRules
{
    public class MajorDiagnosticCategoryCaseFeatureRule : ICaseFeatureRule
    {
        public void Apply(CaseFeatures caseFeatures, CaseData casdeata, DefinitionsDataStore data)
        {
            /*
            The MajorDiagnosticCategory of the Case is determined by the DiagnosisCategory of the Case.
            If the Case has a DiagnosisCategory feature, the Case gets a MajorDiagnosticCategory feature with a code consisting of the two first digits of the DiagnosisCategory code.
            If the Case does not have a DiagnosisCategory feature, the Case gets no MajorDiagnosticCategory feature.
            */

            if (caseFeatures.DiagnosisCategory != null)
            {
                caseFeatures.MajorDiagnosticCategory = caseFeatures.DiagnosisCategory.Value.Substring(0, 2);
            }
        }
    }
}