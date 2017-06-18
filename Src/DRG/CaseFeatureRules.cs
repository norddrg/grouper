using System.Collections.Generic;
using DRG.Core.Definitions;
using DRG.Interfaces;
using DRG.PrimaryCaseFeatureRules;
using DRG.SecondaryCaseFeatureRules;
using DRG.TertiaryCaseFeatureRules;

namespace DRG
{
    public static class CaseFeatureRules
    {
        private static readonly List<ICaseFeatureRule> Rules = new List<ICaseFeatureRule>()
        {
            // Primary Rules
            new PrimaryCaseFeatureRule(),

            // Secondary Rules
            new DiagnosisCategoryCaseFeatureRule(),
            new RecodeMainDiagnosisByGenderCaseFeatureRule(),
            new PrincipalDiagnosisPropertyCaseFeatureRule(),
            new ProcedurePropertiesCaseFeatureRule(),
            new ProcedureORPropertyCaseFeatureRule(),
            new ProcedureCCPropertyCaseFeatureRules(),
            new DiagnosisPropertiesCaseFeatureRule(),
            new RecodeDiagnosisPropertiesByGenderCaseFeatureRule(),
            new DiagnosisProcedurePropertiesCaseFeatureRule(),
            new ComplicationPropertyCaseFeatureRule(),
            
            // Tertiary Rules
            new MajorDiagnosticCategoryCaseFeatureRule(),
            new PresenceOfValidMainDiagnosisFeatureRule(),
            new ComplicationAndComorbidityCaseFeatureRule()
        };

        public static void SetCaseFeatureRules(IEnumerable<ICaseFeatureRule> rules)
        {
            Rules.Clear();
            Rules.AddRange(rules);
        }

        public static void ApplyCaseFeatureRules(this CaseFeatures caseFeatures, CaseData caseData, DefinitionsDataStore definitions)
        {
            // Return all results found
            Rules.ForEach(rule => rule.Apply(caseFeatures, caseData, definitions));
        }
    }
}