using ComparaYa.Models;
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

            MainPage = new NavigationPage( new LoginPage());

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
