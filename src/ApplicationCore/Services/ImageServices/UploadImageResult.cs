using System.Collections.Generic;

namespace ApplicationCore.Services.ImageServices
{
    public class UploadImageResult
    {
        /// <summary>
        /// Se ha subido correctamente?
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Diccionario con dos keys fullPath y filename
        /// </summary>
        public Dictionary<string, string> ReturnMessages { get; set; }

        /// <summary>
        /// Lista de errores en caso de fallo
        /// </summary>
        public List<string> Errors { get; set; }
    }
}
