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
    public class ProcedurePropertyGroupingRuleTests
    {
        static DefinitionsDataStore _definitions;

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            _definitions = CreateDefinitions();

            DrgGroupingRules.SetDrgGroupingRules(new List<IDrgGroupingRule>()
            {
                new ProcedurePropertyGroupingRule()
            });
        }

        [TestCategory("ProcedurePropterty Grouping Rules")]
        [TestMethod]
        public void It_matches_when_procpro_and_secpro_is_required()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.ProcedureProperties = new Dictionary<string, List<ProcedureProperty>>
            {
                {"proc2", new List<ProcedureProperty> {new ProcedureProperty("proc2", null)}},
                {"sec2", new List<ProcedureProperty> {new ProcedureProperty("sec2", null)}}
            };

            var drgLogicResult = _definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord3", drgLogicResult.Ord);
        }

        [TestCategory("ProcedurePropterty Grouping Rules")]
        [TestMethod]
        public void It_does_not_match_when_procpro_and_secpro_is_not_found()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.ProcedureProperties = new Dictionary<string, List<ProcedureProperty>>
            {
                {"wrong1", new List<ProcedureProperty> {new ProcedureProperty("wrong1", null)}},
                {"wrong2", new List<ProcedureProperty> {new ProcedureProperty("wrong2", null)}}
            };

            var drgLogicResult = _definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord7", drgLogicResult.Ord);
        }

        [TestCategory("ProcedurePropterty Grouping Rules")]
        [TestMethod]
        public void It_matches_when_procpro_is_found_and_secpro_is_null()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.ProcedureProperties = new Dictionary<string, List<ProcedureProperty>>
            {
                {"proc3", new List<ProcedureProperty> {new ProcedureProperty("proc3", null)}},
            };

            var drgLogicResult = _definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord4", drgLogicResult.Ord);
        }

        [TestCategory("ProcedurePropterty Grouping Rules")]
        [TestMethod]
        public void It_matches_when_secpro_is_found_and_procpro_is_null()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.ProcedureProperties = new Dictionary<string, List<ProcedureProperty>>
            {
                {"sec3", new List<ProcedureProperty> {new ProcedureProperty("sec3", null)}}
            };

            var drgLogicResult = _definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord5", drgLogicResult.Ord);
        }

        [TestCategory("ProcedurePropterty Grouping Rules")]
        [TestMethod]
        public void It_matches_when_secpro_is_found_and_procpro_is_found()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.ProcedureProperties = new Dictionary<string, List<ProcedureProperty>>
            {
                {"proc4", new List<ProcedureProperty> { new ProcedureProperty("proc4", null)}},
                {"sec2", new List<ProcedureProperty> {new ProcedureProperty("sec2", null)}}
            };

            var drgLogicResult = _definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord7", drgLogicResult.Ord);
        }

        [TestCategory("ProcedurePropterty Grouping Rules")]
        [TestMethod]
        public void It_matches_when_no_procedures_are_set()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.ProcedureProperties = new Dictionary<string, List<ProcedureProperty>>();

            var drgLogicResult = _definitions.ApplyDrgGroupingRules(caseFeatures);
            Assert.AreEqual("ord7", drgLogicResult.Ord);
        }

        private static DefinitionsDataStore CreateDefinitions()
        {
            return new DefinitionsDataStore
            {
                DrgLogicModels = new List<DrgLogic>
                {
                    new DrgLogic(1, "ord1", "", "", "", "", "", "", "proc1", "", "", "", "", "", "", "", "", "", "", "", ""),
                    new DrgLogic(2, "ord2", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "sec1", "", "", ""),
                    new DrgLogic(3, "ord3", "", "", "", "", "", "", "proc2", "", "", "", "", "", "", "", "", "sec2", "", "", ""),
                    new DrgLogic(4, "ord4", "", "", "", "", "", "", "proc3", "", "", "", "", "", "", "", "", "", "", "", ""),
                    new DrgLogic(5, "ord5", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "sec3", "", "", ""),
                    new DrgLogic(6, "ord6", "", "", "", "", "", "", "proc4", "", "", "", "", "", "", "", "", "sec4", "", "", ""),
                    new DrgLogic(7, "ord7", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
                }
            };
        }
    }
}
