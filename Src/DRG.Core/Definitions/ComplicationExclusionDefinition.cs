namespace DRG.Core.Definitions
{
    public class ComplicationExclusionDefinition
    {
        public string Compl { get; private set; }
        public string Code1 { get; private set; }
        public string Code2 { get; private set; }
        public bool HasCode2 { get; private set; }

        public ComplicationExclusionDefinition(string compl, string code, string dcode)
        {
            Compl = compl;
            Code1 = code;
            Code2 = dcode;

            HasCode2 = ! string.IsNullOrEmpty(Code2);
        }
    }
}