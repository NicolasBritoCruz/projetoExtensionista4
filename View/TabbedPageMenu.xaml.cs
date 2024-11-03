namespace Extensionista;

public partial class TabbedPageMenu : TabbedPage
{
	public TabbedPageMenu() {

        InitializeComponent();

		var paginaPesquisa = new NavigationPage(new PaginaPesquisa());
		paginaPesquisa.Title = "Pesquisa";
		paginaPesquisa.IconImageSource = "pesquisa.png";

		Children.Insert(1, paginaPesquisa);
		Children.Add(paginaPesquisa);

		var paginaSisu = new NavigationPage(new PaginaSisu());
		paginaSisu.Title = "Sisu";
		
		Children.Insert(2, paginaSisu);
		Children.Add(paginaSisu);

		var paginaListaSisu = new NavigationPage(new PaginaListaSisu());
		paginaListaSisu.Title = "PaginaListaSisu";

		var paginaCursosSisu = new NavigationPage(new PaginaCursosSisu());
		paginaCursosSisu.Title = "PaginaCursosSisu";

		var paginaTesteBackEnd = new NavigationPage(new PaginaTesteBackEnd());
        paginaTesteBackEnd.Title = "PaginaTesteBackEnd";

    }
}