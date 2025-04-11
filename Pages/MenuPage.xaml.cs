

namespace gestVehMaui.Pages;

public partial class MenuPage : FlyoutPage
{
    //creo una propiedad para almacenar el titulo
    public string titulo { get; set; }
    public MenuPage()
    {
		InitializeComponent();
        App.Navigate = Navigate;
    }
   
}