namespace Extensionista;

public partial class PaginaPesquisa : ContentPage
{
	public PaginaPesquisa()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void pesquisar_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new PaginaLista());
    }
}