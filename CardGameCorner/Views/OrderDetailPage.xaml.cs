using CardGameCorner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CardGameCorner.Views
{
    public partial class OrderDetailPage : ContentPage
    {
        private readonly OrderDetailViewModel _viewModel;

        public OrderDetailPage(OrderDetailViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }
    }
}