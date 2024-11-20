using Extensionista.Models;
using Extensionista.Repositories;

namespace Extensionista.View;

public partial class PaginaCurso : ContentPage
{
    private Cursos _cursoSelecionado;
    private string _nomeUniversidade;
    public PaginaCurso(Cursos curso)
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

        _cursoSelecionado = curso;

        var repository = new CursosGeralRepository();
        var univerisidade = repository.ObterUniversidade(curso.ID_UNIVERSIDADE);
        _nomeUniversidade = univerisidade.NOME_IES;

        labelNome.Text = _nomeUniversidade;

        this.BindingContext = _cursoSelecionado;

        CarregarInformacoesSisu();
    }

    private void CarregarInformacoesSisu()
    {
        string CodigoIes = _cursoSelecionado.CODIGO_CURSO.ToString();
        var sisuCursosRepository = new SisuCursosRepository();
        var cursosSisu = sisuCursosRepository.ObterCursosSisu(CodigoIes);

        PesoNotasSisu.ItemsSource = cursosSisu.Take(1).ToList();
    }

    private async void sairCurso_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopAsync();
    }
}