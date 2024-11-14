using SQLite;


namespace Extensionista.Models
{
    public class Universidades
    {
        public int ID_UNIVERSIDADE { get; set; }

        public int CODIGO_IES { get; set; }

        public string NOME_IES { get; set; } = string.Empty;

        public string CATEGORIA_ADMINISTRATIVA { get; set; } = string.Empty;

        public string MUNICIPIO { get; set; } = string.Empty;

        public string UF { get; set; } = string.Empty;

        public string REGIAO { get; set; } = string.Empty;

        public bool Favorito { get; set; }
    }

    public class Cursos
    {
        public int ID_CURSO { get; set; }

        public int ID_UNIVERSIDADE { get; set; }

        public int CODIGO_CURSO { get; set; }

        public string NOME_CURSO { get; set; } = string.Empty;

        public string GRAU { get; set; } = string.Empty;

        public string MODALIDADE { get; set; } = string.Empty;

        public int QT_VAGAS_AUTORIZADAS { get; set; }

        [Ignore]
        public int Index { get; set; }

        [Ignore]
        public bool IsEven => Index % 2 == 0;

        public bool Favorito { get; set; }

    }
}