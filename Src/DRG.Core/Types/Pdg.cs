using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DRG.Core.Types
{
    public class Pdg
    {
        public PdgValue Operator { get; private set; }
        public string Value { get; private set; }

        public Pdg(string value)
        {
            Operator = PdgValue.Null;
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
                                Operator = PdgValue.Plus;
                                break;
                            case "-":
                                Operator = PdgValue.Minus;
                                break;
                            default:
                                Operator = PdgValue.Blank;
                                break;
                        }
                    }
                }
            }
        }
    }

    public enum PdgValue
    {
        Plus,
        Minus,
        Blank,
        Null
    }
}

