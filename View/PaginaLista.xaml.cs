
using Extensionista.Models;
using Extensionista.Repositories;
namespace Extensionista;

public partial class PaginaLista : ContentPage
{
    private int codigoIES;
    private string municipio;
    bool favoritado = false;
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

            for (int i = 0; i < cursos.Count; i++)
            {
                cursos[i].Index = i;
            }

            ListaCursos.ItemsSource = cursos;
            this.BindingContext = universidades;
        }
    }
    private void favoritar_Clicked(object sender, EventArgs e)
    {
        favoritado = !favoritado;

        var coracaoVazio = "heart.png";
        var coracaoCheio = "fullheart.png";

        var button = favoritar;

        favoritar.Source = (favoritado == false) ? ImageSource.FromFile(coracaoVazio) : ImageSource.FromFile(coracaoCheio);
    }
}



