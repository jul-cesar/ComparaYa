using System;
using System.Collections.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComparaYa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Modal : Popup
    {
        public ObservableCollection<string> Items { get; set; }
        public ObservableCollection<string> Distris { get; set; }

        public Modal()
        {
            InitializeComponent();

            Items = new ObservableCollection<string>
            {
                "5000", "5000", "5000", "5000", "5000", "5000", "5000"
            };

            Distris = new ObservableCollection<string>
            {
                "D1", "Olimpica", "Exito"
            };
            BindingContext = this;
        }
    }
}
