using System.Collections.Generic;
using DRG.Core.Definitions;
using DRG.Core.Drg;
using DRG.DRGGroupingRules;
using DRG.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DRG.Tests.DrgGroupingRulesTests
{
    [TestClass]
    public class PresenceOfValidMainDiagnosisGroupingRuleTests
    {
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            DrgGroupingRules.SetDrgGroupingRules(new List<IDrgGroupingRule>()
            {
                new PresenceOfValidMainDiagnosisGroupingRule()
            });
        }

        [TestCategory("PresenceOfValidMainDiagnosis Grouping Rules")]
        [TestMethod]
        public void Operator_is_plus_and_criterion_is_true()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.PresenceOfValidMainDiagnosis = true;

            var definitions = CreateDefinitions("+");
            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord1", drgLogicResult.Ord);
        }

        [TestCategory("PresenceOfValidMainDiagnosis Grouping Rules")]
        [TestMethod]
        public void Operator_is_plus_and_criterion_is_false()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.PresenceOfValidMainDiagnosis = false;

            var definitions = CreateDefinitions("+");
            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.IsNull(drgLogicResult);
        }

        [TestCategory("PresenceOfValidMainDiagnosis Grouping Rules")]
        [TestMethod]
        public void Operator_is_blank_and_criterion_is_true()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.PresenceOfValidMainDiagnosis = true;

            var definitions = CreateDefinitions("");
            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord1", drgLogicResult.Ord);
        }

        [TestCategory("PresenceOfValidMainDiagnosis Grouping Rules")]
        [TestMethod]
        public void Operator_is_blank_and_criterion_is_false()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.PresenceOfValidMainDiagnosis = false;

            var definitions = CreateDefinitions("");
            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord1", drgLogicResult.Ord);
        }

        private static DefinitionsDataStore CreateDefinitions(string op)
        {
            return new DefinitionsDataStore
            {
                DrgLogicModels = new List<DrgLogic>
                {
                    new DrgLogic(1, "ord1", "", "", op, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
                }
            };
        }
    }
}