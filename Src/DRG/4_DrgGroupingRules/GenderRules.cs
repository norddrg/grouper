using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using DRG.ExcelParser;
using DRG.ExcelParser.Models;

namespace DRG.Rules
{
    public static class GenderRules
    {
        public static void ApplyGenderRules(this DefinitionsDataStore definitions, Case actualCase, List<DrgLogic> map)
        {
            // reduce by sex
            //map.RemoveAll(x => x.Sex != Gender.None && x.Sex != Gender.Unknown && x.Sex != caseData.Sex);
        }
    }
}