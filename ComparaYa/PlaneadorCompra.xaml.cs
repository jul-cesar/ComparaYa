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
            NotifyPropertyChanged();

        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}