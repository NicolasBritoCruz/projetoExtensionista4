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
        if (sender is Element element && element.BindingContext is SisuCursos SelectedCurso)
        {
            try
            {
                string IdCurso = SelectedCurso.ID_CURSO;
                await Navigation.PushAsync(new PaginaIndicadores(IdCurso));
            }
            catch (Exception ex)
            {
                // Log ou mensagem de erro para o usuário
                await DisplayAlert("Erro", $"Ocorreu um erro ao verificar o SISU: {ex.Message}", "OK");
            }
        }
    }
}