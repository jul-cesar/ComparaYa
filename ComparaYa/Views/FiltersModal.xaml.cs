using Android.Views;
using ComparaYa.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Android.Content.ClipData;

namespace ComparaYa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Modal : Popup, INotifyPropertyChanged
    {
        public ObservableCollection<string> Items { get; set; }
        public ObservableCollection<string> Distris { get; set; }
        decimal? priceFrom;
        string distri;
        public Modal()
        {
            InitializeComponent();

           
          

            Items = new ObservableCollection<string>
            {
                "50000", "30000", "20000", "10000", "5000"
            };

            Distris = new ObservableCollection<string>
            {
                "D1", "OLIMPICA", "EXITO"
            };
            BindingContext = this;
        }

      

        private void Button_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var si = button.Text.Trim();
            var precio = decimal.Parse(si, CultureInfo.InvariantCulture);
            priceFrom = precio;
            Console.WriteLine(priceFrom.ToString());


        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {



            var filterData = new FiltrosData
            {
                PriceFrom = priceFrom, 
                Distri = distri         
            };

           

            MessagingCenter.Send(this, "FilterProducts", filterData);
            Dismiss(null);

        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var si = button.Text;
            distri = si;
        }
    }
}
