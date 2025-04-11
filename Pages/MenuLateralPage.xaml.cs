namespace gestVehMaui.Pages;

public partial class MenuLateralPage : ContentPage
{
	public MenuLateralPage()
	{
		InitializeComponent();
	}

    private async void OnConfiguracionClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ConfiguracionPage());
        ((FlyoutPage)Parent.Parent).IsPresented = false;
    }

    private async void OnVehiculosClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new VehiculosPage());
        ((FlyoutPage)Parent.Parent).IsPresented = false;
    }

    private async void OnReportesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ReportesPage());
        ((FlyoutPage)Parent.Parent).IsPresented = false;
    }
}