using SQLite;
using Extensionista.Models;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Controls;
//using Microsoft.UI.Xaml.Controls.Primitives;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;

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

        public List<SisuCursos> ObterCursoSisuID(String IdCurso)
        {
            var SisuCursos = _connection.Table<SisuCursos>()
                                        .Where(u => u.ID_CURSO == IdCurso)                                        
                                        .ToList();
            return SisuCursos;
        }

        public List<SisuCursos> ObterCursosSisuCidade(String CodigoIES, String Cidade)
        {
            var SisuCursos = _connection.Table<SisuCursos>()
                                          .Where(u => u.CODIGO_IES == CodigoIES && u.CIDADE == Cidade)
            .ToList();

            return SisuCursos;
        }
    }
}