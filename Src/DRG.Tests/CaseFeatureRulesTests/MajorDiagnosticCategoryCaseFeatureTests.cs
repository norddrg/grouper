using System.Collections.Generic;
using DRG.Core;
using DRG.Core.Features;
using DRG.DRGGroupingRules;
using DRG.Interfaces;
using DRG.SecondaryCaseFeatureRules;
using DRG.TertiaryCaseFeatureRules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DRG.Tests.CaseFeatureRulesTests
{
    [TestClass]
    public class MajorDiagnosticCategoryCaseFeatureTests
    {
        [TestCategory("MajorDiagnosticCategory CaseFeature Rules")]
        [TestMethod]
        public void Case_gets_a_major_diagnostic_category_if_the_case_has_a_diagnosis_category()
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.DiagnosisCategory = new DiagnosisCategory("X2AB11");

            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>()
            {
                new MajorDiagnosticCategoryCaseFeatureRule()
            });
            caseFeatures.ApplyCaseFeatureRules(null, null);
            Assert.AreEqual("X2", caseFeatures.MajorDiagnosticCategory);
        }

        [TestCategory("MajorDiagnosticCategory CaseFeature Rules")]
        [TestMethod]
        public void Case_does_not_get_a_major_diagnostic_category_if_the_case_has_no_diagnosis_category()
        {
            var caseFeatures = new CaseFeatures();

            CaseFeatureRules.SetCaseFeatureRules(new List<ICaseFeatureRule>()
            {
                new MajorDiagnosticCategoryCaseFeatureRule()
            });
            caseFeatures.ApplyCaseFeatureRules(null, null);
            Assert.IsNull(caseFeatures.MajorDiagnosticCategory);
        }
    }
}
