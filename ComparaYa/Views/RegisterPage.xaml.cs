using Acr.UserDialogs;
using Firebase.Auth;

using Java.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComparaYa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public string ApiKey = "AIzaSyAjgxZOgtQq3PwwHuwIE7MEu05KUIgW4zQ";

        public RegisterPage()
        {
            InitializeComponent();
            var iniciarSesionTap = new TapGestureRecognizer();
            iniciarSesionTap.Tapped += ToLoginPage;
            iniciarSesionLabel.GestureRecognizers.Add(iniciarSesionTap);
        }

        private async void ToLoginPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }

         public async Task RegistrarUserDB(string name, string correo, string pass)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://192.168.1.60:4000/usuarios/");  // Cambia la URL de acuerdo a tu endpoint de destino.
            request.Method = HttpMethod.Post;  // Cambia el método a POST.


            var data = new { nombre =name, correo = correo, contrasena= pass};
            string json = JsonConvert.SerializeObject(data);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var cliente = new HttpClient();
            HttpResponseMessage response = await cliente.SendAsync(request);
        }

        async private void Registrar(object sender, EventArgs e)
        {
            try
            {
                
                UserDialogs.Instance.ShowLoading("Cargando...");
                await Task.Delay(2000);
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(user.Text, pass.Text);
                string gettoken = auth.FirebaseToken;
             
                await RegistrarUserDB(name.Text,user.Text, pass.Text);
                
              
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Registro", user.Text, "ok");
                
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Error", user.Text, "ok");

            }
        }
    }
}

