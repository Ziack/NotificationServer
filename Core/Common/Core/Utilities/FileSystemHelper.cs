using System.IO;

namespace NotificationServer.Core.Utilities
{
    public static class FileSystemHelper
    {
        /// <summary>
        /// Crea el folder padre del archivo indicado en la ruta completa de manera que no haya errores cuando se vaya a escribir el archivo.
        /// Si el folder ya existe, no hace nada.
        /// </summary>
        /// <param name="fileName"></param>
        public static void CreateParentFolder(string fileName)
        {
            string parentFolder = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(parentFolder)) { Directory.CreateDirectory(parentFolder); }
        }

    }
}
