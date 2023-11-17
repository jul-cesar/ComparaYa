using ComparaYa.localBD;
using ComparaYa.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComparaYa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class Favs : ContentPage, INotifyPropertyChanged
    {
        public ObservableCollection<Product> favoriteProducts { get; set; } = new ObservableCollection<Product>();


        public Favs()
        {
            InitializeComponent();
           
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
           await LoadFavorites();
        }

       public async Task LoadFavorites()
{
    favoriteProducts.Clear();

            var favorites = await App.db.GetFavoritosAsync(App.currentId);


    foreach (var favorite in favorites)
    {
        var product = App.ProductosCollection.FirstOrDefault(p => p.id == favorite.ProductoId);
        if (product != null && !favoriteProducts.Any(p => p.id == product.id))
        {
            product.IsFavorite = true;
            product.FavoriteIcon = "sifav.png";
            favoriteProducts.Add(product);
        }
    }

    cvPro.ItemsSource = favoriteProducts; 
    NotifyPropertyChanged(); 
}


        private void Button_Clicked(object sender, EventArgs e)
        {
            App.db.DeleteAllFavoritesAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        private async void AnimationView_Clicked(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.Count > 1)
            {
                await Navigation.PopAsync();
            }
        }
    }
}