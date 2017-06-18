﻿using System.Text.RegularExpressions;

namespace DRG.Core.Types
{
    public class Mdc
    {
        public MdcValue Operator { get; private set; }
        public string Value { get; private set; }

        public Mdc(string value)
        {
            Operator = MdcValue.Null;
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
                                Operator = MdcValue.Plus;
                                break;
                            case "-":
                                Operator = MdcValue.Minus;
                                break;
                            default:
                                Operator = MdcValue.Blank;
                                break;
                        }
                    }
                }
            }
        }
    }

    public enum MdcValue
    {
        Plus,
        Minus,
        Blank,
        Null
    }
}