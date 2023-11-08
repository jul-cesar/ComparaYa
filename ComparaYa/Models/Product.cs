using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ComparaYa.Models
{
   public class Product : INotifyPropertyChanged
    {
     public int id {  get; set; }
        public string nombre { get; set; }
        public string imagen_url { get; set; }
        public string precio_d1 { get; set;}
        public string precio_olim { get; set; }
        public string precio_exito { get; set; }
        public int categoria_id { get; set; }
        public bool isFavorite;
        public bool IsFavorite
        {
            get => isFavorite;
            set
            {
                if (isFavorite != value)
                {
                    isFavorite = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private string _favoriteIcon = "nofav.png";
        public string FavoriteIcon
        {
            get => _favoriteIcon;
            set
            {
                if (_favoriteIcon != value)
                {
                    _favoriteIcon = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private string _cantidad = "1";
        public string cantidad
        {
            get { return _cantidad; }
            set
            {
                _cantidad = value;
                NotifyPropertyChanged();
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }

}
