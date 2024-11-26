using Extensionista.Models;
using Extensionista.Repositories;

namespace Extensionista.View;

public partial class PaginaCurso : ContentPage
{
    public PaginaCurso()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
       
    }
    private async void sairCurso_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopAsync();
    }
}