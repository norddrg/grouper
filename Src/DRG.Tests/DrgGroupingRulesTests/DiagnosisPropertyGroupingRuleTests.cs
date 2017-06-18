using System.Collections.Generic;
using DRG.Core.Definitions;
using DRG.Core.Drg;
using DRG.Core.Features;
using DRG.Core.Types;
using DRG.DRGGroupingRules;
using DRG.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DRG.Tests.DrgGroupingRulesTests
{
    [TestClass]
    public class DiagnosisPropertyGroupingRuleTests
    {
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            DrgGroupingRules.SetDrgGroupingRules(new List<IDrgGroupingRule>()
            {
                new DiagnosisPropertyGroupingRule()
            });
        }

        [TestCategory("DiagnosisProperty Grouping Rules")]
        [TestMethod]
        public void It_matches_when_diagnosis_property_is_1_2_3_4_and_definitions_contains_1_2_3_4()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.DiagnosisProperties = new Dictionary<string, DiagnosisProperty>
            {
                {"1", new DiagnosisProperty("1", new DiagnosisPair("1", "2")) },
                {"2", new DiagnosisProperty("2", new DiagnosisPair("1", "2")) },
                {"3", new DiagnosisProperty("3", new DiagnosisPair("1", "2")) },
                {"4", new DiagnosisProperty("4", new DiagnosisPair("1", "2")) },
            };

            var definitions = CreateDefinitions("1", "2", "3", "4");

            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord1", drgLogicResult.Ord);
        }

        [TestCategory("DiagnosisProperty Grouping Rules")]
        [TestMethod]
        public void It_matches_when_diagnosis_property_is_2_and_4_and_definitions_contains_2_and_4()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.DiagnosisProperties = new Dictionary<string, DiagnosisProperty>
            {
                {"4", new DiagnosisProperty("4", new DiagnosisPair("1", "2")) },
                {"2", new DiagnosisProperty("2", new DiagnosisPair("1", "2")) },
            };

            var definitions = CreateDefinitions("", "2", "", "4");

            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord1", drgLogicResult.Ord);
        }

        [TestCategory("DiagnosisProperty Grouping Rules")]
        [TestMethod]
        public void It_matches_when_diagnosis_property_is_4_and_definitions_contains__plus_4()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.DiagnosisProperties = new Dictionary<string, DiagnosisProperty>
            {
                {"4", new DiagnosisProperty("4", new DiagnosisPair("1", "2")) },
            };

            var definitions = CreateDefinitions("", "+4", "", "");

            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord1", drgLogicResult.Ord);
        }

        [TestCategory("DiagnosisProperty Grouping Rules")]
        [TestMethod]
        public void It_matches_when_diagnosis_property_is_2_and_definitions_contains__minus_3()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.DiagnosisProperties = new Dictionary<string, DiagnosisProperty>
            {
                {"2", new DiagnosisProperty("2", new DiagnosisPair("1", "2")) },
            };

            var definitions = CreateDefinitions("", "-3", "", "");

            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord1", drgLogicResult.Ord);
        }

        [TestCategory("DiagnosisProperty Grouping Rules")]
        [TestMethod]
        public void It_fails_when_diagnosis_property_is_3_and_definitions_contains_minus_3()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.DiagnosisProperties = new Dictionary<string, DiagnosisProperty>
            {
                {"3", new DiagnosisProperty("3", new DiagnosisPair("1", "2")) },
            };

            var definitions = CreateDefinitions("-3", "", "", "");

            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.IsNull(drgLogicResult);
        }

        private static DefinitionsDataStore CreateDefinitions(string dgprop1, string dgprop2, string dgprop3, string dgprop4)
        {
            return new DefinitionsDataStore
            {
                DrgLogicModels = new List<DrgLogic>
                {
                    new DrgLogic(1, "ord1", "", "", "", "", "", "", "", "", "", "", "", dgprop1, dgprop2, dgprop3, dgprop4, "", "", "", "")
                }
            };
        }
    }
}
