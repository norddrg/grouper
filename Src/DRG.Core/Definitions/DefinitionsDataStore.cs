using System.Collections.Generic;
using DRG.Core.Drg;

namespace DRG.Core.Definitions
{
    public class DefinitionsDataStore
    {
        public IList<DrgLogic> DrgLogicModels { get; set; }
        public IDictionary<string, List<DiagnosisDefinition>> DgModels_COMPL { get; set; }
        public IDictionary<string, List<DiagnosisDefinition>> DgModels_DGCAT { get; set; }
        public IDictionary<string, List<DiagnosisDefinition>> DgModels_DGPROP { get; set; }
        public IDictionary<string, List<DiagnosisDefinition>> DgModels_MDC { get; set; }
        public IDictionary<string, List<DiagnosisDefinition>> DgModels_OR { get; set; }
        public IDictionary<string, List<DiagnosisDefinition>> DgModels_PDGPRO { get; set; }
        public IDictionary<string, List<DiagnosisDefinition>> DgModels_PROCPR { get; set; }
        public IDictionary<string, List<ProcedureDefinition>> ProcModels_CC { get; set; }
        public IDictionary<string, List<ProcedureDefinition>> ProcModels_DGPROP { get; set; }
        public IDictionary<string, List<ProcedureDefinition>> ProcModels_OR { get; set; }
        public IDictionary<string, List<ProcedureDefinition>> ProcModels_PROCPR { get; set; }
        public IDictionary<string, List<ComplicationExclusionDefinition>> ComplExclModels { get; set; }
        public IDictionary<string, DrgNameDefinition> DrgNames { get; set; }
        public IList<ComplicationCategoryDefinition> ComplCatModels { get; set; }
        public IList<ComplicationCategoryDefinition> ComplCatInclModels { get; set; }
    }
}