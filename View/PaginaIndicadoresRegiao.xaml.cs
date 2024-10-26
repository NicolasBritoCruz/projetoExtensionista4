namespace Extensionista;

public partial class PaginaIndicadoresRegiao : ContentPage
{
	public PaginaIndicadoresRegiao()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void btnOfertas_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PaginaListaSisu());
    }

    private async void sairIndicadoresRegiao_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PaginaSisu());
    }
}