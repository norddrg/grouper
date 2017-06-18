using System.Collections.Generic;
using System.Linq;
using DRG.Core.Definitions;
using DRG.Core.Types;
using DRG.Interfaces;
using DRG.SecondaryCaseFeatureRules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DRG.Tests.CaseFeatureRulesTests
{
    [TestClass]
    public class ComplicationPropertyCaseFeatureTests
    {
        static DefinitionsDataStore _definitions;

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            _definitions = new DefinitionsDataStore
            {
                DgModels_COMPL = new Dictionary<string, List<DiagnosisDefinition>>
                {
                    {"ABC123",  new List<DiagnosisDefinition>
                    {
                        new DiagnosisDefinition("ABC123", "", "DGCAT", "00C"),
                        new DiagnosisDefinition("ABC123", "DEF456", "DGCAT", "11C"),
                        new DiagnosisDefinition("ABC123", "ABC123", "DGCAT", "22G")
                    }},
                    {"DEF456",  new List<DiagnosisDefinition> { new DiagnosisDefinition("DEF456", "ABC123", "DGCAT", "44I")}},
                    {"HIJ789",  new List<DiagnosisDefinition>
                    {
                        new DiagnosisDefinition("HIJ789", "ABC123", "DGCAT", "55I"),
                        new DiagnosisDefinition("HIJ789", "", "DGCAT", "66J")
                    }},
                    {"Z413",  new List<DiagnosisDefinition> { new DiagnosisDefinition("Z413", "", "DGCAT", "77J")}}
                },
            };
        }

        [TestCategory("ComplicationProperty CaseFeature Rules")]
        [TestMethod]
        public void ComplicationProperties_are_set_from_diagnosis_number_2_and_above()
        {
            // ComplicationProperty information is established by applying DiagnoseFeatureRules of the Definition Data on all Diagnoses except Diagnosis no. 1 from the Diagnoses list of the Case Data.
            // When any single Code within the Diagnoses of the Case (except Diagnosis no. 1)  matches the DiagnosisCode1 value of the rule, and the rule does not contain a value for DiagnosisCode2.

            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { DiagnoseCodes = new[] { new DiagnosisPair("ABC123", ""), new DiagnosisPair("Z413", ""), new DiagnosisPair("DEF456", "not_found_code") } };
            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>
            {
                new ComplicationPropertyCaseFeatureRule()
            });

            caseFeatures.ApplyCaseFeatureRules(caseData, _definitions);

            Assert.AreEqual("77J", caseFeatures.ComplicationProperties.First().Value.Value);
            Assert.IsFalse(caseFeatures.ComplicationProperties.First().Value.IsActive);
            Assert.AreEqual(2, caseFeatures.ComplicationProperties.First().Value.CcLevel);
            Assert.AreEqual(1, caseFeatures.ComplicationProperties.Count);
        }

        [TestCategory("ComplicationProperty CaseFeature Rules")]
        [TestMethod]
        public void ComplicationProperties_gets_isActive_and_CcLevel_set_by_3rd_char()
        {
            // ComplicationProperty information is established by applying DiagnoseFeatureRules of the Definition Data on all Diagnoses except Diagnosis no. 1 from the Diagnoses list of the Case Data.
            // When any single Code within the Diagnoses of the Case (except Diagnosis no. 1)  matches the DiagnosisCode1 value of the rule, and the rule does not contain a value for DiagnosisCode2.

            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { DiagnoseCodes = new[] { new DiagnosisPair("ABC123", ""), new DiagnosisPair("Z413", "not_found"), new DiagnosisPair("DEF456", "ABC123") } };
            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>
            {
                new ComplicationPropertyCaseFeatureRule()
            });

            caseFeatures.ApplyCaseFeatureRules(caseData, _definitions);

            Assert.AreEqual("77J", caseFeatures.ComplicationProperties.First().Value.Value);
            Assert.IsFalse(caseFeatures.ComplicationProperties.First().Value.IsActive);
            Assert.AreEqual(2, caseFeatures.ComplicationProperties.First().Value.CcLevel);
        }
    }
}
