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
                    // Obtém o CODIGO_IES da tabela Universidades
                    var universidadesRepository = new CursosGeralRepository(); // Assuma que o repositório existe
                    var universidade = universidadesRepository.ObterUniversidade(selectedFavorito.ID_UNIVERSIDADE);

                    if (universidade == null)
                    {
                        await DisplayAlert("Erro", "Universidade não encontrada.", "OK");
                        return;
                    }

                    int codigoIES = universidade.CODIGO_IES; // Aqui você pega o atributo CODIGO_IES

                    // Verifica se a universidade está no SISU
                    var sisuCursosRepository = new SisuCursosRepository();
                    var cursosSisu = sisuCursosRepository.ObterCursosSisuCidade(codigoIES.ToString(), selectedFavorito.MUNICIPIO);
                    bool estaNoSisu = cursosSisu.Any();

                    if (estaNoSisu)
                    {
                        await Navigation.PushAsync(new PaginaListaS(selectedFavorito.ID_UNIVERSIDADE, selectedFavorito.MUNICIPIO));
                    }
                    else
                    {
                        await Navigation.PushAsync(new PaginaLista(selectedFavorito.ID_UNIVERSIDADE, estaNoSisu));
                    }
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
