using Extensionista.Models;
using Extensionista.Repositories;
using System.Linq;

namespace Extensionista
{
    public partial class PaginaLista : ContentPage
    {
        private int codigoIES;
        private string municipio;

        public PaginaLista(int codigoIES, string municipio)
        {
            InitializeComponent();
            CarregarCursos(codigoIES, municipio);
            NavigationPage.SetHasNavigationBar(this, false);

            this.codigoIES = codigoIES;
            this.municipio = municipio;
        }

        private async void sairLista_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PaginaPesquisa());
        }

        private void CarregarCursos(int codigoIES, string municipio)
        {
            var repository = new CursosGeralRepository();
            var universidades = repository.ObterUniversidades(codigoIES, municipio).FirstOrDefault();

            if (universidades != null)
            {
                var cursos = repository.ObterCursos(codigoIES, municipio);

                var favoritosRepository = new FavoritosRepository();
                var favoritos = favoritosRepository.ObterFavoritos();

                // Atualiza o estado de favoritado dos cursos
                foreach (var curso in cursos)
                {
                    var favorito = favoritos.FirstOrDefault(f => f.CODIGO_IES == curso.CODIGO_IES);
                    curso.Favorito = favorito != null;  // Marca como favoritado ou não
                }

                for (int i = 0; i < cursos.Count; i++)
                {
                    cursos[i].Index = i;
                }

                // Define a fonte do ListView ou CollectionView
                ListaCursos.ItemsSource = cursos;

                // Atualiza o estado de favorito da universidade
                var favoritoUniversidade = favoritos.FirstOrDefault(f => f.CODIGO_IES == universidades.CODIGO_IES);
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
                CODIGO_IES = this.codigoIES,
                MUNICIPIO = this.municipio,
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
