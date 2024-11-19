using SQLite;
using Extensionista.Models;

namespace Extensionista.Repositories
{
    public class SisuCursosRepository
    {
        private readonly SQLiteConnection _connection;     

        public SisuCursosRepository()
        {
            _connection = DataBaseContext.connection;
        }

        public List<SisuCursos> ObterCursosSisu(String CodigoIES)
        {
            var SisuCursos = _connection.Table<SisuCursos>()
                                          .Where(u => u.Codigo_Ies == CodigoIES)
                                          .ToList();

            return SisuCursos;
        }

        public List<SisuCursos> ObterCursoSisu(String CodigoIES, String IdCurso)
        {
            var SisuCursos = _connection.Table<SisuCursos>()
                                        .Where(u => u.Codigo_Ies == CodigoIES && u.Id_Curso == IdCurso)                                        
                                        .ToList();
            return SisuCursos;
        }     
     
    }
}
