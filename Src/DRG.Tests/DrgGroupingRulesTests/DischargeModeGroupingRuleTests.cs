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
    public class DischargeModeGroupingRuleTests
    {
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            DrgGroupingRules.SetDrgGroupingRules(new List<IDrgGroupingRule>()
            {
                new DischargeModeGroupingRule()
            });
        }

        [TestCategory("DischargeMode Grouping Rules")]
        [TestMethod]
        public void When_definition_is_equals_N_and_casevalue_is_X_it_should_succeed()
        {
            var definitions = CreateDefinition("=N");
            var caseFeatures = new CaseFeatures();
            caseFeatures.DischargeMode = DischargeMode.Any;

            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord1", drgLogicResult.Ord);
        }

        [TestCategory("DischargeMode Grouping Rules")]
        [TestMethod]
        public void When_definition_is_equals_N_and_casevalue_is_E_it_should_fail()
        {
            var definitions = CreateDefinition("=N");
            var caseFeatures = new CaseFeatures();
            caseFeatures.DischargeMode = DischargeMode.E;

            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.IsNull(drgLogicResult);
        }

        [TestCategory("DischargeMode Grouping Rules")]
        [TestMethod]
        public void When_definition_is_equals_N_and_casevalue_is_N_it_should_succeed()
        {
            var definitions = CreateDefinition("=N");
            var caseFeatures = new CaseFeatures();
            caseFeatures.DischargeMode = DischargeMode.N;

            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord1", drgLogicResult.Ord);
        }

        [TestCategory("DischargeMode Grouping Rules")]
        [TestMethod]
        public void When_definition_is_minus_H_and_casevalue_is_H_it_should_fail()
        {
            var definitions = CreateDefinition("-H");
            var caseFeatures = new CaseFeatures();
            caseFeatures.DischargeMode = DischargeMode.H;

            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.IsNull(drgLogicResult);
        }

        [TestCategory("DischargeMode Grouping Rules")]
        [TestMethod]
        public void When_definition_is_minus_H_and_casevalue_is_F_it_should_succeed()
        {
            var definitions = CreateDefinition("-H");
            var caseFeatures = new CaseFeatures();
            caseFeatures.DischargeMode = DischargeMode.Any;

            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord1", drgLogicResult.Ord);
        }





        private DefinitionsDataStore CreateDefinition(string disch)
        {
            return new DefinitionsDataStore
            {
                DrgLogicModels = new List<DrgLogic>
                {
                    new DrgLogic(1, "ord1", "drg1", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", disch, "", "")
                }
            };
        }
    }
}