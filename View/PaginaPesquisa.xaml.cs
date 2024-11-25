using Extensionista.Models;
using Extensionista.Repositories;
using Extensionista.View;
using System.Collections.ObjectModel;

namespace Extensionista
{
    public partial class PaginaPesquisa : ContentPage
    {
        public ObservableCollection<Universidades> UniversidadesList { get; set; } = new ObservableCollection<Universidades>();
        private readonly CursosGeralRepository _cursosGeralRepository;
        private readonly SisuCursosRepository _sisuCursosRepository;
        private int currentPage = 1;
        private bool hasMoreItems = true;
        private bool isLoading = false;

        public PaginaPesquisa()
        {
            InitializeComponent();
            BindingContext = this;
            _cursosGeralRepository = new CursosGeralRepository();
            _sisuCursosRepository = new SisuCursosRepository();
            ListaFaculdades.ItemsSource = UniversidadesList;
            NavigationPage.SetHasNavigationBar(this, false);

            // Carregar as universidades ao iniciar, sem filtro
            _ = LoadFaculdadesAsync();
        }

        // Carrega a lista de faculdades com paginação e filtros opcionais
        private async Task LoadFaculdadesAsync(int? codigoIES = null, string municipio = null, string nome = null)
        {
            if (isLoading) return;

            isLoading = true;

            try
            {
                // Chama o repositório com os filtros suportados (código IES e município)
                var universidades = await Task.Run(() =>
                    _cursosGeralRepository.ObterUniversidades(codigoIES, municipio, nome, currentPage));

                // Verifica se ainda há itens a carregar
                if (universidades.Count == 0)
                {
                    hasMoreItems = false;
                    return;
                }

                foreach (var universidade in universidades)
                {
                    UniversidadesList.Add(universidade);
                }

                // Incrementa a página apenas se não houver filtros
                if (codigoIES == null && string.IsNullOrEmpty(municipio) && string.IsNullOrEmpty(nome))
                {
                    currentPage++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar universidades: {ex.Message}");
                await DisplayAlert("Erro", "Não foi possível carregar os dados.", "OK");
            }
            finally
            {
                isLoading = false;
            }
        }

        // Método acionado ao alcançar o limite de rolagem, carregando a próxima página
        private async void OnRemainingItemsThresholdReached(object sender, EventArgs e)
        {
            if (hasMoreItems)
            {
                await LoadFaculdadesAsync();
            }
        }

        // Método de pesquisa otimizado para usar filtros diretamente no repositório
        private async void Pesquisar_Clicked(object sender, EventArgs e)
        {
            var query = entrySearch.Text?.ToLower();

            // Reinicia o estado da pesquisa
            UniversidadesList.Clear();
            currentPage = 1;
            hasMoreItems = false; // Impede carregamento incremental durante a pesquisa
            isLoading = false;

            if (!string.IsNullOrEmpty(query))
            {
                // Pesquisa por município ou nome da universidade
                await LoadFaculdadesAsync(municipio: query, nome: query);
            }
            else
            {
                // Recarrega a lista completa se a pesquisa estiver vazia
                hasMoreItems = true; // Permite carregamento incremental novamente
                await LoadFaculdadesAsync();
            }

            // Mostre uma mensagem se não houver resultados
            if (UniversidadesList.Count == 0)
            {
                await DisplayAlert("Nenhum resultado", "Nenhuma universidade encontrada para os critérios de busca.", "OK");
                hasMoreItems = true;
                await LoadFaculdadesAsync();
            }
        }

        // Método para manipular a seleção de um item na lista
        private async void OnFaculdadeSelected(object sender, EventArgs e)
        {
            if (sender is Element element && element.BindingContext is Universidades selectedFaculdade)
            {
                try
                {
                    var cursosSisu = _sisuCursosRepository.ObterCursosSisu(selectedFaculdade.CODIGO_IES.ToString());
                    bool estaNoSisu = cursosSisu.Any();

                    // Caso contrário, vá diretamente para `PaginaLista`
                    await Navigation.PushAsync(new PaginaLista(selectedFaculdade.ID_UNIVERSIDADE, estaNoSisu));
                }
                catch (Exception ex)
                {
                    // Log ou mensagem de erro para o usuário
                    await DisplayAlert("Erro", $"Ocorreu um erro ao verificar o SISU: {ex.Message}", "OK");
                }
            }
        }
    }
}
