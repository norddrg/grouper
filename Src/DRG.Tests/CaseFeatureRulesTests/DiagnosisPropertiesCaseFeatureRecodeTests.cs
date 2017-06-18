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
    public class DiagnosisPropertiesCaseFeatureRecodeTests
    {
        static DefinitionsDataStore _definitions;

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            _definitions = new DefinitionsDataStore
            {
                DgModels_DGPROP = new Dictionary<string, List<DiagnosisDefinition>>()
                {
                    { "abc123" , new List<DiagnosisDefinition>()
                        { new DiagnosisDefinition("abc123", "", "DGPROP", "9891") }},
                    { "def456" , new List<DiagnosisDefinition>()
                        { new DiagnosisDefinition("def456", "S567", "DGPROP", "9800") }}

                },
            };

            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>()
            {
                new DiagnosisPropertiesCaseFeatureRule(),
                new RecodeDiagnosisPropertiesByGenderCaseFeatureRule()
            });
        }

        [TestCategory("DiagnosisProperties Recode CaseFeature Rules")]
        [TestMethod]
        public void DiagnosisProperty_starting_with_98_should_be_recoded_to_starting_with_12_if_gender_is_male()
        {
            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { DiagnoseCodes = new[] { new DiagnosisPair("abc123", "") } };
            caseFeatures.Gender = Gender.Male;

            caseFeatures.ApplyCaseFeatureRules(caseData, _definitions);

            Assert.AreEqual("1291", caseFeatures.DiagnosisProperties.First().Value.Value);
        }

        [TestCategory("DiagnosisProperties Recode CaseFeature Rules")]
        [TestMethod]
        public void DiagnosisProperty_starting_with_98_should_be_recoded_to_starting_with_13_if_gender_is_female()
        {
            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { DiagnoseCodes = new[] { new DiagnosisPair("abc123", "") } };
            caseFeatures.Gender = Gender.Female;

            caseFeatures.ApplyCaseFeatureRules(caseData, _definitions);

            Assert.AreEqual("1391", caseFeatures.DiagnosisProperties.First().Value.Value);
        }

        [TestCategory("DiagnosisProperties Recode CaseFeature Rules")]
        [TestMethod]
        public void DiagnosisProperty_starting_with_98_should_not_be_recoded_if_gender_is_not_female()
        {
            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { DiagnoseCodes = new[] { new DiagnosisPair("abc123", "") } };
            caseFeatures.Gender = Gender.Null;

            caseFeatures.ApplyCaseFeatureRules(caseData, _definitions);

            Assert.AreEqual("9891", caseFeatures.DiagnosisProperties.First().Value.Value);
        }

        [TestCategory("DiagnosisProperties Recode CaseFeature Rules")]
        [TestMethod]
        public void DiagnosisProperty_starting_with_98_should_not_be_recoded_if_gender_is_not_male()
        {
            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { DiagnoseCodes = new[] { new DiagnosisPair("abc123", "") } };
            caseFeatures.Gender = Gender.Null;
            
            caseFeatures.ApplyCaseFeatureRules(caseData, _definitions);
            Assert.AreEqual("9891", caseFeatures.DiagnosisProperties.First().Value.Value);
        }
    }
}
