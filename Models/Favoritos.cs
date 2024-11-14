using SQLite;


namespace Extensionista.Models
{
    public class Favoritos
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string NOME_IES { get; set; } = string.Empty;

        public int ID_UNIVERSIDADE { get; set; }

        public string MUNICIPIO { get; set; } = string.Empty;

    }
}