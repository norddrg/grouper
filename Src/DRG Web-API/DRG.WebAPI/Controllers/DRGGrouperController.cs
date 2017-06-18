using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using DRG.WebAPI.Models;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.Swagger.Annotations;

namespace DRG.WebAPI.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class DRGGrouperController : ApiController
    {
        private static Dictionary<string, DrgResolver> _drgGroupers = new Dictionary<string, DrgResolver>();
        private static List<DrgVersjoner> _versjoner = new List<DrgVersjoner>();

        static DRGGrouperController()
        {
            //init resolver

            var filsti = System.Web.HttpContext.Current.Server.MapPath(@"~\App_Data\versjoner.json");
            if (File.Exists(filsti))
            {
                var versjonsJson = File.ReadAllText(filsti);
                _versjoner = JsonConvert.DeserializeObject<List<DrgVersjoner>>(versjonsJson);
                _versjoner.ForEach(versjon =>
                    {
                        var grouper = new DrgResolver();
                        grouper.LoadDefinitionDataFromJson(
                            new StreamReader(
                                ZipFile.OpenRead(
                                        System.Web.HttpContext.Current.Server.MapPath(@"~\App_Data\" + versjon.FilNavn))
                                    .Entries[0].Open()).ReadToEnd());

                        _drgGroupers[versjon.Id] = grouper;
                    }
                );
            }
        }

        // GET api/values
        /// <summary>
        /// Denne funksjonene vil gruppere 'drgstreng' og returnere et <see cref="DrgGroupingResult"/>.
        /// </summary>
        /// <param name="drgstreng">DRG streng som skal gruppers.</param>
        /// <param name="versjonsId">Versjon av DRG grupperer som skal brukes. Hvis null </param>
        /// <returns><see cref="DrgGroupingResult"/></returns>
        /// <exception cref="HttpResponseException"></exception>
        [SwaggerResponse(HttpStatusCode.NotFound, "Hvis DRG grupper versjons Id ikke finnes.")]
        [Route("Grupper/{versjonsId}/{drgstreng}")]
        public DrgGroupingResult GetGrupper(string drgstreng, string versjonsId)
        {
            if (_drgGroupers.ContainsKey(versjonsId))
                try
                {
                    return _drgGroupers[versjonsId].Execute(drgstreng);
                }
                catch (Exception)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                        $"Feil format på drgstreng."));
                }

            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound,
                $"VersjonsId {versjonsId} not found."));
        }


        /// <summary>
        /// Hent ut alle DRG versjoner som støttes.
        /// </summary>
        /// <returns><see cref="DrgVersjoner"/></returns>
        [SwaggerOperation("Versjoner")]
        [SwaggerResponse(HttpStatusCode.OK, "Liste av gyldige Drg versjoner", typeof(DrgVersjoner))]
        [Route("Versjoner")]
        public List<DrgVersjoner> GetVersjoner()
        {
            return _versjoner;
        }
    }
}
