using System;
using System.IO;

namespace Examen_2p.Data
{
    public class Constants
    {
        //abrir sqlite con permisos readwrite, crea si  no existe y cache compartido
        public const SQLite.SQLiteOpenFlags Flags = SQLite.SQLiteOpenFlags.ReadWrite |
                                                    SQLite.SQLiteOpenFlags.Create |
                                                    SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                //genera ruta y se accede al archivo
                string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, "Examen_2p.db3");
            }
        }

    }
}
