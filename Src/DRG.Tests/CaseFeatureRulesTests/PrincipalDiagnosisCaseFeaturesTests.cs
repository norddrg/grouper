using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DRG.Core;
using DRG.Core.Definitions;
using DRG.Core.Features;
using DRG.Core.Types;
using DRG.DRGGroupingRules;
using DRG.Interfaces;
using DRG.SecondaryCaseFeatureRules;
using DRG.TertiaryCaseFeatureRules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DRG.Tests.CaseFeatureRulesTests
{
    [TestClass]
    public class PrincipalDiagnosisCaseFeaturesTests
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
                        "ABC123",
                        new List<DiagnosisDefinition>
                        {
                            new DiagnosisDefinition("ABC123", "", "DGCAT", "0"),
                            new DiagnosisDefinition("ABC123", "DEF456", "DGCAT", "1"),
                            new DiagnosisDefinition("ABC123", "ABC123", "DGCAT", "2")
                        }
                    },
                    {
                        "DEF456",
                        new List<DiagnosisDefinition>
                        {
                            new DiagnosisDefinition("DEF456", "ABC123", "DGCAT", "4"),
                        }
                    },
                    {
                        "HIJ789",
                        new List<DiagnosisDefinition>
                        {
                            new DiagnosisDefinition("HIJ789", "ABC123", "DGCAT", "5"),
                            new DiagnosisDefinition("HIJ789", "", "DGCAT", "6")
                        }
                    }
                },
                DgModels_PROCPR = new Dictionary<string, List<DiagnosisDefinition>>
                {
                    {
                        "Z413",
                        new List<DiagnosisDefinition>
                        {
                            new DiagnosisDefinition("Z413", "", "PROC", "7"),
                            new DiagnosisDefinition("Z413", "Q1234", "PROC", "8")
                        }
                    }
                },
                DgModels_PDGPRO = new Dictionary<string, List<DiagnosisDefinition>>
                {
                    {
                        "XY21",
                        new List<DiagnosisDefinition>
                        {
                            new DiagnosisDefinition("XY21", "", "PDGPRO", "9"),
                        }
                    },
                    {
                        "Q9876",
                        new List<DiagnosisDefinition>
                        {
                            new DiagnosisDefinition("Q9876", "XY21", "PDGPRO", "10")
                        }
                    }
                }
            };
        }

        [TestCategory("PrincipalDiagnosis CaseFeature Rules")]
        [TestMethod]
        public void Any_single_code_within_diagnose_no_1_of_the_case_matches_diagnosiscode1_value_and_no_value_is_set_for_diagnosiscode2()
        {
            // When any single Code within Diagnosis no. 1 of the Case  matches the DiagnosisCode1 value of the rule, and the rule does not contain a value for DiagnosisCode2. 

            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { DiagnoseCodes = new[] { new DiagnosisPair("XY21", "") } };

            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>()
            {
                new PrincipalDiagnosisPropertyCaseFeatureRule()
            });
            caseFeatures.ApplyCaseFeatureRules(caseData, _definitions);

            Assert.AreEqual(1, caseFeatures.PrincipalDiagnosisProperties.Count);
            Assert.AreEqual("9", caseFeatures.PrincipalDiagnosisProperties.First().Value.Value);
        }

        [TestCategory("PrincipalDiagnosis CaseFeature Rules")]
        [TestMethod]
        public void Two_codes_within_diagnosis_no_1_of_the_case_having_codenumber1_and_2_match_diagnosiscode2_and_diagnosiscode1()
        {
            //When two codes within Diagnosis no. 1 of the Case, having CodeNumber 1 and 2, match the DiagnosisCode1 and DiagnosisCode2 values of the rule respectively.

            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { DiagnoseCodes = new[] { new DiagnosisPair("XY21", "Q9876") } };
            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>
            {
                new PrincipalDiagnosisPropertyCaseFeatureRule()
            });
            caseFeatures.ApplyCaseFeatureRules(caseData, _definitions);

            Assert.AreEqual("9", caseFeatures.PrincipalDiagnosisProperties.First().Value.Value);
        }
    }
}
