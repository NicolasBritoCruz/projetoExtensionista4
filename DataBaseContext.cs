using SQLite;
using System.IO;
using Extensionista.Models;

namespace Extensionista
{
    class DataBaseContext
    {
        private const string DB_NAME = "cursos.db3";
        public static SQLiteConnection connection { get; }

        static DataBaseContext()
        {
            // Obtém o caminho do diretório do aplicativo
            string dbPath = Path.Combine(AppContext.BaseDirectory, "DataBase", DB_NAME);

            // Verifica se o arquivo do banco de dados existe
            if (File.Exists(dbPath))
            {
                // Conecta ao banco de dados existente
                connection = new SQLiteConnection(dbPath);
                Console.WriteLine("Conexão com o banco de dados estabelecida com sucesso.");
            }
            else
            {
                throw new FileNotFoundException($"O arquivo do banco de dados '{DB_NAME}' não foi encontrado .");
            }
        }
    }
}