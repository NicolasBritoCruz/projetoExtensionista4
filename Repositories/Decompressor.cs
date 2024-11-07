using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensionista.Repositories
{
    public class Decompressor
    {
        //DICIONÁRIO MAPEANDO STRING PARA NÚMERO
        private readonly Dictionary<string, string> stringsCategoriaAdministrativa = new Dictionary<string, string>
        {
            {"1", "Especial"},
            {"2", "Privada"},
            {"3", "Privada"},
            {"4", "Pública" },
            {"5", "Pública" },
            {"6", "Pública" },
        };

        private readonly Dictionary<string, string> stringsGrau = new Dictionary<string, string>
        {
            {"1", "Bacharelado"},
            {"2", "Licenciatura"},
            {"3", "Sequencial"},
            {"4", "Tecnólogo" },
            {"5", "Área Básica de Ingresso (ABI)" },
        };

        private readonly Dictionary<string, string> stringsModalidade = new Dictionary<string, string>
        {
            {"1", "Educação Presencial"},
            {"2", "Educação à Distância"},
        };

        private readonly Dictionary<string, string> stringsRegiao = new Dictionary<string, string>
        {
            {"1", "Centro-Oeste"},
            {"2", "N/A" },
            {"3", "Nordeste" },
            {"4", "Norte" },
            {"5", "Sudeste" },
            {"6", "Sul" },

        };

        public string Converter(string columnName, string number)
        {
            // Seleciona o dicionário correto com base no nome da coluna
            return columnName.ToLower() switch
            {
                "categoria_administrativa" => stringsCategoriaAdministrativa.TryGetValue(number, out var categoria) ? categoria : "Valor não encontrado",
                "grau" => stringsGrau.TryGetValue(number, out var grau) ? grau : "Valor não encontrado",
                "modalidade" => stringsModalidade.TryGetValue(number, out var modalidade) ? modalidade : "Valor não encontrado",
                "regiao" => stringsRegiao.TryGetValue(number, out var regiao) ? regiao : "Valor não encontrado",
                _ => "Coluna inválida"
            };
        }

    }
}