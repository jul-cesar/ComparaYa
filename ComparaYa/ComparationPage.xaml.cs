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
        // Constants
        private const int TokenSortRatioThreshold = 75;
        private const int TokenSetRatioThreshold = 85;
        private const int PartialRatioThreshold = 85;
        private const int RatioThreshold = 60;

        public ObservableCollection<Product> EqualsProducts { get; set; }
        public ObservableCollection<Product> AlikeProducts { get; set; }

        private Product prod;

        public ComparationPage()
        {

        }

        public ComparationPage(Product prod)
        {
            InitializeComponent();
            this.prod = prod;
            LoadProducts();
            this.BindingContext = this;
        }

        private void LoadProducts()
        {
            EqualsProducts = GetEqualsProducts(prod);
            AlikeProducts = GetAlikeProducts(prod);
        }

        private ObservableCollection<Product> GetEqualsProducts(Product product)
        {
            return new ObservableCollection<Product>(
                App.ProductosCollection.Where(p => IsSimilar(p.nombre, product.nombre))
            );
        }

        private ObservableCollection<Product> GetAlikeProducts(Product product)
        {
            return new ObservableCollection<Product>(
                App.ProductosCollection.Where(p =>
                    IsSomewhatSimilar(p.nombre, product.nombre) &&
                    !NormalizeString(p.nombre).Equals(NormalizeString(product.nombre)))
            );


        }
        private bool IsSimilar(string name1, string name2)
        {
            var normalizedNombre1 = NormalizeString(name1);
            var normalizedNombre2 = NormalizeString(name2);

            return Fuzz.TokenSortRatio(normalizedNombre1, normalizedNombre2) > TokenSortRatioThreshold ||
                   Fuzz.TokenSetRatio(normalizedNombre1, normalizedNombre2) > TokenSetRatioThreshold ||
                   Fuzz.PartialRatio(normalizedNombre1, normalizedNombre2) > PartialRatioThreshold;
        }

        private bool IsSomewhatSimilar(string name1, string name2)
        {
            var normalizedNombre1 = NormalizeString(name1);
            var normalizedNombre2 = NormalizeString(name2);

            return Fuzz.PartialRatio(normalizedNombre1, normalizedNombre2) > RatioThreshold ||
                   Fuzz.TokenSortRatio(normalizedNombre1, normalizedNombre2) > (TokenSortRatioThreshold - 10) ||
                   Fuzz.Ratio(normalizedNombre1, normalizedNombre2) > (RatioThreshold - 10);
        }
        private string NormalizeString(string input)
            {

                input = input.ToLower();

                input = Regex.Replace(input, @"[^a-z0-9\s]", "");

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

            
        }

        private async void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            
            await Navigation.PushAsync(new PlaneadorCompra());
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