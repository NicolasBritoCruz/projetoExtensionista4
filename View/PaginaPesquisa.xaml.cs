using Extensionista.Models;
using Extensionista.Repositories;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Extensionista
{
    public partial class PaginaPesquisa : ContentPage
    {
        public ObservableCollection<Universidades> UniversidadesList { get; set; } = new ObservableCollection<Universidades>();
        private readonly CursosGeralRepository _cursosGeralRepository;
        private int currentPage = 1;
        private bool hasMoreItems = true;
        private bool isLoading = false;

        public PaginaPesquisa()
        {
            InitializeComponent();
            BindingContext = this;
            _cursosGeralRepository = new CursosGeralRepository();
            ListaFaculdades.ItemsSource = UniversidadesList;
            NavigationPage.SetHasNavigationBar(this, false);

            // Carregar as universidades ao iniciar, sem filtro
            _ = LoadFaculdadesAsync();
        }

        // Carrega a lista de faculdades com paginação e filtros opcionais
        private async Task LoadFaculdadesAsync(int? codigoIES = null, string municipio = null)
        {
            if (isLoading || !hasMoreItems) return;

            isLoading = true;
            try
            {
                // Chama a função do repositório diretamente com paginação e filtros
                var universidades = await Task.Run(() =>
                    _cursosGeralRepository.ObterUniversidades(codigoIES, municipio, currentPage));

                if (universidades.Count == 0)
                {
                    hasMoreItems = false; // Marca como última página se não houver mais itens
                    return;
                }

                foreach (var universidade in universidades)
                {
                    if (!UniversidadesList.Contains(universidade))
                    {
                        UniversidadesList.Add(universidade);
                    }
                }

                currentPage++; // Incrementa para a próxima página
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar universidades: {ex.Message}");
            }
            finally
            {
                isLoading = false;
            }
        }

        // Método acionado ao alcançar o limite de rolagem, carregando a próxima página
        private async void OnRemainingItemsThresholdReached(object sender, EventArgs e)
        {
            await LoadFaculdadesAsync();
        }

        // Método de pesquisa otimizado para usar filtros diretamente no repositório
        private void Pesquisar_Clicked(object sender, EventArgs e)
        {
            var query = entrySearch.Text?.ToLower();
            UniversidadesList.Clear();
            currentPage = 1;
            hasMoreItems = true;

            // Determina se o usuário está buscando por código ou município e ajusta a pesquisa
            if (int.TryParse(query, out int codigoIES))
            {
                _ = LoadFaculdadesAsync(codigoIES: codigoIES);
            }
            else if (!string.IsNullOrEmpty(query))
            {
                _ = LoadFaculdadesAsync(municipio: query);
            }
            else
            {
                _ = LoadFaculdadesAsync(); // Recarrega a lista completa se a pesquisa estiver vazia
            }
        }

        // Método para manipular a seleção de um item na lista
        private async void OnFaculdadeSelected(object sender, EventArgs e)
        {
            if (sender is Element element && element.BindingContext is Universidades selectedFaculdade)
            {
                await Navigation.PushAsync(new PaginaLista(selectedFaculdade.ID_UNIVERSIDADE));
            }
        }

        // Carrega a lista ao exibir a página, sem duplicar itens
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (UniversidadesList.Count == 0 && !isLoading)
            {
                await LoadFaculdadesAsync();
            }
        }
    }
}
