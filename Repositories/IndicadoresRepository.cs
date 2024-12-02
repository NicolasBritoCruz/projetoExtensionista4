using SQLite;
using Extensionista.Models;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Controls;
//using Microsoft.UI.Xaml.Controls.Primitives;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;

namespace Extensionista.Repositories
{
    public class IndicadoresRepository
    {
        private readonly SQLiteConnection _connection;

        public IndicadoresRepository()
        {
            _connection = DataBaseContext.connection;
        }

        //Indicadores

        //Listagem da quantas vagas somadas há por região
        public int ObterVagasRegiao(string regiao)
        {
            regiao = regiao.ToUpper();

            var query = @"
                SELECT SUM(QT_VAGAS) AS TotalVagas
                
                    FROM SisuCursos
                    WHERE REGIAO = ?
              
            ";

            //var query = "SELECT COUNT(REGIAO) FROM SisuCursos";
            // Executa a query e retorna o resultado como uma lista
            var resultado = _connection.QueryScalars<int>(query, regiao).FirstOrDefault();
            return resultado;
        }


        //Listagem de vagas por Região e Turno
        public List<VagasRegiaoTurno> ObterVagasRegiaoTurno(string regiao)
        {
            var query = @"
                                SELECT
                                    REGIAO,
                                    SUM(QT_VAGAS) AS QT_VAGAS,
                                    TURNO
                                FROM SISUCURSOS
                                WHERE REGIAO = ?
                                GROUP BY REGIAO, TURNO
                        ";
            var vagasRegiaoTurno = _connection.Query<VagasRegiaoTurno>(query, regiao);
            return vagasRegiaoTurno;
        }
        public List<CursosDiferentesRegiao> ObterCursosPorRegiao(string regiao)
        {   
            var query = @"
                                SELECT COUNT(*) AS TOTAL_LINHAS
                                FROM(
                                    SELECT
                                        COUNT(NOME_CURSO) AS SOMA
                                    FROM SisuCursos
                                    WHERE REGIAO = ?
                                    GROUP BY NOME_CURSO, REGIAO
                                ) AS 'QUANTIDADE CURSOS';
                        ";
            var CursosRegiao = _connection.Query<CursosDiferentesRegiao>(query, regiao);
            return CursosRegiao;
        }

        //Cursos que mais tem vagas, por região
        public List<VagasPorRegiao> ObterVagasPorRegiao(string regiao)
        {
            var query = @"
                                SELECT
	                                NOME_CURSO,
	                                SUM(QT_VAGAS) QT_VAGAS
                                FROM SisuCursos
                                WHERE REGIAO = ?
                                GROUP BY NOME_CURSO
                                ORDER BY QT_VAGAS DESC
                                LIMIT 10;
                        ";
            var ObterVagasPorRegiao = _connection.Query<VagasPorRegiao>(query, regiao);
            return ObterVagasPorRegiao;
        }
    }
}