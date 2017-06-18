namespace DRG.Core.Definitions
{
    public class ComplicationCategoryDefinition
    {
        public string Compl { get; private set; }
        public string InclProp { get; private set; }
        public ComplicationCategoryType CompType { get; private set; }
        public bool HasInclProp { get; private set; }

        public ComplicationCategoryDefinition(string compl, string inclProp)
        {
            Compl = compl;
            InclProp = inclProp;
            CompType = ComplicationCategoryType.Null;

            if (!string.IsNullOrEmpty(inclProp))
                HasInclProp = true;

            if (compl.Length >= 3)
            {
                var character = compl[2];
                switch (character)
                {
                    case 'I':
                        CompType = ComplicationCategoryType.I;
                        break;
                    case 'J':
                        CompType = ComplicationCategoryType.J;
                        break;
                }
            }
        }
    }

    public enum ComplicationCategoryType
    {
        I,
        J,
        Null
    }
}