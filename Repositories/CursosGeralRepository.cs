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
                .GroupBy(u => new { u.CODIGO_IES, u.MUNICIPIO }) // Agrupa por combinação de CODIGO_IES e MUNICIPIO
                .Select(g => g.First())
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

        public List<Cursos> ObterCursos(int codigoIes, string municipio)
        {
            var cursos = _connection.Table<Cursos>()
                                    .Where(c => c.CODIGO_IES == codigoIes && c.MUNICIPIO == municipio)
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