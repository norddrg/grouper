using System.Text.RegularExpressions;

namespace DRG.Core.Types
{
    public class Duration
    {
        public DurationValue Operator { get; private set; }
        public int? Days { get; private set; }

        public Duration(string dur)
        {
            Operator = DurationValue.Null;
            Days = null;
            SetOperatorAndValue(dur);
        }

        private void SetOperatorAndValue(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var match = Regex.Match(value, @"^(?<operator>[<\-\+>=])?(?<value>.*)$");
                if (match.Success)
                {
                    var op = match.Groups["operator"].Value.Trim();
                    int days;
                    int.TryParse(match.Groups["value"].Value.Trim(), out days);

                    switch (op)
                    {
                        case "<":
                            Operator = DurationValue.LessThan;
                            break;
                        case ">":
                            Operator = DurationValue.MoreThan;
                            break;
                        default:
                            Operator = DurationValue.Blank;
                            break;
                    }

                    Days = days;
                }
            }
        }
    }

    public enum DurationValue
    {
        LessThan,
        MoreThan,
        Blank,
        Null
    }
}