using Extensionista.Repositories;
using Extensionista.Models;
namespace Extensionista;

public partial class PaginaTesteBackEnd : ContentPage
{
    public PaginaTesteBackEnd()
    {
        InitializeComponent();
        CarregarCursos();
    }

    private void CarregarCursos()
    {
        var repository = new CursosGeralRepository();
        List<Universidades> universidades = repository.ObterUniversidades("5");
        List<Cursos> cursos = repository.ObterCursos(1);

        // Atribuindo a lista de cursos ao CollectionView
        CursosCollectionView.ItemsSource = cursos;
        UniversidadesCollectionView.ItemsSource = universidades;


    }
}