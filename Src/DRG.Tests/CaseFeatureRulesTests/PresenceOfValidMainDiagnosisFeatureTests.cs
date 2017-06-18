using System.Collections.Generic;
using DRG.Core;
using DRG.Core.Features;
using DRG.DRGGroupingRules;
using DRG.Interfaces;
using DRG.TertiaryCaseFeatureRules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DRG.Tests.CaseFeatureRulesTests
{
    [TestClass]
    public class PresenceOfValidMainDiagnosisFeatureTests
    {
        [TestCategory("PresenceOfValidMDC CaseFeature Rules")]
        [TestMethod]
        public void Case_gets_a_presence_of_valid_main_diagnosis_set_to_true_if_the_case_has_a_diagnosis_category()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.DiagnosisCategory = new DiagnosisCategory("X2AB11");

            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>()
            {
                new PresenceOfValidMainDiagnosisFeatureRule()
            });
            caseFeatures.ApplyCaseFeatureRules(null, null);

            Assert.IsTrue(caseFeatures.PresenceOfValidMainDiagnosis);
        }

        [TestCategory("PresenceOfValidMDC CaseFeature Rules")]
        [TestMethod]
        public void Case_gets_a_presence_of_valid_main_diagnosis_set_to_false_if_the_case_has_no_diagnosis_category()
        {
            var caseFeatures = new CaseFeatures();
            Assert.IsFalse(caseFeatures.PresenceOfValidMainDiagnosis);
        }
    }
}
