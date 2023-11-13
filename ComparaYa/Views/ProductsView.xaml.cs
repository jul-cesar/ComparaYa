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
using ComparaYa.localBD;
using Firebase.Auth;
using System.Globalization;
using System.Web;

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
        public string ApiKey = "AIzaSyAjgxZOgtQq3PwwHuwIE7MEu05KUIgW4zQ";
  
        public ProductsView()
        {
            InitializeComponent();
            BindingContext = this;
            


        }



        protected override async void OnAppearing()
        {

            MessagingCenter.Subscribe<Modal, FiltrosData>(this, "FilterProducts", async (sender, filterData) =>
            {
               await ApplyFilter(filterData);
            });
            base.OnAppearing();

           await GetUserInfo();
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

            await UpdateFavoriteStatusOfProducts(); 

            cvPro.ItemsSource = App.ProductosCollection;

            cvPro.ItemsSource = App.ProductosCollection; 

            NotifyPropertyChanged();

            cvPro.RemainingItemsThresholdReached += async (sender, e) =>
            {
                await LoadMoreItems();
            };
            cvPro.RemainingItemsThreshold = 5;


        }
        private async Task ApplyFilter(FiltrosData filtros)
        {

            try
            {
                string requestUri;

                // Verificar si PriceFrom es un decimal válido y no es cero
                bool isPriceFromValid = filtros.PriceFrom.HasValue && filtros.PriceFrom.Value > 0;

                if (currentCategoryId == null || currentCategoryId == 0)
                {
                    // Caso: Filtrar solo por precio (sin categoría seleccionada y sin distribuidor)
                    if (isPriceFromValid && string.IsNullOrEmpty(filtros.Distri))
                    {
                        requestUri = $"https://api-compara-ya-git-main-jul-cesars-projects.vercel.app/productos/filtrados/{filtros.PriceFrom.Value}";
                    }
                    // Caso: Filtrar por precio y distribuidor (sin categoría seleccionada)
                    else if (isPriceFromValid && !string.IsNullOrEmpty(filtros.Distri))
                    {
                        requestUri = $"https://api-compara-ya-git-main-jul-cesars-projects.vercel.app/productos/filtrados/{filtros.PriceFrom.Value}/distribuidor/{filtros.Distri}";
                    }
                    // Caso: Filtrar solo por distribuidor (sin categoría ni precio)
                    else if (!string.IsNullOrEmpty(filtros.Distri))
                    {
                        requestUri = $"https://api-compara-ya-git-main-jul-cesars-projects.vercel.app/productos/filtrados/distribuidor/{filtros.Distri}";
                    }
                    // Caso: No hay filtros especificados
                    else
                    {
                        requestUri = $"https://api-compara-ya-git-main-jul-cesars-projects.vercel.app/productos";
                    }
                }
                else
                {
                    // Caso: Filtrar por categoría y precio (con o sin distribuidor)
                    if (isPriceFromValid)
                    {
                        if (!string.IsNullOrEmpty(filtros.Distri))
                        {
                            // Caso: Filtrar por categoría, precio y distribuidor específico
                            requestUri = $"https://api-compara-ya-git-main-jul-cesars-projects.vercel.app/productos/filtrados/{filtros.PriceFrom.Value}/distribuidor/{filtros.Distri}/categoria/{currentCategoryId}";
                        }
                        else
                        {
                            // Distribuidor no especificado o es "todos"
                            requestUri = $"https://api-compara-ya-git-main-jul-cesars-projects.vercel.app/productos/filtrados/{filtros.PriceFrom.Value}/categoria/{currentCategoryId}";
                        }
                    }
                    else if (!string.IsNullOrEmpty(filtros.Distri))
                    {
                        // Caso: Filtrar por categoría y distribuidor
                        requestUri = $"https://api-compara-ya-git-main-jul-cesars-projects.vercel.app/productos/filtrados/distribuidor/{filtros.Distri}/categoria/{currentCategoryId}";
                    }
                    else
                    {
                        // Caso: Filtrar solo por categoría
                        requestUri = $"https://api-compara-ya-git-main-jul-cesars-projects.vercel.app/productos/categoria/{currentCategoryId}";
                    }
                }

                var request = new HttpRequestMessage();
                request.RequestUri = new Uri(requestUri);
                request.Method = HttpMethod.Get;
                request.Headers.Add("Accept", "application/json");

                HttpResponseMessage response = await _cliente.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string contentCat = await response.Content.ReadAsStringAsync();
                    var resultadoCat = JsonConvert.DeserializeObject<ObservableCollection<Product>>(contentCat);

                    cvPro.ItemsSource = resultadoCat;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("error", ex.Message, "ok");
            }

            NotifyPropertyChanged();
        
    }


        private async Task UpdateFavoriteStatusOfProducts()
        {
            var favorites = await App.db.GetFavoritosAsync(App.currentId); 

            foreach (var product in App.ProductosCollection)
            {
                product.IsFavorite = favorites.Any(f => f.ProductoId == product.id);
                product.FavoriteIcon = product.IsFavorite ? "sifav.png" : "nofav.png";
            }

            NotifyPropertyChanged("App.ProductosCollection"); 
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
                request.RequestUri = new Uri(requestUri);
                request.Method = HttpMethod.Get;
                request.Headers.Add("Accept", "application/json");

                HttpResponseMessage response = await _cliente.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)

                {
                    string contentCat = await response.Content.ReadAsStringAsync();

                    var resultadoCat = JsonConvert.DeserializeObject<ObservableCollection<Product>>(contentCat);

                   cvPro.ItemsSource = resultadoCat;    
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


        private async void FilterByCategory(object sender, EventArgs e)
        {
           
            var button = (Button)sender;
            var item = (Categoria)button.BindingContext;
            currentCategoryId = item.id;
           

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
                cvPro.ItemsSource = App.ProductosCollection;
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


        private async void OpenImage(object sender, EventArgs e)
        {
            var button = (Image)sender;
            var item = (Product)button.BindingContext;
            await imgModal(item);
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
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



        private async void AddToCart(object sender, EventArgs e)
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

        private async void OpenFilters(object sender, EventArgs e)
        {
            await Navigation.ShowPopupAsync(new Modal());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        private async void Comparation(object sender, EventArgs e)
        {

            backdark.IsVisible = true;
            backdark.IsVisible = load.IsVisible = true;
           
          
            var boton = (AnimationView)sender;
            var item = (Product)boton.BindingContext;
                if (item != null)
            {
                await FetchProductsFromServer();
                await Navigation.PushAsync(new ComparationPage(item));
               
             
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

        private async Task GetUserInfo()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            try
            {
                var savedFirebaseAuth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Xamarin.Essentials.Preferences.Get("firebaseRefreshToken", ""));
                var refreshedContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
                Xamarin.Essentials.Preferences.Set("firebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));
                

                string encodedEmail = Uri.EscapeDataString(savedFirebaseAuth.User.Email);
                int? userId = await getCurrentUserId(encodedEmail);
                App.currentId = userId;
               
            }
            catch (Exception ex)
            {
                await DisplayAlert("error", "token expirado", "ok");
            }
        }

        private async Task<int?> getCurrentUserId(string email)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://api-compara-ya-git-main-jul-cesars-projects.vercel.app/usuarioid/{email}"),
                Method = HttpMethod.Get,
            };
            request.Headers.Add("Accept", "application/json");

            HttpResponseMessage response = await _cliente.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string contentId = await response.Content.ReadAsStringAsync();
                var resultadoNombre = JsonConvert.DeserializeObject<IdUser>(contentId);

                return resultadoNombre?.id; 
            }
            return null; 
        }




        public async void AddToFavorite(object sender, EventArgs e)
        {
            var button = (Image)sender;
            var item = (Product)button.BindingContext;
            item.isFavorite = !item.isFavorite;
            item.FavoriteIcon = item.isFavorite ? "sifav.png" : "nofav.png";
         
            if (item.isFavorite)
            {
                var existingFavorite = await App.db.GetFavoritoByProductoId(item.id, App.currentId);
                if (existingFavorite == null)
                {
                    var favorito = new Favorite { UsuarioId = App.currentId, ProductoId = item.id };
                    await App.db.SaveFavoritoAsync(favorito);
                    UserDialogs.Instance.Toast("Producto agregado a favoritos");
                }
            }
            else
            {
                await App.db.DeleteFavoritoAsync(item.id, App.currentId);
                UserDialogs.Instance.Toast("Producto eliminado de favoritos");
            }

            NotifyPropertyChanged();
        }




    }
}