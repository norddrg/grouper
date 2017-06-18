using System.Text.RegularExpressions;

namespace DRG.Core.Types
{
    public class ProcPro
    {
        public ProcProValue Operator { get; private set; }
        public string Value { get; private set; }

        public ProcPro(string value)
        {
            Operator = ProcProValue.Null;
            Value = null;
            SetOperatorAndValue(value);
        }

        private void SetOperatorAndValue(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var match = Regex.Match(value, @"^(?<operator>[<\-\+>=])?(?<value>.*)$");
                if (match.Success)
                {
                    var val = match.Groups["value"].Value.Trim();
                    if (!string.IsNullOrEmpty(val))
                    {
                        Value = val;
                        var op = match.Groups["operator"].Value.Trim();

                        switch (op)
                        {
                            case "+":
                                Operator = ProcProValue.Plus;
                                break;
                            case "-":
                                Operator = ProcProValue.Minus;
                                break;
                            default:
                                Operator = ProcProValue.Blank;
                                break;
                        }
                    }
                }
            }
        }
    }

    public enum ProcProValue
    {
        Plus,
        Minus,
        Blank,
        Null
    }
}