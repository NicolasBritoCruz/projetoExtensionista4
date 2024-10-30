using SQLite;
using System.IO;

namespace Extensionista
{
    class DataBaseContext
    {
        private const string DB_NAME = "cursos.db3";
        public static SQLiteConnection connection { get; }

        static DataBaseContext()
        {
            // Conecta ao banco de dados existente no AppDataDirectory
            connection = new SQLiteConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
        }
    }
}
