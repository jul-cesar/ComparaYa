using ComparaYa.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComparaYa.localBD
{
    public class Favorite
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int? UsuarioId { get; set; }
        public int ProductoId { get; set; }


    }
}
