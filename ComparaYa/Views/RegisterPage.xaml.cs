using Firebase.Auth;

using Java.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComparaYa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public string ApiKey = "AIzaSyAjgxZOgtQq3PwwHuwIEur05KUIgW4zQ";

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

        async private void Registrar(object sender, EventArgs e)
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(user.Text, pass.Text);
                string gettoken = auth.FirebaseToken;

                await DisplayAlert("Registro", user.Text, "ok");
                user.Text = "";
                pass.Text = "";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", user.Text, "ok");
            }
        }
    }
}

