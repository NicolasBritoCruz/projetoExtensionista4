namespace Extensionista.View;

public partial class PaginaIndicadores : ContentPage
{
	public PaginaIndicadores()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void sairIndicadores_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopAsync();
    }
}