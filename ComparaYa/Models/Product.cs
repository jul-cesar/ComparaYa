using System;
using System.Collections.Generic;
using System.Text;

namespace ComparaYa.Models
{
   public class Product
    {
     public int id {  get; set; }
        public string nombre { get; set; }
        public string imagen_url { get; set; }
        public string precio_d1 { get; set;}
        public string precio_olim { get; set; }
        public string precio_exito { get; set; }
        public int categoria_id { get; set; }
    }

}
