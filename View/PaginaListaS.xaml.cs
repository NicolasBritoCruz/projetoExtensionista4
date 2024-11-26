using Extensionista.Models;
using Extensionista.Repositories;

namespace Extensionista.View;

public partial class PaginaListaS : ContentPage
{
	public PaginaListaS(int idUniversidade)
	{
		InitializeComponent();
        CarregarCursosS(idUniversidade);
        NavigationPage.SetHasNavigationBar(this, false);

        MessagingCenter.Subscribe<PaginaFavoritos>(this, "AtualizarFavoritos", (sender) =>
            {
                AtualizarListaFavoritos();
            });
    }

    private async void sairListaS_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnCursoSSelected(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new PaginaCurso());
    }

    private void CarregarCursosS(int idUniversidade)
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
            ListaCursosS.ItemsSource = cursos;

            // Atualiza o estado de favorito da universidade
            universidades.Favorito = favoritos.Any(f => f.ID_UNIVERSIDADE == universidades.ID_UNIVERSIDADE);

            // Atualiza o �cone de favoritar
            AtualizarIconeFavoritar(universidades.Favorito);

            this.BindingContext = universidades;

        }
    }

    private void AtualizarListaFavoritos()
    {
        var favoritosRepository = new FavoritosRepository();
        var favoritos = favoritosRepository.ObterFavoritos();

        // Atualiza o estado dos favoritos na lista de cursos
        if (ListaCursosS.ItemsSource is IList<Cursos> cursos)
        {
            foreach (var curso in cursos)
            {
                curso.Favorito = favoritos.Any(f => f.ID_UNIVERSIDADE == curso.ID_UNIVERSIDADE);
            }

            // Atualiza a fonte de dados para refletir as mudan�as
            ListaCursosS.ItemsSource = null;
            ListaCursosS.ItemsSource = cursos;
        }

        // Atualiza o �cone de favoritar da universidade
        if (BindingContext is Universidades universidade)
        {
            universidade.Favorito = favoritos.Any(f => f.ID_UNIVERSIDADE == universidade.ID_UNIVERSIDADE);
            AtualizarIconeFavoritar(universidade.Favorito);
        }
    }

    private void AtualizarIconeFavoritar(bool isFavorito)
    {
        // Verifica o estado de favoritado e altera a imagem
        favoritarS.Source = isFavorito ? "fullheart.png" : "heart.png";
    }

    private void favoritarS_Clicked(object sender, EventArgs e)
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

            // Notifica outras p�ginas sobre a mudan�a
            MessagingCenter.Send(this, "AtualizarFavoritos");
        }
    }

}