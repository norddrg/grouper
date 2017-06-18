namespace DRG.Core.Features
{
    public class ProcedureCCProperty
    {
        public int Value { get; private set; }
        public string DerviedProcedureCode { get; set; }

        public ProcedureCCProperty(string varVal, string derivedProcedureCode)
        {
            int temp;
            if (int.TryParse(varVal, out temp))
                Value = temp;

            DerviedProcedureCode = derivedProcedureCode;
        }
    }
}