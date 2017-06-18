using System.Collections.Generic;
using DRG.Core.Definitions;
using DRG.Core.Features;
using DRG.Core.Types;

namespace DRG
{
    /*
        Sex	1
        Age	1
        Duration	1
        DischargeMode	1


        PrincipalDiagnosisProperty	0 to many	PDGPROP
        DiagnosisCategory	0 or 1	DGCAT
        DiagnosisProperty	0 to many	DGPROP
        ComplicationProperty	0 to many	COMPL
        ProcedureProperty	0 to many	PROCPR
        ProcedureORProperty	0 to many	OR
        ProcedureCCProperty	0 to many	CC

        MajorDiagnosticCategory	0 or 1	MDC
        PresenceOfValidMainDiagnosis	1	No explicit term
        ComplicationAndComorbidityLevel	1	CC

   */
    public class CaseFeatures
    {
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public DischargeMode DischargeMode { get; set; }
        public int Duration { get; set; }
        public DiagnosisCategory DiagnosisCategory { get; set; }
        public IDictionary<string, DiagnosisProperty> DiagnosisProperties { get; set; }
        public IDictionary<string, ComplicationProperty> ComplicationProperties { get; set; }
        public IDictionary<string, List<ProcedureProperty>> ProcedureProperties { get; set; }
        public IDictionary<string, ProcedureORProperty> ProcedureORProperties { get; set; }
        public IDictionary<string, ProcedureCCProperty> ProcedureCCProperties { get; set; }
        public string MajorDiagnosticCategory { get; set; }
        public bool PresenceOfValidMainDiagnosis { get; set; }
        public int ComplicationAndComorbidityLevel { get; set; }
        public IDictionary<string, PrincipalDiagnosisProperty> PrincipalDiagnosisProperties { get; set; }

        public CaseFeatures()
        {
            DiagnosisProperties             = new Dictionary<string, DiagnosisProperty>();
            ComplicationProperties          = new Dictionary<string, ComplicationProperty>();
            ProcedureORProperties           = new Dictionary<string, ProcedureORProperty>();
            ProcedureCCProperties           = new Dictionary<string, ProcedureCCProperty>();
            PrincipalDiagnosisProperties    = new Dictionary<string, PrincipalDiagnosisProperty>();
            ProcedureProperties             = new Dictionary<string, List<ProcedureProperty>>();
        }
    }
}