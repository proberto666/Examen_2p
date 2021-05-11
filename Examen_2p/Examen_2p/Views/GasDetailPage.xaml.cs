using Examen_2p.Models;
using Examen_2p.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Examen_2p.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GasDetailPage : ContentPage
    {
        public GasDetailPage()
        {
            InitializeComponent();
            BindingContext = new GasDetailViewModel();
        }

        public GasDetailPage(GasModel GasSelected)
        {
            InitializeComponent();
            BindingContext = new GasDetailViewModel(GasSelected);
        }
    }
}