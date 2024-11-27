using Extensionista.Models;
using Extensionista.Repositories;

namespace Extensionista.View;

public partial class PaginaCurso : ContentPage
{
    private string _IdCurso;
    public PaginaCurso(string IdCurso)
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        _IdCurso = IdCurso;
        CarregarCurso(IdCurso);

    }
    private async void sairCurso_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopAsync();
    }

    private void CarregarCurso(string IdCurso)
    {
        var sisuRepository = new SisuCursosRepository();
        var curso = sisuRepository.ObterCursoSisuID(IdCurso)?.FirstOrDefault();
     
        this.BindingContext = curso;
        PesoNotasSisu.ItemsSource = new List<SisuCursos> { curso };
    
    }

    private async void IndicadoresButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PaginaIndicadores());
    }
}