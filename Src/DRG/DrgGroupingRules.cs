using System.Collections.Generic;
using System.Linq;
using DRG.Core.Definitions;
using DRG.Core.Drg;
using DRG.DRGGroupingRules;
using DRG.Interfaces;

namespace DRG
{
    public static class DrgGroupingRules
    {
        //this should be DI injected, but for now just implement directly.
        private static readonly List<IDrgGroupingRule> Rules = new List<IDrgGroupingRule>
        {
            new ProcedurePropertyGroupingRule(),
            new DiagnosisCategoryGroupingRule(),
            new PrincipalDiagnosisPropertyGroupingRule(),
            new MajorDiagnosticCategoryGroupingRule(),
            new DurationGroupingRule(),
            new PresenceOfValidMainDiagnosisGroupingRule(),
            new ProcedureORPropertyGroupingRule(),
            new DiagnosisPropertyGroupingRule(),
            new AgeGroupingRule(),
            new SexGroupingRule(),
            new DischargeModeGroupingRule(),
            new ComplicationAndComorbidityGroupingRule()
        };

        public static void SetDrgGroupingRules(IEnumerable<IDrgGroupingRule> rules)
        {
            Rules.Clear();
            Rules.AddRange(rules);
        }

        public static DrgLogic ApplyDrgGroupingRules(this DefinitionsDataStore definitions, CaseFeatures caseFeatures)
        {
            //return first found
            return definitions.DrgLogicModels.FirstOrDefault(drgLogicModel => Rules.All(rule => rule.Apply(caseFeatures, drgLogicModel)));
        }

        public static bool ApplyDrgGroupingRules(this DrgLogic drgModel, CaseFeatures caseFeatures)
        {
            return Rules.All(rule => rule.Apply(caseFeatures, drgModel));
        }
    }
}
