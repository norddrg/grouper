namespace DRG.Core.Definitions
{
    public class ProcPropDefinition
    {
        public string ProcProp { get; private set; }
        //public int Extens { get; private set; }

        public ProcPropDefinition(string procprop, string extens)
        {
            ProcProp = procprop;

            /*int parsedExtens;
            int.TryParse(extens, out parsedExtens);
            Extens = parsedExtens;*/
        }
    }
}