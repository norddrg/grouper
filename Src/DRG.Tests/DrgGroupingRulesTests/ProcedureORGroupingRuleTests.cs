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
    public class ProcedureORGroupingRuleTests
    {
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            DrgGroupingRules.SetDrgGroupingRules(new List<IDrgGroupingRule>()
            {
                new ProcedureORPropertyGroupingRule()
            });
        }

        /*
        1
        Blank (NULL)  or “+”	
        “S”	
        The Case must have at least one ProcedureORProperty with a value equal to 1	Frequent in today’s rules
        
        2
        Blank (NULL)  or “+”	
        “P”	
        The Case must have at least one ProcedureORProperty with a value equal to 1 or 2	Frequent in today’s rules
        
        3
        Blank (NULL)  or “+”	
        “N”	
        The Case can not  have any ProcedureORProperty with a value equal to 1	Frequent in today’s rules
        
        4
        Blank (NULL)  or “+”	
        “Z”	
        The Case can not  have any ProcedureORProperty with a value equal to 1 or 2	Frequent in today’s rules
        
        5
        “+”	
        Blank (NULL)	
        The Case must have at least one ProcedureORProperty, regardless of value	
        Same result as the value “P” with today’s configurations
        
        6
        “-”	
        Blank (NULL)	
        The Case can not have any ProcedureORProperty, regardless of value	
        Same result as the value “Z” with today’s configurations
        
        7
        Blank (NULL)	
        Blank (NULL)	
        The criterion is always met	Frequent in today’s rules
        
        8
        “-”	
        Any value (not NULL)	
        The criterion is never met	

        */

        [TestCategory("ProcedureORPropterty Grouping Rules")]
        [TestMethod]
        public void When_definition_is_plus_S_and_property_is_1_it_is_expected_to_succeed()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.ProcedureORProperties = new Dictionary<string, ProcedureORProperty>
            {
                {"1", new ProcedureORProperty("1", null)},
            };

            var definitions = CreateDefinitions("+S");
            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord", drgLogicResult.Ord);
        }

        [TestCategory("ProcedureORPropterty Grouping Rules")]
        [TestMethod]
        public void When_definition_is_S_and_property_is_1_it_is_expected_to_succeed()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.ProcedureORProperties = new Dictionary<string, ProcedureORProperty>
            {
                {"1", new ProcedureORProperty("1", null)},
            };

            var definitions = CreateDefinitions("S");
            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord", drgLogicResult.Ord);
        }

        [TestCategory("ProcedureORPropterty Grouping Rules")]
        [TestMethod]
        public void When_definition_is_plus_P_and_property_is_1_it_is_expected_to_succeed()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.ProcedureORProperties = new Dictionary<string, ProcedureORProperty>
            {
                {"1", new ProcedureORProperty("1", null)},
            };

            var definitions = CreateDefinitions("+P");
            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord", drgLogicResult.Ord);
        }

        [TestCategory("ProcedureORPropterty Grouping Rules")]
        [TestMethod]
        public void When_definition_is_plus_P_and_property_is_2_it_is_expected_to_succeed()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.ProcedureORProperties = new Dictionary<string, ProcedureORProperty>
            {
                {"2", new ProcedureORProperty("2", null)},
            };

            var definitions = CreateDefinitions("+P");
            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord", drgLogicResult.Ord);
        }

        [TestCategory("ProcedureORPropterty Grouping Rules")]
        [TestMethod]
        public void When_definition_is_plus_N_and_property_is_1_it_is_expected_to_fail()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.ProcedureORProperties = new Dictionary<string, ProcedureORProperty>
            {
                {"1", new ProcedureORProperty("1", null)},
            };

            var definitions = CreateDefinitions("+N");
            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.IsNull(drgLogicResult);
        }

        [TestCategory("ProcedureORPropterty Grouping Rules")]
        [TestMethod]
        public void When_definition_is_plus_Z_and_property_is_2_it_is_expected_to_fail()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.ProcedureORProperties = new Dictionary<string, ProcedureORProperty>
            {
                {"1", new ProcedureORProperty("1", null)},
            };

            var definitions = CreateDefinitions("+Z");
            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.IsNull(drgLogicResult);
        }

        [TestCategory("ProcedureORPropterty Grouping Rules")]
        [TestMethod]
        public void When_definition_is_plus_N_and_property_is_3_it_is_expected_to_succeed()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.ProcedureORProperties = new Dictionary<string, ProcedureORProperty>
            {
                {"3", new ProcedureORProperty("3", null)},
            };

            var definitions = CreateDefinitions("+N");
            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord", drgLogicResult.Ord);
        }

        [TestCategory("ProcedureORPropterty Grouping Rules")]
        [TestMethod]
        public void When_definition_is_plus_Z_and_property_is_3_it_is_expected_to_succeed()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.ProcedureORProperties = new Dictionary<string, ProcedureORProperty>
            {
                {"3", new ProcedureORProperty("3", null)},
            };

            var definitions = CreateDefinitions("+Z");
            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord", drgLogicResult.Ord);
        }

        [TestCategory("ProcedureORPropterty Grouping Rules")]
        [TestMethod]
        public void When_definition_is_plus_and_property_is_100_it_is_expected_to_succeed()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.ProcedureORProperties = new Dictionary<string, ProcedureORProperty>
            {
                {"100", new ProcedureORProperty("100", null)},
            };

            var definitions = CreateDefinitions("+");
            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord", drgLogicResult.Ord);
        }

        [TestCategory("ProcedureORPropterty Grouping Rules")]
        [TestMethod]
        public void When_definition_is_plus_and_property_is_null_it_is_expected_to_fail()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.ProcedureORProperties = new Dictionary<string, ProcedureORProperty>();

            var definitions = CreateDefinitions("+");
            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.IsNull(drgLogicResult);
        }

        [TestCategory("ProcedureORPropterty Grouping Rules")]
        [TestMethod]
        public void When_definition_is_minus_and_property_is_null_it_is_expected_to_succeed()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.ProcedureORProperties = new Dictionary<string, ProcedureORProperty>();

            var definitions = CreateDefinitions("-");
            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord", drgLogicResult.Ord);
        }

        [TestCategory("ProcedureORPropterty Grouping Rules")]
        [TestMethod]
        public void When_definition_is_null_and_property_is_null_it_is_expected_to_succeed()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.ProcedureORProperties = new Dictionary<string, ProcedureORProperty>();

            var definitions = CreateDefinitions("");
            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord", drgLogicResult.Ord);
        }

        [TestCategory("ProcedureORPropterty Grouping Rules")]
        [TestMethod]
        public void When_definition_is_minus_and_property_is_anything_it_is_expected_to_fail()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.ProcedureORProperties = new Dictionary<string, ProcedureORProperty>
            {
                {"100", new ProcedureORProperty("100", null)},
            };

            var definitions = CreateDefinitions("-");
            var drgLogicResult = definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.IsNull(drgLogicResult);
        }

        private static DefinitionsDataStore CreateDefinitions(string orprop)
        {
            return new DefinitionsDataStore
            {
                DrgLogicModels = new List<DrgLogic>
                {
                    new DrgLogic(1, "ord", "", "", "", "", "", orprop, "", "", "", "", "", "", "", "", "", "", "", "", "")
                }
            };
        }
    }
}