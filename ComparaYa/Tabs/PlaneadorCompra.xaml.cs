﻿using ComparaYa.Models;
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

                    decimal precioExito = item.precio_exito;
                    decimal precioD1 = item.precio_d1;
                    decimal precioOlimpica = item.precio_olim;

                    // No es necesario usar TryParse aquí ya que los valores ya son decimales
                    float precioE = (float)precioExito;
                    int cantidadE = int.Parse( item.cantidad);
                    totalAmountExito += precioE * cantidadE;

                    float precioD = (float)precioD1;
                    int cantidadD = int.Parse(item.cantidad);
                    totalAmountD1 += precioD * cantidadD;

                    float precioO = (float)precioOlimpica;
                    int cantidadO = int.Parse(item.cantidad);
                    totalAmountOlimpica += precioO * cantidadO;

                   
                }

            }
            totalExito.Text = totalAmountExito.ToString();
            totald1.Text = totalAmountD1.ToString(); 
            totalOlimpica.Text = totalAmountOlimpica.ToString(); 
            float suma = totalAmountExito + totalAmountD1 + totalAmountOlimpica;
            neto.Text = suma.ToString();

            NotifyPropertyChanged();
          
        }




        private void DeleteFromCart(object sender, EventArgs e)
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


        private async void SendImgToModal(object sender, EventArgs e)
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

        private  async void DisminuirCantidad(object sender, EventArgs e)
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

        private async  void AumentarCantidad(object sender, EventArgs e)     
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

        private async void GoToLastPage(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.Count > 1)
            {
                backcart.IsVisible = true; 
                await Navigation.PopAsync();
            }
            else
            {
                backcart.IsVisible = false;
            }
        }
    }
}