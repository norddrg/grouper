using System.Text.RegularExpressions;

namespace DRG.Core.Types
{
    public class OrProp
    {
        public OrPropValue Operator { get; private set; }
        public OrPropType Type { get; private set; }

        public OrProp(string value)
        {
            Operator = OrPropValue.Null;
            Type = OrPropType.Null;
            SetOperatorAndValue(value);
        }

        private void SetOperatorAndValue(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var match = Regex.Match(value, @"^(?<operator>[<\-\+>=])?(?<type>.*)$");
                if (match.Success)
                {
                    var val = match.Groups["type"].Value.Trim();
                    if (!string.IsNullOrEmpty(val))
                    {
                        switch (val.ToUpper())
                        {
                            case "S":
                                Type = OrPropType.S;
                                break;
                            case "P":
                                Type = OrPropType.P;
                                break;
                            case "N":
                                Type = OrPropType.N;
                                break;
                            case "Z":
                                Type = OrPropType.Z;
                                break;
                            default:
                                Type = OrPropType.Other;
                                break;
                        }
                    }

                    var op = match.Groups["operator"].Value.Trim();
                    switch (op)
                    {
                        case "+":
                            Operator = OrPropValue.Plus;
                            break;
                        case "-":
                            Operator = OrPropValue.Minus;
                            break;
                        default:
                            Operator = OrPropValue.Blank;
                            break;
                    }
                }
            }
        }
    }

    public enum OrPropValue
    {
        Plus,
        Minus,
        Blank,
        Null
    }

    public enum OrPropType
    {
        S,
        P,
        N,
        Z,
        Other,
        Null
    }
}