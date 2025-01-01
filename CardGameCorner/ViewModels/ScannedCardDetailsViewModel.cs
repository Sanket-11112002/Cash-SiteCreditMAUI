using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGameCorner.Models;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CardGameCorner.ViewModels
{
    public partial class ScannedCardDetailsViewModel : ObservableObject
    {
        [ObservableProperty]
        public ScannedCardDetails responseContent;

        [ObservableProperty]
        private ImageSource searchResultImage;

        public ScannedCardDetailsViewModel()
        {
            // Initialize default values


        }
    }
}
