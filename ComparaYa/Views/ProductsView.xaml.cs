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

namespace ComparaYa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsView : ContentPage
    {
        public decimal ipEmulador;
        public decimal ipPcCelu;
        int currentPage = 1;
        int limit = 20; 
        bool isLoading = false;
        bool isFilteredApplied;
        private List<Product> filteredTips;
        private readonly HttpClient _cliente = new HttpClient();


        public ProductsView()
        {
            InitializeComponent();
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (s, e) => {
            await openFilters();
            };
            filters.GestureRecognizers.Add(tapGestureRecognizer);

           


        }



        protected override async void OnAppearing()
        {
           
            base.OnAppearing();
            Console.WriteLine("categorias:", App.CategoriasCollection.Count.ToString());
            
            if(App.ProductosCollection.Count == 0 || App.ProductosCollection == null) {

                await FetchProductsFromServer();
            }

            if (App.CategoriasCollection.Count == 0 || App.CategoriasCollection  == null)
            {

                await FetchCategoriasFromServer();
            }
           
            cvPro.RemainingItemsThresholdReached += async (sender, e) =>
            {
                await LoadMoreItems();
            };
            cvPro.RemainingItemsThreshold = 1;
        }

        
        
        public async Task LoadMoreItems()
        {
            if (isLoading) return;
            isLoading = true;
            loadMoreActivityIndicator.IsRunning = true; 

            try
            {
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri($"http://10.0.2.2:4000/productos/{currentPage}/{limit}");
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
                    Task.Delay(1000);

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
        }





        protected async Task FetchCategoriasFromServer()
        {

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://10.0.2.2:4000/categorias/");
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


            }
        }

        protected async Task openFilters() {
          await  Navigation.ShowPopupAsync(new Modal());
        }


        protected async Task FetchProductsFromServer()
        {

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"http://10.0.2.2:4000/productos/{currentPage}/{limit}");
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
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var item = (Categoria)button.BindingContext;

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"http://10.0.2.2:4000/productos/categoria/{item.id}");
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
                currentPage = 2;
            }
        }

    
        private async void cvPro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            var item = (Product)e.CurrentSelection.FirstOrDefault();
            await Navigation.PushAsync(new ComparationPage(item));
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var button = (Label)sender;
            var item = (Product)button.BindingContext;

            Navigation.PushAsync(new ComparationPage(item));
        }

        public async Task imgModal(Product img)
        {

            await Navigation.ShowPopupAsync(new ImgModal(img));
        }


        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            var button = (Image)sender;
            var item = (Product)button.BindingContext;
            imgModal(item);
        }
    }
    }
