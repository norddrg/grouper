using System.Collections.Generic;
using DRG.Core.Definitions;
using DRG.Core.Drg;
using DRG.Core.Features;
using DRG.DRGGroupingRules;
using DRG.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DRG.Tests.DrgGroupingRulesTests
{
    [TestClass]
    public class ComplicationAndComorbidityGroupingRuleTests
    {
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            DrgGroupingRules.SetDrgGroupingRules(new List<IDrgGroupingRule>()
            {
                new ComplicationAndComorbidityGroupingRule()
            });
        }

        [TestCategory("ComplicationAndComorbidity Grouping Rules")]
        [TestMethod]
        public void It_matches_when_cclevel_is_1_and_definition_is_1()
        {
            /*
            var op = drgRow.Compl.Operator;
            var val = drgRow.Compl.Value;
            var caseVal = caseFeatures.ComplicationAndComorbidityLevel;
            */
            var caseFeatures = new CaseFeatures();
            caseFeatures.ComplicationAndComorbidityLevel = 1;

            var definitions = CreateDefinitions("1");

            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord1", drgLogicResult.Ord);
        }

        [TestCategory("ComplicationAndComorbidity Grouping Rules")]
        [TestMethod]
        public void It_matches_when_cclevel_is_2_and_definition_is_plus_1()
        {
            /*
            var op = drgRow.Compl.Operator;
            var val = drgRow.Compl.Value;
            var caseVal = caseFeatures.ComplicationAndComorbidityLevel;
            */
            var caseFeatures = new CaseFeatures();
            caseFeatures.ComplicationAndComorbidityLevel = 2;

            var definitions = CreateDefinitions("+1");

            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord1", drgLogicResult.Ord);
        }

        [TestCategory("ComplicationAndComorbidity Grouping Rules")]
        [TestMethod]
        public void It_matches_when_cclevel_is_1_and_definition_is_minus_2()
        {
            /*
            var op = drgRow.Compl.Operator;
            var val = drgRow.Compl.Value;
            var caseVal = caseFeatures.ComplicationAndComorbidityLevel;
            */
            var caseFeatures = new CaseFeatures();
            caseFeatures.ComplicationAndComorbidityLevel = 1;

            var definitions = CreateDefinitions("-2");

            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord1", drgLogicResult.Ord);
        }

        [TestCategory("ComplicationAndComorbidity Grouping Rules")]
        [TestMethod]
        public void It_fails_when_cclevel_is_2_and_definition_is_minus_2()
        {
            /*
            var op = drgRow.Compl.Operator;
            var val = drgRow.Compl.Value;
            var caseVal = caseFeatures.ComplicationAndComorbidityLevel;
            */
            var caseFeatures = new CaseFeatures();
            caseFeatures.ComplicationAndComorbidityLevel = 2;

            var definitions = CreateDefinitions("-2");

            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.IsNull(drgLogicResult);
        }

        private static DefinitionsDataStore CreateDefinitions(string compl)
        {
            return new DefinitionsDataStore
            {
                DrgLogicModels = new List<DrgLogic>
                {
                    new DrgLogic(1, "ord1", "", "", "", "", "", "", "", "", "", compl, "", "", "", "", "", "", "", "", "")
                }
            };
        }
    }
}