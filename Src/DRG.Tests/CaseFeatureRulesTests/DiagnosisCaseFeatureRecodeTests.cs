using System.Collections.Generic;
using DRG.Core.Definitions;
using DRG.Core.Types;
using DRG.Interfaces;
using DRG.SecondaryCaseFeatureRules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DRG.Tests.CaseFeatureRulesTests
{
    [TestClass]
    public class DiagnosisCaseFeatureRecodeTests
    {
        static DefinitionsDataStore _definitions;

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            _definitions = new DefinitionsDataStore
            {
                DgModels_DGCAT = new Dictionary<string, List<DiagnosisDefinition>>
                {
                    {
                        "abc123",
                        new List<DiagnosisDefinition>
                        {
                            new DiagnosisDefinition("abc123", "", "DGCAT", "98abcd")
                        }
                    }
                }
            };
        }

        [TestCategory("DiagnosisRecode CaseFeature Rules")]
        [TestMethod]
        public void Diagnosis_category_starting_with_98_should_be_recoded_to_starting_with_12_if_gender_is_male()
        {
            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { DiagnoseCodes = new[] { new DiagnosisPair("abc123", "") } };
            caseFeatures.Gender = Gender.Male;
            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>()
            {
                new DiagnosisCategoryCaseFeatureRule(),
                new RecodeMainDiagnosisByGenderCaseFeatureRule()
            });

            caseFeatures.ApplyCaseFeatureRules(caseData, _definitions);

            Assert.AreEqual("12abcd", caseFeatures.DiagnosisCategory.Value);
        }

        [TestCategory("DiagnosisRecode CaseFeature Rules")]
        [TestMethod]
        public void Diagnosis_category_starting_with_98_should_be_recoded_to_starting_with_13_if_gender_is_female()
        {
            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { DiagnoseCodes = new[] { new DiagnosisPair("abc123", "") } };
            caseFeatures.Gender = Gender.Female;

            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>()
            {
                new DiagnosisCategoryCaseFeatureRule(),
                new RecodeMainDiagnosisByGenderCaseFeatureRule()
            });
            caseFeatures.ApplyCaseFeatureRules(caseData, _definitions); 

            Assert.AreEqual("13abcd", caseFeatures.DiagnosisCategory.Value);
        }

        [TestCategory("DiagnosisRecode CaseFeature Rules")]
        [TestMethod]
        public void Diagnosis_category_starting_with_98_should_not_be_recoded_if_gender_is_not_female()
        {
            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { DiagnoseCodes = new[] { new DiagnosisPair("abc123", "") } };
            caseFeatures.Gender = Gender.Null;

            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>()
            {
                new DiagnosisCategoryCaseFeatureRule(),
                new RecodeMainDiagnosisByGenderCaseFeatureRule()
            });
            caseFeatures.ApplyCaseFeatureRules(caseData, _definitions);

            Assert.AreEqual("98abcd", caseFeatures.DiagnosisCategory.Value);
        }

        [TestCategory("DiagnosisRecode CaseFeature Rules")]
        [TestMethod]
        public void Diagnosis_category_starting_with_98_should_not_be_recoded_if_gender_is_not_male()
        {
            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { DiagnoseCodes = new[] { new DiagnosisPair("abc123", "") } };
            caseFeatures.Gender = Gender.Null;

            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>()
            {
                new DiagnosisCategoryCaseFeatureRule(),
                new RecodeMainDiagnosisByGenderCaseFeatureRule()
            });
            caseFeatures.ApplyCaseFeatureRules(caseData, _definitions);
            Assert.AreEqual("98abcd", caseFeatures.DiagnosisCategory.Value);
        }
    }
}
