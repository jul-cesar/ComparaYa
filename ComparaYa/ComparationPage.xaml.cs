using ComparaYa.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComparaYa
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ComparationPage : ContentPage
	{

        public List<Product> EqualsProducts { get; set; }

        public List<Product> AlikeProducts { get; set; }

        string palabraProducto;
        public ComparationPage(Product prod)
        {
            InitializeComponent();

           
            palabraProducto = prod.nombre.Split(' ')[0];

            EqualsProducts = App.ProductosCollection.Where(p => p.nombre.Equals(prod.nombre)).ToList();
            AlikeProducts = App.ProductosCollection
                            .Where(p => p.nombre.Contains(palabraProducto))
                            .Except(EqualsProducts)
                            .ToList();
            eq.Text = $"Se han encontrado {EqualsProducts.Count.ToString()} productos iguales";
            BindingContext = this;


        }
        
    }
   
    }