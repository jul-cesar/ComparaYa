using ComparaYa.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using Xamarin.CommunityToolkit.Extensions;
using Org.Apache.Http.Conn;
using Android.App;
using Acr.UserDialogs;
using Lottie.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;

namespace ComparaYa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsView : ContentPage, INotifyPropertyChanged
    {
        public decimal ipEmulador;
        public decimal ipPcCelu;
        int currentPage = 1;
        int limit = 16;
        Categoria selected;
        bool isLoading = false;
        private bool isRefreshing;
        private List<Product> filteredTips;
        private readonly HttpClient _cliente = new HttpClient();
        bool isFavorite = false;
        private int? currentCategoryId = null;


        public ProductsView()
        {
            InitializeComponent();
            BindingContext = this;


        }



        protected override async void OnAppearing()
        {
            base.OnAppearing();

            currentCategoryId = null;
            currentPage = 1;
            if (App.ProductosCollection.Count == 0 || App.ProductosCollection == null)
            {

                await FetchProductsFromServer();
          
            }

            if (App.CategoriasCollection.Count == 0 || App.CategoriasCollection == null)
            {

                await FetchCategoriasFromServer();
            }

            cvPro.ItemsSource = App.ProductosCollection; 

            NotifyPropertyChanged();

            cvPro.RemainingItemsThresholdReached += async (sender, e) =>
            {
                await LoadMoreItems();
            };
            cvPro.RemainingItemsThreshold = 5;


        }



        public async Task LoadMoreItems()
        {

            if (isLoading || currentCategoryId == null) return;

            isLoading = true;
            loadMoreActivityIndicator.IsRunning = true;

            try
            {
                string requestUri;
                if (currentCategoryId.HasValue)
                {
                   
                    requestUri = $"https://api-compara-ya-git-main-jul-cesars-projects.vercel.app/productos/{currentCategoryId.Value}/{currentPage}/{limit}";
                }
                else
                {
                    
                    requestUri = $"https://api-compara-ya-git-main-jul-cesars-projects.vercel.app/productos/{currentPage}/{limit}";
                }
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri($"{requestUri}");
                request.Method = HttpMethod.Get;
                request.Headers.Add("Accept", "application/json");

                HttpResponseMessage response = await _cliente.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string contentCat = await response.Content.ReadAsStringAsync();
                    var resultadoCat = JsonConvert.DeserializeObject<ObservableCollection<Product>>(contentCat);

                    foreach (var prod in resultadoCat)
                    {
                        App.ProductosCollection.Add(prod);
                    }
                 

                    currentPage++;
                    if (resultadoCat.Count < limit)
                    {
                        cvPro.Footer = null;
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("error", "error", "ok");

            }
            finally
            {

                loadMoreActivityIndicator.IsRunning = false;
                isLoading = false;
            }
            NotifyPropertyChanged();
        }





        protected async Task FetchCategoriasFromServer()
        {

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"https://api-compara-ya-git-main-jul-cesars-projects.vercel.app/categorias");
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accept", "application/json");

            HttpResponseMessage response = await _cliente.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)

            {
                string contentCat = await response.Content.ReadAsStringAsync();

                var resultadoCat = JsonConvert.DeserializeObject<ObservableCollection<Categoria>>(contentCat);

                foreach (var categoria in resultadoCat)
                {
                    App.CategoriasCollection.Add(categoria);
                }

                NotifyPropertyChanged();
;            }
        }



        protected async Task FetchProductsFromServer()
        {

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"https://api-compara-ya-git-main-jul-cesars-projects.vercel.app/productos");
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accept", "application/json");

            HttpResponseMessage response = await _cliente.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)

            {
                string content = await response.Content.ReadAsStringAsync();

                var resultado = JsonConvert.DeserializeObject<ObservableCollection<Product>>(content);
                App.ProductosCollection.Clear();

                foreach (var producto in resultado)
                {
                    App.ProductosCollection.Add(producto);
                }
                NotifyPropertyChanged();
            }
        }


        private async void Button_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var item = (Categoria)button.BindingContext;
            currentCategoryId = item.id;

            if (item.id != currentCategoryId)
            {
                button.BackgroundColor = Color.Gray;
            }


        

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"https://api-compara-ya-git-main-jul-cesars-projects.vercel.app/productos/categoria/{item.id}");
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accept", "application/json");

            HttpResponseMessage response = await _cliente.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)

            {
                string content = await response.Content.ReadAsStringAsync();

                var resultado = JsonConvert.DeserializeObject<ObservableCollection<Product>>(content);
                App.ProductosCollection.Clear();

                foreach (var producto in resultado)
                {
                    App.ProductosCollection.Add(producto);
                    
                }
               
                NotifyPropertyChanged();
            }
        }


       
        public async Task imgModal(Product img)
        {

            await Navigation.ShowPopupAsync(new ImgModal(img.imagen_url));

        }


        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            var button = (Image)sender;
            var item = (Product)button.BindingContext;
            await imgModal(item);
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
           
            string searchText = e.NewTextValue;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                cvPro.ItemsSource = App.ProductosCollection;
            }
            else
            {
                filteredTips = App.ProductosCollection.Where(tip => tip.nombre.ToLower().Normalize(NormalizationForm.FormD).Contains((searchText.ToLower()))).ToList();
                cvPro.ItemsSource = filteredTips;
                NotifyPropertyChanged();
            }
        }



        private async void addAni_Clicked(object sender, EventArgs e)
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

        private async void AnimationView_Clicked(object sender, EventArgs e)
        {
            await Navigation.ShowPopupAsync(new Modal());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        private async void xd_Clicked(object sender, EventArgs e)
        {
            backdark.IsVisible = true;
            backdark.IsVisible = load.IsVisible = true;
            await Task.Delay(1000);
            await Task.Delay(1000);
            var boton = (AnimationView)sender;
            var item = (Product)boton.BindingContext;
                if (item != null)
            {
                await FetchProductsFromServer();
                await Navigation.PushAsync(new ComparationPage(item));
                UserDialogs.Instance.HideLoading();
             
                backdark.IsVisible = load.IsVisible = false;
                backdark.IsVisible = false;

            }
        }

        private void AnimationView_Clicked_1(object sender, EventArgs e)
        {
            if (sender is Lottie.Forms.AnimationView animationView)
            {
             
                if (animationView.IsAnimating)
                {
                    animationView.PauseAnimation();
                }
                else
                {
                    animationView.PlayAnimation();
                }
            }
    }

        public  void TapGestureRecognizer_Tapped(object sender, EventArgs e)
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

            NotifyPropertyChanged(); // Asegúrate de notificar que la propiedad FavoriteIcon cambió.
        }

        private void RefreshView_Refreshing(object sender, EventArgs e)
        {

        }

       
    }
}