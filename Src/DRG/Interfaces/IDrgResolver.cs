namespace DRG.Interfaces
{
    public interface IDrgResolver
    {
        DrgGroupingResult Execute(string drgString);

        DrgGroupingResult Execute(CaseData caseData);

    }
}