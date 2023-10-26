using ComparaYa.Models;
using Firebase.Auth;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComparaYa
{
    public partial class App : Application
    {
        public static ObservableCollection<Product> ProductosCollection { get; set; } = new ObservableCollection<Product>();
        public static ObservableCollection<Categoria> CategoriasCollection { get; set; } = new ObservableCollection<Categoria>();

        public App()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(Xamarin.Essentials.Preferences.Get("firebaseRefreshToken", ""))){
                MainPage = new NavigationPage(new MainTabs());
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
