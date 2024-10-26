namespace Extensionista;

public partial class PaginaLista : ContentPage
{
	public PaginaLista()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void sairLista_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PaginaPesquisa());
    }
}