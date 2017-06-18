namespace DRG.Core.Types
{
    public class DiagnosisPair
    {
        public string Code1 { get; private set; }
        public string Code2 { get; private set; }

        public DiagnosisPair(string code1, string code2)
        {
            Code1 = code1;
            Code2 = code2;
        }

        public bool IsPair
        {
            get
            {
                return !string.IsNullOrEmpty(Code1) && !string.IsNullOrEmpty(Code2);
            }
        }
    }
}