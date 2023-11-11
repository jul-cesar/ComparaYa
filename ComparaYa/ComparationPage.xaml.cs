using Acr.UserDialogs;
using ComparaYa.Models;
using Lottie.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FuzzySharp;

namespace ComparaYa
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ComparationPage : ContentPage, INotifyPropertyChanged
	{
        private bool _hasCalledAPI = false;
        private bool _isDataLoaded = false;
        bool isFav = false;
        public ObservableCollection<Product> EqualsProducts { get; set; }

        public ObservableCollection<Product> AlikeProducts { get; set; }

        string palabraProducto;
        Array palabrasProducto;

        Product prod;
        public ComparationPage(Product prod)
        {
            InitializeComponent();
           

            this.prod = prod;
            palabraProducto = prod.nombre.Split(' ')[0];
            palabrasProducto = prod.nombre.Split(' ');
            int umbralSimilitud = 67;

            EqualsProducts = new ObservableCollection<Product>(
            App.ProductosCollection.Where(p =>
            {
                var normalizedNombre = NormalizeString(p.nombre);
                var normalizedProdNombre = NormalizeString(prod.nombre);

                return Fuzz.TokenSortRatio(normalizedNombre, normalizedProdNombre) > 67 ||
                       Fuzz.TokenSetRatio(normalizedNombre, normalizedProdNombre) > 73 ||
                       Fuzz.PartialRatio(normalizedNombre, normalizedProdNombre) > 83 ;
            })
        );


            AlikeProducts = new ObservableCollection<Product>(App.ProductosCollection
              .Where(p =>
              {
                  var normalizedNombre = NormalizeString(p.nombre);
                  var palabraProductoNormalizada = NormalizeString(palabraProducto);

                  return (Fuzz.PartialRatio(normalizedNombre, palabraProductoNormalizada) > 55||
                          Fuzz.TokenSortRatio(normalizedNombre, palabraProductoNormalizada) > 60 ||
                          Fuzz.Ratio(normalizedNombre, palabraProductoNormalizada) > 50) &&
                          !normalizedNombre.Equals(NormalizeString(prod.nombre));
              })
          );

            this.BindingContext = this;
        }



        /* protected override async void OnAppearing()
         {
             base.OnAppearing();

             if (!_hasCalledAPI)
             {
                 _hasCalledAPI = true;
                 await ApiCall();
             }
         } */

        private string NormalizeString(string input)
        {
            // Convertir a minúsculas
            input = input.ToLower();

            // Eliminar caracteres especiales y espacios adicionales
            // Aquí se puede ajustar la expresión regular según sea necesario
            input = Regex.Replace(input, @"[^a-z0-9\s]", "");

            // Devolver el string normalizado
            return input;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

       

        private async void addAni_Clicked_1(object sender, EventArgs e)
        {
            var button = (AnimationView)sender;
            var item = (Product)button.BindingContext;
            if (App.Carrito.Contains(item))
            {
                await this.DisplayToastAsync("Este producto ya se encuentra en el carrito");
            }
            else
            {
                App.Carrito.Add(item);
                UserDialogs.Instance.Toast("Producto agregado al carrito");
                NotifyPropertyChanged();
            }


            await Task.Delay(1000);
        }

        
        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {

            var button = (Image)sender;
            var item = (Product)button.BindingContext;
            item.isFavorite = !item.isFavorite;

            item.FavoriteIcon = item.isFavorite ? "sifav.png" : "nofav.png";

            if (item.isFavorite)
            {
                App.Favorites.Add(item);
                UserDialogs.Instance.Toast("Producto agregado a favoritos");
            }
            else
            {
                App.Favorites.Remove(item);
                UserDialogs.Instance.Toast("Producto eliminado de favoritos");
            }

            NotifyPropertyChanged();
        }

        private async void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PlaneadorCompra());
        }


        /* public async Task<ObservableCollection<Product>> ApiCall()
         {
             try
             {
                 AlikeProducts = await ProductsFilters.FiltrarProductosSimilares(App.ProductosCollection, prod);
                 NotifyPropertyChanged(nameof(AlikeProducts));
                 _isDataLoaded = true;
             }
             catch (Exception ex)
             {
                 // Handle or log exception
                 Console.WriteLine(ex.Message);
             }
             return AlikeProducts;
         } */
    }
}