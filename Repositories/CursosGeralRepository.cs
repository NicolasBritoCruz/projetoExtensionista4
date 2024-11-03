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


        public List<Universidades> ObterUniversidades(string filterRegiao)
        {
            var universidades = _connection.Table<Universidades>()
                                   .Where(u => u.REGIAO == filterRegiao)
                                   .GroupBy(u => u.CODIGO_IES) // Agrupa por CODIGO_IES
                                   .Select(g => g.First()) // Seleciona o primeiro de cada grupo
                                   .Take(100)
                                   .ToList();

            // Converte os valores numéricos para strings para cada curso
            foreach (var universidade in universidades)
            {
                universidade.CATEGORIA_ADMINISTRATIVA = _decompressor.Converter("categoria_administrativa",universidade.CATEGORIA_ADMINISTRATIVA);
                universidade.REGIAO = _decompressor.Converter("regiao", universidade.REGIAO);
            }

            return universidades;
        }

        public Curso ObterCurso(int codigoCurso)
        {
            var curso = _connection.Table<Curso>()
                                   .Where(c => c.CODIGO_CURSO == codigoCurso)
                                   .FirstOrDefault();

            // Converte os valores numéricos para strings para cada curso
            if (curso != null)
            {
                curso.GRAU = _decompressor.Converter("grau", curso.GRAU);
                curso.MODALIDADE = _decompressor.Converter("modalidade", curso.MODALIDADE);
            }

            return curso;
        }   
    }
}
