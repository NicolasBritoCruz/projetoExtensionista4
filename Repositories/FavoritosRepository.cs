using System;
using System.Collections.Generic;
using SQLite;
using Extensionista.Models;

namespace Extensionista.Repositories
{
    public class FavoritosRepository
    {
        private readonly SQLiteConnection _connection;

        public FavoritosRepository()
        {
            _connection = DataBaseContext.connection;
        }

        public void Favoritar(Favoritos favorito)
        {
            _connection.Insert(favorito);
        }

        public List<Favoritos> ObterFavoritos()
        {
            return _connection.Table<Favoritos>().ToList();
        }
        public void Delete(Favoritos favorito)
        {
            _connection.Delete(favorito);
        }

    }
}