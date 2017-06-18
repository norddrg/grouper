using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using System.Web;
using DRG.WebAPI.Controllers;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;

namespace DRG.WebAPI.Models
{
    /// <summary>
    /// Drg Versjoner.
    /// </summary>
    public class DrgVersjoner
    {
        /// <summary>
        /// Versjons id som skal benyttes når man ønsker å gruppere med en spesifikk versjon. Se <see cref="DRGGrouperController.GetGrupper"/> for mer informasjon.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Beskrivelse av denne versjonen.
        /// </summary>
        public string Beskrivelse { get; set; }

        /// <summary>
        /// Filnavn
        /// </summary>
        public string FilNavn { get; set; }

        public bool ShouldSerializeFilNavn()
        {
            return false;
        }
    }
}