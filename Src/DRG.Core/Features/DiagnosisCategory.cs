namespace DRG.Core.Features
{
    public class DiagnosisCategory
    {
        public string Value { get; private set; }
        public bool IsRecodable { get; set; }

        public DiagnosisCategory(string varVal)
        {
            Value = varVal;
            IsRecodable = Value.StartsWith("98");
        }
    }
}