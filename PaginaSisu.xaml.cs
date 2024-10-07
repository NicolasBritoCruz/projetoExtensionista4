namespace Extensionista;

public partial class PaginaSisu : ContentPage
{
	public PaginaSisu()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void btn_indicadores_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PaginaIndicadoresRegiao());
    }
}