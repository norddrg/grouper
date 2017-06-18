namespace DRG.Core.Features
{
    public class DiagnosisFeature
    {
        public string Value { get; private set; }
        public DiagnosisFeature(string varVal)
        {
            Value = varVal;
        }
    }
}