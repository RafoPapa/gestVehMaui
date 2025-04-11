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

        public static object Navigate { get; internal set; }

        protected override void OnResume()
        {
            base.OnResume();
            MainPage = new NavigationPage(new LoginPage());
        }
    }
}
