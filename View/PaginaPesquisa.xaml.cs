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
        private async Task LoadFaculdadesAsync(int? codigoIES = null, string municipio = null)
        {
            if (isLoading) return;

            isLoading = true;

            try
            {
                // Chama a função do repositório diretamente com paginação e filtros
                var universidades = await Task.Run(() =>
                    _cursosGeralRepository.ObterUniversidades(codigoIES, municipio, currentPage));

                // Debug para validar os resultados retornados
                Console.WriteLine($"Resultados retornados: {universidades.Count}");

                if (universidades.Count == 0)
                {
                    hasMoreItems = false; // Marca como última página se não houver mais itens
                    return;
                }

                foreach (var universidade in universidades)
                {
                    UniversidadesList.Add(universidade);
                }

                // Incrementa a página apenas se não houver filtros ativos
                if (codigoIES == null && string.IsNullOrEmpty(municipio))
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

            if (int.TryParse(query, out int codigoIES))
            {
                // Pesquisa por código IES
                await LoadFaculdadesAsync(codigoIES: codigoIES);
            }
            else if (!string.IsNullOrEmpty(query))
            {
                // Pesquisa por município
                await LoadFaculdadesAsync(municipio: query);
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
            }
        }

        // Método para manipular a seleção de um item na lista
        private async void OnFaculdadeSelected(object sender, EventArgs e)
        {
            if (sender is Element element && element.BindingContext is Universidades selectedFaculdade)
            {
                try
                {
                    // Verificar se o código IES existe no repositório de cursos SISU
                    var cursosSisu = _sisuCursosRepository.ObterCursosSisu(selectedFaculdade.CODIGO_IES.ToString());
                    bool estaNoSisu = cursosSisu.Any();

                    string mensagem = estaNoSisu
                    ? "Esta universidade participa do SISU."
                    : "Esta universidade não participa do SISU.";

                    await DisplayAlert("Informação SISU", mensagem, "OK");

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
