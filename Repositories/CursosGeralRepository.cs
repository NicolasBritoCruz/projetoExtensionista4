using System;
using System.Collections.Generic;
using SQLite;
using Extensionista.Models;

namespace Extensionista.Repositories
{
    public class CursosGeralRepository
    {
        private readonly SQLiteConnection _connection;
        private readonly Decompressor _decompressor;

        public CursosGeralRepository()
        {
            _connection = DataBaseContext.connection;
            _decompressor = new Decompressor();
        }

        public Universidades ObterUniversidade(int idUniversidade)
        {
            var universidade = _connection.Table<Universidades>()
                                        .Where(u => u.ID_UNIVERSIDADE == idUniversidade)
                                        .FirstOrDefault();

            if (universidade != null)
            {
                universidade.CATEGORIA_ADMINISTRATIVA = _decompressor.Converter("categoria_administrativa", universidade.CATEGORIA_ADMINISTRATIVA);
                universidade.REGIAO = _decompressor.Converter("regiao", universidade.REGIAO);
            }

            return universidade;
        }

        public List<Universidades> ObterUniversidades(int? codigoIES = null, string municipio = null, int page = 1)
        {
            var query = _connection.Table<Universidades>().AsQueryable();
            int pageSize = 20; // Número de resultados por página
            int skipAmount = (page - 1) * pageSize; // Quantidade de registros a ignorar

            if (codigoIES.HasValue)
            {
                // Se um código específico for passado, filtra pela faculdade com esse código
                query = query.Where(u => u.CODIGO_IES == codigoIES.Value);
            }

            if (!string.IsNullOrEmpty(municipio))
            {
                query = query.Where(u => u.MUNICIPIO.ToLower().Contains(municipio.ToLower()));
            }

            var universidades = query
                .Skip(skipAmount)
                .Take(20)
                .ToList();

            // Converte os valores numéricos para strings para cada curso
            foreach (var universidade in universidades)
            {
                universidade.CATEGORIA_ADMINISTRATIVA = _decompressor.Converter("categoria_administrativa", universidade.CATEGORIA_ADMINISTRATIVA);
                universidade.REGIAO = _decompressor.Converter("regiao", universidade.REGIAO);
            }

            return universidades;
        }

        public List<Cursos> ObterCursos(int idUniversidade)
        {
            var cursos = _connection.Table<Cursos>()
                                    .Where(c => c.ID_UNIVERSIDADE == idUniversidade)
                                    .ToList();

            // Converte os valores numéricos para strings para cada curso
            foreach (var curso in cursos)
            {
                curso.GRAU = _decompressor.Converter("grau", curso.GRAU);
                curso.MODALIDADE = _decompressor.Converter("modalidade", curso.MODALIDADE);
            }

            return cursos;
        }
    }
}