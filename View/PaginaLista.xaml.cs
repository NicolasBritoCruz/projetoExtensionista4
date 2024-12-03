using Extensionista.Models;
using Extensionista.Repositories;
using Extensionista.View;
using System.Linq;

namespace Extensionista
{
    public partial class PaginaLista : ContentPage
    {
        private int codigoIES;
        private string municipio;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Atualiza o estado de favoritos sempre que a página aparecer
            AtualizarListaFavoritos();
        }

        public PaginaLista(int idUniversidade, bool estaNoSisu)
        {
            InitializeComponent();
            CarregarCursos(idUniversidade, estaNoSisu);
            NavigationPage.SetHasNavigationBar(this, false);

            // Escuta mensagens para atualizar favoritos
            MessagingCenter.Subscribe<PaginaFavoritos>(this, "AtualizarFavoritos", (sender) =>
            {
                AtualizarListaFavoritos();
            });
        }

        private async void sairLista_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void CarregarCursos(int idUniversidade, bool estaNoSisu)
        {
            var repository = new CursosGeralRepository();
            var universidades = repository.ObterUniversidade(idUniversidade);

            if (universidades != null)
            {
                var cursos = repository.ObterCursos(idUniversidade);

                var favoritosRepository = new FavoritosRepository();
                var favoritos = favoritosRepository.ObterFavoritos();

                // Atualiza o estado de favoritado dos cursos
                foreach (var curso in cursos)
                {
                    curso.Favorito = favoritos.Any(f => f.ID_UNIVERSIDADE == curso.ID_UNIVERSIDADE);
                    curso.CODIGO_CURSO = universidades.CODIGO_IES;
                }

                for (int i = 0; i < cursos.Count; i++)
                {
                    cursos[i].Index = i;
                }

                // Define a fonte do ListView ou CollectionView
                ListaCursos.ItemsSource = cursos;

                // Atualiza o estado de favorito da universidade
                universidades.Favorito = favoritos.Any(f => f.ID_UNIVERSIDADE == universidades.ID_UNIVERSIDADE);

                // Atualiza o ícone de favoritar
                AtualizarIconeFavoritar(universidades.Favorito);

                this.BindingContext = universidades;
 
            }
        }

        private void AtualizarListaFavoritos()
        {
            var favoritosRepository = new FavoritosRepository();
            var favoritos = favoritosRepository.ObterFavoritos();

            // Atualiza o estado dos favoritos na lista de cursos
            if (ListaCursos.ItemsSource is IList<Cursos> cursos)
            {
                foreach (var curso in cursos)
                {
                    curso.Favorito = favoritos.Any(f => f.ID_UNIVERSIDADE == curso.ID_UNIVERSIDADE);
                }

                // Atualiza a fonte de dados para refletir as mudanças
                ListaCursos.ItemsSource = null;
                ListaCursos.ItemsSource = cursos;
            }

            // Atualiza o ícone de favoritar da universidade
            if (BindingContext is Universidades universidade)
            {
                universidade.Favorito = favoritos.Any(f => f.ID_UNIVERSIDADE == universidade.ID_UNIVERSIDADE);
                AtualizarIconeFavoritar(universidade.Favorito);
            }
        }

        private void AtualizarIconeFavoritar(bool isFavorito)
        {
            // Verifica o estado de favoritado e altera a imagem
            favoritar.Source = isFavorito ? "fullheart.png" : "heart.png";
        }

        private void favoritar_Clicked(object sender, EventArgs e)
        {
            if (BindingContext is Universidades universidade)
            {
                var favoritosRepository = new FavoritosRepository();

                // Alterna o estado de favoritado
                universidade.Favorito = !universidade.Favorito;

                // Salva ou remove do banco de dados
                var favorito = new Favoritos
                {
                    NOME_IES = universidade.NOME_IES,
                    ID_UNIVERSIDADE = universidade.ID_UNIVERSIDADE,
                    MUNICIPIO = universidade.MUNICIPIO,
                };

                if (universidade.Favorito)
                {
                    favoritosRepository.Favoritar(favorito); // Salva no banco de dados
                }
                else
                {
                    favoritosRepository.Delete(favorito); // Remove do banco de dados
                }

                AtualizarIconeFavoritar(universidade.Favorito);

                // Notifica outras páginas sobre a mudança
                MessagingCenter.Send(this, "AtualizarFavoritos");
            }
        }
    }
}
