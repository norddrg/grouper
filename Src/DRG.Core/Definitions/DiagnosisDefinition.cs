namespace DRG.Core.Definitions
{
    public class DiagnosisDefinition
    {
        public string Code1 { get; private set; }
        public string Code2 { get; private set; }
        public string VarType { get; private set; }
        public string VarVal { get; private set; }
        public string WildcardVarVal { get; private set; }

        public bool HasCode2 { get; private set; }

        public DiagnosisDefinition(string code, string dcode, string varType, string varVal)
        {
            Code1 = code;
            Code2 = dcode;
            VarType = varType;
            VarVal = varVal;

            HasCode2 = !string.IsNullOrEmpty(Code2);

            var array = varVal.ToCharArray();
            if (array.Length > 2)
            {
                array[2] = '*';
                WildcardVarVal = new string(array);
            }
            else
            {
                WildcardVarVal = varVal;
            }
        }
    }
}