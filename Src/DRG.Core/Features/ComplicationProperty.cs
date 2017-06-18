namespace DRG.Core.Features
{
    public class ComplicationProperty
    {
        public string Value { get; private set; }
        public string WildcardValue { get; private set; }
        public bool IsActive { get; private set; }
        public int CcLevel { get; private set; }

        public ComplicationPropertyType CompType { get; private set; }

        /*
            For each ComplicationProperty  of the Case, the attributes IsActive and CCLevel are set according to the following rules:
            3rd character of FeatureValue	IsActive	CCLevel
            “C”	1 (Yes)	1
            “G”	1 (Yes)	2
            “I”	0 (No)	1
            “J”	0 (No)	2
        */

        public ComplicationProperty(string varVal)
        {
            CompType = ComplicationPropertyType.Null;
            Value = varVal;

            if (varVal.Length >= 3)
            {
                var character = varVal[2];
                switch (character)
                {
                    case 'C':
                        CompType = ComplicationPropertyType.C;
                        IsActive = true;
                        CcLevel = 1;
                        break;
                    case 'G':
                        CompType = ComplicationPropertyType.G;
                        IsActive = true;
                        CcLevel = 2;
                        break;
                    case 'I':
                        CompType = ComplicationPropertyType.I;
                        IsActive = false;
                        CcLevel = 1;
                        break;
                    case 'J':
                        CompType = ComplicationPropertyType.J;
                        IsActive = false;
                        CcLevel = 2;
                        break;
                }

                var array = varVal.ToCharArray();
                array[2] = '*';
                WildcardValue = new string(array);
            }
        }

        public void SetActive()
        {
            IsActive = true;
        }

        public void SetInactive()
        {
            IsActive = false;
        }

        public void SetCcLevel(int level)
        {
            CcLevel = level;
        }
    }

    public enum ComplicationPropertyType
    {
        C,
        G,
        I,
        J,
        Null
    }
}