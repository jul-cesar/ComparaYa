using ComparaYa.Models;
using Lottie.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class PlaneadorCompra : ContentPage
    {
      

        public PlaneadorCompra()
        {
            InitializeComponent();
            BindingContext = this;
            if (App.Carrito.Count > 0)
            {
                carText.Text = "Mi carrito";
               
            }


        }

        private async void delete_Clicked(object sender, EventArgs e)
        {
            var botonxd = (AnimationView)sender;
            var itemxd = (Product)botonxd.BindingContext;
            App.Carrito.Remove(itemxd);
            App.Carrito.Where(p => p != itemxd);
            await this.DisplayToastAsync("Producto eliminado del carrito");
          
          
          
        }

        public async Task imgModal(Product img)
        {

            await Navigation.ShowPopupAsync(new ImgModal(img.imagen_url));
           
        }


        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            var button = (Image)sender;
            var item = (Product)button.BindingContext;
            imgModal(item);
        }
    
    public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var item = (Product)button.BindingContext;
            if (int.TryParse(item.cantidad, out int cantidadNumerica))
            {
                cantidadNumerica--;
                item.cantidad = cantidadNumerica.ToString();
              
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)     
        {
            var button = (Button)sender;
            var item = (Product)button.BindingContext;
            if (int.TryParse(item.cantidad, out int cantidadNumerica))
            {
                cantidadNumerica++;
                item.cantidad = cantidadNumerica.ToString();

            }
        }

    }
}