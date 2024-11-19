using Microsoft.Maui.Controls;

namespace Extensionista.View;

public partial class NewPage1 : ContentPage
{
    public int CodigoIes { get; private set; }

    public NewPage1(int Codigo_Ies)
    {
        InitializeComponent();
        CodigoIes = Codigo_Ies;
        BindingContext = this;
    }
}