using System.Collections.Generic;
using DRG.Core.Types;

namespace DRG
{
    public class CaseData
    {
        public string Sex { get; set; }
        public string Age { get; set; }
        public string DischargeMode { get; set; }
        public string LengthOfStay { get; set; }
        public IList<string> ProcedureCodes { get; set; }
        public IList<DiagnosisPair> DiagnoseCodes { get; set; }

        public CaseData()
        {
            ProcedureCodes = new List<string>();
            DiagnoseCodes = new List<DiagnosisPair>();
        }
    }
}