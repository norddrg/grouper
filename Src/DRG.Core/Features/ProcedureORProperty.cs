namespace DRG.Core.Features
{
    public class ProcedureORProperty
    {
        public string Value { get; private set; }
        public string DerviedProcedureCode { get; set; }

        public ProcedureORProperty(string varVal, string derivedProcedureCode)
        {
            Value = varVal;
            DerviedProcedureCode = derivedProcedureCode;
        }
    }
}