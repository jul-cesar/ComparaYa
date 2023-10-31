using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComparaYa.Models
{
    public class NombreUsuario
    {
        [JsonProperty("nombre")]
        public string nombre {  get; set; }    
    }
}
