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
            var existeFavorito = _connection.Table<Favoritos>()
                                             .Any(f => f.ID_UNIVERSIDADE == favorito.ID_UNIVERSIDADE);

            if (!existeFavorito)
            {
                _connection.Insert(favorito);
            }
        }


        public List<Favoritos> ObterFavoritos()
        {
            return _connection.Table<Favoritos>().ToList();
        }
        public void Delete(Favoritos favorito)
        {
            // A remoção deve usar o mesmo código de IES
            var favoritoExistente = _connection.Table<Favoritos>()
                                                .FirstOrDefault(f => f.ID_UNIVERSIDADE == favorito.ID_UNIVERSIDADE);

            if (favoritoExistente != null)
            {
                _connection.Delete(favoritoExistente);
            }
        }


    }
}