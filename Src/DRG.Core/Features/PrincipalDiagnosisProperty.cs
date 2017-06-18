using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRG.Core.Features
{
    public class PrincipalDiagnosisProperty
    {
        public string Value { get; private set; }

        public PrincipalDiagnosisProperty(string varVal)
        {
            Value = varVal;
        }
    }
}
