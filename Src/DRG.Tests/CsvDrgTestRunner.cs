using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using DRG.ConsoleApplication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DRG.Tests
{
    [TestClass]
    public class CsvDrgTestRunner
    {
        static DrgResolver _resolver;

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            _resolver = new DrgResolver();
            //_resolver.LoadDefinitionDataFromExcel(@"Data\DefinitionData.xlsx");
            _resolver.LoadDefinitionDataFromJsonFile(@"Data\DefinitionData.json");
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void TestSingleString()
        {
            //var expected = "108D240110";
            //var str = "1,10248,H,0,B,S525,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,NDJ20,NDA02,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,";
            var expected = "406D152";
            var str = "2,21960,H,4,,K259,,K269,,K250,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,UJD02,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,";
            //var str =
            //    "2,23424,H,0,,J449,,J841,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,GDFX15,GDFX05,GXFX00,PXAA00,GDFC00,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,";

            var groupingResult = _resolver.Execute(str);

            if (groupingResult.GroupingRule != expected)
            {
                var caseData = CaseParser.Parse(str);
                var caseFeatures = new CaseFeatures();
                caseFeatures.ApplyCaseFeatureRules(caseData, _resolver.DefinitionsStore);

                var json = JsonConvert.SerializeObject(caseFeatures, Formatting.Indented);
                Console.WriteLine(json);
            }

            Assert.AreEqual(expected, groupingResult.GroupingRule);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void RunAllCaseStringsFromCsvFile()
        {
            var objReader = new StreamReader(@"TestDataFull.csv");

            var csvRows = new List<CSVData>();
            do
            {
                var textLine = objReader.ReadLine();
                if (!string.IsNullOrEmpty(textLine) && !textLine.StartsWith("RegelNr"))
                {
                    var splitLine = textLine.Split(';');
                    csvRows.Add(new CSVData
                    {
                        RegelNr = splitLine[0],
                        DrgKode = splitLine[1],
                        DrgRegel = splitLine[2],
                        DrgStreng = splitLine[3],
                        ReturKode = splitLine[4]
                    });
                }
            } while (objReader.Peek() != -1);

            var correctResults = new ConcurrentBag<DrgGroupingResult>();
            var nullResults = 0;

            var sw = new Stopwatch();
            sw.Start();
            Parallel.ForEach(csvRows, s =>
            {
                var groupingResult = _resolver.Execute(s.DrgStreng);
                if (groupingResult == null)
                {
                    nullResults++;
                }
                else if (groupingResult.GroupingRule == s.DrgRegel)
                {
                    correctResults.Add(groupingResult);
                }
                // For debugging when needed
                /*else
                {
                    var caseData = CaseParser.Parse(s.DrgStreng);
                    var caseFeatures = new CaseFeatures();
                    caseFeatures.ApplyCaseFeatureRules(caseData, _resolver.DefinitionsStore);

                    string json = JsonConvert.SerializeObject(caseFeatures, Formatting.Indented);
                    Console.WriteLine(json);
                }*/
            });
            sw.Stop();

            Console.WriteLine("Number of DRG strings parsed: " + csvRows.Count);
            Console.WriteLine("Total execution time: " + (double)sw.ElapsedMilliseconds + " ms");
            Console.WriteLine("Execution time per DRG string: " + (double)sw.ElapsedMilliseconds / (double)csvRows.Count + " ms");
            Console.WriteLine("DRG strings that returned with correct result: " + correctResults.Count);
            Console.WriteLine("DRG strings that did not match any rule: " + nullResults);
        }
    }
}
