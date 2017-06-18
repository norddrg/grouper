using System.Collections.Generic;
using System.Linq;
using DRG.Core;
using DRG.Core.Definitions;
using DRG.DRGGroupingRules;
using DRG.Interfaces;
using DRG.SecondaryCaseFeatureRules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DRG.Tests.CaseFeatureRulesTests
{
    [TestClass]
    public class ProcedureORCaseFeatureTests
    {
        [TestCategory("Procedure_OR CaseFeature Rules")]
        [TestMethod]
        public void Valid_procedure_code_gives_a_valid_ProcedureORProperty()
        {
            var definitions = new DefinitionsDataStore
            {
                ProcModels_OR = new Dictionary<string, List<ProcedureDefinition>>
                {
                    {"MCA00", new List<ProcedureDefinition>  { new ProcedureDefinition("MCA00", "", "LLL111") }}
                }
            };

            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { ProcedureCodes = new[] { "MCA00" } };

            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>()
            {
                new ProcedureORPropertyCaseFeatureRule()
            });
            caseFeatures.ApplyCaseFeatureRules(caseData, definitions);


            Assert.AreEqual("LLL111", caseFeatures.ProcedureORProperties.First().Value.Value);
        }

        [TestCategory("Procedure_OR CaseFeature Rules")]
        [TestMethod]
        public void Invalid_procedure_code_gives_no_ProcedureProperties()
        {
            var definitions = new DefinitionsDataStore
            {
                ProcModels_OR = new Dictionary<string, List<ProcedureDefinition>>
                {
                    {"some_code", new List<ProcedureDefinition>  { new ProcedureDefinition("some_code", "", "BUBU") }}
                }
            };

            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { ProcedureCodes = new[] { "invalid" } };
            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>()
            {
                new ProcedureORPropertyCaseFeatureRule()
            });
            caseFeatures.ApplyCaseFeatureRules(caseData, definitions);

            Assert.AreEqual(0, caseFeatures.ProcedureORProperties.Count);
        }
    }
}
