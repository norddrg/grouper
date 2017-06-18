namespace DRG.Core.Definitions
{
    public class ProcedureDefinition
    {
        public string Code { get; private set; }
        public string VarType { get; private set; }
        public string VarVal { get; private set; }

        public ProcedureDefinition(string code, string varType, string varVal)
        {
            Code = code;
            VarType = varType;
            VarVal = varVal;
        }
    }
}