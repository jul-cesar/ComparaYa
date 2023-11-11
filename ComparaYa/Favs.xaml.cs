using ComparaYa.localBD;
using ComparaYa.Models;
using System;
using System.Collections.Generic;
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
        List<Favorite> favorites;
        int count;
        List<Product> favoriteProducts;

        public Favs()
        {
            InitializeComponent();
            favoriteProducts = new List<Product>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

           
            
                LoadFavorites();
            
        }

        public void LoadFavorites()
        {

            favoriteProducts.Clear();
            favorites = App.db.GetFavoritosAsync().Result;
       
            count = favorites.Count;

         

            foreach (var item in favorites)
            {
                var product = App.ProductosCollection.FirstOrDefault(p => p.id == item.ProductoId);

                if (product != null)
                {
                    favoriteProducts.Add(product);

                    item.Producto = product;
                    item.Producto.IsFavorite = true;
                    item.Producto.FavoriteIcon = "sifav.png";
                    NotifyPropertyChanged();
                    App.db.SaveFavoritoAsync(item);
                }
            }

            
            cvPro.ItemsSource = favoriteProducts;
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
    }
}