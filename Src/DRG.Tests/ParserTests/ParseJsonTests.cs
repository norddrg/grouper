using System.IO;
using System.Linq;
using DRG.Core.Definitions;
using DRG.DefinitionsParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DRG.Tests.ParserTests
{
    [TestClass]
    public class ParseJsonTests
    {
        static DefinitionsDataStore _store;

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            // read file into a string and deserialize JSON to a type
            //_store = JsonConvert.DeserializeObject<DefinitionsDataStore>(File.ReadAllText(@"c:definitionData.json"));

            // deserialize JSON directly from a file
            var jsonFromFile = File.ReadAllText(@"definitionData.json");
            _store = new JsonParser(jsonFromFile).GetData();
        }

        [TestCategory("DefinitionParsing")]
        [TestMethod]
        public void Parsing_with_json_works()
        {
            var results = _store.DrgLogicModels;
            Assert.IsTrue(results.Count > 0);
        }

        [TestCategory("DefinitionParsing")]
        [TestMethod]
        public void Parsing_with_json_gets_a_model()
        {
            var model = _store.DrgLogicModels.Take(1).FirstOrDefault();
            Assert.IsNotNull(model);
        }
    }
}
