using System.Text.RegularExpressions;

namespace DRG.Core.Types
{
    public class DgCat
    {
        public DgCatValue Operator { get; private set; }
        public string Value { get; private set; }

        public DgCat(string value)
        {
            Operator = DgCatValue.Null;
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
                                Operator = DgCatValue.Plus;
                                break;
                            case "-":
                                Operator = DgCatValue.Minus;
                                break;
                            default:
                                Operator = DgCatValue.Blank;
                                break;
                        }
                    }
                }
            }
        }
    }

    public enum DgCatValue
    {
        Plus,
        Minus,
        Blank,
        Null
    }
}