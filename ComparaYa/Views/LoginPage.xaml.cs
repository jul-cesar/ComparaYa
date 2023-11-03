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
using Xamarin.CommunityToolkit.UI.Views;

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
       
            try
            {
                backdark.IsVisible = true;
               backdark.IsVisible = load.IsVisible = true;   
                await Task.Delay(1000);
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(email.Text, passw.Text);
                var content = await auth.GetFreshAuthAsync();
                var serializedContent = JsonConvert.SerializeObject(content);
                Xamarin.Essentials.Preferences.Set("firebaseRefreshToken", serializedContent);
                
               
               
                await Navigation.PushAsync(new MainTabs());
                UserDialogs.Instance.HideLoading();
                backdark.Opacity = 0;
                backdark.IsVisible = load.IsVisible = false;
                backdark.IsVisible = false;
                email.Text = "";
                passw.Text = "";


            }
            catch (Exception )
            {
                UserDialogs.Instance.HideLoading();
                backdark.IsVisible = load.IsVisible = false;
                backdark.IsVisible = false;
                await DisplayAlert("Error", "Usuario o contraseña incorrectos", "ok");
               
            }
        }

        private async void ToRegisterPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
       
    }
    }