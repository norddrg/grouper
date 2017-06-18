using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using DRG.Core.Definitions;
using DRG.Interfaces;
using System.Linq;
using DRG.Core.Drg;
using DRG.DefinitionsParser;

namespace DRG
{
    public class DrgResolver : IDrgResolver
    {
        public DefinitionsDataStore DefinitionsStore;

        public void LoadDefinitionDataFromExcel(string definitionsDataFilePath)
        {
            DefinitionsStore = new ExcelParser(definitionsDataFilePath).GetData();
        }

        public void LoadDefinitionDataFromJson(string jsonData)
        {
            DefinitionsStore = new JsonParser(jsonData).GetData();
        }

        public void LoadDefinitionDataFromJsonFile(string jsonDataFilePath)
        {
            var jsonFromFile = File.ReadAllText(jsonDataFilePath);
            DefinitionsStore = new JsonParser(jsonFromFile).GetData();
        }

        public DrgGroupingResult Execute(string drgString)
        {
            var caseData = CaseParser.Parse(drgString);
            return Execute(caseData);
        }

        public DrgGroupingResult Execute(CaseData caseData)
        {
            if (DefinitionsStore == null)
                throw new Exception("No definition data found. Please load definition data before executing DRG");

            var caseFeatures = new CaseFeatures();
            caseFeatures.ApplyCaseFeatureRules(caseData, DefinitionsStore);

            var drgLogicResult = DefinitionsStore.ApplyDrgGroupingRules(caseFeatures);

            if (drgLogicResult != null)
            {
                DrgNameDefinition mdc;
                DefinitionsStore.DrgNames.TryGetValue(drgLogicResult.Drg, out mdc);
                return new DrgGroupingResult(
                    drgLogicResult.Drg, 
                    drgLogicResult.LocDrg, 
                    drgLogicResult.Rtc, 
                    drgLogicResult.Ord,
                    mdc != null ? mdc.Mdc : null
                );
            }

            return null;
        }

        public DrgGroupingResult ExecuteThreaded(CaseData caseData)
        {
            var caseFeatures = new CaseFeatures();
            caseFeatures.ApplyCaseFeatureRules(caseData, DefinitionsStore);

            var results = new ConcurrentBag<DrgLogic>();
            Parallel.ForEach(DefinitionsStore.DrgLogicModels, currentModel =>
            {
                if (currentModel.ApplyDrgGroupingRules(caseFeatures))
                {
                    results.Add(currentModel);
                }
            });

            if (results.Any())
            {
                var drgLogicResult = results.OrderBy(x => x.Id).First();
                DrgNameDefinition mdc;
                DefinitionsStore.DrgNames.TryGetValue(drgLogicResult.Drg, out mdc);
                return new DrgGroupingResult(
                   drgLogicResult.Drg,
                   drgLogicResult.LocDrg,
                   drgLogicResult.Rtc,
                   drgLogicResult.Ord,
                   mdc != null ? mdc.Mdc : null
               );
            }

            return null;
        }
    }
}