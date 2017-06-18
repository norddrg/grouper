using System.Text.RegularExpressions;

namespace DRG.Core.Types
{
    public class DgProp
    {
        public DgPropValue Operator { get; private set; }
        public string Value { get; private set; }

        public DgProp(string value)
        {
            Operator = DgPropValue.Null;
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
                                Operator = DgPropValue.Plus;
                                break;
                            case "-":
                                Operator = DgPropValue.Minus;
                                break;
                            default:
                                Operator = DgPropValue.Blank;
                                break;
                        }
                    }
                }
            }
        }
    }

    public enum DgPropValue
    {
        Plus,
        Minus,
        Blank,
        Null
    }
}