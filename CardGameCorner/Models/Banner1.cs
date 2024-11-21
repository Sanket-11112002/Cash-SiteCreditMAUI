using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CardGameCorner.Models
{
    public class Banner1
    {
        public ImageSource Image;

        public string Title { get; set; }
        public string Url { get; set; }

        //public ImageSource ImageSource
        //{
        //    get => _imageSource;
        //    set
        //    {
        //        _imageSource = value;
        //        OnPropertyChanged();
        //    }
        //}

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}

