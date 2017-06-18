using DRG.Core;
using DRG.Core.Definitions;
using DRG.Core.Features;
using DRG.Core.Types;
using DRG.Interfaces;

namespace DRG.PrimaryCaseFeatureRules
{
    public class PrimaryCaseFeatureRule : ICaseFeatureRule
    {
        public void Apply(CaseFeatures caseFeatures, CaseData caseData, DefinitionsDataStore data)
        {
            int age, lengthOfStay;

            int.TryParse(caseData.Age, out age);
            int.TryParse(caseData.LengthOfStay, out lengthOfStay);

            caseFeatures.Gender = Gender.Null;
            if (caseData.Sex == "1")
                caseFeatures.Gender = Gender.Male;
            else if (caseData.Sex == "2")
                caseFeatures.Gender = Gender.Female;

            caseFeatures.Age = age;
            caseFeatures.Duration = lengthOfStay;
            caseFeatures.DischargeMode = DischargeMode.Null;

            switch (caseData.DischargeMode)
            {
                case "E":
                    caseFeatures.DischargeMode = DischargeMode.E;
                    break;
                case "N":
                    caseFeatures.DischargeMode = DischargeMode.N;
                    break;
                case "H":
                    caseFeatures.DischargeMode = DischargeMode.H;
                    break;
                default:
                    caseFeatures.DischargeMode = DischargeMode.Any;
                    break;
            }
        }
    }
}