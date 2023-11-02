using Android.Content.Res;
using Android.Views;
using ComparaYa.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Java.Util.Jar.Attributes;

namespace ComparaYa
{
    public class ProductsFilters
    {
        public async static  Task<ObservableCollection<Product>> FiltrarProductosSimilares(ObservableCollection<Product> listaProductos, Product productoSeleccionado)
        {
            ObservableCollection<Product> productosSimilares = new ObservableCollection<Product>();

            foreach (var producto in listaProductos)
            {
            
              

                double similarity = await SimilarityAPI(producto.nombre, productoSeleccionado.nombre);
                if (similarity > 0.4)
                {
                    Console.WriteLine($"Agregando producto: {producto.nombre}, Similarity: {similarity}");
                    productosSimilares.Add(producto);
                    
                }

            }

            return productosSimilares;
        }

      



        public async static Task<double> SimilarityAPI(string s1, string s2)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://api.api-ninjas.com/v1/textsimilarity");
            request.Method = HttpMethod.Post;
            var data = new {text_1 = s1, text_2 = s2};
            string json = JsonConvert.SerializeObject(data);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Headers.Add("X-Api-Key", "0YNiw7mA3Iy2VGy0qgjD+A==g4N8JzCpqJuxg99c");

            var cliente = new HttpClient();

            HttpResponseMessage response = await cliente.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                RespuestaApi score = JsonConvert.DeserializeObject<RespuestaApi>(responseContent);
                Console.WriteLine(score.similarity.ToString());
                return score.similarity;
               
            }
            else
            {
                Console.WriteLine("Error: " + response.ReasonPhrase);
                return -1;
        }
    }

      

    }
}
