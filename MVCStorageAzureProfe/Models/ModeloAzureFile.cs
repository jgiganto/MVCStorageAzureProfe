using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using Microsoft.Azure;
using System.IO;
using System.Xml.Linq;

namespace MVCStorageAzureProfe.Models
{
    public class ModeloAzureFile
    {
        //se suben con cloufile
        CloudFileDirectory directorio;
        public ModeloAzureFile()
        {
            String claves = CloudConfigurationManager.GetSetting("cuentastorage");
            CloudStorageAccount cuenta = CloudStorageAccount.Parse(claves);
            CloudFileClient cliente = cuenta.CreateCloudFileClient();
            CloudFileShare recurso = cliente.GetShareReference("ficheros");
            this.directorio = recurso.GetRootDirectoryReference();
        }

        public void SubirFicheroAzure(String nombrefichero, Stream contenido)
        {
            CloudFile fichero = this.directorio.GetFileReference(nombrefichero);
            fichero.UploadFromStream(contenido);
        }

        public List<String> GetFilesAzure()
        {
            IEnumerable<IListFileItem> ficheros = this.directorio.ListFilesAndDirectories();
            List<String> datos = new List<string>();
            foreach(var fic in ficheros)
            {
                //https:lalala.azurestorage.net/ficheros/1.txt   me devuelve esto..toda la uri completa
                String urific = fic.Uri.ToString();
                int posicion = urific.LastIndexOf("/") + 1;
                String nombrefichero = urific.Substring(posicion);
                datos.Add(nombrefichero);
            }
            return datos;
        }

        public List<Pelicula> GetPeliculasFile(String nombrefichero)
        {
            CloudFile ficheroxml = this.directorio.GetFileReference(nombrefichero);
            String datosxml = ficheroxml.DownloadText();
            XDocument docxml = XDocument.Parse(datosxml);
            
            var consulta = from datos in docxml.Descendants("pelicula")
                           select new Pelicula
                           {
                               Titulo = datos.Element("titulo").Value,
                               Descripcion = datos.Element("descripcion").Value,
                               Poster = datos.Element("poster").Value,
                               Escenas = new List<Escena>( //consulta al subnivel
                                   from escena in datos.Descendants("escena")
                                   select new Escena
                                   {
                                       TituloEscena=escena.Element("tituloescena").Value,
                                       Descripcion = escena.Element("descripcion").Value,
                                       Imagen=escena.Element("imagen").Value
                                   })//fin consulta subnivel
                           };
            return consulta.ToList();
        }
    }
}