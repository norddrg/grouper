using System.Collections.Generic;
using System.Linq;
using DRG.Core.Definitions;
using DRG.Core.Types;
using DRG.Interfaces;
using DRG.SecondaryCaseFeatureRules;
using DRG.TertiaryCaseFeatureRules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DRG.Tests.CaseFeatureRulesTests
{
    [TestClass]
    public class ComplicationAndComorbidityCaseFeatureTests
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
                        new DiagnosisDefinition("ABC123", "", "", "00C"),
                        new DiagnosisDefinition("ABC123", "DEF456", "", "11C"),
                        new DiagnosisDefinition("ABC123", "ABC123", "", "22G")
                    }},
                    {"DEF456",  new List<DiagnosisDefinition> { new DiagnosisDefinition("DEF456", "ABC123", "", "44I")}},
                    {"HIJ789",  new List<DiagnosisDefinition>
                    {
                        new DiagnosisDefinition("HIJ789", "ABC123", "", "55I"),
                        new DiagnosisDefinition("HIJ789", "", "", "66J")
                    }},
                    {"Z413",  new List<DiagnosisDefinition> { new DiagnosisDefinition("Z413", "", "", "77J")}}
                },
                ComplExclModels = new Dictionary<string, List<ComplicationExclusionDefinition>>
                {
                    {"11C",  new List<ComplicationExclusionDefinition> {
                        new ComplicationExclusionDefinition("11C", "ABC123", "DEF456")}}
                },
                ComplCatInclModels = new List<ComplicationCategoryDefinition>
                {
                    new ComplicationCategoryDefinition("00C", "inclProp")
                },
            };

            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>
            {
                new ComplicationPropertyCaseFeatureRule(),
                new ComplicationAndComorbidityCaseFeatureRule()
            });
        }

        [TestCategory("ComplicationAndComorbidity CaseFeature Rules")]
        [TestMethod]
        public void It_should_inactivate_complication_properties_1()
        {
            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { DiagnoseCodes = new[]
            {
                new DiagnosisPair("ABC123", "")
            }};          

            caseFeatures.ApplyCaseFeatureRules(caseData, _definitions);

            Assert.AreEqual(0, caseFeatures.ComplicationProperties.Count);
        }
    }
}
