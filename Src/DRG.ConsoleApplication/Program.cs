using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DRG.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            DrgResolver resolver = new DrgResolver();
            //resolver.LoadDefinitionDataFromExcel(@"Data\DefinitionData.xlsx");

            resolver.LoadDefinitionDataFromJsonFile(@"Data\DefinitionData.json");
            var objReader = new StreamReader(@"Data\TestDataFull.csv");

            var csvRows = new List<CSVData>();
            do
            {
                var textLine = objReader.ReadLine();
                if (!string.IsNullOrEmpty(textLine) && !textLine.StartsWith("RegelNr"))
                {
                    //lineCount++;
                    var splitLine = textLine.Split(';');
                    csvRows.Add(new CSVData()
                    {
                        RegelNr = splitLine[0],
                        DrgKode = splitLine[1],
                        DrgRegel = splitLine[2],
                        DrgStreng = splitLine[3],
                        ReturKode = splitLine[4]
                    });
                }
            } while (objReader.Peek() != -1);

            var results = new ConcurrentDictionary<CSVData, DrgGroupingResult>();

            var sw = new Stopwatch();
            sw.Start();

            Parallel.ForEach(csvRows, s =>
            {
                var groupingResult = resolver.Execute(s.DrgStreng);
                if (groupingResult != null)
                    results[s] = groupingResult;
            });

            sw.Stop();

            double average = (double) sw.ElapsedMilliseconds/(double) csvRows.Count;

            Console.WriteLine("Number of DRG strings parsed: " + csvRows.Count);
            Console.WriteLine("Total execution time: " + sw.ElapsedMilliseconds + " ms");
            Console.WriteLine("Execution time per DRG string: " + average + " ms");
            Console.WriteLine("This makes it possible to parse around " + (int)(3600000d / average)  + " case strings per hour");

            var path = args.Length > 1 ? args.ToList().LastOrDefault() : "";

            var resultFile = Path.Combine(path??Directory.GetCurrentDirectory(), "results.csv");
            
            using (var file = File.CreateText(resultFile))
            {
                file.WriteLine("Id;DRGGrupperingsstreng;DRGKode1;DRGKode2;DRGGrupperingsregel1;DRGGrupperingsregel2;Returkode1;Returkode2");

                foreach (var r in results)
                {
                    var drgGroupingResult = r.Value;
                    var line = 
                        r.Key.RegelNr.Trim() + ";" +
                        r.Key.DrgStreng.Trim() + ";" +
                        r.Key.DrgKode.Trim() + ";" +
                        drgGroupingResult.Result.Trim() + ";" +
                        r.Key.DrgRegel.Trim() + ";" +
                        drgGroupingResult.GroupingRule.Trim() + ";" +
                        r.Key.ReturKode.Trim() + ";" +
                        drgGroupingResult.Code.Trim();

                    file.WriteLine(line);
                }
            }

            Console.WriteLine("Press a key to terminate");
            Console.ReadKey();
        }

        
    }
}
