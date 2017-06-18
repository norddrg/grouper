
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
    public class PrincipalDiagnosisPropertyGroupingRuleTests
    {
        static DefinitionsDataStore _definitions;

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            _definitions = CreateDefinitions();

            DrgGroupingRules.SetDrgGroupingRules(new List<IDrgGroupingRule>()
            {
                new PrincipalDiagnosisPropertyGroupingRule()
            });
        }

        [TestCategory("PrincipalDiagnosticProperty Grouping Rules")]
        [TestMethod]
        public void Match_between_pdg_and_value_expected_positive_operator()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.PrincipalDiagnosisProperties = new Dictionary<string, PrincipalDiagnosisProperty>
            {
                { "00P11", new PrincipalDiagnosisProperty("00P11") }
            };
            
            var drgLogicResult = _definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord1", drgLogicResult.Ord);
        }

        [TestCategory("PrincipalDiagnosticProperty Grouping Rules")]
        [TestMethod]
        public void Match_between_pdg_and_value_without_operator()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.PrincipalDiagnosisProperties = new Dictionary<string, PrincipalDiagnosisProperty>
            {
                { "00P21", new PrincipalDiagnosisProperty("00P21") }
            };

            var drgLogicResult = _definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord2", drgLogicResult.Ord);
        }

        [TestCategory("PrincipalDiagnosticProperty Grouping Rules")]
        [TestMethod]
        public void Match_between_pdg_and_value_expected_blank_operator()
        {
            var caseFeatures = new CaseFeatures();
           
            var drgLogicResult = _definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord3", drgLogicResult.Ord);
        }


        private static DefinitionsDataStore CreateDefinitions()
        {
            return new DefinitionsDataStore
            {
                DrgLogicModels = new List<DrgLogic>
                {
                    new DrgLogic(1, "ord1", "", "", "", "", "+00P11", "", "", "", "", "", "", "", "", "", "", "", "", "", ""),
                    new DrgLogic(2, "ord2", "", "", "", "", "00P21", "", "", "", "", "", "", "", "", "", "", "", "", "", ""),
                    new DrgLogic(3, "ord3", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""),
                    new DrgLogic(4, "ord4", "", "", "", "", "-01P04", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
                }
            };
        }
    }
}