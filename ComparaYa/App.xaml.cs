using ComparaYa.localBD;
using ComparaYa.Models;
using Firebase.Auth;
using System;
using System.Collections.ObjectModel;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComparaYa
{
    public partial class App : Application
    {
       public static int? currentId;
        public static string currentUserRol;
        private static Db _db;
        public static Db db
        {
            get
            {
                if (_db == null)
                {
                    string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Favorite.db3");
                    _db = new Db(databasePath);
                }
                return _db;
            }
        }




        public static ObservableCollection<Product> ProductosCollection { get; set; } = new ObservableCollection<Product>();
        public static ObservableCollection<Categoria> CategoriasCollection { get; set; } = new ObservableCollection<Categoria>();
        public static ObservableCollection<Product> Carrito { get; set; } = new ObservableCollection<Product>();


        public App()
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(Xamarin.Essentials.Preferences.Get("firebaseRefreshToken", ""))){
                MainPage = new NavigationPage(new MainTabs());
            }
            else
            {
                MainPage = new NavigationPage(new WelcomePage());
            }

            new NavigationPage(new ComparationPage()); 
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
