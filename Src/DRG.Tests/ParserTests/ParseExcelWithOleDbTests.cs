using System.Linq;
using DRG.Core.Definitions;
using DRG.DefinitionsParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DRG.Tests.ParserTests
{
    [TestClass]
    public class ParseExcelWithOleDbTests
    {
        static DefinitionsDataStore _store;

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            _store = new ExcelParser(@"Data\DefinitionData.xlsx").GetData();
        }

        [TestCategory("DefinitionParsing")]
        [TestMethod]
        public void Parsing_with_excel_works()
        {
            var results = _store.DrgLogicModels;
            Assert.IsTrue(results.Count > 0);
        }

        [TestCategory("DefinitionParsing")]
        [TestMethod]
        public void Parsing_with_excel_gets_a_model()
        {
            var model = _store.DrgLogicModels.Take(1).FirstOrDefault();
            Assert.IsNotNull(model);
        }
    }
}
