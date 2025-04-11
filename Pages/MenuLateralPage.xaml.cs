namespace gestVehMaui.Pages;

public partial class MenuLateralPage : ContentPage
{
	public MenuLateralPage()
	{
		InitializeComponent();
        string nombreUsuario = Preferences.Get("NombreUsuario", string.Empty);
        string emailUsuario = Preferences.Get("EmailUsuario", string.Empty);

        lblNombreUser.Text= nombreUsuario;
        lblEmailUser.Text= emailUsuario;
	}

    private async void OnConfiguracionClicked(object sender, EventArgs e)
    {
        var navigationPage = ((FlyoutPage)Parent.Parent).Detail as NavigationPage;

        if (navigationPage != null)
        {
            await navigationPage.PushAsync(new ConfiguracionPage());
            ((FlyoutPage)Parent.Parent).IsPresented = false;
        }
    }

   
}