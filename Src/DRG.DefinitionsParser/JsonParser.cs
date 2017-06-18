using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using DRG.Core.Definitions;
using DRG.Core.Drg;
using Newtonsoft.Json;

namespace DRG.DefinitionsParser
{
    public class JsonParser : IParser
    {
        private DefinitionsDataStore _data;

        public JsonParser(string jsonData)
        {
            _data = new DefinitionsDataStore();

            var jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonData);

            FillDrgLogic(_data, jsonObject["drglogic"]);
            FillDg(_data, jsonObject["dg"]);
            FillProc(_data, jsonObject["proc"]);
            FillComplCat(_data, jsonObject["complcat"]);
            FillComplExcl(_data, jsonObject["complexcl"]);
            FillDrgNames(_data, jsonObject["drgnames"]);
        }

        public DefinitionsDataStore GetData()
        {
            return _data;
        }

        void FillDrgLogic(DefinitionsDataStore store, dynamic jsonObject)
        {
            store.DrgLogicModels = new List<DrgLogic>();
            var identifier = 1;

            foreach (var row in jsonObject)
            {
                var ord = GetField(row, "ord");
                var drg = GetField(row, "drg");
                var rtc = GetField(row, "rtc");
                var icd = GetField(row, "icd");
                var mdc = GetField(row, "mdc");
                var pdgprop = GetField(row, "pdgprop");
                var orprop = GetField(row, "or");
                var procpro1 = GetField(row, "procpro1");
                var dgcat1 = GetField(row, "dgcat1");
                var agelim = GetField(row, "agelim");
                var compl = GetField(row, "compl");
                var sex = GetField(row, "sex");
                var dgprop1 = GetField(row, "dgprop1");
                var dgprop2 = GetField(row, "dgprop2");
                var dgprop3 = GetField(row, "dgprop3");
                var dgprop4 = GetField(row, "dgprop4");
                var secproc1 = GetField(row, "secproc1");
                var disch = GetField(row, "disch");
                var dur = GetField(row, "dur");
                var locdrg = GetField(row, "loc_drg (link to NDMS)");

                if (string.IsNullOrEmpty(ord))
                    continue;

                var drgLogic = new DrgLogic(
                    identifier++,
                    ord,
                    drg,
                    rtc,
                    icd,
                    mdc,
                    pdgprop,
                    orprop,
                    procpro1,
                    dgcat1,
                    agelim,
                    compl,
                    sex,
                    dgprop1,
                    dgprop2,
                    dgprop3,
                    dgprop4,
                    secproc1,
                    disch,
                    dur,
                    locdrg
                );

                store.DrgLogicModels.Add(drgLogic);
            }
        }
        void FillDg(DefinitionsDataStore store, dynamic jsonObject)
        {
            store.DgModels_COMPL = new Dictionary<string, List<DiagnosisDefinition>>();
            store.DgModels_DGCAT = new Dictionary<string, List<DiagnosisDefinition>>();
            store.DgModels_DGPROP = new Dictionary<string, List<DiagnosisDefinition>>();
            store.DgModels_MDC = new Dictionary<string, List<DiagnosisDefinition>>();
            store.DgModels_OR = new Dictionary<string, List<DiagnosisDefinition>>();
            store.DgModels_PDGPRO = new Dictionary<string, List<DiagnosisDefinition>>();
            store.DgModels_PROCPR = new Dictionary<string, List<DiagnosisDefinition>>();

            foreach (var row in jsonObject)
            {
                var code = GetField(row, "code");
                var dcode = GetField(row, "d_code");
                string varType = GetField(row, "vartype");
                var varVal = GetField(row, "varval");

                if (string.IsNullOrEmpty(code))
                    continue;

                var dg = new DiagnosisDefinition(code, dcode, varType, varVal);

                switch (varType)
                {
                    case "COMPL":
                        if (store.DgModels_COMPL.ContainsKey(dg.Code1))
                        {
                            store.DgModels_COMPL[dg.Code1].Add(dg);
                        }
                        else
                        {
                            store.DgModels_COMPL.Add(dg.Code1, new List<DiagnosisDefinition> { dg });
                        }
                        break;

                    case "DGCAT":
                        if (store.DgModels_DGCAT.ContainsKey(dg.Code1))
                        {
                            store.DgModels_DGCAT[dg.Code1].Add(dg);
                        }
                        else
                        {
                            store.DgModels_DGCAT.Add(dg.Code1, new List<DiagnosisDefinition> { dg });
                        }
                        break;

                    case "DGPROP":
                        if (store.DgModels_DGPROP.ContainsKey(dg.Code1))
                        {
                            store.DgModels_DGPROP[dg.Code1].Add(dg);
                        }
                        else
                        {
                            store.DgModels_DGPROP.Add(dg.Code1, new List<DiagnosisDefinition> { dg });
                        }
                        break;
                    case "MDC":
                        if (store.DgModels_MDC.ContainsKey(dg.Code1))
                        {
                            store.DgModels_MDC[dg.Code1].Add(dg);
                        }
                        else
                        {
                            store.DgModels_MDC.Add(dg.Code1, new List<DiagnosisDefinition> { dg });
                        }
                        break;

                    case "OR":
                        if (store.DgModels_OR.ContainsKey(dg.Code1))
                        {
                            store.DgModels_OR[dg.Code1].Add(dg);
                        }
                        else
                        {
                            store.DgModels_OR.Add(dg.Code1, new List<DiagnosisDefinition> { dg });
                        }
                        break;

                    case "PDGPRO":
                        if (store.DgModels_PDGPRO.ContainsKey(dg.Code1))
                        {
                            store.DgModels_PDGPRO[dg.Code1].Add(dg);
                        }
                        else
                        {
                            store.DgModels_PDGPRO.Add(dg.Code1, new List<DiagnosisDefinition> { dg });
                        }
                        break;

                    case "PROCPR":
                        if (store.DgModels_PROCPR.ContainsKey(dg.Code1))
                        {
                            store.DgModels_PROCPR[dg.Code1].Add(dg);
                        }
                        else
                        {
                            store.DgModels_PROCPR.Add(dg.Code1, new List<DiagnosisDefinition> { dg });
                        }
                        break;
                }
            }
        }

