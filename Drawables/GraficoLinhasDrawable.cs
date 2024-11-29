using Microsoft.Maui.Graphics;
using System.Collections.Generic;
using Extensionista.Models;

namespace Extensionista.Drawables
{
    public class GraficoLinhasDrawable : IDrawable
    {
        public List<SisuModalidades> ModalidadesBarras { get; set; } = new List<SisuModalidades>();
        public List<string> RotulosBarras { get; set; } = new List<string>();

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // Fundo branco
            canvas.FillColor = Colors.White;
            canvas.FillRectangle(dirtyRect);

            // Verifica se há dados para desenhar
            if (ModalidadesBarras == null || ModalidadesBarras.Count == 0)
            {
                canvas.FontSize = 16;
                canvas.FontColor = Color.FromArgb("#092444");
                canvas.DrawString("Sem dados disponíveis", dirtyRect.Center.X, dirtyRect.Center.Y, HorizontalAlignment.Center);
                return;
            }

            // Define os limites de valores
            const float valorMinimoBarra = 0f; // Agora inclui 0 como valor válido
            float valorMaximoBarra = ModalidadesBarras
                .Select(m => m.NOTA_CORTE_2024)
                .DefaultIfEmpty(0) // Evita erro caso todos os valores sejam 0
                .Max();

            // Configuração adicional para evitar gráfico plano
            if (valorMaximoBarra == 0) valorMaximoBarra = 1; // Garante que o gráfico tenha uma escala mesmo com valores 0

            // Configurações do gráfico
            float margemInferiorBarra = 50; // Espaço reservado para rótulos no eixo X
            float margemEsquerdaBarra = 40; // Margem para evitar cortes laterais
            float larguraBarra = dirtyRect.Width / (ModalidadesBarras.Count * 1.5f); // Ajusta espaçamento entre barras
            float alturaMaxBarra = dirtyRect.Height * 0.7f; // 70% da altura para o gráfico

            // Cor das barras
            var corBarra = Color.FromArgb("#092444");

            // Desenha as barras para cada modalidade
            for (int i = 0; i < ModalidadesBarras.Count; i++)
            {
                var modalidade = ModalidadesBarras[i];
                float valor = modalidade.NOTA_CORTE_2024;

                // Calcula a posição e tamanho da barra
                float x = margemEsquerdaBarra + i * (larguraBarra * 1.3f);
                float alturaBarra = (valor / valorMaximoBarra) * alturaMaxBarra;
                float y = dirtyRect.Height - alturaBarra - margemInferiorBarra;

                // Desenha a barra
                canvas.FillColor = corBarra;
                canvas.FillRectangle(x, y, larguraBarra, alturaBarra);

                // Valor do ponto acima da barra (somente se não for zero)
                if (valor != 0)
                {
                    canvas.FontSize = 12;
                    canvas.FontColor = Colors.Black;
                    canvas.DrawString(valor.ToString("0.0"), x + larguraBarra / 2, y - 15, HorizontalAlignment.Center);
                }

                // Rótulo da modalidade no eixo X
                string rotulo = i < RotulosBarras.Count ? RotulosBarras[i] : modalidade.TIPO_CONCORRENCIA;
                canvas.FontSize = 12;
                canvas.FontColor = Color.FromArgb("#092444");
                canvas.DrawString(rotulo, x + larguraBarra / 2, dirtyRect.Height - margemInferiorBarra + 10, HorizontalAlignment.Center);
            }

        }
    }
}

