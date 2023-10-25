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

namespace ComparaYa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsView : ContentPage
    {
        public decimal ipEmulador;
        public decimal ipPcCelu;
        public ProductsView()
        {
            InitializeComponent();
      




        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await FetchProductsFromServer();
            await FetchCategoriasFromServer();
        }

        protected async Task FetchCategoriasFromServer()
        {

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://10.0.2.2:4000/categorias/");
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accept", "application/json");
            var cliente = new HttpClient();
            HttpResponseMessage response = await cliente.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)

            {
                string contentCat = await response.Content.ReadAsStringAsync();

                var resultadoCat = JsonConvert.DeserializeObject<ObservableCollection<Categoria>>(contentCat);
               

                foreach (var categoria in resultadoCat)
                {
                    App.CategoriasCollection.Add(categoria);
                }

            }
        }

        protected async Task FetchProductsFromServer()
        {

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://10.0.2.2:4000/productos/");
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accept", "application/json");
            var cliente = new HttpClient();
            HttpResponseMessage response = await cliente.SendAsync(request);
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


    }
}