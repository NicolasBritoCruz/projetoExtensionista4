namespace Extensionista;

public partial class PaginaListaSisu : ContentPage
{
	public PaginaListaSisu()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void btnCursosSisu_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new PaginaCursosSisu());
    }

    private async void sairListaSisu_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PaginaIndicadoresRegiao());
    }
}