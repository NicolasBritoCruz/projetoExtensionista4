using Extensionista.Drawables;
using Extensionista.Repositories;

namespace Extensionista.View;

public partial class PaginaIndicadores : ContentPage
{
    public PaginaIndicadores(string IdCurso)
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

        // Obter os dados do repositório
        var repository = new SisuModalidadesRepository();
        var modalidades = repository.ObterCursosPorIdCurso(IdCurso);

        // Configurar o drawable
        var drawable = new GraficoBarrasDrawable
        {
            Modalidades = modalidades, // Atribuir diretamente
            Rotulos = new List<string> { "AC", "RaReEs", "RaEs", "Es", "DeEs", "DeReEs", "ReEs", "?", "De" }
        };

        // Associar ao GraphicsView e atualizar
        BarrasGrafico.Drawable = drawable;
        BarrasGrafico.Invalidate();
    }


    private async void sairIndicadores_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
