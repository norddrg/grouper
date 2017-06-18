using System.Collections.Generic;
using DRG.Core.Definitions;
using DRG.Core.Drg;
using DRG.DRGGroupingRules;
using DRG.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DRG.Tests.DrgGroupingRulesTests
{
    [TestClass]
    public class MajorDiagnosticCategoryGroupingRuleTests
    {
        static DefinitionsDataStore _definitions;

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            _definitions = CreateDefinitions();

            DrgGroupingRules.SetDrgGroupingRules(new List<IDrgGroupingRule>()
            {
                new MajorDiagnosticCategoryGroupingRule()
            });
        }

        [TestCategory("MajorDiagnosticCategory Grouping Rules")]
        [TestMethod]
        public void Match_between_mdc_and_value_expected_positive_operator()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.MajorDiagnosticCategory = "15";

            var drgLogicResult = _definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord1", drgLogicResult.Ord);
        }

        [TestCategory("MajorDiagnosticCategory Grouping Rules")]
        [TestMethod]
        public void Match_between_mdc_and_value_without_operator()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.MajorDiagnosticCategory = "17";

            var drgLogicResult = _definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord2", drgLogicResult.Ord);
        }

        [TestCategory("MajorDiagnosticCategory Grouping Rules")]
        [TestMethod]
        public void Match_between_mdc_and_value_expected_blank_operator()
        {
            var caseFeatures = new CaseFeatures();

            var drgLogicResult = _definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord2", drgLogicResult.Ord);
        }

        private static DefinitionsDataStore CreateDefinitions()
        {
            return new DefinitionsDataStore
            {
                DrgLogicModels = new List<DrgLogic>
                {
                    new DrgLogic(1, "ord1", "", "", "", "+15", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""),
                    new DrgLogic(2, "ord2", "", "", "", "-16", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""),
                    new DrgLogic(3, "ord3", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""),
                    new DrgLogic(4, "ord4", "", "", "", "17", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
                }                               
            };
        }
    }
}