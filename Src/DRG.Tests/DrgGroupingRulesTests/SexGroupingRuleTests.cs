using System.Collections.Generic;
using DRG.Core.Definitions;
using DRG.Core.Drg;
using DRG.Core.Types;
using DRG.DRGGroupingRules;
using DRG.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DRG.Tests.DrgGroupingRulesTests
{
    [TestClass]
    public class SexGroupingRuleTests
    {
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            DrgGroupingRules.SetDrgGroupingRules(new List<IDrgGroupingRule>()
            {
                new SexGroupingRule()
            });
        }

        [TestCategory("Sex Grouping Rules")]
        [TestMethod]
        public void It_matches_when_definition_sex_is_equal_to_input_sex_male()
        {
            var definitions = CreateDefinition("M");
            var caseFeatures = new CaseFeatures();
            caseFeatures.Gender = Gender.Male;

            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord1", drgLogicResult.Ord);
        }

        [TestCategory("Sex Grouping Rules")]
        [TestMethod]
        public void It_matches_when_definition_sex_is_equal_to_input_sex_female()
        {
            var definitions = CreateDefinition("F");
            var caseFeatures = new CaseFeatures();
            caseFeatures.Gender = Gender.Female;

            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord1", drgLogicResult.Ord);
        }

        [TestCategory("Sex Grouping Rules")]
        [TestMethod]
        public void It_does_not_match_when_definition_sex_is_different_than_input_sex_male()
        {
            var definitions = CreateDefinition("M");
            var caseFeatures = new CaseFeatures();
            caseFeatures.Gender = Gender.Female;

            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.IsNull(drgLogicResult);
        }

        [TestCategory("Sex Grouping Rules")]
        [TestMethod]
        public void It_matches_when_definition_sex_is_minus_and_input_sex_is_null()
        {
            var definitions = CreateDefinition("-");
            var caseFeatures = new CaseFeatures();
            caseFeatures.Gender = Gender.Null;

            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord1", drgLogicResult.Ord);
        }

        [TestCategory("Sex Grouping Rules")]
        [TestMethod]
        public void It_does_not_match_when_definition_sex_is_minus_and_input_sex_is_not_null()
        {
            var definitions = CreateDefinition("-");
            var caseFeatures = new CaseFeatures();
            caseFeatures.Gender = Gender.Male;

            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.IsNull(drgLogicResult);
        }


        // Handy test helper
        private DefinitionsDataStore CreateDefinition(string sex)
        {
            return new DefinitionsDataStore
            {
                DrgLogicModels = new List<DrgLogic>
                {
                    new DrgLogic(1, "ord1", "drg1", "", "", "", "", "", "", "", "", "", sex, "", "", "", "", "", "", "", "")
                }
            };
        }
    }
}