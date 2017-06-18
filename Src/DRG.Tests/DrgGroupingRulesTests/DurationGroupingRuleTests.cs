using System.Collections.Generic;
using DRG.Core.Definitions;
using DRG.Core.Drg;
using DRG.DRGGroupingRules;
using DRG.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DRG.Tests.DrgGroupingRulesTests
{
    [TestClass]
    public class DurationGroupingRuleTests
    {
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            DrgGroupingRules.SetDrgGroupingRules(new List<IDrgGroupingRule>()
            {
                new DurationGroupingRule()
            });
        }

        [TestCategory("Duration Grouping Rules")]
        [TestMethod]
        public void It_matches_when_operator_is_less_than_and_caseFeatures_Duration_is_lower()
        {
            var definitions = CreateDefinition("<2");
            var caseFeatures = new CaseFeatures();
            caseFeatures.Duration = 1;

            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord1", drgLogicResult.Ord);
        }

        [TestCategory("Duration Grouping Rules")]
        [TestMethod]
        public void It_matches_when_operator_is_more_than_and_caseFeatures_Duration_is_higher()
        {
            var definitions = CreateDefinition(">2");
            var caseFeatures = new CaseFeatures();
            caseFeatures.Duration = 3;

            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord1", drgLogicResult.Ord);
        }

        [TestCategory("Duration Grouping Rules")]
        [TestMethod]
        public void It_matches_when_operator_is_blank_and_caseFeatures_Duration_is_some_value()
        {
            var definitions = CreateDefinition("");
            var caseFeatures = new CaseFeatures();
            caseFeatures.Duration = 0;

            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord1", drgLogicResult.Ord);
        }

        [TestCategory("Duration Grouping Rules")]
        [TestMethod]
        public void It_does_not_match_when_operator_is_less_than_and_caseFeatures_Duration_is_higher()
        {
            var definitions = CreateDefinition("<2");
            var caseFeatures = new CaseFeatures();
            caseFeatures.Duration = 3;

            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.IsNull(drgLogicResult);
        }

        [TestCategory("Duration Grouping Rules")]
        [TestMethod]
        public void It_does_not_match_when_operator_is_more_than_and_caseFeatures_Duration_is_lower()
        {
            var definitions = CreateDefinition(">2");
            var caseFeatures = new CaseFeatures();
            caseFeatures.Duration = 1;

            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.IsNull(drgLogicResult);
        }


        // Handy test helper
        private DefinitionsDataStore CreateDefinition(string duration)
        {
            return new DefinitionsDataStore
            {
                DrgLogicModels = new List<DrgLogic>
                {
                    new DrgLogic(1, "ord1", "drg1", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", duration, "")
                }
            };
        }
    }
}