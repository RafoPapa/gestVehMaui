using gestVehMaui.Modelos;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace gestVehMaui.Pages;

public partial class LoginPage : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient();
    private const string rutaBase = "https://detecpro.somee.com";
    private const string LoginEnpoint = "/usuarios/buscarPorEmail";
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void ValidarIngresoUser(object sender, EventArgs e)
    {
        pgbEspera.IsVisible = true;
        string emailIngresado = EmailEntry.Text;
        string passwordIngresado = PasswordEntry.Text;

        if (string.IsNullOrEmpty(emailIngresado) || string.IsNullOrEmpty(passwordIngresado))
        {
            messageLabel.Text = "Debe ingresar el Email y password requeridos";
            return;
        }

        var ParametroLogin = new
        {
            email = emailIngresado,
            password = passwordIngresado
        };
        var JsonRequerido = JsonConvert.SerializeObject(ParametroLogin);
        var content = new StringContent(JsonRequerido,
                                         System.Text.Encoding.UTF8, "application/json");
        var respuesta = await _httpClient.PostAsync(rutaBase + LoginEnpoint, content);

        if (respuesta.IsSuccessStatusCode)
        {
            var responseContent = await respuesta.Content.ReadAsStringAsync();
            List<CredencialesUser> lstCredencialesUsuario = JsonConvert.DeserializeObject<List<CredencialesUser>>(responseContent);

            if (lstCredencialesUsuario != null && lstCredencialesUsuario.Count > 0)
            {
                CredencialesUser credencialesUsuario = lstCredencialesUsuario[0];

                //validar email y password de la bd
                if (credencialesUsuario.Email.Equals(emailIngresado)
                    && credencialesUsuario.Password.Equals(passwordIngresado))
                {
                    messageLabel.Text = "Inicio de sesión autorizada";
                    pgbEspera.IsVisible = false;
                    //Application.Current.MainPage = new NavigationPage(new MenuPage(credencialesUsuario.NombreUser));

                    Preferences.Set("NombreUsuario", credencialesUsuario.NombreUser);
                    Preferences.Set("EmailUsuario", credencialesUsuario.Email);

                    // Llamar a MenuPage directamente
                    Application.Current.MainPage = new MenuPage();
                }
                else
                {
                    messageLabel.Text = "¡¡ Credenciales incorrectas !!";
                    pgbEspera.IsVisible = false;
                }
            }
            else
            {
                messageLabel.Text = "Usuario no existe en la BD";
                pgbEspera.IsVisible = false;
            }
        }
    }

    private void MostrarContraasena(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = false;
    }

    private void OcultarContrasena(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = true;
    }

    private async void RegistrarUser(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new RegistrarNuevoUser());
    }
}