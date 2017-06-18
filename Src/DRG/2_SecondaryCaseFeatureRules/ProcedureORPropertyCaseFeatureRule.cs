using System.Collections.Generic;
using System.Linq;
using DRG.Core.Definitions;
using DRG.Core.Features;
using DRG.Interfaces;

namespace DRG.SecondaryCaseFeatureRules
{
    public class ProcedureORPropertyCaseFeatureRule : ICaseFeatureRule
    {

        /*
            ProcedureORProperty information is obtained by the following mechanisms:

            1) By applying ProcedureFeatureRules of the Definition Data on all Procedure information of the Case Data.
            2) By applying DiagnoseFeatureRules of the Definition Data on all Diagnosis information of the Case Data

            1
            ==
            The folloving rules from the Definition Data table ProcedureFeatureRules are used:
            - Rules where FeatureType=OR

            The FeatureValue of a given rule is assigned to the Case one or many times, in the following situations:
            - When any single Code within the Procedures of the Case matches the ProcedureCode1 value of the rule.


            2
            ==
            The folloving rules from the Definition Data table DiagnosisFeatureRules are used:
            - Rules where FeatureType=OR

            The FeatureValue of a given rule is assigned to the Case one or many times, in the following situations:
            - When any single Code within the Diagnoses of the Case matches the DiagnosisCode1 value of the rule, 
            and the rule does not contain a value for DiagnosisCode2.
        */

        public void Apply(CaseFeatures caseFeatures, CaseData caseData, DefinitionsDataStore definitions)
        {
            // 1
            foreach (var procedureCode in caseData.ProcedureCodes)
            {
                List<ProcedureDefinition> found;
                definitions.ProcModels_OR.TryGetValue(procedureCode, out found);

                if (found != null)
                {
                    foreach (var procedureDefinition in found)
                    {
                        if (!caseFeatures.ProcedureORProperties.ContainsKey(procedureDefinition.VarVal))
                        {
                            var p = new ProcedureORProperty(procedureDefinition.VarVal, procedureCode);
                            caseFeatures.ProcedureORProperties.Add(procedureDefinition.VarVal, p);
                        }
                    }
                }
            }

            


            // 2
            var diagnosisDefinitions = new List<DiagnosisDefinition>();

            foreach (var diagnosisPair in caseData.DiagnoseCodes)
            {
                var temp1 = new List<DiagnosisDefinition>();
                definitions.DgModels_OR.TryGetValue(diagnosisPair.Code1, out temp1);
                if (temp1 != null)
                    diagnosisDefinitions.AddRange(temp1.Where(x => ! x.HasCode2));

                if (diagnosisPair.IsPair)
                {
                    var temp2 = new List<DiagnosisDefinition>();
                    definitions.DgModels_OR.TryGetValue(diagnosisPair.Code2, out temp2);
                    if (temp2 != null)
                        diagnosisDefinitions.AddRange(temp2.Where(x => ! x.HasCode2));
                }
            }

            foreach (var diagnosisDefinition in diagnosisDefinitions)
            {
                if (!caseFeatures.ProcedureORProperties.ContainsKey(diagnosisDefinition.VarVal))
                {
                    var p = new ProcedureORProperty(diagnosisDefinition.VarVal, null);
                    caseFeatures.ProcedureORProperties.Add(diagnosisDefinition.VarVal, p);
                }
            }
        }
    }
}