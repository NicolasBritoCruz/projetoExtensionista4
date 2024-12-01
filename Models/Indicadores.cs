using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Extensionista.Models
      {
        public class CursoNota
        {
            //[Column("NomeCurso")]
            public string NomeCurso { get; set; } = string.Empty;

            //[Column("TipoConcorrencia")]
            public string TipoConcorrencia { get; set; } = string.Empty;

            public string Regiao { get; set; } = string.Empty;

            public string NotaCorte2023_1 { get; set; } = string.Empty;
            public string NotaCorte2023_2 { get; set; } = string.Empty;
            public string NotaCorte2024 { get; set; } = string.Empty;
        }

        [Table("SisuCursos")]
        public class VagasRegiao
            {
                public int TotalVagas { get; set; } = 0;

            }

        public class VagasRegiaoTurno
        {
            public string Regiao { get; set; } = string.Empty;
            public string QT_VAGAS { get; set; } = string.Empty;
            public string Turno { get; set; } = string.Empty;
        }

        public class RankingMateriaPesos
        {
            public string Regiao { get; set; } = string.Empty;
            public string Nome_Curso { get; set; } = string.Empty;
            public string Max_Peso_Ch { get; set; } = string.Empty;
            public string Max_Peso_Cn { get; set; } = string.Empty;
            public string Max_Peso_L { get; set; } = string.Empty;
            public string Max_Peso_M { get; set; } = string.Empty;
            public string Max_Peso_R { get; set; } = string.Empty;
        }

        public class CursosDiferentesRegiao
        {
            public string Total_Linhas { get; set; } = string.Empty;
        }

        public class VagasPorRegiao
        {
            public string Nome_Curso { get; set; } = string.Empty;
            public string Qt_Vagas { get; set; } = string.Empty;
        }
}
