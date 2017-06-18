using DRG.Core.Definitions;

namespace DRG.DefinitionsParser
{
    public interface IParser
    {
        DefinitionsDataStore GetData();
    }
}