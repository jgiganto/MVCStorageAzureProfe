using MVCStorageAzureProfe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCStorageAzureProfe.Controllers
{
    public class AzureFileController : Controller
    {
        ModeloAzureFile modelo;
        public AzureFileController()
        {
            this.modelo = new ModeloAzureFile();
        }
        // GET: AzureFile
        public ActionResult SubirFichero()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SubirFichero(HttpPostedFileBase fichero)
        {
            modelo.SubirFicheroAzure(fichero.FileName, fichero.InputStream);
            ViewBag.Mensaje = "Archivo subido correctamente a Azure";
            return View();
        }
        public ActionResult FicherosAzure()
        {
            List<String> archivos = modelo.GetFilesAzure();
            return View(archivos);
        }

        public ActionResult PeliculasEscenas(String nombrefichero)
        {
            List<Pelicula> peliculas = modelo.GetPeliculasFile(nombrefichero);
            return View(peliculas);
        }
    }
}