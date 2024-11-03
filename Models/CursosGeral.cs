using SQLite;


namespace Extensionista.Models
{
    [Table("CursosGeral")]
    public class Universidades
    {

        public int CODIGO_IES { get; set; }

        public string NOME_IES { get; set; } = string.Empty;

        public string CATEGORIA_ADMINISTRATIVA { get; set; } = string.Empty;

        public string MUNICIPIO { get; set; } = string.Empty;

        public string UF { get; set; } = string.Empty;

        public string REGIAO { get; set; } = string.Empty;


    }

    [Table("CursosGeral")]
    public class Curso
    {
        public int CODIGO_CURSO { get; set; }

        public string NOME_CURSO { get; set; } = string.Empty;

        public string GRAU { get; set; } = string.Empty;

        public string MODALIDADE { get; set; } = string.Empty;

        public int QT_VAGAS_AUTORIZADAS { get; set; }

    }
}
