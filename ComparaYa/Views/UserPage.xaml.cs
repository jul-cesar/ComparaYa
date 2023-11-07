using ComparaYa.Models;
using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.CommunityToolkit.Extensions;

namespace ComparaYa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        public string ApiKey = "AIzaSyAjgxZOgtQq3PwwHuwIE7MEu05KUIgW4zQ";
        private readonly HttpClient _cliente = new HttpClient();
        private string user { get; set; }
        public UserPage()
        {
            InitializeComponent();
          
            logoutButton.Clicked += LogoutButton_Clicked;
        }

        protected override async void OnAppearing()
        {
            await GetUserInfo();
        }

        private async Task<string> getUserName(string email)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"http://{Configuracion.IpServidor}:4000/usuarionombre/{email}"),
                Method = HttpMethod.Get,
            };
            request.Headers.Add("Accept", "application/json");

            HttpResponseMessage response = await _cliente.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string contentNombre = await response.Content.ReadAsStringAsync();
                var resultadoNombre = JsonConvert.DeserializeObject<NombreUsuario>(contentNombre);
                return resultadoNombre.nombre;
            }
            return null;
        }

        private async Task GetUserInfo()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            try
            {
                var savedFirebaseAuth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Xamarin.Essentials.Preferences.Get("firebaseRefreshToken", ""));
                var refreshedContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
                Xamarin.Essentials.Preferences.Set("firebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));
                emailUser.Text = savedFirebaseAuth.User.Email;

                string encodedEmail = Uri.EscapeDataString(savedFirebaseAuth.User.Email);
                string userName = await getUserName(encodedEmail);  
                nombreUser.Text = userName;  
            }
            catch (Exception ex)
            {
                await DisplayAlert("error", "token expirado", "ok");
            }
        }


        private async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            Xamarin.Essentials.Preferences.Remove("firebaseRefreshToken");
           await Navigation.PushAsync(new WelcomePage());
        }


        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.ShowPopupAsync(new ModalFavorites());
        }
    }
}