        void FillProc(DefinitionsDataStore store, dynamic jsonObject)
        {
            store.ProcModels_CC = new Dictionary<string, List<ProcedureDefinition>>();
            store.ProcModels_DGPROP = new Dictionary<string, List<ProcedureDefinition>>();
            store.ProcModels_OR = new Dictionary<string, List<ProcedureDefinition>>();
            store.ProcModels_PROCPR = new Dictionary<string, List<ProcedureDefinition>>();

            foreach (var row in jsonObject)
            {
                var code = GetField(row, "code");
                string varType = GetField(row, "vartype");
                var varVal = GetField(row, "varval");

                if (string.IsNullOrEmpty(code))
                    continue;

                var proc = new ProcedureDefinition(code, varType, varVal);

                switch (varType)
                {
                    case "CC":
                        if (store.ProcModels_CC.ContainsKey(code))
                        {
                            store.ProcModels_CC[code].Add(proc);
                        }
                        else
                        {
                            store.ProcModels_CC.Add(code, new List<ProcedureDefinition> { proc });
                        }
                        break;

                    case "DGPROP":
                        if (store.ProcModels_DGPROP.ContainsKey(code))
                        {
                            store.ProcModels_DGPROP[code].Add(proc);
                        }
                        else
                        {
                            store.ProcModels_DGPROP.Add(code, new List<ProcedureDefinition> { proc });
                        }
                        break;

                    case "OR":
                        if (store.ProcModels_OR.ContainsKey(code))
                        {
                            store.ProcModels_OR[code].Add(proc);
                        }
                        else
                        {
                            store.ProcModels_OR.Add(code, new List<ProcedureDefinition> { proc });
                        }
                        break;

                    case "PROCPR":
                        if (store.ProcModels_PROCPR.ContainsKey(code))
                        {
                            store.ProcModels_PROCPR[code].Add(proc);
                        }
                        else
                        {
                            store.ProcModels_PROCPR.Add(code, new List<ProcedureDefinition> { proc });
                        }
                        break;
                }
            }
        }

        void FillComplCat(DefinitionsDataStore store, dynamic jsonObject)
        {
            store.ComplCatModels = new List<ComplicationCategoryDefinition>();
            store.ComplCatInclModels = new List<ComplicationCategoryDefinition>();

            foreach (var row in jsonObject)
            {
                var compl = GetField(row, "compl");
                var inclprop = GetField(row, "inclprop");

                if (string.IsNullOrEmpty(compl))
                    continue;

                var complCat = new ComplicationCategoryDefinition(compl, inclprop);
                store.ComplCatModels.Add(complCat);

                if (complCat.HasInclProp)
                {
                    store.ComplCatInclModels.Add(complCat);
                }
            }
        }

        void FillComplExcl(DefinitionsDataStore store, dynamic jsonObject)
        {
            store.ComplExclModels = new Dictionary<string, List<ComplicationExclusionDefinition>>();
            foreach (var row in jsonObject)
            {
                var compl = GetField(row, "compl");
                var code = GetField(row, "code");
                var dcode = GetField(row, "d_code");

                if (string.IsNullOrEmpty(compl))
                    continue;

                var complCat = new ComplicationExclusionDefinition(compl, code, dcode);

                if (store.ComplExclModels.ContainsKey(compl))
                {
                    store.ComplExclModels[compl].Add(complCat);
                }
                else
                {
                    store.ComplExclModels.Add(compl, new List<ComplicationExclusionDefinition> { complCat });
                }
            }
        }

        void FillDrgNames(DefinitionsDataStore store, dynamic jsonObject)
        {
            store.DrgNames = new Dictionary<string, DrgNameDefinition>();

            foreach (var row in jsonObject)
            {
                var drg = GetField(row, "drg");
                var mdc = GetField(row, "mdc");

                if (string.IsNullOrEmpty(drg))
                    continue;

                if (!store.DrgNames.ContainsKey(drg))
                {
                    store.DrgNames.Add(drg, new DrgNameDefinition(drg, mdc));
                }
            }
        }

        private string GetField(dynamic jsobObject, string fieldName)
        {
            var val = jsobObject[fieldName];
            return val == null ? null : val.ToString().Trim();
        }
    }
}