using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using DRG.Core;
using DRG.Core.Definitions;
using DRG.Core.Features;
using DRG.Core.Types;
using DRG.Interfaces;

namespace DRG.SecondaryCaseFeatureRules
{
    public class DiagnosisPropertiesCaseFeatureRule : ICaseFeatureRule
    {
        public void Apply(CaseFeatures caseFeatures, CaseData caseData, DefinitionsDataStore definitions)
        {
            /*
            The folloving rules from the Definition Data table ProcedureFeatureRules are used:
                •	Rules where FeatureType=DGPROP

            The FeatureValue of a given rule is assigned to the Case one or many times, in the following situations:
                •	When any single Code within the Procedures of the Case matches the ProcedureCode1 value of the rule.
            */
            //add all diagnsosis properties.

            foreach (var procedureCode in caseData.ProcedureCodes)
            {
                List<ProcedureDefinition> procedureDefinitions;
                if (definitions.ProcModels_DGPROP.TryGetValue(procedureCode, out procedureDefinitions))
                {
                    foreach (var procedureDefinition in procedureDefinitions)
                    {
                        if (!caseFeatures.DiagnosisProperties.ContainsKey(procedureDefinition.VarVal))
                        {
                            caseFeatures.DiagnosisProperties.Add(procedureDefinition.VarVal,
                                new DiagnosisProperty(procedureDefinition.VarVal, null));
                        }
                    }
                }
            }


            /*;
            The folloving rules from the Definition Data table DiagnosisFeatureRules are used:
                •	Rules where FeatureType=DGPROP

            The FeatureValue of a given rule is assigned to the Case one or many times, in the following situations:
                •	1. When any single Code within the Diagnoses of the Case matches the DiagnosisCode1 value of the rule , and the rule does not contain a value for DiagnosisCode2.
                •	2. When two codes within the Diagnoses of the Case, having CodeNumber 1 and 2, match the DiagnosisCode1 and DiagnosisCode2 values of the rule respectively.
            */

            foreach (var diagnosisPair in caseData.DiagnoseCodes)
            {
                var diagnosisDefinitions = new List<DiagnosisDefinition>();
                var temp1 = new List<DiagnosisDefinition>();

                definitions.DgModels_DGPROP.TryGetValue(diagnosisPair.Code1, out temp1);
                if (temp1 != null)
                    diagnosisDefinitions.AddRange(temp1);

                if (diagnosisPair.IsPair)
                {
                    var temp2 = new List<DiagnosisDefinition>();
                    definitions.DgModels_DGPROP.TryGetValue(diagnosisPair.Code2, out temp2);
                    if (temp2 != null)
                        diagnosisDefinitions.AddRange(temp2);
                }

                foreach (var diagnosisDefinition in diagnosisDefinitions)
                {
                    if (!diagnosisDefinition.HasCode2)
                    {
                        //rule 1.
                        if (diagnosisPair.Code1 == diagnosisDefinition.Code1 || diagnosisPair.Code2 == diagnosisDefinition.Code1)
                        {
                            if (!caseFeatures.DiagnosisProperties.ContainsKey(diagnosisDefinition.VarVal))
                            {
                                var diagnosisProperty = new DiagnosisProperty(diagnosisDefinition.VarVal, diagnosisPair);
                                caseFeatures.DiagnosisProperties.Add(diagnosisDefinition.VarVal, diagnosisProperty);
                            }
                        }
                    }
                    else
                    {
                        //rule 2.
                        if (diagnosisPair.Code1 == diagnosisDefinition.Code2 && diagnosisPair.Code2 == diagnosisDefinition.Code1)
                        {
                            if (!caseFeatures.DiagnosisProperties.ContainsKey(diagnosisDefinition.VarVal))
                            {
                                var diagnosisProperty = new DiagnosisProperty(diagnosisDefinition.VarVal, diagnosisPair);
                                caseFeatures.DiagnosisProperties.Add(diagnosisDefinition.VarVal, diagnosisProperty);
                            }
                        }
                    }
                }

            }
        }
    }
}