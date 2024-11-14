using Extensionista.Models;
using Extensionista.Repositories;
using System.Linq;

namespace Extensionista
{
    public partial class PaginaLista : ContentPage
    {
        private int codigoIES;
        private string municipio;

        public PaginaLista(int idUniversidade)
        {
            InitializeComponent();
            CarregarCursos(idUniversidade);
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void sairLista_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void CarregarCursos(int idUniversidade)
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
                    var favorito = favoritos.FirstOrDefault(f => f.ID_UNIVERSIDADE == curso.ID_UNIVERSIDADE);
                    curso.Favorito = favorito != null;  // Marca como favoritado ou não
                }

                for (int i = 0; i < cursos.Count; i++)
                {
                    cursos[i].Index = i;
                }

                // Define a fonte do ListView ou CollectionView
                ListaCursos.ItemsSource = cursos;

                // Atualiza o estado de favorito da universidade
                var favoritoUniversidade = favoritos.FirstOrDefault(f => f.ID_UNIVERSIDADE == universidades.ID_UNIVERSIDADE);
                universidades.Favorito = favoritoUniversidade != null;

                // Atualiza o ícone de favoritar
                AtualizarIconeFavoritar(universidades.Favorito);

                this.BindingContext = universidades;
            }
        
        }
        private void AtualizarIconeFavoritar(bool isFavorito)
        {
            // Verifica o estado de favoritado e altera a imagem
            favoritar.Source = isFavorito ? "fullheart.png" : "heart.png";
        }


        private void favoritar_Clicked(object sender, EventArgs e)
        {
            var button = sender as ImageButton;
            if (button == null) return;

            var universidade = button.BindingContext as Universidades;
            if (universidade == null) return;

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

            MessagingCenter.Send(this, "AtualizarFavoritos");
        }
    }
}
