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
        private int currentPage = 0;
        private const int PageSize = 30;
        private bool isLoading = false;
        private bool hasMoreItems = true;
        private object lastVisibleItem;
        private bool isSearching = false;
        private bool isLoaded = false;

        public PaginaPesquisa()
        {
            InitializeComponent();
            BindingContext = this;

            _cursosGeralRepository = new CursosGeralRepository();
            ListaFaculdades.ItemsSource = UniversidadesList;

            NavigationPage.SetHasNavigationBar(this, false);

            // Inicializar com carregamento inicial
            LoadFaculdadesAsync();
        }

        private async Task LoadFaculdades(int page)
        {
            if (isLoading || !hasMoreItems) return;

            isLoading = true;
            try
            {
                var universidades = await Task.Run(() =>
                    _cursosGeralRepository.ObterUniversidades()
                                          .Skip(page * PageSize)
                                          .Take(PageSize)
                                          .ToList());

                if (!universidades.Any())
                {
                    hasMoreItems = false;
                    return;
                }

                if (isSearching)
                {
                    // Não adicione novos itens ao resultado da pesquisa, já que estamos filtrando
                    return;
                }

                // Caso não esteja pesquisando, adicione os itens normalmente
                foreach (var universidade in universidades)
                {
                    if (!UniversidadesList.Contains(universidade))
                    {
                        UniversidadesList.Add(universidade);
                    }
                }
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


        private async Task LoadFaculdadesAsync()
        {
            if (UniversidadesList.Count > 0)
            {
                // Se já houver universidades na lista, não faça o carregamento
                return;
            }

            currentPage = 0;
            hasMoreItems = true;
            UniversidadesList.Clear(); // Limpa a lista antes de carregar os novos dados, caso contrário, pode ocorrer duplicação.

            await LoadFaculdades(currentPage); // Carrega os dados da primeira vez

            // Pré-carrega o próximo lote
            _ = LoadFaculdades(currentPage + 1);
        }


        private void Pesquisar_Clicked(object sender, EventArgs e)
        {
            var query = entrySearch.Text?.ToLower();

            if (string.IsNullOrEmpty(query))
            {
                // Se não houver pesquisa, mostra todos os itens carregados
                isSearching = false;

                // Recarrega todos os itens carregados previamente
                var allUniversidades = _cursosGeralRepository.ObterUniversidades();
                UniversidadesList.Clear();
                foreach (var universidade in allUniversidades)
                {
                    UniversidadesList.Add(universidade);
                }
            }
            else
            {
                // Marca que está pesquisando
                isSearching = true;

                // Filtra os itens carregados localmente
                var filteredUniversidades = UniversidadesList
                    .Where(u => u.NOME_IES.ToLower().Contains(query) || u.MUNICIPIO.ToLower().Contains(query))
                    .ToList();

                // Limpa a lista atual e exibe apenas os itens filtrados
                UniversidadesList.Clear();
                foreach (var universidade in filteredUniversidades)
                {
                    UniversidadesList.Add(universidade);
                }

                // Caso a pesquisa não tenha encontrado o suficiente, busque no banco de dados
                var additionalUniversidades = _cursosGeralRepository.ObterUniversidades()
                    .Where(u => u.NOME_IES.ToLower().Contains(query) || u.MUNICIPIO.ToLower().Contains(query))
                    .ToList();

                // Adiciona os itens encontrados no banco à lista, verificando duplicação
                foreach (var universidade in additionalUniversidades)
                {
                    // Verifica se a universidade já não foi carregada (compara pelo código único)
                    if (!UniversidadesList.Any(u => u.CODIGO_IES == universidade.CODIGO_IES))
                    {
                        UniversidadesList.Add(universidade);
                    }
                }
            }
        }




        private async void OnFaculdadeSelected(object sender, EventArgs e)
        {
            if (sender is Element element && element.BindingContext is Universidades selectedFaculdade)
            {
                lastVisibleItem = selectedFaculdade; // Salva o último item selecionado
                var selectedCodigoIES = selectedFaculdade.CODIGO_IES;
                var selectedMunicipio = selectedFaculdade.MUNICIPIO;

                await Navigation.PushAsync(new PaginaLista(selectedCodigoIES, selectedMunicipio));
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Verifica se a lista já foi carregada
            if (UniversidadesList.Count == 0 && !isLoading) // Certifique-se de que a lista está vazia e não está carregando
            {
                await LoadFaculdadesAsync(); // Carrega os dados pela primeira vez
            }

            // Caso haja um item visível, rola até ele
            if (lastVisibleItem != null)
            {
                ListaFaculdades.ScrollTo(lastVisibleItem, position: ScrollToPosition.Start);
            }
        }



        private async void OnRemainingItemsThresholdReached(object sender, EventArgs e)
        {
            if (isLoading || !hasMoreItems) return;

            // Armazene o último item visível
            lastVisibleItem = UniversidadesList.LastOrDefault();

            currentPage++;
            await LoadFaculdades(currentPage);
        }

    }
}