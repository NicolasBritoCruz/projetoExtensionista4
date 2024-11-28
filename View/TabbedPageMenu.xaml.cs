using Extensionista.View;

namespace Extensionista;

public partial class TabbedPageMenu : TabbedPage
{
    public TabbedPageMenu()
    {
        InitializeComponent();

        // Configura a página de pesquisa com NavigationPage
        var paginaPesquisa = new NavigationPage(new PaginaPesquisa())
        {
            Title = "Pesquisa",
            IconImageSource = "pesquisa.png"
        };

        // Configura a página de favoritos com NavigationPage
        var paginaFavoritos = new NavigationPage(new PaginaFavoritos())
        {
            Title = "Favoritos",
            IconImageSource = "favoritos.png"
        };

        var paginaIndicadores = new NavigationPage(new Indicadores())
        {
            Title = "Indicadores",
            IconImageSource = "indicadores.png"
        };

        // Adiciona as páginas ao TabbedPage
        Children.Add(new PaginaInicio()); // Página inicial não precisa de NavigationPage
        Children.Add(paginaPesquisa);
        Children.Add(paginaFavoritos);
        Children.Add(paginaIndicadores);
    }
}