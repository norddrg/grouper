using System.Text.RegularExpressions;

namespace DRG.Core.Types
{
    public class Age
    {
        public AgeValue Operator { get; private set; }
        public int? Days { get; private set; }

        public Age(string age)
        {
            Operator = AgeValue.Null;
            Days = null;
            SetOperatorAndValue(age);
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
                            Operator = AgeValue.LessThan;
                            break;
                        case ">":
                            Operator = AgeValue.MoreThan;
                            break;
                        default:
                            Operator = AgeValue.Blank;
                            break;
                    }

                    Days = days;
                }
            }
        }
    }

    public enum AgeValue
    {
        LessThan,
        MoreThan,
        Blank,
        Null
    }
}