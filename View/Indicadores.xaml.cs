namespace Extensionista.View;

public partial class Indicadores : ContentPage
{
	public Indicadores()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private void OnRegiaoClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            // Aqui você pode capturar o texto do botão clicado
            string regiaoSelecionada = button.Text;

            // Exemplo de lógica futura
            DisplayAlert("Região Selecionada", $"Você selecionou a região: {regiaoSelecionada}", "OK");

            // TODO: Adicione a lógica necessária para carregar dados ou navegar para outra página
        }
    }

}