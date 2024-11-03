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
        Curso curso = repository.ObterCurso(1562800);

        // Atribuindo a lista de cursos ao CollectionView
        CursoStackLayout.BindingContext = curso;
        UniversidadesCollectionView.ItemsSource = universidades;


    }
}