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


        public List<CursosGeral> ObterTodosCursos()
        {
            var cursos = _connection.Table<CursosGeral>().Take(100).ToList();

            // Converte os valores numéricos para strings para cada curso
            foreach (var curso in cursos)
            {
                curso.CATEGORIA_ADMINISTRATIVA = _decompressor.Converter("categoria_administrativa", curso.CATEGORIA_ADMINISTRATIVA);
                curso.GRAU = _decompressor.Converter("grau", curso.GRAU);
                curso.MODALIDADE = _decompressor.Converter("modalidade", curso.MODALIDADE);
                curso.REGIAO = _decompressor.Converter("regiao", curso.REGIAO);
            }

            return cursos;
        }
    }
}
