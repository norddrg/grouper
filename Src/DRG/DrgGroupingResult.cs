namespace DRG
{
    public class DrgGroupingResult
    {
        public string Result { get; private set; }
        public string AlternateResult { get; private set; }
        public string Code { get; private set; }
        public string GroupingRule { get; private set; }
        public string MajorDiagnosticCategory { get; private set; }

        public DrgGroupingResult(string result, string alternateResult, string code, string groupingRule, string mdc)
        {
            Result = result;
            AlternateResult = alternateResult;
            Code = code;
            GroupingRule = groupingRule;
            MajorDiagnosticCategory = mdc;
        }
    }
}