using Extensionista.Models;
using Microsoft.Maui.Graphics;

namespace Extensionista.Drawables
{
    public class GraficoBarrasDrawable : IDrawable
    {
        public List<SisuModalidades> Modalidades { get; set; } = new List<SisuModalidades>();
        public List<string> Rotulos { get; set; } = new List<string>();
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // Fundo branco
            canvas.FillColor = Color.FromArgb("FFFFFF");

            canvas.FillRectangle(dirtyRect);

            // Verifica se há dados para desenhar
            if (Modalidades == null || Modalidades.Count == 0)
            {
                canvas.FontSize = 16;
                canvas.FontColor = Colors.Black;
                canvas.DrawString("Sem dados disponíveis", dirtyRect.Center.X, dirtyRect.Center.Y, HorizontalAlignment.Center);
                return;
            }

            // Determina o maior valor para normalizar as alturas
            float valorMaximo = Modalidades.Max(m => m.QT_VAGAS);
            if (valorMaximo <= 0) return;

            // Configurações para desenho
            float larguraBarra = dirtyRect.Width / (Modalidades.Count * 2f); // Largura da barra
            float espaco = larguraBarra / 1; // Espaço entre as barras
            float alturaMax = dirtyRect.Height * 0.7f; // Reserva 20% da altura para rótulos

            // Desenha as barras
            for (int i = 0; i < Modalidades.Count; i++)
            {
                var modalidade = Modalidades[i];
                float alturaBarra = (modalidade.QT_VAGAS / valorMaximo) * alturaMax;
                float x = i * (larguraBarra + espaco) + espaco;
                float y = dirtyRect.Height - alturaBarra - 30;

                // Desenha a barra
                canvas.FillColor = Color.FromArgb("#092444");
                canvas.FillRectangle(x, y, larguraBarra, alturaBarra);

                // Desenha o valor na barra
                canvas.FontSize = 12;
                canvas.FontColor = Color.FromArgb("#092444");
                canvas.DrawString(modalidade.QT_VAGAS.ToString(), x + larguraBarra / 2, y - 15, HorizontalAlignment.Center);

                // Desenha o rótulo da modalidade
                string rotulo = i < Rotulos.Count ? Rotulos[i] : "?";

                canvas.FontSize = 12;
                canvas.FontColor = Color.Parse("#092444");
                canvas.DrawString(rotulo, x + larguraBarra / 2, y + alturaBarra + 15, HorizontalAlignment.Center);
            }
        }
    }
}
