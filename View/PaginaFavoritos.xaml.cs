using Extensionista.Models;
using Extensionista.Repositories;
using System.Collections.ObjectModel;

namespace Extensionista
{
    public partial class PaginaFavoritos : ContentPage
    {
        public PaginaFavoritos()
        {
            InitializeComponent();

            // Recebe a mensagem para atualizar a lista de favoritos
            MessagingCenter.Subscribe<PaginaLista>(this, "AtualizarFavoritos", (sender) =>
            {
                CarregarFavoritos();
            });

            CarregarFavoritos();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Atualiza a lista de favoritos sempre que a página aparecer
            CarregarFavoritos();
        }

        public void CarregarFavoritos()
        {
            var favoritosRepository = new FavoritosRepository();
            var favoritos = favoritosRepository.ObterFavoritos();

            ListaFavoritos.ItemsSource = favoritos; // Atualiza a lista de favoritos
        }        
      
    }
}
