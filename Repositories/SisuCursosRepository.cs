using SQLite;
using Extensionista.Models;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Controls;
//using Microsoft.UI.Xaml.Controls.Primitives;
using System.Text.RegularExpressions;

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

        //Indicadores

        //Listagem da quantas vagas somadas há por região
        public List<VagasRegiao> ObterVagasRegiao(string regiao)
        {
            var query = @"
                        SELECT COUNT(*) AS TOTAL_LINHAS
                        FROM(
                            SELECT
                                COUNT(NOME_CURSO) AS SOMA
                            FROM SisuCursos
                            WHERE REGIAO = @Regiao
                            GROUP BY NOME_CURSO, REGIAO
                        ) AS SUBCONSULTA;
                        ";
            var vagasRegiao = _connection.Query<VagasRegiao>(query, new { Regiao = regiao });
            return vagasRegiao;
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
                                WHERE REGIAO = @Regiao
                                GROUP BY REGIAO, TURNO
                        ";
            var vagasRegiaoTurno = _connection.Query<VagasRegiaoTurno>(query, new { Regiao = regiao });
            return vagasRegiaoTurno;
        }

        //Ranking de Matérias e pesos somados por região
        public List<RankingMateriaPesos> ObterRankingPesos(string regiao)
        {
            var query = @"
                                SELECT 
                                    REGIAO,
                                    NOME_CURSO,
                                    MAX(PESO_CH) AS MAX_PESO_CH,
                                    MAX(PESO_CN) AS MAX_PESO_CN,
                                    MAX(PESO_L) AS MAX_PESO_L,
                                    MAX(PESO_M) AS MAX_PESO_M,
                                    MAX(PESO_R) AS MAX_PESO_R,
                                    (
                                        MAX(PESO_CH) +
                                        MAX(PESO_CN) +
                                        MAX(PESO_L) +
                                        MAX(PESO_M) +
                                        MAX(PESO_R)
                                    ) AS SOMAS
                                FROM SISUCURSOS
                                WHERE REGIAO = @Regiao
                                GROUP BY REGIAO, NOME_CURSO
                                ORDER BY SOMAS DESC
                                LIMIT 1
                        ";
            var RankingPesos = _connection.Query<RankingMateriaPesos>(query, new { Regiao = regiao });
            return RankingPesos;
        }

        //Quantidade de cursos diferentes por região
        public List<CursosDiferentesRegiao> ObterCursosPorRegiao(string regiao)
        {
            var query = @"
                                SELECT COUNT(*) AS TOTAL_LINHAS
                                FROM(
                                    SELECT
                                        COUNT(NOME_CURSO) AS SOMA
                                    FROM SisuCursos
                                    WHERE REGIAO = @Regiao
                                    GROUP BY NOME_CURSO, REGIAO
                                ) AS 'QUANTIDADE CURSOS';
                        ";
            var CursosRegiao = _connection.Query<CursosDiferentesRegiao>(query, new { Regiao = regiao });
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
                                WHERE REGIAO = @Regiao
                                GROUP BY NOME_CURSO
                                ORDER BY QT_VAGAS DESC
                                LIMIT 10;
                        ";
            var ObterVagasPorRegiao = _connection.Query<VagasPorRegiao>(query, new { Regiao = regiao });
            return ObterVagasPorRegiao;
        }
        
        //Listagem do histórico de notas por curso e região
        public List<CursoNota> ObterCursosComNotas(string regiao)
         {
            var query = @"
                SELECT 
                    S.NOME_CURSO AS NomeCurso,
                    SM.TIPO_CONCORRENCIA AS TipoConcorrencia,
                    S.REGIAO AS Regiao,
                    COALESCE(MAX(SM.NOTA_CORTE_2023_1), '') AS NotaCorte2023_1,
                    COALESCE(MAX(SM.NOTA_CORTE_2023_2), '') AS NotaCorte2023_2,
                    COALESCE(MAX(SM.NOTA_CORTE_2024), '') AS NotaCorte2024
                FROM SisuCursos S
                LEFT JOIN SisuModalidades SM ON S.ID_CURSO = SM.ID_CURSO
                WHERE S.REGIAO = @Regiao
                GROUP BY S.NOME_CURSO, S.REGIAO, SM.TIPO_CONCORRENCIA
                ORDER BY S.NOME_CURSO";

            var cursosComNotas = _connection.Query<CursoNota>(query, new { Regiao = regiao });
            return cursosComNotas;
         }
    }
}
