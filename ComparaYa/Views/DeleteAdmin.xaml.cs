using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Android.Resource;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ComparaYa.Models;
using Lottie.Forms;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace ComparaYa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeleteAdmin : ContentPage, INotifyPropertyChanged
    {
        private readonly HttpClient _cliente = new HttpClient();
        bool isLoading = false;
        private bool isSearching = false;
        public DeleteAdmin()
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
            cvPro.ItemsSource = App.ProductosCollection;

            NotifyPropertyChanged();

            cvPro.RemainingItemsThresholdReached += async (sender, e) =>
            {
                await LoadMoreItems();
            };
            cvPro.RemainingItemsThreshold = 5;


        }
        private async Task LoadMoreItems()
        {
            if (isLoading) return;

            isLoading = true;
            loadMoreActivityIndicator.IsRunning = true;

            try
            {

                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri("https://api-compara-ya-git-main-jul-cesars-projects.vercel.app/productos/{currentPage}/{limit}"),
                    Method = HttpMethod.Get
                };
                request.Headers.Add("Accept", "application/json");

                HttpResponseMessage response = await _cliente.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string contentCat = await response.Content.ReadAsStringAsync();
                    var resultadoCat = JsonConvert.DeserializeObject<ObservableCollection<Product>>(contentCat);
                    // Update your collection here...
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("error", ex.Message, "ok");
            }
            finally
            {
                loadMoreActivityIndicator.IsRunning = false;
                isLoading = false;
            }
            NotifyPropertyChanged();
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
                cvPro.ItemsSource = null;
                cvPro.ItemsSource = App.ProductosCollection;

                NotifyPropertyChanged(nameof(cvPro.ItemsSource));

            }
        }



        protected async Task EliminarProducto(int id)
        {

            var productoAEliminar = App.ProductosCollection.FirstOrDefault(p => p.id == id);
            if (productoAEliminar != null)
            {
                App.ProductosCollection.Remove(productoAEliminar);
                NotifyPropertyChanged();
            }
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"https://api-compara-ya-git-main-jul-cesars-projects.vercel.app/productos/{id}");
            request.Method = HttpMethod.Delete;
            request.Headers.Add("Accept", "application/json");

            HttpResponseMessage response = await _cliente.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                
              


            }
            else
            {
                Console.WriteLine("error");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }


        private async void AnimationView_Clicked_1(object sender, EventArgs e)
        {
            var button = (AnimationView)sender;
            var item = (Product)button.BindingContext;
            await EliminarProducto(item.id);

        }
    

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue?.Trim();

        if (string.IsNullOrEmpty(searchText))
        {
            // Reset to the full list if the search text is cleared.
            cvPro.ItemsSource = App.ProductosCollection;
            isSearching = false;
        }
        else
        {
            // Apply the search filter.
            cvPro.ItemsSource = FiltrarProductos(searchText);
            isSearching = true;
        }

        // Notify the UI that the source has been updated.
        NotifyPropertyChanged(nameof(cvPro.ItemsSource));
    }


    private ObservableCollection<Product> FiltrarProductos(string searchText)
    {
        var filteredList = App.ProductosCollection
            .Where(producto => producto.nombre.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();



        return new ObservableCollection<Product>(filteredList);
    } 
}


}
