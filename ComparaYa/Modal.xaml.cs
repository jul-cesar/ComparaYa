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
        float priceFrom;
        string distri;
        public Modal()
        {
            InitializeComponent();

           
          

            Items = new ObservableCollection<string>
            {
                "5000", "3000", "4000", "2000", "1000"
            };

            Distris = new ObservableCollection<string>
            {
                "D1", "Olimpica", "Exito"
            };
            BindingContext = this;
        }

      

        private void Button_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var si = button.Text.Trim();
            var precio = float.Parse(si, CultureInfo.InvariantCulture);
            priceFrom = precio;
            Console.WriteLine(priceFrom.ToString());


        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {


            
                MessagingCenter.Send(this, "FilterProducts", priceFrom);
          
      
           
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
