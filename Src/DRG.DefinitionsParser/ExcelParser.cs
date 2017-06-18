using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DRG.Core.Definitions;
using DRG.Core.Drg;

namespace DRG.DefinitionsParser
{
    public class ExcelParser : IParser
    {
        readonly string _filePath;

        public ExcelParser(string definitionsDataFilePath)
        {
            _filePath = definitionsDataFilePath;
        }

        public DefinitionsDataStore GetData()
        {
            var tables = Read().Tables;
            var data = new DefinitionsDataStore();

            FillDrgLogic(data, tables.Cast<DataTable>().FirstOrDefault(t => t.TableName == "drglogic"));
            FillDg(data, tables.Cast<DataTable>().FirstOrDefault(t => t.TableName == "dg"));
            FillProc(data, tables.Cast<DataTable>().FirstOrDefault(t => t.TableName == "proc"));
            FillComplCat(data, tables.Cast<DataTable>().FirstOrDefault(t => t.TableName == "complcat"));
            FillComplExcl(data, tables.Cast<DataTable>().FirstOrDefault(t => t.TableName == "complexcl"));
            FillDrgNames(data, tables.Cast<DataTable>().FirstOrDefault(t => t.TableName == "drgnames"));

            return data;
        }

        void FillDrgLogic(DefinitionsDataStore store, DataTable table)
        {
            store.DrgLogicModels = new List<DrgLogic>();
            var identifier = 1;

            foreach (DataRow row in table.Rows)
            {
                var ord         = GetField(row, "ord");
                var drg         = GetField(row, "drg");
                var rtc         = GetField(row, "rtc");
                var icd         = GetField(row, "icd");
                var mdc         = GetField(row, "mdc");
                var pdgprop     = GetField(row, "pdgprop");
                var orprop      = GetField(row, "or");
                var procpro1    = GetField(row, "procpro1");
                var dgcat1      = GetField(row, "dgcat1");
                var agelim      = GetField(row, "agelim");
                var compl       = GetField(row, "compl");
                var sex         = GetField(row, "sex");
                var dgprop1     = GetField(row, "dgprop1");
                var dgprop2     = GetField(row, "dgprop2");
                var dgprop3     = GetField(row, "dgprop3");
                var dgprop4     = GetField(row, "dgprop4");
                var secproc1    = GetField(row, "secproc1");
                var disch       = GetField(row, "disch");
                var dur         = GetField(row, "dur");
                var locdrg      = GetField(row, "loc_drg (link to NDMS)");

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

        void FillDg(DefinitionsDataStore store, DataTable table)
        {
            store.DgModels_COMPL    = new Dictionary<string, List<DiagnosisDefinition>>();
            store.DgModels_DGCAT    = new Dictionary<string, List<DiagnosisDefinition>>();
            store.DgModels_DGPROP   = new Dictionary<string, List<DiagnosisDefinition>>();
            store.DgModels_MDC      = new Dictionary<string, List<DiagnosisDefinition>>();
            store.DgModels_OR       = new Dictionary<string, List<DiagnosisDefinition>>();
            store.DgModels_PDGPRO   = new Dictionary<string, List<DiagnosisDefinition>>();
            store.DgModels_PROCPR   = new Dictionary<string, List<DiagnosisDefinition>>();

            foreach (DataRow row in table.Rows)
            {
                var code        = GetField(row, "code");
                var dcode       = GetField(row, "d_code");
                var varType     = GetField(row, "vartype");
                var varVal      = GetField(row, "varval");

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
                            store.DgModels_DGPROP.Add(dg.Code1, new List<DiagnosisDefinition> { dg});
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

        void FillProc(DefinitionsDataStore store, DataTable table)
        {
            store.ProcModels_CC         = new Dictionary<string, List<ProcedureDefinition>>();
            store.ProcModels_DGPROP     = new Dictionary<string, List<ProcedureDefinition>>();
            store.ProcModels_OR         = new Dictionary<string, List<ProcedureDefinition>>();
            store.ProcModels_PROCPR     = new Dictionary<string, List<ProcedureDefinition>>();

            foreach (DataRow row in table.Rows)
            {
                var code        = GetField(row, "code");
                var varType     = GetField(row, "vartype");
                var varVal      = GetField(row, "varval");

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
                            store.ProcModels_PROCPR.Add(code, new List<ProcedureDefinition> {proc});
                        }
                        break;
                }
            }
        }

        void FillComplCat(DefinitionsDataStore store, DataTable table)
        {
            store.ComplCatModels = new List<ComplicationCategoryDefinition>();
            store.ComplCatInclModels = new List<ComplicationCategoryDefinition>();

            foreach (DataRow row in table.Rows)
            {
                var compl       = GetField(row, "compl");
                var inclprop    = GetField(row, "inclprop");

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

        void FillComplExcl(DefinitionsDataStore store, DataTable table)
        {
            store.ComplExclModels = new Dictionary<string, List<ComplicationExclusionDefinition>>();
            foreach (DataRow row in table.Rows)
            {
                var compl       = GetField(row, "compl");
                var code        = GetField(row, "code");
                var dcode       = GetField(row, "d_code");

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

        void FillDrgNames(DefinitionsDataStore store, DataTable table)
        {
            store.DrgNames = new Dictionary<string, DrgNameDefinition>();

            foreach (DataRow row in table.Rows)
            {
                var drg     = GetField(row, "drg");
                var mdc     = GetField(row, "mdc");

                if (string.IsNullOrEmpty(drg))
                    continue;

                if (! store.DrgNames.ContainsKey(drg))
                {
                    store.DrgNames.Add(drg, new DrgNameDefinition(drg, mdc));
                }
            }
        }

        private string GetField(DataRow row, string fieldName)
        {
            var val = row.Field<string>(fieldName);
            return val == null ? null : val.Trim();
        }


        DataSet Read()
        {
            DataSet ds = new DataSet();

            var connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + _filePath +
                                   ";Extended Properties=Excel 12.0 XML;";

            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                var cmd = new OleDbCommand {Connection = conn};

                var dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                
                if (dtSheet == null) return ds;

                foreach (DataRow dr in dtSheet.Rows)
                {
                    string sheetName = dr["TABLE_NAME"].ToString();

                    if (sheetName.EndsWith("$") || sheetName.EndsWith("$'"))
                    {
                        cmd.CommandText = "SELECT * FROM [" + sheetName + "]";
                        
                        var dt = new DataTable();
                        dt.TableName = Regex.Replace(sheetName, @"[^\w]", "");

                        var da = new OleDbDataAdapter(cmd);
                        da.Fill(dt);

                        ds.Tables.Add(dt);
                    }
                }
                
                conn.Close();
            }

            return ds;
        }
    }
}