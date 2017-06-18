using DRG.Core.Drg;

namespace DRG.Interfaces
{
    public interface IDrgGroupingRule
    {
        bool Apply(CaseFeatures caseFeatures, DrgLogic drgRow);
    }
}