using System.Text.RegularExpressions;

namespace DRG.Core.Types
{
    public class SecPro
    {
        public SecProValue Operator { get; private set; }
        public string Value { get; private set; }

        public SecPro(string value)
        {
            Operator = SecProValue.Null;
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
                                Operator = SecProValue.Plus;
                                break;
                            case "-":
                                Operator = SecProValue.Minus;
                                break;
                            default:
                                Operator = SecProValue.Blank;
                                break;
                        }
                    }
                }
            }
        }
    }

    public enum SecProValue
    {
        Plus,
        Minus,
        Blank,
        Null
    }
}