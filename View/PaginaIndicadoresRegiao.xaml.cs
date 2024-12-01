using Extensionista.Repositories;

namespace Extensionista.View;

public partial class PaginaIndicadoresRegiao : ContentPage
{
	public PaginaIndicadoresRegiao(string regiaoSelecionada)
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

        var repository = new SisuCursosRepository();

        LabelIndicadoresRegiao.Text = regiaoSelecionada;
      
       
    }

    private async void sairIndicadoresS_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}