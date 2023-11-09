using ComparaYa.Models;
using Java.Lang;
using Lottie.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComparaYa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaneadorCompra : ContentPage, INotifyPropertyChanged
    {


        public PlaneadorCompra()
        {
            InitializeComponent();
            BindingContext = this;
            Console.WriteLine(CultureInfo.CurrentCulture);


        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
           await showTotal();
            totalCompra();
            NotifyPropertyChanged();
            
        }

        private async  Task showTotal()
        {
            if (App.Carrito.Count > 0)
            {
                totales.IsVisible = true;
                carText.Text = "Planea tu compra";
                NotifyPropertyChanged();
            }
            else
            {
                totales.IsVisible = false;
                carText.Text = "";
                NotifyPropertyChanged();
            }
        }

        private void totalCompra()
        {
            float totalAmountExito = 0;
            float totalAmountD1 = 0;
            float totalAmountOlimpica = 0;

            if (App.Carrito != null && App.Carrito.Count > 0)
            {
                foreach (Product item in App.Carrito)
                {
                    
                    string precioExito = item.precio_exito.Trim();
                    string precioD1 = item.precio_d1.Trim();
                    string precioOlimpica= item.precio_olim.Trim();

                    if (float.TryParse(precioExito, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out float precioE))
                    {
    
                        int cantidadE = int.Parse(item.cantidad);
                        totalAmountExito += precioE * cantidadE;
                    }if(float.TryParse(precioD1, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out float precioD))
                    {
                        int cantidadD = int.Parse(item.cantidad);
                        totalAmountD1 += precioD * cantidadD;
                    }
                    if (float.TryParse(precioOlimpica, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out float precioO))
                    {
                        int cantidadO = int.Parse(item.cantidad);
                        totalAmountOlimpica += precioO * cantidadO;
                    }
                    else
                    {
                        
                        Console.WriteLine("Failed to parse precio_exito: " + precioExito, precioD1, precioOlimpica);
                    }
                }

            }
            totalExito.Text = totalAmountExito.ToString("F3");
            totald1.Text = totalAmountD1.ToString("F3"); 
            totalOlimpica.Text = totalAmountOlimpica.ToString("F3"); 
            float suma = totalAmountExito + totalAmountD1 + totalAmountOlimpica;
            neto.Text = suma.ToString("F3");

            NotifyPropertyChanged();
          
        }




        private async void delete_Clicked(object sender, EventArgs e)
        {
            var botonxd = (AnimationView)sender;
            var itemxd = (Product)botonxd.BindingContext;
            App.Carrito.Remove(itemxd);
            App.Carrito.Where(p => p != itemxd);
            NotifyPropertyChanged();
            showTotal();
            totalCompra();

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
    
    public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        private  async void Button_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var item = (Product)button.BindingContext;
            if (int.TryParse(item.cantidad, out int cantidadNumerica))
            {
                cantidadNumerica--;
                if(cantidadNumerica  > 0) {
                    item.cantidad = cantidadNumerica.ToString();
                }
                    totalCompra();
               await showTotal();
                NotifyPropertyChanged();

              

            }
        }

        private async  void Button_Clicked_1(object sender, EventArgs e)     
        {
            var button = (Button)sender;
            var item = (Product)button.BindingContext;
            if (int.TryParse(item.cantidad, out int cantidadNumerica))
            {
                cantidadNumerica++;
                item.cantidad = cantidadNumerica.ToString();

            }
            totalCompra();
          await  showTotal();
            NotifyPropertyChanged();

        }

    }
}