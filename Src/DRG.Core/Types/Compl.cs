using System.Text.RegularExpressions;

namespace DRG.Core.Types
{
    public class Compl
    {
        public ComplValue Operator { get; private set; }
        public int? Value { get; private set; }

        public Compl(string value)
        {
            Operator = ComplValue.Null;
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
                        Value = int.Parse(val);
                        var op = match.Groups["operator"].Value.Trim();

                        switch (op)
                        {
                            case "+":
                                Operator = ComplValue.Plus;
                                break;
                            case "-":
                                Operator = ComplValue.Minus;
                                break;
                            case ">":
                                Operator = ComplValue.MoreThan;
                                break;
                            case "<":
                                Operator = ComplValue.LessThan;
                                break;
                            default:
                                Operator = ComplValue.Blank;
                                break;
                        }
                    }
                }
            }
        }
    }

    public enum ComplValue
    {
        Plus,
        Minus,
        MoreThan,
        LessThan,
        Blank,
        Null
    }
}