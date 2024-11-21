using Extensionista.Models;
using Extensionista.View;
using Extensionista.Repositories;
using System.Collections.ObjectModel;

namespace Extensionista
{
    public partial class PaginaFavoritos : ContentPage
    {
        public PaginaFavoritos()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

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

        private async void OnFavoritoSelected(object sender, EventArgs e)
        {
            if (sender is Element element && element.BindingContext is Favoritos selectedFavorito)
            {
                try
                {
                    // Verifica se a universidade está no SISU
                    var sisuCursosRepository = new SisuCursosRepository();
                    var cursosSisu = sisuCursosRepository.ObterCursosSisu(selectedFavorito.ID_UNIVERSIDADE.ToString());
                    bool estaNoSisu = cursosSisu.Any();

                    // Navega para a página de lista com os parâmetros corretos
                    await Navigation.PushAsync(new PaginaLista(selectedFavorito.ID_UNIVERSIDADE, estaNoSisu));
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro", $"Erro ao verificar no SISU: {ex.Message}", "OK");
                }
            }
        }

        private void RemoverFavorito_Clicked(object sender, EventArgs e)
        {
            if (sender is ImageButton button && button.CommandParameter is Favoritos favorito)
            {
                var favoritosRepository = new FavoritosRepository();

                // Remove do banco de dados
                favoritosRepository.Delete(favorito);

                // Atualiza a lista de favoritos
                CarregarFavoritos();

                MessagingCenter.Send(this, "AtualizarFavoritos");
            }
        }
    }
}
