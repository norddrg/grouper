namespace DRG.Core.Types
{
    public class Icd
    {
        public IcdValue Operator { get; private set; }

        public Icd(string icd)
        {
            Operator = IcdValue.Null;

            if (!string.IsNullOrEmpty(icd))
            {
                switch (icd)
                {
                    case "+":
                        Operator = IcdValue.Plus;
                        break;
                    case "-":
                        Operator = IcdValue.Minus;
                        break;
                    default:
                        Operator = IcdValue.Blank;
                        break;
                }
            }
        }
    }
    public enum IcdValue
    {
        Plus,
        Minus,
        Blank,
        Null
    }
}