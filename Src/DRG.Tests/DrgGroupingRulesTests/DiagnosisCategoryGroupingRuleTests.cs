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
    public class DiagnosisGroupingRuleTests
    {
        static DefinitionsDataStore _definitions;

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            _definitions = CreateDefinitions();

            DrgGroupingRules.SetDrgGroupingRules(new List<IDrgGroupingRule>()
            {
                new DiagnosisCategoryGroupingRule()
            });
        }

        /*

        // 1
        Blank (NULL)	
        Any string value (Non blank)
        Match between the DiagnosisCategory code of the Case and the Value of the Diagnosis Category Criterion	Frequent in today’s rules

        // 2
        “+”	(Plus sign)
        Any string value (Non blank)
        Match between the DiagnosisCategory code of the Case and the Value of the Diagnosis Category Criterion	Frequent in today’s rules
        
        // 3
        Blank (NULL)
        Blank (NULL)	
        The criterion is always met
        
        // 4
        “-“ (Minus sign)
        Any string value (Non blank)	
        The criterion is fullfilled when the DiagnosisCategory code of the Case does not match the Value of the Diagnosis Category Criterion	

        */

        [TestCategory("DiagnosisCategory Grouping Rules")]
        [TestMethod]
        public void Operator_is_blank_and_value_is_blank_expects_a_match_with_the_input_value()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.DiagnosisCategory = new DiagnosisCategory("dgcat3");

            var drgLogicResult = _definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord3", drgLogicResult.Ord);
        }

        [TestCategory("DiagnosisCategory Grouping Rules")]
        [TestMethod]
        public void Operator_is_plus_and_value_is_blank_expects_a_match_with_the_input_value()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.DiagnosisCategory = new DiagnosisCategory("dgcat2");

            var drgLogicResult = _definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord2", drgLogicResult.Ord);
        }

        [TestCategory("DiagnosisCategory Grouping Rules")]
        [TestMethod]
        public void Operator_is_minus_and_value_is_blank_expects_a_non_match_with_the_input_value()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.DiagnosisCategory = new DiagnosisCategory("dgcat5");

            var drgLogicResult = _definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord4", drgLogicResult.Ord);
        }

        private static DefinitionsDataStore CreateDefinitions()
        {
            return new DefinitionsDataStore
            {
                DrgLogicModels = new List<DrgLogic>
                {
                    new DrgLogic(1, "ord1", "", "", "", "", "", "", "", "+dgcat1", "", "", "", "", "", "", "", "", "", "", ""),
                    new DrgLogic(2, "ord2", "", "", "", "", "", "", "", "+dgcat2", "", "", "", "", "", "", "", "", "", "", ""),
                    new DrgLogic(3, "ord3", "", "", "", "", "", "", "", "dgcat3", "", "", "", "", "", "", "", "", "", "", ""),
                    new DrgLogic(4, "ord4", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""),
                    new DrgLogic(5, "ord4", "", "", "", "", "", "", "", "-dgcat4", "", "", "", "", "", "", "", "", "", "", "")
                }
            };
        }
    }
}
