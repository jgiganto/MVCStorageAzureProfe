using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCStorageAzureProfe.Models
{
    public class Pelicula
    {
        public String Titulo { get; set; }
        public String Descripcion { get; set; }
        public String Poster { get; set; }
        public List<Escena> Escenas { get; set; }

    }
}