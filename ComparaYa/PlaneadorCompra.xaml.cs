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
        
            NotifyPropertyChanged();
            
        }

        private async  Task showTotal()
        {
            if (App.Carrito.Count > 0)
            {
                totales.IsVisible = true;
                carText.Text = "Mi carrito";
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
            float totalAmount = 0;

            // Verifica si App.Carrito tiene elementos antes de entrar al bucle
            if (App.Carrito != null && App.Carrito.Count > 0)
            {
                foreach (Product item in App.Carrito)
                {
                    
                 

                    // Convierte el string a un float usando una CultureInfo que espera una coma como separador decimal
                    float precio = float.Parse(item.precio_exito);
                    int cantidad = int.Parse(item.cantidad);
                    totalAmount += precio * cantidad;

                }
            }
            else
            {
                Console.WriteLine("El carrito está vacío.");
            }

            NotifyPropertyChanged();
            totalExito.Text = totalAmount.ToString();
        }




        private void delete_Clicked(object sender, EventArgs e)
        {
            var botonxd = (AnimationView)sender;
            var itemxd = (Product)botonxd.BindingContext;
            App.Carrito.Remove(itemxd);
            App.Carrito.Where(p => p != itemxd);
            NotifyPropertyChanged();
            showTotal();
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

        private  void Button_Clicked(object sender, EventArgs e)
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
                showTotal();
                NotifyPropertyChanged();

              

            }
        }

        private  void Button_Clicked_1(object sender, EventArgs e)     
        {
            var button = (Button)sender;
            var item = (Product)button.BindingContext;
            if (int.TryParse(item.cantidad, out int cantidadNumerica))
            {
                cantidadNumerica++;
                item.cantidad = cantidadNumerica.ToString();

            }
            totalCompra();
            showTotal();
            NotifyPropertyChanged();

        }

    }
}