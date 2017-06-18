using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DRG.WebAPI.Tests
{
    [TestClass]
    public class PerFormanceTest
    {
        [TestMethod]
        public void TestAllMethods()
        {
            var input = new List<string>();
            using (var file = new StreamReader(@"TestData.txt"))
            {
                while (!file.EndOfStream)
                {
                    var textLine = file.ReadLine();
                    if (!string.IsNullOrEmpty(textLine) && !textLine.StartsWith("RegelNr"))
                    {
                        if (!textLine.Contains("*"))
                            input.Add(textLine);
                    }
                }
            }

            var sw = new Stopwatch();

            var client = new WebClient();

            input.ForEach(s =>
            {
                try
                {
                    sw.Start();
                    Console.WriteLine(client.DownloadString(@"https://drgwebapi.azurewebsites.net/Beregn/" + s));
                    sw.Stop();
                }
                catch (Exception)
                {
                    Console.WriteLine(s);
                }
                finally
                {
                    sw.Stop();
                }
            });

            Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}
