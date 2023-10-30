using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Java.Util.Prefs;
using Android.Preferences;
using Acr.UserDialogs;

namespace ComparaYa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public string ApiKey = "AIzaSyAjgxZOgtQq3PwwHuwIE7MEu05KUIgW4zQ";
        public LoginPage()
        {
            InitializeComponent();

            // Crear el TapGestureRecognizer
            var tapGestureRecognizer = new TapGestureRecognizer();
            // Asignar el evento Tapped al método ToRegisterPage
            tapGestureRecognizer.Tapped += ToRegisterPage;
            // Añadir el TapGestureRecognizer al Label
            registrarteLabel.GestureRecognizers.Add(tapGestureRecognizer);
        }

        private async void LogIn(object sender, EventArgs e)
        {
            //... (no hay cambios aquí)
            try
            {
                UserDialogs.Instance.ShowLoading("Cargando...");
                await Task.Delay(1000);
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(email.Text, passw.Text);
                var content = await auth.GetFreshAuthAsync();
                var serializedContent = JsonConvert.SerializeObject(content);
                Xamarin.Essentials.Preferences.Set("firebaseRefreshToken", serializedContent);
                
               
               
                await Navigation.PushAsync(new MainTabs());
                UserDialogs.Instance.HideLoading();
                email.Text = "";
                passw.Text = "";


            }
            catch (Exception )
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Error", "Usuario o contraseña incorrectos", "ok");
               
            }
        }

        private async void ToRegisterPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
        private async void GetUserInfo(object sender, EventArgs e)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            try
            {

                var savedFirebaseAuth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Xamarin.Essentials.Preferences.Get("Firebase token", ""));
                var refreshedContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
                Xamarin.Essentials.Preferences.Set("firebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));
               /* UserName.text = savedFirebaseAuth.User.Email; */


            }
            catch (Exception ex)

            {

              await  DisplayAlert("error", "token expirado", "ok");


            }
        }
    }
    }