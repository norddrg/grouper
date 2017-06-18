namespace DRG.Core.Features
{
    public class ProcedureProperty
    {
        public string Value { get; private set; }
        public string DerviedProcedureCode { get; set; }

        public ProcedureProperty(string varVal, string derivedProcedureCode)
        {
            Value = varVal;
            DerviedProcedureCode = derivedProcedureCode;
        }
    }
}