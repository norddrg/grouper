using System.Configuration;
using System.Text.RegularExpressions;
using DRG.Core.Types;

namespace DRG.Core.Features
{
    public class DiagnosisProperty
    {
        public string Value { get; private set; }
        public DiagnosisPair DiagnosisPair { get; private set; }
        public bool IsRecodable { get; set; }

        public DiagnosisProperty(string varVal, DiagnosisPair diagnosisPair)
        {
            Value = varVal;
            DiagnosisPair = diagnosisPair;
            IsRecodable = Value.StartsWith("98");
        }
    }
}