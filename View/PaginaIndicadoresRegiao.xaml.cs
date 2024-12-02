using Extensionista.Drawables;
using Extensionista.Repositories;
using Microsoft.Maui.Controls;

namespace Extensionista.View;

public partial class PaginaIndicadoresRegiao : ContentPage
{
    public PaginaIndicadoresRegiao(string regiaoSelecionada)
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        var repository = new IndicadoresRepository();
        int vagas = repository.ObterVagasRegiao(regiaoSelecionada.ToUpper()); //ta retornadno nao
        var vagasturno = repository.ObterVagasRegiaoTurno(regiaoSelecionada.ToUpper()); //passar direito, tudo maisuclo.
        LabelIndicadoresRegiao.Text = regiaoSelecionada;
        LabelVagasRegiao.Text = $"No ano de 2023, o programa do SISU ofereceu {vagas} vagas para a região {regiaoSelecionada}" + ".";

        var drawable = new GraficoVagasTurno
        {
            VagasPorTurno = vagasturno
        };

        GraficoVagasTurnos.Drawable = drawable;
        GraficoVagasTurnos.Invalidate();
    }


    private async void sairIndicadoresS_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync(); 
    }
}
