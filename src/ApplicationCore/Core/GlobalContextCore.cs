using System;

namespace ApplicationCore.Core
{
    public static class GlobalContextCore
    {
        /// <summary>
        /// Nombre del sitio, obtenido de appsettings.
        /// </summary>
        public static string SiteName { get; set; }

        /// <summary>
        /// http|https, obtenido de appsettings.
        /// </summary>
        public static string Scheme { get; set; }

        /// <summary>
        /// Url del sitio sin slash final, obtenido de appsettings.
        /// </summary>
        public static string SiteUrl { get; set; }
    }
}
