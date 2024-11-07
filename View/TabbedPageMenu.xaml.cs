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

    }
}