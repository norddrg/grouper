using System.Text.RegularExpressions;
using DRG.Core.Features;

namespace DRG.Core.Types
{
    public class Disch
    {
        public DischValue Operator { get; private set; }
        public DischargeMode Value { get; private set; }

        public Disch(string value)
        {
            Operator = DischValue.Null;
            Value = DischargeMode.Null;
            SetOperatorAndValue(value);
        }

        private void SetOperatorAndValue(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var match = Regex.Match(value, @"^(?<operator>[<\-\+>==])?(?<value>.*)$");
                if (match.Success)
                {
                    var val = match.Groups["value"].Value.Trim();
                    if (!string.IsNullOrEmpty(val))
                    {

                        switch (val)
                        {
                            case "E":
                                Value = DischargeMode.E;
                                break;
                            case "N":
                                Value = DischargeMode.N;
                                break;
                            case "H":
                                Value = DischargeMode.H;
                                break;
                            default:
                                Value = DischargeMode.Any;
                                break;
                        }

                        var op = match.Groups["operator"].Value.Trim();

                        switch (op)
                        {
                            case "=":
                                Operator = DischValue.Equals;
                                break;
                            case "-":
                                Operator = DischValue.Minus;
                                break;
                            default:
                                Operator = DischValue.Blank;
                                break;
                        }
                    }
                }
            }
        }
    }

    public enum DischValue
    {
        Equals,
        Minus,
        Blank,
        Null
    }
}