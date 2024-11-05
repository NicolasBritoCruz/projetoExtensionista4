using SQLite;
using System.IO;
using Extensionista.Models;
using System.Reflection;

namespace Extensionista
{
    class DataBaseContext
    {
        private const string DB_NAME = "cursos.db3";

        public static SQLiteConnection connection { get; }

        static DataBaseContext()
        {
            // Caminho para o armazenamento de dados do app
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DB_NAME);

            // Verifica se o banco já foi copiado
            if (!File.Exists(dbPath))
            {
                // Obtém o assembly atual para acessar recursos embutidos
                var assembly = Assembly.GetExecutingAssembly();
                var resourcePath = $"Extensionista.Resources.Raw.{DB_NAME}";

                using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
                using (var fileStream = File.Create(dbPath))
                {
                    stream.CopyTo(fileStream);
                }
            }

            // Conectando ao banco de dados no caminho desejado
            connection = new SQLiteConnection(dbPath);
            Console.WriteLine("Conexão com o banco de dados estabelecida com sucesso.");
        }
    }
}