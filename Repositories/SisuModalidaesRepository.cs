using SQLite;
using Extensionista.Models;

namespace Extensionista.Repositories
{
    public class SisuModalidadesRepository
    {
        private readonly SQLiteConnection _connection;        

        public SisuModalidadesRepository()
        {
            _connection = DataBaseContext.connection;           
        }

        public List<SisuModalidades> ObterCursosPorIdCurso(string IdCurso)
        {
            var cursos = _connection.Table<SisuModalidades>()
                                    .Where(c => c.ID_CURSO == IdCurso)                                    
                                    .ToList();

            return cursos;
        }
    }
}
