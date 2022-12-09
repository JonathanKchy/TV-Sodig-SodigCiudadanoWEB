using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SodigCiudadano.Models.Entidades
{
    public class Captcha
    {
        public bool Success { get; set; }
        public string Challenge_ts { get; set; }
        public string hostname { get; set; }

        [JsonProperty("error-codes")]
        public List<string> errorCodes { get; set; }
    }
}