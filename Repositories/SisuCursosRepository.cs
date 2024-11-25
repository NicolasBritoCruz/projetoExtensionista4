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
                                          .Where(u => u.CODIGO_IES == CodigoIES)
                                          .ToList();

            return SisuCursos;
        }

        public List<SisuCursos> ObterCursoSisuID(String CodigoIES, String IdCurso)
        {
            var SisuCursos = _connection.Table<SisuCursos>()
                                        .Where(u => u.CODIGO_IES == CodigoIES && u.ID_CURSO == IdCurso)                                        
                                        .ToList();
            return SisuCursos;
        }     
     
    }
}
