using Extensionista.Models;
using Extensionista.Repositories;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Extensionista
{
    public partial class PaginaPesquisa : ContentPage
    {
        public ObservableCollection<Universidades> UniversidadesList { get; set; } = new ObservableCollection<Universidades>();
        private int selectedCodigoIES;
        private readonly CursosGeralRepository _cursosGeralRepository;

        public PaginaPesquisa()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            _cursosGeralRepository = new CursosGeralRepository(); // Instanciar o repositório

            // Carrega os dados iniciais no CollectionView de forma assíncrona
            LoadFaculdadesAsync();
            ListaFaculdades.ItemsSource = UniversidadesList;
        }

        // Método assíncrono para carregar as universidades do banco de dados
        private async Task LoadFaculdades()
        {
            // Carrega as universidades do repositório
            var universidades = _cursosGeralRepository.ObterUniversidades();

            // Limpa a coleção atual e adiciona as universidades retornadas
            UniversidadesList.Clear();
            foreach (var universidade in universidades)
            {
                UniversidadesList.Add(universidade);
            }

            // Atualiza o CollectionView
            ListaFaculdades.ItemsSource = UniversidadesList;
        }

        // Método para chamar LoadFaculdades de forma assíncrona
        private async void LoadFaculdadesAsync()
        {
            await LoadFaculdades();
        }

        // Método de pesquisa
        private void Pesquisar_Clicked(object sender, EventArgs e)
        {
            var query = entrySearch.Text?.ToLower();

            // Obtem as universidades com base na pesquisa
            var universidades = _cursosGeralRepository.ObterUniversidades();

            if (!string.IsNullOrEmpty(query))
            {
                universidades = universidades.Where(
                    u => u.NOME_IES.ToLower().Contains(query) || u.MUNICIPIO.ToLower().Contains(query)
                ).ToList();
            }

            // Atualiza a lista com os resultados filtrados
            ListaFaculdades.ItemsSource = new ObservableCollection<Universidades>(universidades);
        }

        // Evento de seleção de uma universidade
        private async void OnFaculdadeSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Universidades selectedFaculdade)
            {
                selectedCodigoIES = selectedFaculdade.CODIGO_IES;
                // Navega para a PaginaLista e passa o selectedCodigoIES como parâmetro
                await Navigation.PushAsync(new PaginaLista(selectedCodigoIES));
            }
        }
    }
}
