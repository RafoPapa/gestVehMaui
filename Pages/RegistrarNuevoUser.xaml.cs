using Newtonsoft.Json;
using System.Text;

namespace gestVehMaui.Pages;

public partial class RegistrarNuevoUser : ContentPage
{
    //definiciones para la llamada a la api
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly string rutaBase = "https://detecpro.somee.com"; // RUTA BASE API
    private readonly string RegistroEndpoint = "/usuarios/creaUsuario"; //EndPoint para crear un nuevo usuario
    private readonly string ValidaEmailEndpoint = "/usuarios/validaEmail/buscarPorEmail"; // Endpoint para validar el email
    public RegistrarNuevoUser()
	{
		InitializeComponent();
	}

    //tarea que valida si existe el correo antes de guardarlo en la bd
    private async Task<bool> EmailExiste(string email)
    {
        try
        {
            var emailParametro = new { Email = email };
            var jsonRequerido = JsonConvert.SerializeObject(emailParametro);
            var content = new StringContent(jsonRequerido, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{rutaBase}{ValidaEmailEndpoint}", content);

            if (response.IsSuccessStatusCode)
            {
                // Debemos validar el contenido de la respuesta, no solo el código de estado.
                var responseContent = await response.Content.ReadAsStringAsync();
                var listaUsuarios = JsonConvert.DeserializeObject<List<object>>(responseContent); // Cambia object por el tipo de tu objeto Usuario

                return listaUsuarios != null && listaUsuarios.Count > 0;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                //si la API devuelve 204, significa que el email no existe.
                return false;
            }
            else
            {
                return false; 
            }
        }
        catch (Exception)
        {
            return false; // o lanza una excepción
        }
    }

    private async void OnRegistrarClicked(object sender, EventArgs e)
    {

        string nombre = NombreEntry.Text;
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

        // Validar campos vacíos
        if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            ResultadoLabel.Text = "Por favor, complete todos los campos.";
            return; // Detener la ejecución si hay campos vacíos
        }

        // Validar contraseñas coincidentes
        if (password != confirmPassword)
        {
            ResultadoLabel.Text = "Las contraseñas no coinciden.";
            return; // Detener la ejecución si las contraseñas no coinciden
        }

        //visualiza mi progressbar
        pgbEspera.IsVisible = true;

        // Validar si el email ya existe
        bool emailExiste = await EmailExiste(email);
        if (emailExiste)
        {
            ResultadoLabel.Text = "Este correo electrónico ya está registrado.";
            //apaga mi progressbar
            pgbEspera.IsVisible = false;
            return;
        }

        //creamos una objeto con propiedades
        var nuevoUsuario = new
        {
            NombreUser = nombre,
            Email = email,
            Password = password
        };

        //Creamos la estructura json que se ira a la API
        //Nos puede aparecer un error en Encoding, simplementa
        //hay que importar la clase text.
        var jsonRequerido = JsonConvert.SerializeObject(nuevoUsuario);
        var content = new StringContent(jsonRequerido, Encoding.UTF8, "application/json");

        try
        {
            var respuesta = await _httpClient.PostAsync(rutaBase + RegistroEndpoint, content);

            if (respuesta.IsSuccessStatusCode)
            {
                ResultadoLabel.Text = "Se registró correctamente el usuario";

                // Mostrar ventana emergente de éxito
                await DisplayAlert("Registro Exitoso", "Usuario registrado correctamente.", "OK");

                
                NombreEntry.Text = string.Empty;
                EmailEntry.Text = string.Empty;
                PasswordEntry.Text = string.Empty;
                ConfirmPasswordEntry.Text = string.Empty;

                //apaga mi progressbar
                pgbEspera.IsVisible = false;

                //si hay paginas de navegacion
                if (Navigation.ModalStack.Count > 0)
                {
                    await Navigation.PopModalAsync(); // Regresa a la página de inicio de sesión
                }
            }
            else
            {
                var errorContent = await respuesta.Content.ReadAsStringAsync();
                ResultadoLabel.Text = $"Error al registrar: {respuesta.StatusCode} - {errorContent}";
                //apaga mi progressbar
                pgbEspera.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            ResultadoLabel.Text = $"Error de conexión: {ex.Message}";
            //apaga mi progressbar
            pgbEspera.IsVisible = false;
        }
    }

    private async void OnRegresarClicked(object sender, EventArgs e)
    {

        //si hay paginas de navegacion
        if (Navigation.ModalStack.Count > 0)
        {
            await Navigation.PopModalAsync(); // Regresa a la página de inicio de sesión
        }
       
    }
}