namespace DRG.Core.Definitions
{
    public class DrgNameDefinition
    {
        public string Drg { get; private set; }
        public string Mdc { get; private set; }

        public DrgNameDefinition(string drg, string mdc)
        {
            Drg = drg;
            Mdc = mdc;
        }
    }
}