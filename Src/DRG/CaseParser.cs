using System;
using System.Collections.Generic;
using DRG.Core.Types;
using DRG.Interfaces;

namespace DRG
{
    public class CaseParser : ICaseParser
    {
        public static CaseData Parse(string caseString)
        {
            var caseData = new CaseData();
            var list = caseString.Replace("*", "").Split(new[] { "," }, StringSplitOptions.None);

            caseData.Sex = list[0];
            caseData.Age = list[1];
            caseData.DischargeMode = list[2];
            caseData.LengthOfStay = list[3];

            var diagnoseCodes = new List<DiagnosisPair>();
            for (var i = 5; i < 64; i = i + 2)
            {
                if (i > 8 && string.IsNullOrEmpty(list[i]) && string.IsNullOrEmpty(list[i+1]))
                    continue;

                diagnoseCodes.Add(new DiagnosisPair(list[i], list[i + 1]));
            }
            caseData.DiagnoseCodes = diagnoseCodes;

            var procedureCodes = new List<string>();
            for (var i = 65; i < 164; i++)
            {
                if (! string.IsNullOrEmpty(list[i]))
                    procedureCodes.Add(list[i]);
            }
            caseData.ProcedureCodes = procedureCodes;

            return caseData;
        }
    }
}