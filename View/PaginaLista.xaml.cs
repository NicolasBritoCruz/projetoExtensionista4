namespace Extensionista;

public partial class PaginaLista : ContentPage
{
    private int codigoIES;
    public PaginaLista(int codigoIES)
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

        this.codigoIES = codigoIES;

        // Exibe um pop-up para confirmar que o valor foi passado corretamente
        DisplayAlert("C�digo IES Recebido", $"O c�digo IES passado �: {codigoIES}", "OK");
    }

    private async void sairLista_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PaginaPesquisa());
    }
}