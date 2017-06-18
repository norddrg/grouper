using DRG.Core.Definitions;

namespace DRG.Interfaces
{
    public interface ICaseFeatureRule
    {
        void Apply(CaseFeatures caseFeatures, CaseData casdeata, DefinitionsDataStore data);
    }
}