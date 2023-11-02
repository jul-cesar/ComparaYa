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

namespace ComparaYa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsView : ContentPage
    {
        public decimal ipEmulador;
        public decimal ipPcCelu;
        int currentPage = 1;
        int limit = 16;
        bool isLoading = false;
        bool isFilteredApplied;
        private List<Product> filteredTips;
        private readonly HttpClient _cliente = new HttpClient();


        public ProductsView()
        {
            InitializeComponent();



        }



        protected override async void OnAppearing()
        {
            base.OnAppearing();


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
            
            if(isLoading) return;   
            isLoading = true;
            loadMoreActivityIndicator.IsRunning = true;
            try
            {
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri($"http://{Configuracion.IpServidor}:4000/productos/{currentPage}/{limit}");
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
            request.RequestUri = new Uri($"http://{Configuracion.IpServidor}:4000/categorias/");
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accept", "application/json");

            HttpResponseMessage response = await _cliente.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)

            {
                string contentCat = await response.Content.ReadAsStringAsync();

                var resultadoCat = JsonConvert.DeserializeObject<ObservableCollection<Categoria>>(contentCat);

                App.ProductosCollection.Clear();
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
            request.RequestUri = new Uri($"http://{Configuracion.IpServidor}:4000/productos/{currentPage}/{limit}");
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

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"http://{Configuracion.IpServidor}:4000/productos/categoria/{item.id}");
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


        private async void cvPro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (Product)e.CurrentSelection.FirstOrDefault();
            if (item != null)
            {
                await Navigation.PushAsync(new ComparationPage(item));

          
                var collectionView = (CollectionView)sender;
                collectionView.SelectedItem = null;
            }

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
    }
}
