using Extensionista.Drawables;
using Extensionista.Models;
using Extensionista.Repositories;
using Microsoft.Maui.Controls;

namespace Extensionista.View
{
    public partial class PaginaIndicadoresRegiao : ContentPage
    {
        private readonly string _regiaoSelecionada;
        public PaginaIndicadoresRegiao(string regiaoSelecionada)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            _regiaoSelecionada = regiaoSelecionada;

            var repository = new IndicadoresRepository();

            // Obter vagas por região
            int vagas = repository.ObterVagasRegiao(regiaoSelecionada.ToUpper());
            var vagasturno = repository.ObterVagasRegiaoTurno(regiaoSelecionada.ToUpper());

            // Configurar Labels existentes
            LabelIndicadoresRegiao.Text = regiaoSelecionada;
            LabelVagasRegiao.Text = $"No ano de 2023, o programa do SISU ofereceu {vagas} vagas para a região {regiaoSelecionada}.";

            // Configurar gráfico
            var drawable = new GraficoVagasTurno
            {
                VagasPorTurno = vagasturno
            };
            GraficoVagasTurnos.Drawable = drawable;
            GraficoVagasTurnos.Invalidate();

            // NOVO: Exibir quantidade de cursos diferentes
            var cursos = repository.ObterCursosPorRegiao(regiaoSelecionada.ToUpper());
            ExibirQuantidadeDeCursos(cursos);

            // Obter e exibir o top 10 de vagas por curso
            var vagasPorCurso = repository.ObterVagasPorRegiao(regiaoSelecionada.ToUpper());
            ListViewTop10Cursos.ItemsSource = vagasPorCurso;
        }

        private void ExibirQuantidadeDeCursos(List<CursosDiferentesRegiao> cursos)
        {
            if (cursos != null && cursos.Any())
            {
                var totalCursos = 0;
                int.TryParse(cursos.FirstOrDefault()?.Total_Linhas, out totalCursos);
                LabelTotalCursos.Text = $"No ano de 2023, o programa do SISU ofereceu {totalCursos} cursos diferentes na região {_regiaoSelecionada}.";
            }
            else
            {
                LabelTotalCursos.Text = "Não foi possível obter os cursos.";
            }
        }

        private void radioButtonNotas_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                if (radioButton == radioButtonIndicadoresVagas && radioButtonIndicadoresVagas.IsChecked)
                {
                    // Exibe o gráfico de vagas
                    LabelIndicadoresRegiao.IsVisible = true;
                    LabelVagasRegiao.IsVisible = true;
                    LabelSoParaDesabilitar.IsVisible = true;
                    GraficoVagasTurnos.IsVisible = true;
                    LabelTotalCursos.IsVisible = false;
                    ListViewTop10Cursos.IsVisible = false;
                    Cabecalho.IsVisible = false;
                    LabelTabela.IsVisible = false;// Esconde a lista
                }
                else if (radioButton == radioButtonIndicadoresCursos && radioButtonIndicadoresCursos.IsChecked)
                {
                    LabelIndicadoresRegiao.IsVisible = true;
                    LabelVagasRegiao.IsVisible = false;
                    LabelSoParaDesabilitar.IsVisible = false;
                    GraficoVagasTurnos.IsVisible = false;
                    LabelTotalCursos.IsVisible = true;
                    ListViewTop10Cursos.IsVisible = true;
                    Cabecalho.IsVisible = true;
                    LabelTabela.IsVisible = true;// Mostra a lista com os cursos
                }
            }
        }

        private async void sairIndicadoresS_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
