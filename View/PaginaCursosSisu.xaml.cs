namespace Extensionista;

public partial class PaginaCursosSisu : ContentPage
{
	public PaginaCursosSisu()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void sairCursosSisu_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PaginaListaSisu());
    }
}