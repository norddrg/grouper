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
    public class DiagnosisCaseFeatureTests
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
                        }
                    },
                    {
                        "Z414",
                        new List<DiagnosisDefinition>
                        {
                            new DiagnosisDefinition("Z414", "", "PROC", "8"),
                        }
                    },
                    {
                        "Z415",
                        new List<DiagnosisDefinition>
                        {
                            new DiagnosisDefinition("Z415", "", "PROC", "14"),
                        }
                    },
                    {
                        "Z416",
                        new List<DiagnosisDefinition>
                        {
                            new DiagnosisDefinition("Z416", "", "PROC", "15"),
                        }
                    }
                },
                DgModels_DGPROP = new Dictionary<string, List<DiagnosisDefinition>>()
                {
                    {
                        "W9191" ,
                        new List<DiagnosisDefinition>
                        {
                            new DiagnosisDefinition("W9191", "", "PROC", "9")
                        }
                    },
                    {
                        "W9291" ,
                        new List<DiagnosisDefinition>
                        {
                            new DiagnosisDefinition("W9291", "S567", "PROC", "10")
                        }
                    }
                },
                ProcModels_DGPROP = new Dictionary<string, List<ProcedureDefinition>>()
                {
                    {
                        "W9191",
                        new List<ProcedureDefinition>
                        { 
                            new ProcedureDefinition("W9191", "", "11"), 
                            new ProcedureDefinition("W9291", "", "12")
                        }
                    }
                }
            };
        }

        [TestCategory("Diagnosis CaseFeature Rules")]
        [TestMethod]
        public void DiagnosePair_is_pair()
        {
            var diagnosePair = new DiagnosisPair("ABC123", "DEF456");
            Assert.IsTrue(diagnosePair.IsPair);
        }

        [TestCategory("Diagnosis CaseFeature Rules")]
        [TestMethod]
        public void DiagnosePair_is_not_pair()
        {
            var diagnosePair = new DiagnosisPair("ABC123", "");
            Assert.IsFalse(diagnosePair.IsPair);
        }

        [TestCategory("Diagnosis CaseFeature Rules")]
        [TestMethod]
        public void Diagnosis_no_1_isPair_and_match_diagnosisCode2_and_diagnosisCode1()
        {
            // If the Diagnosis no.1 of a Case contains two Codes, and Code1 1 and 2 match the DiagnosisCode1 and DiagnosisCode2 values of a rule respectively, this rule is selected.
            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { DiagnoseCodes = new[] { new DiagnosisPair("DEF456", "ABC123")  } };

            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>()
            {
                new DiagnosisCategoryCaseFeatureRule()
            });

            caseFeatures.ApplyCaseFeatureRules(caseData, _definitions);

            Assert.AreEqual("1", caseFeatures.DiagnosisCategory.Value);
        }

        [TestCategory("Diagnosis CaseFeature Rules")]
        [TestMethod]
        public void Diagnosis_no_1_isPair_and_code2_matches_diagnosisCode1()
        {
            // If the Diagnosis no.1 of a Case contains two Codes, and Code1 2 match the DiagnosisCode1 values of a rule, this rule is selected.
            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { DiagnoseCodes = new[] { new DiagnosisPair("invalid", "DEF456") } };
            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>()
            {
                new DiagnosisCategoryCaseFeatureRule()
            });
            caseFeatures.ApplyCaseFeatureRules(caseData, _definitions);

            Assert.AreEqual("4", caseFeatures.DiagnosisCategory.Value);
        }

        [TestCategory("Diagnosis CaseFeature Rules")]
        [TestMethod]
        public void Diagnosis_no_1_isPair_and_code1_matches_diagnosisCode1()
        {
            // If the Diagnosis no.1 of a Case contains two or more Codes, and Code1 1 match the DiagnosisCode1 values of a rule, this rule is selected.
            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { DiagnoseCodes = new[] { new DiagnosisPair("DEF456", "invalid") } };
            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>()
            {
                new DiagnosisCategoryCaseFeatureRule()
            });
            caseFeatures.ApplyCaseFeatureRules(caseData, _definitions);
            Assert.AreEqual("4", caseFeatures.DiagnosisCategory.Value);
        }

        [TestCategory("Diagnosis CaseFeature Rules")]
        [TestMethod]
        public void Diagnosis_no_1_isNotPair_and_code1_matches_diagnosisCode1()
        {
            // If the Diagnosis no.1 of a Case contains just one Code1, and this Code1 match the DiagnosisCode1 values of a rule, this rule is selected.
            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { DiagnoseCodes = new[] { new DiagnosisPair("HIJ789", "") } };
            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>()
            {
                new DiagnosisCategoryCaseFeatureRule()
            });
            caseFeatures.ApplyCaseFeatureRules(caseData, _definitions);
            Assert.AreEqual("5", caseFeatures.DiagnosisCategory.Value);
        }

        [TestCategory("Diagnosis CaseFeature Rules")]
        [TestMethod]
        public void Single_codes_within_the_diagnoses_matches_diagnosisCode1_value_and_no_value_is_set_for_diagnosisCode2()
        {
            //When any single Code1 within the Diagnoses of the Case matches the DiagnosisCode1 value of the rule , and the rule does not contain a value for DiagnosisCode2.

            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { DiagnoseCodes = new[] { new DiagnosisPair("Z413", "Z415") } };
            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>()
            {
                new DiagnosisProcedurePropertiesCaseFeatureRule()
            });
            caseFeatures.ApplyCaseFeatureRules(caseData, _definitions);

            Assert.AreEqual("7", caseFeatures.ProcedureProperties["7"].First().Value);
            Assert.AreEqual("14", caseFeatures.ProcedureProperties["14"].First().Value);
        }

        [TestCategory("Diagnosis CaseFeature Rules")]
        [TestMethod]
        public void Single_code_within_the_diagnoses_matches_diagnosisCode1_value_and_no_value_is_set_for_diagnosisCode2()
        {
            //When any single Code1 within the Diagnoses of the Case matches the DiagnosisCode1 value of the rule , and the rule does not contain a value for DiagnosisCode2.

            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { DiagnoseCodes = new[] { new DiagnosisPair("Z413", "") } };
            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>()
            {
                new DiagnosisProcedurePropertiesCaseFeatureRule()
            });
            caseFeatures.ApplyCaseFeatureRules(caseData, _definitions);

            Assert.AreEqual("7", caseFeatures.ProcedureProperties["7"].First().Value);
        }

        [TestCategory("Diagnosis CaseFeature Rules")]
        [TestMethod]
        public void Two_codes_within_the_diagnoses_having_codeNumber1_and_codeNumber2_matches_diagnosisCode1__and_diagnosisCode2_values()
        {
            // When two codes within the Diagnoses of the Case, having CodeNumber 1 and 2, match the DiagnosisCode1 and DiagnosisCode2 values of the rule respectively.

            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { DiagnoseCodes = new[] { new DiagnosisPair("Z414", "Q1234") } };
            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>()
            {
                new DiagnosisProcedurePropertiesCaseFeatureRule()
            });
            caseFeatures.ApplyCaseFeatureRules(caseData, _definitions);

            Assert.AreEqual("8", caseFeatures.ProcedureProperties["8"].First().Value);
        }

        [TestCategory("Diagnosis CaseFeature Rules")]
        [TestMethod]
        public void Single_code_within_the_procedures_matches_dgprop_diagnosisCode1_value_and_no_value_is_set_for_diagnosisCode2()
        {
            // When any single Code within the Procedures of the Case matches the ProcedureCode1 value of the rule.

            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { ProcedureCodes = new[] { "W9191" }, DiagnoseCodes = new List<DiagnosisPair>() };
            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>()
            {
                new DiagnosisPropertiesCaseFeatureRule()
            });
            caseFeatures.ApplyCaseFeatureRules(caseData, _definitions);

            Assert.AreEqual("11", caseFeatures.DiagnosisProperties.First().Value.Value);
        }

        [TestCategory("Diagnosis CaseFeature Rules")]
        [TestMethod]
        public void Single_code_within_the_diagnoses_matches_dgprop_diagnosisCode1_value_and_no_value_is_set_for_diagnosisCode2()
        {
            // When any single Code within the Diagnoses of the Case matches the DiagnosisCode1 value of the rule , and the rule does not contain a value for DiagnosisCode2.

            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { DiagnoseCodes = new[] { new DiagnosisPair("W9191", "") }, ProcedureCodes = new List<string>() };
            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>()
            {
                new DiagnosisPropertiesCaseFeatureRule()
            });
            caseFeatures.ApplyCaseFeatureRules(caseData, _definitions);

            Assert.AreEqual("9", caseFeatures.DiagnosisProperties.First().Value.Value);
            Assert.AreEqual(1, caseFeatures.DiagnosisProperties.Count);
        }

        [TestCategory("Diagnosis CaseFeature Rules")]
        [TestMethod]
        public void Two_codes_within_the_diagnoses_having_codeNumber1_and_codeNumber2_matches_dgprop_diagnosisCode2__and_diagnosisCode1_values()
        {
            // When two codes within the Diagnoses of the Case, having CodeNumber 1 and 2, match the DiagnosisCode2 and DiagnosisCode1 values of the rule respectively.

            var caseFeatures = new CaseFeatures();
            var caseData = new CaseData { DiagnoseCodes = new[] { new DiagnosisPair("S567", "W9291") }, ProcedureCodes = new List<string>() };
            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>()
            {
                new DiagnosisPropertiesCaseFeatureRule()
            });
            caseFeatures.ApplyCaseFeatureRules(caseData, _definitions);

            Assert.AreEqual("10", caseFeatures.DiagnosisProperties.First().Value.Value);
            Assert.AreEqual(1, caseFeatures.DiagnosisProperties.Count);
        }
    }
}
