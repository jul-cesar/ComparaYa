using ComparaYa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComparaYa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsView : ContentPage
    {
        public ProductsView()
        {
            InitializeComponent();
      




            App.CategoriasCollection.Add(new Categoria
            {
                nombre = "Bebidas"
            });
            App.CategoriasCollection.Add(new Categoria
            {
                nombre = "Lacteos"
            });

            App.CategoriasCollection.Add(new Categoria
            {
                nombre = "Dulces"
            });
            App.CategoriasCollection.Add(new Categoria
            {
                nombre = "Dulces"
            });
            App.CategoriasCollection.Add(new Categoria
            {
                nombre = "Dulces"

            });
            App.CategoriasCollection.Add(new Categoria
            {
                nombre = "Dulces"
            });

            App.ProductosCollection.Add(new Product
            {
                nombre = "CAFÉ INSTANTÁNEO VIEJO MOLINO 85 G 1",
                imagen_url = "https://stockimages.tiendasd1.com/stockimages.tiendasd1.com/kobastockimages/IMAGENES/12000200.png",
                precio_d1 = "7.350",
                precio_olim = "5.000",
                precio_exito = "0",
                categoria_id = 1
            });
               App.ProductosCollection.Add(new Product
            {
                nombre = "CAFÉ INSTANTÁNEO VIEJO MOLINO 85 G 2",
                imagen_url = "https://stockimages.tiendasd1.com/stockimages.tiendasd1.com/kobastockimages/IMAGENES/12000200.png",
                precio_d1 = "7.350",
                precio_olim = "0",
                precio_exito = "4.000",
                categoria_id = 1
            });
            App.ProductosCollection.Add(new Product
            {
                nombre = "CAFÉ INSTANTÁNEO VIEJO MOLINO 85 G 3",
                imagen_url = "https://stockimages.tiendasd1.com/stockimages.tiendasd1.com/kobastockimages/IMAGENES/12000200.png",
                precio_d1 = "7.350",
                precio_olim = "0",
                precio_exito = "0",
                categoria_id = 1
            });
            App.ProductosCollection.Add(new Product
            {
                nombre = "CAFÉ INSTANTÁNEO VIEJO MOLINO 85 G 4",
                imagen_url = "https://stockimages.tiendasd1.com/stockimages.tiendasd1.com/kobastockimages/IMAGENES/12000200.png",
                precio_d1 = "7.350",
                precio_olim = "0",
                precio_exito = "0",
                categoria_id = 1
            });
            App.ProductosCollection.Add(new Product
            {
                nombre = "CAFÉ INSTANTÁNEO VIEJO MOLINO 85 G 5 ",
                imagen_url = "https://stockimages.tiendasd1.com/stockimages.tiendasd1.com/kobastockimages/IMAGENES/12000200.png",
                precio_d1 = "7.350",
                precio_olim = "0",
                precio_exito = "0",
                categoria_id = 1
            });
            App.ProductosCollection.Add(new Product
            {
                nombre = "CAFÉ INSTANTÁNEO VIEJO MOLINO 85 G 6",
                imagen_url = "https://stockimages.tiendasd1.com/stockimages.tiendasd1.com/kobastockimages/IMAGENES/12000200.png",
                precio_d1 = "7.350",
                precio_olim = "0",
                precio_exito = "0",
                categoria_id = 1
            });
            App.ProductosCollection.Add(new Product
            {
                nombre = "CAFÉ INSTANTÁNEO VIEJO MOLINO 85 G 7"
,                imagen_url = "https://stockimages.tiendasd1.com/stockimages.tiendasd1.com/kobastockimages/IMAGENES/12000200.png",
                precio_d1 = "7.350",
                precio_olim = "0",
                precio_exito = "3.200",
                categoria_id = 1
            });
            App.ProductosCollection.Add(new Product
            {
                nombre = "CAFÉ INSTANTÁNEO VIEJO MOLINO 85 G 8",
                imagen_url = "https://stockimages.tiendasd1.com/stockimages.tiendasd1.com/kobastockimages/IMAGENES/12000200.png",
                precio_d1 = "7.350",
                precio_olim = "2.400",
                precio_exito = "0",
                categoria_id = 1
            });
         

        }

        protected virtual void OnAppearing()
        {
            
        }
    }
}