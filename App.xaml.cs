using gestVehMaui.Pages;

namespace gestVehMaui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage()) ;
        }

        protected override void OnResume()
        {
            base.OnResume();
            MainPage = new NavigationPage(new LoginPage());
        }
    }
}
