using System.ComponentModel.DataAnnotations.Schema;

namespace Extensionista.Models
{
    public class SisuModalidades
    {
        public string Id_Curso { get; set; } = string.Empty;
        public string Tipo_Concorrencia { get; set; } = string.Empty;
        public float Nota_Corte_2024 { get; set; } = 0.0f;
        public float Nota_Corte_2023_1 { get; set; } = 0.0f;
        public float Nota_Corte_2023_2 { get; set; } = 0.0f;
        public int Qt_Vagas { get; set; } = 0;

        [ForeignKey(nameof(Id_Curso))]
        public SisuCursos Curso { get; set; } = null!;
    }
}