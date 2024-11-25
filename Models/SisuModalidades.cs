using System.ComponentModel.DataAnnotations.Schema;

namespace Extensionista.Models
{
    public class SisuModalidades
    {
        public string ID_CURSO { get; set; } = string.Empty;
        public string TIPO_CONCORRENCIA { get; set; } = string.Empty;
        public float NOTA_CORTE_2024 { get; set; } = 0.0f;
        public float NOTA_CORTE_2023_1 { get; set; } = 0.0f;
        public float NOTA_CORTE_2023_2 { get; set; } = 0.0f;
        public int QT_VAGAS { get; set; } = 0;

        [ForeignKey(nameof(ID_CURSO))]
        public SisuCursos Curso { get; set; } = null!;
    }
}