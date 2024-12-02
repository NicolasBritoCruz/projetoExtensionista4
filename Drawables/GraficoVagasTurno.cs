using Extensionista.Models;
using Microsoft.Maui.Graphics;
using System.Collections.Generic;

namespace Extensionista.Drawables
{
    public class GraficoVagasTurno : IDrawable
    {
        public List<VagasRegiaoTurno> VagasPorTurno { get; set; } = new List<VagasRegiaoTurno>();

        public void Draw(ICanvas canvas, RectF dirtyRect)
{
    // Fundo branco
    canvas.FillColor = Colors.White;
    canvas.FillRectangle(dirtyRect);

    if (VagasPorTurno == null || VagasPorTurno.Count == 0)
    {
        canvas.FontSize = 16;
        canvas.FontColor = Colors.Black;
        canvas.DrawString("Sem dados disponíveis", dirtyRect.Center.X, dirtyRect.Center.Y, HorizontalAlignment.Center);
        return;
    }

    // Margens e configurações do gráfico
    float margemEsquerda = 50;
    float margemInferior = 50;
    float larguraTotal = dirtyRect.Width - margemEsquerda;
    float alturaTotal = dirtyRect.Height - margemInferior;

    // Ajusta um pouco a altura para garantir espaço para os rótulos
    float alturaComEspacoExtra = alturaTotal * 0.85f;  // Diminui um pouco para aumentar a área dos rótulos

    // Obtém o valor máximo
    float valorMaximo = VagasPorTurno.Max(v => float.TryParse(v.QT_VAGAS, out var val) ? val : 0);

    if (valorMaximo <= 0) return;

    // Largura de cada barra
    float larguraBarra = larguraTotal / VagasPorTurno.Count;

    // Desenha as barras
    for (int i = 0; i < VagasPorTurno.Count; i++)
    {
        var item = VagasPorTurno[i];
        float valor = float.TryParse(item.QT_VAGAS, out var val) ? val : 0;

        // Calcula a altura proporcional da barra com a nova altura
        float alturaBarra = (valor / valorMaximo) * alturaComEspacoExtra;

        // Desenha a barra
        float x = margemEsquerda + i * larguraBarra;
        float y = dirtyRect.Height - margemInferior - alturaBarra;
        canvas.FillColor = Color.FromArgb("#092444");
        canvas.FillRectangle(x, y, larguraBarra - 10, alturaBarra);

        // Rótulo do turno
        canvas.FontSize = 12;
        canvas.FontColor = Color.FromArgb("#092444");
        canvas.DrawString(item.Turno, x + larguraBarra / 2, dirtyRect.Height - margemInferior + 10, HorizontalAlignment.Center);

        // Valor da barra - ajusta a distância do topo da barra
        float valorPosY = y - 25; // Aumenta mais a distância para o topo da barra
        canvas.FontSize = 12;
        canvas.FontColor = Color.FromArgb("#092444");
        canvas.DrawString(valor.ToString("0"), x + larguraBarra / 2, valorPosY, HorizontalAlignment.Center);
    }
}

    }
}
