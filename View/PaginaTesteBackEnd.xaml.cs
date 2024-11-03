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
        List<CursosGeral> cursos = repository.ObterTodosCursos();

        // Atribuindo a lista de cursos ao CollectionView
        CursosCollectionView.ItemsSource = cursos;
    }
}