namespace Extensionista.View;

public partial class PaginaCurso : ContentPage
{
	public PaginaCurso()
	{
		InitializeComponent();
	}

    private async void sairCurso_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopAsync();
    }
}