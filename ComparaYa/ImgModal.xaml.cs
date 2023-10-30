using ComparaYa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComparaYa
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ImgModal : Popup
	{
		public ImgModal (Product img)
		{
			InitializeComponent ();
			foto.Source = img.imagen_url;
		
			
		}
	}
}