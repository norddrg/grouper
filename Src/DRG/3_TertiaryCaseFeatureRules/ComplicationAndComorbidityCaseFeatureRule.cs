using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using DRG.Core.Definitions;
using DRG.Core.Features;
using DRG.Interfaces;

namespace DRG.TertiaryCaseFeatureRules
{
    public class ComplicationAndComorbidityCaseFeatureRule : ICaseFeatureRule
    {

        public void Apply(CaseFeatures caseFeatures, CaseData caseData, DefinitionsDataStore definitions)
        {
            X_4_4_1(caseFeatures, caseData, definitions);
            X_4_4_2(caseFeatures, caseData, definitions);
            X_4_4_3(caseFeatures, caseData, definitions);
        }

        /*
        Rule for determining ComplicationAndComorbidityLevel
        The ComplicationAndComorbidityLevel of the Case is determined through the following steps:
        •	Inactivation of active ComplicationProperties according to ComplicationExclusionRules
        •	Activation of inactive ComplicationProperties according to ComplicationCategoriesAndInclusionRules
        •	Final determination based on the combined evaluation of ProcedureCCProperty values and active ComplicationProperty values
 
        */

        private void X_4_4_1(CaseFeatures caseFeatures, CaseData caseData, DefinitionsDataStore definitions)
        {
            /*
            4.4.1	 Inactivation of active ComplicationProperties according to ComplicationExclusionRules
            ComplicationExclusionRules (“Compl.excl”) are used to determine which Complication Properties to be inactivated.
            The attribute IsActive of a ComplicationProperty within a Case is set to 0 if there is a ComplicationExclusionRule for the ComplicationProperty in question 
            matching the Diagnosis no. 1 from the diagnoses list of the Case Data. There is a match between the rule and the Diagnosis no.1 in the following situations:
            •	If the rule contains values for both DiagnosisCode1 and DiagnosisCode2,  and these values match Code 1 and 2 of the Diagnosis nr. 1 respectively.
            •	If the rule contains values for only DiagnosisCode1,  and this value matches any Code within the Diagnosis nr. 1 of the Case.
            •	(No match in other situations)
            */

            if (caseData.DiagnoseCodes.Count <= 0) return;
            var diagnosisPairOne = caseData.DiagnoseCodes.First();
            
            foreach (var property in caseFeatures.ComplicationProperties)
            {
                var found = new List<ComplicationExclusionDefinition>();
                definitions.ComplExclModels.TryGetValue(property.Key, out found);

                //var found = definitions.ComplExclModels.Where(x => x.Compl == property.Key).ToList();

                if (found == null) continue;

                //If the rule contains values for both DiagnosisCode1 and DiagnosisCode2,  and these values match Code 1 and 2 of the Diagnosis nr. 1 respectively.
                var complExclModel = found.FirstOrDefault(x =>
                    ! x.HasCode2 &&
                    x.Code1 == diagnosisPairOne.Code2 &&
                    x.Code2 == diagnosisPairOne.Code1);

                // If the rule contains values for only DiagnosisCode1,  and this value matches any Code within the Diagnosis nr. 1 of the Case.
                complExclModel = complExclModel ?? found.FirstOrDefault(
                    x => ! x.HasCode2 &&
                    (x.Code1 == diagnosisPairOne.Code1 || 
                    x.Code1 == diagnosisPairOne.Code2));

                if (complExclModel != null)
                {
                    property.Value.SetInactive();
                }
            }            
        }

        private void X_4_4_2(CaseFeatures caseFeatures, CaseData caseData, DefinitionsDataStore definitions)
        {
            /*
            4.4.2	Activation of inactive ComplicationProperties according to ComplicationCategoriesAndInclusionRules
ComplicationCategoriesAndInclusionRules (“compl. cat.”) are used to determine which Complication Properties to be activated.
The attribute IsActive of a ComplicationProperty within a Case is set to 1 when the following conditions of at least one 
ComplicationCategoriesAndInclusionRule are met:
    1) The “COMPL” value of the rule has “I” or “J” as its 3rd character and matches a ComplicationProperty of the Case.
    2) The “INCLPROP” value of the rule matches a DiagnosisProperty of the Case, and this DiagnosisProperty is not related to Diagnosis nr 1 of the Case.

The CCLevel attribute of a ComplicationProperty that has been activated is reset as follows:
•	3rd character = “I”  CCLevel=1
•	3rd character = “J”  CCLevel=2
•	No change in other situations
            */

            var found = definitions.ComplCatInclModels.FirstOrDefault(x =>
                (x.CompType == ComplicationCategoryType.I ||
                 x.CompType == ComplicationCategoryType.J) &&
                caseFeatures.ComplicationProperties.ContainsKey(x.Compl) &&
                caseFeatures.DiagnosisProperties.ContainsKey(x.InclProp)
            );

            if (found == null) return;

            foreach (var kvp in caseFeatures.DiagnosisProperties.Where(x => x.Key == found.InclProp))
            {
                if (kvp.Value.DiagnosisPair == caseData.DiagnoseCodes.First())
                    return;
            }

            var complicationProperty = caseFeatures.ComplicationProperties[found.Compl];

            if (complicationProperty != null)
            {
                if (complicationProperty.CompType == ComplicationPropertyType.I)
                {
                    complicationProperty.SetActive();
                    complicationProperty.SetCcLevel(1);
                }
                else if (complicationProperty.CompType == ComplicationPropertyType.J)
                {
                    complicationProperty.SetActive();
                    complicationProperty.SetCcLevel(2);
                }
            }
        }

        private void X_4_4_3(CaseFeatures caseFeatures, CaseData caseData, DefinitionsDataStore definitions)
        {
            /*
4.4.3	Final determination based on the combined evaluation of ProcedureCCProperty values and active ComplicationProperty values
The ComplicationAndComorbidityLevel of the Case is 0 (zero) by default. 
If the Case has any Secondary Case features of the types ComplicationProperty or ProcedureCCProperty, 
the ComplicationAndComorbidityLevel of the Case is set to the highest integer value of the following:
•	ProcedureCCProperties of the Case
•	The CCLevel attribute of ComplicationProperties of the Case, for which the attribute IsActive is 1.

            */
            var active = caseFeatures.ComplicationProperties.Values.Where(x => x.IsActive).ToList();
            var complicationPropertyMax = 0;
            if (active.Count > 0)
            {
                complicationPropertyMax = active.Max(x => x.CcLevel);
            }

            var ccMax = 0;
            if (caseFeatures.ProcedureCCProperties.Any())
            {
                ccMax = caseFeatures.ProcedureCCProperties.Max(x => x.Value.Value);
            }

            caseFeatures.ComplicationAndComorbidityLevel = ccMax;

            if (complicationPropertyMax > ccMax)
            {
                caseFeatures.ComplicationAndComorbidityLevel = complicationPropertyMax;
            }
        }        
    }
}