using System.ComponentModel.DataAnnotations;

namespace Extensionista.Models
{
    public class SisuCursos
    {
        [Key]
        public string ID_CURSO { get; set; } = string.Empty;
        public string CODIGO_IES { get; set; } = string.Empty;
        public string NOME_CURSO { get; set; } = string.Empty;
        public int QT_VAGAS { get; set; } = 0;
        public string TURNO { get; set; } = string.Empty;
        public string NOME_IES { get; set; } = string.Empty;
        public string SIGLA_IES { get; set; } = string.Empty;
        public string UF { get; set; } = string.Empty;
        public string CIDADE { get; set; } = string.Empty;
        public float PESO_CN { get; set; } = 0.0f;
        public float PESO_CH { get; set; } = 0.0f;
        public float PESO_L { get; set; } = 0.0f;
        public float PESO_M { get; set; } = 0.0f;
        public float PESO_R { get; set; } = 0.0f;
        public string SITE_IES { get; set; } = string.Empty;
        public bool Favorito { get; set; }
    }
}