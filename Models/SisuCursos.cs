using System.ComponentModel.DataAnnotations;

namespace Extensionista.Models
{
    public class SisuCursos
    {
        [Key]
        public string Id_Curso { get; set; } = string.Empty;

        public string Codigo_Ies { get; set; } = string.Empty;
        public string Nome_Curso { get; set; } = string.Empty;
        public int Qt_Vagas { get; set; } = 0;
        public string Turno { get; set; } = string.Empty;
        public string Nome_Ies { get; set; } = string.Empty;
        public string Sigla_Ies { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public float Peso_Cn { get; set; } = 0.0f;
        public float Peso_Ch { get; set; } = 0.0f;
        public float Peso_L { get; set; } = 0.0f;
        public float Peso_M { get; set; } = 0.0f;
        public float Peso_R { get; set; } = 0.0f;
        public string Site_Ies { get; set; } = string.Empty;
    }
}